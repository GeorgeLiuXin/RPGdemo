using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy
{
	public partial class Avatar : Entity
    {
        [SerializeField]
		private AvatarData m_AvatarData;
		
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
		private GCDComponent m_cdCom;
		public GCDComponent CDCom
		{
			get
			{
				if(m_cdCom == null)
				{
					m_cdCom = GetComponent<GCDComponent>();
				}
				return m_cdCom;
			}
        }
        private ThreatComponent m_threatCom;
        public ThreatComponent ThreatCom
        {
            get
            {
                if (m_threatCom == null)
                {
                    m_threatCom = GetComponent<ThreatComponent>();
                }
                return m_threatCom;
            }
        }

        protected virtual void InitComponent()
		{
			gameObject.AddComponent<AnimationComponent>().SetOwner(this);
			gameObject.AddComponent<MoveComponent>().SetOwner(this);
			gameObject.AddComponent<SkillComponent>().SetOwner(this);
			gameObject.AddComponent<GCDComponent>().SetOwner(this);
            gameObject.AddComponent<ThreatComponent>().SetOwner(this);
        }

		protected virtual void ReleaseComponent()
		{
			Destroy(m_animCom);
			m_animCom = null;
			Destroy(m_moveCom);
			m_moveCom = null;
			Destroy(m_skillCom);
			m_skillCom = null;
			Destroy(m_cdCom);
			m_cdCom = null;
			Destroy(m_threatCom);
			m_threatCom = null;
		}
		
		public float ModelRadius
		{
			get
			{
				return 0.5f;
			}
		}

        public CampType Camp
        {
            get
            {
                return m_AvatarData!=null ? m_AvatarData.Camp : CampType.Unknown;
            }
        }

        protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			gameObject.SetLayerRecursively(Constant.Layer.AvatarLayerId);
			InitComponent();
			InitFsm();
		}

		protected override void OnHide(object userData)
		{
			ReleaseComponent();
			base.OnHide(userData);
		}

		protected override void OnShow(object userData)
		{
			base.OnShow(userData);

			m_AvatarData = userData as AvatarData;
			if(m_AvatarData == null)
			{
				Log.Error("AvatarData is invalid.");
			}
		}

		protected virtual void OnDead(Avatar attacker)
        {
            GameEntry.Fsm.DestroyFsm(m_fsm);
        }

        ////////////////////////////////////////////////////
        //移动参数
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

        public void SetDir2D(Vector3 vDir)
        {
            if (vDir == default(Vector3))
                return;

            vDir.Normalize2D();

            Quaternion TargetRotation = Quaternion.LookRotation(vDir);
            CachedTransform.rotation = TargetRotation;
        }

        ////////////////////////////////////////////////////
        //常用参数
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
			if(m_AvatarData.HP <= 0)
			{
				Dead();
				OnDead(this);
			}
		}

		public void SetHeal(int nCasterID, float fValue)
		{
			float hp = Mathf.Min(m_AvatarData.MaxHP, m_AvatarData.HP + fValue);
			m_AvatarData.HP = hp;
		}

        public void SetHpCost(float nCostHp)
        {
            float hp = Mathf.Max(1, m_AvatarData.HP - nCostHp);
            m_AvatarData.HP = hp;
        }

        ////////////////////////////////////////////////////
        //属性集相关
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

        ////////////////////////////////////////////////////
        //仇恨相关
        public void AddThreat(Avatar pAvatar, float fValue)
        {
            if (ThreatCom == null)
            {
                Log.Error("ThreatCom组件不存在!");
                return;
            }

            ThreatCom.AddThreat(pAvatar, fValue);
        }

        ///////////////////////////////////////////////////////
        //状态标记位设置
        public bool IsFight
        {
            get { return CheckState(StateDefine.State_Fight); }
        }
        public void EnterCombat()
        {
            SetState(StateDefine.State_Fight);
        }
        public void LeaveCombat()
        {
            ResetState(StateDefine.State_Fight);
        }
		public bool IsDead
		{
			get { return CheckState(StateDefine.State_Death); }
		}
		public void Dead()
		{
			StateDeathParam param = new StateDeathParam();
			SetFsmState(this, param);
			SetState(StateDefine.State_Death);
		}
    }
}
