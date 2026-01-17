// XNA/MonoGame/FNA Compatibility Shim for Unity
// This file provides Unity equivalents for XNA Framework types
// allowing original Celeste code to compile with minimal changes.

using System;
using UnityEngine;

namespace Microsoft.Xna.Framework
{
    #region Math Types
    
    /// <summary>
    /// XNA-compatible Vector2 that wraps Unity's Vector2
    /// </summary>
    [Serializable]
    public struct Vector2 : IEquatable<Vector2>
    {
        public float X;
        public float Y;

        public static readonly Vector2 Zero = new Vector2(0, 0);
        public static readonly Vector2 One = new Vector2(1, 1);
        public static readonly Vector2 UnitX = new Vector2(1, 0);
        public static readonly Vector2 UnitY = new Vector2(0, 1);

        public Vector2(float x, float y) { X = x; Y = y; }
        public Vector2(float value) { X = value; Y = value; }

        public float Length() => (float)Math.Sqrt(X * X + Y * Y);
        public float LengthSquared() => X * X + Y * Y;

        public void Normalize()
        {
            float length = Length();
            if (length > 0)
            {
                X /= length;
                Y /= length;
            }
        }

        public static Vector2 Normalize(Vector2 value)
        {
            float length = value.Length();
            if (length > 0)
                return new Vector2(value.X / length, value.Y / length);
            return Zero;
        }

        public static float Dot(Vector2 a, Vector2 b) => a.X * b.X + a.Y * b.Y;
        public static float Distance(Vector2 a, Vector2 b) => (a - b).Length();
        public static float DistanceSquared(Vector2 a, Vector2 b) => (a - b).LengthSquared();

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
            => new Vector2(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);

        public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max)
            => new Vector2(
                MathHelper.Clamp(value.X, min.X, max.X),
                MathHelper.Clamp(value.Y, min.Y, max.Y));

