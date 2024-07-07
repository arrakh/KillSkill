using Cinemachine;
using UnityEngine;

namespace KillSkill
{
    public class CameraController : MonoBehaviour
    {
        public Camera Main => mainCamera;
        
        [SerializeField] private CinemachineTargetGroup targetGroup;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        public void AddTargetToGroup(Transform t, float weight = 1f, float radius = 1f)
        {
            targetGroup.AddMember(t, weight, radius);
        }
    }
}