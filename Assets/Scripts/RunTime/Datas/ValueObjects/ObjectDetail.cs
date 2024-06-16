using RunTime.Enums;
using UnityEngine;

namespace RunTime.Datas.ValueObjects
{
    [System.Serializable]
    public struct ObjectDetail
    {
        public Texture texture;
        public GameObject gameObject;
        public EntityTypes entityType;
    }
}