        public static Vector2 Min(Vector2 a, Vector2 b)
            => new Vector2(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));

        public static Vector2 Max(Vector2 a, Vector2 b)
            => new Vector2(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));

        public static Vector2 Transform(Vector2 position, Matrix matrix)
        {
            return new Vector2(
                position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M41,
                position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M42);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator *(Vector2 a, Vector2 b) => new Vector2(a.X * b.X, a.Y * b.Y);
        public static Vector2 operator *(Vector2 v, float s) => new Vector2(v.X * s, v.Y * s);
        public static Vector2 operator *(float s, Vector2 v) => new Vector2(v.X * s, v.Y * s);
        public static Vector2 operator /(Vector2 a, Vector2 b) => new Vector2(a.X / b.X, a.Y / b.Y);
        public static Vector2 operator /(Vector2 v, float s) => new Vector2(v.X / s, v.Y / s);
        public static Vector2 operator -(Vector2 v) => new Vector2(-v.X, -v.Y);
        public static bool operator ==(Vector2 a, Vector2 b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Vector2 a, Vector2 b) => a.X != b.X || a.Y != b.Y;

        // Unity conversion
        public static implicit operator UnityEngine.Vector2(Vector2 v) => new UnityEngine.Vector2(v.X, v.Y);
        public static implicit operator Vector2(UnityEngine.Vector2 v) => new Vector2(v.x, v.y);

        public bool Equals(Vector2 other) => X == other.X && Y == other.Y;
        public override bool Equals(object obj) => obj is Vector2 v && Equals(v);
        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();
        public override string ToString() => $"{{X:{X} Y:{Y}}}";
    }

    /// <summary>
    /// XNA-compatible Vector3
    /// </summary>
    [Serializable]
    public struct Vector3 : IEquatable<Vector3>
    {
        public float X, Y, Z;

        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 One = new Vector3(1, 1, 1);
        public static readonly Vector3 UnitX = new Vector3(1, 0, 0);
        public static readonly Vector3 UnitY = new Vector3(0, 1, 0);
        public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);
        public static readonly Vector3 Up = new Vector3(0, 1, 0);
        public static readonly Vector3 Down = new Vector3(0, -1, 0);
        public static readonly Vector3 Left = new Vector3(-1, 0, 0);
        public static readonly Vector3 Right = new Vector3(1, 0, 0);
        public static readonly Vector3 Forward = new Vector3(0, 0, -1);
        public static readonly Vector3 Backward = new Vector3(0, 0, 1);

        public Vector3(float x, float y, float z) { X = x; Y = y; Z = z; }
        public Vector3(float value) { X = Y = Z = value; }
        public Vector3(Vector2 xy, float z) { X = xy.X; Y = xy.Y; Z = z; }

        public float Length() => (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        public float LengthSquared() => X * X + Y * Y + Z * Z;

        public void Normalize()
        {
            float length = Length();
            if (length > 0) { X /= length; Y /= length; Z /= length; }
        }

        public static Vector3 Normalize(Vector3 value)
        {
            float length = value.Length();
            if (length > 0)
                return new Vector3(value.X / length, value.Y / length, value.Z / length);
            return Zero;
        }

        public static float Dot(Vector3 a, Vector3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        
        public static Vector3 Cross(Vector3 a, Vector3 b)
            => new Vector3(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X);

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
            => new Vector3(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t, a.Z + (b.Z - a.Z) * t);

        public static Vector3 Transform(Vector3 position, Matrix matrix)
        {
            return new Vector3(
                position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41,
                position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42,
                position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vector3 operator *(Vector3 v, float s) => new Vector3(v.X * s, v.Y * s, v.Z * s);
        public static Vector3 operator *(float s, Vector3 v) => new Vector3(v.X * s, v.Y * s, v.Z * s);
        public static Vector3 operator /(Vector3 v, float s) => new Vector3(v.X / s, v.Y / s, v.Z / s);
        public static Vector3 operator -(Vector3 v) => new Vector3(-v.X, -v.Y, -v.Z);
        public static bool operator ==(Vector3 a, Vector3 b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        public static bool operator !=(Vector3 a, Vector3 b) => !(a == b);

        // Unity conversion
        public static implicit operator UnityEngine.Vector3(Vector3 v) => new UnityEngine.Vector3(v.X, v.Y, v.Z);
        public static implicit operator Vector3(UnityEngine.Vector3 v) => new Vector3(v.x, v.y, v.z);

        public bool Equals(Vector3 other) => X == other.X && Y == other.Y && Z == other.Z;
        public override bool Equals(object obj) => obj is Vector3 v && Equals(v);
        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        public override string ToString() => $"{{X:{X} Y:{Y} Z:{Z}}}";
    }

    /// <summary>
    /// XNA-compatible Vector4
    /// </summary>
    [Serializable]
    public struct Vector4 : IEquatable<Vector4>
    {
        public float X, Y, Z, W;

        public static readonly Vector4 Zero = new Vector4(0, 0, 0, 0);
        public static readonly Vector4 One = new Vector4(1, 1, 1, 1);

        public Vector4(float x, float y, float z, float w) { X = x; Y = y; Z = z; W = w; }
        public Vector4(float value) { X = Y = Z = W = value; }
        public Vector4(Vector2 xy, float z, float w) { X = xy.X; Y = xy.Y; Z = z; W = w; }
        public Vector4(Vector3 xyz, float w) { X = xyz.X; Y = xyz.Y; Z = xyz.Z; W = w; }

        public static Vector4 Lerp(Vector4 a, Vector4 b, float t)
            => new Vector4(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t, a.Z + (b.Z - a.Z) * t, a.W + (b.W - a.W) * t);

        public static Vector4 operator +(Vector4 a, Vector4 b) => new Vector4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        public static Vector4 operator -(Vector4 a, Vector4 b) => new Vector4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
        public static Vector4 operator *(Vector4 v, float s) => new Vector4(v.X * s, v.Y * s, v.Z * s, v.W * s);
        public static bool operator ==(Vector4 a, Vector4 b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;
        public static bool operator !=(Vector4 a, Vector4 b) => !(a == b);

        // Unity conversion
        public static implicit operator UnityEngine.Vector4(Vector4 v) => new UnityEngine.Vector4(v.X, v.Y, v.Z, v.W);
        public static implicit operator Vector4(UnityEngine.Vector4 v) => new Vector4(v.x, v.y, v.z, v.w);

        public bool Equals(Vector4 other) => X == other.X && Y == other.Y && Z == other.Z && W == other.W;
        public override bool Equals(object obj) => obj is Vector4 v && Equals(v);
        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();
        public override string ToString() => $"{{X:{X} Y:{Y} Z:{Z} W:{W}}}";
    }

    /// <summary>
    /// XNA-compatible Color
    /// </summary>
    [Serializable]
    public struct Color : IEquatable<Color>
    {
        public byte R, G, B, A;

        public static readonly Color Transparent = new Color(0, 0, 0, 0);
        public static readonly Color White = new Color(255, 255, 255, 255);
        public static readonly Color Black = new Color(0, 0, 0, 255);
        public static readonly Color Red = new Color(255, 0, 0, 255);
        public static readonly Color Green = new Color(0, 255, 0, 255);
        public static readonly Color Blue = new Color(0, 0, 255, 255);
        public static readonly Color Yellow = new Color(255, 255, 0, 255);
        public static readonly Color Cyan = new Color(0, 255, 255, 255);
        public static readonly Color Magenta = new Color(255, 0, 255, 255);
        public static readonly Color Gray = new Color(128, 128, 128, 255);
        public static readonly Color LightGray = new Color(192, 192, 192, 255);
        public static readonly Color DarkGray = new Color(64, 64, 64, 255);
        public static readonly Color Orange = new Color(255, 165, 0, 255);
        public static readonly Color Pink = new Color(255, 192, 203, 255);
        public static readonly Color Purple = new Color(128, 0, 128, 255);
        public static readonly Color Brown = new Color(139, 69, 19, 255);
        public static readonly Color CornflowerBlue = new Color(100, 149, 237, 255);
        public static readonly Color LimeGreen = new Color(50, 205, 50, 255);
        public static readonly Color SkyBlue = new Color(135, 206, 235, 255);

        public Color(byte r, byte g, byte b, byte a = 255) { R = r; G = g; B = b; A = a; }
        public Color(int r, int g, int b, int a = 255) 
        { 
            R = (byte)MathHelper.Clamp(r, 0, 255);
            G = (byte)MathHelper.Clamp(g, 0, 255);
            B = (byte)MathHelper.Clamp(b, 0, 255);
            A = (byte)MathHelper.Clamp(a, 0, 255);
        }
        public Color(float r, float g, float b, float a = 1f)
        {
            R = (byte)MathHelper.Clamp((int)(r * 255), 0, 255);
            G = (byte)MathHelper.Clamp((int)(g * 255), 0, 255);
            B = (byte)MathHelper.Clamp((int)(b * 255), 0, 255);
            A = (byte)MathHelper.Clamp((int)(a * 255), 0, 255);
        }
        public Color(Vector3 rgb) : this(rgb.X, rgb.Y, rgb.Z) { }
        public Color(Vector4 rgba) : this(rgba.X, rgba.Y, rgba.Z, rgba.W) { }

        public Color(uint packedValue)
        {
            R = (byte)(packedValue);
            G = (byte)(packedValue >> 8);
            B = (byte)(packedValue >> 16);
            A = (byte)(packedValue >> 24);
        }

        public uint PackedValue => (uint)(R | (G << 8) | (B << 16) | (A << 24));

        public Vector3 ToVector3() => new Vector3(R / 255f, G / 255f, B / 255f);
        public Vector4 ToVector4() => new Vector4(R / 255f, G / 255f, B / 255f, A / 255f);

        public static Color Lerp(Color a, Color b, float t)
        {
            return new Color(
                (byte)(a.R + (b.R - a.R) * t),
                (byte)(a.G + (b.G - a.G) * t),
                (byte)(a.B + (b.B - a.B) * t),
                (byte)(a.A + (b.A - a.A) * t));
        }

        public static Color Multiply(Color color, float scale)
        {
            return new Color(
                (byte)MathHelper.Clamp((int)(color.R * scale), 0, 255),
                (byte)MathHelper.Clamp((int)(color.G * scale), 0, 255),
                (byte)MathHelper.Clamp((int)(color.B * scale), 0, 255),
                color.A);
        }

        public static Color operator *(Color c, float s) => Multiply(c, s);
        public static Color operator *(float s, Color c) => Multiply(c, s);
        public static bool operator ==(Color a, Color b) => a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
        public static bool operator !=(Color a, Color b) => !(a == b);

        // Unity conversion
        public static implicit operator UnityEngine.Color(Color c) => new UnityEngine.Color(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
        public static implicit operator Color(UnityEngine.Color c) => new Color(c.r, c.g, c.b, c.a);
        public static implicit operator UnityEngine.Color32(Color c) => new UnityEngine.Color32(c.R, c.G, c.B, c.A);
        public static implicit operator Color(UnityEngine.Color32 c) => new Color(c.r, c.g, c.b, c.a);

        public bool Equals(Color other) => R == other.R && G == other.G && B == other.B && A == other.A;
        public override bool Equals(object obj) => obj is Color c && Equals(c);
        public override int GetHashCode() => PackedValue.GetHashCode();
        public override string ToString() => $"{{R:{R} G:{G} B:{B} A:{A}}}";
    }

    /// <summary>
    /// XNA-compatible Rectangle
    /// </summary>
    [Serializable]
    public struct Rectangle : IEquatable<Rectangle>
    {
        public int X, Y, Width, Height;

        public static readonly Rectangle Empty = new Rectangle(0, 0, 0, 0);

        public Rectangle(int x, int y, int width, int height) 
        { 
            X = x; Y = y; Width = width; Height = height; 
        }

        public int Left => X;
        public int Right => X + Width;
        public int Top => Y;
        public int Bottom => Y + Height;
        public Point Location { get => new Point(X, Y); set { X = value.X; Y = value.Y; } }
        public Point Size { get => new Point(Width, Height); set { Width = value.X; Height = value.Y; } }
        public Point Center => new Point(X + Width / 2, Y + Height / 2);
        public bool IsEmpty => Width == 0 && Height == 0 && X == 0 && Y == 0;

        public bool Contains(int x, int y) => X <= x && x < X + Width && Y <= y && y < Y + Height;
        public bool Contains(float x, float y) => X <= x && x < X + Width && Y <= y && y < Y + Height;
        public bool Contains(Point point) => Contains(point.X, point.Y);
        public bool Contains(Vector2 point) => Contains(point.X, point.Y);
        public bool Contains(Rectangle other) => X <= other.X && other.X + other.Width <= X + Width && Y <= other.Y && other.Y + other.Height <= Y + Height;

        public bool Intersects(Rectangle other)
            => other.X < X + Width && X < other.X + other.Width && other.Y < Y + Height && Y < other.Y + other.Height;

        public static Rectangle Intersect(Rectangle a, Rectangle b)
        {
            int x = Math.Max(a.X, b.X);
            int y = Math.Max(a.Y, b.Y);
            int right = Math.Min(a.X + a.Width, b.X + b.Width);
            int bottom = Math.Min(a.Y + a.Height, b.Y + b.Height);
            if (right > x && bottom > y)
                return new Rectangle(x, y, right - x, bottom - y);
            return Empty;
        }

        public static Rectangle Union(Rectangle a, Rectangle b)
        {
            int x = Math.Min(a.X, b.X);
            int y = Math.Min(a.Y, b.Y);
            return new Rectangle(x, y, Math.Max(a.Right, b.Right) - x, Math.Max(a.Bottom, b.Bottom) - y);
        }

        public void Inflate(int horizontal, int vertical)
        {
            X -= horizontal;
            Y -= vertical;
            Width += horizontal * 2;
            Height += vertical * 2;
        }

        public void Offset(int x, int y) { X += x; Y += y; }
        public void Offset(Point point) { X += point.X; Y += point.Y; }

        public static bool operator ==(Rectangle a, Rectangle b) => a.X == b.X && a.Y == b.Y && a.Width == b.Width && a.Height == b.Height;
        public static bool operator !=(Rectangle a, Rectangle b) => !(a == b);

        // Unity conversion
        public static implicit operator UnityEngine.Rect(Rectangle r) => new UnityEngine.Rect(r.X, r.Y, r.Width, r.Height);
        public static implicit operator Rectangle(UnityEngine.Rect r) => new Rectangle((int)r.x, (int)r.y, (int)r.width, (int)r.height);
        public static implicit operator UnityEngine.RectInt(Rectangle r) => new UnityEngine.RectInt(r.X, r.Y, r.Width, r.Height);
        public static implicit operator Rectangle(UnityEngine.RectInt r) => new Rectangle(r.x, r.y, r.width, r.height);

        public bool Equals(Rectangle other) => X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
        public override bool Equals(object obj) => obj is Rectangle r && Equals(r);
        public override int GetHashCode() => X ^ Y ^ Width ^ Height;
        public override string ToString() => $"{{X:{X} Y:{Y} Width:{Width} Height:{Height}}}";
    }

    /// <summary>
    /// XNA-compatible Point
    /// </summary>
    [Serializable]
    public struct Point : IEquatable<Point>
    {
        public int X, Y;

        public static readonly Point Zero = new Point(0, 0);

        public Point(int x, int y) { X = x; Y = y; }

        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
        public static Point operator -(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);
        public static Point operator *(Point a, Point b) => new Point(a.X * b.X, a.Y * b.Y);
        public static Point operator /(Point a, Point b) => new Point(a.X / b.X, a.Y / b.Y);
        public static Point operator -(Point p) => new Point(-p.X, -p.Y);
        public static bool operator ==(Point a, Point b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Point a, Point b) => !(a == b);

        public Vector2 ToVector2() => new Vector2(X, Y);

        // Unity conversion
        public static implicit operator UnityEngine.Vector2Int(Point p) => new UnityEngine.Vector2Int(p.X, p.Y);
        public static implicit operator Point(UnityEngine.Vector2Int p) => new Point(p.x, p.y);

        public bool Equals(Point other) => X == other.X && Y == other.Y;
        public override bool Equals(object obj) => obj is Point p && Equals(p);
        public override int GetHashCode() => X ^ Y;
        public override string ToString() => $"{{X:{X} Y:{Y}}}";
    }

    /// <summary>
    /// XNA-compatible Matrix (4x4)
    /// </summary>
    [Serializable]
    public struct Matrix : IEquatable<Matrix>
    {
        public float M11, M12, M13, M14;
        public float M21, M22, M23, M24;
        public float M31, M32, M33, M34;
        public float M41, M42, M43, M44;

        public static readonly Matrix Identity = new Matrix(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1);

        public Matrix(
            float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44)
        {
            M11 = m11; M12 = m12; M13 = m13; M14 = m14;
            M21 = m21; M22 = m22; M23 = m23; M24 = m24;
            M31 = m31; M32 = m32; M33 = m33; M34 = m34;
            M41 = m41; M42 = m42; M43 = m43; M44 = m44;
        }

        public Vector3 Translation
        {
            get => new Vector3(M41, M42, M43);
            set { M41 = value.X; M42 = value.Y; M43 = value.Z; }
        }

        public static Matrix CreateTranslation(float x, float y, float z)
        {
            Matrix result = Identity;
            result.M41 = x; result.M42 = y; result.M43 = z;
            return result;
        }

        public static Matrix CreateTranslation(Vector3 position)
            => CreateTranslation(position.X, position.Y, position.Z);

        public static Matrix CreateScale(float scale)
        {
            Matrix result = Identity;
            result.M11 = result.M22 = result.M33 = scale;
            return result;
        }

        public static Matrix CreateScale(float x, float y, float z)
        {
            Matrix result = Identity;
            result.M11 = x; result.M22 = y; result.M33 = z;
            return result;
        }

        public static Matrix CreateScale(Vector3 scales)
            => CreateScale(scales.X, scales.Y, scales.Z);

        public static Matrix CreateRotationX(float radians)
        {
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            Matrix result = Identity;
            result.M22 = cos; result.M23 = sin;
            result.M32 = -sin; result.M33 = cos;
            return result;
        }

        public static Matrix CreateRotationY(float radians)
        {
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            Matrix result = Identity;
            result.M11 = cos; result.M13 = -sin;
            result.M31 = sin; result.M33 = cos;
            return result;
        }

        public static Matrix CreateRotationZ(float radians)
        {
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            Matrix result = Identity;
            result.M11 = cos; result.M12 = sin;
            result.M21 = -sin; result.M22 = cos;
            return result;
        }

        public static Matrix CreateLookAt(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
        {
            Vector3 zaxis = Vector3.Normalize(cameraPosition - cameraTarget);
            Vector3 xaxis = Vector3.Normalize(Vector3.Cross(cameraUpVector, zaxis));
            Vector3 yaxis = Vector3.Cross(zaxis, xaxis);

            return new Matrix(
                xaxis.X, yaxis.X, zaxis.X, 0,
                xaxis.Y, yaxis.Y, zaxis.Y, 0,
                xaxis.Z, yaxis.Z, zaxis.Z, 0,
                -Vector3.Dot(xaxis, cameraPosition), -Vector3.Dot(yaxis, cameraPosition), -Vector3.Dot(zaxis, cameraPosition), 1);
        }

        public static Matrix CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane)
        {
            return new Matrix(
                2f / width, 0, 0, 0,
                0, 2f / height, 0, 0,
                0, 0, 1f / (zNearPlane - zFarPlane), 0,
                0, 0, zNearPlane / (zNearPlane - zFarPlane), 1);
        }

        public static Matrix CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane)
        {
            return new Matrix(
                2f / (right - left), 0, 0, 0,
                0, 2f / (top - bottom), 0, 0,
                0, 0, 1f / (zNearPlane - zFarPlane), 0,
                (left + right) / (left - right), (top + bottom) / (bottom - top), zNearPlane / (zNearPlane - zFarPlane), 1);
        }

        public static Matrix CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
        {
            float yScale = 1f / (float)Math.Tan(fieldOfView * 0.5f);
            float xScale = yScale / aspectRatio;
            return new Matrix(
                xScale, 0, 0, 0,
                0, yScale, 0, 0,
                0, 0, farPlaneDistance / (nearPlaneDistance - farPlaneDistance), -1,
                0, 0, nearPlaneDistance * farPlaneDistance / (nearPlaneDistance - farPlaneDistance), 0);
        }

        public static Matrix Invert(Matrix matrix)
        {
            // Simplified 4x4 matrix inversion
            float det = matrix.Determinant();
            if (Math.Abs(det) < 1e-10f) return Identity;
            
            // Use cofactor expansion (simplified version)
            Matrix result = new Matrix();
            float invDet = 1f / det;
            
            result.M11 = (matrix.M22 * matrix.M33 - matrix.M23 * matrix.M32) * invDet;
            result.M12 = -(matrix.M12 * matrix.M33 - matrix.M13 * matrix.M32) * invDet;
            result.M13 = (matrix.M12 * matrix.M23 - matrix.M13 * matrix.M22) * invDet;
            result.M14 = 0;
            
            result.M21 = -(matrix.M21 * matrix.M33 - matrix.M23 * matrix.M31) * invDet;
            result.M22 = (matrix.M11 * matrix.M33 - matrix.M13 * matrix.M31) * invDet;
            result.M23 = -(matrix.M11 * matrix.M23 - matrix.M13 * matrix.M21) * invDet;
            result.M24 = 0;
            
            result.M31 = (matrix.M21 * matrix.M32 - matrix.M22 * matrix.M31) * invDet;
            result.M32 = -(matrix.M11 * matrix.M32 - matrix.M12 * matrix.M31) * invDet;
            result.M33 = (matrix.M11 * matrix.M22 - matrix.M12 * matrix.M21) * invDet;
            result.M34 = 0;
            
            result.M41 = -(matrix.M41 * result.M11 + matrix.M42 * result.M21 + matrix.M43 * result.M31);
            result.M42 = -(matrix.M41 * result.M12 + matrix.M42 * result.M22 + matrix.M43 * result.M32);
            result.M43 = -(matrix.M41 * result.M13 + matrix.M42 * result.M23 + matrix.M43 * result.M33);
            result.M44 = 1;
            
            return result;
        }

        public float Determinant()
        {
            return M11 * (M22 * M33 - M23 * M32) -
                   M12 * (M21 * M33 - M23 * M31) +
                   M13 * (M21 * M32 - M22 * M31);
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            return new Matrix(
                a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41,
                a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42,
                a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43,
                a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44,

                a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41,
                a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42,
                a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43,
                a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44,

                a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41,
                a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42,
                a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43,
                a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44,

                a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41,
                a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42,
                a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43,
                a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44);
        }

        public static bool operator ==(Matrix a, Matrix b)
        {
            return a.M11 == b.M11 && a.M12 == b.M12 && a.M13 == b.M13 && a.M14 == b.M14 &&
                   a.M21 == b.M21 && a.M22 == b.M22 && a.M23 == b.M23 && a.M24 == b.M24 &&
                   a.M31 == b.M31 && a.M32 == b.M32 && a.M33 == b.M33 && a.M34 == b.M34 &&
                   a.M41 == b.M41 && a.M42 == b.M42 && a.M43 == b.M43 && a.M44 == b.M44;
        }

        public static bool operator !=(Matrix a, Matrix b) => !(a == b);

        // Unity conversion
        public static implicit operator UnityEngine.Matrix4x4(Matrix m) =>
            new UnityEngine.Matrix4x4(
                new UnityEngine.Vector4(m.M11, m.M21, m.M31, m.M41),
                new UnityEngine.Vector4(m.M12, m.M22, m.M32, m.M42),
                new UnityEngine.Vector4(m.M13, m.M23, m.M33, m.M43),
                new UnityEngine.Vector4(m.M14, m.M24, m.M34, m.M44));

        public static implicit operator Matrix(UnityEngine.Matrix4x4 m) =>
            new Matrix(
                m.m00, m.m10, m.m20, m.m30,
                m.m01, m.m11, m.m21, m.m31,
                m.m02, m.m12, m.m22, m.m32,
                m.m03, m.m13, m.m23, m.m33);

        public bool Equals(Matrix other) => this == other;
        public override bool Equals(object obj) => obj is Matrix m && Equals(m);
        public override int GetHashCode() => M11.GetHashCode() ^ M22.GetHashCode() ^ M33.GetHashCode() ^ M44.GetHashCode();
    }

    /// <summary>
    /// XNA-compatible Quaternion
    /// </summary>
    [Serializable]
    public struct Quaternion : IEquatable<Quaternion>
    {
        public float X, Y, Z, W;

        public static readonly Quaternion Identity = new Quaternion(0, 0, 0, 1);

        public Quaternion(float x, float y, float z, float w) { X = x; Y = y; Z = z; W = w; }
        public Quaternion(Vector3 vectorPart, float scalarPart) { X = vectorPart.X; Y = vectorPart.Y; Z = vectorPart.Z; W = scalarPart; }

        public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
        {
            float halfAngle = angle * 0.5f;
            float sin = (float)Math.Sin(halfAngle);
            float cos = (float)Math.Cos(halfAngle);
            return new Quaternion(axis.X * sin, axis.Y * sin, axis.Z * sin, cos);
        }

        public static Quaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll)
        {
            float cy = (float)Math.Cos(yaw * 0.5f);
            float sy = (float)Math.Sin(yaw * 0.5f);
            float cp = (float)Math.Cos(pitch * 0.5f);
            float sp = (float)Math.Sin(pitch * 0.5f);
            float cr = (float)Math.Cos(roll * 0.5f);
            float sr = (float)Math.Sin(roll * 0.5f);

            return new Quaternion(
                cy * sp * cr + sy * cp * sr,
                sy * cp * cr - cy * sp * sr,
                cy * cp * sr - sy * sp * cr,
                cy * cp * cr + sy * sp * sr);
        }

        public static Quaternion Lerp(Quaternion q1, Quaternion q2, float t)
        {
            float dot = q1.X * q2.X + q1.Y * q2.Y + q1.Z * q2.Z + q1.W * q2.W;
            float t2 = dot < 0 ? -t : t;
            return Normalize(new Quaternion(
                q1.X + t2 * (q2.X - q1.X),
                q1.Y + t2 * (q2.Y - q1.Y),
                q1.Z + t2 * (q2.Z - q1.Z),
                q1.W + t2 * (q2.W - q1.W)));
        }

        public static Quaternion Normalize(Quaternion q)
        {
            float length = (float)Math.Sqrt(q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W);
            if (length > 0)
                return new Quaternion(q.X / length, q.Y / length, q.Z / length, q.W / length);
            return Identity;
        }

        public static Quaternion operator *(Quaternion a, Quaternion b)
        {
            return new Quaternion(
                a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
                a.W * b.Y - a.X * b.Z + a.Y * b.W + a.Z * b.X,
                a.W * b.Z + a.X * b.Y - a.Y * b.X + a.Z * b.W,
                a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z);
        }

        public static bool operator ==(Quaternion a, Quaternion b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;
        public static bool operator !=(Quaternion a, Quaternion b) => !(a == b);

        // Unity conversion
        public static implicit operator UnityEngine.Quaternion(Quaternion q) => new UnityEngine.Quaternion(q.X, q.Y, q.Z, q.W);
        public static implicit operator Quaternion(UnityEngine.Quaternion q) => new Quaternion(q.x, q.y, q.z, q.w);

        public bool Equals(Quaternion other) => X == other.X && Y == other.Y && Z == other.Z && W == other.W;
        public override bool Equals(object obj) => obj is Quaternion q && Equals(q);
        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();
        public override string ToString() => $"{{X:{X} Y:{Y} Z:{Z} W:{W}}}";
    }

    /// <summary>
    /// XNA-compatible MathHelper
    /// </summary>
    public static class MathHelper
    {
        public const float E = 2.71828175f;
        public const float Log10E = 0.4342945f;
        public const float Log2E = 1.442695f;
        public const float Pi = 3.14159274f;
        public const float PiOver2 = 1.57079637f;
        public const float PiOver4 = 0.7853982f;
        public const float TwoPi = 6.28318548f;

        public static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static float Lerp(float a, float b, float t) => a + (b - a) * t;
        
        public static float SmoothStep(float a, float b, float t)
        {
            t = Clamp(t, 0f, 1f);
            t = t * t * (3f - 2f * t);
            return a + (b - a) * t;
        }

        public static float ToRadians(float degrees) => degrees * (Pi / 180f);
        public static float ToDegrees(float radians) => radians * (180f / Pi);

        public static float WrapAngle(float angle)
        {
            angle = (float)Math.IEEERemainder(angle, TwoPi);
            if (angle <= -Pi)
                angle += TwoPi;
            else if (angle > Pi)
                angle -= TwoPi;
            return angle;
        }

        public static float Distance(float a, float b) => Math.Abs(a - b);

        public static float Min(float a, float b) => a < b ? a : b;
        public static float Max(float a, float b) => a > b ? a : b;

        public static int Min(int a, int b) => a < b ? a : b;
        public static int Max(int a, int b) => a > b ? a : b;

        public static float Barycentric(float value1, float value2, float value3, float amount1, float amount2)
            => value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;

        public static float CatmullRom(float value1, float value2, float value3, float value4, float amount)
        {
            float amountSquared = amount * amount;
            float amountCubed = amountSquared * amount;
            return 0.5f * (2f * value2 + (value3 - value1) * amount +
                (2f * value1 - 5f * value2 + 4f * value3 - value4) * amountSquared +
                (3f * value2 - value1 - 3f * value3 + value4) * amountCubed);
        }

        public static float Hermite(float value1, float tangent1, float value2, float tangent2, float amount)
        {
            float amountSquared = amount * amount;
            float amountCubed = amountSquared * amount;
            float a = 2f * amountCubed - 3f * amountSquared + 1f;
            float b = amountCubed - 2f * amountSquared + amount;
            float c = amountCubed - amountSquared;
            float d = -2f * amountCubed + 3f * amountSquared;
            return value1 * a + tangent1 * b + tangent2 * c + value2 * d;
        }
    }

    #endregion

    #region Game Time

    /// <summary>
    /// XNA-compatible GameTime
    /// </summary>
    public class GameTime
    {
        public TimeSpan TotalGameTime { get; set; }
        public TimeSpan ElapsedGameTime { get; set; }
        public bool IsRunningSlowly { get; set; }

        public GameTime() { }
        public GameTime(TimeSpan totalGameTime, TimeSpan elapsedGameTime)
        {
            TotalGameTime = totalGameTime;
            ElapsedGameTime = elapsedGameTime;
        }
        public GameTime(TimeSpan totalGameTime, TimeSpan elapsedGameTime, bool isRunningSlowly)
        {
            TotalGameTime = totalGameTime;
            ElapsedGameTime = elapsedGameTime;
            IsRunningSlowly = isRunningSlowly;
        }
    }

    #endregion
}

namespace Microsoft.Xna.Framework.Graphics
{
    #region Graphics Stubs (Unity replaces these)

    /// <summary>
    /// Stub for XNA SpriteEffects - Unity uses SpriteRenderer flipX/flipY
    /// </summary>
    [Flags]
    public enum SpriteEffects
    {
        None = 0,
        FlipHorizontally = 1,
        FlipVertically = 2
    }

    /// <summary>
    /// Stub for XNA SpriteSortMode
    /// </summary>
    public enum SpriteSortMode
    {
        Deferred = 0,
        Immediate = 1,
        Texture = 2,
        BackToFront = 3,
        FrontToBack = 4
    }

    /// <summary>
    /// Stub for XNA Blend
    /// </summary>
    public enum Blend
    {
        One = 0,
        Zero = 1,
        SourceColor = 2,
        InverseSourceColor = 3,
        SourceAlpha = 4,
        InverseSourceAlpha = 5,
        DestinationColor = 6,
        InverseDestinationColor = 7,
        DestinationAlpha = 8,
        InverseDestinationAlpha = 9,
        BlendFactor = 10,
        InverseBlendFactor = 11,
        SourceAlphaSaturation = 12
    }

    /// <summary>
    /// Stub for XNA BlendFunction
    /// </summary>
    public enum BlendFunction
    {
        Add = 0,
        Subtract = 1,
        ReverseSubtract = 2,
        Min = 3,
        Max = 4
    }

    /// <summary>
    /// Stub for XNA BlendState - Unity uses Material blend modes
    /// </summary>
    public class BlendState
    {
        public static readonly BlendState AlphaBlend = new BlendState();
        public static readonly BlendState Additive = new BlendState();
        public static readonly BlendState NonPremultiplied = new BlendState();
        public static readonly BlendState Opaque = new BlendState();

        public Blend ColorSourceBlend { get; set; }
        public Blend ColorDestinationBlend { get; set; }
        public BlendFunction ColorBlendFunction { get; set; }
        public Blend AlphaSourceBlend { get; set; }
        public Blend AlphaDestinationBlend { get; set; }
        public BlendFunction AlphaBlendFunction { get; set; }
    }

    /// <summary>
    /// Stub for XNA SamplerState - Unity uses Texture filterMode/wrapMode
    /// </summary>
    public class SamplerState
    {
        public static readonly SamplerState PointClamp = new SamplerState();
        public static readonly SamplerState PointWrap = new SamplerState();
        public static readonly SamplerState LinearClamp = new SamplerState();
        public static readonly SamplerState LinearWrap = new SamplerState();
        public static readonly SamplerState AnisotropicClamp = new SamplerState();
        public static readonly SamplerState AnisotropicWrap = new SamplerState();
    }

    /// <summary>
    /// Stub for XNA DepthStencilState
    /// </summary>
    public class DepthStencilState
    {
        public static readonly DepthStencilState None = new DepthStencilState();
        public static readonly DepthStencilState Default = new DepthStencilState();
        public static readonly DepthStencilState DepthRead = new DepthStencilState();
    }

    /// <summary>
    /// Stub for XNA RasterizerState
    /// </summary>
    public class RasterizerState
    {
        public static readonly RasterizerState CullNone = new RasterizerState();
        public static readonly RasterizerState CullClockwise = new RasterizerState();
        public static readonly RasterizerState CullCounterClockwise = new RasterizerState();
    }

    /// <summary>
    /// Stub for XNA Effect (shaders) - Unity uses Materials/Shaders
    /// </summary>
    public class Effect : IDisposable
    {
        public EffectTechniqueCollection Techniques { get; } = new EffectTechniqueCollection();
        public EffectTechnique CurrentTechnique { get; set; }
        public EffectParameterCollection Parameters { get; } = new EffectParameterCollection();

        public virtual void Dispose() { }
    }

    public class EffectTechniqueCollection
    {
        private readonly System.Collections.Generic.Dictionary<string, EffectTechnique> _techniques = new();
        public EffectTechnique this[string name]
        {
            get
            {
                if (!_techniques.ContainsKey(name))
                    _techniques[name] = new EffectTechnique();
                return _techniques[name];
            }
        }
    }

    public class EffectTechnique { }

    public class EffectParameterCollection
    {
        private readonly System.Collections.Generic.Dictionary<string, EffectParameter> _parameters = new();
        public EffectParameter this[string name]
        {
            get
            {
                if (!_parameters.ContainsKey(name))
                    _parameters[name] = new EffectParameter();
                return _parameters[name];
            }
        }
    }

    public class EffectParameter
    {
        public void SetValue(float value) { }
        public void SetValue(Microsoft.Xna.Framework.Vector2 value) { }
        public void SetValue(Microsoft.Xna.Framework.Vector3 value) { }
        public void SetValue(Microsoft.Xna.Framework.Vector4 value) { }
        public void SetValue(Microsoft.Xna.Framework.Matrix value) { }
        public void SetValue(Texture2D value) { }
        public void SetValue(float[] value) { }
    }

    /// <summary>
    /// Stub for XNA Texture2D - Unity uses UnityEngine.Texture2D
    /// </summary>
    public class Texture2D : Texture
    {
        public UnityEngine.Texture2D UnityTexture { get; private set; }

        public Texture2D(int width, int height)
        {
            UnityTexture = new UnityEngine.Texture2D(width, height, TextureFormat.RGBA32, false);
            UnityTexture.filterMode = FilterMode.Point;
        }

        public int Width => UnityTexture?.width ?? 0;
        public int Height => UnityTexture?.height ?? 0;
        public Microsoft.Xna.Framework.Rectangle Bounds => new Microsoft.Xna.Framework.Rectangle(0, 0, Width, Height);

        public void SetData<T>(T[] data) where T : struct
        {
            if (data is Microsoft.Xna.Framework.Color[] colors)
            {
                Color32[] pixels = new Color32[colors.Length];
                for (int i = 0; i < colors.Length; i++)
                    pixels[i] = new Color32(colors[i].R, colors[i].G, colors[i].B, colors[i].A);
                UnityTexture.SetPixels32(pixels);
                UnityTexture.Apply();
            }
        }

        public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct
        {
            if (data is byte[] bytes)
            {
                UnityTexture.LoadRawTextureData(bytes);
                UnityTexture.Apply();
            }
        }

        public void GetData<T>(T[] data) where T : struct
        {
            if (data is Microsoft.Xna.Framework.Color[] colors)
            {
                var pixels = UnityTexture.GetPixels32();
                for (int i = 0; i < Math.Min(colors.Length, pixels.Length); i++)
                    colors[i] = new Microsoft.Xna.Framework.Color(pixels[i].r, pixels[i].g, pixels[i].b, pixels[i].a);
            }
        }

        public void GetData<T>(T[] data, int startIndex, int elementCount) where T : struct
        {
            GetData(data);
        }

        public override void Dispose()
        {
            if (UnityTexture != null)
            {
                UnityEngine.Object.Destroy(UnityTexture);
                UnityTexture = null;
            }
        }

        // Implicit conversion to Unity Texture2D
        public static implicit operator UnityEngine.Texture2D(Texture2D tex) => tex?.UnityTexture;
    }

    /// <summary>
    /// Base Texture class stub
    /// </summary>
    public abstract class Texture : IDisposable
    {
        public bool IsDisposed { get; protected set; }
        public virtual void Dispose() { IsDisposed = true; }
    }

    /// <summary>
    /// Stub for XNA RenderTarget2D - Unity uses RenderTexture
    /// </summary>
    public class RenderTarget2D : Texture2D
    {
        public RenderTexture RenderTexture { get; private set; }

        public RenderTarget2D(int width, int height, int mipMap = 0, bool depth = false)
            : base(width, height)
        {
            RenderTexture = new RenderTexture(width, height, depth ? 24 : 0, RenderTextureFormat.ARGB32);
            RenderTexture.Create();
        }

        public new int Width => RenderTexture?.width ?? 0;
        public new int Height => RenderTexture?.height ?? 0;

        public override void Dispose()
        {
            base.Dispose();
            if (RenderTexture != null)
            {
                RenderTexture.Release();
                UnityEngine.Object.Destroy(RenderTexture);
                RenderTexture = null;
            }
        }

        public static implicit operator RenderTexture(RenderTarget2D rt) => rt?.RenderTexture;
    }

    /// <summary>
    /// Stub for XNA SpriteBatch - Unity rendering is done differently
    /// This provides a compatibility layer but rendering should be migrated to Unity systems
    /// </summary>
    public class SpriteBatch : IDisposable
    {
        public SpriteBatch() { }

        public void Begin(
            SpriteSortMode sortMode = SpriteSortMode.Deferred,
            BlendState blendState = null,
            SamplerState samplerState = null,
            DepthStencilState depthStencilState = null,
            RasterizerState rasterizerState = null,
            Effect effect = null,
            Microsoft.Xna.Framework.Matrix? transformMatrix = null)
        {
            // In Unity, rendering is handled by the render pipeline
            // This is a stub for compatibility
        }

        public void End()
        {
            // Stub for compatibility
        }

        public void Draw(
            Texture2D texture,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Color color)
        {
            // Unity: Use Graphics.DrawTexture or SpriteRenderer
        }

        public void Draw(
            Texture2D texture,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Rectangle? sourceRectangle,
            Microsoft.Xna.Framework.Color color)
        {
            // Unity: Use Graphics.DrawTexture or SpriteRenderer
        }

        public void Draw(
            Texture2D texture,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Rectangle? sourceRectangle,
            Microsoft.Xna.Framework.Color color,
            float rotation,
            Microsoft.Xna.Framework.Vector2 origin,
            float scale,
            SpriteEffects effects,
            float layerDepth)
        {
            // Unity: Use Graphics.DrawTexture or SpriteRenderer
        }

        public void Draw(
            Texture2D texture,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Rectangle? sourceRectangle,
            Microsoft.Xna.Framework.Color color,
            float rotation,
            Microsoft.Xna.Framework.Vector2 origin,
            Microsoft.Xna.Framework.Vector2 scale,
            SpriteEffects effects,
            float layerDepth)
        {
            // Unity: Use Graphics.DrawTexture or SpriteRenderer
        }

        public void Draw(
            Texture2D texture,
            Microsoft.Xna.Framework.Rectangle destinationRectangle,
            Microsoft.Xna.Framework.Color color)
        {
            // Unity: Use Graphics.DrawTexture or SpriteRenderer
        }

        public void Draw(
            Texture2D texture,
            Microsoft.Xna.Framework.Rectangle destinationRectangle,
            Microsoft.Xna.Framework.Rectangle? sourceRectangle,
            Microsoft.Xna.Framework.Color color)
        {
            // Unity: Use Graphics.DrawTexture or SpriteRenderer
        }

        public void DrawString(
            SpriteFont font,
            string text,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Color color)
        {
            // Unity: Use TextMeshPro or UI Text
        }

        public void DrawString(
            SpriteFont font,
            string text,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Color color,
            float rotation,
            Microsoft.Xna.Framework.Vector2 origin,
            float scale,
            SpriteEffects effects,
            float layerDepth)
        {
            // Unity: Use TextMeshPro or UI Text
        }

        public void DrawString(
            SpriteFont font,
            string text,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Color color,
            float rotation,
            Microsoft.Xna.Framework.Vector2 origin,
            Microsoft.Xna.Framework.Vector2 scale,
            SpriteEffects effects,
            float layerDepth)
        {
            // Unity: Use TextMeshPro or UI Text
        }

        public void Dispose() { }
    }

    /// <summary>
    /// Stub for XNA SpriteFont - Unity uses TMP_FontAsset or Font
    /// </summary>
    public class SpriteFont
    {
        public Microsoft.Xna.Framework.Vector2 MeasureString(string text)
        {
            // Approximate measurement - should be replaced with actual font metrics
            return new Microsoft.Xna.Framework.Vector2(text.Length * 8, 16);
        }

        public float LineSpacing { get; set; } = 16f;
    }

    /// <summary>
    /// Stub for XNA GraphicsDevice
    /// </summary>
    public class GraphicsDevice : IDisposable
    {
        public bool IsDisposed { get; private set; }
        public Viewport Viewport { get; set; }

        public void SetRenderTarget(RenderTarget2D renderTarget)
        {
            // Unity: Use RenderTexture.active or Camera.targetTexture
            if (renderTarget != null)
                RenderTexture.active = renderTarget.RenderTexture;
            else
                RenderTexture.active = null;
        }

        public void SetRenderTarget(RenderTexture renderTexture)
        {
            RenderTexture.active = renderTexture;
        }

        public void Clear(Microsoft.Xna.Framework.Color color)
        {
            // Unity: Use GL.Clear or Camera.backgroundColor
            GL.Clear(true, true, color);
        }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }

    /// <summary>
    /// Stub for XNA Viewport
    /// </summary>
    public struct Viewport
    {
        public int X, Y, Width, Height;
        public float MinDepth, MaxDepth;

        public Viewport(int x, int y, int width, int height)
        {
            X = x; Y = y; Width = width; Height = height;
            MinDepth = 0; MaxDepth = 1;
        }

        public Viewport(Microsoft.Xna.Framework.Rectangle bounds)
        {
            X = bounds.X; Y = bounds.Y; Width = bounds.Width; Height = bounds.Height;
            MinDepth = 0; MaxDepth = 1;
        }

        public Microsoft.Xna.Framework.Rectangle Bounds => new Microsoft.Xna.Framework.Rectangle(X, Y, Width, Height);

        public float AspectRatio => Width / (float)Height;
    }

    /// <summary>
    /// Stub for GraphicsDeviceManager
    /// </summary>
    public class GraphicsDeviceManager : IDisposable
    {
        public GraphicsDevice GraphicsDevice { get; private set; }
        public bool SynchronizeWithVerticalRetrace { get; set; }
        public bool PreferMultiSampling { get; set; }
        public GraphicsProfile GraphicsProfile { get; set; }
        public SurfaceFormat PreferredBackBufferFormat { get; set; }
        public DepthFormat PreferredDepthStencilFormat { get; set; }
        public int PreferredBackBufferWidth { get; set; }
        public int PreferredBackBufferHeight { get; set; }
        public bool IsFullScreen { get; set; }

        public event EventHandler<EventArgs> DeviceReset;
        public event EventHandler<EventArgs> DeviceCreated;

        public GraphicsDeviceManager(object game)
        {
            GraphicsDevice = new GraphicsDevice();
        }

        public void ApplyChanges() { }
        public void Dispose() { }
    }

    public enum GraphicsProfile { Reach, HiDef }
    public enum SurfaceFormat { Color, Bgr565, Bgra5551, Bgra4444, Dxt1, Dxt3, Dxt5 }
    public enum DepthFormat { None, Depth16, Depth24, Depth24Stencil8 }
    public enum RenderTargetUsage { DiscardContents, PreserveContents, PlatformContents }

    #endregion
}

namespace Microsoft.Xna.Framework.Input
{
    #region Input Stubs (Unity Input System replaces these)

    public enum Keys
    {
        None = 0,
        Back = 8, Tab = 9, Enter = 13, Escape = 27, Space = 32,
        Left = 37, Up = 38, Right = 39, Down = 40,
        D0 = 48, D1, D2, D3, D4, D5, D6, D7, D8, D9,
        A = 65, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
        LeftShift = 160, RightShift, LeftControl, RightControl, LeftAlt, RightAlt,
        F1 = 112, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12
    }

    public enum Buttons
    {
        DPadUp = 1,
        DPadDown = 2,
        DPadLeft = 4,
        DPadRight = 8,
        Start = 16,
        Back = 32,
        LeftStick = 64,
        RightStick = 128,
        LeftShoulder = 256,
        RightShoulder = 512,
        A = 4096,
        B = 8192,
        X = 16384,
        Y = 32768,
        LeftTrigger = 8388608,
        RightTrigger = 4194304
    }

    public enum ButtonState { Released = 0, Pressed = 1 }
    public enum PlayerIndex { One = 0, Two = 1, Three = 2, Four = 3 }

    public struct KeyboardState
    {
        public bool IsKeyDown(Keys key) => UnityEngine.Input.GetKey((KeyCode)key);
        public bool IsKeyUp(Keys key) => !UnityEngine.Input.GetKey((KeyCode)key);
        public Keys[] GetPressedKeys() => Array.Empty<Keys>();
    }

    public struct MouseState
    {
        public int X => (int)UnityEngine.Input.mousePosition.x;
        public int Y => (int)(Screen.height - UnityEngine.Input.mousePosition.y);
        public ButtonState LeftButton => UnityEngine.Input.GetMouseButton(0) ? ButtonState.Pressed : ButtonState.Released;
        public ButtonState RightButton => UnityEngine.Input.GetMouseButton(1) ? ButtonState.Pressed : ButtonState.Released;
        public ButtonState MiddleButton => UnityEngine.Input.GetMouseButton(2) ? ButtonState.Pressed : ButtonState.Released;
        public int ScrollWheelValue => (int)(UnityEngine.Input.mouseScrollDelta.y * 120);
    }

    public struct GamePadState
    {
        public bool IsConnected => false; // Use Unity's new Input System for gamepads
        public GamePadButtons Buttons => new GamePadButtons();
        public GamePadDPad DPad => new GamePadDPad();
        public GamePadThumbSticks ThumbSticks => new GamePadThumbSticks();
        public GamePadTriggers Triggers => new GamePadTriggers();
    }

    public struct GamePadButtons
    {
        public ButtonState A => ButtonState.Released;
        public ButtonState B => ButtonState.Released;
        public ButtonState X => ButtonState.Released;
        public ButtonState Y => ButtonState.Released;
        public ButtonState Start => ButtonState.Released;
        public ButtonState Back => ButtonState.Released;
        public ButtonState LeftShoulder => ButtonState.Released;
        public ButtonState RightShoulder => ButtonState.Released;
        public ButtonState LeftStick => ButtonState.Released;
        public ButtonState RightStick => ButtonState.Released;
    }

    public struct GamePadDPad
    {
        public ButtonState Up => ButtonState.Released;
        public ButtonState Down => ButtonState.Released;
        public ButtonState Left => ButtonState.Released;
        public ButtonState Right => ButtonState.Released;
    }

    public struct GamePadThumbSticks
    {
        public Microsoft.Xna.Framework.Vector2 Left => Microsoft.Xna.Framework.Vector2.Zero;
        public Microsoft.Xna.Framework.Vector2 Right => Microsoft.Xna.Framework.Vector2.Zero;
    }

    public struct GamePadTriggers
    {
        public float Left => 0f;
        public float Right => 0f;
    }

    public static class Keyboard
    {
        public static KeyboardState GetState() => new KeyboardState();
    }

    public static class Mouse
    {
        public static MouseState GetState() => new MouseState();
    }

    public static class GamePad
    {
        public static GamePadState GetState(PlayerIndex index) => new GamePadState();
        public static bool SetVibration(PlayerIndex index, float leftMotor, float rightMotor) => false;
    }

    #endregion
}

namespace Microsoft.Xna.Framework.Content
{
    /// <summary>
    /// Stub for XNA ContentManager - Unity uses Resources/Addressables
    /// </summary>
    public class ContentManager : IDisposable
    {
        public string RootDirectory { get; set; } = "Content";

        public ContentManager(IServiceProvider serviceProvider) { }
        public ContentManager(IServiceProvider serviceProvider, string rootDirectory) { RootDirectory = rootDirectory; }

        public T Load<T>(string assetName)
        {
            // Unity: Use Resources.Load or Addressables
            var resource = Resources.Load(assetName);
            if (resource is T typed)
                return typed;
            return default;
        }

        public void Unload() { }
        public void Dispose() { }
    }
}

namespace Microsoft.Xna.Framework.Audio
{
    // Stub for audio - Unity uses AudioSource/AudioClip or FMOD
    public class SoundEffect : IDisposable
    {
        public SoundEffectInstance CreateInstance() => new SoundEffectInstance();
        public void Play() { }
        public void Play(float volume, float pitch, float pan) { }
        public void Dispose() { }
    }

    public class SoundEffectInstance : IDisposable
    {
        public float Volume { get; set; }
        public float Pitch { get; set; }
        public float Pan { get; set; }
        public bool IsLooped { get; set; }
        public SoundState State => SoundState.Stopped;

        public void Play() { }
        public void Stop() { }
        public void Pause() { }
        public void Resume() { }
        public void Dispose() { }
    }

    public enum SoundState { Playing, Paused, Stopped }
}

namespace Microsoft.Xna.Framework.Media
{
    // Stub for media - Unity uses AudioSource or FMOD
    public static class MediaPlayer
    {
        public static float Volume { get; set; }
        public static bool IsRepeating { get; set; }
        public static MediaState State => MediaState.Stopped;

        public static void Play(Song song) { }
        public static void Stop() { }
        public static void Pause() { }
        public static void Resume() { }
    }

    public enum MediaState { Stopped, Playing, Paused }

    public class Song : IDisposable
    {
        public TimeSpan Duration => TimeSpan.Zero;
        public string Name { get; }
        public void Dispose() { }
    }
}
