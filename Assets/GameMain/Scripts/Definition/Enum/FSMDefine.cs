using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Galaxy
{
	public enum CommonAnimation
	{
		Idle = 1,
		Walk,
		Run,
		TakeDamage1,
		TakeDamage2,
		Death,
	}

	public enum StateDefine
	{
		State_None = 0,
		State_Idle = 1,
		State_Move = 2,
		State_Skill = 3,
		State_WeaklyControl = 4,
		State_StronglyControl = 5,
		State_Death = 6,

		State_ActiveState = 48,             //以上为主动状态

		State_LockActiveSkill = 49,
		State_LockMove = 50,

        State_LockState = 64,             //以上为锁定状态

        State_Fight = 65,

		State_Size = 127,
	}

	public class StateFlag : Variable
	{
		private BitArray m_Value;

		public StateFlag()
		{
			m_Value = new BitArray((int)StateDefine.State_Size);
		}

		public override Type Type
		{
			get
			{
				return typeof(long);
			}
		}

		public override object GetValue()
		{
			return m_Value;
		}

		public override void Reset()
		{
			m_Value.SetAll(false);
		}

		public override void SetValue(object value)
		{
			m_Value = value as BitArray;
		}

		public bool CheckState(StateDefine eState)
		{
			return m_Value.Get((int)eState);
		}
		public void SetState(StateDefine eState)
		{
			m_Value.Set((int)eState, true);
		}
		public void ResetState(StateDefine eState)
		{
			m_Value.Set((int)eState, false);
		}
	}
}