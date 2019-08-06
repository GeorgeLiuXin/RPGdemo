using UnityEngine;

namespace Galaxy
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
	public partial class GameEntry : MonoBehaviour
	{
		public static BuiltinDataComponent BuiltinData
		{
			get;
			private set;
		}

		public static TimerManager TimerMgr
		{
			get;
			private set;
		}

		public static CameraManager CameraMgr
		{
			get;
			private set;
		}

		public static StaticGameComponent StaicGame
		{
			get;
			private set;
		}

		public static FsmManager fsmMgr
		{
			get;
			private set;
		}

		//自定义的Component类
		private static void InitCustomComponents()
		{
			BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
			TimerMgr = UnityGameFramework.Runtime.GameEntry.GetComponent<TimerManager>();
			CameraMgr = UnityGameFramework.Runtime.GameEntry.GetComponent<CameraManager>();
			StaicGame = UnityGameFramework.Runtime.GameEntry.GetComponent<StaticGameComponent>();
			StaicGame.InitGameDataManager();
			fsmMgr = UnityGameFramework.Runtime.GameEntry.GetComponent<FsmManager>();
		}
	}
}
