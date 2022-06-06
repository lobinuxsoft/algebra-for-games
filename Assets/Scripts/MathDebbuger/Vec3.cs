using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace CustomMath
{
    public struct Vec3 : IEquatable<Vec3>
    {
        #region Variables
        public float x;
        public float y;
        public float z;

        public float sqrMagnitude { get { return x * x + y * y + z * z; } }
        
        public Vec3 normalized { get { return Normalize(this); } }
        
        public float magnitude { get { return Magnitude(this); } }
        #endregion

        #region constants
        public const float epsilon = 1e-05f;
        #endregion

        #region Default Values
        public static Vec3 Zero { get { return new Vec3(0.0f, 0.0f, 0.0f); } }
        public static Vec3 One { get { return new Vec3(1.0f, 1.0f, 1.0f); } }
        public static Vec3 Forward { get { return new Vec3(0.0f, 0.0f, 1.0f); } }
        public static Vec3 Back { get { return new Vec3(0.0f, 0.0f, -1.0f); } }
        public static Vec3 Right { get { return new Vec3(1.0f, 0.0f, 0.0f); } }
        public static Vec3 Left { get { return new Vec3(-1.0f, 0.0f, 0.0f); } }
        public static Vec3 Up { get { return new Vec3(0.0f, 1.0f, 0.0f); } }
        public static Vec3 Down { get { return new Vec3(0.0f, -1.0f, 0.0f); } }
        public static Vec3 PositiveInfinity { get { return new Vec3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity); } }
        public static Vec3 NegativeInfinity { get { return new Vec3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity); } }
        #endregion                                                                                                                                                                               

        #region Constructors
        public Vec3(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0.0f;
        }

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vec3(Vec3 vec3)
        {
            this.x = vec3.x;
            this.y = vec3.y;
            this.z = vec3.z;
        }

        public Vec3(Vector3 vec3)
        {
            this.x = vec3.x;
            this.y = vec3.y;
            this.z = vec3.z;
        }

        public Vec3(Vector2 vector2)
        {
            this.x = vector2.x;
            this.y = vector2.y;
            this.z = 0.0f;
        }
        #endregion

        #region Operators
        public static bool operator ==(Vec3 left, Vec3 right)
        {
            float diff_x = left.x - right.x;
            float diff_y = left.y - right.y;
            float diff_z = left.z - right.z;
            float sqrmag = diff_x * diff_x + diff_y * diff_y + diff_z * diff_z;
            return sqrmag < epsilon * epsilon;
        }

        public static bool operator !=(Vec3 left, Vec3 right)
        {
            return !(left == right);
        }

        public static Vec3 operator +(Vec3 left, Vec3 right)
        {
            return new Vec3(left.x + right.x, left.y + right.y, left.z + right.z);
        }

        public static Vec3 operator -(Vec3 left, Vec3 right)
        {
            return new Vec3(left.x - right.x, left.y - right.y, left.z - right.z);
        }

        public static Vec3 operator -(Vec3 vec3)
        {
            return new Vec3(0f - vec3.x, 0f - vec3.y, 0f - vec3.z);
        }

        public static Vec3 operator *(Vec3 vec3, float scalar)
        {
            return new Vec3(vec3.x * scalar, vec3.y * scalar, vec3.z * scalar);
        }

        public static Vec3 operator *(float scalar, Vec3 vec3)
        {
            return vec3 * scalar;
        }

        public static Vec3 operator /(Vec3 vec3, float scalar)
        {
            return new Vec3(vec3.x / scalar, vec3.y / scalar, vec3.z / scalar);
        }

        public static implicit operator Vector3(Vec3 vec3)
        {
            return new Vector3(vec3.x, vec3.y, vec3.z);
        }

        public static implicit operator Vec3(Vector3 vector3)
        {
            return new Vec3(vector3.x, vector3.y, vector3.z);
        }

        public static implicit operator Vector2(Vec3 vec3)
        {
            return new Vector2(vec3.x, vec3.y);
        }
        
        #endregion

        #region Functions
        public override string ToString()
        {
            return $"({x},{y},{z})";
        }

        public static float Angle(Vec3 from, Vec3 to)
        {
            float sqrt = (float)Math.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
            if (sqrt < epsilon)
            {
                return 0f;
            }

            float clamp = Mathf.Clamp(Dot(from, to) / sqrt, -1f, 1f);
            return (float)Math.Acos(clamp) * Mathf.Deg2Rad * 180; // conversion a radianes
        }

        public static Vec3 ClampMagnitude(Vec3 vec3, float maxLength)
        {
            float sqrMagnitude = vec3.sqrMagnitude;
            if (sqrMagnitude > maxLength * maxLength)
            {
                float sqrt = (float)Math.Sqrt(sqrMagnitude);
                float newX = vec3.x / sqrt;
                float newY = vec3.y / sqrt;
                float newZ = vec3.z / sqrt;
                return new Vec3(newX * maxLength, newY * maxLength, newZ * maxLength);
            }

            return vec3;
        }

        public static float Magnitude(Vec3 vec3)
        {
            return (float)Math.Sqrt(vec3.x * vec3.x + vec3.y * vec3.y + vec3.z * vec3.z);
        }

        public static Vec3 Cross(Vec3 vectorA, Vec3 vectorB)
        {
            return new Vec3(vectorA.y * vectorB.z - vectorA.z * vectorB.y, vectorA.z * vectorB.x - vectorA.x * vectorB.z, vectorA.x * vectorB.y - vectorA.y * vectorB.x);
        }

        public static float Distance(Vec3 vectorA, Vec3 vectorB)
        {
            float newX = vectorA.x - vectorB.x;
            float newY = vectorA.y - vectorB.y;
            float newZ = vectorA.z - vectorB.z;
            return (float)Math.Sqrt(newX * newX + newY * newY + newZ * newZ);
        }

        public static float Dot(Vec3 vectorA, Vec3 vectorB)
        {
            return vectorA.x * vectorB.x + vectorA.y * vectorB.y + vectorA.z * vectorB.z;
        }

        public static Vec3 Lerp(Vec3 vectorA, Vec3 vectorB, float time)
        {
            time = Mathf.Clamp01(time);
            return vectorA + (vectorB - vectorA) * time;
        }

        public static Vec3 LerpUnclamped(Vec3 vectorA, Vec3 vectorB, float time)
        {
            return vectorA + (vectorB - vectorA) * time;
        }

        public static Vec3 Max(Vec3 vectorA, Vec3 vectorB)
        {
            return new Vec3(Mathf.Max(vectorA.x, vectorB.x), Mathf.Max(vectorA.y, vectorB.y), Mathf.Max(vectorA.z, vectorB.z));
        }

        public static Vec3 Min(Vec3 vectorA, Vec3 vectorB)
        {
            return new Vec3(Mathf.Min(vectorA.x, vectorB.x), Mathf.Min(vectorA.y, vectorB.y), Mathf.Min(vectorA.z, vectorB.z));
        }

        public static float SqrMagnitude(Vec3 vec3)
        {
            return vec3.x * vec3.x + vec3.y * vec3.y + vec3.z * vec3.z;
        }

        // https://www.youtube.com/watch?v=VTV1GTrrtBQ&list=PLW3Zl3wyJwWMcLmUYXMIIxCiLKGOWLETh&index=9
        public static Vec3 Project(Vec3 vec3, Vec3 onNormal) 
        {
            float dot = Dot(onNormal, onNormal); // Esto se hace para corroborar que no se divida por cero

            if (dot < Mathf.Epsilon)
            {
                return Zero;
            }

            float dot2 = Dot(vec3, onNormal); // Esto es para saber si las direcciones son las mismas.
            return new Vec3(onNormal.x * dot2 / dot, onNormal.y * dot2 / dot, onNormal.z * dot2 / dot); // Aca se calcula cuanto se proyecta del vector con el otro.
        }

        // https://www.youtube.com/watch?v=naaeH1qbjdQ
        // https://www.youtube.com/watch?v=NOBhfEHOYZs este se entiende mejor
        public static Vec3 Reflect(Vec3 inDirection, Vec3 inNormal) 
        {
            // Devuelve la reflexion del vector
            return -2 * Dot(inNormal, inDirection) * inNormal + inDirection;

            // Devuelve la proyeccion del vector de incidencia sobre el normal pero con el doble se su magnitud.
            //return -2 * Dot(inNormal, inDirection) * inNormal;

            // Me devuelve la proyeccion en el vector normal del vector de incidencia.
            //return -Dot(inNormal, inDirection) * inNormal;

            // Me devuelve un vector (con el sentido y magnitud), teniendo en cuenta la normal y el vector de incidencia
            //return Dot(inNormal, inDirection) * inNormal;
        }

        public static Vec3 Normalize(Vec3 vec3)
        {
            float num = Magnitude(vec3);
            if (num > epsilon)
            {
                return vec3 / num;
            }

            return Zero;
        }


        public void Set(float newX, float newY, float newZ)
        {
            x = newX;
            y = newY;
            z = newZ;
        }

        public static Vec3 Scale(Vec3 a, Vec3 b) => new Vec3(a.x * b.x, a.y * b.y, a.z * b.z);

        public void Scale(Vec3 vec3)
        {
            x *= vec3.x;
            y *= vec3.y;
            z *= vec3.z;
        }

        public void Normalize()
        {
            float num = Magnitude(this);
            if (num > epsilon)
            {
                this /= num;
            }
            else
            {
                this = Zero;
            }
        }

        #endregion

        #region Internals
        public override bool Equals(object other)
        {
            if (!(other is Vec3)) return false;
            return Equals((Vec3)other);
        }

        public bool Equals(Vec3 other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
        }
        #endregion
    }
}