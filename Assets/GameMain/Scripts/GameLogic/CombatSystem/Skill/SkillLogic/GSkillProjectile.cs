using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

namespace Galaxy
{
	/// <summary>
	/// 技能子弹
	/// </summary>
	public abstract class GSkillProjectile : IReference
	{
		protected int nProjectileSerial;
		protected bool m_bDestroy;
		protected float m_fSpeed;
		protected float m_fCurTime;
		protected float m_fLifeTime;
		protected float m_fCurEffectTime;
		protected float m_fFirstEffectTime;
		protected float m_fEffectTime;
		protected GTargetInfo m_TargetInfo;
		protected DRSkillData m_pSkillData;
		protected SkillAValueData m_SkillAValue;
		protected GSkillExcludeList m_vExcludeList;

		protected Vector3 m_vPreCheckPos;

		public bool IsDestroy()
		{
			return m_bDestroy;
		}


		public virtual bool Init(DRSkillData pSkillData, GTargetInfo sTarInfo, SkillAValueData sSkillAValue, Avatar pCaster)
		{
			if(pSkillData == null)
				return false;

			m_fCurTime = 0;
			m_pSkillData = pSkillData;
			m_TargetInfo = sTarInfo;
			m_fSpeed = m_pSkillData.MSV_ProjectileSpeed;
			m_fLifeTime = m_pSkillData.MSV_ProjectileTime;
			m_fFirstEffectTime = m_pSkillData.MSV_ProjectileFirstEffectTime;
			m_fEffectTime = m_pSkillData.MSV_EffectTime;
			m_fCurEffectTime = m_fFirstEffectTime;
			m_vExcludeList = new GSkillExcludeList();
			m_vExcludeList.m_nCount = m_pSkillData.MSV_ProjectileEffectCount;
			m_vExcludeList.m_bCount = (m_vExcludeList.m_nCount > 0);

			//int nProjectileOffset = m_pSkillData.MSV_ProjectileOffset;
			//Vector3 vOffset = GSkillProjectileOffsetManager.Instance.GetProjectileOffset(nProjectileOffset);
			//m_TargetInfo.m_vSrcPos += vOffset;
			m_TargetInfo.m_vSrcPos += new Vector3(0, 1, 0);

			m_SkillAValue = sSkillAValue.CloneData();

			m_vPreCheckPos = m_TargetInfo.m_vSrcPos;
			return true;
		}

		public void Tick(float fFrameTime, Avatar pCaster)
		{
			if(IsDestroy())
				return;
			if(pCaster == null || m_pSkillData == null)
			{
				KillProjectile(pCaster);
				return;
			}

			m_fCurTime += fFrameTime;
			//移动
			m_vPreCheckPos = m_TargetInfo.m_vSrcPos;
			TickMove(fFrameTime, pCaster);
			if(IsDestroy())
				return;

			//周期效果
			PeriodEffect(fFrameTime, pCaster);

			if(m_fCurTime >= m_fLifeTime)
			{
				KillProjectile(pCaster);
			}
		}

		public abstract void TickMove(float fFrameTime, Avatar pCaster);

		public void PeriodEffect(float fFrameTime, Avatar pCaster)
		{
			if(IsDestroy())
				return;

			if(m_pSkillData == null || !m_pSkillData.IsBulletPeriodEffect())
				return;

			m_fEffectTime -= fFrameTime;
			if(m_fEffectTime > 0)
				return;

			m_fEffectTime += m_pSkillData.MSV_ProjectileEffectTime;

			ProcessEffect(pCaster);
			//子弹产生周期事件
			if(pCaster && pCaster.SkillCom)
			{
				//todo
				//pCaster.SkillCom->PushTriggerNotify(m_pSkillData->m_nDataID, m_TargetInfo.m_nTargetID, NotifyType_Bullet, TriggerNotify_BulletEffect, 0, &m_TargetInfo.m_vSrcPos, &m_TargetInfo.m_vTarPos, &m_TargetInfo.m_vAimDir);
			}

			if(m_vExcludeList.m_bCount && m_vExcludeList.m_nCount <= 0)
			{
				KillProjectile(pCaster);
			}
		}

		private void ProcessEffect(Avatar pCaster)
		{
			if(pCaster == null || pCaster.SkillCom == null)
				return;
			pCaster.SkillCom.ProcessSkillEffect(m_pSkillData, m_TargetInfo, m_SkillAValue, m_vExcludeList);
		}

