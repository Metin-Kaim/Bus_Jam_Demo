using RunTime.Controllers;
using RunTime.Handlers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RunTime.Signals
{
    public class GridSignals : MonoBehaviour
    {
        public static GridSignals Instance;

        public Func<TileHandler[,]> onGetGridTiles;
        public Func<List<Coordinate>> onGetActiveObjectCoordinates;
        public Func<int, int, List<Coordinate>> onGetPathToExit;

        private void Awake()
        {
            Instance = this;
        }
    }

}