using RunTime.Handlers;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace RunTime.Signals
{
    public class BusSignals : MonoBehaviour
    {
        public static BusSignals Instance;

        public UnityAction<BusHandler> onSpawnNewBus;
        public Func<BusHandler> onGetCurrentBus;

        private void Awake()
        {
            Instance = this;
        }
    }
}