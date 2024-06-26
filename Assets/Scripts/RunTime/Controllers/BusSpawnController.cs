﻿using RunTime.Datas.UnityObjects;
using RunTime.Handlers;
using RunTime.Signals;
using System;
using UnityEngine;

namespace RunTime.Controllers
{
    public class BusSpawnController : MonoBehaviour
    {
        [SerializeField] BusHandler _busPrefab;
        [SerializeField] Transform _busContainer;

        private BusMovementController _busMovementController;
        private LevelInfos_SO _levelInfos;
        private int _busListCounter;

        private void Awake()
        {
            _busMovementController = GetComponent<BusMovementController>();
        }

        private void OnEnable()
        {
            BusSignals.Instance.onSpawnNewBus += OnSpawnBus;
            BusSignals.Instance.onGetCurrentBus += OnGetCurrentBus;
        }

        private BusHandler OnGetCurrentBus()
        {
            return _busMovementController.buses[0];
        }

        private void OnDisable()
        {
            BusSignals.Instance.onSpawnNewBus -= OnSpawnBus;
            BusSignals.Instance.onGetCurrentBus -= OnGetCurrentBus;
        }

        private void Start()
        {
            _levelInfos = LevelSignals.Instance.onGetCurrentLevelInfos?.Invoke();

            for (int i = 0; i < 2; i++)
            {
                SpawnBus();
            }

            _busMovementController.MoveTheBuses();
        }

        private void OnSpawnBus(BusHandler oldBus)
        {

            _busMovementController.buses.Remove(oldBus);

            if (_busMovementController.buses.Count <= 0)
            {
                CoreGameSignals.Instance.onWin?.Invoke();
                return;
            }

            SpawnBus();
            _busMovementController.MoveTheBuses();

        }

        private void SpawnBus()
        {
            if (_levelInfos.levelBusInfos.Count <= _busListCounter)
            {
                return;
            }

            BusHandler busHandler = Instantiate(_busPrefab, _busContainer.position + (Vector3.up * .9f), Quaternion.identity, _busContainer);

            Enums.EntityTypes busColorType = _levelInfos.levelBusInfos[_busListCounter].busColorType;

            busHandler.EntityType = busColorType;

            busHandler.BusColor = (Color)(ColorSignals.Instance.onGetColor?.Invoke(busColorType));

            _busMovementController.buses.Add(busHandler);

            _busListCounter++;
        }
    }
}
