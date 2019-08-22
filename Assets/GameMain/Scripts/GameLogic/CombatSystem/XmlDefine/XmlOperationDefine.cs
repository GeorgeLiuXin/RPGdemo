﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy.XmlData
{
    public interface IXmlOperation
    {
        string m_XmlFilePath
        {
            get;
            set;
        }
        string m_XmlPathInEditor
        {
            get;
            set;
        }

        /// <summary>
        /// 创建xml文件
        /// </summary>
        void CreateXml();

        /// <summary>
        /// 添加xml数据
        /// </summary>
        /// <param name="data">数据</param>
        void AddXml(XmlDataList data);
        /// <summary>
        /// 添加xml数据
        /// </summary>
        /// <param name="data">数据</param>
        void AddXml(Dictionary<int, XmlDataList> datadict);
        
        /// <summary>
        /// 更新xml数据
        /// </summary>
        /// <param name="data">数据</param>
        void UpdateXml(Dictionary<int, XmlDataList> datadict);
        /// <summary>
        /// 更新整个xml数据
        /// </summary>
        /// <param name="data">数据</param>
        void UpdateXml(Dictionary<int, XmlDataList> datadict, bool bRemove);

        /// <summary>
        /// 根据所有类型来更新整个xml数据文件
        /// </summary>
        void UpdateXmlByAllClass();

        /// <summary>
        /// 读取xml
        /// </summary>
        /// <param name="dict"></param>
        void ReadXml(ref Dictionary<int, XmlDataList> dict);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        void DeleteXml(string id);
        /// <summary>
        /// 删除所有数据
        /// </summary>
        void DeleteAllXml();

    }

    public class XmlBase
    {
        public string m_XmlFilePath
        {
            get;
            set;
        }
        public string m_XmlPathInEditor
        {
            get;
            set;
        }

        public XmlBase(string filePath, string filePathInEditor)
        {
            if (filePath.IsNE())
            {
                Log.Error("当前XML路径为空");
                return;
            }

            m_XmlFilePath = filePath;
            
            if (filePath.IsNE())
            {
				Log.Error("当前XMLInEditor路径为空");
                return;
            }
            m_XmlPathInEditor = filePathInEditor;
        }
    }

}