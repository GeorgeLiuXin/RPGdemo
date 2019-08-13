using GameFramework;
using GameFramework.Fsm;

namespace Galaxy
{
    /// <summary>
    /// 暂时仅仅使用非常简单的状态机流转 之后改成行为树控制AI行为
    /// </summary>
	public partial class AIComponent : ComponentBase
    {
        public IFsm<Monster> m_aifsm
        {
            get;
            private set;
        }

        protected override void InitComponent()
        {
            Monster pMonster = Owner as Monster;
            if (pMonster == null)
                return;

            m_aifsm = GameEntry.Fsm.CreateFsm(Utility.Text.GetFullName(GetType(), Owner.Id.ToString())
                , pMonster, GameEntry.fsmMgr.GetAllFsmState<Monster>());
            m_aifsm.Start<StateIdle>();
        }

        public override void OnPreDestroy()
        {

        }

        public void Update()
        {

        }

    }
}