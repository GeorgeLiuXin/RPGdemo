using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace Galaxy
{
    public partial class GMCommand : Singleton<GMCommand>
    {
        public delegate void GMCommandFunction(params object[] values);

        public static Dictionary<string, GMCommandFunction> m_GMCommandDict;

        public GMCommand()
        {
            m_GMCommandDict = new Dictionary<string, GMCommandFunction>();
            InitGM();
        }

        public void InitGM()
        {
			if (m_GMCommandDict == null)
				return;

			AddGMCommand("test", GMCommand_Test);
        }

		private void AddGMCommand(string gm, GMCommandFunction func)
		{
			if (m_GMCommandDict.ContainsKey(gm))
			{
				Log.Error("GMCommand Add '{0}' Error!", gm);
				return;
			}

			m_GMCommandDict.Add(gm, func);
		}

		public void HandleGMCommand(string command)
		{
			string[] strs = command.Split(' ');
			if (strs.Length <= 0)
				return;

			string key = strs[0];
			if (!m_GMCommandDict.ContainsKey(key))
			{
				Log.Error("MCommand don't contain this command '{0}'!", key);
				return;
			}
			List<string> list = new List<string>(strs);
			list.RemoveAt(0);
			object[] objs = list.ToArray();
			m_GMCommandDict[key](objs);
		}

    }
}