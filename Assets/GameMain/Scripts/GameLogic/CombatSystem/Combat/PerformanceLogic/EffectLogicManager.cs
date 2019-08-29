using System.Collections.Generic;
using Galaxy.XmlData;
using System;
using UnityGameFramework.Runtime;
using UnityEngine;

namespace Galaxy
{
    public enum eEffectSpecies
    {
        Effect_None = 0,

        Effect_Size = 100,
    };

    /// <summary>
    /// 表现效果id To 效果集合  classDict
    /// </summary>
    public class EffectLogicManager : GameFrameworkComponent
	{
        private EffectLogicReader reader;
        private Dictionary<int, XmlDataList> m_dict;
        
        private Dictionary<long, List<PerformanceLogic>> m_PerformanceLogicDict;
        private List<long> m_RemoveList;
        private long index;

		protected override void Awake()
		{
			base.Awake();
			InitManager();
		}
		private void InitManager()
        {
            reader = new EffectLogicReader();
            InitDataDict();
            
            m_PerformanceLogicDict = new Dictionary<long, List<PerformanceLogic>>();
            m_RemoveList = new List<long>();
            index = 0;

            InitPredicateDict();
            RegisterEvents();
        }
        private void InitDataDict()
        {
            m_dict = new Dictionary<int, XmlDataList>();
            reader.ReadXml(ref m_dict);
        }

		private void OnDestroy()
		{
			UnRegisterEvents();
		}

		private void Update()
		{
			UpdateLogic(Time.deltaTime);
		}
		private void UpdateLogic(float fFrameTime)
		{
			if(m_PerformanceLogicDict == null)
				return;

			foreach(KeyValuePair<long, List<PerformanceLogic>> vList in m_PerformanceLogicDict)
			{
				for(int i = vList.Value.Count - 1; i >= 0; i--)
				{
					if(vList.Value[i] == null)
					{
						vList.Value.RemoveAt(i);
						continue;
					}

					if(!vList.Value[i].Update(fFrameTime))
					{
						vList.Value[i].Destroy();
					}

					if(vList.Value[i].IsDestroy())
					{
						vList.Value.RemoveAt(i);
						continue;
					}
				}
				if(vList.Value.Count == 0)
				{
					m_RemoveList.Add(vList.Key);
				}
			}

			if(m_RemoveList.Count != 0)
			{
				foreach(var key in m_RemoveList)
				{
					m_PerformanceLogicDict.Remove(key);
				}
				m_RemoveList.Clear();
			}
		}

		private void FixedUpdate()
		{
			if(m_PerformanceLogicDict == null)
				return;

			foreach(KeyValuePair<long, List<PerformanceLogic>> vList in m_PerformanceLogicDict)
			{
				for(int i = vList.Value.Count - 1; i >= 0; i--)
				{
					if(vList.Value[i] == null)
					{
						continue;
					}

					vList.Value[i].FixedUpdate(Time.fixedDeltaTime);
				}
			}
		}
        
