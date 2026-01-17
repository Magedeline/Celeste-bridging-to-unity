using UnityEngine;

namespace Monocle
{
    /// <summary>
    /// Unity replacement for XNA VirtualRenderTarget.
    /// Uses Unity's RenderTexture instead of XNA's RenderTarget2D.
    /// </summary>
    public class VirtualRenderTarget : VirtualAsset
    {
        public RenderTexture Target;
        public int MultiSampleCount;

        public bool Depth { get; private set; }

        public bool Preserve { get; private set; }

        public bool IsDisposed => Target == null;

        public Rect Bounds => new Rect(0, 0, Width, Height);

        internal VirtualRenderTarget(
            string name,
            int width,
            int height,
            int multiSampleCount,
            bool depth,
            bool preserve)
        {
            Name = name;
            Width = width;
            Height = height;
            MultiSampleCount = multiSampleCount;
            Depth = depth;
            Preserve = preserve;
            Reload();
        }

        internal override void Unload()
        {
            if (Target == null)
                return;
            Target.Release();
            Object.Destroy(Target);
            Target = null;
        }

        internal override void Reload()
        {
            Unload();
            
            // Create Unity RenderTexture
            RenderTextureFormat format = RenderTextureFormat.ARGB32;
            int depthBits = Depth ? 24 : 0;
            
            Target = new RenderTexture(Width, Height, depthBits, format);
            Target.antiAliasing = MultiSampleCount > 1 ? MultiSampleCount : 1;
            Target.name = Name;
            Target.Create();
        }

        public override void Dispose()
        {
            Unload();
            Target = null;
            VirtualContent.Remove(this);
        }

        public static implicit operator RenderTexture(VirtualRenderTarget target) => target?.Target;
    }
}
