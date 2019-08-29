using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace Galaxy
{
    public interface IPerformanceData
    {
        void InitMyData(XmlData.XmlClassData data);
        ePerformanceLogic GetLogicType();
    }
    public abstract class PerformanceDataBase<T> where T : IPerformanceData, new()
    {
        //以下两个参数仅用于InitDataDefine修改使用,正常逻辑使用下面定义的方法
        protected static System.Reflection.FieldInfo[] m_DataFields = null;
        protected static Dictionary<string, int> m_DataFieldsDict = null;

        public System.Reflection.FieldInfo[] DataFields
        {
            get
            {
                if (m_DataFields == null)
                {
                    InitDataDefine();
                }
                return m_DataFields;
            }
        }
        public Dictionary<string, int> DataFieldsDict
        {
            get
            {
                if (m_DataFieldsDict == null)
                {
                    InitDataDefine();
                }
                return m_DataFieldsDict;
            }
        }
        /// <summary>
        /// 初始化m_DataFields与m_DataFieldsDict参数
        /// </summary>
        public static void InitDataDefine()
        {
            T data = new T();
            m_DataFields = data.GetType().GetFields();
            m_DataFieldsDict = new Dictionary<string, int>();
            for (int i = 0; i < m_DataFields.Length; i++)
            {
                System.Reflection.FieldInfo info = m_DataFields[i];
                if (info == null)
                    continue;
                m_DataFieldsDict.Add(info.Name, i);
            }
        }
        
        public void InitMyData(XmlData.XmlClassData data)
        {
            System.Reflection.FieldInfo field;
            int index = 0;
            foreach (var item in data)
            {
                if (DataFields == null || DataFieldsDict == null || !DataFieldsDict.ContainsKey(item.sName))
                {
					Log.Error(this.GetType().Name + "该逻辑参数与xml配置不一致");
                    continue;
                }

                index = DataFieldsDict[item.sName];
                field = DataFields[index];
                if (field != null)
                {
                    if (item.sType.Equals("System.Boolean"))
                    {
                        field.SetValue(this, Convert.ToBoolean(item.sValue));
                    }
                    else if (item.sType.Equals("System.Int32"))
                    {
                        field.SetValue(this, Convert.ToInt32(item.sValue));
                    }
                    else if (item.sType.Equals("System.Single"))
                    {
                        field.SetValue(this, Convert.ToSingle(item.sValue));
                    }
                    else if (item.sType.Equals("System.String"))
                    {
                        field.SetValue(this, item.sValue);
                    }
                }
            }
        }
    }
}