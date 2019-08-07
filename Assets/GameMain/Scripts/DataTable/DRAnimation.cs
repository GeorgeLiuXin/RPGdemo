//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2019-08-07 23:11:26.046
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
    /// 动画表。
    /// </summary>
    public class DRAnimation : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取动画编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取动画资源名。
        /// </summary>
        public string ResAnimName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否打断自身。
        /// </summary>
        public int IsSelfRestart
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否动画位移。
        /// </summary>
        public int Motion
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取动画层。
        /// </summary>
        public int Layer
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取动画组ID。
        /// </summary>
        public int GroupID
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取动画播放逻辑。
        /// </summary>
        public int GroupLogic
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取播放逻辑参数。
        /// </summary>
        public float GroupParam
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
            ResAnimName = columnTexts[index++];
            IsSelfRestart = int.Parse(columnTexts[index++]);
            Motion = int.Parse(columnTexts[index++]);
            Layer = int.Parse(columnTexts[index++]);
            GroupID = int.Parse(columnTexts[index++]);
            GroupLogic = int.Parse(columnTexts[index++]);
            GroupParam = float.Parse(columnTexts[index++]);

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
                    ResAnimName = binaryReader.ReadString();
                    IsSelfRestart = binaryReader.ReadInt32();
                    Motion = binaryReader.ReadInt32();
                    Layer = binaryReader.ReadInt32();
                    GroupID = binaryReader.ReadInt32();
                    GroupLogic = binaryReader.ReadInt32();
                    GroupParam = binaryReader.ReadSingle();
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
