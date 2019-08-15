using GameFramework;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
using UnityEngine;

namespace Galaxy
{
	public class StateMoveParam : StateParam
	{
		public override StateDefine m_state
		{
			get
			{
				return StateDefine.State_Move;
			}
		}
		
		public StateMoveParam Fill(Vector3 vGoalPos)
		{
			m_vGoalPos = vGoalPos;
			return this;
		}

		public override void Clear()
		{
			m_vGoalPos = default(Vector3);
		}

		public Vector3 m_vGoalPos;
	}

	public class StateMove : StateBase
	{
		private const float speed = 4f;
		private Vector3 m_vGoalPos;

		public override StateDefine m_state
		{
			get
			{
				return StateDefine.State_Move;
			}
		}

		protected override bool CanChangeState(IFsm<Avatar> fsm, StateDefine nextState)
		{
			if(nextState == StateDefine.State_None
				|| nextState == StateDefine.State_Idle)
				return false;
			return true;
		}

		protected override void OnEnterState(IFsm<Avatar> pAvatar, StateParam nextParam)
		{
			StateMoveParam param = nextParam as StateMoveParam;
			if(param == null)
			{
				Log.Error("Current State '{0}': the Variable's(the initParam) type isn't right! '{1}'", typeof(StateIdle), typeof(Variable));
				return;
			}

			m_vGoalPos = param.m_vGoalPos;
		}

		protected override void OnEnter(IFsm<Avatar> pAvatar)
		{
			base.OnEnter(pAvatar);
			pAvatar.Owner.PlayAnimation((int)CommonAnimation.Walk);
		}

		protected override void OnUpdate(IFsm<Avatar> pAvatar, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(pAvatar, elapseSeconds, realElapseSeconds);

			if(m_vGoalPos != Vector3.zero)
			{
				if(pAvatar == null || pAvatar.Owner == null)
				{
					ChangeState<StateIdle>(pAvatar);
					return;
				}

				if(pAvatar.Owner.GetPos().Distance2D(m_vGoalPos) <= 0.2f)
				{
					pAvatar.Owner.StopMovement();
					ChangeState<StateIdle>(pAvatar);
					return;
				}

				pAvatar.Owner.CachedTransform.LookAt(new Vector3(m_vGoalPos.x, pAvatar.Owner.GetPos().y, m_vGoalPos.z));
				Vector3 vMotion = pAvatar.Owner.GetDir() * speed;
				pAvatar.Owner.MoveDistance(vMotion * Time.deltaTime, true);
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