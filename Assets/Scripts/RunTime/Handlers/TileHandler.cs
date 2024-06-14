using Assets.Scripts.RunTime.Handlers;
using RunTime.Signals;
using UnityEngine;

namespace RunTime.Handlers
{
    public class TileHandler : MonoBehaviour
    {
        ObjectHandler _currentObjectHandler;
        [SerializeField] private byte row;
        [SerializeField] private byte col;
        public bool IsChecked;

        public ObjectHandler CurrentObjectHandler { get => _currentObjectHandler; set => _currentObjectHandler = value; }
        public byte Row { get => row; set => row = value; }
        public byte Col { get => col; set => col = value; }

        public void ActivateOtherObjects(int row, int col)
        {
            if (row <= 8 && row >= 0)//check for bounds
            {
                if (col <= 8 && col >= 0)//check for bounds
                {
                    TileHandler[,] tileHandlers = GridSignals.Instance.onGetGridTiles.Invoke();
                    TileHandler otherTile = tileHandlers[row, col];

                    if (otherTile == null)
                    {
                        return;
                    }

                    if (otherTile.IsChecked) return;
                    else if (otherTile.CurrentObjectHandler == null)
                    {
                        otherTile.IsChecked = true;
                        otherTile.CheckEveryDirections(row, col);
                    }
                    else if (otherTile.CurrentObjectHandler.IsClickable == false)
                    {
                        otherTile.IsChecked = true;
                        otherTile.CurrentObjectHandler.OpenThisObject();
                    }

                }
            }
        }


        private void CheckEveryDirections(int row, int col)
        {
            ActivateOtherObjects(row, col + 1);
            ActivateOtherObjects(row, col - 1);
            ActivateOtherObjects(row + 1, col);
            ActivateOtherObjects(row - 1, col);
        }

        public void OpenAccessibleObjects()
        {
            IsChecked = true;
            CheckEveryDirections(Row, Col);
            //CurrentObject.tileHandler = null; // Remember: eğer arama algoritmasında sıkıntı çıkarsa burayı aç!!! 
            CurrentObjectHandler = null;
            AllCheksFalse();
        }

        private void AllCheksFalse()
        {
            TileHandler[,] tileHandlers = GridSignals.Instance.onGetGridTiles.Invoke();

            foreach (var item in tileHandlers)
            {
                if (item == null) continue;
                item.IsChecked = false;
            }
        }
    }
}