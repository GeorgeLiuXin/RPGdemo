//using UnityEngine;
//using System.Collections;
//using System;
//using System.Collections.Generic;

//namespace Galaxy
//{
//	public class GColliderManager : GalaxyGameManagerBase
//	{
//		protected GalaxyActorManager m_actorMgr;
//		public GalaxyActorManager ActorMgr
//		{
//			get
//			{
//				if(m_actorMgr == null)
//				{
//					m_actorMgr = GalaxyGameModule.GetGameManager<GalaxyActorManager>();
//				}
//				return m_actorMgr;
//			}
//		}

//		public List<GalaxyActor> CollideCheck(SSphere sSphere, Vector3 vDir)
//		{
//			List<GalaxyActor> pActorList = ActorMgr.GetAllActor();
//			if(pActorList == null || pActorList.Count <= 0)
//				return null;
//			List<GalaxyActor> list = new List<GalaxyActor>();
//			foreach(GalaxyActor actor in pActorList)
//			{
//				if(ActorMgr.IsLocalPlayer(actor.ServerID))
//					continue;
//				ActorObj obj = (ActorObj)actor;
//				if(obj.CollisionCom == null)
//				{
//					continue;
//				}
//				for(int i = 0; i < obj.CollisionCom.GetShapeCount(); i++)
//				{
//					SShapeData shp = obj.CollisionCom.GetShape(i);
//					if(shp != null)
//					{
//						SSphere sTarSphere = new SSphere(shp.Pos, shp.r);
//						if(GCollider.SphereCollideCheck(sSphere, sTarSphere, vDir))
//						{
//							list.Add(actor);
//							break;
//						}
//					}
//				}
//			}
//			return list;
//		}
//		public List<GalaxyActor> CollideCheck(SRect sRect, Vector3 vDir)
//		{
//			List<GalaxyActor> pActorList = ActorMgr.GetAllActor();
//			if(pActorList == null || pActorList.Count <= 0)
//				return null;
//			List<GalaxyActor> list = new List<GalaxyActor>();
//			foreach(GalaxyActor actor in pActorList)
//			{
//				if(ActorMgr.IsLocalPlayer(actor.ServerID))
//					continue;
//				ActorObj obj = (ActorObj)actor;
//				if(obj.CollisionCom == null)
//				{
//					continue;
//				}
//				for(int i = 0; i < obj.CollisionCom.GetShapeCount(); i++)
//				{
//					SShapeData shp = obj.CollisionCom.GetShape(i);
//					if(shp != null)
//					{
//						SSphere sTarSphere = new SSphere(shp.Pos, shp.r);
//						if(GCollider.RectCollideCheck(sRect, sTarSphere, vDir))
//						{
//							list.Add(actor);
//							break;
//						}
//					}
//				}
//			}
//			return list;
//		}
//		public List<GalaxyActor> CollideCheck(SSector sSector, Vector3 vDir)
//		{
//			List<GalaxyActor> pActorList = ActorMgr.GetAllActor();
//			if(pActorList == null || pActorList.Count <= 0)
//				return null;
//			List<GalaxyActor> list = new List<GalaxyActor>();
//			foreach(GalaxyActor actor in pActorList)
//			{
//				if(ActorMgr.IsLocalPlayer(actor.ServerID))
//					continue;
//				ActorObj obj = (ActorObj)actor;
//				if(obj.CollisionCom == null)
//				{
//					continue;
//				}
//				for(int i = 0; i < obj.CollisionCom.GetShapeCount(); i++)
//				{
//					SShapeData shp = obj.CollisionCom.GetShape(i);
//					if(shp != null)
//					{
//						SSphere sTarSphere = new SSphere(shp.Pos, shp.r);
//						if(GCollider.SectorCollideCheck(sSector, sTarSphere, vDir))
//						{
//							list.Add(actor);
//							break;
//						}
//					}
//				}

//			}
//			return list;
//		}
//		public List<GalaxyActor> CollideCheck(SRing sRing, Vector3 vDir)
//		{
//			List<GalaxyActor> pActorList = ActorMgr.GetAllActor();
//			if(pActorList == null || pActorList.Count <= 0)
//				return null;
//			List<GalaxyActor> list = new List<GalaxyActor>();
//			foreach(GalaxyActor actor in pActorList)
//			{
//				if(ActorMgr.IsLocalPlayer(actor.ServerID))
//					continue;
//				ActorObj obj = (ActorObj)actor;
//				if(obj.CollisionCom == null)
//				{
//					continue;
//				}
//				for(int i = 0; i < obj.CollisionCom.GetShapeCount(); i++)
//				{
//					SShapeData shp = obj.CollisionCom.GetShape(i);
//					if(shp != null)
//					{
//						SSphere sTarSphere = new SSphere(shp.Pos, shp.r);
//						if(GCollider.RingCollideCheck(sRing, sTarSphere, vDir))
//						{
//							list.Add(actor);
//							break;
//						}
//					}
//				}
//			}
//			return list;
//		}


//		private Dictionary<int, GSkillAreaLogic> m_vAreaLogicDict;

//		public override void InitManager()
//		{
//			m_vAreaLogicDict = new Dictionary<int, GSkillAreaLogic>();
//			m_vAreaLogicDict.Add((int)eSkillAreaLogic.SkillArea_Singleton, new GSkillAreaSingelton());
//			m_vAreaLogicDict.Add((int)eSkillAreaLogic.SkillArea_Sphere, new GSkillAreaSphere());
//			m_vAreaLogicDict.Add((int)eSkillAreaLogic.SkillArea_Sector, new GSkillAreaSector());
//			m_vAreaLogicDict.Add((int)eSkillAreaLogic.SkillArea_Ring, new GSkillAreaRing());
//			m_vAreaLogicDict.Add((int)eSkillAreaLogic.SkillArea_Rect, new GSkillAreaRect());
//		}

//		public override void ShutDown()
//		{
//			m_vAreaLogicDict.Clear();
//			m_vAreaLogicDict = null;
//		}

//		public override void Update(float fElapseTimes)
//		{

//		}

//		public List<GalaxyActor> CalculationHit(GSkillData pSkillData, GTargetInfo targetInfo)
//		{
//			var areaLogicID = pSkillData.MSV_AreaLogic;
//			if((int)eSkillAreaLogic.SkillArea_Min < areaLogicID && areaLogicID < (int)eSkillAreaLogic.SkillArea_Max)
//			{
//				return m_vAreaLogicDict[pSkillData.MSV_AreaLogic].CalculationHit(pSkillData, targetInfo);
//			}

//			return null;
//		}

//	}


//}
