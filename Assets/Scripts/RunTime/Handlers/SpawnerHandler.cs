using DG.Tweening;
using RunTime.Abstracts;
using RunTime.Datas.UnityObjects;
using RunTime.Datas.ValueObjects;
using RunTime.Managers;
using RunTime.Signals;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RunTime.Handlers
{
    public class SpawnerHandler : MonoBehaviour, ICoordinate
    {
        public SpawnerObjectInfo spawnableTypes;

        [SerializeField] TextMeshPro _spawnerObjectCountTxt;
        [SerializeField] ObjectHandler _objectPrefab;

        Vector3 _objectCountRotation;
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
            SetObjectCount();

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
                    _objectCountRotation = new(90, 0, 0);
                    break;
                case 90:
                    _spawnableColumn++;
                    _objectCountRotation = new(90, 0, 90);
                    break;
                case 180:
                    _spawnableRow++;
                    _objectCountRotation = new(90, 0, 180);
                    break;
                case 270:
                    _objectCountRotation = new(90, 0, 270);
                    _spawnableColumn--;
                    break;
            }

            _spawnerObjectCountTxt.transform.localRotation = Quaternion.Euler(_objectCountRotation);
        }

        private void SetObjectCount()
        {
            _spawnerObjectCountTxt.text = spawnableTypes.spawnerObjects.Count.ToString();
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
                SetObjectCount();
            }
        }

        private void SpawnObject()
        {
            GameManager.IsSpawnedObject = true;

            ObjectDetail objectDetail = _objectDetails_SO.objectDetails.FirstOrDefault(x => x.entityType == spawnableTypes.spawnerObjects[0]);

            Vector3 spawnPosition = _tiles[_spawnableRow, _spawnableColumn].gameObject.transform.position + (Vector3.up * 0.03f);
            ObjectHandler objectHandler = Instantiate(_objectPrefab, spawnPosition, Quaternion.identity, transform.parent);

            objectHandler.name = $"Spawned Object {_spawnableRow}-{_spawnableColumn}";

            objectHandler.EntityType = objectDetail.entityType;
            objectHandler.HatObjectColor = (Color)ColorSignals.Instance.onGetColor?.Invoke(objectDetail.entityType);

            objectHandler.transform.localScale = Vector3.zero;

            objectHandler.transform.DOMove(new(spawnPosition.x, objectHandler.transform.position.y, spawnPosition.z), .3f).From(transform.position);

            spawnableTypes.spawnerObjects.RemoveAt(0);

            objectHandler.IsLastObjetOfSpawner = spawnableTypes.spawnerObjects.Count <= 0;

            _grid[_spawnableRow, _spawnableColumn] = 2;

            objectHandler.CurrentTileHandler = _tiles[_spawnableRow, _spawnableColumn];
            objectHandler.CurrentTileHandler.CurrentObjectHandler = objectHandler;
            objectHandler.OpenThisObject();
        }
    }
}