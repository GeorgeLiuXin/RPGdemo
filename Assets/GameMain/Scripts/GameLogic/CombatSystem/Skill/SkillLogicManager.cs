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

		public GSkillProjectile CreateProjectile(int nType)
		{
			switch(nType)
			{
				case (int)eProjectileType.Projectile_Track:
					return ReferencePool.Acquire<GSkillProjectile_Track>();
				case (int)eProjectileType.Projectile_Trap:
					return ReferencePool.Acquire<GSkillProjectile_Trap>();
			}
			return null;
		}

		//检查目标
		public bool CheckTarget(DRSkillData pSkillData, Avatar pCaster, GTargetInfo sTarInfo)
		{
			if(pSkillData == null || !pCaster)
				return false;

			//检查目标条件
			if(pSkillData.IsTargetAvatar())
			{
				if(!CheckTarget(pSkillData, pCaster, sTarInfo.m_nTargetID))
					return false;
			}
			else if(pSkillData.IsTargetPos())
			{
				float fRange = pSkillData.MSV_Range;
				if(fRange > 0)
				{
					float fDistance = pCaster.GetPos().Distance2D(sTarInfo.m_vTarPos);
					if(fDistance > fRange + 0.5f)
						return false;
				}
			}

			return true;
		}

		public bool CheckTarget(DRSkillData pSkillData, Avatar pCaster, int nTargetID)
		{
			Avatar pTarget = GameEntry.Entity.GetGameEntity(nTargetID) as Avatar;
			if(!pTarget)
				return false;
			
			int nTarCheck = pSkillData.MSV_TarCheck;
			if(nTarCheck > 0)
			{
				//todo 条件检查组
				//SDConditionParamAvatar sParam;
				//sParam.ParamAvatar = pCaster;
				//if(!GSKillConditionCheckManager::Instance().Check(nTarCheck, pTarget, &sParam))
				//	return false;
			}

			float fRange = pSkillData.MSV_Range;
			if(fRange > 0)
			{
				float fDistance = pCaster.GetPos().Distance2D(pTarget.GetPos());
				if(fDistance > fRange + 0.5f)
					return false;
			}

			return true;
		}
	}
}