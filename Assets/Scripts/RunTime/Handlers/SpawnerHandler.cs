using RunTime.Abstracts;
using UnityEngine;

namespace RunTime.Handlers
{
    public class SpawnerHandler : MonoBehaviour, ICoordinate
    {
        public byte Row { get; set; }
        public byte Column { get; set; }

        byte _spawnableRow;
        byte _spawnableColumn;

        private void Start()
        {
            _spawnableRow = Row;
            _spawnableColumn = Column;

            Vector3 eulerAngles = transform.rotation.eulerAngles;

            switch (eulerAngles.y)
            {
                case 0:
                    _spawnableRow--;
                    break;
                case 90:
                    _spawnableColumn++;
                    break;
                case 180:
                    _spawnableRow++;
                    break;
                case 270:
                    _spawnableColumn--;
                    break;
            }
            print("Spawnable Coordinate: " + _spawnableRow + "," + _spawnableColumn);
            print(transform.rotation.eulerAngles);
        }
    }
}