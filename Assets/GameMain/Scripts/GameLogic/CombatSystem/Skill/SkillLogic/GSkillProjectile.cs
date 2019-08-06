using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
	/// <summary>
	/// 技能子弹
	/// </summary>
	public class GSkillProjectile : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}

//namespace Galaxy
//{

//    FINISH_FACTORY_Arg0(GSkillProjectile);
//    GSkillProjectile::GSkillProjectile()
//		: m_bDestroy(false)
//		, m_fSpeed(0)
//		, m_nProjectileID(0)
//		, m_nCurTime(0)
//		, m_nLifeTime(0)
//		, m_nFirstEffectTime(0)
//		, m_nEffectTime(0)
//		, m_pSkillData(NULL)
//		, m_nFlyTime(0)
//        , m_PreCheckPos(0.0f, 0.0f, 0.0f)
//		, m_nCurveID(-1)
//    {

//    }

//    GSkillProjectile::~GSkillProjectile()
//    {
//        FACTORY_DELOBJ(m_pSkillData);
//    }
//    bool GSkillProjectile::Init(GSkillData* pSkillData, GSkillTargetInfo& sTarInfo, RoleAValue& sRoleValue, GNodeAvatar* pCast)
//    {
//        if (!pSkillData)
//            return false;
//        m_TargetInfo = sTarInfo;
//        //if (pCast && sTarInfo.m_nShapeID>=0)
//        //{
//        //	GNodeAvatar* pTar = pCast->GetSceneAvatar(sTarInfo.m_nTargetID);
//        //	if (pTar)
//        //	{
//        //		m_TargetInfo.m_vTarPos = pTar->GetCollisionShapePos(sTarInfo.m_nShapeID);
//        //	}
//        //}

//        f32 foff = (f32)pSkillData->GetIntValue(MSV_ProjectileParam1) / 10.0f;
//        Vector3 vOff;
//        vOff.x = GALAXY_RANDOM.RandFloat(-foff, foff);
//        vOff.y = GALAXY_RANDOM.RandFloat(-foff, foff);
//        vOff.z = 0;
//        m_TargetInfo.m_vTarPos += vOff;

//        m_pSkillData = pSkillData->Clone();
//        m_fSpeed = m_pSkillData->GetFloatValue(MSV_ProjectileSpeed);
//        m_nLifeTime = m_pSkillData->GetIntValue(MSV_ProjectileTime);
//        m_nFirstEffectTime = m_pSkillData->GetIntValue(MSV_ProjectileFirstEffectTime);
//        m_nEffectTime = m_nFirstEffectTime;
//        m_vExcludeList.m_nCount = m_pSkillData->GetIntValue(MSV_ProjectileEffectCount);
//        m_vExcludeList.m_bCount = (m_vExcludeList.m_nCount > 0);

//        int32 nProjectileOffset = m_pSkillData->GetIntValue(MSV_ProjectileOffset);
//        Vector3 vOffset = GSkillProjectileOffsetManager::Instance().GetProjectileOffset(nProjectileOffset);
//        m_TargetInfo.m_vSrcPos += vOffset;

//        //      float maxDis = m_fSpeed * (m_nLifeTime / 1000);
//        //      float randomRange = ((int)(maxDis / 3)) * 0.1;      //

//        ////tempcode
//        //      m_TargetInfo.m_vTarPos += sTarInfo.m_vOffset;
//        ////base noise
//        //m_TargetInfo.m_vTarPos.x += GALAXY_RANDOM.RandFloat(-randomRange, randomRange);
//        //m_TargetInfo.m_vTarPos.y += GALAXY_RANDOM.RandFloat(-randomRange, randomRange);
//        //m_TargetInfo.m_vTarPos.z += GALAXY_RANDOM.RandFloat(-randomRange, randomRange);

//        m_RoleValue.Copy(sRoleValue);

//        if (m_TargetInfo.m_vTarPos != m_TargetInfo.m_vSrcPos)
//            m_TargetInfo.m_vAimDir = m_TargetInfo.m_vTarPos - m_TargetInfo.m_vSrcPos;

//        m_TargetInfo.m_vAimDir.normalize();
//        m_PreCheckPos = m_TargetInfo.m_vSrcPos;
//        m_bTimeOut = false;

//        return true;
//    }

