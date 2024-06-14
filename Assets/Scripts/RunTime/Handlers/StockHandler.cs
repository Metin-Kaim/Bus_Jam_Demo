using UnityEngine;

namespace RunTime.Handlers
{
    public class StockHandler : MonoBehaviour
    {
        private bool _isFull;
        private bool _isOpen;

        public bool IsFull { get => _isFull; set => _isFull = value; }
        public bool IsOpen { get => _isOpen; set => _isOpen = value; }
    }
}
