using DG.Tweening;
using RunTime.Abstracts;
using RunTime.Controllers;
using RunTime.Enums;
using RunTime.Managers;
using RunTime.Signals;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Handlers
{
    public class ObjectHandler : MonoBehaviour, IEntityTypes
    {
        [SerializeField] private EntityTypes _entityType;

        private StockHandler _currentStockHandler;
        private TileHandler _currentTileHandler;
        private bool _isClickable;
        Tweener _shakeTween;
        private bool _isLastObjetOfSpawner = false;

        public bool IsLastObjetOfSpawner { get => _isLastObjetOfSpawner; set => _isLastObjetOfSpawner = value; }
        public EntityTypes EntityType { get => _entityType; set => _entityType = value; }
        public StockHandler CurrentStockHandler { get => _currentStockHandler; set => _currentStockHandler = value; }
        public TileHandler CurrentTileHandler { get => _currentTileHandler; set => _currentTileHandler = value; }
        public bool IsClickable { get => _isClickable; set => _isClickable = value; }

        private void Start()
        {
            transform.localScale = Vector3.one / 1.2f;
        }

        private void OnMouseDown()
        {
            if (!(bool)InputSignals.Instance.onGetIsClickable?.Invoke())
            {
                return;
            }
            else if (!_isClickable || (!(bool)BusSignals.Instance.onGetCurrentBus?.Invoke().IsArrivedToCenter && StockSignals.Instance.onGetAvailableStock?.Invoke() == null))
            {
                _shakeTween ??= transform.DOShakePosition(.2f, .2f, 5).OnComplete(() => _shakeTween = null);
                return;
            }


            List<Coordinate> exitPath = GridSignals.Instance.onGetPathToExit?.Invoke(_currentTileHandler.Row, _currentTileHandler.Column);
            TileHandler[,] tileHandlers = GridSignals.Instance.onGetGridTiles?.Invoke();
            TileHandler currentTileBackUp = _currentTileHandler;
            _currentTileHandler.CurrentObjectHandler = null;
            _currentTileHandler = null;

            SpawnerSignals.Instance.onCheckToSpawnObject?.Invoke();

            if (!GameManager.IsSpawnedObject || IsLastObjetOfSpawner)
            {
                currentTileBackUp.OpenAccessibleObjects();
            }
            else
            {
                GameManager.IsSpawnedObject = false;
            }

            Sequence moveSeq = DOTween.Sequence();

            foreach (Coordinate coord in exitPath)
            {
                Vector3 position = tileHandlers[coord.x, coord.y].transform.position;
                moveSeq.Append(transform.DOMove(new Vector3(position.x, transform.position.y, position.z), .1f));
            }
            moveSeq.AppendCallback(() =>
            {
                bool isMovedToBus = MoveToBus();

                if (isMovedToBus) return;

                MoveToStock();
            });
        }
        public bool MoveToBus()
        {
            BusHandler _currentBusHandler = BusSignals.Instance.onGetCurrentBus?.Invoke();

            if (_currentBusHandler == null) return false;

            if (_currentBusHandler.EntityType == _entityType && _currentBusHandler.IsArrivedToCenter)
            {
                transform.DOMove(new(_currentBusHandler.transform.position.x, transform.position.y, _currentBusHandler.transform.position.z), .3f).OnComplete(() =>
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

            stockHandler.IsEmpty = false;
            _currentStockHandler = stockHandler;
            stockHandler.CurrentObjectHandler = this;

            if (stockHandler != null)
            {
                transform.DOMove(new(stockHandler.transform.position.x, transform.position.y, stockHandler.transform.position.z), .3f);
            }
        }

        internal void OpenThisObject()
        {
            IsClickable = true;
            transform.DOScale(Vector3.one, .5f).SetEase(Ease.InOutElastic);
        }
    }
}