//    void GSkillProjectile::Tick(int32 nFrameTime, GNodeAvatar* pCaster)
//    {
//        if (IsDestroy())
//        {
//            return;
//        }

//        if (!pCaster || !m_pSkillData)
//        {
//            KillProjectile(pCaster);
//            return;
//        }

//        m_nCurTime += nFrameTime;
//        //目标是自己 立即造成效果
//        if (m_pSkillData->IsTargetAvatar() && pCaster->GetAvatarID() == m_TargetInfo.m_nTargetID)
//        {
//            HitEffect(pCaster);
//            return;
//        }

//        //移动
//        m_PreCheckPos = m_TargetInfo.m_vSrcPos;
//        TickMove(nFrameTime, pCaster);
//        if (IsDestroy())
//            return;

//        //周期效果
//        PeriodEffect(nFrameTime, pCaster);

//        m_nLifeTime -= nFrameTime;
//        if (m_nLifeTime <= 0)
//        {
//            m_bTimeOut = true;
//            OnTimeout(pCaster);
//            KillProjectile(pCaster);
//        }
//    }

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

//    void GSkillProjectile::OnTimeout(GNodeAvatar* pCaster)
//    {
//        if (IsDestroy())
//            return;

//        if (!m_pSkillData)
//            return;

//        if (m_pSkillData->IsBulletTimeOutEffect())
//        {
//            ProcessEffect(pCaster);
//        }
//    }

//    void GSkillProjectile::PeriodEffect(int32 nFrameTime, GNodeAvatar* pCaster)
//    {
//        if (IsDestroy())
//            return;

//        if (!m_pSkillData || !m_pSkillData->IsBulletPeriodEffect())
//            return;

//        m_nEffectTime -= nFrameTime;
//        if (m_nEffectTime > 0)
//            return;

//        m_nEffectTime += m_pSkillData->GetIntValue(MSV_ProjectileEffectTime);

//        ProcessEffect(pCaster);
//        //DrawPos(*pCaster, m_TargetInfo.m_vSrcPos);
//        //子弹产生周期事件
//        if (pCaster && pCaster->GetSkillComponent())
//        {
//            pCaster->GetSkillComponent()->PushTriggerNotify(m_pSkillData->m_nDataID, m_TargetInfo.m_nTargetID, NotifyType_Bullet, TriggerNotify_BulletEffect, 0, &m_TargetInfo.m_vSrcPos, &m_TargetInfo.m_vTarPos, &m_TargetInfo.m_vAimDir);
//        }

//        if (m_vExcludeList.m_bCount && m_vExcludeList.m_nCount <= 0)
//        {
//            KillProjectile(pCaster);
//        }
//    }

//    void GSkillProjectile::HitEffect(GNodeAvatar* pCaster)
//    {
//        if (IsDestroy())
//            return;

//        if (!m_pSkillData)
//            return;

//        if (m_pSkillData->IsBulletHitEffect())
//        {
//            ProcessEffect(pCaster);
//        }

//        //子弹产生命中事件
//        if (m_pSkillData->IsBulletNotify())
//        {
//            if (m_TargetInfo.m_nTargetID == -1)
//            {
//                if (pCaster && pCaster->GetSkillComponent())
//                {
//                    pCaster->GetSkillComponent()->PushTriggerNotify(m_pSkillData->m_nDataID, m_TargetInfo.m_nTargetID, NotifyType_Bullet, TriggerNotify_BulletHitEnv, 0, &m_TargetInfo.m_vSrcPos, &m_TargetInfo.m_vTarPos, &m_TargetInfo.m_vAimDir);
//                }
//            }
//            else
//            {
//                if (pCaster && pCaster->GetSkillComponent())
//                {
//                    pCaster->GetSkillComponent()->PushTriggerNotify(m_pSkillData->m_nDataID, m_TargetInfo.m_nTargetID, NotifyType_Bullet, TriggerNotify_BulletHit, 0, &m_TargetInfo.m_vSrcPos, &m_TargetInfo.m_vTarPos, &m_TargetInfo.m_vAimDir);
//                }
//            }
//        }

//        if (!m_pSkillData->IsBulletHitNoRemove())
//        {
//            KillProjectile(pCaster);
//        }
//    }

