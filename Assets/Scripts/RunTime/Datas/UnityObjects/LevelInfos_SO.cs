using RunTime.Datas.ValueObjects;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Datas.UnityObjects
{
    [CreateAssetMenu(fileName ="new Level",menuName ="Bus Jam/Create Level")]
    public class LevelInfos_SO : ScriptableObject
    {
        public List<LevelCellInfo> levelCellInfos;
        public List<LevelBusInfo> levelBusInfos;
        public bool isSlideGridToLeft;
    }
}
