using RunTime.Datas.ValueObjects;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Datas.UnityObjects
{
    [CreateAssetMenu(fileName ="new Level",menuName ="Bus Jam/Create Level")]
    public class LevelCellInfos_SO : ScriptableObject
    {
        public List<LevelCellInfo> levelCellInfos;
    }
}
