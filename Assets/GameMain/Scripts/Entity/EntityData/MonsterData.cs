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
		public MonsterInfo info;

		public MonsterData(int entityId, int typeId, MonsterInfo _info)
            : base(entityId, typeId, CampType.Enemy)
        {
            DRMonster data = GameEntry.DataTable.GetDataTable<DRMonster>().GetDataRow(_info.nMonsterID);
            if (data == null)
            {
                Log.Error("当前怪物id不存在!");
                return;
            }

            m_nMonsterAValueID = data.MonsterAValue;
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
