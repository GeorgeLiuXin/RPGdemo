using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
	public partial class SkillComponent
	{
		public int GetCurrentSkillID()
		{
			if(m_pSpellLogic == null)
				return -1;
			return m_pSpellLogic.m_pSkillData.Id;
		}

		public void FinishSkill()
		{
			if(m_pSpellLogic == null)
				return;
			m_pSpellLogic.Reset();
			m_pSpellLogic = null;
		}

		//处理技能效果
		public void ProcessSkillEffect(DRSkillData pSkillData, GTargetInfo sTarInfo, SkillAValueData AValue)
		{
			List<int> vExcludeList = new List<int>();
			ProcessSkillEffect(pSkillData, sTarInfo, AValue, vExcludeList);
		}

		public void ProcessSkillEffect(DRSkillData pSkillData, GTargetInfo sTarInfo, SkillAValueData AValue, List<int> vExcludeList)
		{
			if(pSkillData == null)
				return;

			Avatar pCaster = Owner;
			if(!pCaster)
				return;

			int nAreaLogic = pSkillData.MSV_AreaLogic;
			GSkillAreaLogic pAreaLogic = GSkillLogicManager.Instance.GetAreaLogic(nAreaLogic);
			if(pAreaLogic == null)
				return;
			
			if(nAreaLogic != (int)eSkillAreaLogic.SkillArea_Singleton && !pSkillData.IsAreaIncludeSelf())
				vExcludeList.Add(pCaster.Id);

			List<Avatar> vTargetList = pAreaLogic.GetTargetList(pSkillData, pCaster, sTarInfo, vExcludeList);
			if(vTargetList == null || vTargetList.Count == 0)
				return;

			////合并攻击方属性 todo合并技能属性集和模板属性集
			//RoleAValue sSkillAValue;
			//sSkillAValue.Copy(sRoleValue);
			//sSkillAValue.Combine(pSkillData->m_RoleValue);

			//RoleAValue sCasterRoleValue;
			//sCasterRoleValue.Copy(sSkillAValue);
			//sCasterRoleValue.Combine(pCaster->GetRoleAValue());

			//属性合并节点 todo 等技能数据包裹修复后
			//if(pSkillData->IsCombineEffectNotify())
			//{
			//	PushTriggerNotifyAValue(pSkillData->m_nDataID, 0, NotifyType_CombineEffect, pSkillData->GetIntValue(MSV_EffectType), &sCasterRoleValue);
			//}

			int nEffectLogic = pSkillData.MSV_EffectLogic;
			GSkillEffect pEffectLogic = GSkillLogicManager.Instance.GetEffectLogic(nEffectLogic);
			if(pEffectLogic == null)
				return;

			GTargetInfo tempTarInfo;
			foreach(var pTarget in vTargetList)
			{
				if(pTarget == null)
					continue;
				
				if(vExcludeList.Contains(pTarget.Id))
					continue;

				PlayerAValueData sCasterRoleValue = pCaster.GetRoleAValue();
				PlayerAValueData sTargetRoleValue = pTarget.GetRoleAValue();
				if(sCasterRoleValue == null || sCasterRoleValue == null)
					continue;

				GSkillCalculation sCaluation = new GSkillCalculation();
				
				sCaluation.m_pSkillData = pSkillData;
				sCaluation.m_pCaster = pCaster;
				sCaluation.m_pTarget = pTarget;
				sCaluation.m_CasterAValue = sCasterRoleValue.CloneData();
				sCaluation.m_TargetAValue = sTargetRoleValue.CloneData();
				sCaluation.m_SkillAValue = AValue;
				sCaluation.TransfromEffectTarget();

				tempTarInfo = sTarInfo;
				tempTarInfo.m_nTargetID = pTarget.Id;
				tempTarInfo.m_vTarPos = pTarget.GetPos();

				if(pEffectLogic.Process(sCaluation, tempTarInfo, AValue))
				{
					//todo 添加log范围
					//pAreaLogic.Draw(pSkillData, pCaster, pTarget, sTarInfo);
				}
				//todo 技能数据添加完后补充 排除列表修复
				////填充重复列表
				//if(pSkillData->IsAreaAddExclude())
				//{
				//	vExcludeList.insert(pTarget->GetAvatarID());
				//}
				////减少效果次数
				//if(vExcludeList.m_nCount > 0)
				//{
				//	--vExcludeList.m_nCount;
				//}
			}
		}

		internal void CreateSkillProjectile(DRSkillData m_pSkillData, GTargetInfo m_TargetInfo, SkillAValueData m_AValue)
		{
			throw new NotImplementedException();
		}
	}
}