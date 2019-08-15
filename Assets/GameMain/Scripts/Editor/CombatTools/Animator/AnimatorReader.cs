using UnityEngine;
using Galaxy.XmlData;

namespace Galaxy
{
	//XML 表现效果读取方式
	public class AnimatorReader : XmlReaderBase
	{
		private static string m_xmlPath;
		public static string XmlPath
		{
			get
			{
#if !UNITY_EDITOR && UNITY_ANDROID
                m_xmlPath = "AnimatorXmlData.xml";
#elif !UNITY_EDITOR && UNITY_IPHONE
                m_xmlPath = "AnimatorXmlData.xml";
#else
				m_xmlPath = "AnimatorXmlData.xml";
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
                m_xmlPathInEditor = Application.dataPath + "/GameMain/Scripts/Editor/CombatTools/Animator/AnimatorXmlData/AnimatorXmlData.xml";
#elif !UNITY_EDITOR && UNITY_IPHONE
                m_xmlPathInEditor = Application.dataPath + "/GameMain/Scripts/Editor/CombatTools/Animator/AnimatorXmlData/AnimatorXmlData.xml";
#else
				m_xmlPathInEditor = Application.dataPath + "/GameMain/Scripts/Editor/CombatTools/Animator/AnimatorXmlData/AnimatorXmlData.xml";
#endif
				return m_xmlPathInEditor;
			}
		}

		public AnimatorReader() : base(XmlPath, XmlPathInEditor)
		{

		}

	}

}