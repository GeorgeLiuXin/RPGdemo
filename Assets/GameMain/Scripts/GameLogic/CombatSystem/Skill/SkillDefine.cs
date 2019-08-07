using System;
using GameFramework;
using UnityEngine;
using System.Collections.Generic;

namespace Galaxy
{
    public enum eSkillType
    {
        SkillType_Template = 1 << 0,        //模版技能
        SkillType_Active = 1 << 1,      //主动技能
        SkillType_Passive = 1 << 2,     //被动技能
        SkillType_Trigger = 1 << 3,     //触发技能
        SkillType_Buff = 1 << 4,        //Buff技能
    };

    public enum eSkillSpellLogic
    {
        //主动技能
        SkillSpell_Branch = 10001, //技能帧
        SkillSpell_Channel = 10002, //吟唱

        //被动技能
        SkillSpell_Eot = 20001, //DOT/HOT
        SkillSpell_AValue = 20002, //属性集

		//触发技能
		SkillSpell_Trigger = 30001, //触发
    }

    public enum eSkillAttr
    {
        SkillAttr_EffectStateCost = 1 << 0, //效果阶段产生消耗
        SkillAttr_AreaUseTarPos = 1 << 1,   //范围效果使用目标坐标
        SkillAttr_AreaIncludeSelf = 1 << 2, //范围效果包含自身
        SkillAttr_AreaAddExclude = 1 << 3,  //范围效果填充排除列表
        SkillAttr_TriggerCommonCD = 1 << 4, //触发共CD
        SkillAttr_TriggerRemoveBuff = 1 << 5,   //触发后移除Buff
        SkillAttr_TriggerRemoveLayer = 1 << 6,  //触发后减少Buff层数
        SkillAttr_TriggerNotify = 1 << 7,  //触发后产生触发事件
        SkillAttr_SkillNotify = 1 << 8, //产生技能施放事件
        SkillAttr_BulletPeriodEffect = 1 << 9,  //子弹产生周期效果
        SkillAttr_BulletHitEffect = 1 << 10,    //子弹产生命中效果
        SkillAttr_BulletHitNoRemove = 1 << 11,  //子弹命中不移除
        SkillAttr_BulletNotify = 1 << 12,   //子弹产生事件
        SkillAttr_BulletBornTarPos = 1 << 13, //子弹出生在目标点
    };

    public enum eSkillTargetSelect
    {
        TargetSelect_CurTarget = 1,     //当前目标
        TargetSelect_NearestTarget,     //最近目标
    };

    public enum eSkillTargetType
    {
        TargetType_Self = 1 << 0,           //对自己使用
        TargetType_Other = 1 << 1,			//对其他角色使用
        TargetType_Pos = 1 << 2,            //对坐标使用
        TargetType_Dir = 1 << 3,            //对朝向使用
    };

    public enum eSkillTarget
    {
        SkillTarget_Self = 1,               //对自己使用
        SkillTarget_Avatar,					//对角色使用
        SkillTarget_Pos,                    //对坐标使用
        SkillTarget_Dir,                    //对朝向使用
    };

    public enum eSkillCost
    {
        SkillCost_Hp = 1,                   //消耗HP
        SkillCost_Mp,                       //消耗MP
        //SkillCost_E1,                       //消耗能量1
        //SkillCost_E2,                       //消耗能量2
    };

    public enum eSkillSpellResult
    {
        SpellResult_Suc = 0,
        SpellResult_Failed,
        SpellResult_Failed_TargetConditon,	//目标条件失败
        SpellResult_Failed_OutOfRange,      //超出范围
    };

    public enum eSkillAreaLogic
    {
        SkillArea_Min = 0,
        SkillArea_Singleton = 1, //单体
        SkillArea_Sphere, //球形范围
        SkillArea_Sector, //扇形范围
        SkillArea_Ring, //环形范围
        SkillArea_Rect, //矩形范围
        SkillArea_Max,
    };