//    void GSkillProjectile::ProcessEffect(GNodeAvatar* pCaster)
//    {
//        if (pCaster && pCaster->GetSkillComponent())
//        {
//            pCaster->GetSkillComponent()->ProcessSkillEffect(m_pSkillData, m_TargetInfo, m_RoleValue, m_vExcludeList);
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

//#if _DEBUG
//		{
//			GPacketDrawLine pkt;
//			pkt.x1 = m_PreCheckPos.x;
//			pkt.y1 = m_PreCheckPos.y;
//			pkt.z1 = m_PreCheckPos.z;
//			pkt.x = pos.x;
//			pkt.y = pos.y;
//			pkt.z = pos.z;
//			pCaster->SendPacket(&pkt);
//		}
//#endif

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

//	void GSkillProjectile::DetonateProjectile(GNodeAvatar* pCaster)
//{
//    if (IsDestroy())
//        return;

//    if (!m_pSkillData)
//        return;

//    if (m_pSkillData->IsBulletHitEffect())
//    {
//        ProcessEffect(pCaster);
//    }
//    m_bTimeOut = false;
//    GSkillProjectile::KillProjectile(pCaster);
//}

//void GSkillProjectile::KillProjectile(GNodeAvatar* pCaster)
//{
//    if (IsDestroy())
//        return;

//    m_bDestroy = true;

//    //子弹产生死亡事件
//    if (m_pSkillData && m_pSkillData->IsBulletNotify() && pCaster && pCaster->GetSkillComponent())
//    {
//        pCaster->GetSkillComponent()->PushTriggerNotify(m_pSkillData->m_nDataID, m_TargetInfo.m_nTargetID, NotifyType_Bullet, TriggerNotify_BulletDead, 0, &m_TargetInfo.m_vSrcPos, &m_TargetInfo.m_vTarPos, &m_TargetInfo.m_vAimDir);
//    }
//}

//void GSkillProjectile::DrawPos(GNodeAvatar & Caster, const Vector3 & vec3)
//{
//# ifdef _DEBUG
//    {
//        GPacketDrawLine posPkt;
//        posPkt.x1 = vec3.x;
//        posPkt.y1 = vec3.y;
//        posPkt.z1 = vec3.z;
//        posPkt.x = m_PreCheckPos.x;
//        posPkt.y = m_PreCheckPos.y;
//        posPkt.z = m_PreCheckPos.z;
//        Caster.SendPacket(&posPkt);
//    }
//    if (!m_pSkillData)
//        return;
//    //if (SkillArea_Sphere == m_pSkillData->GetValue(MSV_AreaLogic))
//    //{
//    //    GPacketDrawSphere pktSphere;
//    //    f32 fRadius = m_pSkillData->GetValue(MSV_AreaParam1) / 10.0f;
//    //    if (fRadius < 0.01)
//    //        fRadius = 0.5;
//    //    pktSphere.radius = fRadius;
//    //    pktSphere.x = vec3.x;
//    //    pktSphere.y = vec3.y;
//    //    pktSphere.z = vec3.z;
//    //    pktSphere.r = 0;
//    //    pktSphere.g = 255;
//    //    pktSphere.b = 0;
//    //    Caster.SendPacket(&pktSphere);
//    //}
//    //if (SkillArea_Rect == m_pSkillData->GetValue(MSV_AreaLogic))
//    //{
//    //    GPacketDrawBox pktBox;
//    //    pktBox.x = vec3.x;
//    //    pktBox.y = vec3.y;
//    //    pktBox.z = vec3.z;
//    //    f32 minDis = m_pSkillData->GetValue(MSV_AreaParam1) / 10.0f;
//    //    f32 maxDis = m_pSkillData->GetValue(MSV_AreaParam2) / 10.0f;
//    //    Vector3 dir = m_TargetInfo.m_vAimDir;
//    //    Vector3 minPos = vec3 + dir * minDis;
//    //    Vector3 maxPos = vec3 + dir * maxDis;
//    //    f32 len = minPos.GetDistance(maxPos);
//    //    pktBox.dx = dir.x;
//    //    pktBox.dy = dir.y;
//    //    pktBox.dz = dir.z;
//    //    pktBox.l = len;
//    //    pktBox.w = m_pSkillData->GetValue(MSV_AreaParam3) / 10.0f;
//    //    pktBox.h = 3.0f;    //
//    //    Caster.SendPacket(&pktBox);
//    //}
//#endif // _DEBUG
//}

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
