using RunTime.Handlers;
using RunTime.Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Controllers
{
    public class ObjectController : MonoBehaviour
    {
        TileHandler[,] tileHandlers;
        List<Coordinate> coordinates;
        private IEnumerator Start()
        {
            yield return null;

            tileHandlers = GridSignals.Instance.onGetGridTiles?.Invoke();
            coordinates = GridSignals.Instance.onGetActiveObjectCoordinates?.Invoke();

            for (int i = 0; i < coordinates.Count; i++)
            {
                tileHandlers[coordinates[i].x, coordinates[i].y].CurrentObjectHandler.OpenThisObject();
            }
        }
    }
}