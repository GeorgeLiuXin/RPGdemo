//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2019-08-15 22:19:45.454
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy
{
    /// <summary>
    /// 技能表。
    /// </summary>
    public class DRSkillData : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取技能ID。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取自身检查。
        /// </summary>
        public int MSV_SrcCheck
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取目标检查。
        /// </summary>
        public int MSV_TarCheck
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取目标类型。
        /// </summary>
        public int MSV_TarType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取技能范围。
        /// </summary>
        public float MSV_Range
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取CD组。
        /// </summary>
        public int MSV_CDGroup
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取CD时间。
        /// </summary>
        public float MSV_CDTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取消耗类型1。
        /// </summary>
        public int MSV_CostType1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取消耗值1。
        /// </summary>
        public float MSV_CostValue1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取消耗类型2。
        /// </summary>
        public int MSV_CostType2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取消耗值2。
        /// </summary>
        public float MSV_CostValue2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取技能类型。
        /// </summary>
        public int MSV_SkillType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取升级用父技能。
        /// </summary>
        public int MSV_BaseSkillID
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取技能属性。
        /// </summary>
        public int MSV_SkillAttr
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取触发类型。
        /// </summary>
        public int MSV_TriggerType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取触发检查。
        /// </summary>
        public int MSV_TriggerCheck
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取触发消息。
        /// </summary>
        public int MSV_TriggerNotify
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取触发DataID。
        /// </summary>
        public int MSV_TriggerDataID
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取触发Value。
        /// </summary>
        public int MSV_TriggerValue
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取触发几率。
        /// </summary>
        public float MSV_TriggerProbability
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取技能逻辑。
        /// </summary>
        public int MSV_SpellLogic
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取逻辑参数1。
        /// </summary>
        public int MSV_SpellParam1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取逻辑参数2。
        /// </summary>
        public int MSV_SpellParam2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取逻辑参数3。
        /// </summary>
        public int MSV_SpellParam3
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取发射逻辑。
        /// </summary>
        public int MSV_LauncherLogic
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取发射参数1。
        /// </summary>
        public int MSV_LauncherParam1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取发射参数2。
        /// </summary>
        public int MSV_LauncherParam2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取发射参数3。
        /// </summary>
        public int MSV_LauncherParam3
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取技能打断时间。
        /// </summary>
        public float MSV_LockTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取技能持续时间。
        /// </summary>
        public float MSV_LastTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取第一次效果时间。
        /// </summary>
        public float MSV_FirstEffectTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取效果循环时间。
        /// </summary>
        public float MSV_EffectTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取最大效果次数。
        /// </summary>
        public int MSV_EffectCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取子弹逻辑。
        /// </summary>
        public int MSV_ProjectileLogic
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取子弹参数1。
        /// </summary>
        public int MSV_ProjectileParam1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取子弹参数2。
        /// </summary>
        public int MSV_ProjectileParam2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取子弹参数3。
        /// </summary>
        public int MSV_ProjectileParam3
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取子弹持续时间。
        /// </summary>
        public float MSV_ProjectileTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取子弹速度。
        /// </summary>
        public float MSV_ProjectileSpeed
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取子弹命中半径。
        /// </summary>
        public float MSV_ProjectileRadius
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取子弹第一次效果时间。
        /// </summary>
        public float MSV_ProjectileFirstEffectTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取子弹效果时间。
        /// </summary>
        public float MSV_ProjectileEffectTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取子弹效果数量人次上限。
        /// </summary>
        public int MSV_ProjectileEffectCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取效果类型。
        /// </summary>
        public int MSV_EffectType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取效果逻辑。
        /// </summary>
        public int MSV_EffectLogic
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取效果参数1。
        /// </summary>
        public int MSV_EffectParam1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取效果参数2。
        /// </summary>
        public int MSV_EffectParam2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取效果参数3。
        /// </summary>
        public int MSV_EffectParam3
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取效果结算。
        /// </summary>
        public int MSV_EffectCalculation
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取效果转换。
        /// </summary>
        public int MSV_EffectTransform
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取范围逻辑。
        /// </summary>
        public int MSV_AreaLogic
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取范围参数1。
        /// </summary>
        public float MSV_AreaParam1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取范围参数2。
        /// </summary>
        public float MSV_AreaParam2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取范围参数3。
        /// </summary>
        public float MSV_AreaParam3
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取范围目标检查（先）。
        /// </summary>
        public int MSV_AreaTarCheck
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取范围目标筛选（后）。
        /// </summary>
        public int MSV_AreaFilter
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取范围目标数量。
        /// </summary>
        public int MSV_AreaTarCnt
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取技能动画。
        /// </summary>
        public int MSV_AnimID
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取前置瞄准模式。
        /// </summary>
        public int MSV_PreSkillMode
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取辅助瞄准模式。
        /// </summary>
        public int MSV_AimHelperMode
        {
            get;
            private set;
        }

        public override bool ParseDataRow(GameFrameworkSegment<string> dataRowSegment)
        {
            // Metroidvania3D 示例代码，正式项目使用时请调整此处的生成代码，以处理 GCAlloc 问题！
            string[] columnTexts = dataRowSegment.Source.Substring(dataRowSegment.Offset, dataRowSegment.Length).Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnTexts.Length; i++)
            {
                columnTexts[i] = columnTexts[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnTexts[index++]);
            index++;
            MSV_SrcCheck = int.Parse(columnTexts[index++]);
            MSV_TarCheck = int.Parse(columnTexts[index++]);
            MSV_TarType = int.Parse(columnTexts[index++]);
            MSV_Range = float.Parse(columnTexts[index++]);
            MSV_CDGroup = int.Parse(columnTexts[index++]);
            MSV_CDTime = float.Parse(columnTexts[index++]);
            MSV_CostType1 = int.Parse(columnTexts[index++]);
            MSV_CostValue1 = float.Parse(columnTexts[index++]);
            MSV_CostType2 = int.Parse(columnTexts[index++]);
            MSV_CostValue2 = float.Parse(columnTexts[index++]);
            MSV_SkillType = int.Parse(columnTexts[index++]);
            MSV_BaseSkillID = int.Parse(columnTexts[index++]);
            MSV_SkillAttr = int.Parse(columnTexts[index++]);
            MSV_TriggerType = int.Parse(columnTexts[index++]);
            MSV_TriggerCheck = int.Parse(columnTexts[index++]);
            MSV_TriggerNotify = int.Parse(columnTexts[index++]);
            MSV_TriggerDataID = int.Parse(columnTexts[index++]);
            MSV_TriggerValue = int.Parse(columnTexts[index++]);
            MSV_TriggerProbability = float.Parse(columnTexts[index++]);
            MSV_SpellLogic = int.Parse(columnTexts[index++]);
            MSV_SpellParam1 = int.Parse(columnTexts[index++]);
            MSV_SpellParam2 = int.Parse(columnTexts[index++]);
            MSV_SpellParam3 = int.Parse(columnTexts[index++]);
            MSV_LauncherLogic = int.Parse(columnTexts[index++]);
            MSV_LauncherParam1 = int.Parse(columnTexts[index++]);
            MSV_LauncherParam2 = int.Parse(columnTexts[index++]);
            MSV_LauncherParam3 = int.Parse(columnTexts[index++]);
            MSV_LockTime = float.Parse(columnTexts[index++]);
            MSV_LastTime = float.Parse(columnTexts[index++]);
            MSV_FirstEffectTime = float.Parse(columnTexts[index++]);
            MSV_EffectTime = float.Parse(columnTexts[index++]);
            MSV_EffectCount = int.Parse(columnTexts[index++]);
            MSV_ProjectileLogic = int.Parse(columnTexts[index++]);
            MSV_ProjectileParam1 = int.Parse(columnTexts[index++]);
            MSV_ProjectileParam2 = int.Parse(columnTexts[index++]);
            MSV_ProjectileParam3 = int.Parse(columnTexts[index++]);
            MSV_ProjectileTime = float.Parse(columnTexts[index++]);
            MSV_ProjectileSpeed = float.Parse(columnTexts[index++]);
            MSV_ProjectileRadius = float.Parse(columnTexts[index++]);
            MSV_ProjectileFirstEffectTime = float.Parse(columnTexts[index++]);
            MSV_ProjectileEffectTime = float.Parse(columnTexts[index++]);
            MSV_ProjectileEffectCount = int.Parse(columnTexts[index++]);
            MSV_EffectType = int.Parse(columnTexts[index++]);
            MSV_EffectLogic = int.Parse(columnTexts[index++]);
            MSV_EffectParam1 = int.Parse(columnTexts[index++]);
            MSV_EffectParam2 = int.Parse(columnTexts[index++]);
            MSV_EffectParam3 = int.Parse(columnTexts[index++]);
            MSV_EffectCalculation = int.Parse(columnTexts[index++]);
            MSV_EffectTransform = int.Parse(columnTexts[index++]);
            MSV_AreaLogic = int.Parse(columnTexts[index++]);
            MSV_AreaParam1 = float.Parse(columnTexts[index++]);
            MSV_AreaParam2 = float.Parse(columnTexts[index++]);
            MSV_AreaParam3 = float.Parse(columnTexts[index++]);
            MSV_AreaTarCheck = int.Parse(columnTexts[index++]);
            MSV_AreaFilter = int.Parse(columnTexts[index++]);
            MSV_AreaTarCnt = int.Parse(columnTexts[index++]);
            MSV_AnimID = int.Parse(columnTexts[index++]);
            MSV_PreSkillMode = int.Parse(columnTexts[index++]);
            MSV_AimHelperMode = int.Parse(columnTexts[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(GameFrameworkSegment<byte[]> dataRowSegment)
        {
            // Metroidvania3D 示例代码，正式项目使用时请调整此处的生成代码，以处理 GCAlloc 问题！
            using (MemoryStream memoryStream = new MemoryStream(dataRowSegment.Source, dataRowSegment.Offset, dataRowSegment.Length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.ReadInt32();
                    MSV_SrcCheck = binaryReader.ReadInt32();
                    MSV_TarCheck = binaryReader.ReadInt32();
                    MSV_TarType = binaryReader.ReadInt32();
                    MSV_Range = binaryReader.ReadSingle();
                    MSV_CDGroup = binaryReader.ReadInt32();
                    MSV_CDTime = binaryReader.ReadSingle();
                    MSV_CostType1 = binaryReader.ReadInt32();
                    MSV_CostValue1 = binaryReader.ReadSingle();
                    MSV_CostType2 = binaryReader.ReadInt32();
                    MSV_CostValue2 = binaryReader.ReadSingle();
                    MSV_SkillType = binaryReader.ReadInt32();
                    MSV_BaseSkillID = binaryReader.ReadInt32();
                    MSV_SkillAttr = binaryReader.ReadInt32();
                    MSV_TriggerType = binaryReader.ReadInt32();
                    MSV_TriggerCheck = binaryReader.ReadInt32();
                    MSV_TriggerNotify = binaryReader.ReadInt32();
                    MSV_TriggerDataID = binaryReader.ReadInt32();
                    MSV_TriggerValue = binaryReader.ReadInt32();
                    MSV_TriggerProbability = binaryReader.ReadSingle();
                    MSV_SpellLogic = binaryReader.ReadInt32();
                    MSV_SpellParam1 = binaryReader.ReadInt32();
                    MSV_SpellParam2 = binaryReader.ReadInt32();
                    MSV_SpellParam3 = binaryReader.ReadInt32();
                    MSV_LauncherLogic = binaryReader.ReadInt32();
                    MSV_LauncherParam1 = binaryReader.ReadInt32();
                    MSV_LauncherParam2 = binaryReader.ReadInt32();
                    MSV_LauncherParam3 = binaryReader.ReadInt32();
                    MSV_LockTime = binaryReader.ReadSingle();
                    MSV_LastTime = binaryReader.ReadSingle();
                    MSV_FirstEffectTime = binaryReader.ReadSingle();
                    MSV_EffectTime = binaryReader.ReadSingle();
                    MSV_EffectCount = binaryReader.ReadInt32();
                    MSV_ProjectileLogic = binaryReader.ReadInt32();
                    MSV_ProjectileParam1 = binaryReader.ReadInt32();
                    MSV_ProjectileParam2 = binaryReader.ReadInt32();
                    MSV_ProjectileParam3 = binaryReader.ReadInt32();
                    MSV_ProjectileTime = binaryReader.ReadSingle();
                    MSV_ProjectileSpeed = binaryReader.ReadSingle();
                    MSV_ProjectileRadius = binaryReader.ReadSingle();
                    MSV_ProjectileFirstEffectTime = binaryReader.ReadSingle();
                    MSV_ProjectileEffectTime = binaryReader.ReadSingle();
                    MSV_ProjectileEffectCount = binaryReader.ReadInt32();
                    MSV_EffectType = binaryReader.ReadInt32();
                    MSV_EffectLogic = binaryReader.ReadInt32();
                    MSV_EffectParam1 = binaryReader.ReadInt32();
                    MSV_EffectParam2 = binaryReader.ReadInt32();
                    MSV_EffectParam3 = binaryReader.ReadInt32();
                    MSV_EffectCalculation = binaryReader.ReadInt32();
                    MSV_EffectTransform = binaryReader.ReadInt32();
                    MSV_AreaLogic = binaryReader.ReadInt32();
                    MSV_AreaParam1 = binaryReader.ReadSingle();
                    MSV_AreaParam2 = binaryReader.ReadSingle();
                    MSV_AreaParam3 = binaryReader.ReadSingle();
                    MSV_AreaTarCheck = binaryReader.ReadInt32();
                    MSV_AreaFilter = binaryReader.ReadInt32();
                    MSV_AreaTarCnt = binaryReader.ReadInt32();
                    MSV_AnimID = binaryReader.ReadInt32();
                    MSV_PreSkillMode = binaryReader.ReadInt32();
                    MSV_AimHelperMode = binaryReader.ReadInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(GameFrameworkSegment<Stream> dataRowSegment)
        {
            Log.Warning("Not implemented ParseDataRow(GameFrameworkSegment<Stream>)");
            return false;
        }

        private KeyValuePair<int, int>[] m_MSV_CostType = null;

        public int MSV_CostTypeCount
        {
            get
            {
                return m_MSV_CostType.Length;
            }
        }

        public int GetMSV_CostType(int id)
        {
            foreach (KeyValuePair<int, int> i in m_MSV_CostType)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetMSV_CostType with invalid id '{0}'.", id.ToString()));
        }

        public int GetMSV_CostTypeAt(int index)
        {
            if (index < 0 || index >= m_MSV_CostType.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetMSV_CostTypeAt with invalid index '{0}'.", index.ToString()));
            }

            return m_MSV_CostType[index].Value;
        }

        private KeyValuePair<int, float>[] m_MSV_CostValue = null;

        public int MSV_CostValueCount
        {
            get
            {
                return m_MSV_CostValue.Length;
            }
        }

        public float GetMSV_CostValue(int id)
        {
            foreach (KeyValuePair<int, float> i in m_MSV_CostValue)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetMSV_CostValue with invalid id '{0}'.", id.ToString()));
        }

        public float GetMSV_CostValueAt(int index)
        {
            if (index < 0 || index >= m_MSV_CostValue.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetMSV_CostValueAt with invalid index '{0}'.", index.ToString()));
            }

            return m_MSV_CostValue[index].Value;
        }

        private KeyValuePair<int, int>[] m_MSV_SpellParam = null;

        public int MSV_SpellParamCount
        {
            get
            {
                return m_MSV_SpellParam.Length;
            }
        }

        public int GetMSV_SpellParam(int id)
        {
            foreach (KeyValuePair<int, int> i in m_MSV_SpellParam)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetMSV_SpellParam with invalid id '{0}'.", id.ToString()));
        }

        public int GetMSV_SpellParamAt(int index)
        {
            if (index < 0 || index >= m_MSV_SpellParam.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetMSV_SpellParamAt with invalid index '{0}'.", index.ToString()));
            }

            return m_MSV_SpellParam[index].Value;
        }

        private KeyValuePair<int, int>[] m_MSV_LauncherParam = null;

        public int MSV_LauncherParamCount
        {
            get
            {
                return m_MSV_LauncherParam.Length;
            }
        }

        public int GetMSV_LauncherParam(int id)
        {
            foreach (KeyValuePair<int, int> i in m_MSV_LauncherParam)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetMSV_LauncherParam with invalid id '{0}'.", id.ToString()));
        }

        public int GetMSV_LauncherParamAt(int index)
        {
            if (index < 0 || index >= m_MSV_LauncherParam.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetMSV_LauncherParamAt with invalid index '{0}'.", index.ToString()));
            }

            return m_MSV_LauncherParam[index].Value;
        }

        private KeyValuePair<int, int>[] m_MSV_ProjectileParam = null;

        public int MSV_ProjectileParamCount
        {
            get
            {
                return m_MSV_ProjectileParam.Length;
            }
        }

        public int GetMSV_ProjectileParam(int id)
        {
            foreach (KeyValuePair<int, int> i in m_MSV_ProjectileParam)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetMSV_ProjectileParam with invalid id '{0}'.", id.ToString()));
        }

        public int GetMSV_ProjectileParamAt(int index)
        {
            if (index < 0 || index >= m_MSV_ProjectileParam.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetMSV_ProjectileParamAt with invalid index '{0}'.", index.ToString()));
            }

            return m_MSV_ProjectileParam[index].Value;
        }

        private KeyValuePair<int, int>[] m_MSV_EffectParam = null;

        public int MSV_EffectParamCount
        {
            get
            {
                return m_MSV_EffectParam.Length;
            }
        }

        public int GetMSV_EffectParam(int id)
        {
            foreach (KeyValuePair<int, int> i in m_MSV_EffectParam)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetMSV_EffectParam with invalid id '{0}'.", id.ToString()));
        }

        public int GetMSV_EffectParamAt(int index)
        {
            if (index < 0 || index >= m_MSV_EffectParam.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetMSV_EffectParamAt with invalid index '{0}'.", index.ToString()));
            }

            return m_MSV_EffectParam[index].Value;
        }

        private KeyValuePair<int, float>[] m_MSV_AreaParam = null;

        public int MSV_AreaParamCount
        {
            get
            {
                return m_MSV_AreaParam.Length;
            }
        }

        public float GetMSV_AreaParam(int id)
        {
            foreach (KeyValuePair<int, float> i in m_MSV_AreaParam)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetMSV_AreaParam with invalid id '{0}'.", id.ToString()));
        }

        public float GetMSV_AreaParamAt(int index)
        {
            if (index < 0 || index >= m_MSV_AreaParam.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetMSV_AreaParamAt with invalid index '{0}'.", index.ToString()));
            }

            return m_MSV_AreaParam[index].Value;
        }

        private void GeneratePropertyArray()
        {
            m_MSV_CostType = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(1, MSV_CostType1),
                new KeyValuePair<int, int>(2, MSV_CostType2),
            };

            m_MSV_CostValue = new KeyValuePair<int, float>[]
            {
                new KeyValuePair<int, float>(1, MSV_CostValue1),
                new KeyValuePair<int, float>(2, MSV_CostValue2),
            };

            m_MSV_SpellParam = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(1, MSV_SpellParam1),
                new KeyValuePair<int, int>(2, MSV_SpellParam2),
                new KeyValuePair<int, int>(3, MSV_SpellParam3),
            };

            m_MSV_LauncherParam = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(1, MSV_LauncherParam1),
                new KeyValuePair<int, int>(2, MSV_LauncherParam2),
                new KeyValuePair<int, int>(3, MSV_LauncherParam3),
            };

            m_MSV_ProjectileParam = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(1, MSV_ProjectileParam1),
                new KeyValuePair<int, int>(2, MSV_ProjectileParam2),
                new KeyValuePair<int, int>(3, MSV_ProjectileParam3),
            };

            m_MSV_EffectParam = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(1, MSV_EffectParam1),
                new KeyValuePair<int, int>(2, MSV_EffectParam2),
                new KeyValuePair<int, int>(3, MSV_EffectParam3),
            };

            m_MSV_AreaParam = new KeyValuePair<int, float>[]
            {
                new KeyValuePair<int, float>(1, MSV_AreaParam1),
                new KeyValuePair<int, float>(2, MSV_AreaParam2),
                new KeyValuePair<int, float>(3, MSV_AreaParam3),
            };
        }
    }
}
