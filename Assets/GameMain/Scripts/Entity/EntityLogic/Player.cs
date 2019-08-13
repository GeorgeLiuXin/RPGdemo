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

		private LocalController m_localController;
		public LocalController localController
		{
			get
			{
				if(m_localController == null)
				{
					m_localController = GetComponent<LocalController>();
				}
				return m_localController;
			}
		}
		private PreSkillComponent m_preSkillCom;
		public PreSkillComponent PreSkillCom
		{
			get
			{
				if(m_preSkillCom == null)
				{
					m_preSkillCom = GetComponent<PreSkillComponent>();
				}
				return m_preSkillCom;
			}
		}
		private AimComponent m_aimCom;
		public AimComponent AimCom
		{
			get
			{
				if(m_aimCom == null)
				{
					m_aimCom = GetComponent<AimComponent>();
				}
				return m_aimCom;
			}
		}

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
		}

		protected override void InitComponent()
		{
			base.InitComponent();
			gameObject.AddComponent<LocalController>().SetOwner(this);
			gameObject.AddComponent<PreSkillComponent>().SetOwner(this);
			gameObject.AddComponent<AimComponent>().SetOwner(this);
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