		private void HitEffect(Avatar pCaster)
		{
			if(IsDestroy())
				return;

			if(m_pSkillData == null)
				return;

			if(m_pSkillData.IsBulletHitEffect())
			{
				ProcessEffect(pCaster);
			}

			//子弹产生命中事件
			if(m_pSkillData.IsBulletNotify())
			{
				if(m_TargetInfo.m_nTargetID == -1)
				{
					if(pCaster && pCaster.SkillCom)
					{
						//todo
						//pCaster.SkillCom->PushTriggerNotify(m_pSkillData->m_nDataID, m_TargetInfo.m_nTargetID, NotifyType_Bullet, TriggerNotify_BulletHitEnv, 0, &m_TargetInfo.m_vSrcPos, &m_TargetInfo.m_vTarPos, &m_TargetInfo.m_vAimDir);
					}
				}
				else
				{
					if(pCaster && pCaster.SkillCom)
					{
						//todo
						//pCaster->GetSkillComponent()->PushTriggerNotify(m_pSkillData->m_nDataID, m_TargetInfo.m_nTargetID, NotifyType_Bullet, TriggerNotify_BulletHit, 0, &m_TargetInfo.m_vSrcPos, &m_TargetInfo.m_vTarPos, &m_TargetInfo.m_vAimDir);
					}
				}
			}

			if(!m_pSkillData.IsBulletHitNoRemove())
			{
				KillProjectile(pCaster);
			}
		}

		public void KillProjectile(Avatar pCaster)
		{
			if(IsDestroy())
				return;

			m_bDestroy = true;
			//子弹产生死亡事件
			if(m_pSkillData != null && m_pSkillData.IsBulletNotify() && pCaster && pCaster.SkillCom)
			{
				//todo
				//pCaster.SkillCom.PushTriggerNotify(m_pSkillData->m_nDataID, m_TargetInfo.m_nTargetID, NotifyType_Bullet, TriggerNotify_BulletDead, 0, &m_TargetInfo.m_vSrcPos, &m_TargetInfo.m_vTarPos, &m_TargetInfo.m_vAimDir);
			}
		}

		public void Clear()
		{
			m_fCurTime = 0;
			m_pSkillData = null;
			m_TargetInfo = null;
			m_fSpeed = 0;
			m_fLifeTime = 0;
			m_fFirstEffectTime = 0;
			m_fEffectTime = 0;
			m_fCurEffectTime = 0;
			m_vExcludeList = null;
			m_vExcludeList.m_nCount = 0;
			m_vExcludeList.m_bCount = false;

			m_SkillAValue = null;
			m_vPreCheckPos = default(Vector3);
		}
	}

	//追踪子弹
	public class GSkillProjectile_Track : GSkillProjectile
	{
		public override void TickMove(float fFrameTime, Avatar pCaster)
		{

		}
	}

	//陷阱子弹
	public class GSkillProjectile_Trap : GSkillProjectile
	{
		public override void TickMove(float fFrameTime, Avatar pCaster)
		{

		}
	}

}


//    void GSkillProjectile::TickMove(int32 nFrameTime, GNodeAvatar* pCaster)
//    {
//        if (!pCaster || !m_pSkillData)
//        {
//            KillProjectile(pCaster);
//            return;
//        }
//        f32 fLength = nFrameTime * m_fSpeed / 1000;
//        Vector3 dir = m_TargetInfo.m_vAimDir * fLength;
//        m_TargetInfo.m_vSrcPos += dir;
//        if (RayCastCheck(pCaster, m_TargetInfo.m_vSrcPos))
//        {
//            //DrawPos(*pCaster, m_TargetInfo.m_vSrcPos);
//            HitEffect(pCaster);
//            KillProjectile(pCaster);
//            return;
//        }
//    }

