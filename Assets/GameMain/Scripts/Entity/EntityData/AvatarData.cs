using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy
{
	[Serializable]
	public abstract class AvatarData : EntityData
	{
		[SerializeField]
		protected PlayerAValueData m_AValue;
		[SerializeField]
		private CampType m_Camp = CampType.Unknown;
		
		public AvatarData(int entityId, int typeId, CampType camp)
			: base(entityId, typeId)
		{
			m_Camp = camp;
			m_AValue = AValueManager.Instance.GetPlayerAValue();
		}

		protected abstract int AValueDataID { get; }
		protected void UpdateAValueByInstance()
		{
			List<DRAvatarAValue> list = new List<DRAvatarAValue>
				(GameEntry.DataTable.GetDataTable<DRAvatarAValue>().GetDataRows(CheckAValueDataID));
			UpdateAValue(list);
		}
		private bool CheckAValueDataID(DRAvatarAValue data)
		{
			return data.AValueID == AValueDataID;
		}

		/// <summary>
		/// 获取属性集
		/// </summary>
		/// <returns></returns>
		public PlayerAValueData GetAValue()
		{
			return m_AValue;
		}

		public void UpdateAValue(List<DRAvatarAValue> list)
		{
			if(m_AValue == null)
				return;
			if(list == null || list.Count == 0)
				return;
			foreach(var item in list)
			{
				object value;
				switch((AValueType)item.AValueType)
				{
					case AValueType.AValueType_Int:
						value = item.AValueInt;
						break;
					case AValueType.AValueType_Float:
						value = item.AValueFloat;
						break;
					case AValueType.AValueType_Percent:
						value = item.AValuePercent;
						break;
					default:
						Log.Error("刷新目标属性集 '{0}' '{1}' 错误!", Id, TypeId);
						return;
				}
				m_AValue.SetAValueData(item.AvatarAValueDefine, item.AValueType, value);
			}
		}

		/// <summary>
		/// 角色阵营。
		/// </summary>
		public CampType Camp
		{
			get
			{
				return m_Camp;
			}
		}

		/// <summary>
		/// 当前生命。
		/// </summary>
		public float HP
		{
			get
			{
				if(m_AValue != null)
				{
					return m_AValue.GetFloatValue(AvatarAValueDefine.hp);
				}
				return 0;
			}
			set
			{
				if(m_AValue == null)
				{
					Log.Error("当前设置目标 '{0}' '{1}' 属性集未初始化!", Id, TypeId);
					return;
				}
				m_AValue.SetFloatValue(AvatarAValueDefine.hp, value);
			}
		}

		/// <summary>
		/// 最大生命。
		/// </summary>
		public float MaxHP
		{
			get
			{
				if(m_AValue != null)
				{
					return m_AValue.GetFloatValue(AvatarAValueDefine.hpmax);
				}
				return 0;
			}
		}

		/// <summary>
		/// 生命百分比。
		/// </summary>
		public float HPRatio
		{
			get
			{
				return MaxHP > 0 ? HP / MaxHP : 0f;
			}
		}

		public void SetAValueData(AvatarAValueDefine define, int type, object value)
		{
			m_AValue.SetAValueData(define, type, value);
		}
		public int GetIntValue(AvatarAValueDefine define)
		{
			return m_AValue.GetIntValue(define);
		}
		public float GetFloatValue(AvatarAValueDefine define)
		{
			return m_AValue.GetFloatValue(define);
		}
		public double GetPercentValue(AvatarAValueDefine define)
		{
			return m_AValue.GetPercentValue(define);
		}
	}
}