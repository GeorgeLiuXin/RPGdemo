using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEditor;
using Galaxy.XmlData;
using System.Reflection;

namespace Galaxy
{
	public class AutoGenerateViewTree : TreeView
	{
		// All columns
		enum propertyColumns
		{
			ResourcePath,
			DataType,
			AnimationLayer,
			TransitionDuration,
			Describe,
			DeleteBtn,
		}

		enum ResourceType
		{
			Floder,
			Animation,
		}
		enum AnimationLayer
		{
			Layer_Base,
			Layer_Up,
		}

		private string[] m_ResourceTypes = { ResourceType.Floder.ToString(), ResourceType.Animation.ToString() };
		private int[] m_ResourceTypeIndex = { 1, 2 };
		private string[] m_AnimationLayers = { AnimationLayer.Layer_Base.ToString(), AnimationLayer.Layer_Up.ToString() };
		private int[] m_AnimationLayerIndex = { 0, 1 };

		private GUIStyle style;
		public XmlDataList m_data;

		public AutoGenerateViewTree(TreeViewState state, MultiColumnHeaderState mchs)
			: base(state, new MultiColumnHeader(mchs))
		{
			showBorder = true;
			showAlternatingRowBackgrounds = true;
			DefaultStyles.label.richText = true;

			style = new GUIStyle(GUI.skin.button);
			style.alignment = TextAnchor.MiddleLeft;

			m_data = null;
			Reload();
		}

		protected override TreeViewItem BuildRoot()
		{
			TreeViewItem root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
			root.children = new List<TreeViewItem>();
			if(m_data != null)
			{
				List<XmlClassData>.Enumerator itor = m_data.GetEnumerator();

				while(itor.MoveNext())
				{
					XmlClassData _class = itor.Current;
					if(_class == null)
						continue;
					if(!_class.sLogicName.Equals("AnimatorXmlData"))
						continue;

					AnimatorDataViewTreeItem arrayItem = new AnimatorDataViewTreeItem(_class, 0);
					root.AddChild(arrayItem);
				}
			}
			return root;
		}

		public override void OnGUI(Rect rect)
		{
			base.OnGUI(rect);
		}

		protected override void RowGUI(RowGUIArgs args)
		{
			base.RowGUI(args);

			AnimatorDataViewTreeItem treeItem = args.item as AnimatorDataViewTreeItem;
			if(treeItem == null)
				return;
			for(int i = 0; i < args.GetNumVisibleColumns(); ++i)
				CellGUI(args.GetCellRect(i), treeItem, args.GetColumn(i));
		}

		private void CellGUI(Rect cellRect, AnimatorDataViewTreeItem treeitem, int column)
		{
			CenterRectUsingSingleLineHeight(ref cellRect);

			AnimatorDataItem item;
			switch((propertyColumns)column)
			{
				case propertyColumns.ResourcePath:
					item = treeitem.m_items[column];
					if(item == null)
						return;
					if(item.param.sType == "System.Boolean")
					{
						bool.TryParse(item.param.sValue, out item.bValue);
						item.bValue = EditorGUI.Toggle(cellRect, item.bValue);
						item.param.sValue = item.bValue.ToString();
					}
					else if(item.param.sType == "System.Int32")
					{
						int.TryParse(item.param.sValue, out item.nValue);
						item.nValue = EditorGUI.IntField(cellRect, item.nValue);
						item.param.sValue = item.nValue.ToString();
					}
					else if(item.param.sType == "System.Single")
					{
						float.TryParse(item.param.sValue, out item.fValue);
						item.fValue = EditorGUI.FloatField(cellRect, item.fValue);
						item.param.sValue = item.fValue.ToString();
					}
					else if(item.param.sType == "System.String")
					{
						item.sValue = item.param.sValue;
						item.sValue = EditorGUI.TextField(cellRect, item.sValue);
						item.param.sValue = item.sValue;
					}
					break;
				case propertyColumns.DataType:
					item = treeitem.m_items[column];
					if(item == null)
						return;
					if(item.param.sType == "System.Int32")
					{
						int.TryParse(item.param.sValue, out item.nValue);
						item.nValue = EditorGUI.IntPopup(cellRect, item.nValue, m_ResourceTypes, m_ResourceTypeIndex);
						item.param.sValue = item.nValue.ToString();
					}
					break;
				case propertyColumns.AnimationLayer:
					item = treeitem.m_items[column];
					if(item == null)
						return;
					if(item.param.sType == "System.Int32")
					{
						int.TryParse(item.param.sValue, out item.nValue);
						item.nValue = EditorGUI.IntPopup(cellRect, item.nValue, m_AnimationLayers, m_AnimationLayerIndex);
						item.param.sValue = item.nValue.ToString();
					}
					break;
				case propertyColumns.TransitionDuration:
					item = treeitem.m_items[column];
					if(item == null)
						return;
					if(item.param.sType == "System.Single")
					{
						float.TryParse(item.param.sValue, out item.fValue);
						item.fValue = EditorGUI.FloatField(cellRect, item.fValue);
						item.param.sValue = item.fValue.ToString();
					}
					break;
				case propertyColumns.Describe:
					item = treeitem.m_items[column];
					if(item == null)
						return;
					if(item.param.sType == "System.String")
					{
						item.sValue = item.param.sValue;
						item.sValue = EditorGUI.TextField(cellRect, item.sValue);
						item.param.sValue = item.sValue;
					}
					break;
				case propertyColumns.DeleteBtn:
					Rect rectDeleteBtn = new Rect(cellRect.x + 5, cellRect.y, 50, cellRect.height);
					if(GUI.Button(rectDeleteBtn, "删除"))
					{
						if(EditorUtility.DisplayDialog("删除", "是否删除当前行?", "是", "否"))
						{
							RemoveClassData(treeitem.m_class.sLogicName);
						}
					}
					break;
				default:
					break;
			}
		}

