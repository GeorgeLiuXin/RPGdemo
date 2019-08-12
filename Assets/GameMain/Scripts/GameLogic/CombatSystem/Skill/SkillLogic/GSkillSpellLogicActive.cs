using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    /// <summary>
    /// 主动技能
    /// </summary>
    public class GSkillSpellLogic_Active : GSkillSpellLogic
	{
		protected float m_fCurTime;
		protected float m_fLockTime;
		protected float m_fLastTime;

		protected int m_nCurEffectCount;
		protected float m_fCurEffectTime;
		protected int m_nEffectCount;
		protected float m_fEffectTime;
		protected float m_fFirstEffectTime;

		public GSkillSpellLogic_Active()
		{
			m_fCurTime = 0f;
			m_fLockTime = 0f;
			m_fLastTime = 0f;

			m_nCurEffectCount = 0;
			m_fCurEffectTime = 0f;
			m_nEffectCount = 0;
			m_fEffectTime = 0f;
			m_fFirstEffectTime = 0f;
		}

		public override bool Init(GSkillInitParam param)
		{
			if(!base.Init(param))
				return false;

			m_fCurTime = 0f;
			m_fLockTime = m_pSkillData.MSV_LockTime;
			m_fLastTime = m_pSkillData.MSV_LastTime;

			m_nEffectCount = m_pSkillData.MSV_EffectCount;
			m_fEffectTime = m_pSkillData.MSV_EffectTime;
			m_fFirstEffectTime = m_pSkillData.MSV_FirstEffectTime;

			m_nCurEffectCount = 0;
			m_fCurEffectTime = m_fEffectTime - m_fFirstEffectTime;
			return true;
		}

		public override bool ReInit()
		{
			if(!base.ReInit())
				return false;
			return SetFSMState();
		}

		public override void Tick(float fFrameTime)
		{
			base.Tick(fFrameTime);
			m_fLockTime -= fFrameTime;
			m_fLastTime -= fFrameTime;
			if(m_fLastTime <= 0)
			{
				Finish();
			}
		}

		protected void TickEffect(float fFrameTime)
		{
			if(m_nCurEffectCount >= m_nEffectCount)
				return;

			m_fCurTime += fFrameTime;
			if(m_fCurTime >= m_fEffectTime)
			{
				++m_nCurEffectCount;
				m_fCurTime -= m_fEffectTime;
				ProcessEffect();
			}
		}

		public override void Reset()
		{
			base.Reset();
			m_fCurTime = 0f;
			m_fLockTime = m_pSkillData.MSV_LockTime;
			m_fLastTime = m_pSkillData.MSV_LastTime;

			m_nEffectCount = m_pSkillData.MSV_EffectCount;
			m_fEffectTime = m_pSkillData.MSV_EffectTime;
			m_fFirstEffectTime = m_pSkillData.MSV_FirstEffectTime;

			m_nCurEffectCount = 0;
			m_fCurEffectTime = m_fEffectTime - m_fFirstEffectTime;
		}

		public override bool SetFSMState()
		{
			if(!m_pOwner)
				return false;
			StateSkillParam param = new StateSkillParam();
			param.m_nSkillID = m_pSkillData.Id;
			param.m_fTotalTime = m_pSkillData.MSV_LastTime;
			param.m_fBreakTime = m_pSkillData.MSV_LockTime;
			return m_pOwner.SetFsmState(this, param);
		}

		public override bool IsLock()
		{
			return m_fLockTime > 0;
		}
		
		public override void ProcessEffect()
		{
			if(EffectCost())
			{
				base.ProcessEffect();
			}
		}

		protected bool EffectCost()
		{
			if(m_pSkillData == null)
				return false;
			
			//效果阶段消耗逻辑
			if(!m_bCosted && m_pSkillData.IsEffectStateCost())
			{
				Avatar pCaster = GetCaster();
				if(!pCaster)
				{
					Finish();
					return false;
				}
				if(!m_pOwner.SkillCom 
					|| !m_pOwner.SkillCom.CheckCost(m_pSkillData) 
					|| !m_pOwner.SkillCom.CheckCD(m_pSkillData))
				{
					Finish();
					return false;
				}

				m_pOwner.SkillCom.DoCost(m_pSkillData);
				m_pOwner.SkillCom.StartCD(m_pSkillData, false);
				SetCosted();
			}

			return true;
		}
	}

	/// <summary>
	/// 技能伤害帧
	/// </summary>
	public class GSkillSpellLogic_Branch : GSkillSpellLogic_Active
	{
		public override void Tick(float fFrameTime)
		{
			base.Tick(fFrameTime);
			TickEffect(fFrameTime);
		}
	}

	/// <summary>
	/// 吟唱技能
	/// </summary>
	public class GSkillSpellLogic_Channel : GSkillSpellLogic_Active
	{
		public int m_nChannelAnim;
		public float m_fChannelTime;
		public override bool Init(GSkillInitParam param)
		{
			if(!base.Init(param))
				return false;

			m_nChannelAnim = m_pSkillData.MSV_SpellParam1;
			m_fChannelTime = m_pSkillData.MSV_SpellParam2 / 1000f;
			m_fEffectTime += m_fChannelTime;
			return true;
		}

		public override bool ReInit()
		{
			if(!base.ReInit())
				return false;

			m_nChannelAnim = m_pSkillData.MSV_SpellParam1;
			m_fChannelTime = m_pSkillData.MSV_SpellParam2 / 1000f;
			m_fEffectTime += m_fChannelTime;
			return true;
		}

		public override void Tick(float fFrameTime)
		{
			base.Tick(fFrameTime);
			TickEffect(fFrameTime);
		}

		public override void Reset()
		{
			base.Reset();
			m_nChannelAnim = m_pSkillData.MSV_SpellParam1;
			m_fChannelTime = m_pSkillData.MSV_SpellParam2 / 1000f;
			m_fEffectTime += m_fChannelTime;
		}
	}
}
