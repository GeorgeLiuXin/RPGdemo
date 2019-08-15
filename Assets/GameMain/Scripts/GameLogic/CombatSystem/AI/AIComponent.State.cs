using GameFramework.Fsm;
using UnityEngine;
using Random = UnityEngine.Random;
using StateOwner = GameFramework.Fsm.IFsm<Galaxy.Monster>;

namespace Galaxy
{
    public partial class AIComponent : ComponentBase
    {
        public enum AIStateDefine
        {
            State_None,
            State_Idle,
            State_HangOut,
            State_Chase,
            State_Combat,
            State_Size
        }

        public abstract class AIStateBase : FsmState<Monster>
        {
            public abstract AIStateDefine m_state { get; }

            /// <summary>
            /// 状态初始化时调用。
            /// </summary>
            /// <param name="pAvatar">状态拥有者。</param>
            protected override void OnInit(StateOwner pAvatar)
            {
                base.OnInit(pAvatar);
            }

            /// <summary>
            /// 进入状态时调用。
            /// </summary>
            /// <param name="pAvatar">状态拥有者。</param>
            protected override void OnEnter(StateOwner pAvatar)
            {
                base.OnEnter(pAvatar);
            }

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
            }
        }

        public class AIState_Idle : AIStateBase
        {
            private float m_fTimer;

            public override AIStateDefine m_state
            {
                get { return AIStateDefine.State_Idle; }
            }

            protected override void OnEnter(StateOwner pAvatar)
            {
                base.OnEnter(pAvatar);

                m_fTimer = Constant.AI.GetRandomIdleTime();
            }

            protected override void OnUpdate(StateOwner pAvatar, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(pAvatar, elapseSeconds, realElapseSeconds);

                if (pAvatar.Owner.IsFight)
                {
                    ChangeState<AIState_Chase>(pAvatar);
                    return;
                }

                m_fTimer -= elapseSeconds;
                if (m_fTimer <= 0)
                {
                    ChangeState<AIState_HangOut>(pAvatar);
                }
            }
        }

        public class AIState_HangOut : AIStateBase
        {
            private Vector3 m_vGoalPos;

            public override AIStateDefine m_state
            {
                get { return AIStateDefine.State_HangOut; }
            }

            protected override void OnEnter(StateOwner pAvatar)
            {
                base.OnEnter(pAvatar);

                m_vGoalPos = pAvatar.Owner.GetPos();
                m_vGoalPos.x += Random.Range(-1, 1) * Constant.AI.HangOutRange;
                m_vGoalPos.z += Random.Range(-1, 1) * Constant.AI.HangOutRange;
                pAvatar.Owner.MoveToPoint(m_vGoalPos);
            }

            protected override void OnUpdate(StateOwner pAvatar, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(pAvatar, elapseSeconds, realElapseSeconds);
                if (pAvatar == null || pAvatar.Owner == null)
                {
                    ChangeState<AIState_Idle>(pAvatar);
                    return;
                }

                if (pAvatar.Owner.IsFight)
                {
                    ChangeState<AIState_Chase>(pAvatar);
                    return;
                }

                if (m_vGoalPos != Vector3.zero && pAvatar.Owner.GetPos().Distance2D(m_vGoalPos) <= 0.2f)
                {
                    pAvatar.Owner.StopMovement();
                    ChangeState<AIState_Idle>(pAvatar);
                }
            }
        }

        public class AIState_Chase : AIStateBase
        {
            private Avatar m_Target;
            private float m_SkillRange;

            private float m_fTimer;
            private const float m_fChaseTickTime = 0.5f;

            public override AIStateDefine m_state
            {
                get { return AIStateDefine.State_Chase; }
            }

            protected override void OnEnter(StateOwner pAvatar)
            {
                base.OnEnter(pAvatar);

                if (pAvatar == null || pAvatar.Owner == null || pAvatar.Owner.AICom==null)
                {
                    ChangeState<AIState_Idle>(pAvatar);
                    return;
                }

                m_fTimer = m_fChaseTickTime;

                int nTarget = pAvatar.Owner.ThreatCom.GetTarget();
                m_Target = GameEntry.Entity.GetGameEntity(nTarget) as Avatar;
                m_SkillRange = pAvatar.Owner.AICom.GetNextSkillRange();
            }

            protected override void OnUpdate(StateOwner pAvatar, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(pAvatar, elapseSeconds, realElapseSeconds);

                int nTarget = pAvatar.Owner.ThreatCom.GetTarget();
                if (nTarget != m_Target.Id)
                {
                    m_Target = GameEntry.Entity.GetGameEntity(nTarget) as Avatar;
                }

                if (m_Target == null || !pAvatar.Owner.IsFight)
                {
                    ChangeState<AIState_Idle>(pAvatar);
                    return;
                }

                if (m_Target.GetPos().Distance2D(pAvatar.Owner.GetPos()) <= m_SkillRange)
                {
                    ChangeState<AIState_Combat>(pAvatar);
                }

                m_fTimer -= elapseSeconds;
                if (m_fTimer <= 0)
                {
                    m_fTimer = m_fChaseTickTime;
                    pAvatar.Owner.MoveToPoint(m_Target.GetPos());
                }
            }
        }

        public class AIState_Combat : AIStateBase
        {
            public override AIStateDefine m_state
            {
                get { return AIStateDefine.State_Combat; }
            }

            protected override void OnEnter(StateOwner pAvatar)
            {
                base.OnEnter(pAvatar);
                pAvatar.Owner.AICom.SpellSkill();
            }

            protected override void OnUpdate(StateOwner pAvatar, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(pAvatar, elapseSeconds, realElapseSeconds);

                if (!pAvatar.Owner.IsFight)
                {
                    ChangeState<AIState_Idle>(pAvatar);
                    return;
                }
                if (pAvatar.Owner.SkillCom.GetCurrentSkillID() != -1)
                    return;

                if (!pAvatar.Owner.AICom.CanSpellSkill())
                {
                    ChangeState<AIState_Chase>(pAvatar);
                    return;
                }
                pAvatar.Owner.AICom.SpellSkill();
            }
        }

    }
}