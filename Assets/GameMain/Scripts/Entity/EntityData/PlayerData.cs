using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy
{
	[Serializable]
	public class PlayerData : AvatarData
	{
		[SerializeField]
		private int m_Level;

		[SerializeField]
		private string m_Name = null;

		public PlayerData(int entityId, int typeId, int level)
			: base(entityId, typeId, CampType.Player)
		{
			m_Level = level;
			UpdateAValueByInstance();
		}
		
		protected override int AValueDataID
		{
			get
			{
				return 0 + m_Level;
			}
		}

		/// <summary>
		/// 角色名称。
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}
	}
}
