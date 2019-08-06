using System;
using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;
using StateOwner = GameFramework.Fsm.IFsm<Galaxy.Avatar>;
using UnityGameFramework.Runtime;

namespace Galaxy
{
	public abstract class StateParam : Variable<StateParam> ,IReference
	{
		public abstract StateDefine m_state { get; }

		public abstract void Clear();
	}
	public abstract class StateBase : FsmState<Avatar>
	{
		public abstract StateDefine m_state { get; }

		/// <summary>
		/// 状态初始化时调用。
		/// </summary>
		/// <param name="pAvatar">状态拥有者。</param>
		protected override void OnInit(StateOwner pAvatar)
		{
			base.OnInit(pAvatar);
			SubscribeEvent((int)eFsmEvent.ChangeState, DefaultChangeState);
			SubscribeMyEvent();
		}
		protected abstract void SubscribeMyEvent();

		protected void DefaultChangeState(IFsm<Avatar> fsm, object sender, object userData)
		{
			StateParam param = userData as StateParam;
			if(param == null)
				return;

			if(!CanChangeState(fsm, param.m_state))
				return;

			ChangeState(fsm, param.m_state);
			fsm.Owner.ChangeStateSucc();
		}
		protected abstract bool CanChangeState(IFsm<Avatar> fsm, StateDefine nextState);

		/// <summary>
		/// 进入状态时调用。
		/// </summary>
		/// <param name="pAvatar">状态拥有者。</param>
		protected override void OnEnter(StateOwner pAvatar)
		{
			base.OnEnter(pAvatar);
			StateParam nextParam = pAvatar.Owner.GetNextStateData();
			OnEnterState(nextParam);
		}
		protected abstract void OnEnterState(StateParam nextParam);

		/// <summary>
		/// 状态轮询时调用。
		/// </summary>
		/// <param name="pAvatar">状态拥有者。</param>
		/// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
		/// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
		protected override void OnUpdate(StateOwner pAvatar, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(pAvatar, elapseSeconds, realElapseSeconds);
		}

		/// <summary>
		/// 离开状态时调用。
		/// </summary>
		/// <param name="pAvatar">状态拥有者。</param>
		/// <param name="isShutdown">是否是关闭状态机时触发。</param>
		protected override void OnLeave(StateOwner pAvatar, bool isShutdown)
		{
			base.OnLeave(pAvatar, isShutdown);
		}

		/// <summary>
		/// 状态销毁时调用。
		/// </summary>
		/// <param name="pAvatar">状态拥有者。</param>
		protected override void OnDestroy(StateOwner pAvatar)
		{
			base.OnDestroy(pAvatar);
			UnsubscribeEvent((int)eFsmEvent.ChangeState, DefaultChangeState);
			UnsubscribeMyEvent();
		}
		protected abstract void UnsubscribeMyEvent();
		
		//temp 可以优化，重新架构状态机不使用框架状态机
		protected void ChangeState(IFsm<Avatar> fsm, StateDefine eState)
		{
			switch(eState)
			{
				case StateDefine.State_Idle:
					ChangeState<StateIdle>(fsm);
					break;
				case StateDefine.State_Move:
					ChangeState<StateMove>(fsm);
					break;
				case StateDefine.State_Skill:
					ChangeState<StateSkill>(fsm);
					break;
				case StateDefine.State_WeaklyControl:
					ChangeState<StateWeaklyControl>(fsm);
					break;
				case StateDefine.State_StronglyControl:
					ChangeState<StateStronglyControl>(fsm);
					break;
				case StateDefine.State_Death:
					ChangeState<StateDeath>(fsm);
					break;
				default:
					Log.Fatal("State machine change fatal!!! '{0}'", eState.ToString());
					break;
			}
		}
	}
}