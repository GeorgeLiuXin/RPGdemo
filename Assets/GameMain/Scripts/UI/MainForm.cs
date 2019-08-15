using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Galaxy
{
    public class MainForm : UGuiForm
    {
        private Player m_LocalPlayer;
        private Avatar m_Target;

        [SerializeField]
        private Text m_PlayerHpRatio = null;

        [SerializeField]
        private Slider m_PlayerHpBar = null;

        [SerializeField]
        private Text m_TargetName = null;

        [SerializeField]
        private Text m_TargetHpRatio = null;

        [SerializeField]
        private Slider m_TargetHpBar = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(ChangeTargetEvent.EventId, OnChangeTarget);

            m_LocalPlayer = GameEntry.Entity.GetGameEntity(GameEntry.StaicGame.m_LocalPlayerID) as Player;
            if (m_LocalPlayer == null)
            {
                Log.Error("当前没有本地玩家!");
                return;
            }

            SetPlayerValue();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            SetPlayerValue();
            SetTargetValue();
        }

        protected void OnChangeTarget(object sender, GameEventArgs e)
        {
            ChangeTargetEvent ne = (ChangeTargetEvent)e;
            if (ne == null)
                return;

            m_Target = GameEntry.Entity.GetGameEntity(ne.TargetID) as Avatar;
            if (m_Target == null)
                return;
            SetTargetValue();
        }

        protected override void OnClose(object userData)
        {
            GameEntry.Event.Unsubscribe(ChangeTargetEvent.EventId, OnChangeTarget);
            base.OnClose(userData);
        }

        protected void SetPlayerValue()
        {
            if (m_LocalPlayer == null)
                return;
            m_PlayerHpBar.maxValue = m_LocalPlayer.MaxHP;
            m_PlayerHpBar.value = m_LocalPlayer.HP;
            m_PlayerHpRatio.text = Utility.Text.Format("'{0}'%", m_LocalPlayer.HPRatio);
        }

        protected void SetTargetValue()
        {
            if (m_Target == null)
                return;
            m_TargetName.text = m_Target.Name;
            m_TargetHpBar.maxValue = m_Target.MaxHP;
            m_TargetHpBar.value = m_Target.HP;
            m_TargetHpRatio.text = Utility.Text.Format("'{0}'%", m_Target.HPRatio);
        }
    }
}