using System;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    //todo 嘲讽值需要随时间逐渐下降，以防止越界
    public class Threat: IComparable
    {
        public int nAvatarID;
        public float fThreat;
        public float fValidTime;
        
        public int CompareTo(object obj)
        {
            Threat threat = obj as Threat;
            if (threat != null)
            {
                return CompareTo(threat);
            }

            return -1;
        }
        private int CompareTo(Threat rhs)
        {
            if (rhs != null && rhs != this && rhs.fThreat >= fThreat * 1.1f)
                return 1;
            return -1;
        }
    }

    public class ThreatComponent : ComponentBase
    {
        private float m_fTimer;

        protected Threat m_pTarget;

        protected List<Threat> m_ThreatList;
        private List<Threat> m_RemoveList;

        protected override void InitComponent()
        {
            m_ThreatList = new List<Threat>();
            m_RemoveList = new List<Threat>();

            m_pTarget = null;
            m_fTimer = Constant.Threat.ThreatRefreshTime;
        }

        public override void OnPreDestroy()
        {
            m_ThreatList.Clear();
            m_RemoveList.Clear();

            m_pTarget = null;
            m_fTimer = Constant.Threat.ThreatRefreshTime;
        }

        public void Update()
        {
            m_fTimer -= Time.deltaTime;
            if (m_fTimer > 0)
                return;

            m_fTimer += Constant.Threat.ThreatRefreshTime;
            //仇恨计时
            TickThreat(Time.deltaTime);
            //刷新目标
            TickTarget();
        }

        private void TickThreat(float fTime)
        {
            foreach (Threat pThreat in m_ThreatList)
            {
                if (pThreat == null)
                    continue;

                pThreat.fValidTime -= fTime;
                if (pThreat.fValidTime <= 0)
                {
                    if (pThreat == m_pTarget)
                    {
                        ResetTarget();
                    }
                    m_RemoveList.Add(pThreat);
                }
            }

            foreach (var threat in m_RemoveList)
            {
                m_ThreatList.Remove(threat);
            }

            m_ThreatList.Sort();
        }

        public void TickTarget()
        {
            //当前没有目标, 清空仇恨, 脱战
            if (m_ThreatList == null || m_ThreatList.Count == 0)
            {
                ResetTarget();
                Owner.LeaveCombat();
                return;
            }

            Threat pThreat = m_ThreatList[0];
            if (pThreat.CompareTo(m_pTarget) > 0)
            {
                SetTarget(pThreat);
            }
        }
        
        public void SetTarget(Threat pThreat)
        {
            m_pTarget = pThreat;
        }

        public void ResetTarget()
        {
            m_pTarget = null;
        }

        /// <summary>
        /// 被攻击后产生仇恨
        /// </summary>
        /// <param name="pAvatar"></param>
        /// <param name="fValue"></param>
        public void OnHurt(Avatar pAvatar, float fValue)
        {
            if (Owner != null)
            {
                AddThreat(pAvatar, fValue);
            }
        }

        /// <summary>
        /// 治疗后对当前攻击治疗目标的所有人产生仇恨
        /// </summary>
        /// <param name="pAvatar"></param>
        /// <param name="fValue"></param>
        public void OnHeal(Avatar pAvatar, float fValue)
        {
            if (Owner == null)
                return;

            foreach (Threat pThreat in m_ThreatList)
            {
                if (pThreat == null)
                    continue;
                Avatar avatar = GameEntry.Entity.GetGameEntity(pThreat.nAvatarID) as Avatar;
                if (avatar == null)
                    continue;

                avatar.AddThreat(pAvatar, fValue);
            }
        }

        public void OnTaunt(Avatar pAvatar, float fValue)
        {
            if (Owner == null)
                return;

            Threat pThreat = GetThreat(pAvatar.Id, true);
            if (pThreat == null)
                return;

            //受到嘲讽时，将第一名的加上去
            pThreat.fThreat += fValue;
            pThreat.fValidTime = Constant.Threat.LeaveCombatTimer;
            if (!(pThreat.CompareTo(m_pTarget) > 0))
            {
                float fMaxThreat = Mathf.Max(m_pTarget.fThreat, pThreat.fThreat);
                pThreat.fThreat += fMaxThreat;
            }
            SetTarget(pThreat);
        }

        public void AddThreat(Avatar pAvatar, float fValue)
        {
            if (Owner == null || pAvatar == null)
                return;
            if (Owner == pAvatar)
                return;

            Threat pThreat = GetThreat(pAvatar.Id, true);
            if (pThreat == null)
                return;

            if (fValue >= 0 || pThreat.fThreat <= 0)
            {
                pThreat.fThreat += fValue;
            }
            pThreat.fThreat = Mathf.Max(0, pThreat.fThreat);
            pThreat.fValidTime = Constant.Threat.LeaveCombatTimer;
            if ((pThreat.CompareTo(m_pTarget) > 0))
            {
                SetTarget(pThreat);
            }

            //进入战斗状态
            if (m_pTarget != null)
            {
                Owner.EnterCombat();
            }
        }

        private int m_nTempPredicateAvatar;
        public Threat GetThreat(int nAvatarID, bool bCreate)
        {
            m_nTempPredicateAvatar = nAvatarID;
            Threat pThreat = m_ThreatList.Find(GetThreat);
            if (pThreat != null || !bCreate)
                return pThreat;

            pThreat = new Threat();
            m_ThreatList.Add(pThreat);
            pThreat.nAvatarID = nAvatarID;
            pThreat.fThreat = 0;
            pThreat.fValidTime = Constant.Threat.LeaveCombatTimer;
            return pThreat;
        }

        private bool GetThreat(Threat obj)
        {
            return obj.nAvatarID == m_nTempPredicateAvatar;
        }
        
        public float GetTarget()
        {
            return m_pTarget.fThreat;
        }

        public int GetThreatByIndex(int id)
        {
            TickThreat(0);
            if (m_ThreatList == null || m_ThreatList.Count < id || m_ThreatList[id] == null)
                return 0;
            return m_ThreatList[id].nAvatarID;
        }

    }
}
