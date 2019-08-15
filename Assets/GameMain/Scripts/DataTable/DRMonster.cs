//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2019-08-15 22:19:45.483
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
    /// 怪物表。
    /// </summary>
    public class DRMonster : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取怪物编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取模型id。
        /// </summary>
        public int ModelID
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取怪物属性集。
        /// </summary>
        public int MonsterAValue
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取怪物类型。
        /// </summary>
        public int MonsterType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取怪物技能id。
        /// </summary>
        public int AISkill1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取怪物技能id。
        /// </summary>
        public int AISkill2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取怪物技能id。
        /// </summary>
        public int AISkill3
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取怪物技能id。
        /// </summary>
        public int AISkill4
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取怪物技能id。
        /// </summary>
        public int AISkill5
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
            ModelID = int.Parse(columnTexts[index++]);
            MonsterAValue = int.Parse(columnTexts[index++]);
            MonsterType = int.Parse(columnTexts[index++]);
            AISkill1 = int.Parse(columnTexts[index++]);
            AISkill2 = int.Parse(columnTexts[index++]);
            AISkill3 = int.Parse(columnTexts[index++]);
            AISkill4 = int.Parse(columnTexts[index++]);
            AISkill5 = int.Parse(columnTexts[index++]);

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
                    ModelID = binaryReader.ReadInt32();
                    MonsterAValue = binaryReader.ReadInt32();
                    MonsterType = binaryReader.ReadInt32();
                    AISkill1 = binaryReader.ReadInt32();
                    AISkill2 = binaryReader.ReadInt32();
                    AISkill3 = binaryReader.ReadInt32();
                    AISkill4 = binaryReader.ReadInt32();
                    AISkill5 = binaryReader.ReadInt32();
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

        private KeyValuePair<int, int>[] m_AISkill = null;

        public int AISkillCount
        {
            get
            {
                return m_AISkill.Length;
            }
        }

        public int GetAISkill(int id)
        {
            foreach (KeyValuePair<int, int> i in m_AISkill)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetAISkill with invalid id '{0}'.", id.ToString()));
        }

        public int GetAISkillAt(int index)
        {
            if (index < 0 || index >= m_AISkill.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetAISkillAt with invalid index '{0}'.", index.ToString()));
            }

            return m_AISkill[index].Value;
        }

        private void GeneratePropertyArray()
        {
            m_AISkill = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(1, AISkill1),
                new KeyValuePair<int, int>(2, AISkill2),
                new KeyValuePair<int, int>(3, AISkill3),
                new KeyValuePair<int, int>(4, AISkill4),
                new KeyValuePair<int, int>(5, AISkill5),
            };
        }
    }
}
