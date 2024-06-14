using Assets.Scripts.RunTime.Handlers;
using UnityEngine;

namespace RunTime.Handlers
{
    public class StockHandler : MonoBehaviour
    {
        [SerializeField] private bool _isOpen;
        private bool _isEmpty = true;
        private ObjectHandler _currentObjectHandler;

        public ObjectHandler CurrentObjectHandler { get => _currentObjectHandler; set => _currentObjectHandler = value; }
        public bool IsEmpty { get => _isEmpty; set => _isEmpty = value; }
        public bool IsOpen { get => _isOpen; set => _isOpen = value; }

        public void CheckObjectToMoveToBus()
        {
            if (_isEmpty || !_isOpen) return;

            bool isMovedToBus = _currentObjectHandler.MoveToBus();

            if (isMovedToBus)
            {
                _currentObjectHandler.CurrentStockHandler = null;
                _currentObjectHandler = null;
                _isEmpty = true;
            }
        }
    }
}
