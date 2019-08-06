using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy
{
	public class Player : Avatar
	{
		[SerializeField]
		private PlayerData m_PlayerData = null;
		
		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
		}

		protected override void InitComponent()
		{
			base.InitComponent();
			gameObject.AddComponent<LocalController>().SetOwner(this);
		}

		protected override void OnShow(object userData)
		{
			base.OnShow(userData);

			m_PlayerData = userData as PlayerData;
			if(m_PlayerData == null)
			{
				Log.Error("PlayerData is invalid.");
				return;
			}

			PlayAnimation((int)CommonAnimation.Idle);
			GameEntry.Event.Fire(this, ReferencePool.Acquire<CameraEvent>().Fill(this, true, null));
		}
		
	}
}
