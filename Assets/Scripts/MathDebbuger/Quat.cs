using System;
using UnityEngine;

namespace CustomMath
{
    public struct Quat : IEquatable<Quat>
    {
        #region Variables

        public float x;
        public float y;
        public float z;
        public float w;

        #endregion

        #region Constants

        public const float kEpsilon = 1E-06F;

        #endregion

        #region Default Values

        public static Quat Identity => new Quat(0, 0, 0, 1);

        #endregion

        #region Constructors

        public Quat(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        #endregion

        #region Operators

        // TODO ver tema presicion
        public static bool operator ==(Quat lhs, Quat rhs) => lhs.Equals(rhs);

        public static bool operator !=(Quat lhs, Quat rhs) => !lhs.Equals(rhs);

        public static Quat operator *(Quat lhs, Quat rhs)
        {
            float w = lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z; // Real
            float x = lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y; // imaginario I
            float y = lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z; // imaginario J
            float z = lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x; // imaginario K

            return new Quat(x, y, z, w); // Choclo final xD
        }

        public static Vec3 operator *(Quat rotation, Vec3 point)
        {
            float rotX = rotation.x * 2f;
            float rotY = rotation.y * 2f;
            float rotZ = rotation.z * 2f;

            float rotX2 = rotation.x * rotX;
            float rotY2 = rotation.y * rotY;
            float rotZ2 = rotation.z * rotZ;

            float rotXY = rotation.x * rotY;
            float rotXZ = rotation.x * rotZ;
            float rotYZ = rotation.y * rotZ;

            float rotWX = rotation.w * rotX;
            float rotWY = rotation.w * rotY;
            float rotWZ = rotation.w * rotZ;

            Vector3 result = default(Vector3);

            result.x = (1f - (rotY2 + rotZ2)) * point.x + (rotXY - rotWZ) * point.y + (rotXZ + rotWY) * point.z;
            result.y = (rotXY + rotWZ) * point.x + (1f - (rotX2 + rotZ2)) * point.y + (rotYZ - rotWX) * point.z;
            result.z = (rotXZ - rotWY) * point.x + (rotYZ + rotWX) * point.y + (1f - (rotX2 + rotY2)) * point.z;

            return result;
        }

        public static implicit operator Quaternion(Quat quat)
        {
            return new Quaternion(quat.x, quat.y, quat.z, quat.w);
        }

        public static implicit operator Quat(Quaternion quaternion)
        {
            return new Quat(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        }

        #endregion

        #region Functions

        /// <summary>
        /// Devuelve los angulos de euler de un <see cref="Quat"/>
        /// y tambien se le puede asignar un <see cref="Vec3"/> como angulos
        /// </summary>
        public Vec3 EulerAngles
        {
            get => ToEulerAngles(this) * Mathf.Rad2Deg;

            set => this = ToQuaternion(value * Mathf.Deg2Rad);
        }

        /// <summary>
        /// Transforma el <see cref="Vec3"/> en un <see cref="Quat"/>.
        /// </summary>
        /// <param name="vec3"></param>
        /// <returns></returns>
        private Quat ToQuaternion(Vec3 vec3) // yaw (Z), pitch (Y), roll (X)
        {
            float cy = Mathf.Cos(vec3.z * .5f);
            float sy = Mathf.Sin(vec3.z * .5f);
            float cp = Mathf.Cos(vec3.y * .5f);
            float sp = Mathf.Sin(vec3.y * .5f);
            float cr = Mathf.Cos(vec3.x * .5f);
            float sr = Mathf.Sin(vec3.x * .5f);

            Quat quat = new Quat();

            quat.w = cr * cp * cy + sr * sp * sy;
            quat.x = sr * cp * cy - cr * sp * sy;
            quat.y = cr * sp * cy + sr * cp * sy;
            quat.z = cr * cp * sy - sr * sp * cy;

            return quat;
        }

        /// <summary>
        /// Transforma el <see cref="Quat"/> en un <see cref="Vec3"/>.
        /// (En radianes)
        /// </summary>
        /// <param name="quat"></param>
        /// <returns></returns>
        private Vec3 ToEulerAngles(Quat quat)
        {
            Vec3 angles;

            // roll (x-axis rotation)
            float sinr_cosp = 2 * (quat.w * quat.x + quat.y * quat.z);
            float cosr_cosp = 1 - 2 * (quat.x * quat.x + quat.y * quat.y);
            angles.x = Mathf.Atan2(sinr_cosp, cosr_cosp);

            // pitch (y-axis rotation)
            float sinp = 2 * (quat.w * quat.y - quat.z * quat.x);
            if (Mathf.Abs(sinp) >= 1)
                angles.y = (Mathf.PI / 2) * Mathf.Sign(sinp); // use 90 degrees if out of range
            else
                angles.y = Mathf.Asin(sinp);

            // yaw / z
            float siny_cosp = 2 * (quat.w * quat.z + quat.x * quat.y);
            float cosy_cosp = 1 - 2 * (quat.y * quat.y + quat.z * quat.z);
            angles.z = Mathf.Atan2(siny_cosp, cosy_cosp);

            return angles;
        }


        /// <summary>
        /// Invierte la rotacion del quaternion
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static Quat Inverse(Quat rotation)
        {
            Quat q;
            q.w = rotation.w;
            q.x = -rotation.x;
            q.y = -rotation.y;
            q.z = -rotation.z;
            return q;
        }

        // TODO angulo, dot, lerp, lerpunclamped, slerp, slerpunclamped, lookrotation, rotatetowards

        #endregion

        #region Internals

        public override bool Equals(object other)
        {
            if (!(other is Quat))
            {
                return false;
            }

            return Equals((Quat)other);
        }

        public bool Equals(Quat other)
        {
            return x.Equals(other.x) && 
                   y.Equals(other.y) && 
                   z.Equals(other.z) && 
                   w.Equals(other.w);
        }

        public override string ToString() => $"({x:0.0},{y:0.0},{z:0.0},{w:0.0})";

        public override int GetHashCode() => x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2) ^ (w.GetHashCode() >> 1);
        #endregion
    }
}

