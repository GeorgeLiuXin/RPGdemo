using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy
{
	public class GameMain : GameBase
	{
		private LevelTest m_tempLevel;

        private Player m_Player;

        private int? m_MainFormId;

		public override GameMode GameMode
		{
			get
			{
				return GameMode.Main;
			}
		}

		public override void Initialize()
		{
			GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
			GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);

			GameEntry.Entity.ShowEntity(
				typeof(Player),
				Constant.Entity.PlayerGroupName,
				Constant.AssetPriority.PlayerAsset,
				new PlayerData(GameEntry.Entity.GenerateSerialId(), 10000, 1)
				{
					Name = "PlayerName",
					Position = Vector3.zero,
				});
			GameOver = false;
			m_Player = null;

			//level temp code
			m_tempLevel = new LevelTest();
			m_tempLevel.Initialize();

			m_MainFormId = GameEntry.UI.OpenUIForm(UIFormId.MainForm);
        }

        public override void Shutdown()
		{
			if(m_MainFormId != null)
            {
                GameEntry.UI.CloseUIForm((int)m_MainFormId);
            }

            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
			GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);

			//level temp code
			m_tempLevel.Shutdown();
        }

		public override void Update(float elapseSeconds, float realElapseSeconds)
		{

		}

		protected void OnShowEntitySuccess(object sender, GameEventArgs e)
		{
			ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
			if(ne.EntityLogicType == typeof(Player))
			{
				m_Player = (Player)ne.Entity.Logic;
				DRScene data = GameEntry.DataTable.GetDataTable<DRScene>().GetDataRow(2);
				m_Player.transform.position = new Vector3(data.PosX, data.PosY, data.PosZ);

                GameEntry.StaicGame.SetLocalPlayer(m_Player.Id);
            }
			else if(ne.EntityLogicType == typeof(Monster))
			{
				m_tempLevel.OnShowEntitySuccess(sender, e);
			}
		}

		protected void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
            if (ne != null)
            {
                Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
            }
        }
    }
}
