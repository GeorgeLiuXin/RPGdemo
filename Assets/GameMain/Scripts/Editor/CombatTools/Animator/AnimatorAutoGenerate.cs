//using System;
//using System.Collections.Generic;
//using Galaxy.XmlData;
//using UnityEditor;
//using UnityEditor.IMGUI.Controls;
//using UnityEngine;
//using UnityGameFramework.Runtime;

//namespace Galaxy
//{
//    public class AnimatorAutoGenerateEditor
//	{
//		[MenuItem("Metroidvania3D/Generate Animator")]
//		public static void ShowEditor()
//        {
//            AnimatorAutoGenerateEditorWindow window = EditorWindow.GetWindow<AnimatorAutoGenerateEditorWindow>();
//            if (window)
//            {
//                window.ShowUtility();
//                var position = window.position;
//                position.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height).center;
//                window.position = position;
//                window.Show();
//            }
//        }
//    }

//    public partial class AnimatorAutoGenerateEditorWindow : EditorWindow
//    {
//        public AnimatorReader reader;

//        [NonSerialized]
//        bool m_Initialized;
//        [SerializeField]
//        TreeViewState m_TreeViewState;
//        SearchField m_SearchField;
//        AutoGenerateViewTree m_DataTreeView;

//        [SerializeField]
//        TreeViewState m_XmlTreeViewState;
//        AnimatorXMLViewTree m_XmlListTreeView;

//        private void OnEnable()
//        {

//        }

//        private void OnDisable()
//        {

//        }

//        private void OnGUI()
//        {
//            InitIfNeeded();

//            GUILayout.BeginHorizontal();
//            {
//                GUILayout.BeginVertical();
//                {
//                    SearchBar(toolbarRect);
//                    DoXmlTreeView(xmlRect);
//                    DoXmlFile(xmlFileRect);
//                }
//                GUILayout.EndVertical();

//                GUILayout.BeginVertical();
//                {
//                    GUILayout.BeginHorizontal();
//                    {
//                        TitleBar(titleRect);

//                        //tree 生成与初始化
//                        DoTreeView(treeViewRect);
//                        XmlCustomerView(xmlCustomerRect);
//                        BottomToolBar(bottomToolbarRect);
//                    }
//                    GUILayout.EndHorizontal();
//                }
//                GUILayout.EndVertical();
//            }
//            manualUpdate();
//            GUILayout.EndHorizontal();
//        }
        
//        void InitIfNeeded()
//        {
//            if (!m_Initialized)
//            {
//                InitData();

//                if (m_XmlTreeViewState == null)
//                    m_XmlTreeViewState = new TreeViewState();

//                m_XmlListTreeView = new AnimatorXMLViewTree(m_XmlTreeViewState, m_DesToDictIndex);
//                InitHandle();

//                if (m_TreeViewState == null)
//                    m_TreeViewState = new TreeViewState();

//                m_DataTreeView = new AutoGenerateViewTree(m_TreeViewState, AutoGenerateViewTree.CreateDefaultMultiColumnHeaderState());

//                m_SearchField = new SearchField();
//                m_SearchField.downOrUpArrowKeyPressed += m_XmlListTreeView.SetFocusAndEnsureSelectedItem;

//                m_Initialized = true;
//            }
//        }

//        void InitData()
//        {
//            reader = new AnimatorReader();
//            m_dataDict = new Dictionary<int, XmlDataList>();
//            m_DesToDictIndex = new Dictionary<string, int>();
//            ReadXmlData();

//            m_CurName = "";
//            bNeedToExpand = false;

////            string sXmlFilePath = "";
////#if UNITY_EDITOR
////            sXmlFilePath = Application.dataPath + "/Editor/CombatTools/Animator/AnimatorXmlData/AnimatorCustomer.txt";
////#endif
////            if (sXmlFilePath.IsNE())
////                return;
////            string strFileCon = ResourcesProxy.LoadTextString(sXmlFilePath);
////            if (string.IsNullOrEmpty(strFileCon))
////                return;
//        }

