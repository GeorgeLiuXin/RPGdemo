//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2019-08-07 23:11:26.032
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
    /// 角色属性集。
    /// </summary>
    public class DRAvatarAValue : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取属性集编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取整体属性集编号。
        /// </summary>
        public int AValueID
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取属性集名称。
        /// </summary>
        public string AvatarAValueDefine
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数值类型。
        /// </summary>
        public int AValueType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取int值。
        /// </summary>
        public int AValueInt
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取float值。
        /// </summary>
        public float AValueFloat
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取百分比值。
        /// </summary>
        public double AValuePercent
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
            AValueID = int.Parse(columnTexts[index++]);
            AvatarAValueDefine = columnTexts[index++];
            AValueType = int.Parse(columnTexts[index++]);
            AValueInt = int.Parse(columnTexts[index++]);
            AValueFloat = float.Parse(columnTexts[index++]);
            AValuePercent = double.Parse(columnTexts[index++]);

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
                    AValueID = binaryReader.ReadInt32();
                    AvatarAValueDefine = binaryReader.ReadString();
                    AValueType = binaryReader.ReadInt32();
                    AValueInt = binaryReader.ReadInt32();
                    AValueFloat = binaryReader.ReadSingle();
                    AValuePercent = binaryReader.ReadDouble();
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

        private void GeneratePropertyArray()
        {

        }
    }
}