        /// <summary>
        /// 开始某些表现效果 XML数据类型
        /// </summary>
        /// <param name="effectType">效果类型</param>
        /// <param name="longID">当前表现效果唯一ID</param>
        /// <returns></returns>
        public bool StartPerformanceLogic(eEffectSpecies effectType, ref long longID)
        {
            if (effectType <= eEffectSpecies.Effect_None || effectType >= eEffectSpecies.Effect_Size)
                return false;
            int nPerformanceLogicID = (int)effectType;
            return StartPerformanceLogic(nPerformanceLogicID, ref longID);
        }
        /// <summary>
        /// 开始某些表现效果 XML数据类型
        /// </summary>
        /// <param name="nPerformanceLogicID">表现效果逻辑ID</param>
        /// <param name="longID">当前表现效果唯一ID</param>
        /// <returns></returns>
        public bool StartPerformanceLogic(int nPerformanceLogicID, ref long longID)
        {
            return StartPerformanceLogic(nPerformanceLogicID, ref longID, 0);
        }
        /// <summary>
        /// 开始某些表现效果 XML数据类型
        /// </summary>
        /// <param name="nPerformanceLogicID">表现效果逻辑ID</param>
        /// <param name="fTime">当前效果共持续多久</param>
        /// <param name="longID">当前表现效果唯一ID</param>
        /// <returns></returns>
        public bool StartPerformanceLogic(int nPerformanceLogicID, ref long longID, float fTime)
        {
            return StartPerformanceLogic(nPerformanceLogicID, ref longID, 0, fTime);
        }
        /// <summary>
        /// 开始某些表现效果 XML数据类型
        /// </summary>
        /// <param name="nAvatarID">表现效果拥有者server ID,当此目标死亡后对应效果会移除</param>
        /// <param name="nPerformanceLogicID">表现效果逻辑ID</param>
        /// <param name="longID">当前表现效果唯一ID</param>
        /// <returns></returns>
        public bool StartPerformanceLogic(int nPerformanceLogicID, ref long longID, int nAvatarID)
        {
            return StartPerformanceLogic(nPerformanceLogicID, ref longID, nAvatarID, null);
        }
        /// <summary>
        /// 开始某些表现效果 XML数据类型
        /// </summary>
        /// <param name="nAvatarID">表现效果拥有者server ID,当此目标死亡后对应效果会移除</param>
        /// <param name="nPerformanceLogicID">表现效果逻辑ID</param>
        /// <param name="fTime">当前效果共持续多久</param>
        /// <param name="longID">当前表现效果唯一ID</param>
        /// <returns></returns>
        public bool StartPerformanceLogic(int nPerformanceLogicID, ref long longID, int nAvatarID, float fTime)
        {
            return StartPerformanceLogic(nPerformanceLogicID, ref longID, nAvatarID, fTime, null);
        }
        /// <summary>
        /// 开始某些表现效果 XML数据类型
        /// </summary>
        /// <param name="nAvatarID">表现效果拥有者server ID,当此目标死亡后对应效果会移除</param>
        /// <param name="nPerformanceLogicID">表现效果逻辑ID</param>
        /// <param name="longID">当前表现效果唯一ID</param>
        /// <param name="values">相关参数</param>
        /// <returns></returns>
        public bool StartPerformanceLogic(int nPerformanceLogicID, ref long longID, int nAvatarID, params object[] values)
        {
            if (m_dict == null || !m_dict.ContainsKey(nPerformanceLogicID))
                return false;

            List<PerformanceLogic> list = new List<PerformanceLogic>();
            XmlDataList dataList = m_dict[nPerformanceLogicID];
            foreach (XmlClassData data in dataList)
            {
                PerformanceLogic logic = PerformanceLogicFactory.Instance.GetPerformanceLogic(data.sLogicName);
                if (logic == null)
                    continue;

                logic.SetOwner(nAvatarID);
                if (!logic.InitData(data) || !logic.Init(values))
                    continue;
                list.Add(logic);
            }
            longID = GetLogicIndex();
            m_PerformanceLogicDict.Add(longID, list);
            return true;
        }
        /// <summary>
        /// 开始某些表现效果 XML数据类型
        /// </summary>
        /// <param name="nAvatarID">表现效果拥有者server ID,当此目标死亡后对应效果会移除</param>
        /// <param name="nPerformanceLogicID">表现效果逻辑ID</param>
        /// <param name="fTime">当前效果共持续多久</param>
        /// <param name="longID">当前表现效果唯一ID</param>
        /// <param name="values">相关参数</param>
        /// <returns></returns>
        public bool StartPerformanceLogic(int nPerformanceLogicID, ref long longID, int nAvatarID, float fTime, params object[] values)
        {
            if (m_dict == null || !m_dict.ContainsKey(nPerformanceLogicID))
                return false;
            
            List<PerformanceLogic> list = new List<PerformanceLogic>();
            XmlDataList dataList = m_dict[nPerformanceLogicID];
            foreach (XmlClassData data in dataList)
            {
                PerformanceLogic logic = PerformanceLogicFactory.Instance.GetPerformanceLogic(data.sLogicName);
                if (logic == null)
                    continue;

                logic.SetOwner(nAvatarID);
                if (!logic.InitData(data) || !logic.Init(values))
                    continue;
                logic.SetTotalTime(fTime);
                list.Add(logic);
            }
            longID = GetLogicIndex();
            m_PerformanceLogicDict.Add(longID, list);
            return true;
        }

		#region 暂时不用，之后使用json/xml节点化配置替代配表
		
		//     /// <summary>
		//     /// 开始某个表现效果 表格配置类型
		//     /// </summary>
		//     /// <param name="nAvatarID">表现效果拥有者server ID,当此目标死亡后对应效果会移除</param>
		//     /// <param name="sTableName">表格名称</param>
		//     /// <param name="nTableId">表格id</param>
		//     /// <param name="logicType">逻辑类型</param>
		//     /// <param name="longID">当前表现效果唯一ID</param>
		//     /// <param name="values">对应单个逻辑的相关参数</param>
		//     /// <returns></returns>
		//     public bool StartPerformanceLogic(int nAvatarID, string sTableName, int nTableId, ePerformanceLogic logicType, ref long longID, params object[] values)
		//     {
		//         if (logicType <= ePerformanceLogic.Xml_Size)
		//             return false;
		//         ConfigData data = ConfigDataTableManager.Instance.GetData(sTableName, nTableId);
		//         if (data == null)
		//             return false;

		//List<PerformanceLogic> list = new List<PerformanceLogic>();

		//         PerformanceLogic logic = PerformanceLogicFactory.Instance.GetPerformanceLogic((int)logicType);
		//         if (logic == null)
		//             return false;

