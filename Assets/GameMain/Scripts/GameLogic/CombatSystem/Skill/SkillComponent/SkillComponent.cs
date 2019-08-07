using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

namespace Galaxy
{
	public partial class SkillComponent : ComponentBase
	{
		//实例子弹存储 todo 子弹也是场景的一个实例
		public Dictionary<int, List<GSkillProjectile>> m_ProjectileDict;

		//防死锁/效率优化 两级事件缓存
		public List<GTriggerNotify> m_TriggerNotifyCurList;
		public List<GTriggerNotify> m_TriggerNotifyWaitList;
		
		//所有技能
		public Dictionary<int, GSkillSpellLogic> m_SkillLogicDict;
		//主动技能
		protected GSkillSpellLogic m_pSpellLogic;
		//被动技能
		protected List<GSkillSpellLogic> m_PassiveList;
		protected List<GSkillSpellLogic> m_PassiveTempList;
		//触发技能
		protected List<GSkillSpellLogic> m_TriggerSkillList;
		
		protected override void InitComponent()
		{
			m_ProjectileDict = new Dictionary<int, List<GSkillProjectile>>();

			m_TriggerNotifyCurList = new List<GTriggerNotify>();
			m_TriggerNotifyWaitList = new List<GTriggerNotify>();

			m_SkillLogicDict = new Dictionary<int, GSkillSpellLogic>();
			m_PassiveList = new List<GSkillSpellLogic>();
			m_TriggerSkillList = new List<GSkillSpellLogic>();
		}

		public override void OnPreDestroy()
		{
			FinishSkill();
			
			m_ProjectileDict.Clear();

			m_TriggerNotifyCurList.Clear();
			m_TriggerNotifyWaitList.Clear();

			if(m_SkillLogicDict != null)
			{
				foreach(var item in m_SkillLogicDict)
				{
					ReferencePool.Release(item.Value);
				}
				m_SkillLogicDict.Clear();
			}
			if(m_PassiveList != null)
			{
				foreach(var item in m_PassiveList)
				{
					ReferencePool.Release(item);
				}
				m_PassiveList.Clear();
			}
			if(m_TriggerSkillList != null)
			{
				foreach(var item in m_TriggerSkillList)
				{
					ReferencePool.Release(item);
				}
				m_TriggerSkillList.Clear();
			}
		}

		public void Update()
		{
			//    TickProjectile(nFrameTime);
			//    TickPassiveSkill(nFrameTime);
			//    TickSkill(nFrameTime);
			//    ProcessTriggerNotify();
		}

		////清除被动技能
		//void GNodeSkillComponent::ClearPassiveSkill()
		//{
		//    for (int32 i = 0; i < NotifyType_Count; ++i)
		//    {
		//        m_vTriggerSkillList[i].clear();
		//    }

		//    m_vPassiveList.clear();
		//    m_vPassiveTempList.clear();
		//}

		//void GNodeSkillComponent::ClearSkillProjectile()
		//{
		//    if (m_pOwner && m_pOwner->GetScene())
		//    {
		//        ((GNodeScene*)m_pOwner->GetScene())->ChangeProjsCount(-m_nProjectileCount);
		//    }

		//    m_vProjectileMap.Begin();
		//    while (!m_vProjectileMap.IsEnd())
		//    {
		//        ProjectileList & vList = m_vProjectileMap.Get();
		//        for (ProjectileList::iterator iter = vList.begin(); iter != vList.end(); ++iter)
		//        {
		//            FACTORY_DELOBJ(*iter);
		//        }
		//        m_vProjectileMap.Remove();
		//    }
		//    m_nProjectileCount = 0;
		//}

		//增加技能
		public bool AddSkill(int nSkillID/*, int nSlots = 0*/)
		{
			if(!Owner)
				return false;
			DRSkillData pSkillData = GameEntry.DataTable.GetDataTable<DRSkillData>().GetDataRow(nSkillID);
			if(pSkillData == null)
				return false;

			if(HasSkill(nSkillID))
				return false;

			GTargetInfo sTarInfo = new GTargetInfo();
			sTarInfo.m_nTargetID = Owner.Id;
			sTarInfo.m_vSrcPos = Owner.GetPos();
			sTarInfo.m_vAimDir = Owner.GetDir();
			sTarInfo.m_vTarPos = Owner.GetPos();

			GSkillInitParam param = new GSkillInitParam(Owner, sTarInfo, pSkillData);
			GSkillSpellLogic pSpellLogic = CreateSkill(param);
			if(pSpellLogic == null)
				return false;

			m_SkillLogicDict.Add(nSkillID, pSpellLogic);
			if(pSkillData.IsPassiveSkill() && !pSkillData.IsBuffSkill())
			{
				AddPassiveSkill(pSpellLogic);
				TryCalculateAttribute(pSpellLogic);
			}

			AddSubSkill(pSpellLogic);
			return true;
		}

