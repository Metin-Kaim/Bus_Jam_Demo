using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RunTime.Datas.UnityObjects;
using RunTime.Datas.ValueObjects;
using System;
using log4net.Core;

public class GridLevelEditor : EditorWindow
{
    private readonly float _cellSize = 35;
    private int _gridRow;
    private int _gridColumn;
    private bool _isFirstDone;

    private int _selectedCell;
    private int _selectedTexture;
    private int _selectedLevel;
    private int _selectedLevelBackup;
    private Texture[] _cellTextures;
    private Texture[] _editorTextures;
    private GUIStyle _cellStyle;
    EditorTextures_SO _cellInfos_SO;
    LevelCellInfos_SO _editorCellTextures;
    private bool _isSelectedObstaclePart;
    private int _levelCount;
    private string[] _levelNames;
    private readonly string _separator = "-------------------------------------------------------------------------------------------" +
      "-------------------------------------------------------------------------------------------" +
      "-------------------------------------------------------------------------------------------";

    private int GetGridSize => _gridRow * _gridColumn;


    [MenuItem("Tools/Grid Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<GridLevelEditor>("Grid Level Editor");
    }

    void OnGUI()
    {
        if (!_isFirstDone)
        {
            _isFirstDone = true;
            GetGridSizeFromSO();
            _cellTextures = new Texture[GetGridSize];
            InitEditorTextures();
            InitCellStyle();
            InitGameCellInfos();
            GetLevels();
        }
        GenerateGrid();
        if (_selectedCell != -1)
        {
            DetectSelectedGrid();
        }
        if (_selectedLevel != _selectedLevelBackup)
        {
            Debug.Log("Level Changed");
            _selectedLevelBackup = _selectedLevel;
            OnChanceLevel();
        }
        Seperator();


        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Get Objects"))
        {
            _isSelectedObstaclePart = false;
            InitEditorTextures();
        }
        if (GUILayout.Button("Get Obstacles"))
        {
            _isSelectedObstaclePart = true;
            InitEditorTextures();
        }
        EditorGUILayout.EndHorizontal();

        if (_isSelectedObstaclePart)
        {
            _selectedTexture = EditorGUILayout.IntSlider(_selectedTexture, 0, _cellInfos_SO.ObstacleTextures.Count - 1);
        }
        else
        {
            _selectedTexture = EditorGUILayout.IntSlider(_selectedTexture, 0, _cellInfos_SO.ObjectTextures.Count - 1);
        }
        _ = (Texture)EditorGUILayout.ObjectField(_editorTextures[_selectedTexture] != null ? _editorTextures[_selectedTexture].name : "Empty", _editorTextures[_selectedTexture], typeof(Texture), false);

        Seperator();

        #region Level Selector
        EditorGUILayout.LabelField("Choose Level");

        _selectedLevel = EditorGUILayout.Popup(_selectedLevel, _levelNames);
        EditorGUILayout.LabelField("Selected Level: " + _levelNames[_selectedLevel]);

        #endregion
    }

    private void GetGridSizeFromSO()
    {
        GridInfo gridInfo = Resources.Load<GridInfo_SO>("RunTime/GridInfo").gridInfo;
        _gridColumn = gridInfo.columnSize;
        _gridRow = gridInfo.rowSize;
    }

    private void GetLevels()
    {
        _levelCount = Resources.LoadAll<LevelCellInfos_SO>("RunTime/Levels").Length;
        Debug.Log(_levelCount);
        _levelNames = new string[_levelCount];

        for (int i = 0; i < _levelCount; i++)
        {
            _levelNames[i] = $"Level {i}";
        }
    }
    private void OnChanceLevel()
    {
        InitGameCellInfos();
    }

    private void InitGameCellInfos()
    {
        _editorCellTextures = Resources.Load<LevelCellInfos_SO>($"RunTime/Levels/Level {_selectedLevel}");


        if (_editorCellTextures.levelCellInfos.Count == 0)
        {
            _editorCellTextures.levelCellInfos = new List<LevelCellInfo>();

            for (int i = 0; i < GetGridSize; i++)
            {
                _editorCellTextures.levelCellInfos.Add(new());
            }

            ClearLevelData();

        }
        else
        {
            for (int i = 0; i < GetGridSize; i++)
            {
                _cellTextures[i] = _editorCellTextures.levelCellInfos[i].texture;
            }
        }
    }

    private void InitEditorTextures()
    {
        _cellInfos_SO = Resources.Load<EditorTextures_SO>("Editor/EditorTextures");
        List<EditorTexture> entityTextures;

        if (_isSelectedObstaclePart)
        {
            entityTextures = _cellInfos_SO.ObstacleTextures;
        }
        else
        {
            entityTextures = _cellInfos_SO.ObjectTextures;
        }


        _editorTextures = new Texture[entityTextures.Count];

        for (int i = 0; i < entityTextures.Count; i++)
        {
            _editorTextures[i] = entityTextures[i].texture;
        }
    }

    private void DetectSelectedGrid()
    {
        Texture texture = _editorTextures[_selectedTexture];
        _cellTextures[_selectedCell] = texture;
        _editorCellTextures.levelCellInfos[_selectedCell].texture = texture;
        _editorCellTextures.levelCellInfos[_selectedCell].isObstacle = _selectedTexture != 0 && _isSelectedObstaclePart;
    }

    private void GenerateGrid()
    {
        _selectedCell = GUILayout.SelectionGrid(-1, _cellTextures, _gridColumn, _cellStyle);
    }
    private void InitCellStyle()
    {
        _cellStyle = new GUIStyle(GUI.skin.button)
        {
            fixedWidth = _cellSize,
            fixedHeight = _cellSize,
        };
    }
    private void Seperator()
    {
        EditorGUILayout.LabelField(_separator, EditorStyles.toolbar);
    }
    private void ClearLevelData()
    {
        for (int i = 0; i < _cellTextures.Length; i++)
        {
            _cellTextures[i] = null;
            _editorCellTextures.levelCellInfos[i].texture = null;
            _editorCellTextures.levelCellInfos[i].isObstacle = false;
        }


    }
}
