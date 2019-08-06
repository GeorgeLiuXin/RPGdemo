using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy
{
	public class CameraDemo : MonoBehaviour
	{
		private Player m_Player;
		private Transform TargetTransform
		{
			get
			{
				if(m_Player == null)
					return null;
				return m_Player.transform;
			}
		}
		private Transform m_Pivot;
		private Camera m_EngineCamera;

		//////////////////////////////////////////////
		[SerializeField]
		private float rotateSpeed = 2;
		[SerializeField]
		private float distance = 0f;
		[SerializeField]
		private float scrollSpeed = 3;
		//////////////////////////////////////////////
		

		// Use this for initialization
		void Start()
		{
			GameEntry.Event.Subscribe(CameraEvent.EventId, OnCameraEvent);
			InitCameraConfig();
		}

		private void OnDestroy()
		{
			GameEntry.Event.Unsubscribe(CameraEvent.EventId, OnCameraEvent);
		}

		private void InitCameraConfig()
		{
			transform.rotation = Quaternion.Euler(new Vector3(350, 0, 0));

			m_Pivot = transform.GetChild(0);
			m_Pivot.localPosition = new Vector3(0, 6f, -3.2f);
			m_Pivot.localRotation = Quaternion.Euler(new Vector3(60, 0, 0));
			m_Pivot.localScale = Vector3.one;

			m_EngineCamera = gameObject.GetComponentInChildren<Camera>();
			if(m_EngineCamera == null)
			{
				Log.Error("Can not find the engine camera On cameraDemo.");
			}
		}

		private void OnCameraEvent(object sender, GameEventArgs e)
		{
			CameraEvent ne = (CameraEvent)e;
			if(ne == null)
				return;
			if(ne.Player == null || !(ne.Player is Player))
				return;
			m_Player = ne.Player;
		}
		
		void FixedUpdate()
		{
			if(TargetTransform == null)
				return;
			HandleInput();
			ScrollView();
			UpdatePos();
		}

		private void HandleInput()
		{
			if(Input.GetMouseButton(1))
			{
				Vector3 originalPosition = transform.position;
				Quaternion originalRotation = transform.rotation;

				transform.RotateAround(TargetTransform.position,
					TargetTransform.up, rotateSpeed * Input.GetAxis("Mouse X"));
				transform.RotateAround(TargetTransform.position,
					transform.right, -rotateSpeed * Input.GetAxis("Mouse Y"));

				float x = transform.eulerAngles.x;
				if(x <= 315 || x >= 355)
				{
					transform.position = originalPosition;
					transform.rotation = originalRotation;
				}
			}
		}

		//控制视野拉近与拉远
		void ScrollView()
		{
			if(Input.GetAxis("Mouse ScrollWheel") != 0)
			{
				Vector3 vOffest = m_Pivot.localPosition;
				distance = vOffest.magnitude;
				distance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
				distance = Mathf.Clamp(distance, 3, 10);
				vOffest = vOffest.normalized * distance;
				m_Pivot.localPosition = vOffest;
			}
		}

		private void UpdatePos()
		{
			Vector3 vPos = TargetTransform.position - transform.position;
			transform.position += vPos * 0.1f;
		}
		
		public Camera GetEngineCamera()
		{
			return m_EngineCamera;
		}
		public Vector3 GetCamPos()
		{
			if(m_EngineCamera == null)
				return Vector3.zero;
			return m_EngineCamera.transform.position;
		}
		public Vector3 GetCamDir()
		{
			if(m_EngineCamera == null)
				return Vector3.zero;
			return m_EngineCamera.transform.forward;
		}
		public Vector3 GetWorldToViewportPoint(Vector3 vWorldPos)
		{
			if(m_EngineCamera == null)
				return vWorldPos;
			return m_EngineCamera.WorldToViewportPoint(vWorldPos);
		}
	}
}