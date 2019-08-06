﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

namespace Galaxy
{
    /// <summary>
    /// 技能效果
    /// </summary>
    public abstract class GSkillEffect 
    {
		public abstract bool Process(GSkillCalculation sCalculation, GTargetInfo sTarInfo, SkillAValueData sSkillAValue);

		public void ProcessHurtThreat(PlayerAValueData sCasterAValue, Avatar pCaster, Avatar pTarget, bool bHit)
		{
			//if(!pCaster || !pTarget)
			//	return;

			//int nThreat = GetThreatValue(sCasterAValue, bHit);
			//if(pTarget.GetThreatComponent())
			//{
			//	pTarget.GetThreatComponent().OnHurt(pCaster, nThreat);
			//}
		}

		public float GetThreatValue(PlayerAValueData sCasterAValue, bool bHit)
		{
			//float fThreat = sCasterAValue.Values[AValue::threat] * (1 + sCasterAValue.Values[AValue::threat_r]);
			//fThreat = Mathf.Max(fThreat * ((bHit) ? 1 : 0.1), 1);
			//return fThreat;
			return 0;
		}

	}

	public class GSkillEffect_Damage : GSkillEffect
	{
		public override bool Process(GSkillCalculation sCalculation, GTargetInfo sTarInfo, SkillAValueData sSkillAValue)
		{
			Avatar pCaster = sCalculation.m_pCaster;
			Avatar pTarget = sCalculation.m_pTarget;
			DRSkillData pSkillData = sCalculation.m_pSkillData;
			if(pSkillData == null || !pCaster || !pTarget)
				return false;

			if(pTarget.IsDead)
				return false;
			SkillEffectEvent gameEvent = ReferencePool.Acquire<SkillEffectEvent>();
			gameEvent.SkillID = pSkillData.Id;
			gameEvent.CasterID = pCaster.Id;
			gameEvent.TargetID = pTarget.Id;
			gameEvent.NotifyType = (int)eTriggerNotifyType.NotifyType_Damage;
			gameEvent.EffectType = pSkillData.MSV_EffectType;
			gameEvent.EffectValue = 0;
			sCalculation.CalculationDamage(ref gameEvent.EffectType,ref gameEvent.EffectValue);
			
			//////////////////////////////////////////////////////////////////////////
			//产生效果事件 todo 技能组件添加完后修复
			//if(pCaster->GetSkillComponent())
			//{
			//	pCaster->GetSkillComponent()->PushTriggerNotify(pSkillData->m_nDataID, pTarget->GetAvatarID(), NotifyType_Damage, pkt.effectType, pkt.effectValue, &sTarInfo.m_vSrcPos, &sTarInfo.m_vTarPos, &sTarInfo.m_vAimDir);
			//}

			//if(pTarget->GetSkillComponent())
			//{
			//	pTarget->GetSkillComponent()->PushTriggerNotify(pSkillData->m_nDataID, pCaster->GetAvatarID(), NotifyType_OnDamage, pkt.effectType, pkt.effectValue, &sTarInfo.m_vSrcPos, &sTarInfo.m_vTarPos, &sTarInfo.m_vAimDir);
			//}
			//产生效果事件
			//////////////////////////////////////////////////////////////////////////

			bool bHit = ((gameEvent.EffectType & (int)eTriggerNotify.TriggerNotify_Hit) > 0);
			//产生仇恨
			ProcessHurtThreat(sCalculation.m_CasterAValue, pCaster, pTarget, bHit);

			GameEntry.Event.Fire(this, gameEvent);
			return bHit;
		}
	}

	public class GSkillEffect_Heal : GSkillEffect
	{
		public override bool Process(GSkillCalculation sCalculation, GTargetInfo sTarInfo, SkillAValueData sSkillAValue)
		{
			Avatar pCaster = sCalculation.m_pCaster;
			Avatar pTarget = sCalculation.m_pTarget;
			DRSkillData pSkillData = sCalculation.m_pSkillData;
			if(pSkillData == null || !pCaster || !pTarget)
				return false;

			if(pTarget.IsDead)
				return false;
			SkillEffectEvent gameEvent = ReferencePool.Acquire<SkillEffectEvent>();
			gameEvent.SkillID = pSkillData.Id;
			gameEvent.CasterID = pCaster.Id;
			gameEvent.TargetID = pTarget.Id;
			gameEvent.NotifyType = (int)eTriggerNotifyType.NotifyType_Heal;
			gameEvent.EffectType = pSkillData.MSV_EffectType;
			gameEvent.EffectValue = 0;
			sCalculation.CalculationHeal(ref gameEvent.EffectType, ref gameEvent.EffectValue);

			//////////////////////////////////////////////////////////////////////////
			////产生效果事件 todo 技能组件添加完后修复
			//if(pCaster->GetSkillComponent())
			//{
			//	pCaster->GetSkillComponent()->PushTriggerNotify(pSkillData->m_nDataID, pTarget->GetAvatarID(), NotifyType_Heal, pkt.effectType, pkt.effectValue, &sTarInfo.m_vSrcPos, &sTarInfo.m_vTarPos, &sTarInfo.m_vAimDir);
			//}
			//if(pTarget->GetSkillComponent())
			//{
			//	pTarget->GetSkillComponent()->PushTriggerNotify(pSkillData->m_nDataID, pCaster->GetAvatarID(), NotifyType_OnHeal, pkt.effectType, pkt.effectValue, &sTarInfo.m_vSrcPos, &sTarInfo.m_vTarPos, &sTarInfo.m_vAimDir);
			//}
			////产生效果事件
			//////////////////////////////////////////////////////////////////////////

			//产生仇恨
			ProcessHurtThreat(sCalculation.m_CasterAValue, pCaster, pTarget, true);

			GameEntry.Event.Fire(this, gameEvent);
			return true;
		}
	}
}