		//创建技能
		private GSkillSpellLogic CreateSkill(GSkillInitParam initParam)
		{
			if(!Owner)
				return null;

			if(initParam == null 
				|| initParam.m_pOwner == null 
				|| initParam.m_pSkillData == null)
				return null;

			int nLogicID = initParam.m_pSkillData.MSV_SpellLogic;
			GSkillSpellLogic pSpellLogic = GSkillLogicManager.Instance.CreateSpellLogic(nLogicID);
			if(pSpellLogic == null)
				return null;

			if(!pSpellLogic.Init(initParam))
			{
				ReferencePool.Release(pSpellLogic);
				return null;
			}
			return pSpellLogic;
		}

		//增加子技能
		private void AddSubSkill(GSkillSpellLogic pSpellLogic)
		{
			if(pSpellLogic == null)
				return;

			List<int> pSkillList = GSkillDataManager.Instance.GetSubSkillList(pSpellLogic.GetSkillID());
			if(pSkillList == null)
				return;

			//todo 技能修正
			//int nSlots = pSpellLogic.GetSkillSlots();
			foreach(var item in pSkillList)
			{
				AddSkill(item/*, nSlots*/);
			}
		}

		public void RemoveSkill(int nSkillID)
		{
			if(!HasSkill(nSkillID))
				return;

			GSkillSpellLogic pSpellLogic = GetSkill(nSkillID);
			if(pSpellLogic == null)
				return;

			//停止施法
			if(pSpellLogic == m_pSpellLogic)
			{
				FinishSkill();
			}

			RemoveSubSkill(pSpellLogic);

			//刷新属性集
			TryCalculateAttribute(pSpellLogic);
			//移除被动技能
			RemovePassiveSkill(pSpellLogic);

			ReferencePool.Release(pSpellLogic);
			m_SkillLogicDict.Remove(nSkillID);
		}

		//移除子技能
		private void RemoveSubSkill(GSkillSpellLogic pSpellLogic)
		{
			if(pSpellLogic == null)
				return;

			List<int> pSkillList = GSkillDataManager.Instance.GetSubSkillList(pSpellLogic.GetSkillID());
			if(pSkillList == null)
				return;

			foreach(var item in pSkillList)
			{
				RemoveSkill(item);
			}
		}

		//是否拥有技能
		public bool HasSkill(int nSkillID)
		{
			return (GetSkill(nSkillID) != null);
		}
		
		//获取技能
		public GSkillSpellLogic GetSkill(int nSkillID)
		{
			if(m_SkillLogicDict.ContainsKey(nSkillID))
			{
				return m_SkillLogicDict[nSkillID];
			}
			return null;
		}

		private void AddPassiveSkill(GSkillSpellLogic pSpellLogic)
		{
			if(pSpellLogic == null)
				return;
			m_PassiveTempList.Add(pSpellLogic);
		}

		//移除被动技能
		private void RemovePassiveSkill(GSkillSpellLogic pSpellLogic)
		{
			m_PassiveList.Remove(pSpellLogic);
			m_PassiveTempList.Remove(pSpellLogic);
			RemoveTriggerSkill(pSpellLogic);
		}

		//增加技能触发
		private void AddTriggerSkill(GSkillSpellLogic pTriggerLogic)
		{
			if(pTriggerLogic == null
				|| pTriggerLogic.m_pSkillData == null
				|| !pTriggerLogic.m_pSkillData.IsTriggerSkill())
				return;

			int nType = pTriggerLogic.m_pSkillData.MSV_TriggerType;
			if(nType >= 0 && nType < (int)eTriggerNotifyType.NotifyType_Count)
			{
				m_TriggerSkillList[nType].Add(pTriggerLogic);
			}
		}
		//移除技能触发
		private void RemoveTriggerSkill(GSkillSpellLogic pTriggerLogic)
		{
			if(pTriggerLogic == null || pTriggerLogic.m_pSkillData == null)
				return;

			int nType = pTriggerLogic.m_pSkillData.MSV_TriggerType;
			if(nType >= 0 && nType < (int)eTriggerNotifyType.NotifyType_Count)
			{
				m_TriggerSkillList[nType].Remove(pTriggerLogic);
			}
		}

		////获取技能，如果不是实例技能，返回模版技能
		//GSkillData* GNodeSkillComponent::GetSkillData(int32 nSkillID)
		//{
		//    GSkillSpellLogic* pSpellLogic = GetSkill(nSkillID);
		//    if (pSpellLogic)
		//        return pSpellLogic->m_pSkillData;

		//    GSkillData* pSkillData = GSkillDataManager::Instance().GetSkillData(nSkillID);
		//    if (pSkillData && pSkillData->IsTemplateSkill())
		//        return pSkillData;

		//    return NULL;
		//}
	}

}

//GNodeSkillComponent::~GNodeSkillComponent()
//{
//    FinishSkill();
//    ClearPassiveSkill();
//    ClearSkillProjectile();

//    m_SkillLogicMap.Begin();
//    while (!m_SkillLogicMap.IsEnd())
//    {
//        FACTORY_DELOBJ(m_SkillLogicMap.Get());
//        m_SkillLogicMap.Remove();
//    }
//}

//void GNodeSkillComponent::BeforeExitScene()
//{
//    ClearSkillProjectile();
//    if (m_pOwner)
//    {
//        GPacketClearProjectile pkt;
//        m_pOwner->BroadcastPacket(&pkt);
//    }
//}

