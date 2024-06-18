using RunTime.Signals;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RunTime.Managers
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] GameObject _winPanel;
        [SerializeField] GameObject _losePanel;
        [SerializeField] TextMeshProUGUI _levelTxt;

        private void Start()
        {
            _levelTxt.text = $"Level {SaveManager.Instance.SaveCurrentLevelIndex + 1}";
        }

        private void OnEnable()
        {
            CoreGameSignals.Instance.onWin += OnWin;
            CoreGameSignals.Instance.onLose += OnLose;
        }

        private void OnWin()
        {
            OnPause();
            StartCoroutine(OpenPanel(_winPanel));
        }
        private void OnLose()
        {
            OnPause();
            StartCoroutine(OpenPanel(_losePanel));
        }

        private void OnDisable()
        {
            CoreGameSignals.Instance.onWin -= OnWin;
            CoreGameSignals.Instance.onLose -= OnLose;
        }

        private void OnPause()
        {
            InputSignals.Instance.onCloseClickable?.Invoke();
        }

        private IEnumerator OpenPanel(GameObject panel)
        {
            yield return new WaitForSeconds(1);
            panel.SetActive(true);
            Time.timeScale = 0;

        }

        public void OnClickedPanelButton() // Level up - Restart
        {
            CoreGameSignals.Instance.onLoadScene?.Invoke(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnClickedStartButton()
        {
            CoreGameSignals.Instance.onLoadScene?.Invoke(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}