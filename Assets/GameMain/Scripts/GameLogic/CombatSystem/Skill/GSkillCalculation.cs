using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
	/// <summary>
	/// 技能效果结算
	/// </summary>
	public class GSkillCalculation
	{
		public DRSkillData m_pSkillData;
		public Avatar m_pCaster;
		public Avatar m_pTarget;
		public PlayerAValueData m_CasterAValue;
		public PlayerAValueData m_TargetAValue;
		public SkillAValueData m_SkillAValue;

		public GSkillCalculation()
		{
		}

		public void DecisionOnHit(ref int nEffectType)
		{
			if(m_pSkillData == null || !m_pCaster || !m_pTarget)
				return;

			////检查无敌 todo等之后添加完整体角色标记位后修复
			//if(m_pTarget.CheckState(GAS_God))
			//	return;

			nEffectType |= (int)eTriggerNotify.TriggerNotify_Hit;
		}

		public void TransfromEffectTarget()
		{
			//if(!m_pSkillData || !m_pCaster || !m_pTarget)
			//	return;

			////转换技能效果
			//int32 nTransfromID = m_pSkillData->GetIntValue(MSV_EffectTransform);
			//GSkillEffectTransform* pTransform = GSkillDataManager::Instance().GetSkillEffectTransform(nTransfromID);
			//if(!pTransform)
			//	return;
		}

		public float GetPower()
		{
			if(m_pSkillData == null)
				return 0;

			float fValue = m_SkillAValue.GetFloatValue(SkillAValueDefine.ad_skill_p);
			//计算攻击
			if(m_pSkillData.IsCalculationAtk())
			{
				fValue += Mathf.Max(m_CasterAValue.GetFloatValue(AvatarAValueDefine.atk_d)
					* (1 + (float)m_CasterAValue.GetPercentValue(AvatarAValueDefine.atk_d_r)), 1);
			}
			return fValue;
		}

		//伤害会心判断
		private void DecisionOnDamageCrit(ref int nEffectType)
		{
			//if(m_pSkillData == null || !m_pCaster || !m_pTarget)
			//	return;
			//if(!m_pSkillData->IsCalculationCrit())
			//	return;

			//float C0 = 0.6f;
			//float C1 = 200.0f;
			//float C2 = 0.04f;
			//float fDHR = MIN(C0, (m_CasterAValue.Values[dhp] + C1) / (m_TargetAValue.Values[be_dhp] + C1) * C2);
			//fDHR += m_CasterAValue.Values[dhr] - m_TargetAValue.Values[be_dhr];

			//float fRand = GALAXY_RANDOM.RandFloat();
			//if(fDHR >= fRand)
			//	nEffectType |= TriggerNotify_Crit;
		}

		//伤害结算
		public void CalculationDamage(ref int nEffectType, ref float fEffectValue)
		{
			if(m_pSkillData == null || !m_pCaster || !m_pTarget)
				return;

			//命中判断
			DecisionOnHit(ref nEffectType);
			if((nEffectType & (int)eTriggerNotify.TriggerNotify_Hit) <= 0)
				return;
			
			//会心判断
			DecisionOnDamageCrit(ref nEffectType);

			float fAtkPower = GetPower();

			//计算护甲
			//if(m_pSkillData->IsCalculationAC())
			{
				float fAc = m_TargetAValue.GetFloatValue(AvatarAValueDefine.d_ac) * 
					(1 + (float)m_TargetAValue.GetPercentValue(AvatarAValueDefine.d_ac_r));
				if(fAc > 0 && fAtkPower > 0)
				{
					fAtkPower = fAtkPower * (1 - fAc / (fAc + fAtkPower));
				}
			}

			//if((nEffectType & (int)eTriggerNotify.TriggerNotify_Crit) > 0)
			//{
			//	float MIN_K_DHCR = 0;
			//	float MAX_K_DHCR = 1;
			//	float fDHCR = MIN(MAX(m_CasterAValue.Values[dhcr] - m_TargetAValue.Values[be_dhcr], MIN_K_DHCR), MAX_K_DHCR);
			//	fAtkPower *= (1.5 + fDHCR);
			//}

			//f32 fDDR = 0;
			////计算伤增/伤减
			//if(m_pSkillData->IsCalculationDR())
			//{
			//	f32 MIN_K_DDR = -1.0f;
			//	f32 MAX_K_DDR = 1.0f;
			//	fDDR = m_CasterAValue.Values[dr] - m_TargetAValue.Values[cdr];
			//	fDDR = MIN(MAX(fDDR, MIN_K_DDR), MAX_K_DDR);
			//}

			float fDamage = fAtkPower * (float)m_SkillAValue.GetPercentValue(SkillAValueDefine.ad_skill_r);
			//float fDamageDM = 0.0f; //计算真实伤害
			//if(m_pSkillData->IsCalculationDM())
			//{
			//	fDamageDM = MAX(m_CasterAValue.Values[dm] - m_TargetAValue.Values[da], 0);
			//}

			float fDFR = 0.1f;
			float fRandom = Random.Range(1 - fDFR, 1 + fDFR);
			float fTotalDamage = Mathf.Max(0, fDamage * fRandom);

			ModifyCalculation pModifyCal;

			pModifyCal = ReferencePool.Acquire<ModifyCalculation>();
			pModifyCal.type = eTriggerNotifyType.NotifyType_MakeDamage;
			pModifyCal.fValue = fTotalDamage;
			//造成的伤害
			if(m_pCaster.SkillCom)
			{
				m_pCaster.SkillCom.PushTriggerNotifyEffect(m_pSkillData.Id, m_pTarget.Id, 
					(int)eTriggerNotifyType.NotifyType_MakeDamage, nEffectType, pModifyCal);
			}
			ReferencePool.Release(pModifyCal);

			pModifyCal = ReferencePool.Acquire<ModifyCalculation>();
			pModifyCal.type = eTriggerNotifyType.NotifyType_TakeDamage;
			pModifyCal.fValue = fTotalDamage;
			//承受的伤害
			if(m_pTarget.SkillCom)
			{
				m_pTarget.SkillCom.PushTriggerNotifyEffect(m_pSkillData.Id, m_pCaster.Id,
					(int)eTriggerNotifyType.NotifyType_TakeDamage, nEffectType, pModifyCal);
			}
			ReferencePool.Release(pModifyCal);

			if(fTotalDamage >= 1.0f)
			{
				fEffectValue = fTotalDamage;
				m_pTarget.SetDamage(m_pCaster.Id, fEffectValue);
			}
		}

		//治疗会心判断
		private void DecisionOnHealCrit(ref int nEffectType)
		{
			//if(!m_pSkillData || !m_pCaster || !m_pTarget)
			//	return;
			//if(!m_pSkillData->IsCalculationCrit())
			//	return;

			//float fCrit = Mathf.Max(m_CasterAValue.Values[dhr], 0);
			//float fRand = GALAXY_RANDOM.RandFloat();
			//if(fCrit >= fRand)
			//	nEffectType |= TriggerNotify_Crit;
		}

		//治疗结算
		public void CalculationHeal(ref int nEffectType, ref float fEffectValue)
		{
			if(m_pSkillData == null || !m_pCaster || !m_pTarget)
				return;

			//会心判断
			DecisionOnHealCrit(ref nEffectType);

			//计算治疗
			float fHeal = GetPower();

			//if((nEffectType & (int)eTriggerNotify.TriggerNotify_Crit) > 0)
			//{
			//	fHeal *= (1.5 + m_CasterAValue.Values[dhcr]);
			//}

			//float fHHP_R = 0;
			//if(m_pSkillData->IsCalculationHHR())
			//{
			//	float MIN_K_HHP_R = -1;
			//	float MAX_K_HHP_R = 2;
			//	fHHP_R = MIN(MAX((m_CasterAValue.Values[hhp_r] + m_TargetAValue.Values[be_hhp_r]), MIN_K_HHP_R), MAX_K_HHP_R);
			//}

			fHeal *= m_SkillAValue.GetFloatValue(SkillAValueDefine.ad_skill_r);
			if(fHeal >= 1.0f)
			{
				fEffectValue = fHeal;
				m_pTarget.SetHeal(m_pCaster.Id, fEffectValue);
			}
		}

	}
}