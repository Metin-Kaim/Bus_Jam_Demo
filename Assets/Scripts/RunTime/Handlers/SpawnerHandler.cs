using UnityEngine;

namespace RunTime.Handlers
{
    public class SpawnerHandler : MonoBehaviour
    {
        

        private void Start()
        {
            print(transform.rotation.eulerAngles);
        }
    }
}