//    bool GSkillEffect_AddBuff::Process(GSkillEffectCalculation & sCalculation, GSkillTargetInfo & sTarInfo, RoleAValue & sSkillAValue)

//    {
//        GNodeAvatar* pCaster = sCalculation.m_pCaster;
//        GNodeAvatar* pTarget = sCalculation.m_pTarget;
//        GSkillData* pSkillData = sCalculation.m_pSkillData;
//        if (!pSkillData || !pCaster || !pTarget)
//            return false;

//        GPacketEffect pkt;
//        pkt.skillID = pSkillData->m_nDataID;
//        pkt.casterID = pCaster->GetAvatarID();
//        pkt.targetID = pTarget->GetAvatarID();
//        pkt.notifyType = NotifyType_Buff;
//        pkt.effectType = pSkillData->GetIntValue(MSV_EffectType);
//        pkt.effectValue = 0;

//        sCalculation.DecisionOnHit(pkt.effectType);
//        bool bHit = ((pkt.effectType & TriggerNotify_Hit) > 0);
//        if (bHit)
//        {
//            GBuffCreateArg arg;
//            arg.m_nBuffID = pSkillData->GetIntValue(MSV_EffectParam1);
//            arg.m_nBuffLevel = pSkillData->GetIntValue(MSV_EffectParam2);
//            arg.m_nBuffLayer = pSkillData->GetIntValue(MSV_EffectParam3);
//            arg.m_fUserData = sCalculation.GetPower();
//            pTarget->AddBuff(pCaster, &arg);
//        }
//        pCaster->BroadcastPacket(&pkt);

//        int32 nEffectType = pSkillData->GetIntValue(MSV_EffectType);
//        if ((nEffectType & TriggerNotify_Buff) > 0)
//        {
//            //产生仇恨
//            ProcessHealThreat(sCalculation.m_CasterAValue, pCaster, pTarget);
//        }
//        else if ((nEffectType & TriggerNotify_Debuff) > 0)
//        {
//            //产生仇恨
//            ProcessHurtThreat(sCalculation.m_CasterAValue, pCaster, pTarget, bHit);
//        }

//        return bHit;
//    }

//    bool GSkillEffect_RemoveBuff::Process(GSkillEffectCalculation & sCalculation, GSkillTargetInfo & sTarInfo, RoleAValue & sSkillAValue)

//    {
//        GNodeAvatar* pCaster = sCalculation.m_pCaster;
//        GNodeAvatar* pTarget = sCalculation.m_pTarget;
//        GSkillData* pSkillData = sCalculation.m_pSkillData;
//        if (!pSkillData || !pCaster || !pTarget)
//            return false;

//        if (pTarget->GetBuffComponent())
//        {
//            int32 nBuffID = pSkillData->GetIntValue(MSV_EffectParam1);
//            int32 nBuffLayer = pSkillData->GetIntValue(MSV_EffectParam2);
//            int32 nOwnerType = pSkillData->GetIntValue(MSV_EffectParam3);

//            int64 nCasterDID = -1;
//            if (nOwnerType == 1)
//            {
//                nCasterDID = pCaster->GetAvatarDID();
//            }
//            else if (nOwnerType == 2)
//            {
//                nCasterDID = pTarget->GetAvatarDID();
//            }

//            if (nBuffLayer > 0)
//            {
//                pTarget->GetBuffComponent()->RemoveBuffByLayerCnt(nBuffID, nCasterDID, nBuffLayer);
//            }
//            else
//            {
//                pTarget->GetBuffComponent()->RemoveBuff(nBuffID, nCasterDID);
//            }
//        }

//        return true;
//    }

