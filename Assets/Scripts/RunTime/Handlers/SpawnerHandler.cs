using DG.Tweening;
using RunTime.Abstracts;
using RunTime.Controllers;
using RunTime.Datas.UnityObjects;
using RunTime.Datas.ValueObjects;
using RunTime.Signals;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RunTime.Handlers
{
    public class SpawnerHandler : MonoBehaviour, ICoordinate
    {
        public SpawnerObjectInfo spawnableTypes;

        byte _spawnableRow;
        byte _spawnableColumn;
        TileHandler[,] _tiles;
        private ObjectDetails_SO _objectDetails_SO;
        int[,] _grid;

        public byte Row { get; set; }
        public byte Column { get; set; }

        private void OnEnable()
        {
            SpawnerSignals.Instance.onCheckToSpawnObject += CheckNextTileToSpawn;
        }
        private void OnDisable()
        {
            SpawnerSignals.Instance.onCheckToSpawnObject -= CheckNextTileToSpawn;
        }

        private void Start()
        {
            _tiles = GridSignals.Instance.onGetGridTiles?.Invoke();
            _objectDetails_SO = Resources.Load<ObjectDetails_SO>("RunTime/ObjectDetails");
            _grid = GridSignals.Instance.onGetGrid?.Invoke();

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
        }

        public void CheckNextTileToSpawn()
        {
            int spawnableCount = spawnableTypes.spawnerObjects.Count;
            if (spawnableCount == 0)
            {
                return;
            }
            else if (spawnableCount == 1)
            {
                transform.DOScale(transform.localScale * .9f, .5f).SetEase(Ease.InOutBounce);
            }
            if (_tiles[_spawnableRow, _spawnableColumn].CurrentObjectHandler == null)
            {
                SpawnObject();
            }
        }

        private void SpawnObject()
        {
            GameObject newObject = _objectDetails_SO.objectDetails.FirstOrDefault(x => x.entityType == spawnableTypes.spawnerObjects[0]).gameObject;
            Vector3 spawnPosition = _tiles[_spawnableRow, _spawnableColumn].gameObject.transform.position;
            GameObject spawnedObject = Instantiate(newObject, spawnPosition, Quaternion.identity, transform.parent);
            spawnedObject.transform.DOMove(new(spawnPosition.x, spawnedObject.transform.position.y, spawnPosition.z), .3f).From(transform.position);
            spawnedObject.transform.localScale = Vector3.zero;
            spawnableTypes.spawnerObjects.RemoveAt(0);
            ObjectHandler objectHandler = spawnedObject.GetComponent<ObjectHandler>();
            _grid[_spawnableRow, _spawnableColumn] = 2;
            objectHandler.CurrentTileHandler = _tiles[_spawnableRow, _spawnableColumn];
            objectHandler.CurrentTileHandler.CurrentObjectHandler = objectHandler;
            objectHandler.OpenThisObject();
        }
    }
}