//void GNodeSkillComponent::TickSkill(int32 nFrameTime)
//{
//    if (!m_pSpellLogic)
//    {
//        return;
//    }

//    if (!m_pSpellLogic->IsFinished())
//    {
//        m_pSpellLogic->Tick(nFrameTime);
//    }

//    if (m_pSpellLogic->IsFinished())
//    {
//        GalaxyLog::debug("Avatar[%d] Skill[%d] End [%d]", m_pOwnerNode->GetAvatarID(), m_pSpellLogic->GetSkillID(), GTimeManager::Instance().CurSecond());
//        FinishSkill();
//    }
//}

//void GNodeSkillComponent::TickPassiveSkill(int32 nFrameTime)
//{
//    {
//        GSkillLogicList::iterator iter = m_vPassiveTempList.begin();
//        GSkillLogicList::iterator iter_end = m_vPassiveTempList.end();
//        for (; iter != iter_end; ++iter)
//        {
//            GSkillSpellLogic* pSpellLogic = *iter;
//            if (pSpellLogic)
//            {
//                m_vPassiveList.push_back(pSpellLogic);
//                AddTriggerSkill(pSpellLogic);
//            }
//        }
//        m_vPassiveTempList.clear();
//    }
//    {
//        GSkillLogicList::iterator iter = m_vPassiveList.begin();
//        GSkillLogicList::iterator iter_end = m_vPassiveList.end();
//        for (; iter != iter_end; ++iter)
//        {
//            GSkillSpellLogic* pSpellLogic = *iter;
//            if (pSpellLogic)
//            {
//                pSpellLogic->Tick(nFrameTime);
//            }
//        }
//    }
//}

//void GNodeSkillComponent::TickProjectile(int32 nFrameTime)
//{
//    m_vProjectileMap.Begin();
//    while (!m_vProjectileMap.IsEnd())
//    {
//        ProjectileList & vList = m_vProjectileMap.Get();
//        for (ProjectileList::iterator iter = vList.begin(); iter != vList.end();)
//        {
//            GSkillProjectile* pProjectile = *iter;
//            if (pProjectile)
//            {
//                pProjectile->Tick(nFrameTime, m_pOwnerNode);
//                if (pProjectile->IsDestroy())
//                {
//                    if (m_pOwner)
//                    {
//                        GPacketDestroyProjectile pkt;
//                        pkt.ProjectileID = pProjectile->m_nProjectileID;
//                        pkt.bTimeOut = pProjectile->m_bTimeOut;
//                        Vector3 pos = pProjectile->GetPos();
//                        pkt.x = pos.x;
//                        pkt.y = pos.y;
//                        pkt.z = pos.z;
//                        m_pOwner->BroadcastPacket(&pkt);
//                    }

//                    if (m_pOwner && m_pOwner->GetScene())
//                    {
//                        ((GNodeScene*)m_pOwner->GetScene())->ChangeProjsCount(-1);
//                    }

//                    --m_nProjectileCount;
//                    FACTORY_DELOBJ(pProjectile);
//                    iter = vList.erase(iter);
//                    continue;
//                }
//            }
//            ++iter;
//        }

//        (vList.empty()) ? m_vProjectileMap.Remove() : m_vProjectileMap.Next();
//    }
//}

////使用技能
//bool GNodeSkillComponent::SpellSkill(int32 nSkillID, GSkillTargetInfo& sTarInfo, GSpellParam* pSpellParam)
//{
//    FuncPerformance(SpellSkill);
//    if (!m_pOwnerNode)
//        return false;

//    GSkillSpellLogic* pSpellLogic = GetSkill(nSkillID);
//    if (!pSpellLogic)
//    {
//        GSkillData* pSkillData = GSkillDataManager::Instance().GetSkillData(nSkillID);
//        if (pSkillData && pSkillData->IsTemplateSkill())
//        {
//            if (AddSkill(nSkillID))
//            {
//                pSpellLogic = GetSkill(nSkillID);
//            }
//        }
//    }
//    if (!pSpellLogic)
//    {
//        return false;
//    }

//    GSkillData* pSkillData = pSpellLogic->m_pSkillData;
//    if (!pSkillData)
//        return false;

//    //if (pSpellParam && pSpellParam->bClient && pSkillData->IsServerSkill())
//    //	return false;


//    if (sTarInfo.m_nShapeID >= 0)
//    {
//        GNodeAvatar* pTarget = m_pOwnerNode->GetSceneAvatar(sTarInfo.m_nTargetID);
//        if (pTarget)
//        {
//            sTarInfo.m_vTarPos = pTarget->GetCollisionShapePos(sTarInfo.m_nShapeID);
//        }
//    }

//    if (sTarInfo.m_vMoveTarPos.IsZero())
//    {
//        sTarInfo.m_vMoveTarPos = sTarInfo.m_vSrcPos;
//    }

//    if (!CanSpellSkill(pSkillData, sTarInfo, pSpellParam))
//        return false;

//    if (m_pSpellLogic)
//        FinishSkill();

//    //初始化施法逻辑
//    m_pSpellLogic = pSpellLogic;


//    m_pSpellLogic->Reset();
//    if (sTarInfo.m_vAimDir.IsZero())
//        sTarInfo.m_vAimDir = m_pOwnerNode->GetDir();

