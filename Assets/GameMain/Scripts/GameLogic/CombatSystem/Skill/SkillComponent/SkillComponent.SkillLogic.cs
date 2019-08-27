using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Galaxy
{
	public partial class SkillComponent
	{
		public int GetCurrentSkillID()
		{
			if(m_pSpellLogic == null)
				return -1;
			return m_pSpellLogic.m_pSkillData.Id;
		}

		public void FinishSkill()
		{
			if(m_pSpellLogic == null)
				return;
			m_pSpellLogic.Reset();
			m_pSpellLogic = null;
		}

		//处理技能效果
		public void ProcessSkillEffect(DRSkillData pSkillData, GTargetInfo sTarInfo, SkillAValueData AValue)
		{
			GSkillExcludeList vExcludeList = new GSkillExcludeList();
			ProcessSkillEffect(pSkillData, sTarInfo, AValue, vExcludeList);
		}

		public void ProcessSkillEffect(DRSkillData pSkillData, GTargetInfo sTarInfo, SkillAValueData AValue, GSkillExcludeList vExcludeList)
		{
			if(pSkillData == null)
				return;

			Avatar pCaster = Owner;
			if(!pCaster)
				return;

			int nAreaLogic = pSkillData.MSV_AreaLogic;
			GSkillAreaLogic pAreaLogic = GSkillLogicManager.Instance.GetAreaLogic(nAreaLogic);
			if(pAreaLogic == null)
				return;
			
			if(nAreaLogic != (int)eSkillAreaLogic.SkillArea_Singleton && !pSkillData.IsAreaIncludeSelf())
				vExcludeList.Add(pCaster.Id);

			List<Avatar> vTargetList = pAreaLogic.GetTargetList(pSkillData, pCaster, sTarInfo, vExcludeList);
			if(vTargetList == null || vTargetList.Count == 0)
				return;

			////合并攻击方属性 todo合并技能属性集和模板属性集
			//RoleAValue sSkillAValue;
			//sSkillAValue.Copy(sRoleValue);
			//sSkillAValue.Combine(pSkillData->m_RoleValue);

			//RoleAValue sCasterRoleValue;
			//sCasterRoleValue.Copy(sSkillAValue);
			//sCasterRoleValue.Combine(pCaster->GetRoleAValue());

			//属性合并节点 todo 等技能数据包裹修复后
			//if(pSkillData->IsCombineEffectNotify())
			//{
			//	PushTriggerNotifyAValue(pSkillData->m_nDataID, 0, NotifyType_CombineEffect, pSkillData->GetIntValue(MSV_EffectType), &sCasterRoleValue);
			//}

			int nEffectLogic = pSkillData.MSV_EffectLogic;
			GSkillEffect pEffectLogic = GSkillLogicManager.Instance.GetEffectLogic(nEffectLogic);
			if(pEffectLogic == null)
				return;

			GTargetInfo tempTarInfo;
			foreach(var pTarget in vTargetList)
			{
				if(pTarget == null)
					continue;
				
				if(vExcludeList.Contains(pTarget.Id))
					continue;

				PlayerAValueData sCasterRoleValue = pCaster.GetRoleAValue();
				PlayerAValueData sTargetRoleValue = pTarget.GetRoleAValue();
				if(sCasterRoleValue == null || sCasterRoleValue == null)
					continue;

				GSkillCalculation sCaluation = new GSkillCalculation();
				
				sCaluation.m_pSkillData = pSkillData;
				sCaluation.m_pCaster = pCaster;
				sCaluation.m_pTarget = pTarget;
				sCaluation.m_CasterAValue = sCasterRoleValue.CloneData();
				sCaluation.m_TargetAValue = sTargetRoleValue.CloneData();
				sCaluation.m_SkillAValue = AValue;
				sCaluation.TransfromEffectTarget();

				tempTarInfo = sTarInfo;
				tempTarInfo.m_nTargetID = pTarget.Id;
				tempTarInfo.m_vTarPos = pTarget.GetPos();

				if(pEffectLogic.Process(sCaluation, tempTarInfo, AValue))
				{
					//todo 添加log范围
					//pAreaLogic.Draw(pSkillData, pCaster, pTarget, sTarInfo);
				}
				//填充重复列表
				if(pSkillData.IsAreaAddExclude())
				{
					vExcludeList.Add(pTarget.Id);
				}
				//减少效果次数
				if(vExcludeList.m_nCount > 0)
				{
					--vExcludeList.m_nCount;
				}
			}
		}

		//创建技能子弹 todo技能子弹修复后添加
		public void CreateSkillProjectile(DRSkillData pSkillData, GTargetInfo sTarInfo, SkillAValueData m_SkillValue)
		{
			//if(Owner == null)
			//	return;

			//if(pSkillData == null)
			//	return;
			
			//int nType = pSkillData.MSV_ProjectileLogic;
			//GSkillProjectile pProjectile = GSkillLogicManager::Instance().CreateProjectile(nType);
			//if(!pProjectile)
			//	return;

			//if(!pProjectile->Init(pSkillData, sTarInfo, sRoleValue, (GNodeAvatar*)m_pOwner))
			//{
			//	FACTORY_DELOBJ(pProjectile);
			//	return;
			//}

			//if(m_pOwner->GetScene())
			//{
			//	((GNodeScene*)m_pOwner->GetScene())->ChangeProjsCount(+1);
			//}

			//pProjectile->SetID(++m_nProjectileID);
			////子弹坐标使用当前角色坐标
			//if(pSkillData->IsBulletBornTarPos())
			//{
			//	pProjectile->m_TargetInfo.m_vSrcPos = pProjectile->m_TargetInfo.m_vTarPos;
			//	pProjectile->m_TargetInfo.m_vSrcPos.z = m_pOwner->GetSceneHeight(pProjectile->m_TargetInfo.m_vTarPos);
			//}
			
			//pProjectile->Tick(0, Owner);     //MSV_ProjectileFirstEffectTime 为0的情况
			//if(m_ProjectileDict.ContainsKey(pSkillData.Id))
			//{
			//	m_ProjectileDict[pSkillData.Id].Add(pProjectile);
			//}
			//else
			//{
			//	List<GSkillProjectile> list = new List<GSkillProjectile>();
			//	list.Add(pProjectile);
			//	m_ProjectileDict[pSkillData.Id].Add(list);
			//}
		}

		//使用技能
		public bool SpellSkill(int nSkillID, GTargetInfo sTarInfo)
		{
			if(Owner == null)
				return false;

			GSkillSpellLogic pSpellLogic = GetSkill(nSkillID);
			if(pSpellLogic == null)
			{
				DRSkillData skillData = GameEntry.DataTable.GetDataTable<DRSkillData>().GetDataRow(nSkillID);
				if(skillData != null && skillData.IsTemplateSkill())
				{
					if(AddSkill(nSkillID))
						pSpellLogic = GetSkill(nSkillID);
				}
			}
			if(pSpellLogic == null)
				return false;

			DRSkillData pSkillData = pSpellLogic.m_pSkillData;
			if(pSkillData == null)
				return false;
			
			if(!CanSpellSkill(pSkillData, sTarInfo))
				return false;

			if(m_pSpellLogic != null)
				FinishSkill();

			if(!pSpellLogic.SetFSMState())
				return false;

			//初始化施法逻辑
			m_pSpellLogic = pSpellLogic;
			
			m_pSpellLogic.Reset();
			if(sTarInfo.m_vAimDir == default(Vector3))
				sTarInfo.m_vAimDir = Owner.GetDir();

			m_pSpellLogic.m_TargetInfo = sTarInfo;

			m_pSpellLogic.ResetSkillAValue();

			if(!pSkillData.IsEffectStateCost())
			{
				DoCost(pSkillData);
				StartCD(pSkillData, true);
				//todo 允许部分技能在生效时产生消耗
				//m_pSpellLogic.SetCosted();
			}

			m_pSpellLogic.ReInit();
			sTarInfo = m_pSpellLogic.m_TargetInfo;

			Log.Debug("Avatar'{0}' Skill'{1}' Spell", Owner.Id, nSkillID);

			//产生技能事件
			if(pSkillData.IsTriggerSkillNotify())
			{
				PushTriggerNotify(nSkillID, sTarInfo.m_nTargetID,
					(int)eTriggerNotifyType.NotifyType_Skill,
					(int)eTriggerNotify.TriggerNotify_SkillStart, 0,
					sTarInfo.m_vSrcPos, sTarInfo.m_vTarPos, sTarInfo.m_vAimDir);
			}

			//执行瞬发效果
			m_pSpellLogic.Tick(0);
			return true;
		}

		//检查使用技能
		public bool CanSpellSkill(int nSkillID, GTargetInfo sTarInfo)
		{
			return CanSpellSkill(GetSkillData(nSkillID), sTarInfo);
		}

		public bool CanSpellSkill(DRSkillData pSkillData, GTargetInfo sTarInfo)
		{
			Avatar pCaster = Owner;
			if(!pCaster || pSkillData == null)
				return false;

			if(!pSkillData.IsActiveSkill())
				return false;

			//检查当前施放技能
			if(m_pSpellLogic != null && m_pSpellLogic.IsLock())
			{
				return false;
			}

			//检查消耗
			if(!CheckCost(pSkillData))
			{
				return false;
			}
			//检查CD
			if(!CheckCD(pSkillData))
			{
				return false;
			}

			//检查施法者位置
			if(pCaster.GetPos().Distance2D(sTarInfo.m_vSrcPos) > 64.0f)
				return false;

			//自身检查
			int nSrvCheck = pSkillData.MSV_SrcCheck;
			if(nSrvCheck > 0)
			{
				//todo 添加条件检查组后修复
				//if(!GSKillConditionCheckManager::Instance().Check(nSrvCheck, pCaster))
				//	return false;
			}

			//目标检查
			if(!GSkillLogicManager.Instance.CheckTarget(pSkillData, pCaster, sTarInfo))
				return false;

			return true;
		}

		//检查技能消耗
		public bool CheckCost(DRSkillData pSkillData)
		{
			if(Owner == null || pSkillData == null)
				return false;

			if(!CheckCost(pSkillData.MSV_CostType1, pSkillData.MSV_CostValue1))
				return false;
			if(!CheckCost(pSkillData.MSV_CostType2, pSkillData.MSV_CostValue2))
				return false;

			return true;
		}
		//根据角色类型初始化角色属性集添加对应的能量值
		private bool CheckCost(int nCostType, float fCostValue)
		{
			switch((eSkillCost)nCostType)
			{
				case eSkillCost.SkillCost_Hp:
					return (Owner.HP > fCostValue);
				//case eSkillCost.SkillCost_Mp:
				//	return (Owner.MP >= fCostValue);
				//case eSkillCost.SkillCost_E1:
				//	return (Owner.GetEp(1) >= fCostValue);
				//case eSkillCost.SkillCost_E2:
				//	return (Owner.GetEp(2) >= fCostValue);
				default:
					return true;
			}
		}

		//扣除技能消耗
		public void DoCost(DRSkillData pSkillData)
		{
			if(Owner == null || pSkillData == null)
				return;

			DoCost(pSkillData.MSV_CostType1, pSkillData.MSV_CostValue1, pSkillData.IsActiveSkill());
			DoCost(pSkillData.MSV_CostType2, pSkillData.MSV_CostValue2, pSkillData.IsActiveSkill());

		}
		private void DoCost(int nCostType, float fCostValue, bool bActive = false)
		{
			if(fCostValue == 0)
				return;

			switch((eSkillCost)nCostType)
			{
				case eSkillCost.SkillCost_Hp:
					float hp = Mathf.Max(1, fCostValue);
					Owner.SetHpCost(hp);
					return;
				//case eSkillCost.SkillCost_Mp:
				//	return (Owner.MP >= fCostValue);
				//case eSkillCost.SkillCost_E1:
				//	float ep1 = Mathf.Max(0, Owner.GetEp(1) - fCostValue);
				//	Owner.SetEp(1, ep1);
				//	return;
				//case eSkillCost.SkillCost_E2:
				//	float ep2 = Mathf.Max(0, Owner.GetEp(2) - fCostValue);
				//	Owner.SetEp(2, ep2);
				//	return;
				default:
					return;
			}
		}

		//检查技能CD
		public bool CheckCD(DRSkillData pSkillData)
		{
			if(Owner == null || pSkillData == null)
				return false;

			//检查CD
			int nCDGroup = pSkillData.MSV_CDGroup;
			if(nCDGroup < 0)//-1 no cd
				return true;

			if(Owner.CDCom == null || Owner.CDCom.CheckCD(nCDGroup))
			{
				return false;
			}
			return true;
		}

		//开始技能CD
		public void StartCD(DRSkillData pSkillData, bool bStart)
		{
			if(Owner == null || pSkillData == null)
				return;
			
			if(Owner.CDCom == null)
				return;
			Owner.CDCom.StartCD(pSkillData.MSV_CDGroup, pSkillData.MSV_CDTime);
		}
	}
}