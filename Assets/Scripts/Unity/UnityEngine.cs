using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UnityEngine;

// Unity compatibility layer for XNA/MonoGame types
namespace Unity.Compatibility
{
    /// <summary>
    /// Compatibility layer to allow XNA/MonoGame code to work with Unity
    /// </summary>
    public static class XNAToUnity
    {
        // Convert XNA Vector2 to Unity Vector2
        public static UnityEngine.Vector2 ToUnity(this Microsoft.Xna.Framework.Vector2 vector)
        {
            return new UnityEngine.Vector2(vector.X, vector.Y);
        }
        
        // Convert Unity Vector2 to XNA Vector2
        public static Microsoft.Xna.Framework.Vector2 ToXNA(this UnityEngine.Vector2 vector)
        {
            return new Microsoft.Xna.Framework.Vector2(vector.x, vector.y);
        }
        
        // Convert XNA Vector3 to Unity Vector3
        public static UnityEngine.Vector3 ToUnity(this Microsoft.Xna.Framework.Vector3 vector)
        {
            return new UnityEngine.Vector3(vector.X, vector.Y, vector.Z);
        }
        
        // Convert Unity Vector3 to XNA Vector3
        public static Microsoft.Xna.Framework.Vector3 ToXNA(this UnityEngine.Vector3 vector)
        {
            return new Microsoft.Xna.Framework.Vector3(vector.x, vector.y, vector.z);
        }
        
        // Convert XNA Color to Unity Color
        public static UnityEngine.Color ToUnity(this Microsoft.Xna.Framework.Color color)
        {
            return new UnityEngine.Color(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }
        
        // Convert Unity Color to XNA Color
        public static Microsoft.Xna.Framework.Color ToXNA(this UnityEngine.Color color)
        {
            return new Microsoft.Xna.Framework.Color((byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), (byte)(color.a * 255));
        }
        
        // Convert XNA Rectangle to Unity Rect
        public static UnityEngine.Rect ToUnity(this Microsoft.Xna.Framework.Rectangle rectangle)
        {
            return new UnityEngine.Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
        
        // Convert Unity Rect to XNA Rectangle
        public static Microsoft.Xna.Framework.Rectangle ToXNA(this UnityEngine.Rect rect)
        {
            return new Microsoft.Xna.Framework.Rectangle((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
        }
    }
}