//    m_pSpellLogic->m_TargetInfo = sTarInfo;

//    if (pSpellParam)
//        m_pSpellLogic->m_vTargetList = pSpellParam->vTargetList;

//    m_pSpellLogic->ResetRoleAValue();
//    m_pSpellLogic->SetFSMState();

//    if (!pSkillData->IsEffectStateCost())
//    {
//        DoCost(pSkillData);
//        StartCD(pSkillData, true);
//        m_pSpellLogic->SetCosted();
//    }

//    m_pSpellLogic->ReInit();
//    sTarInfo = m_pSpellLogic->m_TargetInfo;

//    if (pSkillData->IsCombineSpellNotify())
//    {
//        PushTriggerNotifyAValue(pSkillData->m_nDataID, sTarInfo.m_nTargetID, NotifyType_CombineSpell, pSkillData->GetIntValue(MSV_EffectType), &m_pSpellLogic->m_AValue);
//    }

//    GPacketSkillSpell pkt;
//    pkt.SkillID = nSkillID;
//    pkt.SkillSlots = pSkillData->m_nSlots;
//    pkt.SkillTarget = sTarInfo.m_nTargetID;
//    pkt.SetPos(sTarInfo.m_vSrcPos.x, sTarInfo.m_vSrcPos.y, sTarInfo.m_vSrcPos.z);
//    pkt.SetAimDir(sTarInfo.m_vAimDir.x, sTarInfo.m_vAimDir.y, sTarInfo.m_vAimDir.z);
//    pkt.SetTarPos(sTarInfo.m_vTarPos.x, sTarInfo.m_vTarPos.y, sTarInfo.m_vTarPos.z);
//    pkt.SetMovetTarPos(sTarInfo.m_vMoveTarPos.x, sTarInfo.m_vMoveTarPos.y, sTarInfo.m_vMoveTarPos.z);
//    if (pSpellParam)
//    {
//        pSpellParam->GetTargetList(pkt.SkillTarget_0, pkt.SkillTarget_1, pkt.SkillTarget_2);
//    }
//    else
//    {
//        pkt.SkillTarget_0 = 0;
//        pkt.SkillTarget_1 = 0;
//        pkt.SkillTarget_2 = 0;
//    }

//    m_pOwnerNode->BroadcastPacket(&pkt);

//    GalaxyLog::debug("Avatar[%d] Skill[%d] Spell [%d]", m_pOwnerNode->GetAvatarID(), nSkillID, GTimeManager::Instance().CurSecond());

//    //产生技能事件
//    if (pSkillData->IsTriggerSkillNotify())
//    {
//        PushTriggerNotify(nSkillID, sTarInfo.m_nTargetID, NotifyType_Skill, TriggerNotify_SkillStart, 0, &sTarInfo.m_vSrcPos, &sTarInfo.m_vTarPos, &sTarInfo.m_vAimDir);
//    }

//    if (m_pOwnerNode->IsPlayer())
//    {
//        GNodeScene* pScene = (GNodeScene*)m_pOwnerNode->GetScene();
//        if (pScene)
//        {
//            EventSceneNotify event;
//				event.NotifyType = SceneNotify_SpellSkill;
//				event.ExtData0 = pSkillData->GetIntValue(MSV_EndureLevel);
//				event.ExtData1 = sTarInfo.m_nTargetID;
//				event.SrcAvatar = m_pOwnerNode->GetAvatarID();
//            pScene->EventNotify(&event);
//        }
//    }

//    //执行瞬发效果
//    m_pSpellLogic->Tick(0);
//    return true;
//}

////检查使用技能
//bool GNodeSkillComponent::CanSpellSkill(int32 nSkillID, GSkillTargetInfo& sTarInfo, GSpellParam* pSpellParam)
//{
//    return CanSpellSkill(GetSkillData(nSkillID), sTarInfo, pSpellParam);
//}

//bool GNodeSkillComponent::CanSpellSkill(GSkillData* pSkillData, GSkillTargetInfo& sTarInfo, GSpellParam* pSpellParam)
//{
//    GNodeAvatar* pCaster = m_pOwnerNode;
//    if (!pCaster || !pSkillData)
//        return false;

//    if (!pSkillData->IsActiveSkill())
//        return false;

//    //检查当前施放技能
//    if (m_pSpellLogic && m_pSpellLogic->IsLock())
//    {
//        return false;
//    }

//    //检查消耗
//    if (!CheckCost(pSkillData))
//    {
//        return false;
//    }
//    //检查CD
//    if (!CheckCD(pSkillData))
//    {
//        return false;
//    }

//    //检查施法者位置
//    if (pCaster->GetPos().GetSquaredDistance2D(sTarInfo.m_vSrcPos) > 64.0f)
//        return false;

//    //自身检查
//    int32 nSrvCheck = pSkillData->GetIntValue(MSV_SrcCheck);
//    if (nSrvCheck > 0)
//    {
//        if (!GSKillConditionCheckManager::Instance().Check(nSrvCheck, pCaster))
//            return false;
//    }

