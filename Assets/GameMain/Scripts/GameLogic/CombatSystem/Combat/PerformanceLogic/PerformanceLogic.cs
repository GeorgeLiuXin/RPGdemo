using System;
using UnityGameFramework.Runtime;

namespace Galaxy
{
    [Flags]
    public enum PerformanceLogicMode
    {
        LogicMode_BindToAvatar = 1 << 0,                    //绑定在某个角色上,随角色消失
        LogicMode_ReleaseWhenChangeScene = 1 << 1,          //当场景切换移除表现效果
        //LogicMode_HaveToLimitDuration = 1 << 2,             //必须限定类的总期限时长

        LogicMode_Default = LogicMode_BindToAvatar | LogicMode_ReleaseWhenChangeScene,
    }
    public interface IPerformanceLogic
    {
        void SetOwner(int nAvatarID);

        bool InitData(object _data);
        bool Init(params object[] values);
        bool Update(float fTime);
        bool UpdateTimer(float fTime);
        bool UpdateLogic(float fTime);
        void Reset();
        void SetTotalTime(float fTotalTime);
        void Destroy();
        bool IsDestroy();
        void OnTrigger(int index, params object[] values);
    }
    
    /// <summary>
    /// 表现效果整体控制
    /// </summary>
    public abstract class PerformanceLogic : IPerformanceLogic
    {
        protected int m_OwenrID;
        private bool m_bDestroy;
        private float m_CurTime;
        private float m_TotalTime;

        protected PerformanceLogic()
        {
            m_OwenrID = 0;
            m_bDestroy = false;
            m_CurTime = 0;
            m_TotalTime = -1;
        }

        public int GetOwner()
        {
            return m_OwenrID;
        }
        public void SetOwner(int nAvatarID)
        {
            m_OwenrID = nAvatarID;
        }

        public abstract bool InitData(object _data);
        public abstract bool Init(params object[] values);

        public bool Update(float fTime)
        {
            return UpdateLogic(fTime) && UpdateTimer(fTime);
        }

        public abstract void Reset();

        public bool UpdateTimer(float fTime)
        {
            if (IsDestroy())
                return false;

            if (m_TotalTime == -1)
                return true;
            if (m_CurTime < m_TotalTime)
            {
                m_CurTime += fTime;
                return true;
            }
            Destroy();
            return false;
        }

        public abstract bool UpdateLogic(float fTime);

        public virtual void FixedUpdate(float fFixedTime)
        {

        }
        public void SetTotalTime(float fTotalTime)
        {
            m_CurTime = 0;
            m_TotalTime = fTotalTime;
        }

        public void Destroy()
        {
            m_bDestroy = true;
            Reset();
        }
        public bool IsDestroy()
        {
            return m_bDestroy;
        }

        public virtual void OnTrigger(int index, params object[] values) { }


        //表现逻辑模式定义
        protected abstract PerformanceLogicMode GetMode();

        //表现逻辑模式检查
        public bool CheckSkillAttr(PerformanceLogicMode mode) { return (GetMode() & mode) > 0; }
        public bool IsBindToAvatar() { return CheckSkillAttr(PerformanceLogicMode.LogicMode_BindToAvatar); } //绑定在某个角色上,随角色消失
        public bool IsReleaseWhenChangeScene() { return CheckSkillAttr(PerformanceLogicMode.LogicMode_ReleaseWhenChangeScene); } //当场景切换移除表现效果
    }

    public abstract class PerformanceLogic_Xml<T> : PerformanceLogic where T : IPerformanceData
    {
        protected T m_BaseData;
        public T data
        {
            get
            {
                return m_BaseData;
            }
        }

        public PerformanceLogic_Xml(T _data)
        {
            m_BaseData = _data;
        }
        
        public override bool InitData(object _data)
        {
            XmlData.XmlClassData _xmldata = _data as XmlData.XmlClassData;
            if (_xmldata == null)
                return false;
            m_BaseData.InitMyData(_xmldata);
            return true;
        }
    }

    public abstract class PerformanceLogic_Table : PerformanceLogic
    {
        protected DataRowBase m_ConfigData;

        public PerformanceLogic_Table()
        {

        }

        public override bool InitData(object _data)
        {
			DataRowBase _configdata = _data as DataRowBase;
            if (_configdata == null)
                return false;
            m_ConfigData = _configdata;
            return true;
        }
    }
}