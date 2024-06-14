using RunTime.Handlers;
using RunTime.Signals;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Managers
{
    public class StockManager : MonoBehaviour
    {
        [SerializeField] List<StockHandler> _stockList;


        private void OnEnable()
        {
            StockSignals.Instance.onGetAvailableStock += GetAvailableStock;
            StockSignals.Instance.onCheckStockObjectsToMoveToBus += OnCheckStockObjectsToMoveToBus;
        }

        private StockHandler GetAvailableStock()
        {
            for (int i = 0; i < _stockList.Count; i++)
            {
                if (_stockList[i].IsEmpty && _stockList[i].IsOpen)
                {
                    _stockList[i].IsEmpty = false;
                    return _stockList[i];
                }
            }
            return null;
        }
        public void OnCheckStockObjectsToMoveToBus()
        {
            foreach (StockHandler handler in _stockList)
            {
                handler.CheckObjectToMoveToBus();
            }
        }

        private void OnDisable()
        {
            StockSignals.Instance.onGetAvailableStock -= GetAvailableStock;
            StockSignals.Instance.onCheckStockObjectsToMoveToBus += OnCheckStockObjectsToMoveToBus;
        }
    }
}