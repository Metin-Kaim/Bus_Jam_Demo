using RunTime.Abstracts;
using RunTime.Enums;
using UnityEngine;

namespace Assets.Scripts.RunTime.Handlers
{
    public class ObjectHandler : MonoBehaviour, IEntityTypes
    {
        [SerializeField] private EntityTypes _entityTypes;

        public EntityTypes EntityTypes { get => _entityTypes; set => _entityTypes = value; }
    }
}