//        Rect titleRect
//        {
//            get { return new Rect(290, 0, position.width - 300, 30); }
//        }

//        Rect treeViewRect
//        {
//            get { return new Rect(290, 25, position.width - 300, position.height - 300); }
//        }

//        Rect xmlCustomerRect
//        {
//            get { return new Rect(290, position.height - 300 + 40, position.width - 300, 240); }
//        }
        
//        Rect bottomToolbarRect
//        {
//            get { return new Rect(position.width - 340, position.height - 27, 320, 30); }
//        }

//        Rect toolbarRect
//        {
//            get { return new Rect(10, 15, 270, 15); }
//        }
        
//        Rect xmlRect
//        {
//            get { return new Rect(10, 35, 270, position.height - 75); }
//        }

//        Rect xmlFileRect
//        {
//            get { return new Rect(10, position.height - 35, 270, 45); }
//        }

//        void TitleBar(Rect rect)
//        {
//            GUILayout.BeginArea(rect);
//            using (new EditorGUILayout.HorizontalScope())
//            {
//                EditorGUILayout.LabelField("当前动画信息包含: ");
//                if (GUILayout.Button("添加", GUILayout.Width(80)))
//                {
//                    AddXmlClassData();
//                    Repaint();
//                }
//            }
//            GUILayout.EndArea();
//        }

//        void SearchBar(Rect rect)
//        {
//            m_XmlListTreeView.searchString = m_SearchField.OnGUI(rect, m_XmlListTreeView.searchString);
//        }

//        void DoTreeView(Rect rect)
//        {
//            m_DataTreeView.OnGUI(rect);
//        }

//        private Dictionary<string, XmlParamItem> ItemDict = new Dictionary<string, XmlParamItem>();
//        void XmlCustomerView(Rect rect)
//        {
//            if (m_CurData == null)
//                return;

//            XmlClassData curXmlSetting = null;
//            List<XmlClassData>.Enumerator itor = m_CurData.GetEnumerator();
//            while (itor.MoveNext())
//            {
//                XmlClassData _class = itor.Current;
//                if (_class == null)
//                    continue;
//                if (!_class.sLogicName.Equals("AnimatorXmlSetting"))
//                    continue;

//                curXmlSetting = _class;
//            }

//            if (curXmlSetting == null)
//            {
//				Log.Error("当前xml不含有setting信息，请重新创建该数据！");
//                return;
//            }

//            ItemDict.Clear();
//            foreach (var item in curXmlSetting)
//            {
//                ItemDict.Add(item.sName, item);
//            }
            
//            GUILayout.BeginArea(rect);

//            using (new EditorGUILayout.VerticalScope())
//            {
//                EditorGUILayout.Space();
//                using (new EditorGUILayout.HorizontalScope())
//                {
//                    EditorGUILayout.LabelField("输出设置: ");
//                    EditorGUILayout.Space();
//                    EditorGUILayout.Space();
//                    EditorGUILayout.Space();
//                    if (GUILayout.Button("打开文件目录", GUILayout.Width(160)))
//                    {
//                        EditorUtility.OpenFilePanelWithFilters("文件目录", ItemDict["SavePath"].sValue, new string[] { });
//                    }
//                }
//                EditorGUILayout.Space();
//                using (new EditorGUILayout.HorizontalScope())
//                {
//                    string savePath = ItemDict["SavePath"].sValue;
//                    savePath = EditorGUILayout.TextField("保存路径: ", savePath);
//                    ItemDict["SavePath"].sValue = savePath;
//                }
//                using (new EditorGUILayout.HorizontalScope())
//                {
//                    string saveName = ItemDict["SaveName"].sValue;
//                    saveName = EditorGUILayout.TextField("保存名称: ", saveName);
//                    ItemDict["SaveName"].sValue = saveName;
//                }
//                EditorGUILayout.Space();
//                EditorGUILayout.LabelField("其他设置: ");
//                EditorGUILayout.Space();
//                using (new EditorGUILayout.HorizontalScope())
//                {
//                    bool bSetEmptyState;
//                    bool.TryParse(ItemDict["bSetEmptyState"].sValue, out bSetEmptyState);
//                    bSetEmptyState = EditorGUILayout.ToggleLeft("SetEmptyState: ", bSetEmptyState);
//                    ItemDict["bSetEmptyState"].sValue = bSetEmptyState.ToString();
//                }
//                using (new EditorGUILayout.HorizontalScope())
//                {
//                    EditorGUILayout.LabelField("自动重命名:(暂无功能，留坑) ");
//                }
//            }

