using GameFramework.Event;

namespace Galaxy
{
	public class SkillEffectEvent : GameEventArgs
	{
		/// <summary>
		/// 技能效果事件编号。
		/// </summary>
		public static readonly int EventId = typeof(SkillEffectEvent).GetHashCode();

		public override int Id
		{
			get
			{
				return EventId;
			}
		}

		public int SkillID;
		public int CasterID;
		public int TargetID;
		public int NotifyType;
		public int EffectType;
		public float EffectValue;
		
		public override void Clear()
		{
			SkillID = default(int);
			CasterID = default(int);
			TargetID = default(int);
			NotifyType = default(int);
			EffectType = default(int);
			EffectValue = default(float);
		}
	}
}
