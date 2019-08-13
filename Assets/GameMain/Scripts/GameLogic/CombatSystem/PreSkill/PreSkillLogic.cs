using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Galaxy
{
	public interface IPreSkillHelper
	{
		void SetOwner(Avatar pActor);
		bool Enter(DRSkillData pSkillData);
		void Update();
		void Reset();
		bool UseSkill();
	}

	public class PreSkillLogicBase : IPreSkillHelper
	{
		protected Player Owner;
		protected DRSkillData m_pSkillData;

		public virtual bool Enter(DRSkillData pSkillData)
		{
			m_pSkillData = pSkillData;
			return true;
		}

		public virtual void Update()
		{
			return;
		}

		public virtual void Reset()
		{
			return;
		}

		public void SetOwner(Avatar pActor)
		{
			Owner = pActor as Player;
			if(Owner == null)
			{
				Log.Error(Utility.Text.Format("技能前置瞄准初始化失败!"));
			}
		}

		protected GTargetInfo SetDefaultTargetInfo()
		{
			GTargetInfo sTarInfo = new GTargetInfo();
			sTarInfo.m_nTargetID = Owner.Id;
			sTarInfo.m_vSrcPos = Owner.GetPos();
			sTarInfo.m_vAimDir = Owner.GetDir();
			sTarInfo.m_vTarPos = Owner.GetPos();
			return sTarInfo;
		}

		public virtual bool UseSkill()
		{
			return false;
		}
	}

	public class PreSkillLogic_CurTarget : PreSkillLogicBase
	{
		public override bool Enter(DRSkillData pSkillData)
		{
			if(!base.Enter(pSkillData))
				return false;
			return true;
		}

		public override bool UseSkill()
		{
			GTargetInfo sTarInfo = SetDefaultTargetInfo();
			if(m_pSkillData == null || sTarInfo == null)
				return false;

			if(m_pSkillData.IsTargetSelfOnly())
			{
				sTarInfo.m_nTargetID = Owner.Id;
				sTarInfo.m_vSrcPos = Owner.GetPos();
				sTarInfo.m_vAimDir = Owner.GetDir();
				sTarInfo.m_vTarPos = Owner.GetPos();
			}
			else if(m_pSkillData.IsTargetAvatar() || m_pSkillData.IsTargetPos())
			{
				Avatar pTarget = Owner.AimCom.GetTarget();
				if(pTarget == null)
					return false;

				sTarInfo.m_nTargetID = pTarget.Id;
				sTarInfo.m_vSrcPos = Owner.GetPos();
				sTarInfo.m_vTarPos = pTarget.GetPos();
				sTarInfo.m_vAimDir = (pTarget.GetPos() - Owner.GetPos()).normalized2d();
			}
			else if(m_pSkillData.IsTargetDir())
			{
				return false;
			}
			return Owner.SkillCom.SpellSkill(m_pSkillData.Id, sTarInfo);
		}

		public override void Update()
		{

		}

		public override void Reset()
		{
			return;
		}
	}

	public class PreSkillLogic_CameraDir : PreSkillLogicBase
	{
		public override bool Enter(DRSkillData pSkillData)
		{
			if(!base.Enter(pSkillData))
				return false;
			return true;
		}

		public override void Reset()
		{

		}

		public override void Update()
		{
		}
	}

	public class PreSkillLogic_AimArea : PreSkillLogicBase
	{
		public override bool Enter(DRSkillData pSkillData)
		{
			if(!base.Enter(pSkillData))
				return false;
			return true;
		}

		public override void Reset()
		{

		}

		public override void Update()
		{
		}
	}

}