//    //目标检查
//    int32 nSize = MIN(pSkillData->TargetListSize(), MAX_SPELL_TARGET);
//    if (nSize <= 0)
//    {
//        if (!GSkillLogicManager::Instance().CheckTarget(pSkillData, pCaster, sTarInfo))
//            return false;
//    }
//    else
//    {
//        if (!pSpellParam)
//            return false;

//        if (!GSkillLogicManager::Instance().CheckTargetList(pSkillData, pCaster, pSpellParam->vTargetList))
//            return false;
//    }
//    return true;
//}

//void GNodeSkillComponent::CastSkill(int32 nSkillID, int32 chargeLevel/* = -1*/)
//{
//    if (!m_pSpellLogic || !m_pSpellLogic->m_pSkillData)
//        return;

//    if (m_pSpellLogic->m_pSkillData->m_nDataID != nSkillID)
//        return;

//    m_pSpellLogic->Cast(chargeLevel);
//}

////检查技能消耗
//bool GNodeSkillComponent::CheckCost(GSkillData* pSkillData)
//{
//    if (!m_pOwnerNode || !pSkillData)
//        return false;

//    {
//        int32 nCostType = pSkillData->GetIntValue(MSV_CostType1);
//        int32 nCostValue = pSkillData->GetIntValue(MSV_CostValue1);
//        if (!CheckCost(nCostType, nCostValue))
//            return false;
//    }
//    {
//        int32 nCostType = pSkillData->GetIntValue(MSV_CostType2);
//        int32 nCostValue = pSkillData->GetIntValue(MSV_CostValue2);
//        if (!CheckCost(nCostType, nCostValue))
//            return false;
//    }

//    return true;
//}

//bool GNodeSkillComponent::CheckCost(int32 nCostType, int32 nCostValue)
//{
//    if (nCostType == SkillCost_Hp)
//    {
//        return (m_pOwnerNode->GetHp() > nCostValue);
//    }
//    else if (nCostType == SkillCost_Item)
//    {
//        if (!m_pOwnerNode->GetItemComponent())
//            return false;

//        int32 nCount = m_pOwnerNode->GetItemComponent()->GetItemCount(nCostValue) >= 1;
//        return nCount >= 1;
//    }
//    else if (nCostType == SkillCost_Ep1)
//    {
//        if (nCostValue == -1)
//        {
//            // 意味着要消耗全部能量，此时能量应该不为零才能释放
//            return (m_pOwnerNode->GetEp(1) > 0);
//        }
//        else
//        {
//            return (m_pOwnerNode->GetEp(1) >= nCostValue);
//        }
//    }
//    else if (nCostType == SkillCost_Ep2)
//    {
//        if (nCostValue == -1)
//        {
//            // 意味着要消耗全部能量，此时能量应该不为零才能释放
//            return (m_pOwnerNode->GetEp(2) > 0);
//        }
//        else
//        {
//            return (m_pOwnerNode->GetEp(2) >= nCostValue);
//        }
//    }
//    return true;
//}

////扣除技能消耗
//void GNodeSkillComponent::DoCost(GSkillData* pSkillData)
//{
//    if (!m_pOwnerNode || !pSkillData)
//        return;

//    {
//        int32 nCostType = pSkillData->GetIntValue(MSV_CostType1);
//        int32 nCostValue = pSkillData->GetIntValue(MSV_CostValue1);
//        DoCost(nCostType, nCostValue, pSkillData->IsActiveSkill());
//    }
//    {
//        int32 nCostType = pSkillData->GetIntValue(MSV_CostType2);
//        int32 nCostValue = pSkillData->GetIntValue(MSV_CostValue2);
//        DoCost(nCostType, nCostValue, pSkillData->IsActiveSkill());
//    }
//}

//void GNodeSkillComponent::DoCost(int32 nCostType, int32 nCostValue, bool bActive/* = false*/)
//{
//    if (nCostValue == 0)
//        return;

//    if (nCostType == SkillCost_Hp)
//    {
//        f32 hp = MAX(1, m_pOwnerNode->GetHp() - nCostValue);
//        m_pOwnerNode->SetHp(hp, m_pOwnerNode->GetAvatarID(), TRUE);
//    }
//    else if (nCostType == SkillCost_Item)
//    {
//        if (m_pOwnerNode->GetItemComponent())
//        {
//            ItemLogInfo itemLogInfo;
//            itemLogInfo.m_strOpReason = "UseSkill";
//            bool bBind = false;
//            m_pOwnerNode->GetItemComponent()->RemoveItem(nCostValue, 1);
//        }
//    }
//    else if (nCostType == SkillCost_Ep1)
//    {
//        if (nCostValue == -1)
//        {
//            // 消耗全部
//            nCostValue = m_pOwnerNode->GetEp(1);
//            m_pOwnerNode->SetEp(0, 1, TRUE);
//        }
//        else
//        {
//            f32 ep = MAX(0, m_pOwnerNode->GetEp(1) - nCostValue);
//            m_pOwnerNode->SetEp(ep, 1, TRUE);
//        }

