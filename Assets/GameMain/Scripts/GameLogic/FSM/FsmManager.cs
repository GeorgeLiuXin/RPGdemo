using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;

namespace Galaxy
{
	public class FsmManager : GameFrameworkComponent
	{
		private List<Type> EntityFsmList;

		void Start()
		{
			EntityFsmList =new List<Type>();
			AddAvatarFsm();
		}

		private void AddAvatarFsm()
		{
			EntityFsmList.Add(typeof(StateIdle));
			EntityFsmList.Add(typeof(StateMove));
			EntityFsmList.Add(typeof(StateSkill));
			EntityFsmList.Add(typeof(StateWeaklyControl));
			EntityFsmList.Add(typeof(StateStronglyControl));
			EntityFsmList.Add(typeof(StateDeath));
		}

		public FsmState<T>[] GetAllFsmState<T>() where T : Entity
		{
			if(EntityFsmList == null)
				return null;

			FsmState<T>[] list = new FsmState<T>[EntityFsmList.Count];
			for(int i = 0; i < EntityFsmList.Count; i++)
			{
				list[i] = (FsmState<T>)Activator.CreateInstance(EntityFsmList[i]);
				if(list[i] == null)
				{
					Log.Error("Can not create FsmState instance '{0}'.", EntityFsmList[i]);
					return null;
				}
			}
			return list;
		}
	}
}