using DG.Tweening;
using RunTime.Abstracts;
using RunTime.Enums;
using RunTime.Signals;
using UnityEngine;

namespace RunTime.Handlers
{
    public class BusHandler : MonoBehaviour, IEntityTypes
    {
        [SerializeField] private EntityTypes _entityTypes;

        private int _objectCount;

        public int ObjectCount
        {
            get => _objectCount;
            set
            {
                if (value >= 3)
                {
                    MoveBus();
                }
                _objectCount = value;
            }
        }
        public EntityTypes EntityTypes { get => _entityTypes; set => _entityTypes = value; }

        private void MoveBus()
        {
            transform.DOMoveX(20, 1);
            BusSignals.Instance.onSpawnNewBus?.Invoke(this);
        }
        public void IncreaseObjectCount()
        {
            ObjectCount++;
        }
    }
}
