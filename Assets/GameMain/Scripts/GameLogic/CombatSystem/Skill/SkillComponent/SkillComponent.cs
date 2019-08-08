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
		public List<int> m_ProjectileToRemove;

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
		protected Dictionary<int, List<GSkillSpellLogic>> m_TriggerSkillList;

		protected override void InitComponent()
		{
			m_ProjectileDict = new Dictionary<int, List<GSkillProjectile>>();
			m_ProjectileToRemove = new List<int>();

			m_TriggerNotifyCurList = new List<GTriggerNotify>();
			m_TriggerNotifyWaitList = new List<GTriggerNotify>();

			m_SkillLogicDict = new Dictionary<int, GSkillSpellLogic>();
			m_PassiveList = new List<GSkillSpellLogic>();
			m_PassiveTempList = new List<GSkillSpellLogic>();
			m_TriggerSkillList = new Dictionary<int, List<GSkillSpellLogic>>();
			for(int i = 0; i < (int)eTriggerNotifyType.NotifyType_Count; i++)
			{
				m_TriggerSkillList.Add(i, new List<GSkillSpellLogic>());
			}
		}

		public override void OnPreDestroy()
		{
			FinishSkill();
			ClearPassiveSkill();
			ClearSkillProjectile();

			if(m_ProjectileDict != null)
			{
				m_ProjectileDict.Clear();
			}
			if(m_ProjectileToRemove != null)
			{
				m_ProjectileToRemove.Clear();
			}
			if(m_TriggerNotifyCurList != null)
			{
				m_TriggerNotifyCurList.Clear();
			}
			if(m_TriggerNotifyWaitList != null)
			{
				m_TriggerNotifyWaitList.Clear();
			}

			if(m_SkillLogicDict != null)
			{
				foreach(var item in m_SkillLogicDict)
				{
					ReferencePool.Release(item.Value);
				}
				m_SkillLogicDict.Clear();
			}
		}

		public void Update()
		{
			TickProjectile(Time.deltaTime);
			TickPassiveSkill(Time.deltaTime);
			TickSkill(Time.deltaTime);
			ProcessTriggerNotify();
		}

		//清除被动技能
		private void ClearPassiveSkill()
		{
			if(m_TriggerSkillList != null)
			{
				foreach(var item in m_TriggerSkillList)
				{
					item.Value.Clear();
				}
				m_TriggerSkillList.Clear();
			}
			if(m_PassiveList != null)
			{
				m_PassiveList.Clear();
			}
			if(m_PassiveTempList != null)
			{
				m_PassiveTempList.Clear();
			}
		}
		
		private void ClearSkillProjectile()
		{
			foreach(var list in m_ProjectileDict)
			{
				foreach(var item in list.Value)
				{
					ReferencePool.Release(item);
				}
				list.Value.Clear();
			}
			m_ProjectileDict.Clear();
		}

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

		//获取技能，如果不是实例技能，返回模版技能
		public DRSkillData GetSkillData(int nSkillID)
		{
			GSkillSpellLogic pSpellLogic = GetSkill(nSkillID);
			if(pSpellLogic != null)
				return pSpellLogic.m_pSkillData;

			DRSkillData pSkillData = GameEntry.DataTable.GetDataTable<DRSkillData>().GetDataRow(nSkillID);
			if(pSkillData != null && pSkillData.IsTemplateSkill())
				return pSkillData;

			return null;
		}

		private void TickSkill(float fFrameTime)
		{
			if(m_pSpellLogic == null)
				return;
			if(m_pSpellLogic.IsFinished())
			{
				m_pSpellLogic.Tick(fFrameTime);
			}

			if(m_pSpellLogic.IsFinished())
			{
				FinishSkill();
			}
		}

		private void TickPassiveSkill(float fFrameTime)
		{
			foreach(var pSpellLogic in m_PassiveTempList)
			{
				if(pSpellLogic == null)
					continue;

				m_PassiveList.Add(pSpellLogic);
				AddTriggerSkill(pSpellLogic);
			}
			m_PassiveTempList.Clear();

			foreach(var pSpellLogic in m_PassiveList)
			{
				if(pSpellLogic == null)
					continue;
				pSpellLogic.Tick(fFrameTime);
			}
		}
		
		private void TickProjectile(float fFrameTime)
		{
			if(m_ProjectileDict == null)
				return;

			foreach(KeyValuePair<int, List<GSkillProjectile>> vList in m_ProjectileDict)
			{
				for(int i = vList.Value.Count-1; i >=0; i--)
				{
					if(vList.Value[i]==null)
					{
						vList.Value.RemoveAt(i);
						continue;
					}

					vList.Value[i].Tick(fFrameTime, Owner);
					if(vList.Value[i].IsDestroy())
					{
						ReferencePool.Release(vList.Value[i]);
						vList.Value.RemoveAt(i);
						continue;
					}
				}
				if(vList.Value.Count == 0)
				{
					m_ProjectileToRemove.Add(vList.Key);
				}
			}

			if(m_ProjectileToRemove.Count!=0)
			{
				foreach(var key in m_ProjectileToRemove)
				{
					m_ProjectileDict.Remove(key);
				}
				m_ProjectileToRemove.Clear();
			}
		}


		public bool CanTriggerNotify(int nType)
		{
			return (nType >= 0
				&& nType < (int)eTriggerNotifyType.NotifyType_Count
				&& m_TriggerSkillList.ContainsKey(nType)
				&& m_TriggerSkillList[nType].Count != 0);
		}

		//接受触发消息
		public void PushTriggerNotify(GTriggerNotify pTriggerNotify)
		{
			if(pTriggerNotify == null)
				return;

			int nType = pTriggerNotify.m_nType;
			if(CanTriggerNotify(nType))
			{
				if(pTriggerNotify.IsTriggerAtOnce())
				{
					ProcessTriggerNotify(pTriggerNotify, m_TriggerSkillList[nType]);
					ReferencePool.Release(pTriggerNotify);
				}
				else
				{
					m_TriggerNotifyWaitList.Add(pTriggerNotify);
				}
			}
			else
			{
				ReferencePool.Release(pTriggerNotify);
			}
		}
		//接受通用触发消息
		public void PushTriggerNotify(int nSkillID, int nTargetID, int nType, int nFlag, 
			int nValue, Vector3 pSrcPos, Vector3 pTarPos, Vector3 pAimDir)
		{
			if(CanTriggerNotify(nType))
			{
				GTriggerNotify pNotify = ReferencePool.Acquire<GTriggerNotifyNormal>();
				if(pNotify != null)
				{
					pNotify.m_nDataID = nSkillID;
					pNotify.m_nTargetID = nTargetID;
					pNotify.m_nType = nType;
					pNotify.m_nFlag = nFlag;
					pNotify.m_nValue = nValue;
					pNotify.m_vSrcPos = pSrcPos;
					pNotify.m_vTarPos = pTarPos;
					pNotify.m_vDir = pAimDir;
					PushTriggerNotify(pNotify);
				}
			}
		}
		// 伤害吸收触发事件
		public void PushTriggerNotifyEffect(int nSkillID, int nTargetID, int nType,
			 int nFlag, ModifyCalculation pValue)
		{
			if(CanTriggerNotify(nType))
			{
				GTriggerNotifyEffect pNotify = ReferencePool.Acquire<GTriggerNotifyEffect>();
				if(pNotify != null)
				{
					pNotify.m_nDataID = nSkillID;
					pNotify.m_nTargetID = nTargetID;
					pNotify.m_nType = nType;
					pNotify.m_nFlag = nFlag;
					pNotify.m_pValue = pValue;
					PushTriggerNotify(pNotify);
				}
			}
		}

		//处理触发消息
		public void ProcessTriggerNotify()
		{
			foreach(var pNotify in m_TriggerNotifyCurList)
			{
				if(pNotify != null && pNotify.m_nType >= 0
					&& pNotify.m_nType < (int)eTriggerNotifyType.NotifyType_Count
					&& m_TriggerSkillList.ContainsKey(pNotify.m_nType))
				{
					ProcessTriggerNotify(pNotify, m_TriggerSkillList[pNotify.m_nType]);
				}
				ReferencePool.Release(pNotify);
			}
			m_TriggerNotifyCurList.Clear();
			m_TriggerNotifyCurList.AddRange(m_TriggerNotifyWaitList);
			m_TriggerNotifyWaitList.Clear();
		}

		public void ProcessTriggerNotify(GTriggerNotify pTriggerNotify, List<GSkillSpellLogic> vTriggerList)
		{
			if(pTriggerNotify == null)
				return;

			foreach(var pTriggerLogic in vTriggerList)
			{
				if(pTriggerLogic == null)
					continue;
				pTriggerLogic.ProcessTrigger(pTriggerNotify);
			}
		}

		public void BeforeChangeScene()
		{
			FinishSkill();
			ClearSkillProjectile();
		}
		public void BeforeExitScene()
		{
			FinishSkill();
			ClearSkillProjectile();
		}
		//死亡处理
		public void OnDead()
		{
			FinishSkill();
		}
	}

}
