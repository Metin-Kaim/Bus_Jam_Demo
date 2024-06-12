using RunTime.Abstracts;
using RunTime.Enums;
using UnityEngine;

namespace RunTime.Handlers
{
    public class BusHandler : MonoBehaviour, IEntityTypes
    {
        [SerializeField] private EntityTypes _entityTypes;

        public EntityTypes EntityTypes { get => _entityTypes; set => _entityTypes = value; }



    }
}
