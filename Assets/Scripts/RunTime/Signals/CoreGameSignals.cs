using UnityEngine;
using UnityEngine.Events;

namespace RunTime.Signals
{
    public class CoreGameSignals : MonoBehaviour
    {
        public static CoreGameSignals Instance;

        public UnityAction onWin;
        public UnityAction<int> onLoadScene;

        private void Awake()
        {
            Instance = this;
        }
    }
}