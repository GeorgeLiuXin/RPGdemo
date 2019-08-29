using GameFramework.Event;
using System;

namespace Galaxy
{
	public class UIEvent : GameEventArgs
	{
		/// <summary>
		/// UIEvent编号。
		/// </summary>
		public static readonly int EventId = typeof(UIEvent).GetHashCode();

		public override int Id
		{
			get
			{
				return EventId;
			}
		}

		public override void Clear()
		{

		}
	}
}