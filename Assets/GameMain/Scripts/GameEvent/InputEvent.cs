using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System;

namespace Galaxy
{
	public class MouseEvent : GameEventArgs
	{
		/// <summary>
		/// 鼠标事件编号。
		/// </summary>
		public static readonly int EventId = typeof(MouseEvent).GetHashCode();

		/// <summary>
		/// 获取鼠标事件编号。
		/// </summary>
		public override int Id
		{
			get
			{
				return EventId;
			}
		}
		
		public override void Clear()
		{
			throw new NotImplementedException();
		}
	}

}