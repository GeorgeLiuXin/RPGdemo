using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy
{
	public class CameraManager : GameFrameworkComponent
	{
		private CameraDemo m_CurCamera;
        
        public void InitEngineCamera(CameraDemo _camera)
        {
            if (_camera == null)
                return;
            m_CurCamera = _camera;
        }
        
		public Vector3 GetWorldToViewportPoint(Vector3 vWorldPos)
		{
			if(m_CurCamera == null)
				return vWorldPos;

			return m_CurCamera.GetWorldToViewportPoint(vWorldPos);
		}
		public Vector3 GetWorldSpaceDir2D(Vector3 vOriginalDir)
		{
			if(m_CurCamera == null)
				return vOriginalDir;

			Vector3 vDir = m_CurCamera.GetCamDir();
			vDir.y = 0;
			vDir.Normalize();

			Quaternion quat = Quaternion.LookRotation(vDir);
			Quaternion inverseQuat = Quaternion.Inverse(quat);
			return inverseQuat * vOriginalDir;
		}

		public Vector3 GetCameraSpaceDir2D(Vector3 vOriginalDir)
		{
			if(m_CurCamera == null)
				return vOriginalDir;

			Vector3 vDir = m_CurCamera.GetCamDir();
			vDir.y = 0;
			vDir.Normalize();

			Quaternion quat = Quaternion.LookRotation(vDir);
			return quat * vOriginalDir;
		}

		public Vector3 GetCamDir()
		{
			if(m_CurCamera == null)
				return Vector3.forward;

			return m_CurCamera.GetCamDir();
		}
		public Vector3 GetCamPos()
		{
			if(m_CurCamera == null)
				return Vector3.forward;

			return m_CurCamera.GetCamPos();
		}
		public Camera GetCurEngineCamera()
		{
			if(m_CurCamera == null)
				return null;

			return m_CurCamera.GetEngineCamera();
		}
		public bool IsInMidView(Vector3 worldPos, float fX1 = 0.0f, float fX2 = 1.0f, float fY1 = 0.0f, float fY2 = 1.0f)
		{
			Camera curCamera = GetCurEngineCamera();
			if(curCamera == null)
			{
				return false;
			}
			Transform camTransform = curCamera.transform;
			Vector2 viewPos = curCamera.WorldToViewportPoint(worldPos);
			Vector3 dir = (worldPos - camTransform.position).normalized;
			float dot = Vector3.Dot(camTransform.forward, dir);//判断物体是否在相机前面

			if(dot > 0 && viewPos.x >= fX1 && viewPos.x <= fX2 && viewPos.y >= fY1 && viewPos.y <= fY2)
				return true;
			else
				return false;
		}
		public bool IsInMidViewX(Vector3 worldPos, float fX1 = 0.0f, float fX2 = 1.0f)
		{
			Camera curCamera = GetCurEngineCamera();
			if(curCamera == null)
			{
				return false;
			}
			Transform camTransform = curCamera.transform;
			Vector2 viewPos = curCamera.WorldToViewportPoint(worldPos);
			Vector3 dir = (worldPos - camTransform.position).normalized;
			float dot = Vector3.Dot(camTransform.forward, dir);//判断物体是否在相机前面

			if(dot > 0 && viewPos.x >= fX1 && viewPos.x <= fX2 && viewPos.y >= 0.0f && viewPos.y <= 1.0f)
				return true;
			else
				return false;
		}
		public bool IsInMidViewY(Vector3 worldPos, float fY1 = 0.0f, float fY2 = 1.0f)
		{
			Camera curCamera = GetCurEngineCamera();
			if(curCamera == null)
			{
				return false;
			}
			Transform camTransform = curCamera.transform;
			Vector2 viewPos = curCamera.WorldToViewportPoint(worldPos);
			Vector3 dir = (worldPos - camTransform.position).normalized;
			float dot = Vector3.Dot(camTransform.forward, dir);//判断物体是否在相机前面

			if(dot > 0 && viewPos.x >= 0.0f && viewPos.x <= 1.0f && viewPos.y >= fY1 && viewPos.y <= fY2)
				return true;
			else
				return false;
		}
		public Vector2 GetViewPos(Vector3 worldPos)
		{
			Camera curCamera = GetCurEngineCamera();
			if(curCamera == null)
			{
				return Vector2.zero;
			}
			Transform camTransform = curCamera.transform;
			Vector2 viewPos = curCamera.WorldToViewportPoint(worldPos);
			Vector3 dir = (worldPos - camTransform.position).normalized;
			float dot = Vector3.Dot(camTransform.forward, dir);//判断物体是否在相机前面

			if(dot <= 0)
			{
				return Vector2.zero;
			}

			return viewPos;
		}
	}
}