using RunTime.Datas.UnityObjects;
using RunTime.Managers;
using RunTime.Signals;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.RunTime.Managers
{
    public class LevelManager : MonoBehaviour
    {
        private int _currentLevelIndex;
        private LevelInfos_SO _levelInfos;

        private void Awake()
        {
            _currentLevelIndex = SaveManager.Instance.SaveCurrentLevelIndex;
            _levelInfos = Resources.Load<LevelInfos_SO>($"RunTime/Levels/Level {_currentLevelIndex}");
        }

        private void OnEnable()
        {
            LevelSignals.Instance.onGetCurrentLevelInfos += OnGetCurrentLevelInfos;
            CoreGameSignals.Instance.onWin += IncreaseLevelIndex;
        }
        private void OnDisable()
        {
            LevelSignals.Instance.onGetCurrentLevelInfos -= OnGetCurrentLevelInfos;
            CoreGameSignals.Instance.onWin -= IncreaseLevelIndex;
        }

        private LevelInfos_SO OnGetCurrentLevelInfos()
        {
            return _levelInfos;
        }

        public void IncreaseLevelIndex()
        {
            SaveManager.Instance.SaveCurrentLevelIndex++;
            print(SaveManager.Instance.SaveCurrentLevelIndex);
        }
    }
}