    public enum eSkillEffectLogic
    {
        SkillEffect_Damage = 1, //伤害
        SkillEffect_Heal,           //治疗
		//SkillEffect_CDReduce,   //技能减CD
		//SkillEffect_Relive,     //复活
		//SkillEffect_AddBuff,        //AddBuff
		//SkillEffect_RemoveBuff, //RemoveBuff
		//SkillEffect_Dispel,     //驱散
		//SkillEffect_Repel,          //击退/拉拽
		SkillEffect_Size
	};

    public enum eSkillLauncherType
    {
        SkillLauncher_Direct = 1,   //直接
        SkillLauncher_Bullet = 2,   //单发子弹
		SkillLauncher_Size,
	};

    public enum eProjectileType
    {
        Projectile_Track = 1,           //追踪子弹
        Projectile_Trap = 2,            //陷阱子弹
    };

	public enum eNotifyObject
	{
		NotifyObject_Base = 0,
		NotifyObject_Normal,
		NotifyObject_Effect,
	};

	//todo 改成引用池管理
	public class GTargetInfo
	{
        public int m_nTargetID;
        public Vector3 m_vSrcPos;
        public Vector3 m_vTarPos;
        public Vector3 m_vMoveTarPos;
		public Vector3 m_vAimDir;
        public GTargetInfo()
        {
			m_nTargetID = 0;
			m_vSrcPos = Vector3.zero;
			m_vTarPos = Vector3.zero;
			m_vAimDir = Vector3.zero;
			m_vMoveTarPos = Vector3.zero;
		}
		public void Reset()
		{
			m_nTargetID = 0;
			m_vSrcPos = Vector3.zero;
			m_vTarPos = Vector3.zero;
			m_vAimDir = Vector3.zero;
			m_vMoveTarPos = Vector3.zero;
		}
	}

    public class GSkillExcludeList : HashSet<int>
    {
        public GSkillExcludeList()
        {
            m_bCount = false;
            m_nCount = 0;
        }
        public bool m_bCount;
        public int m_nCount;
    }

    public enum eTriggerNotifyType
    {
        NotifyType_Skill = 0,           //技能
        NotifyType_Buff,                //Buff
        NotifyType_Damage,              //伤害
        NotifyType_Heal,                //治疗
        NotifyType_OnDamage,            //受到伤害
        NotifyType_OnHeal,              //受到治疗
        NotifyType_TakeDamage,          //承受伤害
        NotifyType_MakeDamage,          //造成伤害
        NotifyType_Trigger,             //触发事件
        NotifyType_Dispel,              //驱散事件
        NotifyType_Bullet,              //子弹事件
        NotifyType_Calculation,         //结算事件
        NotifyType_Count,
    };

    public enum eTriggerNotify
    {
        //////////////////////////////////////////////////////////////////////////
        //技能事件
        TriggerNotify_SkillStart = 1 << 0,  //技能开始
        TriggerNotify_SKillEnd = 1 << 1,    //技能结束
        TriggerNotify_SkillBreak = 1 << 2,  //技能打断
                                            
        //////////////////////////////////////////////////////////////////////////
        //Buff事件
        TriggerNotify_BuffAdd = 1 << 0, //Buff添加
        TriggerNotify_BuffEnd = 1 << 1, //Buff结束
        TriggerNotify_BuffDispel = 1 << 2,  //Buff驱散
        TriggerNotify_BuffStacks = 1 << 3,  //Buff叠加
                  
