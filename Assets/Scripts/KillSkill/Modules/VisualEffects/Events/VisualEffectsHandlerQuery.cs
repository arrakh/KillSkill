using VisualEffects;

namespace KillSkill.Modules.VisualEffects.Events
{
    public class VisualEffectsHandlerQuery
    {
        public readonly IVisualEffectsHandler handler;

        public VisualEffectsHandlerQuery(IVisualEffectsHandler handler)
        {
            this.handler = handler;
        }
    }
}