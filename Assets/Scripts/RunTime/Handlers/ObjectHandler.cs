using DG.Tweening;
using RunTime.Abstracts;
using RunTime.Enums;
using RunTime.Handlers;
using RunTime.Signals;
using System;
using UnityEngine;

namespace Assets.Scripts.RunTime.Handlers
{
    public class ObjectHandler : MonoBehaviour, IEntityTypes
    {
        [SerializeField] private EntityTypes _entityTypes;

        private StockHandler _currentStockHandler;
        private TileHandler _currentTileHandler;
        private bool _isClickable;
        Tweener _shakeTween;

        public EntityTypes EntityTypes { get => _entityTypes; set => _entityTypes = value; }
        public StockHandler CurrentStockHandler { get => _currentStockHandler; set => _currentStockHandler = value; }
        public TileHandler CurrentTileHandler { get => _currentTileHandler; set => _currentTileHandler = value; }
        public bool IsClickable { get => _isClickable; set => _isClickable = value; }

        private void Start()
        {
            transform.localScale = Vector3.one / 1.2f;
        }

        private void OnMouseDown()
        {
            if (!_isClickable)
            {
                _shakeTween ??= transform.DOShakePosition(.2f, .2f, 5).OnComplete(() => _shakeTween = null);
                return;
            }

            _currentTileHandler.OpenAccessibleObjects();

            _currentTileHandler.CurrentObjectHandler = null;
            _currentTileHandler = null;

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

        internal void OpenThisObject()
        {
            IsClickable = true;
            transform.DOScale(Vector3.one, .5f).SetEase(Ease.InOutElastic);
        }
    }
}