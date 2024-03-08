using System;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI
{
    public class SkillTapElement : MonoBehaviour
    {
        [SerializeField] private Image circleImage;
        [SerializeField] private float animateTime = 0.3f;
        [SerializeField] private float spinSpeed = 360f;

        private float currentTime = 0f;

        public bool IsActive => currentTime < animateTime;

        private void Awake()
        {
            currentTime = animateTime;
        }

        private void Update()
        {
            if (!IsActive) return;
            
            transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);

            var a = currentTime / animateTime;
            var color = circleImage.color;
            color.a = 1f - a;
            circleImage.color = color;
            
            circleImage.transform.localScale = Vector3.one * a;
            
            currentTime += Time.deltaTime;
        }

        public void Animate()
        {
            currentTime = 0f;
        }
    }
}