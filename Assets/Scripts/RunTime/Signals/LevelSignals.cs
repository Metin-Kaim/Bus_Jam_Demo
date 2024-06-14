using RunTime.Datas.UnityObjects;
using System;
using UnityEngine;

namespace RunTime.Signals
{
    public class LevelSignals : MonoBehaviour
    {
        public static LevelSignals Instance;

        public Func<LevelInfos_SO> onGetCurrentLevelInfos;

        private void Awake()
        {
            Instance = this;
        }
    }
}