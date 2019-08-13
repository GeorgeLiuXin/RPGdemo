using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityGameFramework.Runtime;

namespace Galaxy
{
	public class GSkillInitParam
	{
		public Avatar m_pOwner;
		public GTargetInfo m_TargetInfo;
		public DRSkillData m_pSkillData;
		//public GBuff buff;		//通过buff挂载的技能
		//public int nSlots;		//技能修正槽位

		public GSkillInitParam(Avatar pOwner, GTargetInfo targetInfo, DRSkillData pSkillData)
		{
			m_pOwner = pOwner;
			m_TargetInfo = targetInfo;
			m_pSkillData = pSkillData;
		}
	}

	/// <summary>
	/// 技能基础逻辑
	/// </summary>
	public class GSkillSpellLogic: IReference
	{
		protected bool m_bFinish;
		protected bool m_bCosted;

		protected int m_nCasterID;
		protected Avatar m_pOwner;
		public DRSkillData m_pSkillData;
		public SkillAValueData m_AValue;            //技能属性集

		public GTargetInfo m_TargetInfo;
		// todo 添加 技能属性集与玩家合并接口及数据类型
		//protected XXX m_pXXXX;

		public virtual bool Init(GSkillInitParam param)
		{
			if(param.m_pOwner == null 
				|| param.m_pSkillData == null 
				|| param.m_TargetInfo == null)
				return false;

			m_pOwner = param.m_pOwner;
			m_TargetInfo = param.m_TargetInfo;
			m_pSkillData = param.m_pSkillData;

			m_nCasterID = m_pOwner.Id;

			////属性转换
			//int32 nTransfromID = MSV_EffectTransform;
			//m_pXXXX = data.get(MSV_EffectTransform)
			return true;
		}

		public virtual bool ReInit()
		{
			return true;
		}

		public virtual void Tick(float fFrameTime)
		{

		}

		public virtual void Reset()
		{
			m_bFinish = false;
			m_bCosted = false;
			m_TargetInfo.Reset();
		}

		public void Clear()
		{
			Reset();
		}

		public virtual bool IsLock()
		{
			return false;
		}

		public virtual bool SetFSMState()
		{
			return false;
		}
		public virtual bool ProcessTrigger(GTriggerNotify pNotify)
		{
			return false;
		}

		public bool IsFinished()
		{
			return m_bFinish;
		}
		public void	Finish()
		{
			m_bFinish = true;
		}
		public virtual void SetCosted()
		{
			m_bCosted = true;
		}

		public SkillAValueData GetSkillAValue()
		{
			return m_AValue;
		}

		public void ResetSkillAValue()
		{
			m_AValue = AValueManager.Instance.GetSkillAValue();
			UpdateAValueByInstance();
			TransfromRoleAValue();
		}
		private void UpdateAValueByInstance()
		{
			List<DRSkillAValue> list = new List<DRSkillAValue>
				(GameEntry.DataTable.GetDataTable<DRSkillAValue>().GetDataRows(CheckAValueDataID));
			UpdateAValue(list);
		}
		private bool CheckAValueDataID(DRSkillAValue data)
		{
			return data.AValueID == GetSkillID();
		}
		private void UpdateAValue(List<DRSkillAValue> list)
		{
			if(m_AValue == null)
				return;
			if(list == null || list.Count == 0)
				return;
			foreach(var item in list)
			{
				object value;
				switch((AValueType)item.AValueType)
				{
					case AValueType.AValueType_Int:
						value = item.AValueInt;
						break;
					case AValueType.AValueType_Float:
						value = item.AValueFloat;
						break;
					case AValueType.AValueType_Percent:
						value = item.AValuePercent;
						break;
					default:
						Log.Error("刷新技能属性集 '{0}' 错误!", GetSkillID());
						return;
				}
				m_AValue.SetAValueData(item.AvatarAValueDefine, item.AValueType, value);
			}
		}

		/// <summary>
		/// 根据效果类型转化 将玩家/目标属性集一部分转化为技能效果固定值
		/// </summary>
		public void TransfromRoleAValue()
		{
		}


		protected Avatar GetCaster()
		{
			if(!m_pOwner)
				return null;

			return (m_pOwner.Id == m_nCasterID) ?
				m_pOwner : GameEntry.Entity.GetGameEntity(m_nCasterID) as Avatar;
		}
		
		protected Avatar GetTarget()
		{
			if(!m_pOwner)
				return null;

			return (m_pOwner.Id == m_TargetInfo.m_nTargetID) ?
				m_pOwner : GameEntry.Entity.GetGameEntity(m_TargetInfo.m_nTargetID) as Avatar;
		}

		public int GetSkillID()
		{
			return (m_pSkillData != null) ? m_pSkillData.Id : 0;
		}

		public virtual void ProcessEffect()
		{
			int nLauncherType = m_pSkillData.MSV_LauncherLogic;
			GSkillLauncher pLauncher = GSkillLogicManager.Instance.GetLauncherLogic(nLauncherType);
			if(pLauncher != null)
			{
				pLauncher.Process(this, GetCaster());
			}
		}

		////todo 技能修正
		//private void SetSkillSlots()
		//{

		//}
		//private int GetSkillSlots()
		//{

		//}
		//private void ClearSkillSlots()
		//{

		//}
	}

}