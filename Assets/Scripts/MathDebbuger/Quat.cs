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
        /// Devuelve una copia del Quat ya normalizado.
        /// </summary>
        public Quaternion Normalized => Normalize(this);

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
        /// Invierte la rotacion del quaternion.
        /// </summary>
        /// <param name="rotation">Quaternion que queremos invertir.</param>
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

        /// <summary>
        /// Devuelve un Quat normalizado.
        /// </summary>
        /// <param name="quat">Quaternion que queremos normalizar.</param>
        /// <returns></returns>
        public static Quat Normalize(Quat quat)
        {
            float sqrtDot = Mathf.Sqrt(Dot(quat, quat));

            if(sqrtDot < Mathf.Epsilon)
            {
                return Identity;
            }

            return new Quat(quat.x / sqrtDot, quat.y / sqrtDot, quat.z / sqrtDot, quat.w / sqrtDot);
        }

        /// <summary>
        /// Normaliza el Quat.
        /// </summary>
        public void Normalize() => this = Normalize(this);

        public static Quat Lerp(Quat a, Quat b, float t)
        {
            Quat r;
            float time = 1 - t;
            r.x = time * a.x + t * b.x;
            r.y = time * a.y + t * b.y;
            r.z = time * a.z + t * b.z;
            r.w = time * a.w + t * b.w;

            r.Normalize();

            return r;
        }

        public static Quat LerpUnclamped(Quat a, Quat b, float t)
        {
            // TODO implementar LerpUnclamped
            return Quat.Identity;
        }

        public static Quat Slerp(Quat a, Quat b, float t)
        {
            Quat r;

            float time = 1 - t;

            float wa, wb;

            float theta = Mathf.Acos(Dot(a, b));
            float sn = Mathf.Sin(theta);

            wa = Mathf.Sin(time * theta) / sn;
            wb = Mathf.Sin(time * theta) / sn;

            r.x = wa * a.x + wb * b.x;
            r.y = wa * a.y + wb * b.y;
            r.z = wa * a.z + wb * b.z;
            r.w = wa * a.w + wb * b.w;

            r.Normalize();

            return r;
        }

        public static Quat SlerpUnclamped(Quat a, Quat b, float t)
        {
            // TODO implementar SlerpUnclamped
            return Quat.Identity;
        }

        public static float Angle(Quat a, Quat b)
        {
            float dot = Dot(a, b);
            return IsEqualUsingDot(dot) ? 0f : (Mathf.Acos(Mathf.Min(Mathf.Abs(dot), 1f)) * 2f * Mathf.Rad2Deg);
        }

        private static bool IsEqualUsingDot(float dot) => dot > 0.999999f;

        public static float Dot(Quat a, Quat b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

        public static Quat LookRotation(Vec3 forward, Vec3 upwards)
        {
            // TODO implementar LookRotation con 2 vectores
            return Quat.Identity;
        }

        public static Quat LookRotation(Vec3 forward) => LookRotation(forward, Vec3.Up);

        public static Quat RotateTowards(Quat from, Quat to, float maxDegreesDelta)
        {
            float angle = Angle(from, to);

            if (angle == 0f)
            {
                return to;
            }

            return SlerpUnclamped(from, to, Mathf.Min(1f, maxDegreesDelta / angle));
        }

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

