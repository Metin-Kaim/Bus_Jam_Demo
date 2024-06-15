using UnityEngine;

namespace RunTime.Datas.ValueObjects
{
    [System.Serializable]
    public class LevelCellInfo
    {
        public Texture texture;
        public bool isObstacle;
        public Vector3 rotation;
    }
}
