using System.Collections.Generic;

namespace Galaxy
{
	//暂时 之后修改为slot修正后修复为管理所有技能数据
	public class GSkillDataManager : Singleton<GSkillDataManager>
	{
		private bool m_bInit;
		private Dictionary<int, List<int>> m_SubSkillDict;

		public GSkillDataManager()
		{
			m_bInit = false;
			m_SubSkillDict = new Dictionary<int, List<int>>();
		}

		public void InitData()
		{
			if(m_bInit)
				return;
			List<DRSkillData> list = new List<DRSkillData>();
			GameEntry.DataTable.GetDataTable<DRSkillData>().GetAllDataRows(list);
			if(list == null || list.Count == 0)
				return;
			foreach(var item in list)
			{
				if(m_SubSkillDict.ContainsKey(item.MSV_BaseSkillID))
				{
					m_SubSkillDict[item.MSV_BaseSkillID].Add(item.Id);
				}
				else
				{
					List<int> subList = new List<int>();
					subList.Add(item.Id);
					m_SubSkillDict.Add(item.MSV_BaseSkillID, subList);
				}
			}
			m_bInit = true;
		}

		public void ClearData()
		{
			m_SubSkillDict.Clear();
			m_bInit = false;
		}

		public List<int> GetSubSkillList(int nSkillID)
		{
			if(m_SubSkillDict.ContainsKey(nSkillID))
			{
				return m_SubSkillDict[nSkillID];
			}
			return null;
		}
	}


}

#region 旧数据结构
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Collections;

//namespace Galaxy
//{
//	public class SkillDataManager : ModifyDataDict_T<SkillData>
//    {
//        private ModifyDict m_SkillSlotsModifyDict; //技能强化表
//        private Dictionary<int, List<int>> m_SubSkillDict;

//        public SkillDataManager()
//        {
//            m_SkillSlotsModifyDict = new ModifyDict();
//            m_SubSkillDict = new Dictionary<int, List<int>>();
//        }

//        public void OnLoadSkillData(ConfigData data)
//        {
//            SkillData skillData = new SkillData();
//            if (skillData != null && data != null)
//            {
//                skillData.OnLoadData(data);
//                AddData(skillData.SkillID, 1, skillData);
//                //插入子技能
//                AddSubSkill(skillData);
//            }   
//		}
//        public void OnLoadSkillSlotsData(ConfigData data)
//        {
//            ModifyItem modifyItem = new ModifyItem();
//            if (modifyItem == null)
//                return;

//            modifyItem.m_DataID = data.GetInt("DataID");
//            modifyItem.m_ModifyID = data.GetInt("ModifyID");
//            string valueName = data.GetString("ValueName");
//            modifyItem.m_ValueID = SkillData.GetModifyValueID(valueName);
//            modifyItem.m_IValue = data.GetInt("IValue");
//            modifyItem.m_FValue = data.GetFloat("FValue");
//            modifyItem.m_Precent = data.GetFloat("Precent");

//            m_SkillSlotsModifyDict.AddModifyItem(modifyItem);

//            SkillData skillData = GetData(modifyItem.m_DataID);
//            if (skillData != null)
//            {
//                skillData.m_nSlotsMask |= modifyItem.m_ModifyID;

//                SkillData baseSkill = GetData(skillData.MSV_BaseSkillID);
//                if (baseSkill != null)
//                {
//                    baseSkill.m_nSlotsMask |= modifyItem.m_ModifyID;
//                }
//            }
//        }

//        public ModifyData GetSkillSlotsData(int nDataID)
//        {
//            return m_SkillSlotsModifyDict.GetModifyData(nDataID);
//        }

//        public void AddSubSkill(SkillData data)
//        {
//            if (data == null)
//                return;

//            if (data.MSV_BaseSkillID <= 0)
//                return;

//            List<int> list;
//            if (!m_SubSkillDict.TryGetValue(data.MSV_BaseSkillID, out list))
//            {
//                list = new List<int>();
//                m_SubSkillDict.Add(data.MSV_BaseSkillID, list);
//            }
//            list.Add(data.DataID);
//        }

//        public List<int> GetSubSkillList(int nSkillID)
//        {
//            List<int> list;
//            if (m_SubSkillDict.TryGetValue(nSkillID, out list))
//                return list;
//            return null;
//        }

//        public override void ClearData()
//        {
//            base.ClearData();
//            m_SkillSlotsModifyDict.Clear();
//            m_SubSkillDict.Clear();
//        }
//    }
//}
#endregion
