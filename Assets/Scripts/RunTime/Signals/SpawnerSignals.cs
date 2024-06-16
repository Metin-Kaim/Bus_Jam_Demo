using UnityEngine;
using UnityEngine.Events;

namespace RunTime.Signals
{
    public class SpawnerSignals : MonoBehaviour
    {
        public static SpawnerSignals Instance;

        public UnityAction onCheckToSpawnObject;

        private void Awake()
        {
            Instance = this;
        }
    }
}