//        if (bActive)
//        {
//            m_vEpCostCount[1] = nCostValue;
//        }
//    }
//    else if (nCostType == SkillCost_Ep2)
//    {
//        if (nCostValue == -1)
//        {
//            // 消耗全部
//            nCostValue = m_pOwnerNode->GetEp(2);
//            m_pOwnerNode->SetEp(0, 2, TRUE);
//        }
//        else
//        {
//            f32 ep = MAX(0, m_pOwnerNode->GetEp(2) - nCostValue);
//            m_pOwnerNode->SetEp(ep, 2, TRUE);
//        }

//        if (bActive)
//        {
//            m_vEpCostCount[2] = nCostValue;
//        }
//    }
//}

////检查技能CD
//bool GNodeSkillComponent::CheckCD(GSkillData* pSkillData)
//{
//    if (!m_pOwnerNode || !pSkillData)
//        return false;

//    //检查CD
//    int32 nCDGroup = pSkillData->GetIntValue(MSV_CDGroup);
//    if (nCDGroup < 0)//-1 no cd
//    {
//        return true;
//    }
//    CDTime* pCDTime = GCDManager::Instance().GetCDTime(nCDGroup);
//    if (!pCDTime)
//        return false;

//    //CD 消耗Buff
//    if (pCDTime->m_nBuffID > 0)
//    {
//        if (m_pOwnerNode->HasBuff(pCDTime->m_nBuffID))
//        {
//            return true;
//        }
//    }

//    GCDComponent* pCDComponent = m_pOwnerNode->GetCDComponent();
//    if (!pCDComponent || pCDComponent->CheckCD(nCDGroup))
//    {
//        return false;
//    }
//    return true;
//}
////开始技能CD
//void GNodeSkillComponent::StartCD(GSkillData* pSkillData, bool bStart)
//{
//    if (!m_pOwnerNode || !pSkillData)
//        return;

//    GCDComponent* pCDComponent = m_pOwnerNode->GetCDComponent();
//    if (!pCDComponent)
//        return;

//    int32 nCDGroup = pSkillData->GetIntValue(MSV_CDGroup);
//    int32 nCDTime = pSkillData->GetIntValue(MSV_CDTime);
//    //消耗Buff
//    CDTime* pCDTime = GCDManager::Instance().GetCDTime(nCDGroup);
//    if (pCDTime && pCDTime->m_nBuffID > 0 && pCDTime->m_nBuffLayer > 0)
//    {
//        if (m_pOwnerNode->GetBuffComponent())
//        {
//            m_pOwnerNode->GetBuffComponent()->RemoveBuffByLayerCnt(pCDTime->m_nBuffID, m_pOwnerNode->GetAvatarDID(), 1);
//            pCDComponent->StartCDCommon(pCDTime->m_nCDCommon);
//            return;
//        }
//    }
//    pCDComponent->StartCD(nCDGroup, nCDTime, bStart);
//}

////创建技能子弹
//void GNodeSkillComponent::CreateSkillProjectile(GSkillData* pSkillData, GSkillTargetInfo& sTarInfo, RoleAValue& sRoleValue)
//{
//    if (!m_pOwner)
//        return;

//    if (!pSkillData)
//        return;

//    if (m_nProjectileCount >= MAX_SKILL_PROJECTILE_COUNT)
//        return;

//    int32 nType = pSkillData->GetIntValue(MSV_ProjectileLogic);
//    GSkillProjectile* pProjectile = GSkillLogicManager::Instance().CreateProjectile(nType);
//    if (!pProjectile)
//        return;

//    if (!pProjectile->Init(pSkillData, sTarInfo, sRoleValue, (GNodeAvatar*)m_pOwner))
//    {
//        FACTORY_DELOBJ(pProjectile);
//        return;
//    }

//    if (m_pOwner->GetScene())
//    {
//        ((GNodeScene*)m_pOwner->GetScene())->ChangeProjsCount(+1);
//    }

//    pProjectile->SetID(++m_nProjectileID);
//    //子弹坐标使用当前角色坐标
//    if (pSkillData->IsBulletBornTarPos())
//    {
//        pProjectile->m_TargetInfo.m_vSrcPos = pProjectile->m_TargetInfo.m_vTarPos;
//        pProjectile->m_TargetInfo.m_vSrcPos.z = m_pOwner->GetSceneHeight(pProjectile->m_TargetInfo.m_vTarPos);
//    }

//    GPacketCreateProjectile pkt;
//    pkt.ProjectileID = pProjectile->GetID();
//    pkt.SkillID = pSkillData->m_nDataID;
//    pkt.SkillSlots = pSkillData->m_nSlots;
//    pkt.TargetID = pProjectile->m_TargetInfo.m_nTargetID;
//    pkt.x = pProjectile->m_TargetInfo.m_vSrcPos.x;
//    pkt.y = pProjectile->m_TargetInfo.m_vSrcPos.y;
//    pkt.z = pProjectile->m_TargetInfo.m_vSrcPos.z;
//    pkt.dx = pProjectile->m_TargetInfo.m_vAimDir.x;
//    pkt.dy = pProjectile->m_TargetInfo.m_vAimDir.y;
//    pkt.dz = pProjectile->m_TargetInfo.m_vAimDir.z;
//    pkt.x2 = pProjectile->m_TargetInfo.m_vTarPos.x;
//    pkt.y2 = pProjectile->m_TargetInfo.m_vTarPos.y;
//    pkt.z2 = pProjectile->m_TargetInfo.m_vTarPos.z;
//    pkt.TotalTime = pProjectile->m_nLifeTime;
//    pkt.curveID = pProjectile->m_nCurveID;
//    m_pOwner->BroadcastPacket(&pkt);

