using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galaxy
{
    public class PerformanceTestData : PerformanceDataBase<PerformanceTestData>, IPerformanceData
    {
        public ePerformanceLogic GetLogicType()
        {
            return ePerformanceLogic.PerformanceTestLogic;
        }
        [PerformanceLogicItemDes("测试int参数1")]
        public int test1 = 0;
        [PerformanceLogicItemDes("测试string参数2")]
        public string test2 = "";
        [PerformanceLogicItemDes("测试bool参数3")]
        public bool test3 = true;
    }

    //测试
    [PerformanceLogicDes("测试逻辑")]
    public class PerformanceTestLogic : PerformanceLogic_Xml<PerformanceTestData>
    {
        protected override PerformanceLogicMode GetMode()
        {
            return PerformanceLogicMode.LogicMode_Default;
        }

        public PerformanceTestLogic() : base(new PerformanceTestData())
        {
        }

        public override bool Init(params object[] values)
        {
            return true;
        }

        public override void Reset()
        {

        }

        public override bool UpdateLogic(float fTime)
        {
            return true;
        }
    }
}