using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using UnityEngine;

namespace Monocle
{
    /// <summary>
    /// Unity-adapted Draw class.
    /// Original XNA SpriteBatch drawing is replaced with Unity's rendering system.
    /// For actual rendering in Unity, use SpriteRenderers, LineRenderers, or Graphics.DrawMesh.
    /// This class maintains API compatibility for Celeste code that calls these methods.
    /// </summary>
    public static class Draw
    {
        public static MTexture Particle;
        public static MTexture Pixel;
        private static Microsoft.Xna.Framework.Rectangle rect;

        public static Renderer Renderer { get; internal set; }

        // Stub SpriteBatch for compatibility - actual rendering should use Unity systems
        public static SpriteBatch SpriteBatch { get; private set; }

        public static SpriteFont DefaultFont { get; private set; }

        // Unity-specific drawing helpers
        private static Material _lineMaterial;
        
        internal static void Initialize()
        {
            SpriteBatch = new SpriteBatch();
            DefaultFont = new SpriteFont();
            UseDebugPixelTexture();
            CreateLineMaterial();
        }

        private static void CreateLineMaterial()
        {
            if (_lineMaterial == null)
            {
                // Unity standard unlit shader for drawing
                Shader shader = Shader.Find("Sprites/Default");
                if (shader != null)
                {
                    _lineMaterial = new Material(shader);
                    _lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                }
            }
        }

        public static void UseDebugPixelTexture()
        {
            MTexture parent = new(VirtualContent.CreateTexture("debug-pixel", 3, 3, Microsoft.Xna.Framework.Color.White));
            Pixel = new MTexture(parent, 1, 1, 1, 1);
            Particle = new MTexture(parent, 1, 1, 1, 1);
        }

        // The following methods maintain API compatibility but rendering should be handled by Unity
        // In a full port, these would draw using Unity's GL class, Graphics.DrawMesh, or LineRenderer

        public static void Point(Microsoft.Xna.Framework.Vector2 at, Microsoft.Xna.Framework.Color color)
        {
            // Unity: Use GL.Begin/GL.End or spawn a point particle
        }

        public static void Line(Microsoft.Xna.Framework.Vector2 start, Microsoft.Xna.Framework.Vector2 end, Microsoft.Xna.Framework.Color color) 
            => LineAngle(start, Calc.Angle(start, end), Microsoft.Xna.Framework.Vector2.Distance(start, end), color);

        public static void Line(Microsoft.Xna.Framework.Vector2 start, Microsoft.Xna.Framework.Vector2 end, Microsoft.Xna.Framework.Color color, float thickness) 
            => LineAngle(start, Calc.Angle(start, end), Microsoft.Xna.Framework.Vector2.Distance(start, end), color, thickness);

        public static void Line(float x1, float y1, float x2, float y2, Microsoft.Xna.Framework.Color color) 
            => Line(new Microsoft.Xna.Framework.Vector2(x1, y1), new Microsoft.Xna.Framework.Vector2(x2, y2), color);

        public static void Line(float x1, float y1, float x2, float y2, Microsoft.Xna.Framework.Color color, float thickness)
        {
            Line(new Microsoft.Xna.Framework.Vector2(x1, y1), new Microsoft.Xna.Framework.Vector2(x2, y2), color, thickness);
        }

        public static void LineAngle(Microsoft.Xna.Framework.Vector2 start, float angle, float length, Microsoft.Xna.Framework.Color color)
        {
            // Unity: Use GL.Begin(GL.LINES) or LineRenderer
        }

        public static void LineAngle(Microsoft.Xna.Framework.Vector2 start, float angle, float length, Microsoft.Xna.Framework.Color color, float thickness)
        {
            // Unity: Use GL.Begin(GL.QUADS) or LineRenderer with width
        }

        public static void LineAngle(float startX, float startY, float angle, float length, Microsoft.Xna.Framework.Color color)
        {
            LineAngle(new Microsoft.Xna.Framework.Vector2(startX, startY), angle, length, color);
        }

