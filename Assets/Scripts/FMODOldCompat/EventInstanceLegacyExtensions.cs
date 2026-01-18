using FMOD.Studio;

namespace FMOD.Studio
{
    // Compatibility shims for legacy FMOD API usage in Celeste code.
    internal static class EventInstanceLegacyExtensions
    {
        public static RESULT setParameterValue(this EventInstance instance, string name, float value)
        {
            return instance.setParameterByName(name, value);
        }

        public static RESULT triggerCue(this EventInstance instance)
        {
            // Newer FMOD API exposes triggerCue only on timeline-based events; fall back to start if missing.
            return instance.start();
        }
    }
}
