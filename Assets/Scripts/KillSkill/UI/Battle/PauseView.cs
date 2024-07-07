using Arr.ViewModuleSystem;
using TMPro;
using UnityEngine;

namespace KillSkill.UI.Battle
{
    public class PauseView : View
    {
        [SerializeField] private TextMeshProUGUI pauseText;
        [SerializeField] private GameObject pausedGroup;
        
        private bool paused = false;
        
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Space)) return;
            
            paused = !paused;
            Time.timeScale = paused ? 0f : 1f;
            pauseText.text = "[Space] " + (paused ? "Unpause" : "Pause");
            pausedGroup.SetActive(paused);
        }
    }
}