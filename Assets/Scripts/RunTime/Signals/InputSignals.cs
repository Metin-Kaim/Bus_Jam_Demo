using System;
using UnityEngine;
using UnityEngine.Events;

namespace RunTime.Signals
{
    public class InputSignals : MonoBehaviour
    {
        public static InputSignals Instance;

        public UnityAction onCloseClickable;
        public UnityAction onOpenClickable;
        public Func<bool> onGetIsClickable;

        private void Awake()
        {
            Instance = this;
        }
    }
}