//    ++m_nProjectileCount;
//    pProjectile->Tick(0, m_pOwnerNode);     //MSV_ProjectileFirstEffectTime 为0的情况
//    m_vProjectileMap[pSkillData->GetSkillID()].push_back(pProjectile);
//}

//Galaxy::ProjectileList* GNodeSkillComponent::GetSkillProjectileList(int32 nSkillID)
//{
//    if (m_vProjectileMap.Find(nSkillID))
//        return &m_vProjectileMap.Get();
//    return NULL;
//}

//GSkillProjectile* GNodeSkillComponent::GetProjectileByID(int32 nSkillID, int32 nProjectileID)
//{
//    if (!m_vProjectileMap.Find(nSkillID))
//        return NULL;

//    ProjectileList & vList = m_vProjectileMap.Get();
//    for (ProjectileList::iterator iter = vList.begin(); iter != vList.end();)
//    {
//        GSkillProjectile* pProjectile = *iter;
//        if (pProjectile && pProjectile->GetID() == nProjectileID)
//        {
//            return pProjectile;
//        }
//        ++iter;
//    }
//    return NULL;
//}

////死亡处理
//void GNodeSkillComponent::OnDead()
//{
//    FinishSkill();
//}
//bool GNodeSkillComponent::CanTriggerNotify(int32 nType)
//{
//    return (nType >= 0 && nType < NotifyType_Count && !m_vTriggerSkillList[nType].empty());
//}
////接受触发消息
//void GNodeSkillComponent::PushTriggerNotify(GTriggerNotify* pTriggerNotify)
//{
//    if (!pTriggerNotify)
//        return;

//    int32 nType = pTriggerNotify->m_nType;
//    if (CanTriggerNotify(nType))
//    {
//        if (pTriggerNotify->IsTriggerAtOnce())
//        {
//            ProcessTriggerNotify(pTriggerNotify, m_vTriggerSkillList[nType]);
//            FACTORY_DELOBJ(pTriggerNotify);
//        }
//        else
//        {
//            m_vTriggerNotifyList[m_nTriggerNotifyCache].push_back(pTriggerNotify);
//        }
//    }
//    else
//    {
//        FACTORY_DELOBJ(pTriggerNotify);
//    }
//}
////接受通用触发消息
//void GNodeSkillComponent::PushTriggerNotify(int32 nSkillID, int32 nTargetID, int32 nType, int32 nFlag, int32 nValue, Vector3* pSrcPos, Vector3* pTarPos, Vector3* pAimDir)
//{
//    if (CanTriggerNotify(nType))
//    {
//        GTriggerNotify* pNotify = FACTORY_NEWOBJ(GTriggerNotifyNormal);
//        if (pNotify)
//        {
//            pNotify->m_nDataID = nSkillID;
//            pNotify->m_nTargetID = nTargetID;
//            pNotify->m_nType = nType;
//            pNotify->m_nFlag = nFlag;
//            pNotify->m_nValue = nValue;
//            if (pSrcPos) pNotify->m_vSrcPos = *pSrcPos;
//            if (pTarPos) pNotify->m_vTarPos = *pTarPos;
//            if (pAimDir) pNotify->m_vDir = *pAimDir;
//            PushTriggerNotify(pNotify);
//        }
//    }
//}

//void GNodeSkillComponent::PushTriggerNotifyEffect(int32 nSkillID, int32 nTargetID, int32 nType, int32 nFlag, f32* fValue)
//{
//    if (CanTriggerNotify(nType))
//    {
//        GTriggerNotifyEffect* pNotify = FACTORY_NEWOBJ(GTriggerNotifyEffect);
//        if (pNotify)
//        {
//            pNotify->m_nDataID = nSkillID;
//            pNotify->m_nTargetID = nTargetID;
//            pNotify->m_nType = nType;
//            pNotify->m_nFlag = nFlag;
//            pNotify->m_pValue = fValue;
//            PushTriggerNotify(pNotify);
//        }
//    }
//}

//void GNodeSkillComponent::PushTriggerNotifyAValue(int32 nSkillID, int32 nTargetID, int32 nType, int32 nFlag, RoleAValue* pRoleAValue)
//{
//    if (CanTriggerNotify(nType))
//    {
//        GTriggerNotifyCombine* pNotify = FACTORY_NEWOBJ(GTriggerNotifyCombine);
//        if (pNotify)
//        {
//            pNotify->m_nDataID = nSkillID;
//            pNotify->m_nTargetID = nTargetID;
//            pNotify->m_nType = nType;
//            pNotify->m_nFlag = nFlag;
//            pNotify->m_pRoleAValue = pRoleAValue;
//            PushTriggerNotify(pNotify);
//        }
//    }
//}

////处理触发消息
//void GNodeSkillComponent::ProcessTriggerNotify()
//{
//    GTriggerNotifyList & vNotifyList = m_vTriggerNotifyList[m_nTriggerNotifyCache];
//    m_nTriggerNotifyCache = !m_nTriggerNotifyCache; //对调消息缓存

