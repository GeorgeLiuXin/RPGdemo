using System.Collections.Generic;

namespace Galaxy
{
    //此处添加的enum 需要与原本C#代码中逻辑名称一致，其用于创建对应逻辑与数据
    public enum ePerformanceLogic
    {
        None = -1,
		PerformanceTestLogic = 0,
        Xml_Size,

        //表格数据logic
        CombatLaunchEffect = 1000,
        Table_Size,
    }

    public class PerformanceLogicFactory : Singleton<PerformanceLogicFactory>
    {
        public Dictionary<string, ePerformanceLogic> m_dict;
        public Dictionary<ePerformanceLogic, IPerformanceData> m_TemplateDataDict;
        public Dictionary<ePerformanceLogic, PerformanceLogic> m_TemplateLogicDict;

        public PerformanceLogicFactory()
        {
            m_dict = new Dictionary<string, ePerformanceLogic>();
            m_TemplateDataDict = new Dictionary<ePerformanceLogic, IPerformanceData>();
            m_TemplateLogicDict = new Dictionary<ePerformanceLogic, PerformanceLogic>();

            for (ePerformanceLogic i = ePerformanceLogic.None + 1; i < ePerformanceLogic.Xml_Size; i++)
            {
                InitPerformance(i);
            }
        }
        void InitPerformance(ePerformanceLogic logicEnum)
        {
            m_dict.Add(logicEnum.ToString(), logicEnum);
            m_TemplateDataDict.Add(logicEnum, GetPerformanceData((int)logicEnum));
            m_TemplateLogicDict.Add(logicEnum, GetPerformanceLogic((int)logicEnum));
        }

        public IPerformanceData GetTemplateData(string logicName)
        {
            if (m_dict == null
                || !m_dict.ContainsKey(logicName)
                || m_TemplateDataDict == null
                || !m_TemplateDataDict.ContainsKey(m_dict[logicName]))
            {
                return null;
            }
            return m_TemplateDataDict[m_dict[logicName]];
        }
        public PerformanceLogic GetTemplateLogic(string logicName)
        {
            if (m_dict == null
                || !m_dict.ContainsKey(logicName)
                || m_TemplateLogicDict == null
                || !m_TemplateLogicDict.ContainsKey(m_dict[logicName]))
            {
                return null;
            }
            return m_TemplateLogicDict[m_dict[logicName]];
        }


        private IPerformanceData GetPerformanceData(int index)
        {
            switch ((ePerformanceLogic)index)
            {
                case ePerformanceLogic.PerformanceTestLogic:
                    return new PerformanceTestData();
                default:
                    return null;
            }
        }
        public PerformanceLogic GetPerformanceLogic(string logicName)
        {
            if (m_dict == null || !m_dict.ContainsKey(logicName))
                return null;
            ePerformanceLogic tempEnum = m_dict[logicName];
            return GetPerformanceLogic((int)tempEnum);
        }
        public PerformanceLogic GetPerformanceLogic(int index)
        {
            switch ((ePerformanceLogic)index)
            {
                case ePerformanceLogic.PerformanceTestLogic:
                    return new PerformanceTestLogic();
                //以上为正常的xml配置数据的表现逻辑
                ////////////////////////////////////////////////
                //以下为配置对应表作为表现数据的表现逻辑
                default:
                    return null;
            }
        }
    }
}