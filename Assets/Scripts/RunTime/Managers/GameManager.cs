using RunTime.Signals;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RunTime.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static bool IsSpawnedObject { get; set; }

        private void Awake()
        {
            Time.timeScale = 1;
        }

        private void OnEnable()
        {
            CoreGameSignals.Instance.onLoadScene += LoadScene;
        }
        private void OnDisable()
        {
            CoreGameSignals.Instance.onLoadScene -= LoadScene;
        }

        private void LoadScene(int sceneIndex)
        {
            StartCoroutine(ILoadSceneAsync(sceneIndex));
        }

        private IEnumerator ILoadSceneAsync(int sceneIndex)
        {
            SceneManager.LoadSceneAsync(sceneIndex);
            yield return null;
        }
    }
}