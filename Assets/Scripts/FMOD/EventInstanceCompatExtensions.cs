using FMOD.Studio;

namespace FMOD.Studio
{
    internal static class EventInstanceCompatExtensions
    {
        // Celeste upstream code uses the old lower-case wrapper method names.
        public static RESULT setParameterValue(this EventInstance instance, string name, float value)
            => instance.setParameterByName(name, value);

        public static RESULT triggerCue(this EventInstance instance)
            => RESULT.OK;
    }
}
