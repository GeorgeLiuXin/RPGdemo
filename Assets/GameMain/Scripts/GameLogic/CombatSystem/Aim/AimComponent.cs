using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Entity;
using GameFramework;

namespace Galaxy
{
	public class AimComponent : ComponentBase
	{
		private const float fTickTime = 0.5f;
		private float m_fTimer;
		//当前目标
		private Avatar m_CurTarget;
		//目标列表
		private List<Avatar> m_TargetList;

		public override void OnComponentStart()
		{
			m_CurTarget = null;
			m_TargetList = new List<Avatar>();
			m_fTimer = fTickTime;
		}

		public override void OnPreDestroy()
		{
			if(m_TargetList != null)
			{
				m_TargetList.Clear();
				m_TargetList = null;
			}
			m_fTimer = fTickTime;
			m_CurTarget = null;
		}

		public void Update()
		{
			TickTarget();

			m_fTimer -= Time.deltaTime;
			if(m_fTimer > 0)
				return;

			m_fTimer += fTickTime;
			//刷新列表
			TickEntities();
		}

		private void TickEntities()
		{
			IEntityGroup group = GameEntry.Entity.GetEntityGroup(Constant.Entity.MonsterGroupName);
			if(group == null)
				return;
			IEntity[] list = group.GetAllEntities();
			if(list == null || list.Length == 0)
				return;

			m_TargetList.Clear();
			foreach(var item in list)
			{
				Avatar actor = GameEntry.Entity.GetGameEntity(item.Id) as Avatar;
				if(actor == null || actor.IsDead)
					continue;

				if(AIUtility.GetRelation(Owner.Camp, actor.Camp) != RelationType.Hostile)
					continue;

				Vector3 vOffestPos = actor.GetPos() - Owner.GetPos();
				vOffestPos.y = 0;
				float fMagnitude = vOffestPos.magnitude + 0.5f;

				// 排除最远距离外的目标
				if(fMagnitude > 25f)
					continue;

				m_TargetList.Add(actor);
			}
		}

		private void TickTarget()
		{
			if(m_CurTarget == null || m_CurTarget.IsDead)
			{
				ResetTarget();
			}
		}

		public void ResetTarget()
		{
			m_CurTarget = null;
		}

		public void SetTarget(Avatar pAvatar)
		{
			if(pAvatar != null)
			{
				m_CurTarget = pAvatar;

				ChangeTargetEvent e = new ChangeTargetEvent();
				e.TargetID = m_CurTarget.Id;
				GameEntry.Event.Fire(Owner, e);
			}
		}

		public Avatar GetTarget()
		{
			return m_CurTarget;
		}

		public Avatar GetTabTarget()
		{
			int index = 0;
			if(m_CurTarget != null)
			{
				TickTarget();
				TickEntities();
				index = m_TargetList.IndexOf(m_CurTarget);
			}

			if(m_TargetList.Count == 0)
			{
				ResetTarget();
				return null;
			}

			index++;
			index %= m_TargetList.Count;
			Avatar nextTarget = m_TargetList[index];
			if(nextTarget == null || nextTarget.IsDead)
			{
				Log.Error(Utility.Text.Format("下一个目标不合法!"));
				return null;
			}

			SetTarget(nextTarget);
			return nextTarget;
		}
	}

}