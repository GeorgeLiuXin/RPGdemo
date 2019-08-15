using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy
{
	public class StaticGameComponent : GameFrameworkComponent
	{
		//temp
		public GameObject m_effectClick;

        public int m_LocalPlayerID
        {
            get;
            private set;
        }

		// Use this for initialization
		void Start()
		{
			//ModifyDataManager.Instance.InitDefine();
		}

		public void InitGameDataManager()
		{

		}

		void Update()
		{
			
		}

		void OnDestroy()
		{
			
		}

        public void SetLocalPlayer(int player)
        {
            m_LocalPlayerID = player;
        }
	}
}
