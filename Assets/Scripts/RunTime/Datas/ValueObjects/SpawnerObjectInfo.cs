using RunTime.Enums;
using System.Collections.Generic;

namespace RunTime.Datas.ValueObjects
{
    [System.Serializable]
    public class SpawnerObjectInfo
    {
        public List<EntityTypes> spawnerObjects;

        public SpawnerObjectInfo() { spawnerObjects = new(); }
    }
}