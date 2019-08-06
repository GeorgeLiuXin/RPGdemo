using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    //////////////////////////////////////////////////////////////////////////
    //Attribute in Animator
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class AnimatorInfoAttribute : Attribute
    {
        protected string description;
        public AnimatorInfoAttribute(string _des)
        {
            description = _des;
        }
        public string Description
        {
            get
            {
                return description;
            }
        }
    }

    public class AnimatorXmlData
    {
        [AnimatorInfo("路径")]
        public string XmlPath;
        [AnimatorInfo("当前数据类型")]
        public int nType;
        [AnimatorInfo("属于哪层动画Layer")]
        public int nLayer;
        [AnimatorInfo("动画融合时长")]
        public float fTime;
        [AnimatorInfo("描述")]
        public string sDes;
    }
    public class AnimatorXmlSetting
    {
        [AnimatorInfo("保存路径")]
        public string SavePath;
        [AnimatorInfo("保存名称")]
        public string SaveName;
        [AnimatorInfo("添加空状态")]
        public bool bSetEmptyState;
    }
}