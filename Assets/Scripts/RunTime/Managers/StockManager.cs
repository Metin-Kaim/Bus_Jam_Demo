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
                    print("a");
                    return _stockList[i];
                }
            }
            return null;
        }

        private IEnumerator IChecker()
        {
            yield return null;

            bool areAllStocksFull = true;
            Debug.LogWarning("Kontrol İşlemi Başlatılıyor...");
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
                print(currentBus.EntityType);
                StockHandler matchedStock = _stockList.FirstOrDefault(x => x.CurrentObjectHandler.EntityType == currentBus.EntityType);
                print(matchedStock);
                if (matchedStock == null)
                {
                    Debug.LogWarning("OYUN DURDURULUYOR...");
                    CoreGameSignals.Instance.onLose?.Invoke();
                }
            }
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