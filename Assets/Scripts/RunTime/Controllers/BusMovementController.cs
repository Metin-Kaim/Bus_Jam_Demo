using DG.Tweening;
using RunTime.Handlers;
using System.Collections.Generic;
using UnityEngine;

namespace RunTime.Controllers
{
    public class BusMovementController : MonoBehaviour
    {
        [SerializeField] Transform _centerBusPoint;
        [SerializeField] Transform _leftSideBusPoint;

        public List<BusHandler> buses;

        public void MoveTheBuses()
        {
            if (buses.Count >= 1)
                buses[0].transform.DOMoveX(_centerBusPoint.position.x, 1);

            if (buses.Count >= 2)
                buses[1].transform.DOMoveX(_leftSideBusPoint.position.x, 1);
        }
    }
}