//    bool GSkillEffect_Dispel::Process(GSkillEffectCalculation & sCalculation, GSkillTargetInfo & sTarInfo, RoleAValue & sSkillAValue)

//    {
//        GNodeAvatar* pCaster = sCalculation.m_pCaster;
//        GNodeAvatar* pTarget = sCalculation.m_pTarget;
//        GSkillData* pSkillData = sCalculation.m_pSkillData;
//        if (!pSkillData || !pCaster || !pTarget)
//            return false;

//        GPacketEffect pkt;
//        pkt.skillID = pSkillData->m_nDataID;
//        pkt.casterID = pCaster->GetAvatarID();
//        pkt.targetID = pTarget->GetAvatarID();
//        pkt.notifyType = NotifyType_Dispel;
//        pkt.effectType = pSkillData->GetIntValue(MSV_EffectType);
//        pkt.effectValue = 0;

//        sCalculation.DecisionOnHit(pkt.effectType);
//        bool bHit = ((pkt.effectType & TriggerNotify_Hit) > 0);
//        int32 nDispelCount = 0;
//        if (bHit)
//        {
//            int32 nBuffCount = pSkillData->GetIntValue(MSV_EffectParam1);
//            if ((pkt.effectType & TriggerNotify_DispelBuff) > 0)
//            {
//                nDispelCount = pTarget->DispelBuff(pCaster, BTF_PlusBuff, nBuffCount);
//            }
//            else if ((pkt.effectType & TriggerNotify_DispelDebuff) > 0)
//            {
//                nDispelCount = pTarget->DispelBuff(pCaster, BTF_DeBuff, nBuffCount);
//            }
//        }
//        pCaster->BroadcastPacket(&pkt);

//        if (pCaster->GetSkillComponent())
//        {
//            //Buff添加事件
//            if (pCaster && pCaster->GetSkillComponent())
//            {
//                pCaster->GetSkillComponent()->PushTriggerNotify(pSkillData->m_nDataID, pCaster->GetAvatarID(), NotifyType_Dispel, pkt.effectType, nDispelCount, &sTarInfo.m_vSrcPos, &sTarInfo.m_vTarPos, &sTarInfo.m_vAimDir);
//            }
//        }

//        int32 nEffectType = pSkillData->GetIntValue(MSV_EffectType);
//        if ((nEffectType & TriggerNotify_DispelBuff) > 0)
//        {
//            //产生仇恨
//            ProcessHurtThreat(sCalculation.m_CasterAValue, pCaster, pTarget, bHit);
//        }
//        else if ((nEffectType & TriggerNotify_DispelDebuff) > 0)
//        {
//            //产生仇恨
//            ProcessHealThreat(sCalculation.m_CasterAValue, pCaster, pTarget);
//        }

//        return bHit;
//    }

//    bool GSkillEffect_Repel::Process(GSkillEffectCalculation & sCalculation, GSkillTargetInfo & sTarInfo, RoleAValue & sSkillAValue)
//    {
//        GNodeAvatar* pCaster = sCalculation.m_pCaster;
//        GNodeAvatar* pTarget = sCalculation.m_pTarget;
//        GSkillData* pSkillData = sCalculation.m_pSkillData;
//        if (!pSkillData || !pCaster || !pTarget)
//            return false;

//        if (pTarget->IsDead())
//            return false;

//        //产生效果事件
//        int32 mEffectType = pSkillData->GetIntValue(MSV_EffectType);
//        sCalculation.DecisionOnHit(mEffectType);

//        bool bHit = ((mEffectType & TriggerNotify_Hit) > 0);
//        SetStrongControlled(pSkillData, sTarInfo, pCaster, pTarget);
//        return bHit;
//    }

//    bool GSkillEffect_CDReduce::Process(GSkillEffectCalculation & sCalculation, GSkillTargetInfo & sTarInfo, RoleAValue & sSkillAValue)

//    {
//        GNodeAvatar* pCaster = sCalculation.m_pCaster;
//        GNodeAvatar* pTarget = sCalculation.m_pTarget;
//        GSkillData* pSkillData = sCalculation.m_pSkillData;
//        if (!pSkillData || !pCaster || !pTarget)
//            return false;

//        if (pTarget->IsDead())
//            return false;

//        int32 nCDGroup = pSkillData->GetIntValue(MSV_EffectParam1);
//        int32 nCDTime = pSkillData->GetIntValue(MSV_EffectParam2);
//        int32 nCDType = pSkillData->GetIntValue(MSV_EffectParam3);

//        //产生效果事件
//        if (pCaster->GetCDComponent())
//        {
//            if (nCDType > 0)
//                pCaster->GetCDComponent()->AddCD(nCDGroup, nCDTime);
//            else
//                pCaster->GetCDComponent()->ReduceCD(nCDGroup, nCDTime);
//        }
//        return true;
//    }