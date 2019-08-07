using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace Galaxy
{
	public enum AValueType
	{
		AValueType_Int,
		AValueType_Float,
		AValueType_Percent,
		AValueType_Size,
	}

	public enum AvatarAValueDefine
	{
		atk_d,          //攻击
		atk_d_r,        //攻击加成
		d_ac,           //防御
		d_ac_r,         //防御减免
		hp,             //当前生命值
		hpmax,          //最大生命值
		size,
	}

	public enum SkillAValueDefine
	{
		ad_skill_p,     //技能固定值
		ad_skill_r,     //技能系数
		size,
	}

	//Todo 添加最大最小值限制 表结构可优化
	[Serializable]
	public class AValueItem
	{
		public int m_DataID;
		public AValueType m_ValueID;
		public int m_IValue;
		public float m_FValue;
		public double m_Percent;
		public AValueItem()
		{
			m_DataID = 0;
			m_ValueID = AValueType.AValueType_Size;
			m_IValue = 0;
			m_FValue = 0;
			m_Percent = 0;
		}
		public AValueItem(int define, int nValue)
		{
			m_DataID = define;
			m_ValueID = AValueType.AValueType_Int;
			m_IValue = nValue;
			m_FValue = 0;
			m_Percent = 0;
		}
		public AValueItem(int define, float fValue)
		{
			m_DataID = define;
			m_ValueID = AValueType.AValueType_Float;
			m_IValue = 0;
			m_FValue = fValue;
			m_Percent = 0;
		}
		public AValueItem(int define, double fPercent)
		{
			m_DataID = define;
			m_ValueID = AValueType.AValueType_Percent;
			m_IValue = 0;
			m_FValue = 0;
			m_Percent = fPercent;
		}

		//可以外面再加一层，把这层改成模板以优化
		public object GetValue()
		{
			switch(m_ValueID)
			{
				case AValueType.AValueType_Int:
					return m_IValue;
				case AValueType.AValueType_Float:
					return m_FValue;
				case AValueType.AValueType_Percent:
					return m_Percent;
			}
			return null;
		}
		public int GetIntValue()
		{
			if(m_ValueID != AValueType.AValueType_Int)
			{
				Log.Error("属性集 '{0}' 获取type不一致!", m_DataID);
				return 0;
			}
			return m_IValue;
		}
		public float GetFloatValue()
		{
			if(m_ValueID != AValueType.AValueType_Float)
			{
				Log.Error("属性集 '{0}' 获取type不一致!", m_DataID);
				return 0;
			}
			return m_FValue;
		}
		public double GetPercentValue()
		{
			if(m_ValueID != AValueType.AValueType_Percent)
			{
				Log.Error("属性集 '{0}' 获取type不一致!", m_DataID);
				return 0;
			}
			return m_Percent;
		}

		public void SetIntValue(int iValue)
		{
			SetAValueData((int)AValueType.AValueType_Int, iValue);
		}
		public void SetFloatValue(float fValue)
		{
			SetAValueData((int)AValueType.AValueType_Float, fValue);
		}
		public void SetPercentValue(double percent)
		{
			SetAValueData((int)AValueType.AValueType_Percent, percent);
		}
		public void SetAValueData(int type, object value)
		{
			m_ValueID = (AValueType)type;
			switch(m_ValueID)
			{
				case AValueType.AValueType_Int:
					m_IValue = Convert.ToInt32(value);
					return;
				case AValueType.AValueType_Float:
					m_FValue = Convert.ToSingle(value);
					return;
				case AValueType.AValueType_Percent:
					m_Percent = Convert.ToDouble(value);
					return;
			}
			Log.Error("属性集 '{0}' 设置失败!", m_DataID);
			return;
		}

		public void Merge(AValueItem rhs)
		{
			if(m_DataID != rhs.m_DataID || m_ValueID != rhs.m_ValueID)
			{
				Log.Error("属性集合并时参数不一致! lhs:'{0}'  '{1}' rhs:'{2}'  '{3}' ");
				return;
			}
			switch(m_ValueID)
			{
				case AValueType.AValueType_Int:
					m_IValue = rhs.m_IValue;
					return;
				case AValueType.AValueType_Float:
					m_FValue = rhs.m_FValue;
					return;
				case AValueType.AValueType_Percent:
					m_Percent = rhs.m_Percent;
					return;
			}
		}
	}

	public class AValueManager : Singleton<AValueManager>
	{
		public PlayerAValueData GetPlayerAValue()
		{
			PlayerAValueData data = new PlayerAValueData();
			for(int i = 0; i < (int)AvatarAValueDefine.size; i++)
			{
				data.Add((AvatarAValueDefine)i, new AValueItem());
			}
			return data;
		}

		public SkillAValueData GetSkillAValue()
		{
			SkillAValueData data = new SkillAValueData();
			for(int i = 0; i < (int)SkillAValueDefine.size; i++)
			{
				data.Add((SkillAValueDefine)i, new AValueItem());
			}
			return data;
		}
	}

	//更换成模板继承
	[Serializable]
	public class PlayerAValueData : Dictionary<AvatarAValueDefine, AValueItem>, System.ICloneable
	{
		public static Dictionary<string, AvatarAValueDefine> NameToEnum = new Dictionary<string, AvatarAValueDefine>
		{
			{ "atk_d",AvatarAValueDefine.atk_d },
			{ "atk_d_r",AvatarAValueDefine.atk_d_r },
			{ "d_ac",AvatarAValueDefine.d_ac },
			{ "d_ac_r",AvatarAValueDefine.d_ac_r },
			{ "hp",AvatarAValueDefine.hp },
			{ "hpmax",AvatarAValueDefine.hpmax },
		};
		public object Clone()
		{
			return this.MemberwiseClone();
		}
		public PlayerAValueData CloneData()
		{
			return Clone() as PlayerAValueData;
		}
		public void Merge(PlayerAValueData rhs)
		{
			foreach(var item in rhs)
			{
				if(!ContainsKey(item.Key))
					continue;
				this[item.Key].Merge(item.Value);
			}
		}

		public int GetIntValue(AvatarAValueDefine define)
		{
			if(define < 0 || define >= AvatarAValueDefine.size || !ContainsKey(define))
			{
				Log.Error("属性集参数传递有误 '{0}' !", define);
				return 0;
			}
			return this[define].GetIntValue();
		}
		public float GetFloatValue(AvatarAValueDefine define)
		{
			if(define < 0 || define >= AvatarAValueDefine.size || !ContainsKey(define))
			{
				Log.Error("属性集参数传递有误 '{0}' !", define);
				return 0;
			}
			return this[define].GetFloatValue();
		}
		public double GetPercentValue(AvatarAValueDefine define)
		{
			if(define < 0 || define >= AvatarAValueDefine.size || !ContainsKey(define))
			{
				Log.Error("属性集参数传递有误 '{0}' !", define);
				return 0;
			}
			return this[define].GetPercentValue();
		}


		public void SetIntValue(string define, int iValue)
		{
			if(!NameToEnum.ContainsKey(define))
			{
				Log.Error("属性集参数传递有误 '{0}' !", define);
				return;
			}
			SetIntValue(NameToEnum[define], iValue);
		}
		public void SetIntValue(AvatarAValueDefine define, int iValue)
		{
			SetAValueData(define, (int)AValueType.AValueType_Int, iValue);
		}
		public void SetFloatValue(string define, float fValue)
		{
			if(!NameToEnum.ContainsKey(define))
			{
				Log.Error("属性集参数传递有误 '{0}' !", define);
				return;
			}
			SetFloatValue(NameToEnum[define], fValue);
		}
		public void SetFloatValue(AvatarAValueDefine define, float fValue)
		{
			SetAValueData(define, (int)AValueType.AValueType_Float, fValue);
		}
		public void SetPercentValue(string define, double percent)
		{
			if(!NameToEnum.ContainsKey(define))
			{
				Log.Error("属性集参数传递有误 '{0}' !", define);
				return;
			}
			SetPercentValue(NameToEnum[define], percent);
		}
		public void SetPercentValue(AvatarAValueDefine define, double percent)
		{
			SetAValueData(define, (int)AValueType.AValueType_Percent, percent);
		}
		public void SetAValueData(string define, int type, object value)
		{
			if(!NameToEnum.ContainsKey(define))
			{
				Log.Error("属性集参数传递有误 '{0}' !", define);
				return;
			}
			SetAValueData(NameToEnum[define], type, value);
		}
		public void SetAValueData(AvatarAValueDefine define, int type, object value)
		{
			if(define < 0 || define >= AvatarAValueDefine.size || !ContainsKey(define))
			{
				Log.Error("属性集参数传递有误 '{0}' !", define);
				return;
			}
			this[define].SetAValueData(type, value);
		}
	}

	[Serializable]
	public class SkillAValueData : Dictionary<SkillAValueDefine, AValueItem>, System.ICloneable
	{
		public static Dictionary<string, SkillAValueDefine> NameToEnum = new Dictionary<string, SkillAValueDefine>
		{
			{ "ad_skill_p",SkillAValueDefine.ad_skill_p },
			{ "ad_skill_r",SkillAValueDefine.ad_skill_r },
		};
		public object Clone()
		{
			return this.MemberwiseClone();
		}
		public SkillAValueData CloneData()
		{
			return Clone() as SkillAValueData;
		}

		public int GetIntValue(SkillAValueDefine define)
		{
			if(define < 0 || define >= SkillAValueDefine.size || !ContainsKey(define))
			{
				Log.Error("属性集参数传递有误 '{0}' !", define);
				return 0;
			}
			return this[define].GetIntValue();
		}
		public float GetFloatValue(SkillAValueDefine define)
		{
			if(define < 0 || define >= SkillAValueDefine.size || !ContainsKey(define))
			{
				Log.Error("属性集参数传递有误 '{0}' !", define);
				return 0;
			}
			return this[define].GetFloatValue();
		}
		public double GetPercentValue(SkillAValueDefine define)
		{
			if(define < 0 || define >= SkillAValueDefine.size || !ContainsKey(define))
			{
				Log.Error("属性集参数传递有误 '{0}' !", define);
				return 0;
			}
			return this[define].GetPercentValue();
		}

		public void SetAValueData(string define, int type, object value)
		{
			if(!NameToEnum.ContainsKey(define))
			{
				Log.Error("属性集参数传递有误 '{0}' !", define);
				return;
			}
			SetAValueData(NameToEnum[define], type, value);
		}
		public void SetAValueData(SkillAValueDefine define, int type, object value)
		{
			if(define < 0 || define >= SkillAValueDefine.size || !ContainsKey(define))
			{
				Log.Error("属性集参数传递有误 '{0}' !", define);
				return;
			}
			this[define].SetAValueData(type, value);
		}
	}

	//public class AValueList : List<AValueItem>
	//{

	//}

	//public class AValueData : Dictionary<int, AValueList>
	//{
	//	public void AddModifyItem(AValueItem modifyItem)
	//	{
	//		AValueList modifyList = null;
	//		if(!TryGetValue(modifyItem.m_ModifyID, out modifyList))
	//		{
	//			modifyList = new AValueList();
	//			Add(modifyItem.m_ModifyID, modifyList);
	//		}
	//		modifyList.Add(modifyItem);
	//	}
	//	public AValueList GetModifyList(int modifyID)
	//	{
	//		AValueList modifyList = null;
	//		if(TryGetValue(modifyID, out modifyList))
	//			return modifyList;
	//		return null;
	//	}
	//}

}