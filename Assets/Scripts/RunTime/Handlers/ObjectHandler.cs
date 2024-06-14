using DG.Tweening;
using RunTime.Abstracts;
using RunTime.Enums;
using RunTime.Handlers;
using RunTime.Signals;
using UnityEngine;

namespace Assets.Scripts.RunTime.Handlers
{
    public class ObjectHandler : MonoBehaviour, IEntityTypes
    {
        [SerializeField] private EntityTypes _entityTypes;

        StockHandler _currentStockHandler;
        public EntityTypes EntityTypes { get => _entityTypes; set => _entityTypes = value; }
        public StockHandler CurrentStockHandler { get => _currentStockHandler; set => _currentStockHandler = value; }

        private void OnMouseDown()
        {
            bool isMovedToBus = MoveToBus();

            if (isMovedToBus) return;

            MoveToStock();
        }
        public bool MoveToBus()
        {
            BusHandler _currentBusHandler = BusSignals.Instance.onGetCurrentBus?.Invoke();

            if (_currentBusHandler.EntityTypes == _entityTypes && _currentBusHandler.IsArrivedToCenter)
            {
                transform.DOMove(new(_currentBusHandler.transform.position.x, transform.position.y, _currentBusHandler.transform.position.z), 1).OnComplete(() =>
                {
                    _currentBusHandler.IncreaseObjectCount();
                    Destroy(gameObject);
                });
                return true;
            }
            return false;
        }
        private void MoveToStock()
        {
            StockHandler stockHandler = StockSignals.Instance.onGetAvailableStock?.Invoke();

            _currentStockHandler = stockHandler;
            stockHandler.CurrentObjectHandler = this;

            if (stockHandler != null)
            {
                transform.DOMove(new(stockHandler.transform.position.x, transform.position.y, stockHandler.transform.position.z), 1);
            }
        }


    }
}