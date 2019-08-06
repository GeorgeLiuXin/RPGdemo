using System;
using Galaxy.XmlData;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Galaxy
{
    public partial class AnimatorAutoGenerateEditorWindow : EditorWindow
    {
		public enum eLayer
		{
			Layer_Base,
			Layer_Up,
		}
		private AnimatorXmlSetting m_xmlSetting;
        private List<AnimatorXmlData> m_xmlDataList;
        private Dictionary<int, AnimatorState> m_LayerDefaultState;

        public void Generate()
        {
            if (m_CurData == null || m_CurData.Count <= 1)
                return;

            if (m_LayerDefaultState != null)
            {
                m_LayerDefaultState.Clear();
            }
            if (m_dictFBXFiles != null)
            {
                m_dictFBXFiles.Clear();
            }
            m_FBXFilesCount = 0;

            InitFBXFilesData(m_CurData);
            ReadAllFBXFiles();
            if (m_FBXFilesCount <= 0)
            {
                Debug.LogError("目录中没有动画文件！");
                return;
            }
            AnimatorController ac = CreateAnimatorController();
            if (ac == null)
            {
                Debug.LogError("AnimatorController创建失败！");
                return;
            }
            BuildAnimatorController(ac);
            Debug.Log("创建结束！");
        }

        private void InitFBXFilesData(XmlDataList list)
        {
            m_xmlSetting = new AnimatorXmlSetting();
            m_xmlDataList = new List<AnimatorXmlData>();
            m_LayerDefaultState = new Dictionary<int, AnimatorState>();
            foreach (var item in list)
            {
                if (item.sLogicName.Equals("AnimatorXmlSetting"))
                {
                    SetLogicFieldInfo(m_xmlSetting, item);
                }
                if (item.sLogicName.Equals("AnimatorXmlData"))
                {
                    AnimatorXmlData data = new AnimatorXmlData();
                    SetLogicFieldInfo(data, item);
                    m_xmlDataList.Add(data);
                }
            }
        }
        private void SetLogicFieldInfo<T>(T logic, XmlClassData data)
        {
            System.Reflection.FieldInfo field;
            foreach (var item in data)
            {
                field = logic.GetType().GetField(item.sName);
                if (field != null)
                {
                    if (item.sType.Equals("System.Boolean"))
                    {
                        field.SetValue(logic, Convert.ToBoolean(item.sValue));
                    }
                    else if (item.sType.Equals("System.Int32"))
                    {
                        field.SetValue(logic, Convert.ToInt32(item.sValue));
                    }
                    else if (item.sType.Equals("System.Single"))
                    {
                        field.SetValue(logic, Convert.ToSingle(item.sValue));
                    }
                    else if (item.sType.Equals("System.String"))
                    {
                        field.SetValue(logic, item.sValue);
                    }
                }
            }
        }

        private class AnimationClipAndInfo
        {
            public AnimatorXmlData m_Xmldata;
            public Dictionary<string, AnimationClip> m_Animations;

            public AnimationClipAndInfo(AnimatorXmlData _Xmldata)
            {
                m_Xmldata = _Xmldata;
                m_Animations = new Dictionary<string, AnimationClip>();
            }
        }
        private Dictionary<int, AnimationClipAndInfo> m_dictFBXFiles = new Dictionary<int, AnimationClipAndInfo>();
        int m_FBXFilesCount;

        private void ReadAllFBXFiles()
        {
            for (int i = 0; i < m_xmlDataList.Count; i++)
            {
                var item = m_xmlDataList[i];
                if (item == null)
                    continue;

                string strPath = item.XmlPath.Replace("\\", "/");
                ReadFBXFiles(strPath, item, i);
            }
        }
        private void ReadFBXFiles(string strFBXPath, AnimatorXmlData data, int nGroupID)
        {
            if (!Directory.Exists(strFBXPath))
                return;

            AnimationClipAndInfo clipAndinfo = new AnimationClipAndInfo(data);
            m_dictFBXFiles.Add(nGroupID, clipAndinfo);

            DirectoryInfo direction = new DirectoryInfo(strFBXPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
				if(files[i].Name.EndsWith(".anim"))
				{
					string strAnimName = files[i].Name.Remove(files[i].Name.LastIndexOf('.'));
					if(clipAndinfo.m_Animations.ContainsKey(strAnimName))
					{
						Debug.LogError("有重复的文件名称：" + strAnimName);
						continue;
					}

					string strFilePath = files[i].ToString().Replace("\\", "/");
					strFilePath = strFilePath.Substring(strFilePath.IndexOf("Assets"));
					AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(strFilePath);
					if(clip != null)
					{
						clipAndinfo.m_Animations.Add(strAnimName, clip);
						++m_FBXFilesCount;
					}
				}
				else if(files[i].Name.EndsWith(".FBX"))
				{
					string strFilePath = files[i].ToString().Replace("\\", "/");
					strFilePath = strFilePath.Substring(strFilePath.IndexOf("Assets"));
					UnityEngine.Object[] datas = AssetDatabase.LoadAllAssetsAtPath(strFilePath);
					if(datas.Length == 0)
					{
						Debug.Log(string.Format("Can't find clip in {0}", strFBXPath));
						return;
					}
					foreach(var item in datas)
					{
						if(!(item is AnimationClip))
							continue;
						var clip = item as AnimationClip;
						// 取出动画名字，添加到state里面
						if(clip != null)
						{
							clipAndinfo.m_Animations.Add(clip.name, clip);
							++m_FBXFilesCount;
						}
					}
				}
            }
        }
        private AnimatorController CreateAnimatorController()
        {
            if (!Directory.Exists(m_xmlSetting.SavePath))
            {
                Directory.CreateDirectory(m_xmlSetting.SavePath);
            }
            if (string.IsNullOrEmpty(m_xmlSetting.SaveName))
            {
                StringBuilder strMD5 = new StringBuilder();
                MD5 md5Provider = new MD5CryptoServiceProvider();
                byte[] outputHash = md5Provider.ComputeHash(Encoding.Default.GetBytes(DateTime.Now.ToString()));
                for (int i = 0; i < outputHash.Length; i++)
                {
                    strMD5.Append(outputHash[i].ToString("X"));
                }
                m_xmlSetting.SaveName = strMD5.ToString();
            }

            AnimatorController ac = AnimatorController.CreateAnimatorControllerAtPath(m_xmlSetting.SavePath + "/" + m_xmlSetting.SaveName + ".controller");
            ac.AddLayer("Up Layer");

            //每层添加空状态
            if (m_xmlSetting.bSetEmptyState)
            {
                for (int i = 0; i < ac.layers.Length; i++)
                {
                    AddDefaultState(ac, i);
                }
            }

            return ac;
        }
        private void AddDefaultState(AnimatorController ac, int i)
        {
            AnimatorControllerLayer layer = ac.layers[i];
            AnimatorStateMachine stateMachine = layer.stateMachine;
            layer.defaultWeight = 1.0f;

            string strEmptyNameParam = "Empty" + i;

            //每层必须先加一个空状态
            AnimatorState stateEmpty = stateMachine.AddState(strEmptyNameParam, new Vector3(stateMachine.entryPosition.x + 300, stateMachine.entryPosition.y, 0));

            //添加trigger参数
            ac.AddParameter(strEmptyNameParam, AnimatorControllerParameterType.Trigger);

            //添加速度修正参数
            AnimatorControllerParameter acpEmpty = new AnimatorControllerParameter();
            string strSpeedParamEmpty = strEmptyNameParam + "speed";
            acpEmpty.name = strSpeedParamEmpty;
            acpEmpty.defaultFloat = 1.0f;
            acpEmpty.type = AnimatorControllerParameterType.Float;
            ac.AddParameter(acpEmpty);

            //添加Any到Empty的transition
            AnimatorStateTransition ToEmptyTransition = stateMachine.AddAnyStateTransition(stateEmpty);
            ToEmptyTransition.AddCondition(AnimatorConditionMode.If, 0, strEmptyNameParam);
            ToEmptyTransition.duration = 0f;
            ToEmptyTransition.name = strEmptyNameParam;

            m_LayerDefaultState.Add(i, stateEmpty);
        }

        private void BuildAnimatorController(AnimatorController ac)
        {
            Dictionary<int, int> nCountToLayer = new Dictionary<int, int>();
            foreach (var item in m_dictFBXFiles)
            {
                if (item.Value == null || item.Value.m_Animations.Count == 0)
                    return;

                int nCurLayer = item.Value.m_Xmldata.nLayer;
                if (!nCountToLayer.ContainsKey(nCurLayer))
                {
                    nCountToLayer.Add(nCurLayer, 0);
                }

                AnimatorControllerLayer layer = ac.layers[nCurLayer];
                AnimatorStateMachine stateMachine = layer.stateMachine;
                layer.defaultWeight = 1.0f;

                int nAnimCount = 0;
                foreach (var animationInfo in item.Value.m_Animations)
                {
                    string animName = animationInfo.Key;

                    //添加trigger参数
                    ac.AddParameter(animName, AnimatorControllerParameterType.Trigger);

                    //添加速度修正参数
                    AnimatorControllerParameter acp = new AnimatorControllerParameter();
                    string strSpeedParam = animName + "speed";
                    acp.name = strSpeedParam;
                    acp.defaultFloat = 1.0f;
                    acp.type = AnimatorControllerParameterType.Float;
                    ac.AddParameter(acp);

                    //添加state
                    AnimatorState state = stateMachine.AddState(animName, new Vector3(stateMachine.entryPosition.x + 300 * nCountToLayer[nCurLayer], stateMachine.entryPosition.y + 50 * (nAnimCount + 1), 0));
                    state.motion = animationInfo.Value;
                    state.speedParameterActive = true;
                    state.speedParameter = strSpeedParam;

                    //添加transition
                    AnimatorStateTransition animatorStateTransition = stateMachine.AddAnyStateTransition(state);
                    animatorStateTransition.AddCondition(AnimatorConditionMode.If, 0, animName);
                    animatorStateTransition.name = animName;
                    if (item.Value.m_Xmldata.fTime == 0)
                    {
                        animatorStateTransition.duration = 0.01f;
                    }
                    else
                    {
                        animatorStateTransition.duration = item.Value.m_Xmldata.fTime;
                    }

                    //填充EmptyState的动画
                    if (m_xmlSetting.bSetEmptyState)
                    {
                        if (animName == "basic_stand" && m_LayerDefaultState.ContainsKey(nCurLayer))
                        {
                            m_LayerDefaultState[nCurLayer].motion = animationInfo.Value;
                        }
                        if (nCurLayer == (int)eLayer.Layer_Up)
                        {
                            AnimatorStateTransition toEmpty = state.AddTransition(m_LayerDefaultState[nCurLayer]);
                            toEmpty.hasExitTime = true;
                        }
                    }
                    //行位置增加
                    nAnimCount++;
                }
                //列位置增加
                nCountToLayer[item.Value.m_Xmldata.nLayer]++;
            }
        }
    }
}