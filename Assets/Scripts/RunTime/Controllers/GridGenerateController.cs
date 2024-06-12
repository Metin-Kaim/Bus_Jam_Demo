using UnityEngine;

public class GridGenerateController : MonoBehaviour
{
    [SerializeField] GameObject _tilePrefab;
    [SerializeField] Transform _gridContainer;
    [SerializeField] int _gridWidth;
    [SerializeField] int _gridHeight;
    [SerializeField] float _tileSpacing;

    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                Vector3 tilePosition = new(x * _tileSpacing, 0, y * _tileSpacing);
                Instantiate(_tilePrefab, tilePosition, Quaternion.identity, _gridContainer);
            }
        }
    }
}
