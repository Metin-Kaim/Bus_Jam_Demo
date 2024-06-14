using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RunTime.Datas.UnityObjects;
using RunTime.Datas.ValueObjects;
using RunTime.Enums;

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
    LevelInfos_SO _editorCellTextures;
    private bool _isSelectedObstaclePart;
    private int _levelCount;
    private string[] _levelNames;
    private readonly string _separator = "-------------------------------------------------------------------------------------------" +
      "-------------------------------------------------------------------------------------------" +
      "-------------------------------------------------------------------------------------------";
    List<EditorTexture> _entityTextures;
    private List<EntityTypes> busColors = new();
    private Vector2 _scrollPosition = Vector2.zero;

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
            _entityTextures = _cellInfos_SO.ObjectTextures;
        }
        GenerateGrid();

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        #region Detection Process
        if (_selectedCell != -1)
        {
            DetectSelectedGrid();
        }
        if (_selectedLevel != _selectedLevelBackup)
        {
            _selectedLevelBackup = _selectedLevel;
            OnChanceLevel();
        }
        #endregion

        Seperator();

        if (GUILayout.Button("Clear Grid"))
        {
            ClearLevelData();
        }

        Seperator();

        #region Entity Selection
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Get Objects"))
        {
            _entityTextures = _cellInfos_SO.ObjectTextures;
            _isSelectedObstaclePart = false;
            InitEditorTextures();
        }
        if (GUILayout.Button("Get Obstacles"))
        {
            _entityTextures = _cellInfos_SO.ObstacleTextures;

            _isSelectedObstaclePart = true;
            InitEditorTextures();
        }
        EditorGUILayout.EndHorizontal();

        _selectedTexture = EditorGUILayout.IntSlider(_selectedTexture, 0, _entityTextures.Count - 1);

        _ = (Texture)EditorGUILayout.ObjectField(_editorTextures[_selectedTexture] != null ? _editorTextures[_selectedTexture].name : "Empty", _editorTextures[_selectedTexture], typeof(Texture), false);
        #endregion

        Seperator();

        #region Bus List
        EditorGUILayout.LabelField("Bus List");

        if (GUILayout.Button("Add Bus Color"))
        {
            busColors.Add(EntityTypes.Red);
            _editorCellTextures.levelBusInfos.Add(new() { busColorType = EntityTypes.Red });
        }

        for (int i = 0; i < busColors.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            busColors[i] = (EntityTypes)EditorGUILayout.EnumPopup("Bus Color " + (i), busColors[i]);
            if (busColors[i] == EntityTypes.None)
            {
                busColors[i] = EntityTypes.Red;
            }
            _editorCellTextures.levelBusInfos[i].busColorType = busColors[i];
            if (GUILayout.Button("Remove"))
            {
                busColors.RemoveAt(i);
                _editorCellTextures.levelBusInfos.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        #endregion

        Seperator();

        #region Level Selector
        EditorGUILayout.LabelField("Choose Level");

        EditorGUILayout.BeginHorizontal();
        _selectedLevel = EditorGUILayout.Popup(_selectedLevel, _levelNames);
        _selectedLevel = EditorGUILayout.IntField(_selectedLevel, new GUIStyle(GUI.skin.textField)
        {
            fixedWidth = 50,
        });

        _selectedLevel = Mathf.Clamp(_selectedLevel, 0, _levelCount - 1);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Selected Level: " + _levelNames[_selectedLevel]);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create New Level Data"))
        {
            CreateNewLevelData();
        }
        if (GUILayout.Button("Refresh Level Infos"))
        {
            GetLevels();
        }
        EditorGUILayout.EndHorizontal();

        #endregion

        EditorGUILayout.EndScrollView();
    }

    private void GetGridSizeFromSO()
    {
        GridInfo gridInfo = Resources.Load<GridInfo_SO>("RunTime/GridInfo").gridInfo;
        _gridColumn = gridInfo.columnSize;
        _gridRow = gridInfo.rowSize;
    }

    private void GetLevels()
    {
        _levelCount = Resources.LoadAll<LevelInfos_SO>("RunTime/Levels").Length;
        
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
        _editorCellTextures = Resources.Load<LevelInfos_SO>($"RunTime/Levels/Level {_selectedLevel}");

        busColors.Clear();
        _editorCellTextures.levelBusInfos ??= new List<LevelBusInfo>();


        if (_editorCellTextures.levelCellInfos == null || _editorCellTextures.levelCellInfos.Count == 0)
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
            for (int i = 0; i < _editorCellTextures.levelBusInfos.Count; i++)
            {
                busColors.Add(_editorCellTextures.levelBusInfos[i].busColorType);
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
        AssetDatabase.SaveAssets();
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

    private void CreateNewLevelData()
    {
        LevelInfos_SO newLevelData = CreateInstance<LevelInfos_SO>();

        string path = $"Assets/Resources/RunTime/Levels/Level {_levelCount}.asset";
        AssetDatabase.CreateAsset(newLevelData, path);
        AssetDatabase.SaveAssets();
        GetLevels();
    }
}
