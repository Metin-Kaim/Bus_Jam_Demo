using RunTime.Signals;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RunTime.Managers
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] GameObject _gameOverPanel;

        private void OnEnable()
        {
            CoreGameSignals.Instance.onWin += OnWin;
        }

        private void OnWin()
        {
            InputSignals.Instance.onCloseClickable?.Invoke();
            StartCoroutine(OpenPanel(_gameOverPanel));
        }

        private void OnDisable()
        {
            CoreGameSignals.Instance.onWin -= OnWin;
        }

        private IEnumerator OpenPanel(GameObject panel)
        {
            yield return new WaitForSeconds(2);
            panel.SetActive(true);
        }

        public void OnClickedLevelUpButton()
        {
            CoreGameSignals.Instance.onLoadScene?.Invoke(SceneManager.GetActiveScene().buildIndex);
        }
    }
}