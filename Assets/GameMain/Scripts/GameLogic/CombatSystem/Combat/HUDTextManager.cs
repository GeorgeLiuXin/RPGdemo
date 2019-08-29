using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework;
using GameFramework.Event;

namespace Galaxy
{
    public class HUDTextManager
    {
        private bl_HUDText HUDRoot;

        public void Initialize(bl_HUDText _root)
        {
			HUDRoot = _root;
			GameEntry.Event.Subscribe(SkillEffectEvent.EventId, OnShowHUDText);
        }

        public void Release()
        {
            GameEntry.Event.Unsubscribe(SkillEffectEvent.EventId, OnShowHUDText);
			HUDRoot = null;
		}

		protected void OnShowHUDText(object sender, GameEventArgs e)
		{
			SkillEffectEvent ne = (SkillEffectEvent)e;
			if(ne == null)
				return;
			Entity pCaster = GameEntry.Entity.GetGameEntity(ne.CasterID);
			if(pCaster == null)
				return;
			Entity pTarget = GameEntry.Entity.GetGameEntity(ne.TargetID);
			if(pTarget == null)
				return;

			if((ne.EffectType & (int)eTriggerNotify.TriggerNotify_Damage) != 0)
			{
				Vector3 vOffset = pTarget.GetPos() - pCaster.GetPos();
				vOffset.y = 0;

				Camera pCamera = GameEntry.CameraMgr.GetCurEngineCamera();
				if(pCamera == null || pCamera.transform == null)
					return;
				Vector3 vRight = pCamera.transform.right;
				vRight.y = 0;

				float dot = Vector3.Dot(vOffset, vRight);
				bl_Guidance dir = dot <= 0 ? bl_Guidance.LeftDown : bl_Guidance.RightDown;

				HUDRoot.NewText(Utility.Text.Format("- {0:N1}", ne.EffectValue), pTarget.CachedTransform, Color.red,
					8, 20f, -1f, 2.2f, dir);
			}
			else if((ne.EffectType & (int)eTriggerNotify.TriggerNotify_Heal) != 0)
			{
				HUDRoot.NewText(Utility.Text.Format("+ {0:N1}", ne.EffectValue), pTarget.CachedTransform, Color.green,
					8, 20f, 0, 0, bl_Guidance.Up);
			}
		}
	}
}