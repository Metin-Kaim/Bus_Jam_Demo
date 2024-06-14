using RunTime.Handlers;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace RunTime.Signals
{
    public class StockSignals : MonoBehaviour
    {
        public static StockSignals Instance;

        public Func<StockHandler> onGetAvailableStock;
        public UnityAction onCheckStockObjectsToMoveToBus;

        private void Awake()
        {
            Instance = this;
        }
    }
}