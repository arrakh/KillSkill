using System.Collections.Generic;
using Arr;
using JetBrains.Annotations;
using UnityEngine;

namespace KillSkill.SettingsData
{
    public static class GameplaySettings
    {
        private static List<KeyCode> _skillBindings = new()
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
            KeyCode.Alpha0,
            KeyCode.Q,
            KeyCode.W,
            KeyCode.E,
            KeyCode.R,
            KeyCode.T,
            KeyCode.Y,
            KeyCode.U,
            KeyCode.I,
            KeyCode.O,
            KeyCode.P,
        };

        public static IReadOnlyList<KeyCode> SkillBindings => _skillBindings;
        public static EventTemplate OnBindingsUpdated = new();

        public static void SetSkillBinding(int index, KeyCode newKey)
        {
            _skillBindings[index] = newKey;
            OnBindingsUpdated?.Invoke();
        }

        public static string GetFormattedKeybinding(int index)
        {
            var binding = _skillBindings[index];
            return FormatBinding(binding.ToString());
        }
        
        private static string FormatBinding(string keycode)
        {
            if (keycode.StartsWith("Alpha")) return keycode.Substring(5);
            return keycode;
        }
    }
}