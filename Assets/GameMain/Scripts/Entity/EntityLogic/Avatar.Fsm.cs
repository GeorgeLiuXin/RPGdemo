using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
	public partial class Avatar : Entity
	{
		public IFsm<Avatar> m_fsm
		{
			get;
			private set;
		}

		//temp 需要优化
		private bool m_changeFlag;
		public void ChangeStateSucc()
		{
			m_changeFlag = true;
		}

		//temp 需要优化 可以通过外层包裹一个类封装住IFsm<Avatar>
		private static string fsmStateFlag = "StateFlag";
		private static string fsmNextData = "NextData";

		private void InitFsm()
		{
			m_fsm = GameEntry.Fsm.CreateFsm(this, GameEntry.fsmMgr.GetAllFsmState<Avatar>());
			m_fsm.Start<StateIdle>();
			m_fsm.SetData(fsmStateFlag, new StateFlag());
			m_fsm.SetData(fsmNextData, null);
			m_changeFlag = false;
		}

		public bool SetFsmState(object sender, StateParam stateData)
		{
			if(m_fsm == null)
				return false;
			m_changeFlag = false;
			m_fsm.SetData(fsmNextData, stateData);
			m_fsm.FireEvent(sender, (int)eFsmEvent.ChangeState, stateData);
			m_fsm.SetData(fsmNextData, null);
			return m_changeFlag;
		}
		
		public StateParam GetNextStateData()
		{
			StateParam param = m_fsm.GetData(fsmNextData) as StateParam;
			return param;
		}

		public bool CheckState(StateDefine eState)
		{
			StateFlag flag = m_fsm.GetData(fsmStateFlag) as StateFlag;
			if(flag == null)
				return false;
			return flag.CheckState(eState);
		}
		public void SetState(StateDefine eState)
		{
			StateFlag flag = m_fsm.GetData(fsmStateFlag) as StateFlag;
			if(flag == null)
				return;
			flag.SetState(eState);
		}
		public void ResetState(StateDefine eState)
		{
			StateFlag flag = m_fsm.GetData(fsmStateFlag) as StateFlag;
			if(flag == null)
				return;
			flag.ResetState(eState);
		}
	}
}