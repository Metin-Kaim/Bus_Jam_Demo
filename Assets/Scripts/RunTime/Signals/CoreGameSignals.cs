using UnityEngine;
using UnityEngine.Events;

namespace RunTime.Signals
{
    public class CoreGameSignals : MonoBehaviour
    {
        public static CoreGameSignals Instance;

        public UnityAction onWin;
        public UnityAction onLose;
        public UnityAction<int> onLoadScene;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
    }
}