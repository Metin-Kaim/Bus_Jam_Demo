using RunTime.Enums;
using System;
using UnityEngine;

namespace RunTime.Signals
{
    public class ColorSignals : MonoBehaviour
    {
        public static ColorSignals Instance;

        public Func<EntityTypes, Color> onGetColor;

        private void Awake()
        {
            Instance = this;
        }
    }
}