		public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState()
		{
			return new MultiColumnHeaderState(GetColumns());
		}

		private static MultiColumnHeaderState.Column[] GetColumns()
		{
			var retVal = new MultiColumnHeaderState.Column[] {
				new MultiColumnHeaderState.Column(),
				new MultiColumnHeaderState.Column(),
				new MultiColumnHeaderState.Column(),
				new MultiColumnHeaderState.Column(),
				new MultiColumnHeaderState.Column(),
				new MultiColumnHeaderState.Column()
			};
			retVal[0].headerContent = new GUIContent("路径", "");
			retVal[0].minWidth = 375;
			retVal[0].width = 420;
			retVal[0].maxWidth = 720;
			retVal[0].headerTextAlignment = TextAlignment.Left;
			retVal[0].canSort = false;
			retVal[0].autoResize = true;

			retVal[1].headerContent = new GUIContent("类型", "");
			retVal[1].minWidth = 80;
			retVal[1].width = 100;
			retVal[1].maxWidth = 125;
			retVal[1].headerTextAlignment = TextAlignment.Left;
			retVal[1].canSort = false;
			retVal[1].autoResize = true;

			retVal[2].headerContent = new GUIContent("类型", "");
			retVal[2].minWidth = 80;
			retVal[2].width = 100;
			retVal[2].maxWidth = 125;
			retVal[2].headerTextAlignment = TextAlignment.Left;
			retVal[2].canSort = false;
			retVal[2].autoResize = true;

			retVal[3].headerContent = new GUIContent("融合时长", "");
			retVal[3].minWidth = 50;
			retVal[3].width = 60;
			retVal[3].maxWidth = 80;
			retVal[3].headerTextAlignment = TextAlignment.Left;
			retVal[3].canSort = false;
			retVal[3].autoResize = true;

			retVal[4].headerContent = new GUIContent("描述", "");
			retVal[4].minWidth = 60;
			retVal[4].width = 100;
			retVal[4].maxWidth = 120;
			retVal[4].headerTextAlignment = TextAlignment.Left;
			retVal[4].canSort = false;
			retVal[4].autoResize = true;

			retVal[5].headerContent = new GUIContent("删除", "");
			retVal[5].minWidth = 60;
			retVal[5].width = 80;
			retVal[5].maxWidth = 100;
			retVal[5].headerTextAlignment = TextAlignment.Left;
			retVal[5].canSort = false;
			retVal[5].autoResize = true;

			return retVal;
		}


		public void RefreshByNewData(ref XmlDataList data)
		{
			m_data = data;
			Reload();
			Repaint();
		}

		public void AddNewClassData()
		{
			AnimatorXmlData data = new AnimatorXmlData();
			System.Reflection.FieldInfo[] m_DataFields = data.GetType().GetFields();
			XmlClassData list = new XmlClassData();
			list.sLogicName = "AnimatorXmlData";
			foreach(var field in m_DataFields)
			{
				XmlParamItem item = new XmlParamItem();
				item.sName = field.Name;
				item.sType = field.FieldType.ToString();
				item.sValue = "";
				list.Add(item);
			}
			m_data.SafeAdd(list);

			Reload();
			Repaint();
		}

		public void RemoveClassData(string logicName)
		{
			XmlClassData list = null;
			foreach(var item in m_data)
			{
				if(item.sLogicName.Equals(logicName))
				{
					list = item;
					break;
				}
			}
			if(list != null)
			{
				m_data.Remove(list);
			}
			Reload();
			Repaint();
		}

		public class AnimatorDataViewTreeItem : TreeViewItem
		{
			public XmlClassData m_class;
			public List<AnimatorDataItem> m_items;

			public AnimatorDataViewTreeItem(XmlClassData _class, int _depth)
			{
				m_class = _class;
				base.depth = _depth;

				m_items = new List<AnimatorDataItem>();
				foreach(XmlParamItem item in m_class)
				{
					AnimatorDataItem dataItem = new AnimatorDataItem(item);
					m_items.Add(dataItem);
				}
			}
		}

		public class AnimatorDataItem
		{
			public bool bValue;
			public int nValue;
			public float fValue;
			public string sValue;
			public XmlParamItem param;

			public AnimatorDataItem(XmlParamItem _param)
			{
				param = _param;
			}
		}

	}

}