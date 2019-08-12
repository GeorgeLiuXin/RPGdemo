//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2019-08-09 08:04:17.503
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
    /// CD表。
    /// </summary>
    public class DRCDdefine : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取CD组编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取CDAttr。
        /// </summary>
        public int CDAttr
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取CD时间。
        /// </summary>
        public float CDTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取CD次数。
        /// </summary>
        public int CDCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取公共CD。
        /// </summary>
        public int CDCommon
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
            CDAttr = int.Parse(columnTexts[index++]);
            CDTime = float.Parse(columnTexts[index++]);
            CDCount = int.Parse(columnTexts[index++]);
            CDCommon = int.Parse(columnTexts[index++]);

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
                    CDAttr = binaryReader.ReadInt32();
                    CDTime = binaryReader.ReadSingle();
                    CDCount = binaryReader.ReadInt32();
                    CDCommon = binaryReader.ReadInt32();
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
