using System;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// 4D vector compatible with XNA Framework
    /// </summary>
    public struct Vector4 : IEquatable<Vector4>
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vector4(float value)
        {
            X = Y = Z = W = value;
        }

        public bool Equals(Vector4 other) => X == other.X && Y == other.Y && Z == other.Z && W == other.W;
        public override bool Equals(object obj) => obj is Vector4 other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);
        public override string ToString() => $"{{X:{X} Y:{Y} Z:{Z} W:{W}}}";

        public static implicit operator UnityEngine.Vector4(Vector4 value) => new UnityEngine.Vector4(value.X, value.Y, value.Z, value.W);
        public static implicit operator Vector4(UnityEngine.Vector4 value) => new Vector4(value.x, value.y, value.z, value.w);
    }

    /// <summary>
    /// Matrix for transformations
    /// </summary>
    public struct Matrix
    {
        public float M11, M12, M13, M14;
        public float M21, M22, M23, M24;
        public float M31, M32, M33, M34;
        public float M41, M42, M43, M44;

        public static readonly Matrix Identity = new Matrix
        {
            M11 = 1f, M22 = 1f, M33 = 1f, M44 = 1f
        };

        public static Matrix CreateTranslation(float x, float y, float z)
        {
            Matrix result = Identity;
            result.M41 = x;
            result.M42 = y;
            result.M43 = z;
            return result;
        }

        public static Matrix CreateTranslation(Vector3 position) => CreateTranslation(position.X, position.Y, position.Z);

        public static Matrix CreateScale(float scale)
        {
            Matrix result = Identity;
            result.M11 = scale;
            result.M22 = scale;
            result.M33 = scale;
            return result;
        }

        public static Matrix CreateScale(float x, float y, float z)
        {
            Matrix result = Identity;
            result.M11 = x;
            result.M22 = y;
            result.M33 = z;
            return result;
        }

        public static Matrix CreateScale(Vector3 scale) => CreateScale(scale.X, scale.Y, scale.Z);

        public static Matrix CreateRotationZ(float radians)
        {
            Matrix result = Identity;
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            result.M11 = cos;
            result.M12 = sin;
            result.M21 = -sin;
            result.M22 = cos;
            return result;
        }

        public static Matrix Invert(Matrix matrix)
        {
            UnityEngine.Matrix4x4 unity = matrix;
            unity = unity.inverse;
            return unity;
        }

        public static Matrix CreateLookAt(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
        {
            UnityEngine.Matrix4x4 unity = UnityEngine.Matrix4x4.LookAt(cameraPosition, cameraTarget, cameraUpVector);
            return unity;
        }

        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            Matrix result;
            result.M11 = matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21 + matrix1.M13 * matrix2.M31 + matrix1.M14 * matrix2.M41;
            result.M12 = matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22 + matrix1.M13 * matrix2.M32 + matrix1.M14 * matrix2.M42;
            result.M13 = matrix1.M11 * matrix2.M13 + matrix1.M12 * matrix2.M23 + matrix1.M13 * matrix2.M33 + matrix1.M14 * matrix2.M43;
            result.M14 = matrix1.M11 * matrix2.M14 + matrix1.M12 * matrix2.M24 + matrix1.M13 * matrix2.M34 + matrix1.M14 * matrix2.M44;

            result.M21 = matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21 + matrix1.M23 * matrix2.M31 + matrix1.M24 * matrix2.M41;
            result.M22 = matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22 + matrix1.M23 * matrix2.M32 + matrix1.M24 * matrix2.M42;
            result.M23 = matrix1.M21 * matrix2.M13 + matrix1.M22 * matrix2.M23 + matrix1.M23 * matrix2.M33 + matrix1.M24 * matrix2.M43;
            result.M24 = matrix1.M21 * matrix2.M14 + matrix1.M22 * matrix2.M24 + matrix1.M23 * matrix2.M34 + matrix1.M24 * matrix2.M44;

            result.M31 = matrix1.M31 * matrix2.M11 + matrix1.M32 * matrix2.M21 + matrix1.M33 * matrix2.M31 + matrix1.M34 * matrix2.M41;
            result.M32 = matrix1.M31 * matrix2.M12 + matrix1.M32 * matrix2.M22 + matrix1.M33 * matrix2.M32 + matrix1.M34 * matrix2.M42;
            result.M33 = matrix1.M31 * matrix2.M13 + matrix1.M32 * matrix2.M23 + matrix1.M33 * matrix2.M33 + matrix1.M34 * matrix2.M43;
            result.M34 = matrix1.M31 * matrix2.M14 + matrix1.M32 * matrix2.M24 + matrix1.M33 * matrix2.M34 + matrix1.M34 * matrix2.M44;

            result.M41 = matrix1.M41 * matrix2.M11 + matrix1.M42 * matrix2.M21 + matrix1.M43 * matrix2.M31 + matrix1.M44 * matrix2.M41;
            result.M42 = matrix1.M41 * matrix2.M12 + matrix1.M42 * matrix2.M22 + matrix1.M43 * matrix2.M32 + matrix1.M44 * matrix2.M42;
            result.M43 = matrix1.M41 * matrix2.M13 + matrix1.M42 * matrix2.M23 + matrix1.M43 * matrix2.M33 + matrix1.M44 * matrix2.M43;
            result.M44 = matrix1.M41 * matrix2.M14 + matrix1.M42 * matrix2.M24 + matrix1.M43 * matrix2.M34 + matrix1.M44 * matrix2.M44;

            return result;
        }

        public static implicit operator UnityEngine.Matrix4x4(Matrix value)
        {
            UnityEngine.Matrix4x4 result = new UnityEngine.Matrix4x4();
            result.m00 = value.M11; result.m01 = value.M12; result.m02 = value.M13; result.m03 = value.M14;
            result.m10 = value.M21; result.m11 = value.M22; result.m12 = value.M23; result.m13 = value.M24;
            result.m20 = value.M31; result.m21 = value.M32; result.m22 = value.M33; result.m23 = value.M34;
            result.m30 = value.M41; result.m31 = value.M42; result.m32 = value.M43; result.m33 = value.M44;
            return result;
        }

        public static implicit operator Matrix(UnityEngine.Matrix4x4 value)
        {
            Matrix result = new Matrix();
            result.M11 = value.m00; result.M12 = value.m01; result.M13 = value.m02; result.M14 = value.m03;
            result.M21 = value.m10; result.M22 = value.m11; result.M23 = value.m12; result.M24 = value.m13;
            result.M31 = value.m20; result.M32 = value.m21; result.M33 = value.m22; result.M34 = value.m23;
            result.M41 = value.m30; result.M42 = value.m31; result.M43 = value.m32; result.M44 = value.m33;
            return result;
        }
    }
}
