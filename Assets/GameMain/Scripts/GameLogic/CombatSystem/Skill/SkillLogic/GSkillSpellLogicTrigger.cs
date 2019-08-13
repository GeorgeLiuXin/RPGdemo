using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy
{
    /// <summary>
    /// 触发技能
    /// </summary>
    public class GSkillSpellLogic_Trigger : GSkillSpellLogic_Passive
	{
		public override bool ProcessTrigger(GTriggerNotify pNotify)
		{
			//检查触发条件
			if(!CheckNotify(pNotify))
				return false;
			
			Log.Debug("Skill Trigger Succ '{0}'", m_pSkillData.Id);
			//设置触发消耗
			SetTriggerCost();
			//转化技能效果
			TransfromEffectNotify(pNotify);

			Reset();
			ResetSkillAValue();

			//产生触发效果
			OnTrigger(pNotify);
			//产生触发事件
			if(m_pSkillData != null && m_pSkillData.IsTriggerTriggerNotify())
			{
				Avatar pCaster = GetCaster();
				if(pCaster != null && pCaster.SkillCom)
				{
					GetTarget();
					pCaster.SkillCom.PushTriggerNotify(m_pSkillData.Id,
						m_TargetInfo.m_nTargetID, (int)eTriggerNotifyType.NotifyType_Trigger,
						0, 0, pNotify.m_vSrcPos, pNotify.m_vTarPos, pNotify.m_vDir);
				}
			}

			return true;
		}

		public bool CheckNotify(GTriggerNotify pNotify)
		{
			if(!m_pOwner || m_pSkillData == null || pNotify == null)
				return false;

			Avatar pCaster = GetCaster();
			if(!pCaster || !pCaster.SkillCom)
				return false;

			if(m_pSkillData.IsTargetSelfOnly())
			{
				m_TargetInfo.m_nTargetID = pCaster.Id;
				m_TargetInfo.m_vSrcPos = pCaster.GetPos();
				m_TargetInfo.m_vTarPos = pCaster.GetPos();
				m_TargetInfo.m_vAimDir = pCaster.GetDir();
			}
			else
			{
				m_TargetInfo.m_nTargetID = pNotify.m_nTargetID;
				m_TargetInfo.m_vTarPos = pNotify.m_vTarPos;
				m_TargetInfo.m_vSrcPos = pNotify.m_vSrcPos;
				if(pNotify.m_vDir == default(Vector3))
				{
					m_TargetInfo.m_vAimDir = m_TargetInfo.m_vTarPos - m_TargetInfo.m_vSrcPos;
				}
				else
				{
					m_TargetInfo.m_vAimDir = pNotify.m_vDir;
				}
				m_TargetInfo.m_vAimDir.Normalize2D();
			}

			//检查cd与消耗
			if(!m_pOwner.CDCom || m_pOwner.CDCom.CheckCD(m_pSkillData.MSV_CDGroup))
				return false;
			if(!pCaster.SkillCom.CheckCost(m_pSkillData))
				return false;

			if(!pNotify.CheckTrigger(pCaster, m_pSkillData))
				return false;

			//if(m_pBuff && m_pBuff->IsInvalid())
			//	return false;

			if(!PassiveProcessCheck())
				return false;

			return true;
		}

		private void SetTriggerCost()
		{
			if(!m_pOwner || m_pSkillData == null)
				return;

			//todo 触发移除buff相关
			//if(m_pBuff && m_pOwner.GetBuffComponent())
			//{
			//	if(m_pSkillData.IsTriggerRemoveBuff())
			//	{
			//		m_pBuff.SetInvalid();
			//	}
			//	else if(m_pSkillData.IsTriggerRemoveLayer())
			//	{
			//		m_pOwner->GetBuffComponent().RemoveBuffByLayerCnt(m_pBuff, 1);
			//	}
			//}

			//消耗
			Avatar pCaster = GetCaster();
			if(pCaster!=null && pCaster.SkillCom)
			{
				pCaster.SkillCom.DoCost(m_pSkillData);
			}

			//cd
			if(pCaster != null && pCaster.CDCom)
			{
				pCaster.CDCom.StartCD(m_pSkillData.MSV_CDGroup, m_pSkillData.MSV_CDTime);
			}
		}

		public void OnTrigger(GTriggerNotify pNotify)
		{
			ProcessEffect();
		}

		//触发值合并转化
		private void TransfromEffectNotify(GTriggerNotify pNotify)
		{
			//if(!pNotify || !m_pTransform)
			//	return;

			//if(m_pTransform->m_nTransformType == SkillEffectTransform_Trigger_Value)
			//{
			//	m_AValue.Values[AValue::ad_skill_p] += pNotify->m_nValue * m_pTransform->m_fTransformPrecent;
			//}
		}
	}
}