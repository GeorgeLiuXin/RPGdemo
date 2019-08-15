using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Galaxy
{
    public struct MonsterInfo
    {
        //出怪点参数
        public Vector3 vPos;
        public Vector3 vRot;

        public int nMonsterID;
        public MonsterInfo(int monsterid, Vector3 pos, Vector3 rot)
        {
            nMonsterID = monsterid;
            vPos = pos;
            vRot = rot;
        }
    }

    //关卡 用于控制关卡怪物生成及剧情表演等
    public interface LevelBase
    {

    }

    //怪物生成 temp 临时
    public class LevelTest : LevelBase
    {
        private List<MonsterInfo> list;
        private bool bInit;

        public void Initialize()
        {
            list = new List<MonsterInfo>
            {
                new MonsterInfo(101, new Vector3(-1.377827f, 18.90628f, 14.0027f), Vector3.zero),
                new MonsterInfo(102, new Vector3(-4.949947f, 18.80193f, -9.916924f), Vector3.zero),
                new MonsterInfo(103, new Vector3(-32.1039f, 19.2231f, -4.73109f), new Vector3(0f, 49.105f, 0f))
            };

            foreach (var item in list)
            {
                DRMonster data = GameEntry.DataTable.GetDataTable<DRMonster>().GetDataRow(item.nMonsterID);
                if (data == null)
                {
                    Log.Error("当前刷怪器的怪物数据有误'{0}'!", item.nMonsterID);
                    continue;
                }

                GameEntry.Entity.ShowEntity(
                    typeof(Monster),
                    Constant.Entity.MonsterGroupName,
                    Constant.AssetPriority.EnemyAsset,
                    new MonsterData(GameEntry.Entity.GenerateSerialId(),
                        data.ModelID, item));
            }
        }
        public void Shutdown()
        {
        }

        public void Update()
        {
        }

        public void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne.EntityLogicType == typeof(Monster))
            {
                Monster monster = ne.Entity.Logic as Monster;
                if (monster == null)
                    return;
                MonsterData data = ne.UserData as MonsterData;
                if (data == null)
                    return;
                monster.transform.position = data.info.vPos;
                monster.transform.rotation = Quaternion.Euler(data.info.vRot);
            }
        }
    }
}