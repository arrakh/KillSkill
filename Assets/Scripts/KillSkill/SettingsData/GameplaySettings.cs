using System.Collections.Generic;
using Arr;
using Arr.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace KillSkill.SettingsData
{
    public static class GameplaySettings
    {
        private static List<KeyCode> _skillBindings = new()
        {
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
            
            KeyCode.A,
            KeyCode.S,
            KeyCode.D,
            KeyCode.F,
            KeyCode.G,
            KeyCode.H,
            KeyCode.J,
            KeyCode.K,
            KeyCode.L,
            KeyCode.Semicolon,
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
            if (keycode.Equals("Semicolon")) return ";";
            if (keycode.StartsWith("Alpha")) return keycode.Substring(5);
            return keycode;
        }
    }
}