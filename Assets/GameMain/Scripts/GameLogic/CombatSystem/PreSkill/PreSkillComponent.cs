using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
	public class PreSkillComponent : ComponentBase
	{
		private bool m_bInPreSkill;
		//当前技能瞄准方式
		private int m_nCurMode;
		private IPreSkillHelper[] m_PreSkillModes;

		protected override void InitComponent()
		{
			m_PreSkillModes = new IPreSkillHelper[(int)ePreSkillMode.PreSkillMode_Size];
			m_PreSkillModes[(int)ePreSkillMode.PreSkillMode_None] = new PreSkillLogicBase();
			m_PreSkillModes[(int)ePreSkillMode.PreSkillMode_CurTarget] = new PreSkillLogic_CurTarget();
			m_PreSkillModes[(int)ePreSkillMode.PreSkillMode_CameraDir] = new PreSkillLogic_CameraDir();
			m_PreSkillModes[(int)ePreSkillMode.PreSkillMode_AimArea] = new PreSkillLogic_AimArea();
		}

		public override void OnComponentStart()
		{
			m_nCurMode = 0;
			foreach(var item in m_PreSkillModes)
			{
				item.SetOwner(Owner);
			}
			m_bInPreSkill = false;
		}

		public override void OnPreDestroy()
		{
		}

		public bool PreSkill(int nSkillID)
		{
			if(m_bInPreSkill)
			{
				ResetSkill();
			}

			DRSkillData skillData = GameEntry.DataTable.GetDataTable<DRSkillData>().GetDataRow(nSkillID);
			if(skillData == null)
				return false;
			m_nCurMode = skillData.MSV_PreSkillMode;
			if(m_nCurMode <= (int)ePreSkillMode.PreSkillMode_None
				|| m_nCurMode >= (int)ePreSkillMode.PreSkillMode_Size)
				return false;
			m_bInPreSkill = m_PreSkillModes[m_nCurMode].Enter(skillData);
			return m_bInPreSkill;
		}

		public void Update()
		{
			if(m_nCurMode <= (int)ePreSkillMode.PreSkillMode_None
				|| m_nCurMode >= (int)ePreSkillMode.PreSkillMode_Size)
				return;
			m_PreSkillModes[m_nCurMode].Update();
		}

		public bool UseSkill()
		{
			if(m_nCurMode <= (int)ePreSkillMode.PreSkillMode_None
				|| m_nCurMode >= (int)ePreSkillMode.PreSkillMode_Size)
				return false;
			bool bResult = m_PreSkillModes[m_nCurMode].UseSkill();
			ResetSkill();
			return bResult;
		}

		private void ResetSkill()
		{
			if(m_nCurMode <= (int)ePreSkillMode.PreSkillMode_None
				|| m_nCurMode >= (int)ePreSkillMode.PreSkillMode_Size)
				return;

			m_PreSkillModes[m_nCurMode].Reset();
			m_bInPreSkill = false;
		}
	}
}