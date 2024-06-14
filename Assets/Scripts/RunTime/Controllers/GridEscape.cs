using Assets.Scripts.RunTime.Handlers;
using RunTime.Datas.UnityObjects;
using RunTime.Signals;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Controllers
{
    public struct Coordinate
    {
        public int x;
        public int y;

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class GridEscape : MonoBehaviour
    {
        // Koordinat yapısı

        // Grid tanımları
        public int[,] grid = new int[9, 9];
        public List<Coordinate> escapePaths;
        LevelInfos_SO levelInfos_SO;

        private void OnEnable()
        {
            GridSignals.Instance.onGetActiveObjectCoordinates += () => escapePaths;
        }
        private void OnDisable()
        {
            GridSignals.Instance.onGetActiveObjectCoordinates -= () => escapePaths;
        }

        private void Start()
        {
            levelInfos_SO = LevelSignals.Instance.onGetCurrentLevelInfos();

            for (int x = 0, i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    grid[i, j] = levelInfos_SO.levelCellInfos[x].texture == null ? 0 : levelInfos_SO.levelCellInfos[x].isObstacle ? 1 : 2;
                    x++;
                }
            }
            string cells = "";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cells += grid[i, j];
                }
                cells += "\n";
            }
            print(cells);

            // Çıkış yollarını bul
            escapePaths = GetObjectsWithEscapePath(grid);

            // Çıkış yolu olan objeleri yazdır
            foreach (var obj in escapePaths)
            {
                Debug.Log($"Obj x: {obj.x}, y: {obj.y} çıkış yolu var.");
            }
        }

        // Grid için BFS algoritması
        public bool IsPathToFirstRow(int[,] grid, int startX, int startY)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            bool[,] visited = new bool[rows, cols];
            Queue<Coordinate> queue = new Queue<Coordinate>();

            // Başlangıç koordinatını kuyruğa ekleyin
            queue.Enqueue(new Coordinate(startX, startY));
            visited[startX, startY] = true;

            // Hareket yönleri (yukarı, aşağı, sol, sağ)
            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            while (queue.Count > 0)
            {
                Coordinate current = queue.Dequeue();

                // İlk satıra ulaşıldıysa
                if (current.x == 0)
                {
                    return true;
                }

                // Tüm komşu hücreleri kontrol edin
                for (int i = 0; i < 4; i++)
                {
                    int newX = current.x + dx[i];
                    int newY = current.y + dy[i];

                    // Grid sınırları içinde mi ve ziyaret edilmemiş mi ve engel değil mi ve obje değil mi
                    if (newX >= 0 && newX < rows && newY >= 0 && newY < cols && !visited[newX, newY] && grid[newX, newY] == 0)
                    {
                        queue.Enqueue(new Coordinate(newX, newY));
                        visited[newX, newY] = true;
                    }
                }
            }

            return false;
        }

        // Tüm objeler için çıkış yolu olanları bulma
        public List<Coordinate> GetObjectsWithEscapePath(int[,] grid)
        {
            List<Coordinate> objectsWithPath = new List<Coordinate>();
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    // Grid'de obje (2) bulunan hücreleri kontrol et
                    if (grid[x, y] == 2)
                    {
                        if (IsPathToFirstRow(grid, x, y))
                        {
                            objectsWithPath.Add(new Coordinate(x, y));
                        }
                    }
                }
            }

            return objectsWithPath;
        }
    }
}