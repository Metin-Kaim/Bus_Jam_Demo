using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEditor;
using UnityEngine;
using System;
using RunTime.Datas.UnityObjects;
using System.Drawing.Printing;
using RunTime.Datas.ValueObjects;

public class GridLevelEditor : EditorWindow
{
    private readonly int _gridRow = 9;
    private readonly int _gridColumn = 9;
    private readonly float _cellSize = 35;
    private bool _isFirstDone;

    private int _selectedCell;
    private int _selectedTexture;
    private Texture[] _cellTextures;
    private Texture[] _editorTextures;
    private GUIStyle _cellStyle;
    EditorTextures_SO _cellInfos_SO;
    EditorCellTextures_SO _editorCellTextures;

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
            _cellTextures = new Texture[GetGridSize];
            InitEditorTextures();
            InitCellStyle();
            InitGameCellInfos();
        }
        GenerateGrid();
        DetectSelectedGrid();

        _selectedTexture = EditorGUILayout.IntSlider(_selectedTexture, 0, _cellInfos_SO.cellInfos.Count - 1);
        _ = (Texture)EditorGUILayout.ObjectField(_editorTextures[_selectedTexture] != null ? _editorTextures[_selectedTexture].name : "Empty", _editorTextures[_selectedTexture], typeof(Texture), false);

    }

    private void InitGameCellInfos()
    {
        _editorCellTextures = Resources.Load<EditorCellTextures_SO>("Editor/EditorCellTextures");

        if (_editorCellTextures.editorCellTextures.Count == 0)
        {
            _editorCellTextures.editorCellTextures = new List<EditorCellTexture>();

            for (int i = 0; i < GetGridSize; i++)
            {
                _editorCellTextures.editorCellTextures.Add(new());
            }
        }
        else
        {
            for (int i = 0; i < GetGridSize; i++)
            {
                _cellTextures[i] = _editorCellTextures.editorCellTextures[i].texture;
            }
        }
    }

    private void InitEditorTextures()
    {
        _cellInfos_SO = Resources.Load<EditorTextures_SO>("Editor/EditorTextures");

        _editorTextures = new Texture[_cellInfos_SO.cellInfos.Count];

        for (int i = 0; i < _cellInfos_SO.cellInfos.Count; i++)
        {
            _editorTextures[i] = _cellInfos_SO.cellInfos[i].texture;
        }
    }

    private void DetectSelectedGrid()
    {
        if (_selectedCell != -1)
        {
            Texture texture = _editorTextures[_selectedTexture];
            _cellTextures[_selectedCell] = texture;
            _editorCellTextures.editorCellTextures[_selectedCell].texture = texture;
        }
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
}