		//         logic.SetOwner(nAvatarID);
		//         if (!logic.InitData(data) || !logic.Init(values))
		//             return false;
		//         list.Add(logic);

		//         longID = GetLogicIndex();
		//         m_PerformanceLogicDict.Add(longID, list);
		//         return true;
		//     }
		//     /// <summary>
		//     /// 开始某个表现效果 表格配置类型
		//     /// </summary>
		//     /// <param name="nAvatarID">表现效果拥有者server ID,当此目标死亡后对应效果会移除</param>
		//     /// <param name="sTableName">表格名称</param>
		//     /// <param name="nTableId">表格id</param>
		//     /// <param name="logicType">逻辑类型</param>
		//     /// <param name="fTime">当前效果共持续多久</param>
		//     /// <param name="longID">当前表现效果唯一ID</param>
		//     /// <param name="values">相关参数</param>
		//     /// <returns></returns>
		//     public bool StartPerformanceLogic(int nAvatarID, string sTableName, int nTableId, ePerformanceLogic logicType, float fTime, ref long longID, params object[] values)
		//     {
		//         if (logicType <= ePerformanceLogic.Xml_Size)
		//             return false;
		//         ConfigData data = ConfigDataTableManager.Instance.GetData(sTableName, nTableId);
		//         if (data == null)
		//             return false;

		//         List<PerformanceLogic> list = new List<PerformanceLogic>();

		//         PerformanceLogic logic = PerformanceLogicFactory.Instance.GetPerformanceLogic((int)logicType);
		//         if (logic == null)
		//             return false;

		//         logic.SetOwner(nAvatarID);
		//         if (!logic.InitData(data) || !logic.Init(values))
		//             return false;
		//         logic.SetTotalTime(fTime);
		//         list.Add(logic);

		//         longID = GetLogicIndex();
		//         m_PerformanceLogicDict.Add(longID, list);
		//         return true;
		//     }
		#endregion

		/// <summary>
		/// 结束某个表现效果
		/// </summary>
		/// <param name="longID">表现效果唯一ID</param>
		public void EndPerformanceLogic(long longID)
        {
            if (m_PerformanceLogicDict == null || !m_PerformanceLogicDict.ContainsKey(longID))
                return;

            foreach (PerformanceLogic item in m_PerformanceLogicDict[longID])
            {
                item.Destroy();
            }
        }
        /// <summary>
        /// 结束某个表现效果
        /// </summary>
        /// <param name="eRemoveType">根据模式进行移除</param>
        /// <param name="values">对应模式的参数</param>
        public void EndPerformanceLogic(PerformanceLogicMode eRemoveType, params object[] values)
        {
            foreach (KeyValuePair<long, List<PerformanceLogic>> vList in m_PerformanceLogicDict)
            {
                foreach (var item in vList.Value)
                {
                    if (PredicateDict[eRemoveType](item, values))
                    {
                        item.Destroy();
                    }
                }
            }
            //主动刷新以移除无用logic
            UpdateLogic(0);
        }

        private Dictionary<PerformanceLogicMode, PerformanceCheck> PredicateDict;
        private delegate bool PerformanceCheck(PerformanceLogic logic, object[] values);
        private void InitPredicateDict()
        {
            PredicateDict = new Dictionary<PerformanceLogicMode, PerformanceCheck>
            {
                {PerformanceLogicMode.LogicMode_BindToAvatar, IsEqualsAvatar},
                {PerformanceLogicMode.LogicMode_ReleaseWhenChangeScene, IsReleaseWhenChangeScene}
            };
        }

        private static bool IsEqualsAvatar(PerformanceLogic logic, object[] values)
        {
            if (values == null || values.Length < 1)
                return false;
            int nAvatarID = (int)values[0];
            return logic.IsBindToAvatar() && logic.GetOwner() == nAvatarID;
        }
        private static bool IsReleaseWhenChangeScene(PerformanceLogic logic, object[] values)
        {
            return logic.IsReleaseWhenChangeScene();
        }


        public void OnChangeScene(params object[] values)
        {
            EndPerformanceLogic(PerformanceLogicMode.LogicMode_ReleaseWhenChangeScene);
        }

        public void OnAvatarDestroy(int nAvatarID)
        {
            EndPerformanceLogic(PerformanceLogicMode.LogicMode_BindToAvatar, nAvatarID);
        }
        

        private void RegisterEvents()
        {
			//注册切换场景事件监听
			//GameEntry.Event.Subscribe();
		}

        private void UnRegisterEvents()
		{
			//注册切换场景事件监听
			//GameEntry.Event.Unsubscribe();
		}


        public void ReloadDataDict()
        {
            m_dict.Clear();
            InitDataDict();
        }

        //当index大于int size后
        private long GetLogicIndex()
        {
            index += 1;
            if (index > 2147483647)
            {
                index = 0;
            }
            return index;
        }

    }
}