        public static void Circle(Microsoft.Xna.Framework.Vector2 position, float radius, Microsoft.Xna.Framework.Color color, int resolution)
        {
            Microsoft.Xna.Framework.Vector2 vector1 = Microsoft.Xna.Framework.Vector2.UnitX * radius;
            Microsoft.Xna.Framework.Vector2 vector2_1 = vector1.Perpendicular();
            for (int index = 1; index <= resolution; ++index)
            {
                Microsoft.Xna.Framework.Vector2 vector2 = Calc.AngleToVector(index * 1.57079637f / resolution, radius);
                Microsoft.Xna.Framework.Vector2 vector2_2 = vector2.Perpendicular();
                Line(position + vector1, position + vector2, color);
                Line(position - vector1, position - vector2, color);
                Line(position + vector2_1, position + vector2_2, color);
                Line(position - vector2_1, position - vector2_2, color);
                vector1 = vector2;
                vector2_1 = vector2_2;
            }
        }

        public static void Circle(float x, float y, float radius, Microsoft.Xna.Framework.Color color, int resolution) 
            => Circle(new Microsoft.Xna.Framework.Vector2(x, y), radius, color, resolution);

        public static void Circle(Microsoft.Xna.Framework.Vector2 position, float radius, Microsoft.Xna.Framework.Color color, float thickness, int resolution)
        {
            Microsoft.Xna.Framework.Vector2 vector1 = Microsoft.Xna.Framework.Vector2.UnitX * radius;
            Microsoft.Xna.Framework.Vector2 vector2_1 = vector1.Perpendicular();
            for (int index = 1; index <= resolution; ++index)
            {
                Microsoft.Xna.Framework.Vector2 vector2 = Calc.AngleToVector(index * 1.57079637f / resolution, radius);
                Microsoft.Xna.Framework.Vector2 vector2_2 = vector2.Perpendicular();
                Line(position + vector1, position + vector2, color, thickness);
                Line(position - vector1, position - vector2, color, thickness);
                Line(position + vector2_1, position + vector2_2, color, thickness);
                Line(position - vector2_1, position - vector2_2, color, thickness);
                vector1 = vector2;
                vector2_1 = vector2_2;
            }
        }

        public static void Circle(float x, float y, float radius, Microsoft.Xna.Framework.Color color, float thickness, int resolution)
        {
            Circle(new Microsoft.Xna.Framework.Vector2(x, y), radius, color, thickness, resolution);
        }

        public static void Rect(float x, float y, float width, float height, Microsoft.Xna.Framework.Color color)
        {
            // Unity: Use GL.Begin(GL.QUADS) or Graphics.DrawMesh with quad
        }

        public static void Rect(Microsoft.Xna.Framework.Vector2 position, float width, float height, Microsoft.Xna.Framework.Color color) 
            => Rect(position.X, position.Y, width, height, color);

        public static void Rect(Microsoft.Xna.Framework.Rectangle rect, Microsoft.Xna.Framework.Color color)
        {
            Rect(rect.X, rect.Y, rect.Width, rect.Height, color);
        }

        public static void Rect(Collider collider, Microsoft.Xna.Framework.Color color) 
            => Rect(collider.AbsoluteLeft, collider.AbsoluteTop, collider.Width, collider.Height, color);

        public static void HollowRect(float x, float y, float width, float height, Microsoft.Xna.Framework.Color color)
        {
            // Draw four lines to make a hollow rectangle
            Line(x, y, x + width, y, color);
            Line(x + width, y, x + width, y + height, color);
            Line(x + width, y + height, x, y + height, color);
            Line(x, y + height, x, y, color);
        }

        public static void HollowRect(Microsoft.Xna.Framework.Vector2 position, float width, float height, Microsoft.Xna.Framework.Color color) 
            => HollowRect(position.X, position.Y, width, height, color);

        public static void HollowRect(Microsoft.Xna.Framework.Rectangle rect, Microsoft.Xna.Framework.Color color) 
            => HollowRect(rect.X, rect.Y, rect.Width, rect.Height, color);

        public static void HollowRect(Collider collider, Microsoft.Xna.Framework.Color color) 
            => HollowRect(collider.AbsoluteLeft, collider.AbsoluteTop, collider.Width, collider.Height, color);

