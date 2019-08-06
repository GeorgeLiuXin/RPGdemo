using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
	public class NpcData : AvatarData
	{
		public NpcData(int entityId, int typeId)
			: base(entityId, typeId, CampType.Neutral)
		{
		}

		protected override int AValueDataID
		{
			get
			{
				return 0;
			}
		}
	}
}
