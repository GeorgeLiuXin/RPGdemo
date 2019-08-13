//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2019-08-12 23:59:03.061
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
    /// 场景配置表。
    /// </summary>
    public class DRScene : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取场景编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取资源名称。
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取背景音乐编号。
        /// </summary>
        public int BackgroundMusicId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取起始点X。
        /// </summary>
        public float PosX
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取起始点Y。
        /// </summary>
        public float PosY
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取起始点Z。
        /// </summary>
        public float PosZ
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
            AssetName = columnTexts[index++];
            BackgroundMusicId = int.Parse(columnTexts[index++]);
            PosX = float.Parse(columnTexts[index++]);
            PosY = float.Parse(columnTexts[index++]);
            PosZ = float.Parse(columnTexts[index++]);

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
                    AssetName = binaryReader.ReadString();
                    BackgroundMusicId = binaryReader.ReadInt32();
                    PosX = binaryReader.ReadSingle();
                    PosY = binaryReader.ReadSingle();
                    PosZ = binaryReader.ReadSingle();
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