//            GUILayout.EndArea();
//        }

//        void BottomToolBar(Rect rect)
//        {
//            GUILayout.BeginArea(rect);

//            using (new EditorGUILayout.HorizontalScope())
//            {
//                if (GUILayout.Button("保存XML"))
//                {
//                    if (EditorUtility.DisplayDialog("保存XML", "是否保存当前修改?", "保存数据", "取消"))
//                    {
//                        reader.UpdateXml(m_dataDict, true);
//                        RefreshXmlView();
//                    }
//                }
//                if (GUILayout.Button("重新导入"))
//                {
//                    if (EditorUtility.DisplayDialog("重新导入", "是否重新导入数据?", "重新导入", "取消"))
//                    {
//                        RefreshXmlView();
//                    }
//                }
//                if (GUILayout.Button("创建 Animator"))
//                {
//                    if (EditorUtility.DisplayDialog("创建 Animator", "是否生成Animator并保存数据?", "生成并保存", "仅生成"))
//                    {
//                        Generate();
//                    }
//                }
//            }

//            GUILayout.EndArea();
//        }


//        void DoXmlTreeView(Rect rect)
//        {
//            m_XmlListTreeView.OnGUI(rect);
//        }

//        void DoXmlFile(Rect rect)
//        {
//            GUILayout.BeginArea(rect);
//            using (new EditorGUILayout.HorizontalScope())
//            {
//                if (GUILayout.Button("打开文件目录"))
//                {
//                    string[] strs = reader.m_XmlPathInEditor.Split('/');
//                    int nLastIndex = strs[strs.Length - 1].Length;
//                    string str = reader.m_XmlPathInEditor.Left(reader.m_XmlPathInEditor.Length - nLastIndex);
//                    EditorUtility.OpenFilePanelWithFilters("文件目录", str, new string[] { });
//                }
//            }
//            GUILayout.EndArea();
//        }

//        #region 具体Editor参数及逻辑
//        private bool bNeedToExpand;

//        private void manualUpdate()
//        {
//            if (EditorApplication.isPlaying)
//            {
//                return;
//            }

//            if (bNeedToExpand)
//            {
//                m_DataTreeView.ExpandAll();
//                bNeedToExpand = !bNeedToExpand;
//            }
//        }
//        #endregion

//        #region Handle

//        public void InitHandle()
//        {
//            m_XmlListTreeView.OnAdd += AddXmlNode;
//            m_XmlListTreeView.OnChange += SetCurData;
//            m_XmlListTreeView.OnDelete += DeleteXmlNode;
//        }

//        #endregion

//        #region CurData

//        private string m_CurName;
//        private XmlDataList m_CurData;

//        public void SetCurData(string sName)
//        {
//            if (sName.IsNE())
//                return;
            
//            if (!m_DesToDictIndex.ContainsKey(sName))
//                return;

//            if (!m_dataDict.ContainsKey(m_DesToDictIndex[sName]))
//                return;

//            m_CurName = sName;
//            m_CurData = m_dataDict[m_DesToDictIndex[sName]];
//            m_DataTreeView.RefreshByNewData(ref m_CurData);

