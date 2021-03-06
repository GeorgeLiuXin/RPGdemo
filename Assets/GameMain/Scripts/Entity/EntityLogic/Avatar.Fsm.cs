﻿using GameFramework;
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
		private static readonly string fsmNextData = "NextData";

		private StateFlag m_flag;

		private void InitFsm()
        {
            m_fsm = GameEntry.Fsm.CreateFsm(Utility.Text.GetFullName(GetType(), Id.ToString())
                , this, GameEntry.fsmMgr.GetAllFsmState<Avatar>());
            m_fsm.Start<StateIdle>();
            m_fsm.SetData(fsmNextData, null);
			m_flag = new StateFlag();
			m_changeFlag = false;
		}

		public bool SetFsmState(object sender, StateParam stateData)
		{
			if(m_fsm == null || m_fsm.CurrentState == null)
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
			if(m_flag == null)
				return false;
			return m_flag.CheckState(eState);
		}
		public void SetState(StateDefine eState)
		{
			if(m_flag == null)
				return;
			m_flag.SetState(eState);
		}
		public void ResetState(StateDefine eState)
		{
			if(m_flag == null)
				return;
			m_flag.ResetState(eState);
		}
	}
}