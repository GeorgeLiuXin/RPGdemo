using UnityEngine;
using Galaxy.XmlData;

namespace Galaxy
{
    //XML 表现效果读取方式
    public class EffectLogicReader : XmlReaderBase
    {
        private static string m_xmlPath;
        public static string XmlPath
        {
            get
            {
#if !UNITY_EDITOR && UNITY_ANDROID
                m_xmlPath = "CombatXmlDefine.xml";
#elif !UNITY_EDITOR && UNITY_IPHONE
                m_xmlPath = "CombatXmlDefine.xml";
#else
                m_xmlPath = "CombatXmlDefine.xml";
#endif
                return m_xmlPath;
            }
        }

        private static string m_xmlPathInEditor;
        public static string XmlPathInEditor
        {
            get
            {
#if !UNITY_EDITOR && UNITY_ANDROID
                m_xmlPathInEditor = Application.streamingAssetsPath + "/CombatXmlDefine.xml";
#elif !UNITY_EDITOR && UNITY_IPHONE
                m_xmlPathInEditor = Application.streamingAssetsPath + "/CombatXmlDefine.xml";
#else
                m_xmlPathInEditor = Application.streamingAssetsPath + "/CombatXmlDefine.xml";
#endif
                return m_xmlPathInEditor;
            }
        }

        public EffectLogicReader() : base(XmlPath, XmlPathInEditor)
        {

        }

    }

}