//            bNeedToExpand = true;
//        }

//        #endregion

//        #region XmlView 当前Xml包含数据

//        //描述转换为表现效果对应id
//        private Dictionary<string, int> m_DesToDictIndex;
//        private Dictionary<int, XmlDataList> m_dataDict;

//        /// <summary>
//        /// 读取Xml中所有的数据
//        /// </summary>
//        private void ReadXmlData()
//        {
//            m_dataDict.Clear();
//            reader.ReadXml(ref m_dataDict);
//            m_DesToDictIndex.Clear();
//            foreach (var item in m_dataDict)
//            {
//                m_DesToDictIndex.Add(item.Value.sDescribe, item.Key);
//            }
//        }

//        private void RefreshXmlView()
//        {
//            ReadXmlData();

//            RefreshLogicViewTree();
//            RefreshXMLViewTree();

//            Repaint();
//        }
//        private void RefreshLogicViewTree()
//        {
//            SetCurData(m_CurName);

//            m_DataTreeView.Reload();
//            m_DataTreeView.Repaint();
//        }
//        private void RefreshXMLViewTree()
//        {
//            m_XmlListTreeView.RefreshXmlClassList(m_DesToDictIndex);

//            m_XmlListTreeView.Reload();
//            m_XmlListTreeView.Repaint();
//        }

//        private void AddXmlNode(string nodeInfo)
//        {
//            XmlDataList data = new XmlDataList {iIndex = GetMaxXmlNode(), sDescribe = nodeInfo};
//            AddXmlNode(data);
//        }
//        private void AddXmlNode(XmlDataList data)
//        {
//            if (data.iIndex == 0 || data.sDescribe.IsNE())
//            {
//                EditorUtility.DisplayDialog("添加错误", "当前输入的名称为空!", "OK");
//                return;
//            }
//            if (m_DesToDictIndex.ContainsKey(data.sDescribe))
//            {
//                EditorUtility.DisplayDialog("添加错误", "存在同名的文件!", "OK");
//                return;
//            }

//            AddSettingData(data);
//            reader.AddXml(data);
//            m_CurName = data.sDescribe;
//            RefreshXmlView();
//        }
        
//        private void AddSettingData(XmlDataList list)
//        {
//            AnimatorXmlSetting setting = new AnimatorXmlSetting();
//            System.Reflection.FieldInfo[] m_DataFields = setting.GetType().GetFields();
//            XmlClassData data = new XmlClassData {sLogicName = "AnimatorXmlSetting"};
//            foreach (var field in m_DataFields)
//            {
//                XmlParamItem item = new XmlParamItem
//                {
//                    sName = field.Name, sType = field.FieldType.ToString(), sValue = ""
//                };
//                data.Add(item);
//            }
//            list.SafeAdd(data);
//        }

//        private int GetMaxXmlNode()
//        {
//            int nMaxIndex = 0;
//            foreach (var item in m_dataDict)
//            {
//                if (item.Key > nMaxIndex)
//                {
//                    nMaxIndex = item.Key;
//                }
//            }

//            nMaxIndex++;
//            return nMaxIndex;
//        }

//        private void DeleteXmlNode(string nodeIndex)
//        {
//            if (!m_DesToDictIndex.ContainsKey(nodeIndex))
//                return;

//            if (!m_dataDict.ContainsKey(m_DesToDictIndex[nodeIndex]))
//                return;

//            if (EditorUtility.DisplayDialog("确认", "是否确认删除改节点?", "Y", "N"))
//            {
//                reader.DeleteXml(m_DesToDictIndex[nodeIndex].ToString());
//            }

//            RefreshXmlView();
//        }

//        private void AddXmlClassData()
//        {
//            m_DataTreeView.AddNewClassData();
//            Repaint();
//        }
//        #endregion
//    }
//}