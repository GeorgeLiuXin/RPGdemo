using GameFramework;
using System;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;

namespace Galaxy
{
	public class StateWeaklyControlParam : StateParam
	{
		public override StateDefine m_state
		{
			get
			{
				return StateDefine.State_WeaklyControl;
			}
		}

		public override void Clear()
		{

		}
	}

	public class StateWeaklyControl : StateBase
	{
		public override StateDefine m_state
		{
			get
			{
				return StateDefine.State_WeaklyControl;
			}
		}

		protected override bool CanChangeState(IFsm<Avatar> fsm, StateDefine nextState)
		{
			if(nextState == StateDefine.State_None
				|| nextState == StateDefine.State_Idle
				|| nextState == StateDefine.State_Move
				|| nextState == StateDefine.State_Skill)
				return false;
			return true;
		}

		protected override void OnEnterState(StateParam nextParam)
		{
			StateWeaklyControlParam param = nextParam as StateWeaklyControlParam;
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