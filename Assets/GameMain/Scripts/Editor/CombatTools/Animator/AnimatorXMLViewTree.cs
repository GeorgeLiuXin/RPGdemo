using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Galaxy
{
    public class AnimatorXMLViewTree : TreeView
    {
        private Dictionary<string, int> m_DesToDictIndex;
        private readonly string m_AddNodeDisplayName = "New Animator XML...";

        private bool m_IsAddState;
        private string m_XmlNodeName;
        private string m_sCurDataName;

        public AnimatorXMLViewTree(TreeViewState treeViewState, Dictionary<string, int> _dict) : base(treeViewState)
        {
            showAlternatingRowBackgrounds = true;
            showBorder = true;

            m_IsAddState = false;
            m_XmlNodeName = "";
            m_DesToDictIndex = new Dictionary<string, int>(_dict);
            m_sCurDataName = "";
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            TreeViewItem root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };

            List<TreeViewItem> allItems = new List<TreeViewItem>();

            foreach (KeyValuePair<string, int> item in m_DesToDictIndex)
            {
                TreeViewItem node = new TreeViewItem {id = item.Value, depth = 0, displayName = item.Key};
                allItems.Add(node);
            }

            TreeViewItem nodeAdd = new TreeViewItem { depth = 0, displayName = m_AddNodeDisplayName };
            allItems.Add(nodeAdd);

            SetupParentsAndChildrenFromDepths(root, allItems);
            return root;
        }

        public override void OnGUI(Rect rect)
        {
            base.OnGUI(rect);

            TreeViewItem item = FindItem(state.lastClickedID, rootItem);
            if (item != null && m_sCurDataName != item.displayName)
            {
                SetCurString(item.displayName);
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            base.RowGUI(args);

            TreeViewItem node = args.item;

            if (node.displayName.Equals(m_AddNodeDisplayName))
            {
                MyRowGUI(args, node);
            }
            else
            {
                MyRowGUI(args, node.displayName);
            }
        }
        private void MyRowGUI(RowGUIArgs args, TreeViewItem node)
        {
            Color oldColor = GUI.color;
            if (!m_IsAddState)
            {
                if (GUI.Button(args.rowRect, node.displayName))
                {
                    m_IsAddState = !m_IsAddState;
                    Repaint();
                }
            }
            else
            {
                GUILayout.BeginArea(args.rowRect);
                using (new EditorGUILayout.HorizontalScope())
                {
                    m_XmlNodeName = EditorGUILayout.TextField(m_XmlNodeName);
                    if (GUILayout.Button("添加", GUILayout.Width(50)))
                    {
                        if (m_XmlNodeName.IsNE())
                        {
                            EditorUtility.DisplayDialog("添加错误", "当前名称为空!", "OK");
                            return;
                        }

                        m_IsAddState = !m_IsAddState;
                        AddXmlNode();
                        Repaint();
                    }
                }
                GUILayout.EndArea();
            }
            GUI.color = oldColor;
        }
        private void MyRowGUI(RowGUIArgs args, string dataIndex)
        {
            Rect rectDeleteBtn = new Rect(args.rowRect.width - 50, args.rowRect.y, 50, args.rowRect.height);
            if (GUI.Button(rectDeleteBtn, "删除"))
            {
                DeleteXmlNode(dataIndex);
            }
        }

        public GameAction<string> OnAdd;
        private void AddXmlNode()
        {
            OnAdd(m_XmlNodeName);
            m_XmlNodeName = "";
        }

        public GameAction<string> OnChange;
        private void SetCurString(string index)
        {
            m_sCurDataName = index;
            OnChange(m_sCurDataName);
        }

        public GameAction<string> OnDelete;
        private void DeleteXmlNode(string dataIndex)
        {
            OnDelete(dataIndex);
        }

        public void RefreshXmlClassList(Dictionary<string, int> _dict)
        {
            m_DesToDictIndex.Clear();
            m_DesToDictIndex = new Dictionary<string, int>(_dict);
            Reload();
            Repaint();
        }

    }

}