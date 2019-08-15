using GameFramework;
using System;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;

namespace Galaxy
{
	public class StateSkillParam : StateParam
	{
		public override StateDefine m_state
		{
			get
			{
				return StateDefine.State_Skill;
			}
		}

		public int m_nSkillID;
		public float m_fTotalTime;
		public float m_fBreakTime;

		public override void Clear()
		{

		}
	}

	public class StateSkill : StateBase
	{
		public int m_nSkillID;
		public float m_fCurTime;
		public float m_fTotalTime;
		//根据此时间决定能否移动
		public float m_fBreakTime;

		public override StateDefine m_state
		{
			get
			{
				return StateDefine.State_Skill;
			}
		}

		protected override bool CanChangeState(IFsm<Avatar> fsm, StateDefine nextState)
		{
			if(nextState == StateDefine.State_None
				|| nextState == StateDefine.State_Idle
				|| nextState == StateDefine.State_Move
				|| fsm.Owner.CheckState(StateDefine.State_LockActiveSkill))
				return false;
			return true;
		}

		protected override void OnEnterState(IFsm<Avatar> pAvatar, StateParam nextParam)
		{
			StateSkillParam param = nextParam as StateSkillParam;
			if(param == null)
			{
				Log.Error("Current State '{0}': the Variable's(the initParam) type isn't right! '{1}'", typeof(StateIdle), typeof(Variable));
				return;
			}
			m_nSkillID = param.m_nSkillID;
			m_fCurTime = 0;
			m_fTotalTime = param.m_fTotalTime;
			m_fBreakTime = param.m_fBreakTime;

			pAvatar.Owner.SetState(StateDefine.State_LockActiveSkill);
			pAvatar.Owner.SetState(StateDefine.State_LockMove);

			DRSkillData pSkillData = GameEntry.DataTable.GetDataTable<DRSkillData>().GetDataRow(m_nSkillID);
			if(pSkillData == null)
			{
				Log.Error("pSkillData '{0}': pSkillData is null!", m_nSkillID);
				return;
			}
			pAvatar.Owner.PlayAnimation(pSkillData.MSV_AnimID);
		}

		protected override void OnUpdate(IFsm<Avatar> pAvatar, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(pAvatar, elapseSeconds, realElapseSeconds);
			m_fCurTime += elapseSeconds;
			if(m_fCurTime > m_fBreakTime)
			{
				pAvatar.Owner.ResetState(StateDefine.State_LockActiveSkill);
				pAvatar.Owner.ResetState(StateDefine.State_LockMove);
			}

			if(m_fCurTime > m_fTotalTime)
			{
				ChangeState(pAvatar, StateDefine.State_Idle);
				return;
			}
		}

		protected override void OnLeave(IFsm<Avatar> pAvatar, bool isShutdown)
		{
			base.OnLeave(pAvatar, isShutdown);
			pAvatar.Owner.SkillCom.FinishSkill();
		}

		protected override void SubscribeMyEvent()
		{

		}

		protected override void UnsubscribeMyEvent()
		{

		}
	}

}