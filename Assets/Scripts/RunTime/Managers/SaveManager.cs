using UnityEngine;

namespace RunTime.Managers
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance;

        private readonly string _saveCurrentLevel;
        private readonly string _saveIsGameStarted;


        public int SaveGameIsStarted { get => PlayerPrefs.GetInt(_saveIsGameStarted); set => PlayerPrefs.SetInt(_saveIsGameStarted, value); }
        public int SaveCurrentLevelIndex { get => PlayerPrefs.GetInt(_saveCurrentLevel); set => PlayerPrefs.SetInt(_saveCurrentLevel, value); }

        private void Awake()
        {
            Instance = this;

            if (!PlayerPrefs.HasKey(_saveIsGameStarted) || PlayerPrefs.GetInt(_saveIsGameStarted) == 0)
            {
                SaveGameIsStarted = 1;
                SaveCurrentLevelIndex = 0;
            }
        }
    }
}