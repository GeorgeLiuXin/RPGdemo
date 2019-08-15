using UnityEngine;

namespace Galaxy
{
    public static partial class Constant
    {
        /// <summary>
        /// 实体
        /// </summary>
        public static class Entity
        {
            public const string PlayerGroupName = "Player";
            public const string MonsterGroupName = "Monster";
        }

        /// <summary>
        /// AI
        /// </summary>
        public static class AI
        {
            public const float MinIdleLastTime = 3f;
            public const float MaxIdleLastTime = 10f;
            /// <summary>
            /// AI 空闲状态随机持续时间
            /// </summary>
            /// <returns></returns>
            public static float GetRandomIdleTime()
            {
                return Random.Range(MinIdleLastTime, MaxIdleLastTime);
            }

            public const float HangOutRange = 3f;
            public const float AISkillDefaultCommonRange = 2.5f;
        }
    }
}