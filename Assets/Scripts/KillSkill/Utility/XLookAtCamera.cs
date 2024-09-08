using System;
using UnityEngine;

namespace KillSkill.Utility
{
    public class XLookAtCamera : MonoBehaviour
    {
        [SerializeField] private float xOffset;
        private Camera camera;
        
        private void Awake()
        {
            camera = Camera.main;
        }

        private void Update()
        {
            Vector3 direction = 2 * camera.transform.position - transform.position;
        
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(xOffset + rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }
}