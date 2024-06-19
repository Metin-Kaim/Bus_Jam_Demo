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

    public class PathFindingController : MonoBehaviour
    {
        public int[,] grid = new int[9, 9];
        public List<Coordinate> escapePaths;
        LevelInfos_SO levelInfos_SO;

        private void OnEnable()
        {
            GridSignals.Instance.onGetActiveObjectCoordinates += () => escapePaths;
            GridSignals.Instance.onGetPathToExit += FindPathToFirstRow;
            GridSignals.Instance.onGetGrid += () => grid;
        }
        private void OnDisable()
        {
            GridSignals.Instance.onGetActiveObjectCoordinates -= () => escapePaths;
            GridSignals.Instance.onGetPathToExit -= FindPathToFirstRow;
            GridSignals.Instance.onGetGrid -= () => grid;
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

            escapePaths = GetObjectsWithEscapePath();

        }

        public bool IsPathToFirstRow(int startX, int startY)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            bool[,] visited = new bool[rows, cols];
            Queue<Coordinate> queue = new Queue<Coordinate>();

            queue.Enqueue(new Coordinate(startX, startY));
            visited[startX, startY] = true;

            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            while (queue.Count > 0)
            {
                Coordinate current = queue.Dequeue();

                if (current.x == 0)
                {
                    return true;
                }

                for (int i = 0; i < 4; i++)
                {
                    int newX = current.x + dx[i];
                    int newY = current.y + dy[i];

                    if (newX >= 0 && newX < rows && newY >= 0 && newY < cols && !visited[newX, newY] && grid[newX, newY] == 0)
                    {
                        queue.Enqueue(new Coordinate(newX, newY));
                        visited[newX, newY] = true;
                    }
                }
            }

            return false;
        }
        public List<Coordinate> GetObjectsWithEscapePath()
        {
            List<Coordinate> objectsWithPath = new List<Coordinate>();
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    if (grid[x, y] == 2)
                    {
                        if (IsPathToFirstRow(x, y))
                        {
                            objectsWithPath.Add(new Coordinate(x, y));
                        }
                    }
                }
            }

            return objectsWithPath;
        }

        private List<Coordinate> FindPathToFirstRow(int startX, int startY)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            bool[,] visited = new bool[rows, cols];
            Coordinate[,] parents = new Coordinate[rows, cols];
            Queue<Coordinate> queue = new();
            List<Coordinate> path = new();

            queue.Enqueue(new Coordinate(startX, startY));
            visited[startX, startY] = true;
            parents[startX, startY] = new Coordinate(-1, -1);

            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            while (queue.Count > 0)
            {
                Coordinate current = queue.Dequeue();

                if (current.x == 0)
                {
                    while (current.x != -1 && current.y != -1)
                    {
                        path.Add(current);
                        current = parents[current.x, current.y];
                    }
                    path.Reverse();

                    path.RemoveAt(0);
                    grid[startX, startY] = 0;
                    return path;
                }

                for (int i = 0; i < 4; i++)
                {
                    int newX = current.x + dx[i];
                    int newY = current.y + dy[i];

                    if (newX >= 0 && newX < rows && newY >= 0 && newY < cols && !visited[newX, newY] && grid[newX, newY] == 0)
                    {
                        queue.Enqueue(new Coordinate(newX, newY));
                        visited[newX, newY] = true;
                        parents[newX, newY] = current;
                    }
                }
            }

            return path;
        }
    }
}