using RunTime.Handlers;
using RunTime.Signals;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                    StartCoroutine(IChecker());
                    return _stockList[i];
                }
            }
            return null;
        }

        private IEnumerator IChecker()
        {
            yield return null;

            bool areAllStocksFull = true;

            for (int j = 0; j < _stockList.Count; j++)
            {
                if (_stockList[j].IsEmpty)
                {
                    areAllStocksFull = false;
                    break;
                }
            }
            if (areAllStocksFull)
            {
                BusHandler currentBus = BusSignals.Instance.onGetCurrentBus?.Invoke();
                StockHandler matchedStock = _stockList.FirstOrDefault(x => x.CurrentObjectHandler.EntityType == currentBus.EntityType);
                if (matchedStock == null)
                {
                    CoreGameSignals.Instance.onLose?.Invoke();
                }
            }
        }

        public void OnCheckStockObjectsToMoveToBus()
        {
            int objCounter = 0;

            foreach (StockHandler handler in _stockList)
            {
                if (handler.CheckObjectToMoveToBus())
                {
                    objCounter++;
                }
                if (objCounter >= 3)
                {
                    break;
                }
            }
        }

        private void OnDisable()
        {
            StockSignals.Instance.onGetAvailableStock -= GetAvailableStock;
            StockSignals.Instance.onCheckStockObjectsToMoveToBus += OnCheckStockObjectsToMoveToBus;
        }
    }
}