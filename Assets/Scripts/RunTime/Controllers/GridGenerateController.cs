using Assets.Scripts.RunTime.Datas.ValueObjects;
using RunTime.Datas.UnityObjects;
using RunTime.Datas.ValueObjects;
using RunTime.Managers;
using System.Linq;
using UnityEngine;

public class GridGenerateController : MonoBehaviour
{
    [SerializeField] GameObject _tilePrefab;
    [SerializeField] Transform _gridContainer;
    [SerializeField] float _tileSpacing;

    int _gridWidth;
    int _gridHeight;
    ObjectDetails_SO _objectDetails_SO;
    LevelCellInfos_SO editorCellTextures_SO;
    int _currentLevelIndex;

    private void Awake()
    {
        _currentLevelIndex = SaveManager.Instance.SaveCurrentLevelIndex;
        GridInfo gridInfo = Resources.Load<GridInfo_SO>("RunTime/GridInfo").gridInfo;
        _gridWidth = gridInfo.columnSize;
        _gridHeight = gridInfo.columnSize;
        _objectDetails_SO = Resources.Load<ObjectDetails_SO>("RunTime/ObjectDetails");
        editorCellTextures_SO = Resources.Load<LevelCellInfos_SO>($"RunTime/Levels/Level {_currentLevelIndex}");
    }

    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int i = 0, y = _gridWidth; y > 0; y--)
        {
            for (int x = 0; x < _gridHeight; x++)
            {
                Vector3 tilePosition = new(x * _tileSpacing, 0, y * _tileSpacing);
                Vector3 objectPosition;

                ObjectDetail objectDetail;
                if (editorCellTextures_SO.levelCellInfos[i].isObstacle)
                {
                    objectPosition = tilePosition;
                    objectDetail = _objectDetails_SO.obstacleDetails.FirstOrDefault(a => a.texture == editorCellTextures_SO.levelCellInfos[i].texture);
                }
                else
                {
                    objectPosition = tilePosition + (Vector3.up * .5f);
                    Instantiate(_tilePrefab, tilePosition, Quaternion.identity, _gridContainer);
                    objectDetail = _objectDetails_SO.objectDetails.FirstOrDefault(a => a.texture == editorCellTextures_SO.levelCellInfos[i].texture);
                }

                GameObject newObject = objectDetail.gameObject;
                if (newObject != null)
                {
                    Instantiate(newObject, objectPosition, Quaternion.identity, _gridContainer);
                }

                i++;
            }
        }
    }
}
