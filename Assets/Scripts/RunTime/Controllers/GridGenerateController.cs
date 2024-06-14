using Assets.Scripts.RunTime.Datas.ValueObjects;
using Assets.Scripts.RunTime.Handlers;
using RunTime.Datas.UnityObjects;
using RunTime.Datas.ValueObjects;
using RunTime.Handlers;
using RunTime.Signals;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RunTime.Controllers
{
    public class GridGenerateController : MonoBehaviour
    {
        [SerializeField] TileHandler _tilePrefab;
        [SerializeField] Transform _gridContainer;
        [SerializeField] float _tileSpacing;

        int _gridWidth;
        int _gridHeight;
        ObjectDetails_SO _objectDetails_SO;
        LevelInfos_SO levelInfos;
        TileHandler[,] _gridTiles = new TileHandler[9, 9];

        private void Awake()
        {
            GridInfo gridInfo = Resources.Load<GridInfo_SO>("RunTime/GridInfo").gridInfo;
            _gridWidth = gridInfo.columnSize;
            _gridHeight = gridInfo.columnSize;
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
            levelInfos = LevelSignals.Instance.onGetCurrentLevelInfos?.Invoke();
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            for (int i = 0, j = 0, y = _gridWidth; y > 0; y--)
            {
                for (int x = 0; x < _gridHeight; x++)
                {
                    Vector3 tilePosition = new(x * _tileSpacing, 0, y * _tileSpacing);
                    Vector3 objectPosition;
                    TileHandler tileHandler = null;
                    ObjectDetail objectDetail;
                    if (levelInfos.levelCellInfos[i].isObstacle)
                    {
                        objectPosition = tilePosition;
                        objectDetail = _objectDetails_SO.obstacleDetails.FirstOrDefault(a => a.texture == levelInfos.levelCellInfos[i].texture);
                    }
                    else
                    {
                        objectPosition = tilePosition + (Vector3.up * .5f);
                        tileHandler = Instantiate(_tilePrefab, tilePosition, Quaternion.identity, _gridContainer);
                        tileHandler.Row = (byte)j;
                        tileHandler.Col = (byte)x;
                        _gridTiles[j, x] = tileHandler;
                        objectDetail = _objectDetails_SO.objectDetails.FirstOrDefault(a => a.texture == levelInfos.levelCellInfos[i].texture);
                    }

                    GameObject newObject = objectDetail.gameObject;

                    if (newObject != null)
                    {
                        GameObject gameObject1 = Instantiate(newObject, objectPosition, Quaternion.identity, _gridContainer);
                        if (tileHandler != null)
                        {
                            ObjectHandler objectHandler = gameObject1.GetComponent<ObjectHandler>();
                            tileHandler.CurrentObjectHandler = objectHandler;
                            objectHandler.CurrentTileHandler = tileHandler;
                        }
                    }

                    i++;
                }
                j++;
            }
        }
    }
}