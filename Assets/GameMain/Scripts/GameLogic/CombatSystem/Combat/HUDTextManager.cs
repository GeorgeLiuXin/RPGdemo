using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework;
using GameFramework.Event;

namespace Galaxy
{
    public class HUDTextManager
    {
        public bl_HUDText HUDRoot;

        public void Initialize()
        {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowHUDText);
        }

        public void Release()
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowHUDText);
        }

        protected void OnShowHUDText(object sender, GameEventArgs e)
        {
            SkillEffectEvent ne = (SkillEffectEvent)e;
            if (ne == null)
                return;
            Entity pCaster = GameEntry.Entity.GetGameEntity(ne.CasterID);
            if (pCaster == null)
                return;
            Entity pTarget = GameEntry.Entity.GetGameEntity(ne.TargetID);
            if (pTarget == null)
                return;

            switch (ne.EffectType)
            {
                case (int)eTriggerNotify.TriggerNotify_Damage:
                    HUDRoot.NewText(Utility.Text.Format("- {0}" + ne.EffectValue), pTarget.CachedTransform, Color.red,
                        8, 20f, -1f, 2.2f, bl_Guidance.RightDown);
                    break;
                case (int)eTriggerNotify.TriggerNotify_Heal:
                    HUDRoot.NewText(Utility.Text.Format("+ {0}" + ne.EffectValue), pTarget.CachedTransform, Color.green,
                        8, 20f, 0, 0, bl_Guidance.Up);
                    break;
            }
        }
    }
}