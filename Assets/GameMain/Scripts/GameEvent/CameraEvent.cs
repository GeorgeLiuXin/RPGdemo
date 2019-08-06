using GameFramework.Event;

namespace Galaxy
{
	public class CameraEvent : GameEventArgs
	{
		/// <summary>
		/// 相机事件编号。
		/// </summary>
		public static readonly int EventId = typeof(CameraEvent).GetHashCode();

		public override int Id
		{
			get
			{
				return EventId;
			}
		}

		/// <summary>
		/// 相机挂载的实体。
		/// </summary>
		public Player Player
		{
			get;
			private set;
		}

		/// <summary>
		/// 相机是否重置目标。
		/// </summary>
		public bool Reset
		{
			get;
			private set;
		}

		/// <summary>
		/// 获取用户自定义数据。
		/// </summary>
		public object UserData
		{
			get;
			private set;
		}

		/// <summary>
		/// 清理相机事件。
		/// </summary>
		public override void Clear()
		{
			Player = default(Player);
			Reset = default(bool);
			UserData = default(object);
		}

		/// <summary>
		/// 填充相机事件。
		/// </summary>
		/// <param name="player">要跟随的玩家</param>
		/// <param name="reset">是否重置</param>
		/// <param name="userData">自定义数据</param>
		public CameraEvent Fill(Player player, bool reset, object userData)
		{
			Player = player;
			Reset = reset;
			UserData = userData;
			return this;
		}
	}
}