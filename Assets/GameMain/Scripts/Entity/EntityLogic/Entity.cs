﻿using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy
{
    public abstract class Entity : EntityLogic
    {
        [SerializeField]
        private EntityData m_EntityData = null;

        public int Id
        {
            get
            {
                return Entity.Id;
            }
        }

        public Animator CachedAnimator
		{
            get;
            private set;
        }

		//temp 可将引擎物体与逻辑分离
		public GameObject GetEngineObject()
		{
			return gameObject;
		}

		public Vector3 GetPos()
		{
			return gameObject.transform.position;
		}

		public Vector3 GetDir()
		{
			return gameObject.transform.forward;
		}

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
			CachedAnimator = GetComponent<Animator>();
        }
		
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_EntityData = userData as EntityData;
            if (m_EntityData == null)
            {
                Log.Error("Entity data is invalid.");
                return;
            }

            Name = Utility.Text.Format("[Entity {0}]", Id.ToString());
            CachedTransform.localPosition = m_EntityData.Position;
            CachedTransform.localRotation = m_EntityData.Rotation;
            CachedTransform.localScale = Vector3.one;
        }
		
        protected override void OnHide(object userData)
        {
            base.OnHide(userData);
        }
		
        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
        }

        protected override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
        }

        protected override void OnDetachFrom(EntityLogic parentEntity, object userData)
        {
            base.OnDetachFrom(parentEntity, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
    }
}