        public static void Text(SpriteFont font, string text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color)
        {
            // Unity: Use TextMeshPro or GUI.Label
        }

        public static void Text(SpriteFont font, string text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color, Microsoft.Xna.Framework.Vector2 origin, Microsoft.Xna.Framework.Vector2 scale, float rotation)
        {
            // Unity: Use TextMeshPro with transform
        }

        public static void TextJustified(SpriteFont font, string text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color, Microsoft.Xna.Framework.Vector2 justify)
        {
            Microsoft.Xna.Framework.Vector2 origin = font.MeasureString(text);
            origin.X *= justify.X;
            origin.Y *= justify.Y;
            Text(font, text, position - origin, color);
        }

        public static void TextJustified(SpriteFont font, string text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color, float scale, Microsoft.Xna.Framework.Vector2 justify)
        {
            Microsoft.Xna.Framework.Vector2 origin = font.MeasureString(text);
            origin.X *= justify.X;
            origin.Y *= justify.Y;
            Text(font, text, position, color, origin, Microsoft.Xna.Framework.Vector2.One * scale, 0f);
        }

        public static void TextCentered(SpriteFont font, string text, Microsoft.Xna.Framework.Vector2 position) 
            => Text(font, text, position - font.MeasureString(text) * 0.5f, Microsoft.Xna.Framework.Color.White);

        public static void TextCentered(SpriteFont font, string text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color) 
            => Text(font, text, position - font.MeasureString(text) * 0.5f, color);

        public static void TextCentered(SpriteFont font, string text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color, float scale)
        {
            Text(font, text, position, color, font.MeasureString(text) * 0.5f, Microsoft.Xna.Framework.Vector2.One * scale, 0.0f);
        }

        public static void TextCentered(SpriteFont font, string text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color, float scale, float rotation)
        {
            Text(font, text, position, color, font.MeasureString(text) * 0.5f, Microsoft.Xna.Framework.Vector2.One * scale, rotation);
        }

        public static void OutlineTextCentered(SpriteFont font, string text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color, float scale)
        {
            // Unity: Use TextMeshPro with outline settings
        }

        public static void OutlineTextCentered(SpriteFont font, string text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color, Microsoft.Xna.Framework.Color outlineColor)
        {
            // Unity: Use TextMeshPro with outline settings
        }

        public static void OutlineTextCentered(SpriteFont font, string text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color, Microsoft.Xna.Framework.Color outlineColor, float scale)
        {
            // Unity: Use TextMeshPro with outline settings
        }

        public static void OutlineTextJustify(SpriteFont font, string text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color, Microsoft.Xna.Framework.Color outlineColor, Microsoft.Xna.Framework.Vector2 justify)
        {
            // Unity: Use TextMeshPro with outline settings
        }

        public static void OutlineTextJustify(SpriteFont font, string text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color, Microsoft.Xna.Framework.Color outlineColor, Microsoft.Xna.Framework.Vector2 justify, float scale)
        {
            // Unity: Use TextMeshPro with outline settings
        }

        public static void SineTextureH(MTexture tex, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 origin, Microsoft.Xna.Framework.Vector2 scale, float rotation, Microsoft.Xna.Framework.Color color, SpriteEffects effects, float sineCounter, float amplitude = 2f, int sliceSize = 2, float sliceAdd = 0.7853982f)
        {
            // Unity: Use custom shader with sine wave distortion
        }

        public static void SineTextureV(MTexture tex, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 origin, Microsoft.Xna.Framework.Vector2 scale, float rotation, Microsoft.Xna.Framework.Color color, SpriteEffects effects, float sineCounter, float amplitude = 2f, int sliceSize = 2, float sliceAdd = 0.7853982f)
        {
            // Unity: Use custom shader with sine wave distortion
        }

        public static void TextureBannerV(MTexture tex, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 origin, Microsoft.Xna.Framework.Vector2 scale, float rotation, Microsoft.Xna.Framework.Color color, SpriteEffects effects, float sineCounter, float amplitude = 2f, int sliceSize = 2, float sliceAdd = 0.7853982f)
        {
            // Unity: Use custom shader with banner distortion
        }
    }
}