//    GTriggerNotifyList::iterator iter = vNotifyList.begin();
//    GTriggerNotifyList::iterator iter_end = vNotifyList.end();
//    for (iter; iter != iter_end; ++iter)
//    {
//        GTriggerNotify* pNotify = *iter;
//        if (pNotify && pNotify->m_nType >= 0 && pNotify->m_nType < NotifyType_Count)
//        {
//            GSkillLogicList & vTriggerList = m_vTriggerSkillList[pNotify->m_nType];
//            ProcessTriggerNotify(pNotify, vTriggerList);
//        }
//        FACTORY_DELOBJ(pNotify);
//    }
//    vNotifyList.clear();
//}

//void GNodeSkillComponent::ProcessTriggerNotify(GTriggerNotify* pTriggerNotify, GSkillLogicList& vTriggerList)
//{
//    if (!pTriggerNotify)
//        return;

//    GSkillLogicList::iterator iter = vTriggerList.begin();
//    GSkillLogicList::iterator iter_end = vTriggerList.end();
//    for (; iter != iter_end; ++iter)
//    {
//        GSkillSpellLogic* pTriggerLogic = *iter;
//        if (pTriggerLogic)
//        {
//            pTriggerLogic->ProcessTrigger(pTriggerNotify);
//        }
//    }
//}

//int32 GNodeSkillComponent::SpellUsableSkill()
//{
//    GNodeAvatar* pSrc = (GNodeAvatar*)m_pOwner;
//    GNodeAvatar* pTar = pSrc->GetTargetAvatar();
//    if (!pTar)
//    {
//        return -1;
//    }

//    GSkillTargetInfo sTarInfo;
//    sTarInfo.m_nTargetID = pTar->GetAvatarID();
//    sTarInfo.m_vSrcPos = m_pOwner->GetPos();
//    sTarInfo.m_vAimDir = m_pOwner->GetDir();
//    sTarInfo.m_vTarPos = pTar->GetPos();
//    sTarInfo.m_nShapeID = 0;

//    int32 cnt = (int32)m_SkillPriorityList.size();
//    for (int32 i = 0; i < cnt; ++i)
//    {
//        GSkillSpellLogic* pSpellLogic = m_SkillPriorityList[i];
//        if (!pSpellLogic)
//        {
//            continue;
//        }

//        if (SpellSkill(pSpellLogic->GetSkillID(), sTarInfo))
//        {
//            return pSpellLogic->GetSkillID();
//        }
//    }
//    return -1;
//}

//int32 GNodeSkillComponent::SpellSkillByID(int32 nSkillID)
//{
//    GNodeAvatar* pSrc = (GNodeAvatar*)m_pOwner;
//    GNodeAvatar* pTar = pSrc->GetTargetAvatar();
//    if (!pTar)
//    {
//        return -1;
//    }

//    GSkillTargetInfo sTarInfo;
//    sTarInfo.m_nTargetID = pTar->GetAvatarID();
//    sTarInfo.m_vSrcPos = m_pOwner->GetPos();
//    sTarInfo.m_vAimDir = m_pOwner->GetDir();
//    sTarInfo.m_vTarPos = pTar->GetPos();
//    sTarInfo.m_vMoveTarPos = sTarInfo.m_vSrcPos;
//    sTarInfo.m_nShapeID = 0;

//    int32 cnt = (int32)m_SkillPriorityList.size();
//    for (int32 i = 0; i < cnt; ++i)
//    {
//        GSkillSpellLogic* pSpellLogic = m_SkillPriorityList[i];
//        if (!pSpellLogic || nSkillID != pSpellLogic->GetSkillID())
//        {
//            continue;
//        }

//        if (SpellSkill(pSpellLogic->GetSkillID(), sTarInfo))
//        {
//            return pSpellLogic->GetSkillID();
//        }
//    }
//    return -1;
//}

//int32 GNodeSkillComponent::GetSkillRange()
//{
//    int32 nSkillRange = 9999;
//    int32 cnt = (int32)m_SkillPriorityList.size();
//    for (int32 i = 0; i < cnt; ++i)
//    {
//        GSkillSpellLogic* pSpellLogic = m_SkillPriorityList[i];
//        if (!pSpellLogic)
//        {
//            continue;
//        }

//        if (!pSpellLogic->m_pSkillData)
//        {
//            continue;
//        }

//        if (pSpellLogic->m_pSkillData->GetFloatValue(MSV_Range) > 0 &&
//            pSpellLogic->m_pSkillData->GetFloatValue(MSV_Range) < nSkillRange)
//        {
//            nSkillRange = pSpellLogic->m_pSkillData->GetFloatValue(MSV_Range);
//        }
//    }

//    return nSkillRange;
//}

//void GSkillLogicList::Remove(GSkillSpellLogic* pSpellLogic)
//{
//    GSkillLogicList::iterator iter = begin();
//    GSkillLogicList::iterator iter_end = end();
//    for (; iter != iter_end; ++iter)
//    {
//        if (pSpellLogic == *iter)
//        {
//            erase(iter);
//            break;
//        }
//    }
//}
//}