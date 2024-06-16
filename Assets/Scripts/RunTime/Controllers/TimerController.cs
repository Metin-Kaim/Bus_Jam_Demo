using RunTime.Datas.UnityObjects;
using RunTime.Signals;
using System.Collections;
using TMPro;
using UnityEngine;

namespace RunTime.Controllers
{
    public class TimerController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _timerText;

        private int _timer;
        private LevelInfos_SO _levelInfos;
        private IEnumerator _timerEnumerator;

        private void OnEnable()
        {
            CoreGameSignals.Instance.onWin += TimerStopper;
            CoreGameSignals.Instance.onLose += TimerStopper;
        }
        private void OnDisable()
        {
            CoreGameSignals.Instance.onWin -= TimerStopper;
            CoreGameSignals.Instance.onLose -= TimerStopper;
        }

        private void Start()
        {
            _levelInfos = LevelSignals.Instance.onGetCurrentLevelInfos();
            _timer = _levelInfos.timer;
            SetTimerText();
            _timerEnumerator = ITimerDecreaser();
            StartCoroutine(_timerEnumerator);
        }

        private IEnumerator ITimerDecreaser()
        {
            WaitForSeconds oneSecond = new(1);

            while (_timer > 0)
            {
                yield return oneSecond;
                _timer--;
                SetTimerText();
            }
            _timerEnumerator = null;
            CoreGameSignals.Instance.onLose?.Invoke();
        }

        private void TimerStopper()
        {
            if (_timerEnumerator != null)
            {
                StopCoroutine(_timerEnumerator);
                _timerEnumerator = null;
            }

        }

        private void SetTimerText()
        {
            _timerText.text = _timer.ToString();
        }
    }
}