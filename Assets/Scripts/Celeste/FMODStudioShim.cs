using FMOD;

namespace FMOD.Studio
{
    // Minimal shim for FMOD Studio 3D attributes used by Celeste code.
    public struct _3D_ATTRIBUTES
    {
        public VECTOR position;
        public VECTOR velocity;
        public VECTOR forward;
        public VECTOR up;
    }
}
