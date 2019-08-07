using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
	/// <summary>
	/// 技能逻辑管理器
	/// </summary>
	public class GSkillLogicManager : Singleton<GSkillLogicManager>
	{
		public GSkillLauncher[] m_vLauncherLogic;
		public GSkillAreaLogic[] m_vAreaLogic;
		public GSkillEffect[] m_vEffectLogic;

		public GSkillLogicManager()
		{
			///////////////////////////////////////////////////////
			//技能发动逻辑
			m_vLauncherLogic = new GSkillLauncher[(int)eSkillLauncherType.SkillLauncher_Size];
			m_vLauncherLogic[(int)eSkillLauncherType.SkillLauncher_Direct] = new GSkillLauncher_Direct();
			m_vLauncherLogic[(int)eSkillLauncherType.SkillLauncher_Bullet] = new GSkillLauncher_Bullet();

			///////////////////////////////////////////////////////
			//技能范围逻辑
			m_vAreaLogic = new GSkillAreaLogic[(int)eSkillAreaLogic.SkillArea_Max];
			m_vAreaLogic[(int)eSkillAreaLogic.SkillArea_Singleton] = new GSkillAreaSingelton();
			m_vAreaLogic[(int)eSkillAreaLogic.SkillArea_Sector] = new GSkillAreaSector();
			m_vAreaLogic[(int)eSkillAreaLogic.SkillArea_Sphere] = new GSkillAreaSphere();
			m_vAreaLogic[(int)eSkillAreaLogic.SkillArea_Rect] = new GSkillAreaRect();
			m_vAreaLogic[(int)eSkillAreaLogic.SkillArea_Ring] = new GSkillAreaRing();

			///////////////////////////////////////////////////////
			//技能效果逻辑
			m_vEffectLogic = new GSkillEffect[(int)eSkillEffectLogic.SkillEffect_Size];
			m_vEffectLogic[(int)eSkillEffectLogic.SkillEffect_Damage] = new GSkillEffect_Damage();
			m_vEffectLogic[(int)eSkillEffectLogic.SkillEffect_Heal] = new GSkillEffect_Heal();
		}
		
		public GSkillLauncher GetLauncherLogic(int nLogicType)
		{
			if(nLogicType >= (int)eSkillLauncherType.SkillLauncher_Size
				|| nLogicType <= 0)
				return null;
			return m_vLauncherLogic[nLogicType];
		}

		public GSkillAreaLogic GetAreaLogic(int nLogicType)
		{
			if(nLogicType >= (int)eSkillAreaLogic.SkillArea_Max
				|| nLogicType <= 0)
				return null;
			return m_vAreaLogic[nLogicType];
		}

		public GSkillEffect GetEffectLogic(int nLogicType)
		{
			if(nLogicType >= (int)eSkillEffectLogic.SkillEffect_Size
				|| nLogicType <= 0)
				return null;
			return m_vEffectLogic[nLogicType];
		}

		public GSkillSpellLogic CreateSpellLogic(int nLogicID)
		{
			switch(nLogicID)
			{
				case (int)eSkillSpellLogic.SkillSpell_Branch:
					return ReferencePool.Acquire<GSkillSpellLogic_Branch>();
				case (int)eSkillSpellLogic.SkillSpell_Channel:
					return ReferencePool.Acquire<GSkillSpellLogic_Channel>();
				case (int)eSkillSpellLogic.SkillSpell_Eot:
					return ReferencePool.Acquire<GSkillSpellLogic_Eot>();
				case (int)eSkillSpellLogic.SkillSpell_AValue:
					return ReferencePool.Acquire<GSkillSpellLogic_AValue>();
				case (int)eSkillSpellLogic.SkillSpell_Trigger:
					return ReferencePool.Acquire<GSkillSpellLogic_Trigger>();
			}
			return null;
		}

		//GSkillProjectile CreateProjectile(int nType)
		//{
		//	switch(nType)
		//	{
		//		case (int)eProjectileType.Projectile_Track:
		//			return FACTORY_NEWOBJ(GSkillProjectile_Track);
		//		case (int)eProjectileType.Projectile_Trap:
		//			return FACTORY_NEWOBJ(GSkillProjectile_Trap);
		//	}
		//	return null;
		//}

	}
}

////检查目标
//bool GSkillLogicManager::CheckTarget(GSkillData* pSkillData, GNodeAvatar* pCaster, GSkillTargetInfo& sTarInfo)
//{
//    if (!pSkillData || !pCaster)
//        return false;

//    //检查目标条件
//    if (pSkillData->IsTargetAvatar())
//    {
//        if (!CheckTarget(pSkillData, pCaster, sTarInfo.m_nTargetID))
//            return false;
//    }
//    else if (pSkillData->IsTargetPos())
//    {
//        f32 fRange = pSkillData->GetFloatValue(MSV_Range);
//        if (fRange > 0)
//        {
//            f32 fDistance = pCaster->GetPos().GetDistance(sTarInfo.m_vTarPos);
//            if (fDistance > fRange + OffestRange)
//                return false;
//        }
//    }

//    return true;
//}

//bool GSkillLogicManager::CheckTarget(GSkillData* pSkillData, GNodeAvatar* pCaster, int32 nTargetID)
//{
//    GNodeAvatar* pTarget = pCaster->GetSceneAvatar(nTargetID);
//    if (!pTarget)
//        return false;

//    //当前目标类型隐含的条件检查
//    if (pSkillData->IsTargetOhterFriend())
//    {
//        if (!pCaster->CheckRelation(pTarget, ToFriend))
//            return false;
//    }
//    if (pSkillData->IsTargetOhterEnemy())
//    {
//        if (!pCaster->CheckRelation(pTarget, ToEnemy))
//            return false;
//    }

//    int32 nTarCheck = pSkillData->GetIntValue(MSV_TarCheck);
//    if (nTarCheck > 0)
//    {
//        SDConditionParamAvatar sParam;
//        sParam.ParamAvatar = pCaster;
//        if (!GSKillConditionCheckManager::Instance().Check(nTarCheck, pTarget, &sParam))
//            return false;
//    }

//    f32 fRange = pSkillData->GetFloatValue(MSV_Range);
//    if (fRange > 0)
//    {
//        f32 fDistance = pCaster->GetPos().GetDistance(pTarget->GetPos());
//        if (fDistance > fRange + OffestRange)
//            return false;
//    }

//    return true;
//}