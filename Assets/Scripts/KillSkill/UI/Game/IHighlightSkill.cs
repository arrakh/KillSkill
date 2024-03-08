using System;

namespace KillSkill.UI.Game
{
    public interface IHighlightSkill
    {
        public event Action<bool> OnSetHighlight;
    }
}