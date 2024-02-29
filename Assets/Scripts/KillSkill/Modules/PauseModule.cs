using UnityEngine;

namespace Modules
{
    public class PauseModule : MonoBehaviour
    {
        private bool paused = false;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                paused = !paused;
                Time.timeScale = paused ? 0f : 1f;
            }
        }
    }
}