//    bool GSkillProjectile::RayCastCheck(GNodeAvatar* pCaster, Vector3& pos)
//    {
//        bool res = false;
//        //check arround avatar 
//        Vector3 vHitPos = pos;
//        int32 hitAvatarID = -1;
//        AOIList* pList = pCaster->GetArroundAvatar();
//        if (pList)
//        {
//            for (int32 i = 0; i < pList->GetCount(); ++i)
//            {
//                int32 avatarID = pList->GetIdByIndex(i);
//                if (avatarID == pCaster->GetAvatarID())
//                {
//                    continue;
//                }
//                GNodeAvatar* pTar = pCaster->GetSceneAvatar(avatarID);
//                if (!pTar || !pTar->GetCollisionComponent())
//                    continue;
//                if (!pCaster->CheckRelation(pTar, ToEnemy_Neutral))
//                    continue;
//                //check shape
//                SSphere sph;
//                //pTar->GetCollisionComponent()->OrderByDistance(pCaster->GetPos());
//                for (int32 a = 0; a < pTar->GetCollisionComponent()->GetShapeCount(); a++)
//                {
//                    if (!pTar->GetCollisionComponent()->GetShape(a, sph))
//                        continue;

//                    Vector3 sCenter(sph.x, sph.y, sph.z);
//            GServerCollider::Local2World(sCenter, pTar->GetPos(), pTar->GetDir());
//            sph.x = sCenter.x;
//            sph.y = sCenter.y;
//            sph.z = sCenter.z;
//            SSegment segment;
//            segment.r = 0.25;
//            segment.sx = m_PreCheckPos.x;
//            segment.sy = m_PreCheckPos.y;
//            segment.sz = m_PreCheckPos.z;

//            segment.ex = pos.x;
//            segment.ey = pos.y;
//            segment.ez = pos.z;

//            if (GServerCollider::CollideCheckLS(segment, sph))
//            {
//                hitAvatarID = avatarID;
//                Vector3 dir = Vector3(sph.x, sph.y, sph.z) - m_PreCheckPos;
//                f32 len = dir.GetLength() - sph.r + 0.2f;
//                dir = pos - m_PreCheckPos;
//                dir.SetLength(len);
//                vHitPos = m_PreCheckPos + dir;
//                break;
//            }
//        }
//    }
//}

//		if (pCaster->RayCast(m_PreCheckPos, pos))
//		{
//			if (hitAvatarID != -1)
//			{
//				f32 wDis = m_PreCheckPos.GetDistance(pos);
//f32 aDis = m_PreCheckPos.GetDistance(vHitPos);
//				if (wDis<aDis)
//				{
//					//raycast type projectile clear targetID,
//					m_TargetInfo.m_nTargetID = -1;
//					m_TargetInfo.m_vSrcPos = pos;
//					return true;
//				}
//				else
//				{
//					m_TargetInfo.m_nTargetID = hitAvatarID;
//					m_TargetInfo.m_vSrcPos = vHitPos;
//					return true;
//				}
//			}
//			else
//			{
//				//raycast type projectile clear targetID,
//				m_TargetInfo.m_nTargetID = -1;
//				m_TargetInfo.m_vSrcPos = pos;
//				return true;
//			}

//		}
//		if (hitAvatarID != -1)
//		{
//			m_TargetInfo.m_nTargetID = hitAvatarID;
//			m_TargetInfo.m_vSrcPos = vHitPos;
//			return true;
//		}

//		return false;
//	}

//    //////////////////////////////////////////////////////////////////////////
//    //追踪子弹
//    FINISH_FACTORY_Arg0(GSkillProjectile_Track);
//void GSkillProjectile_Track::TickMove(int32 nFrameTime, GNodeAvatar* pCaster)
//{
//    if (!pCaster || !m_pSkillData)
//    {
//        KillProjectile(pCaster);
//        return;
//    }

//    //是静止子弹不处理位移
//    if (m_fSpeed < 0 || FLOAT_EQUAL_ZERO(m_fSpeed))
//    {
//        return;
//    }

//    Vector3 vSrcPos = m_TargetInfo.m_vSrcPos;
//    Vector3 vTarPos = m_TargetInfo.m_vTarPos;

//    if (m_pSkillData->IsTargetAvatar())
//    {
//        GNodeAvatar* pTarget = pCaster->GetSceneAvatar(m_TargetInfo.m_nTargetID);
//        if (!pTarget)
//        {
//            KillProjectile(pCaster);
//            return;
//        }

//        vTarPos = pTarget->GetPos();
//        vTarPos.z += 1.0f;
//        if (pTarget->GetCollisionComponent())
//        {
//            SSphere sph;
//            if (pTarget->GetCollisionComponent()->GetShape(0, sph))
//            {
//                Vector3 sCenter(sph.x, sph.y, sph.z);
//                GServerCollider::Local2World(sCenter, pTarget->GetPos(), pTarget->GetDir());
//                vTarPos = sCenter;
//            }
//        }
//    }

