using Arr.EventsSystem;
using KillSkill.Modules.VisualEffects.Events;
using VisualEffects;

namespace KillSkill.Modules.VisualEffects
{
    public static class Vfx
    {
        public static IVisualEffectsHandler GetHandler() => GlobalEvents.Query<VisualEffectsHandlerQuery>().handler;
    }
}