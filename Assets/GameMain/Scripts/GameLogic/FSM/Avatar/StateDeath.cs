using GameFramework;
using System;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;

namespace Galaxy
{
	public class StateDeathParam : StateParam
	{
		public override StateDefine m_state
		{
			get
			{
				return StateDefine.State_Death;
			}
		}

		public override void Clear()
		{

		}
	}

	public class StateDeath : StateBase
	{
		public override StateDefine m_state
		{
			get
			{
				return StateDefine.State_Death;
			}
		}

		protected override bool CanChangeState(IFsm<Avatar> fsm, StateDefine nextState)
		{
			if(nextState == StateDefine.State_None
				|| nextState == StateDefine.State_Idle
				|| nextState == StateDefine.State_Move
				|| nextState == StateDefine.State_Skill
				|| nextState == StateDefine.State_WeaklyControl
				|| nextState == StateDefine.State_StronglyControl)
				return false;
			return true;
		}

		protected override void OnEnterState(IFsm<Avatar> pAvatar, StateParam nextParam)
		{
			StateDeathParam param = nextParam as StateDeathParam;
			if(param == null)
			{
				Log.Error("Current State '{0}': the Variable's(the initParam) type isn't right! '{1}'", typeof(StateIdle), typeof(Variable));
				return;
			}
		}

		protected override void SubscribeMyEvent()
		{

		}

		protected override void UnsubscribeMyEvent()
		{

		}
	}
}