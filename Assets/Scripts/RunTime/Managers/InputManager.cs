using RunTime.Signals;
using System.Collections;
using UnityEngine;

namespace RunTime.Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] bool _isClickable;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(.5f);
            _isClickable = true;
        }

        private void OnEnable()
        {
            InputSignals.Instance.onGetIsClickable += () => _isClickable;
            InputSignals.Instance.onCloseClickable += () => _isClickable = false;
            InputSignals.Instance.onOpenClickable += () => _isClickable = true;
        }
        private void OnDisable()
        {
            InputSignals.Instance.onGetIsClickable -= () => _isClickable;
            InputSignals.Instance.onCloseClickable -= () => _isClickable = false;
            InputSignals.Instance.onOpenClickable -= () => _isClickable = true;
        }
    }
}