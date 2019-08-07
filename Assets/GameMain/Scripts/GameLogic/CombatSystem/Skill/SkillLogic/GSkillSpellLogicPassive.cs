using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
	/// <summary>
	/// 被动技能
	/// </summary>
	public class GSkillSpellLogic_Passive : GSkillSpellLogic
	{
		//记录挂载自己的buff
		//public GBuff m_pBuff;

		public override bool Init(GSkillInitParam param)
		{
			if(!base.Init(param))
				return false;

			//m_pBuff = param.pBuff;
			return true;
		}
		
		protected bool PassiveProcessCheck()
		{
			if(!m_pOwner || m_pSkillData == null)
				return false;

			Avatar pCaster = GetCaster();
			if(!pCaster)
				return false;

			////自身检查 todo 添加条件检查库
			//int32 nSrvCheck = m_pSkillData.MSV_SrcCheck;
			//if(nSrvCheck > 0)
			//{
			//	if(!GSKillConditionCheckManager::Instance().Check(nSrvCheck, pCaster))
			//		return false;
			//}

			//目标检查
			if(!GSkillLogicManager.Instance.CheckTarget(m_pSkillData, pCaster, m_TargetInfo))
			{
				return false;
			}
			return true;
		}
	}

	/// <summary>
	/// Effect of time (dot/hot)
	/// </summary>
	public class GSkillSpellLogic_Eot : GSkillSpellLogic_Passive
	{
		public bool m_bTick;

		protected int m_nCurEffectCount;
		protected float m_fCurEffectTime;
		protected int m_nEffectCount;
		protected float m_fEffectTime;
		protected float m_fFirstEffectTime;

		public GSkillSpellLogic_Eot()
		{
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
			m_bTick = true;
			return true;
		}

		public override void Tick(float fFrameTime)
		{
			base.Tick(fFrameTime);
			if(m_pSkillData == null || !m_bTick)
				return;

			m_fCurEffectTime += fFrameTime;
			if(m_fCurEffectTime >= m_fEffectTime)
			{
				m_fCurEffectTime -= m_fEffectTime;
				if(PassiveProcessCheck())
				{
					m_TargetInfo.m_vSrcPos = m_pOwner.GetPos();
					m_TargetInfo.m_vAimDir = m_pOwner.GetDir();
					ProcessEffect();
				}

				if(m_pSkillData.MSV_EffectCount > 0)
				{
					++m_nCurEffectCount;
					m_bTick = (m_nEffectCount > m_nCurEffectCount);
				}
			}
		}

		public override void Reset()
		{
			base.Reset();
			m_nEffectCount = m_pSkillData.MSV_EffectCount;
			m_fEffectTime = m_pSkillData.MSV_EffectTime;
			m_fFirstEffectTime = m_pSkillData.MSV_FirstEffectTime;

			m_nCurEffectCount = 0;
			m_fCurEffectTime = m_fEffectTime - m_fFirstEffectTime;
		}
	}

	/// <summary>
	/// 被动技能属性集合并
	/// </summary>
	public class GSkillSpellLogic_AValue : GSkillSpellLogic_Passive
	{

	}
}