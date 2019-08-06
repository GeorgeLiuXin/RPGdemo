using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy
{
	[Serializable]
	public class MonsterData : AvatarData
	{
		[SerializeField]
		private int m_nMonsterAValueID;

		public MonsterData(int entityId, int typeId, int monsterAValueID) 
			: base(entityId, typeId, CampType.Enemy)
		{
			m_nMonsterAValueID = monsterAValueID;
			UpdateAValueByInstance();
		}

		protected override int AValueDataID
		{
			get
			{
				return m_nMonsterAValueID;
			}
		}
	}
}
