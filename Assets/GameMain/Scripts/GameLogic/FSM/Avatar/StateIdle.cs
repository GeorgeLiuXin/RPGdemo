using GameFramework;
using System;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;

namespace Galaxy
{
	public class StateIdleParam : StateParam
	{
		public override StateDefine m_state
		{
			get
			{
				return StateDefine.State_Idle;
			}
		}

		public override void Clear()
		{

		}
	}

	public class StateIdle : StateBase
	{
		public override StateDefine m_state
		{
			get
			{
				return StateDefine.State_Idle;
			}
		}

		protected override bool CanChangeState(IFsm<Avatar> fsm, StateDefine nextState)
		{
			if(nextState == StateDefine.State_None)
				return false;
			return true;
		}

		protected override void OnEnterState(IFsm<Avatar> pAvatar, StateParam nextParam)
		{
			StateIdleParam param = nextParam as StateIdleParam;
			if(param == null)
			{
				Log.Error("Current State '{0}': the Variable's(the initParam) type isn't right! '{1}'", typeof(StateIdle).ToString(), typeof(Variable).ToString());
				return;
			}
		}

		protected override void OnEnter(IFsm<Avatar> pAvatar)
		{
			base.OnEnter(pAvatar);
			pAvatar.Owner.PlayAnimation((int)CommonAnimation.Idle);
		}

		protected override void SubscribeMyEvent()
		{
		}

		protected override void UnsubscribeMyEvent()
		{
		}
	}

}