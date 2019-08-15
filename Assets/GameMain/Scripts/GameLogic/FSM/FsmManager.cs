using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;

namespace Galaxy
{
	public class FsmManager : GameFrameworkComponent
	{
		private Dictionary<Type, List<Type>> m_FsmDict;
		private List<Type> EntityFsmList;
		private List<Type> MonsterFsmList;

		void Start()
		{
			m_FsmDict = new Dictionary<Type, List<Type>>();
			AddAvatarFsm();
			AddMonsterFsm();
		}

		private void AddAvatarFsm()
		{
			EntityFsmList = new List<Type>();
			EntityFsmList.Add(typeof(StateIdle));
			EntityFsmList.Add(typeof(StateMove));
			EntityFsmList.Add(typeof(StateSkill));
			EntityFsmList.Add(typeof(StateWeaklyControl));
			EntityFsmList.Add(typeof(StateStronglyControl));
			EntityFsmList.Add(typeof(StateDeath));
			m_FsmDict.Add(typeof(Avatar), EntityFsmList);
		}

		private void AddMonsterFsm()
		{
			MonsterFsmList = new List<Type>();
			MonsterFsmList.Add(typeof(AIComponent.AIState_Idle));
			MonsterFsmList.Add(typeof(AIComponent.AIState_HangOut));
			MonsterFsmList.Add(typeof(AIComponent.AIState_Chase));
			MonsterFsmList.Add(typeof(AIComponent.AIState_Combat));
			m_FsmDict.Add(typeof(Monster), MonsterFsmList);
		}

		public FsmState<T>[] GetAllFsmState<T>() where T : Entity
		{
			if(!m_FsmDict.ContainsKey(typeof(T)))
				return null;
			List<Type> fsmList = m_FsmDict[typeof(T)];
			if(fsmList == null)
				return null;

			FsmState<T>[] list = new FsmState<T>[fsmList.Count];
			for(int i = 0; i < fsmList.Count; i++)
			{
				list[i] = (FsmState<T>)Activator.CreateInstance(fsmList[i]);
				if(list[i] == null)
				{
					Log.Error("Can not create FsmState instance '{0}'.", fsmList[i]);
					return null;
				}
			}
			return list;
		}
	}
}