using GameFramework;
using GameFramework.Fsm;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy
{
    /// <summary>
    /// 暂时仅仅使用非常简单的状态机流转 之后改成行为树控制AI行为
    /// </summary>
	public partial class AIComponent : ComponentBase
    {
        public IFsm<Monster> m_aifsm
        {
            get;
            private set;
        }

        public Monster pMonster
        {
            get;
            private set;
        }

        private int m_nNextSkill;
        private int m_nNextIndex;
        private List<int> m_SkillList;
        
        public override void OnComponentReadyToStart()
        {
            pMonster = Owner as Monster;
            if (pMonster == null)
            {
                Log.Error("'{0}' : 当前AICom挂载在了错误的actor上!", Owner.Id);
                return;
            }

            m_aifsm = GameEntry.Fsm.CreateFsm(Utility.Text.GetFullName(GetType(), pMonster.Id.ToString())
                , pMonster, GameEntry.fsmMgr.GetAllFsmState<Monster>());
            m_aifsm.Start<AIState_Idle>();
        }

        public override void OnComponentStart()
        {
            m_SkillList = new List<int>();
            AddAISkill();
            if (m_SkillList != null && m_SkillList.Count > 0)
            {
                m_nNextSkill = m_SkillList[0];
            }
        }

        private void AddAISkill()
        {
            MonsterData data = pMonster.GetMonsterData();
            if (data == null)
                return;

            DRMonster config = GameEntry.DataTable.GetDataTable<DRMonster>().GetDataRow(data.info.nMonsterID);
            if (config == null)
                return;

            for (int i = 0; i < config.AISkillCount; i++)
            {
                int nSkillID = config.GetAISkillAt(i);
                if (nSkillID == 0)
                    return;
                Owner.SkillCom.AddSkill(nSkillID);
                m_SkillList.Add(nSkillID);
            }

            m_nNextIndex = 0;
        }

		public void OnDead()
		{
			ResetData();
		}

        public override void OnPreDestroy()
		{
			ResetData();
		}

		private void ResetData()
		{
			if(m_aifsm != null)
			{
				GameEntry.Fsm.DestroyFsm(m_aifsm);
			}
			if(m_SkillList != null)
			{
				m_SkillList.Clear();
			}
		}

		public void Update()
        {

        }

        private void GetNextSkill()
        {
            float fProbalitity = Random.Range(0f, 1f);
            if (fProbalitity >= 0.8f)
            {
                int index = m_SkillList.Count;
                index = index > 1 ? index - 1 : index;
                m_nNextSkill = m_SkillList[index];
                return;
            }

            m_nNextIndex++;
            m_nNextIndex %= m_SkillList.Count;
            m_nNextSkill = m_SkillList[m_nNextIndex];
        }

        public float GetNextSkillRange()
        {
            float fSkillRange = Constant.AI.AISkillDefaultCommonRange;
            DRSkillData pSkillData = pMonster.SkillCom.GetSkillData(m_nNextSkill);
            if (pSkillData != null)
            {
                fSkillRange = pSkillData.MSV_Range / 2f;
            }

            return fSkillRange;
        }

        public bool CanSpellSkill()
        {
            if (pMonster == null)
                return false;
            
            int nTarget = pMonster.ThreatCom.GetTarget();
            Avatar pTarget = GameEntry.Entity.GetGameEntity(nTarget) as Avatar;
            if (pTarget == null)
                return false;

            return pTarget.GetPos().Distance2D(pMonster.GetPos()) <= GetNextSkillRange();
        }

        public bool SpellSkill()
        {
            if (Owner == null || Owner.SkillCom == null)
                return false;
            if (!CanSpellSkill())
                return false;

            int nTarget = pMonster.ThreatCom.GetTarget();
            Avatar pTarget = GameEntry.Entity.GetGameEntity(nTarget) as Avatar;
            if (pTarget == null)
                return false;

            GTargetInfo sTarInfo = new GTargetInfo
            {
                m_nTargetID = pTarget.Id,
                m_vSrcPos = Owner.GetPos(),
                m_vTarPos = pTarget.GetPos(),
                m_vAimDir = (pTarget.GetPos() - Owner.GetPos()).normalized2d()
            };

            bool bResult = Owner.SkillCom.SpellSkill(m_nNextSkill, sTarInfo);
            GetNextSkill();
            return bResult;
        }
    }
}