namespace Galaxy
{
    /// <summary>
    /// 技能发射器
    /// 1、直接伤害帧
    /// 2、子弹
    /// </summary>
    public abstract class GSkillLauncher
	{
		public abstract void Process(GSkillSpellLogic pSpellLogic, Avatar pCaster);
	}

	public class GSkillLauncher_Direct : GSkillLauncher
	{
		public override void Process(GSkillSpellLogic pSpellLogic, Avatar pCaster)
		{
			if(pCaster && pCaster.SkillCom != null && pSpellLogic.m_pSkillData != null)
			{
				pCaster.SkillCom.ProcessSkillEffect(pSpellLogic.m_pSkillData, pSpellLogic.m_TargetInfo, pSpellLogic.m_AValue);
			}
		}
	}

	public class GSkillLauncher_Bullet : GSkillLauncher
	{
		public override void Process(GSkillSpellLogic pSpellLogic, Avatar pCaster)
		{
			if(pCaster && pCaster.SkillCom != null && pSpellLogic.m_pSkillData != null)
			{
				int count = pSpellLogic.m_pSkillData.MSV_ProjectileParam1;
				for(int i = 0; i < count; ++i)
				{
					pCaster.SkillCom.CreateSkillProjectile(pSpellLogic.m_pSkillData, pSpellLogic.m_TargetInfo, pSpellLogic.m_AValue);
				}
			}
		}
	}
}
