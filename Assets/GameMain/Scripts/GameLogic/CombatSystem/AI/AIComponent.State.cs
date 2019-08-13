using GameFramework;
using GameFramework.Fsm;
using StateOwner = GameFramework.Fsm.IFsm<Galaxy.Monster>;

namespace Galaxy
{
    public partial class AIComponent : ComponentBase
    {
        public abstract class AIStateBase : FsmState<Monster>
        {
            public abstract StateDefine m_state { get; }

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
                pAvatar.Owner.ResetState(m_state);
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
            private StateDefine m_State;

            public override StateDefine m_state
            {
                get { return m_State; }
            }
        }
    }
}