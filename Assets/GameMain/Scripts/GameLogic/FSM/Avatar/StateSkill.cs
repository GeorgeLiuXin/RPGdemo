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

		public float m_fTotalTime;
		public float m_fBreakTime;

		public override void Clear()
		{

		}
	}

	public class StateSkill : StateBase
	{
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
				|| nextState == StateDefine.State_Move)
				return false;
			return true;
		}

		protected override void OnEnterState(StateParam nextParam)
		{
			StateSkillParam param = nextParam as StateSkillParam;
			if(param == null)
			{
				Log.Error("Current State '{0}': the Variable's(the initParam) type isn't right! '{1}'", typeof(StateIdle), typeof(Variable));
				return;
			}
			m_fCurTime = 0;
			m_fTotalTime = param.m_fTotalTime;
			m_fBreakTime = param.m_fBreakTime;
		}

		protected override void OnUpdate(IFsm<Avatar> pAvatar, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(pAvatar, elapseSeconds, realElapseSeconds);
			m_fCurTime += elapseSeconds;
			if(m_fCurTime > m_fTotalTime)
			{
				ChangeState(pAvatar, StateDefine.State_Idle);
				return;
			}
		}

		protected override void OnLeave(IFsm<Avatar> pAvatar, bool isShutdown)
		{
			base.OnLeave(pAvatar, isShutdown);
			//todo 结束技能逻辑
			//pAvatar.Owner
		}

		protected override void SubscribeMyEvent()
		{

		}

		protected override void UnsubscribeMyEvent()
		{

		}
	}

}