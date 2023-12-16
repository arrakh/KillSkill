using System;
using UnityEngine;

namespace Systems
{
    public class PauseSystem : MonoBehaviour
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