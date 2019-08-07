using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy
{
	public class AnimationComponent : ComponentBase
	{
		class SMotionData
		{
			public bool bMotion;
			public bool bPhy;
			public Vector3 vGoalPos;
			public Vector3 vGoalDir;
			public float fMotionTime;
			public Vector3 vModifySpeed;
			public Vector3 vExSpeed;

			public void Reset()
			{
				bPhy = true;
				vGoalPos = Vector3.zero;
				vGoalDir = Vector3.zero;
				fMotionTime = 0;
				vModifySpeed = Vector3.zero;
				vExSpeed = Vector3.zero;
			}
		}

		//当前播放动画
		private int m_curAnimID;
		private SMotionData m_motionData = new SMotionData();
		private Animator m_Animator = null;
		//动画trigger压栈
		private Stack<string> m_triggerWaitingList = new Stack<string>();

		public override void SetOwner(Avatar logicObject)
		{
			base.SetOwner(logicObject);

			if(Owner == null)
				return;

			m_curAnimID = -1;

			GameObject pEngineObj = Owner.GetEngineObject();
			if(pEngineObj == null)
				return;
			
			m_Animator = logicObject.CachedAnimator;
			if(m_Animator != null)
			{
				m_Animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
				m_Animator.applyRootMotion = false;
			}
		}

		private DRAnimation GetConfigData(int nAnimID)
		{
			DRAnimation data = GameEntry.DataTable.GetDataTable<DRAnimation>().GetDataRow(nAnimID);
			if(data == null)
			{
				Log.Warning("动画表中没有 '{0}' ", nAnimID);
				return null;
			}
			return data;
		}

		public bool PlayAnimation(int nAnimID, Vector3 vGoalPos = default(Vector3), Vector3 vGoalDir = default(Vector3), float fMotionTime = 0, bool bPhy = false)
		{
			if(Owner == null)
				return false;

			if(m_Animator == null)
				return false;
			
			DRAnimation animData = GetConfigData(nAnimID);
			if(animData == null)
				return false;
			
			if(animData.IsSelfRestart != 0 && m_curAnimID == nAnimID)
			{
				StopAnim();
			}
			if(animData.IsSelfRestart == 0 && m_curAnimID == nAnimID)
			{
				return true;
			}

			m_triggerWaitingList.Push(animData.ResAnimName);

			m_curAnimID = nAnimID;
			if(animData.Layer == 0)
			{
				m_motionData.bMotion = (animData.Motion != 0);
				if(m_motionData.bMotion == true)
				{
					m_motionData.Reset();
					SaveMotionData(vGoalPos, vGoalDir, fMotionTime, bPhy);
				}
			}

			return true;
		}

		private void SaveMotionData(Vector3 vGoalPos, Vector3 vGoalDir, float fMotionTime, bool bPhy)
		{
			if(Owner == null || Math.Abs(fMotionTime) < 0.1f)
			{
				return;
			}
			vGoalDir.Normalize();
			m_motionData.vGoalPos = vGoalPos;
			m_motionData.vGoalDir = vGoalDir;
			m_motionData.fMotionTime = fMotionTime;
			m_motionData.bPhy = bPhy;
		}

		protected bool CalculateMotion(string animName, Vector3 vGoalPos, Vector3 vGoalDir, float fMotionTime, bool bPhy)
		{
			if(Owner == null || Math.Abs(fMotionTime) < 0.1f)
			{
				return false;
			}
			vGoalDir.Normalize();
			m_motionData.vGoalPos = vGoalPos;
			m_motionData.vGoalDir = vGoalDir;
			m_motionData.fMotionTime = fMotionTime;
			m_motionData.bPhy = bPhy;

			//世界坐标下玩家的位移
			Vector3 vDeltaPos = vGoalPos - Owner.GetPos();

			//世界坐标下动画位移
			Vector3 vAnimDeltaPos = Vector3.zero;
			if(GetAnimDeltaPos(animName, fMotionTime, vGoalDir, ref vAnimDeltaPos) == false)
			{
				return false;
			}

			if(Math.Abs(vAnimDeltaPos.x) > 0.09f)
			{
				m_motionData.vModifySpeed.x = vDeltaPos.x / vAnimDeltaPos.x;
			}
			else
			{
				m_motionData.vExSpeed.x = vDeltaPos.x / fMotionTime;
			}

			if(Math.Abs(vAnimDeltaPos.y) > 0.09f)
			{
				m_motionData.vModifySpeed.y = vDeltaPos.y / vAnimDeltaPos.y;
			}
			else
			{
				m_motionData.vExSpeed.y = vDeltaPos.y / fMotionTime;
			}

			if(Math.Abs(vAnimDeltaPos.z) > 0.09f)
			{
				m_motionData.vModifySpeed.z = vDeltaPos.z / vAnimDeltaPos.z;
			}
			else
			{
				m_motionData.vExSpeed.z = vDeltaPos.z / fMotionTime;
			}


			return true;
		}

		protected bool GetAnimDeltaPos(string animName, float fMotionTime, Vector3 vGoalDir, ref Vector3 vAnimDeltaPos)
		{
			m_Animator.Play(animName);
			m_Animator.Update(0);
			m_Animator.Update(fMotionTime);
			Vector3 vWorldDelta = m_Animator.deltaPosition;
			m_Animator.Play(animName, -1, 0);
			m_Animator.Update(0);

			vAnimDeltaPos = vWorldDelta;
			return true;
		}

		public bool GetAnimPos(int nAnimID, float fMotionTime, ref Vector3 vAnimDeltaPos)
		{
			if(m_Animator == null)
				return false;

			DRAnimation animData = GetConfigData(nAnimID);
			if(animData == null)
				return false;
			
			m_Animator.Play(animData.ResAnimName);
			m_Animator.Update(0);
			m_Animator.Update(fMotionTime);
			Vector3 vWorldDelta = m_Animator.deltaPosition;
			m_Animator.Play(animData.ResAnimName, -1, 0);
			m_Animator.Update(0);

			vAnimDeltaPos = vWorldDelta;
			return true;
		}

		protected void UpdateMotion(float fFrameTime)
		{
			if(m_Animator == null)
				return;

			if(m_motionData.bMotion == false)
				return;

			if(Owner == null)
				return;
			
			if(m_Animator.speed < 0.01f)
				return;

			m_motionData.fMotionTime -= fFrameTime;
			if(m_motionData.fMotionTime < 0)
			{
				m_motionData.bMotion = false;
				return;
			}

			Vector3 vMotion = Vector3.zero;
			vMotion.x = m_Animator.deltaPosition.x * m_motionData.vModifySpeed.x + m_motionData.vExSpeed.x * Time.deltaTime;
			//vMotion.y = m_Animator.deltaPosition.y * m_motionData.vModifySpeed.y + m_motionData.vExSpeed.y * Time.deltaTime;
			vMotion.y = 0;
			vMotion.z = m_Animator.deltaPosition.z * m_motionData.vModifySpeed.z + m_motionData.vExSpeed.z * Time.deltaTime;

			Owner.MoveDistance(vMotion, m_motionData.bPhy);
		}

		protected void StopAnim()
		{
			if(m_Animator == null)
				return;
			m_Animator.SetTrigger("Empty0");
		}

		public float GetAnimTime(int nAnimID)
		{
			DRAnimation animData = GetConfigData(nAnimID);
			if(animData == null)
				return 0.0f;
			
			if(null == m_Animator || string.IsNullOrEmpty(animData.ResAnimName) || null == m_Animator.runtimeAnimatorController)
				return 0.0f;

			RuntimeAnimatorController ac = m_Animator.runtimeAnimatorController;
			AnimationClip[] tAnimationClips = ac.animationClips;
			if(null == tAnimationClips || tAnimationClips.Length <= 0)
				return 0.0f;

			AnimationClip tAnimationClip;
			for(int tCounter = 0; tCounter < tAnimationClips.Length; tCounter++)
			{
				tAnimationClip = ac.animationClips[tCounter];
				if(null != tAnimationClip && tAnimationClip.name == animData.ResAnimName)
					return tAnimationClip.length;
			}

			return 0.0f;
		}

		bool IsCurAnimEnd()
		{
			if(m_Animator == null)
				return true;

			AnimatorStateInfo stateinfo = m_Animator.GetCurrentAnimatorStateInfo(0);
			if(stateinfo.loop == true)
			{
				return false;
			}
			if(stateinfo.normalizedTime >= 1.0f)
			{
				return true;
			}
			return false;
		}

		public void Update()
		{
			if(Owner == null)
				return;

			//UpdateAnimEnd();
		}
		public void FixedUpdate()
		{
			if(Owner == null)
				return;

			UpdateMotion(Time.deltaTime);
		}
		private void LateUpdate()
		{
			if(Owner == null || m_Animator == null)
				return;

			int nListCount = m_triggerWaitingList.Count;
			if(nListCount > 0)
			{
				string sTriggerName = m_triggerWaitingList.Pop();
				m_Animator.SetTrigger(sTriggerName);
				if(m_motionData.bMotion)
				{
					if(CalculateMotion(sTriggerName, 
						m_motionData.vGoalPos, m_motionData.vGoalDir, 
						m_motionData.fMotionTime, m_motionData.bPhy) == false)
					{
						m_motionData.bMotion = false;
					}
				}
				m_triggerWaitingList.Clear();
			}
		}

	}
}