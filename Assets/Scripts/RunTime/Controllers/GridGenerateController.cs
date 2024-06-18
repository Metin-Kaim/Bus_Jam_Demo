using RunTime.Abstracts;
using RunTime.Datas.UnityObjects;
using RunTime.Datas.ValueObjects;
using RunTime.Handlers;
using RunTime.Signals;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
//.75f
namespace RunTime.Controllers
{
    public class GridGenerateController : MonoBehaviour
    {
        [SerializeField] TileHandler _tilePrefab;
        [SerializeField] Transform _gridContainer;
        [SerializeField] float _tileSpacing;
        [SerializeField] ObjectHandler _objectPrefab;

        int _gridWidth;
        int _gridHeight;
        ObjectDetails_SO _objectDetails_SO;
        LevelInfos_SO _levelInfos;
        TileHandler[,] _gridTiles;

        private void Awake()
        {
            GridInfo gridInfo = Resources.Load<GridInfo_SO>("RunTime/GridInfo").gridInfo;
            _gridWidth = gridInfo.columnSize;
            _gridHeight = gridInfo.columnSize;
            _gridTiles = new TileHandler[_gridWidth, _gridHeight];
            _objectDetails_SO = Resources.Load<ObjectDetails_SO>("RunTime/ObjectDetails");
        }

        private void OnEnable()
        {
            GridSignals.Instance.onGetGridTiles += () => _gridTiles;
        }
        private void OnDisable()
        {
            GridSignals.Instance.onGetGridTiles -= () => _gridTiles;
        }

        void Start()
        {
            _levelInfos = LevelSignals.Instance.onGetCurrentLevelInfos?.Invoke();
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            int spawnerCounter = 0;
            for (int i = 0, j = 0, y = _gridWidth; y > 0; y--)
            {
                for (int x = 0; x < _gridHeight; x++)
                {
                    Vector3 tilePosition = new(x * _tileSpacing, 0, y * _tileSpacing);
                    Vector3 objectPosition;
                    TileHandler tileHandler = null;

                    if (_levelInfos.levelCellInfos[i].isObstacle)
                    {
                        objectPosition = tilePosition;
                        ObstacleDetail obstacleDetail = _objectDetails_SO.obstacleDetails.FirstOrDefault(a => a.texture == _levelInfos.levelCellInfos[i].texture);

                        GameObject gameObject1 = Instantiate(obstacleDetail.gameObject, objectPosition, Quaternion.identity, _gridContainer);
                        gameObject1.transform.rotation = Quaternion.Euler(_levelInfos.levelCellInfos[i].rotation);

                        if (gameObject1.TryGetComponent(out SpawnerHandler spawner))
                        {
                            gameObject1.name = $"Spawner {_gridWidth - y}-{x}";

                            spawner.Row = (byte)j;
                            spawner.Column = (byte)x;
                            spawner.spawnableTypes.spawnerObjects.AddRange(_levelInfos.spawnerList[spawnerCounter].spawnerObjects);
                            spawnerCounter++;
                        }
                        else
                        {
                            gameObject1.name = $"Obstacle {_gridWidth - y}-{x}";
                        }
                    }
                    else
                    {
                        objectPosition = tilePosition + (Vector3.up* 0.03f);
                        tileHandler = Instantiate(_tilePrefab, tilePosition, Quaternion.identity, _gridContainer);

                        tileHandler.name = $"Tile {_gridWidth - y}-{x}";

                        tileHandler.Row = (byte)j;
                        tileHandler.Column = (byte)x;

                        _gridTiles[j, x] = tileHandler;
                        ObjectDetail objectDetail = _objectDetails_SO.objectDetails.FirstOrDefault(a => a.texture == _levelInfos.levelCellInfos[i].texture);

                        ObjectHandler newObject = Instantiate(_objectPrefab, objectPosition, Quaternion.identity, _gridContainer);
                        newObject.EntityType = objectDetail.entityType;
                        newObject.HatObjectColor = (Color)(ColorSignals.Instance.onGetColor?.Invoke(objectDetail.entityType));

                        tileHandler.CurrentObjectHandler = newObject;
                        newObject.CurrentTileHandler = tileHandler;
                        newObject.name = $"Object {_gridWidth - y}-{x}";
                    }
                    i++;
                }
                j++;
            }
            if (_levelInfos.isSlideGridToLeft)
            {
                _gridContainer.position = new(_gridContainer.position.x - .75f, _gridContainer.position.y, _gridContainer.position.z);
            }
        }
    }
}