using GameFramework.Entity;
using UnityEngine;

namespace Galaxy
{
    /// <summary>
    /// 选敌辅助器
    /// </summary>
    public interface IAimHelper
    {
        int AimAssist(DRSkillData pSkillData, Avatar pOwner);
    }
    
    public abstract class AimAssistBase : IAimHelper
    {
        // 最远距离
        protected const float fMaxScanDis = 10.0f;
        protected float GetMaxRange(DRSkillData pSkillData)
        {
            return (pSkillData == null ? fMaxScanDis : pSkillData.MSV_Range);
        }

        /// <summary>
        /// 暂时只有玩家与敌方两个阵营
        /// </summary>
        /// <param name="selfCamp"></param>
        /// <returns></returns>
        protected IEntity[] GetEnemyGroup(CampType selfCamp)
        {
            IEntityGroup group;
            switch (selfCamp)
            {
                case CampType.Player:
                    group = GameEntry.Entity.GetEntityGroup(Constant.Entity.MonsterGroupName);
                    return group.GetAllEntities();
                case CampType.Enemy:
                    group = GameEntry.Entity.GetEntityGroup(Constant.Entity.PlayerGroupName);
                    return group.GetAllEntities();
                case CampType.Neutral:
                    return null;
            }
            return null;
        }

        public abstract int AimAssist(DRSkillData pSkillData, Avatar pOwner);
    }

    public class AimAssistNormal : AimAssistBase
    {
        public override int AimAssist(DRSkillData pSkillData, Avatar pOwner)
        {
            if (pOwner == null)
                return 0;
            Vector3 vPos = pOwner.GetPos();
            Vector3 vDir = GameEntry.CameraMgr.GetCamDir().normalized2d();

            float fMinActorWeight = 3600000;
            int nAvatarID = 0;
            
            IEntity[] list = GetEnemyGroup(pOwner.Camp);
            if (list == null || list.Length == 0)
                return 0;

            foreach (IEntity item in list)
            {
                Avatar actor = GameEntry.Entity.GetGameEntity(item.Id) as Avatar;
                if (actor == null || actor.IsDead)
                    continue;

                if (AIUtility.GetRelation(pOwner.Camp, actor.Camp) != RelationType.Hostile)
                    continue;

                Vector3 vOffestPos = actor.GetPos() - vPos;
                vOffestPos.y = 0;
                float fMagnitude = vOffestPos.magnitude + 0.5f;

                // 排除最远距离外的目标
                if (fMagnitude > GetMaxRange(pSkillData))
                    continue;

                //计算各个角度的权重  以距离为权重
                float fCurActorWeight;
                float angle = Vector3.Angle(vDir, vOffestPos);
                if (angle <= 45f)
                {
                    fCurActorWeight = 9000 + fMagnitude;
                }
                else if (angle > 45f && angle < 90f)
                {
                    fCurActorWeight = 18000 + fMagnitude;
                }
                else
                {
                    fCurActorWeight = 36000 + fMagnitude;
                }

                if (fCurActorWeight < fMinActorWeight)
                {
                    fMinActorWeight = fCurActorWeight;
                    nAvatarID = actor.Id;
                }
            }
            return nAvatarID != 0 ? nAvatarID : 0;
        }
    }
}