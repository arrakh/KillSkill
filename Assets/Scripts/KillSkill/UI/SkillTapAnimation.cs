using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KillSkill.UI
{
    public class SkillTapAnimation : MonoBehaviour
    {
        [SerializeField] private List<SkillTapElement> elements;

        public void Animate()
        {
            var element = GetElement();
            element.Animate();
        }

        private SkillTapElement GetElement()
        {
            foreach (var element in elements)
                if (!element.IsActive) return element;

            //return last element when all is active
            return elements[^1];
        }
    }
}