        //////////////////////////////////////////////////////////////////////////
        //效果事件
        TriggerNotify_Damage = 1 << 0,  //伤害
        TriggerNotify_Heal = 1 << 1,    //治疗
        TriggerNotify_DOT = 1 << 2, //Damage of time
        TriggerNotify_HOT = 1 << 3, //Heal of time
        TriggerNotify_Hit = 1 << 4, //命中
        TriggerNotify_Crit = 1 << 5,    //暴击
        TriggerNotify_Kill = 1 << 6,    //击杀
        TriggerNotify_Miss = 1 << 7,    //未命中
        TriggerNotify_Immunity = 1 << 8,  //免疫
        TriggerNotify_Buff = 1 << 9,    //增益状态
        TriggerNotify_Debuff = 1 << 10, //减益效果
        TriggerNotify_DispelBuff = 1 << 11, //驱散增益
        TriggerNotify_DispelDebuff = 1 << 12,   //驱散减益

        //子弹事件
        TriggerNotify_BulletEffect = 1 << 0,    //子弹周期事件
        TriggerNotify_BulletHit = 1 << 1,   //子弹命中事件
        TriggerNotify_BulletDead = 1 << 2,  //子弹死亡事件
    };

    public enum eSkillCalculation
    {
        SkillCalculation_Hit = 1 << 0,  //计算命中
        SkillCalculation_Atk = 1 << 1,  //计算攻击
        SkillCalculation_AC = 1 << 2,   //计算护甲
        //SkillCalculation_Crit = 1 << 3, //计算暴击
        //SkillCalculation_DR = 1 << 4,   //计算伤增/伤减
        //SkillCalculation_DM = 1 << 5,  //计算附加/吸收
        //SkillCalculation_HHR = 1 << 6,  //计算治疗/承受治疗
        //SkillCalculation_Ex = 1 << 7,   //计算特殊效果
    };

    public enum eSkillEffectTransfrom
    {
        SkillEffectTransfrom_Caster_AValue = 1, //施法属性集
        SkillEffectTransfrom_Target_AValue,         //目标属性集
        SkillEffectTransfrom_Caster_Param,          //施法参数集
        SkillEffectTransfrom_Target_Param,          //目标参数集
        SkillEffectTransfrom_Trigger_Value,         //触发效果值
    };

    public enum eSkillEffectClass
    {
        EffectClass_Physical = 0,
        EffectClass_Magical,
        EffectClass_Total,
    };

    public enum eSkillTriggerCheck
    {
        TriggerCheck_Data_UnChecked = 1 << 0,   // 不检查ID
        TriggerCheck_Data_ID = 1 << 1,  // 检查ID ==

        TriggerCheck_Flag_UnChecked = 1 << 10, // 不检查消息类型
        TriggerCheck_Flag_Equal = 1 << 11,  // 检查消息类型==
        TriggerCheck_Flag_And = 1 << 12,    // 检查消息类型&
        TriggerCheck_Flag_Or = 1 << 13, // 检查消息类型|
        TriggerCheck_Value_UnChecked = 1 << 20, // 不检查消息参数
        TriggerCheck_Value_Greater = 1 << 21,   // 检查消息参数 >
        TriggerCheck_Value_GreaterAndEqual = 1 << 22,   // 检查消息参数 >=
        TriggerCheck_Value_Less = 1 << 23,  // 检查消息参数 <
        TriggerCheck_Value_LessAndEqual = 1 << 24,  // 检查消息参数 <=
        TriggerCheck_Value_Equal = 1 << 25, // 检查消息参数 ==
        TriggerCheck_Value_NotEqual = 1 << 26,  // 检查消息参数 !=
        TriggerCheck_Value_And = 1 << 27, // 检查消息参数 &
        TriggerCheck_Value_Or = 1 << 28, // 检查消息参数 |
    };

	public enum eAreaFilter
	{
		AreaFilter_MinHp = 1,
	};

	public enum eStrongControlledDisDir
    {
        DisDir_Push = 1,                //击退
        DisDir_Pull = 2,                //拉拽
        DisDir_CenterPoint = 3,         //拉拽到中点位置
    };

    public enum eSkillHint
    {
        Hint_None,
        Hint_BeyondTheDistance,
        Hint_TargetHasDead,
        Hint_NoTarget,
    }
}