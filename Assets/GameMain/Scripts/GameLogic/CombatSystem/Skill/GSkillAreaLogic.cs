using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
	/// <summary>
	/// SkillArea_Singleton = 1, //单体
	/// SkillArea_Sphere = 2, //球形范围
	/// SkillArea_Sector = 3, //扇形范围
	/// 参数1：最近边距自己的距离，参数2：半径，参数3：弧度
	/// SkillArea_Ring = 4, //环形范围
	/// SkillArea_Rect = 5, //矩形范围
	/// 参数1：最近边距自己的距离，参数2：最远边距自己的距离，参数3：宽度
	/// </summary>
	public abstract class GSkillAreaLogic
	{
		public abstract List<Avatar> GetTargetList(DRSkillData pSkillData,
			Avatar pCaster, GTargetInfo sTarInfo, HashSet<int> vExcludeList);

		protected bool TryAddTarget(DRSkillData pSkillData, Avatar pCaster,
			Avatar pTarget, HashSet<int> vExcludeList)
		{
			if(pSkillData == null || !pCaster || !pTarget)
				return false;

			if(vExcludeList.Contains(pTarget.Id))
				return false;

			//int nTarCheck = pSkillData->GetValue(MSV_AreaTarCheck);
			//if (nTarCheck > 0)
			//{
			//	SDConditionParamAvatar sParam;
			//	sParam.ParamAvatar = pCaster;
			//	if (!GSKillConditionCheckManager::Instance().Check(nTarCheck, pTarget, &sParam))
			//		return false;
			//}

			//if (pSkillData->IsAreaAddExclude())
			//{
			//	vExcludeList.insert(pTarget.GetAvatarID());
			//}
			return true;
		}

		protected int GetTargetCount(DRSkillData pSkillData/*, GSkillExcludeList& vExcludeList*/)
		{
			if(pSkillData == null)
				return 0;

			//int nCount = pSkillData.MSV_AreaTarCnt;
			//if(vExcludeList.m_bCount)
			//	nCount = MIN(vExcludeList.m_nCount, nCount);
			//return nCount;
			return pSkillData.MSV_AreaTarCnt;
		}

		protected void UpdateAreaFilter(int nFilter, int nCount, List<Avatar> vTargetList)
		{
			if(vTargetList.Count <= nCount)
				return;

			switch(nFilter)
			{
				case (int)eAreaFilter.AreaFilter_MinHp:
					UpdateAreaFilterMinHp(vTargetList);
					break;
			}
		}

		protected void UpdateAreaFilterMinHp(List<Avatar> vTargetList)
		{
			Avatar pAvatar = null;
			foreach(var item in vTargetList)
			{
				if(pAvatar == null || item.HPRatio > pAvatar.HPRatio)
				{
					pAvatar = item;
				}
			}
			if(pAvatar)
			{
				vTargetList.Remove(pAvatar);
			}
		}
	}

	public class GSkillAreaSingelton : GSkillAreaLogic
	{
		public override List<Avatar> GetTargetList(DRSkillData pSkillData,
			Avatar pCaster, GTargetInfo sTarInfo, HashSet<int> vExcludeList)
		{
			List<Avatar> vTargetList = new List<Avatar>();
			if(pSkillData == null || !pCaster)
				return vTargetList;

			Avatar pTarget = GameEntry.Entity.GetGameEntity(sTarInfo.m_nTargetID) as Avatar;
			if(!pTarget)
				return vTargetList;

			if(!GCollider.SingletonCollideCheck())
				return vTargetList;

			TryAddTarget(pSkillData, pCaster, pTarget, vExcludeList);
			vTargetList.Add(pTarget);
			return vTargetList;
		}
	}

	public class GSkillAreaSphere : GSkillAreaLogic
	{
		public override List<Avatar> GetTargetList(DRSkillData pSkillData,
			Avatar pCaster, GTargetInfo sTarInfo, HashSet<int> vExcludeList)
		{
			List<Avatar> vTargetList = new List<Avatar>();
			if(pSkillData == null || !pCaster)
				return vTargetList;
			Avatar pTarget = GameEntry.Entity.GetGameEntity(sTarInfo.m_nTargetID) as Avatar;
			if(!pTarget)
			{
				pTarget = pCaster;
			}

			int cnt = GetTargetCount(pSkillData/*, vExcludeList*/);
			int filter = pSkillData.MSV_AreaFilter;
			float r = pSkillData.MSV_AreaParam1;
			float dis = pSkillData.MSV_AreaParam2;

			List<UnityGameFramework.Runtime.Entity> pList = new List<UnityGameFramework.Runtime.Entity>();
			GameEntry.Entity.GetAllLoadedEntities(pList);
			if(pList == null || pList.Count == 0)
			{
				return vTargetList;
			}
			//temp
			Vector3 pos = /*pSkillData.IsAreaUseTarPos() ? sTarInfo.m_vTarPos :*/ sTarInfo.m_vSrcPos;
			Vector3 dir = sTarInfo.m_vAimDir;
			
			Vector3 spPos = pos + dir * dis;
			SSphere sSphere = new SSphere(spPos, r);
			foreach(var item in pList)
			{
				Avatar actor = item.Logic as Avatar;
				if(actor == null || actor == pCaster)
					continue;
				SSphere sTarSphere = new SSphere(actor.GetPos(), actor.ModelRadius);
				if(GCollider.SphereCollideCheck(sSphere, sTarSphere, dir))
				{
					TryAddTarget(pSkillData, pCaster, actor, vExcludeList);
					if(vTargetList.Count >= cnt)
					{
						if(filter > 0)
							UpdateAreaFilter(filter, cnt, vTargetList);
						else
							return vTargetList;
					}
					break;
				}
			}
			return vTargetList;
		}
	}

	public class GSkillAreaSector : GSkillAreaLogic
	{
		public override List<Avatar> GetTargetList(DRSkillData pSkillData,
			Avatar pCaster, GTargetInfo sTarInfo, HashSet<int> vExcludeList)
		{
			List<Avatar> vTargetList = new List<Avatar>();
			if(pSkillData == null || !pCaster)
				return vTargetList;
			Avatar pTarget = GameEntry.Entity.GetGameEntity(sTarInfo.m_nTargetID) as Avatar;
			if(!pTarget)
			{
				pTarget = pCaster;
			}

			int cnt = GetTargetCount(pSkillData/*, vExcludeList*/);
			int filter = pSkillData.MSV_AreaFilter;
			float minDis = pSkillData.MSV_AreaParam1;
			float maxDis = pSkillData.MSV_AreaParam2;
			float angle = pSkillData.MSV_AreaParam3;

			List<UnityGameFramework.Runtime.Entity> pList = new List<UnityGameFramework.Runtime.Entity>();
			GameEntry.Entity.GetAllLoadedEntities(pList);
			if(pList == null || pList.Count == 0)
			{
				return vTargetList;
			}
			Vector3 pos = /*pSkillData.IsAreaUseTarPos() ? sTarInfo.m_vTarPos :*/ sTarInfo.m_vSrcPos;
			Vector3 dir = sTarInfo.m_vAimDir;

			SSector sSector = new SSector(pos, dir, maxDis, minDis, angle);
			foreach(var item in pList)
			{
				Avatar actor = item.Logic as Avatar;
				if(actor == null || actor == pCaster)
					continue;
				SSphere sTarSphere = new SSphere(actor.GetPos(), actor.ModelRadius);
				if(GCollider.SectorCollideCheck(sSector, sTarSphere, dir))
				{
					TryAddTarget(pSkillData, pCaster, actor, vExcludeList);
					if(vTargetList.Count >= cnt)
					{
						if(filter > 0)
							UpdateAreaFilter(filter, cnt, vTargetList);
						else
							return vTargetList;
					}
					break;
				}
			}
			return vTargetList;
		}
	}

	public class GSkillAreaRing : GSkillAreaLogic
	{
		public override List<Avatar> GetTargetList(DRSkillData pSkillData,
			Avatar pCaster, GTargetInfo sTarInfo, HashSet<int> vExcludeList)
		{
			List<Avatar> vTargetList = new List<Avatar>();
			if(pSkillData == null || !pCaster)
				return vTargetList;
			Avatar pTarget = GameEntry.Entity.GetGameEntity(sTarInfo.m_nTargetID) as Avatar;
			if(!pTarget)
			{
				pTarget = pCaster;
			}

			int cnt = GetTargetCount(pSkillData/*, vExcludeList*/);
			int filter = pSkillData.MSV_AreaFilter;
			float rMin = pSkillData.MSV_AreaParam1;
			float rMax = pSkillData.MSV_AreaParam2;
			float dis = pSkillData.MSV_AreaParam3;

			List<UnityGameFramework.Runtime.Entity> pList = new List<UnityGameFramework.Runtime.Entity>();
			GameEntry.Entity.GetAllLoadedEntities(pList);
			if(pList == null || pList.Count == 0)
			{
				return vTargetList;
			}
			Vector3 pos = /*pSkillData.IsAreaUseTarPos() ? sTarInfo.m_vTarPos :*/ sTarInfo.m_vSrcPos;
			Vector3 dir = sTarInfo.m_vAimDir;

			SRing sRing = new SRing(pos, rMax, rMin);
			foreach(var item in pList)
			{
				Avatar actor = item.Logic as Avatar;
				if(actor == null || actor == pCaster)
					continue;
				SSphere sTarSphere = new SSphere(actor.GetPos(), actor.ModelRadius);
				if(GCollider.RingCollideCheck(sRing, sTarSphere, dir))
				{
					TryAddTarget(pSkillData, pCaster, actor, vExcludeList);
					if(vTargetList.Count >= cnt)
					{
						if(filter > 0)
							UpdateAreaFilter(filter, cnt, vTargetList);
						else
							return vTargetList;
					}
					break;
				}
			}
			return vTargetList;
		}
	}

	public class GSkillAreaRect : GSkillAreaLogic
	{
		public override List<Avatar> GetTargetList(DRSkillData pSkillData,
			Avatar pCaster, GTargetInfo sTarInfo, HashSet<int> vExcludeList)
		{
			List<Avatar> vTargetList = new List<Avatar>();
			if(pSkillData == null || !pCaster)
				return vTargetList;
			Avatar pTarget = GameEntry.Entity.GetGameEntity(sTarInfo.m_nTargetID) as Avatar;
			if(!pTarget)
			{
				pTarget = pCaster;
			}

			int cnt = GetTargetCount(pSkillData/*, vExcludeList*/);
			int filter = pSkillData.MSV_AreaFilter;
			float minDis = pSkillData.MSV_AreaParam1;
			float maxDis = pSkillData.MSV_AreaParam2;
			float w = pSkillData.MSV_AreaParam3;

			List<UnityGameFramework.Runtime.Entity> pList = new List<UnityGameFramework.Runtime.Entity>();
			GameEntry.Entity.GetAllLoadedEntities(pList);
			if(pList == null || pList.Count == 0)
			{
				return vTargetList;
			}
			Vector3 pos = /*pSkillData.IsAreaUseTarPos() ? sTarInfo.m_vTarPos :*/ sTarInfo.m_vSrcPos;
			Vector3 dir = sTarInfo.m_vAimDir;
			Vector3 center = pos + dir * ((minDis + maxDis) / 2);

			SRect sRect = new SRect(center, maxDis - minDis, w);
			foreach(var item in pList)
			{
				Avatar actor = item.Logic as Avatar;
				if(actor == null || actor == pCaster)
					continue;
				SSphere sTarSphere = new SSphere(actor.GetPos(), actor.ModelRadius);
				if(GCollider.RectCollideCheck(sRect, sTarSphere, dir))
				{
					TryAddTarget(pSkillData, pCaster, actor, vExcludeList);
					if(vTargetList.Count >= cnt)
					{
						if(filter > 0)
							UpdateAreaFilter(filter, cnt, vTargetList);
						else
							return vTargetList;
					}
					break;
				}
			}
			return vTargetList;
		}
	}
}