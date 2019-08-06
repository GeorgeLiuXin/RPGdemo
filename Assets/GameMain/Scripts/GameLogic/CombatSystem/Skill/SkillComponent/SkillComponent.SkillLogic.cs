using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
	public partial class SkillComponent
	{
		private GSkillSpellLogic m_pSpellLogic;

		public void FinishSkill()
		{
			if(m_pSpellLogic == null)
				return;
			m_pSpellLogic.Reset();
			m_pSpellLogic = null;
		}

		internal void ProcessSkillEffect(DRSkillData m_pSkillData, GTargetInfo m_TargetInfo, SkillAValueData m_AValue)
		{
			throw new NotImplementedException();
		}

		internal void CreateSkillProjectile(DRSkillData m_pSkillData, GTargetInfo m_TargetInfo, SkillAValueData m_AValue)
		{
			throw new NotImplementedException();
		}
	}
}