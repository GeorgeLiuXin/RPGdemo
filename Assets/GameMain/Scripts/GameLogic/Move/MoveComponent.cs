using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
	/// <summary>
	/// 控制玩家位移
	/// </summary>
	public class MoveComponent : ComponentBase
	{
		private CharacterController m_characterController;
		[SerializeField]
		public float gravity;

		private bool m_bPhysics;
		private Vector3 m_vMotion;

		private Vector3 m_vGravitySpeed;

		public override void OnComponentStart()
		{
			m_characterController = GetComponent<CharacterController>();
			gravity = 0.5f;
			m_bPhysics = true;
			m_vMotion = new Vector3();
			m_vGravitySpeed = new Vector3();
		}

		public override void OnPreDestroy()
		{

		}
		
		void FixedUpdate()
		{
			if(m_vMotion == Vector3.zero)
				return;

			if(m_bPhysics && !m_characterController.isGrounded)
			{
				//先落地
				m_vGravitySpeed.y -= gravity * Time.deltaTime;
			}
			else
			{
				m_vGravitySpeed = Vector3.zero;
			}
			m_characterController.Move(m_vMotion + m_vGravitySpeed);
		}

		public void MoveDistance(Vector3 vMotion, bool bPhysics)
		{
			if(Owner == null)
				return;

			m_bPhysics = bPhysics;
			m_vMotion = vMotion;
		}
	}
}
