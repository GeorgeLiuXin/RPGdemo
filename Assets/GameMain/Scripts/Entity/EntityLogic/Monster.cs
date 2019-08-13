using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
	public class Monster : Avatar
	{
		[SerializeField]
		private MonsterData m_MonsterData;
		
		private AIComponent m_aiCom;
		public AIComponent AICom
		{
			get
			{
				if(m_aiCom == null)
				{
					m_aiCom = GetComponent<AIComponent>();
				}
				return m_aiCom;
			}
		}

		protected override void InitComponent()
		{
			base.InitComponent();
			
			gameObject.AddComponent<AIComponent>().SetOwner(this);
		}
	}
}
