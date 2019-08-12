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

		//tempcode
		public LevelInfo info;

		public MonsterData(int entityId, int typeId, int monsterAValueID, LevelInfo _info)
			: base(entityId, typeId, CampType.Enemy)
		{
			m_nMonsterAValueID = monsterAValueID;
			UpdateAValueByInstance();
			info = _info;
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
