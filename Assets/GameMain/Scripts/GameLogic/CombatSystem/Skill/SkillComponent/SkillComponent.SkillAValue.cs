using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
	public partial class SkillComponent
	{
		public void TryCalculateAttribute(GSkillSpellLogic pSpellLogic)
		{

		}

		#region 之后添加技能等级、天赋修正

		////设置星级
		//void GNodeSkillComponent::SetSkillSlots(int32 nSkillID, int32 nSlots)
		//{
		//    GSkillSpellLogic* pSpellLogic = GetSkill(nSkillID);
		//    if (pSpellLogic)
		//    {
		//        int nCurSlots = pSpellLogic->GetSkillSlots() | nSlots;
		//        pSpellLogic->SetSkillSlots(nCurSlots);
		//        TryCalculateAttribute(pSpellLogic);
		//        //设置子技能星级
		//        SetSubSkillSlots(nSkillID, nCurSlots);
		//    }
		//}
		////移除星级
		//void GNodeSkillComponent::ClearSkillSlots(int32 nSkillID, int32 nSlots)
		//{
		//    GSkillSpellLogic* pSpellLogic = GetSkill(nSkillID);
		//    if (pSpellLogic)
		//    {
		//        int32 nCurSlots = pSpellLogic->GetSkillSlots();
		//        nCurSlots ^= nCurSlots & nSlots;
		//        pSpellLogic->SetSkillSlots(nCurSlots);
		//        TryCalculateAttribute(pSpellLogic);
		//        //设置子技能星级
		//        SetSubSkillSlots(nSkillID, nCurSlots);
		//    }
		//}
		////刷新子技能星级
		//void GNodeSkillComponent::SetSubSkillSlots(int32 nSkillID, int32 nSlots)
		//{
		//    GSkillList* pSkillList = GSkillDataManager::Instance().GetSubSkillList(nSkillID);
		//    if (!pSkillList)
		//        return;

		//    GSkillList::iterator iter = pSkillList->begin();
		//    for (; iter != pSkillList->end(); ++iter)
		//    {
		//        GSkillSpellLogic* pSubSkill = GetSkill(*iter);
		//        if (pSubSkill)
		//        {
		//            pSubSkill->SetSkillSlots(nSlots);
		//            TryCalculateAttribute(pSubSkill);
		//        }
		//    }
		//}

		//void GNodeSkillComponent::AddupAValues(AValueStruct &value, AValueMask* pMask)
		//{
		//    FuncPerformance(SkillComponent_AddupAttr)

		//        if (!m_pOwnerNode || m_pOwnerNode->CheckAValueAddupMask(AValueMask_Skill))
		//        return;

		//    m_SkillLogicMap.Begin();
		//    while (!m_SkillLogicMap.IsEnd())
		//    {
		//        GSkillSpellLogic* pSpellLogic = m_SkillLogicMap.Get();
		//        m_SkillLogicMap.Next();

		//        if (!pSpellLogic || !pSpellLogic->m_pSkillData)
		//            return;

		//        if (pSpellLogic->m_pSkillData->GetIntValue(MSV_SpellLogic) != SkillSpell_AValue)
		//            return;

		//        AValueStruct av;
		//        AValueStruct* pInfo = SkillLevelAValueManager::Instance().GetAValue(pSpellLogic->GetSkillID(), 1);
		//        if (pInfo)
		//        {
		//            av.Combine(pInfo, NULL);
		//        }

		//        int32 nSlots = pSpellLogic->m_pSkillData->m_nSlots;
		//        int32 nSlotsMask = pSpellLogic->m_pSkillData->m_nSlotsMask;
		//        if ((nSlotsMask & nSlots) > 0)
		//        {
		//            for (int32 i = 0; i < 32; ++i)
		//            {
		//                if ((nSlots & (1 << i)) <= 0)
		//                    continue;

		//                AValueStruct* pInfo = SkillStarAValueManager::Instance().GetAValue(pSpellLogic->GetSkillID(), i);
		//                if (pInfo)
		//                {
		//                    av.Combine(pInfo, NULL);
		//                }
		//            }
		//        }
		//        value.Combine(av, pMask);
		//    }
		//}

		//void GNodeSkillComponent::TryCalculateAttribute(GSkillSpellLogic* pSpellLogic)
		//{
		//    if (!pSpellLogic || !pSpellLogic->m_pSkillData)
		//        return;

		//    if (pSpellLogic->m_pSkillData->GetIntValue(MSV_SpellLogic) != SkillSpell_AValue)
		//        return;

		//    AValueMask valueMask;
		//    {
		//        AValueMask* pMask = SkillLevelAValueManager::Instance().GetAValueMask(pSpellLogic->GetSkillID());
		//        if (pMask)
		//        {
		//            valueMask.Combine(*pMask);
		//        }
		//    }
		//    {
		//        AValueMask* pMask = SkillStarAValueManager::Instance().GetAValueMask(pSpellLogic->GetSkillID());
		//        if (pMask)
		//        {
		//            valueMask.Combine(*pMask);
		//        }
		//    }
		//    ReCalculate(false, &valueMask);
		//}
		#endregion
	}
}