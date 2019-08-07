using GameFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy
{
	public partial class Avatar : Entity
	{
		[SerializeField]
		private AvatarData m_AvatarData = null;
		
		private AnimationComponent m_animCom;
		public AnimationComponent AnimCom
		{
			get
			{
				if(m_animCom == null)
				{
					m_animCom = GetComponent<AnimationComponent>();
				}
				return m_animCom;
			}
		}
		private MoveComponent m_moveCom;
		public MoveComponent MoveCom
		{
			get
			{
				if(m_moveCom == null)
				{
					m_moveCom = GetComponent<MoveComponent>();
				}
				return m_moveCom;
			}
		}
		private SkillComponent m_skillCom;
		public SkillComponent SkillCom
		{
			get
			{
				if(m_skillCom == null)
				{
					m_skillCom = GetComponent<SkillComponent>();
				}
				return m_skillCom;
			}
		}

		protected virtual void InitComponent()
		{
			gameObject.AddComponent<AnimationComponent>().SetOwner(this);
			gameObject.AddComponent<MoveComponent>().SetOwner(this);
			gameObject.AddComponent<SkillComponent>().SetOwner(this);
		}

		public bool IsDead
		{
			get
			{
				return m_AvatarData.HP <= 0;
			}
		}
		
		public float ModelRadius
		{
			get
			{
				return 0.5f;
			}
		}

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			gameObject.SetLayerRecursively(Constant.Layer.AvatarLayerId);
			InitComponent();
			InitFsm();
		}

		protected override void OnShow(object userData)
		{
			base.OnShow(userData);

			m_AvatarData = userData as AvatarData;
			if(m_AvatarData == null)
			{
				Log.Error("AvatarData is invalid.");
				return;
			}
		}

		protected virtual void OnDead(Avatar attacker)
		{
			GameEntry.Entity.HideEntity(this);
		}
		
		public void MoveToPoint(Vector3 vPos)
		{
			if(MoveCom == null)
			{
				Log.Error("MoveCom组件不存在!");
				return;
			}

			SetFsmState(this, ReferencePool.Acquire<StateMoveParam>().Fill(vPos));
		}
		
		public void MoveDistance(Vector3 vMotion, bool bPhysics)
		{
			if(MoveCom == null)
			{
				Log.Error("MoveCom组件不存在!");
				return;
			}
			MoveCom.MoveDistance(vMotion, bPhysics);
		}
		public void StopMovement()
		{
			MoveDistance(Vector3.zero, true);
		}

		public void LookAtPos(Vector3 vWorldPos)
		{
			if(CachedTransform == null)
				return;
			CachedTransform.LookAt2D(vWorldPos);
		}

		public void PlayAnimation(int nAnimID)
		{
			if(AnimCom == null)
			{
				Log.Error("AnimCom组件不存在!");
				return;
			}
			AnimCom.PlayAnimation(nAnimID);
		}

		public float HP
		{
			get
			{
				if(m_AvatarData == null)
					return 0;
				return m_AvatarData.HP;
			}
		}
		public float MaxHP
		{
			get
			{
				if(m_AvatarData == null)
					return 0;
				return m_AvatarData.MaxHP;
			}
		}
		public float HPRatio
		{
			get
			{
				if(m_AvatarData == null)
					return 0;
				return m_AvatarData.HPRatio;
			}
		}

		public void SetDamage(int nCasterID, float fValue)
		{
			float hp = Mathf.Max(0, m_AvatarData.HP - fValue);
			m_AvatarData.HP = hp;
		}

		public void SetHeal(int nCasterID, float fValue)
		{
			float hp = Mathf.Min(m_AvatarData.MaxHP, m_AvatarData.HP + fValue);
			m_AvatarData.HP = hp;
		}

		public void SetAValueData(AvatarAValueDefine define, int type, object value)
		{
			m_AvatarData.SetAValueData(define, type, value);
		}
		public int GetIntValue(AvatarAValueDefine define)
		{
			return m_AvatarData.GetIntValue(define);
		}
		public float GetFloatValue(AvatarAValueDefine define)
		{
			return m_AvatarData.GetFloatValue(define);
		}
		public double GetPercentValue(AvatarAValueDefine define)
		{
			return m_AvatarData.GetPercentValue(define);
		}

		public PlayerAValueData GetRoleAValue()
		{
			if(m_AvatarData == null)
				return null;
			return m_AvatarData.GetAValue();
		}
	}
}
