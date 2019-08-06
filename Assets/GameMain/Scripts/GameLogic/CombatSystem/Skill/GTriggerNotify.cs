using UnityEngine;

namespace Galaxy
{
	/// <summary>
	/// 触发技能相关检查
	/// </summary>
	public class GTriggerNotify
	{
		int m_nDataID;
		int m_nTargetID;
		int m_nType;
		int m_nFlag;
		int m_nValue;
		Vector3 m_vSrcPos;
		Vector3 m_vTarPos;
		Vector3 m_vDir;

		public GTriggerNotify()
		{
			m_nDataID = 0;
			m_nTargetID = 0;
			m_nType = 0;
			m_nFlag = 0;
			m_nValue = 0;
			m_vSrcPos = default(Vector3);
			m_vTarPos = default(Vector3);
			m_vDir = default(Vector3);
		}
		public virtual eNotifyObject GetNotifyType()
		{
			return eNotifyObject.NotifyObject_Base;
		}
		//立即处理触发逻辑, 如伤害合并/假死
		public virtual bool IsTriggerAtOnce()
		{
			return false;
		}
		public virtual bool CheckTrigger(Avatar pCaster, DRSkillData pSkillData)
		{
			if(!pCaster || pSkillData == null)
				return false;

			int nType = pSkillData.MSV_TriggerType;
			if(m_nType != nType)
				return false;

			int nCheck = pSkillData.MSV_TriggerCheck;

			//检查消息DataID
			if((nCheck & (int)eSkillTriggerCheck.TriggerCheck_Data_UnChecked) <= 0)
			{
				int nDataID = pSkillData.MSV_TriggerDataID;
				if((nCheck & (int)eSkillTriggerCheck.TriggerCheck_Data_ID) > 0)
				{
					if(m_nDataID != nDataID)
						return false;
				}
			}
			//检查消息参数
			if((nCheck & (int)eSkillTriggerCheck.TriggerCheck_Flag_UnChecked) <= 0)
			{
				int nFlag = pSkillData.MSV_TriggerNotify;
				if((nCheck & (int)eSkillTriggerCheck.TriggerCheck_Flag_Equal) > 0)
				{
					if(m_nFlag != nFlag)
						return false;
				}
				else if((nCheck & (int)eSkillTriggerCheck.TriggerCheck_Flag_And) > 0)
				{
					if((m_nFlag & nFlag) != nFlag)
						return false;
				}
				else if((nCheck & (int)eSkillTriggerCheck.TriggerCheck_Flag_Or) > 0)
				{
					if((m_nFlag & nFlag) <= 0)
						return false;
				}
			}
			//检查消息数值
			if((nCheck & (int)eSkillTriggerCheck.TriggerCheck_Value_UnChecked) <= 0)
			{
				int nValue = pSkillData.MSV_TriggerValue;
				if((nCheck & (int)eSkillTriggerCheck.TriggerCheck_Value_Greater) > 0)
				{
					if(m_nValue <= nValue)
						return false;
				}
				else if((nCheck & (int)eSkillTriggerCheck.TriggerCheck_Value_GreaterAndEqual) > 0)
				{
					if(m_nValue < nValue)
						return false;
				}
				else if((nCheck & (int)eSkillTriggerCheck.TriggerCheck_Value_Less) > 0)
				{
					if(m_nValue >= nValue)
						return false;
				}
				else if((nCheck & (int)eSkillTriggerCheck.TriggerCheck_Value_LessAndEqual) > 0)
				{
					if(m_nValue > nValue)
						return false;
				}
				else if((nCheck & (int)eSkillTriggerCheck.TriggerCheck_Value_Equal) > 0)
				{
					if(m_nValue != nValue)
						return false;
				}
				else if((nCheck & (int)eSkillTriggerCheck.TriggerCheck_Value_NotEqual) > 0)
				{
					if(m_nValue == nValue)
						return false;
				}
				else if((nCheck & (int)eSkillTriggerCheck.TriggerCheck_Value_And) > 0)
				{
					if((m_nValue & nValue) != nValue)
						return false;
				}
				else if((nCheck & (int)eSkillTriggerCheck.TriggerCheck_Value_Or) > 0)
				{
					if((m_nValue & nValue) <= 0)
						return false;
				}
			}

			//检查概率
			float nProbalitity = pSkillData.MSV_TriggerProbability;
			float nRand = Random.Range(0.0f, 1.0f);
			return (nProbalitity >= nRand);
		}
	}

	/// <summary>
	/// 通用触发事件
	/// </summary>
	public class GTriggerNotifyNormal : GTriggerNotify
	{
		public override eNotifyObject GetNotifyType()
		{
			return eNotifyObject.NotifyObject_Normal;
		}
	}

	/// <summary>
	/// 伤害吸收触发事件
	/// </summary>
	public class GTriggerNotifyEffect : GTriggerNotify
	{
		//需要修正的值
		public float m_pValue;

		public override eNotifyObject GetNotifyType()
		{
			return eNotifyObject.NotifyObject_Effect;
		}
		public override bool IsTriggerAtOnce()
		{
			return true;
		}
	}
}