//    if (m_pSkillData->IsTargetDir())
//    {
//        f32 fLength = nFrameTime * m_fSpeed / 1000;
//        Vector3 vDir = m_TargetInfo.m_vAimDir;
//        vDir.z = 0.0f; //方向类子弹锁定Z轴
//        vDir.Normalize();
//        vDir.SetLength(fLength);
//        m_TargetInfo.m_vSrcPos = vSrcPos + vDir;
//        m_TargetInfo.m_vTarPos = vTarPos;
//        RayCastCheck(pCaster, m_TargetInfo.m_vSrcPos);
//        DrawPos(*pCaster, m_TargetInfo.m_vSrcPos);
//        return;
//    }
//    else if (m_pSkillData->IsTargetAvatar() || m_pSkillData->IsTargetPos())
//    {
//        f32 fLength = nFrameTime * m_fSpeed / 1000;
//        f32 fDistance = vTarPos.GetDistance(vSrcPos);
//        if (fLength >= fDistance)
//        {
//            m_TargetInfo.m_vSrcPos = vTarPos;
//            m_TargetInfo.m_vTarPos = vTarPos;
//            RayCastCheck(pCaster, m_TargetInfo.m_vSrcPos);
//            DrawPos(*pCaster, m_TargetInfo.m_vSrcPos);
//            HitEffect(pCaster);
//            return;
//        }
//        else
//        {
//            Vector3 vDir = vTarPos - vSrcPos;
//            vDir.Normalize();
//            vDir.SetLength(fLength);
//            m_TargetInfo.m_vSrcPos = vSrcPos + vDir;
//            m_TargetInfo.m_vTarPos = vTarPos;
//            RayCastCheck(pCaster, m_TargetInfo.m_vSrcPos);
//            DrawPos(*pCaster, m_TargetInfo.m_vSrcPos);
//            return;
//        }
//    }
//    else
//    {
//        KillProjectile(pCaster);
//        return;
//    }
//}

//bool GSkillProjectile_Track::RayCastCheck(GNodeAvatar* pCaster, Vector3& pos)
//{
//    if (pCaster->RayCast(m_PreCheckPos, pos))
//    {
//        KillProjectile(pCaster);
//        return true;
//    }
//    m_PreCheckPos = pos;
//    return false;
//}

//    //////////////////////////////////////////////////////////////////////////
//    //陷阱子弹
//    FINISH_FACTORY_Arg0(GSkillProjectile_Trap);

//bool GSkillProjectile_Trap::Init(GSkillData* pSkillData, GSkillTargetInfo& sTarInfo, RoleAValue& sRoleValue, GNodeAvatar* pCast)
//{
//    if (!pSkillData)
//        return false;
//    m_TargetInfo = sTarInfo;

//    m_pSkillData = pSkillData->Clone();
//    m_RoleValue.Copy(sRoleValue);

//    Vector3 pos = sTarInfo.m_vSrcPos;
//    pos.z = pCast->GetSceneHeight(pos);
//    pos.z += 0.2f;
//    m_TargetInfo.m_vSrcPos = pos;

//    //m_TargetInfo.m_vAimDir = m_TargetInfo.m_vTarPos - m_TargetInfo.m_vSrcPos;
//    m_TargetInfo.m_vAimDir.normalize();
//    m_fSpeed = m_pSkillData->GetFloatValue(MSV_ProjectileSpeed);
//    m_nLifeTime = m_pSkillData->GetIntValue(MSV_ProjectileTime);
//    m_nFirstEffectTime = m_pSkillData->GetIntValue(MSV_ProjectileFirstEffectTime);
//    m_nEffectTime = m_nFirstEffectTime;
//    m_vExcludeList.m_nCount = m_pSkillData->GetIntValue(MSV_ProjectileEffectCount);
//    m_vExcludeList.m_bCount = (m_vExcludeList.m_nCount > 0);
//    m_PreCheckPos = m_TargetInfo.m_vSrcPos;
//    m_bTimeOut = false;

//    return true;
//}

//void GSkillProjectile_Trap::TickMove(int32 nFrameTime, GNodeAvatar* pCaster)
//{
//    if (!pCaster || !m_pSkillData)
//    {
//        KillProjectile(pCaster);
//        return;
//    }
//    //DrawPos(*pCaster, m_TargetInfo.m_vSrcPos);  //就先暴力一些吧
//}
