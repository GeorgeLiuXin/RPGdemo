using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy.XmlData
{
    //XML读取
    public class XmlReaderBase : XmlBase, IXmlOperation
    {
        protected readonly string sRoot = "root";
        protected readonly string iIndex = "iIndex";
        protected readonly string sDes = "sDes";

        protected readonly string sPreName = "ID_";
        protected readonly string sLogicName = "LogicName";

        public XmlReaderBase(string filePath, string filePathInEditor) : base(filePath, filePathInEditor)
        {
#if UNITY_EDITOR
            CheckXml();
#endif
        }
        public void CheckXml()
        {
#if UNITY_EDITOR
            string path = m_XmlPathInEditor;
            if (!File.Exists(path))
            {
				Log.Error("当前路径下没有对应的XML文件: " + path);
                CreateXml();
            }
#endif
        }

        /// <summary>
        /// 检查当前是否包含改节点
        /// </summary>
        /// <param name="parentNode">当前检查的父节点</param>
        /// <param name="sKey">需要查找的节点名称</param>
        /// <returns></returns>
        public bool ContainsNode(XmlElement parentNode, string sKey)
        {
            XmlElement checkEle = parentNode.SelectSingleNode(sKey) as XmlElement;
            if (checkEle != null)
                return true;
            return false;
        }

        public string GetPropertyValue(XmlParamItem item)
        {
            return item.sValue;
        }

        public void CreateXml()
        {
#if UNITY_EDITOR
            if (!File.Exists(m_XmlPathInEditor))
            {
                // 创建xml文档实例
                XmlDocument xmlDoc = new XmlDocument();
                // 创建根节点
                XmlElement root = xmlDoc.CreateElement(sRoot);
                xmlDoc.AppendChild(root);

                xmlDoc.Save(m_XmlPathInEditor);
                Log.Debug("已创建: " + m_XmlPathInEditor);
            }
#endif
        }

        public void AddXml(XmlDataList data)
        {
#if UNITY_EDITOR
            if (File.Exists(m_XmlPathInEditor))
            {
                // xml文档实例
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(m_XmlPathInEditor);
                // 获取根节点
                XmlElement root = xmlDoc.SelectSingleNode(sRoot) as XmlElement;
                if (root == null)
                    return;

                if (data.iIndex == 0)
                    return;

                if (ContainsNode(root, sPreName + data.iIndex.ToString()))
                    return;

                //创建整个表现节点
                XmlElement curParentElm = xmlDoc.CreateElement(sPreName + data.iIndex.ToString());
                curParentElm.SetAttribute(iIndex, data.iIndex.ToString());
                curParentElm.SetAttribute(sDes, data.sDescribe);

                foreach (XmlClassData _class in data)
                {
                    //创建单个表现类型
                    XmlElement elm = xmlDoc.CreateElement(_class.sLogicName);
                    foreach (XmlParamItem _property in _class)
                    {
                        XmlElement elmProperty = xmlDoc.CreateElement(_property.sName);
                        elmProperty.SetAttribute("Type", _property.sType);
                        elmProperty.InnerText = GetPropertyValue(_property);
                        elm.AppendChild(elmProperty);
                    }
                    curParentElm.AppendChild(elm);
                }
                root.AppendChild(curParentElm);

                xmlDoc.Save(m_XmlPathInEditor);
            }
#endif
        }
        public void AddXml(Dictionary<int, XmlDataList> datadict)
        {
#if UNITY_EDITOR
            if (File.Exists(m_XmlPathInEditor))
            {
                // xml文档实例
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(m_XmlPathInEditor);
                // 获取根节点
                XmlElement root = xmlDoc.SelectSingleNode(sRoot) as XmlElement;
                if (root == null)
                    return;

                foreach (KeyValuePair<int, XmlDataList> pair in datadict)
                {
                    if (ContainsNode(root, sPreName + pair.Key.ToString()))
                        continue;

                    //创建整个表现节点
                    XmlElement curParentElm = xmlDoc.CreateElement(sPreName + pair.Key.ToString());
                    curParentElm.SetAttribute(iIndex, pair.Key.ToString());
                    curParentElm.SetAttribute(sDes, pair.Value.sDescribe);

                    foreach (XmlClassData _class in pair.Value)
                    {
                        //创建单个表现类型
                        XmlElement elm = xmlDoc.CreateElement(_class.sLogicName);
                        foreach (XmlParamItem _property in _class)
                        {
                            XmlElement elmProperty = xmlDoc.CreateElement(_property.sName);
                            elmProperty.SetAttribute("Type", _property.sType);
                            elmProperty.InnerText = GetPropertyValue(_property);
                            elm.AppendChild(elmProperty);
                        }
                        curParentElm.AppendChild(elm);
                    }
                    root.AppendChild(curParentElm);
                }
                
                xmlDoc.Save(m_XmlPathInEditor);
            }
#endif
        }
        
        public void UpdateXml(Dictionary<int, XmlDataList> datadict)
        {
#if UNITY_EDITOR
            if (File.Exists(m_XmlPathInEditor))
            {
                // xml文档实例
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(m_XmlPathInEditor);
                // 获取根节点
                XmlElement root = xmlDoc.SelectSingleNode(sRoot) as XmlElement;
                if (root == null)
                    return;
                
                foreach (KeyValuePair<int, XmlDataList> pair in datadict)
                {
                    bool bDataCreate = false;
                    XmlElement dataElm = root.SelectSingleNode(sPreName + pair.Key.ToString()) as XmlElement;
                    if (dataElm == null)
                    {
                        dataElm = xmlDoc.CreateElement(sPreName + pair.Key.ToString());
                        bDataCreate = true;
                    }
                    dataElm.SetAttribute(iIndex, pair.Key.ToString());
                    dataElm.SetAttribute(sDes, pair.Value.sDescribe);
                    foreach (XmlClassData _class in pair.Value)
                    {
                        bool bClassCreate = false;
                        XmlElement elmClass;
                        //elmClass = dataElm.SelectSingleNode(_class.sLogicName) as XmlElement;
                        //if (elmClass == null)
                        //{
                        //    elmClass = xmlDoc.CreateElement(_class.sLogicName);
                        //    bClassCreate = true;
                        //}
                        
                        elmClass = xmlDoc.CreateElement(_class.sLogicName);
                        bClassCreate = true;

                        foreach (XmlParamItem _property in _class)
                        {
                            bool bPropertyCreate = false;
                            XmlElement elmProperty;
                            elmProperty = elmClass.SelectSingleNode(_property.sName) as XmlElement;
                            if (elmProperty == null)
                            {
                                elmProperty = xmlDoc.CreateElement(_property.sName);
                                bPropertyCreate = true;
                            }

                            elmProperty.SetAttribute("Type", _property.sType);
                            elmProperty.InnerText = GetPropertyValue(_property);

                            if (bPropertyCreate)
                                elmClass.AppendChild(elmProperty);
                        }
                        
                        if (bClassCreate)
                            dataElm.AppendChild(elmClass);
                    }
                    
                    if (bDataCreate)
                        root.AppendChild(dataElm);
                }
                xmlDoc.Save(m_XmlPathInEditor);
            }
#endif
        }
        public void UpdateXml(Dictionary<int, XmlDataList> datadict, bool bRemove)
        {
#if UNITY_EDITOR
            if (!bRemove)
            {
                UpdateXml(datadict);
            }
            else
            {
                if (File.Exists(m_XmlPathInEditor))
                {
                    // xml文档实例
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(m_XmlPathInEditor);
                    // 获取根节点
                    XmlElement root = xmlDoc.SelectSingleNode(sRoot) as XmlElement;
                    if (root == null)
                        return;

                    root.RemoveAll();

                    xmlDoc.Save(m_XmlPathInEditor);

                    UpdateXml(datadict);
                }
            }
#endif
        }

        public void UpdateXmlByAllClass()
        {
            return;
        }

        public void ReadXml(ref Dictionary<int, XmlDataList> dict)
        {
#if UNITY_EDITOR
            if (File.Exists(m_XmlPathInEditor))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc = new XmlDocument();
                xmlDoc.Load(m_XmlPathInEditor);
                XmlElement root = xmlDoc.SelectSingleNode(sRoot) as XmlElement;
                if (root == null)
                    return;

                foreach (XmlElement xml_data in root)
                {
                    if (xml_data == null)
                        continue;

                    XmlDataList _data = new XmlDataList();
                    _data.iIndex = Convert.ToInt32(xml_data.GetAttribute(iIndex));
                    _data.sDescribe = xml_data.GetAttribute(sDes);

                    foreach (XmlElement xml_class in xml_data)
                    {
                        XmlClassData _class = new XmlClassData();
                        _class.sLogicName = xml_class.LocalName;

                        foreach (XmlElement xml_property in xml_class)
                        {
                            XmlParamItem _property = new XmlParamItem();
                            _property.sName = xml_property.LocalName;
                            _property.sType = xml_property.GetAttribute("Type");
                            _property.sValue = xml_property.InnerText;

                            _class.Add(_property);
                        }

                        _data.Add(_class);
                    }

                    if (dict.ContainsKey(_data.iIndex))
                    {
                        dict.Remove(_data.iIndex);
                    }
                    dict.Add(_data.iIndex, _data);
                }
            }
#else
            string strFileCon = ResourcesProxy.LoadTextString(m_XmlFilePath);
            if (string.IsNullOrEmpty(strFileCon))
                return;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc = new XmlDocument();
            try
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(strFileCon);
                MemoryStream ms = new MemoryStream(buffer);
                xmlDoc.Load(ms);

            }
            catch (XmlException e)
            {
                Log.Debug("LoadDefine error!");
                Log.Debug("line : " + e.LineNumber + " pos : " + e.LinePosition);
                Log.Debug("reason : " + e.Message);
            }

            XmlElement root = xmlDoc.SelectSingleNode(sRoot) as XmlElement;
            if (root == null)
                return;

            foreach (XmlElement xml_data in root)
            {
                if (xml_data == null)
                    continue;

                XmlDataList _data = new XmlDataList();
                _data.iIndex = Convert.ToInt32(xml_data.GetAttribute(iIndex));
                _data.sDescribe = xml_data.GetAttribute(sDes);

                foreach (XmlElement xml_class in xml_data)
                {
                    XmlClassData _class = new XmlClassData();
                    _class.sLogicName = xml_class.LocalName;

                    foreach (XmlElement xml_property in xml_class)
                    {
                        XmlParamItem _property = new XmlParamItem();
                        _property.sName = xml_property.LocalName;
                        _property.sType = xml_property.GetAttribute("Type");
                        _property.sValue = xml_property.InnerText;

                        _class.Add(_property);
                    }

                    _data.Add(_class);
                }

                if (dict.ContainsKey(_data.iIndex))
                {
                    dict.Remove(_data.iIndex);
                }
                dict.Add(_data.iIndex, _data);
            }
#endif
		}

		public void DeleteXml(string id)
        {
#if UNITY_EDITOR
            if (File.Exists(m_XmlPathInEditor))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(m_XmlPathInEditor);
                XmlNode rootNode = xmlDoc.SelectSingleNode(sRoot);
                if (rootNode == null)
                    return;

                XmlNode xe = rootNode.SelectSingleNode(sPreName + id);
                if (xe == null)
                    return;

                rootNode.RemoveChild(xe);

                xmlDoc.Save(m_XmlPathInEditor);
            }
#endif
        }
        public void DeleteAllXml()
        {
#if UNITY_EDITOR
            if (File.Exists(m_XmlPathInEditor))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(m_XmlPathInEditor);
                XmlNode rootNode = xmlDoc.SelectSingleNode(sRoot);
                if (rootNode == null)
                    return;

                rootNode.RemoveAll();

                xmlDoc.Save(m_XmlPathInEditor);
            }
#endif
        }
    }
}