using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy
{
    public class Monster : Avatar
    {
        [SerializeField]
        private MonsterData m_MonsterData;

        private AIComponent m_aiCom;
        public AIComponent AICom
        {
            get
            {
                if (m_aiCom == null)
                {
                    m_aiCom = GetComponent<AIComponent>();
                }
                return m_aiCom;
            }
        }

        protected override void InitComponent()
        {
            base.InitComponent();

            gameObject.AddComponent<AIComponent>().SetOwner(this);
        }

        public Vector3 GetSpawnPos()
        {
            if (m_MonsterData==null)
            {
                Log.Error("Current Monster '{0}': don't have self MonsterData.", Id);
                return GetPos();
            }
            return m_MonsterData.info.vPos;
        }

        public MonsterData GetMonsterData()
        {
            return m_MonsterData;
        }
    }
}
