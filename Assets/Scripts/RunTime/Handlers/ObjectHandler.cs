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

        public EntityTypes EntityTypes { get => _entityTypes; set => _entityTypes = value; }


        private void OnMouseDown()
        {
            BusHandler busHandler = BusSignals.Instance.onGetCurrentBus?.Invoke();

            if (busHandler.EntityTypes == _entityTypes)
            {
                transform.DOMove(busHandler.transform.position, 1).OnComplete(() =>
                {
                    busHandler.IncreaseObjectCount();
                    Destroy(gameObject);
                });

            }
        }
    }
}