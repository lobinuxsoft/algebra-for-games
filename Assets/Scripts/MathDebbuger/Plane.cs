namespace CustomMath
{
    public struct Plane
    {
        private Vec3 normal;
        private float distance;
        private float area;

        /// <summary>
        /// Normal del plano
        /// </summary>
        public Vec3 Normal
        {
            get { return normal; }
            set { normal = value; }
        }

        /// <summary>
        /// La distancia medida desde el Plano hasta el origen, a lo largo de la normal del Plano.
        /// </summary>
        public float Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        public float Area
        {
            get { return area; }
            set { area = value; }
        }

        /// <summary>
        /// Crea un plano.
        /// </summary>
        /// <param name="inNormal"></param>
        /// <param name="inPoint"></param>
        public Plane(Vec3 inNormal, Vec3 inPoint)
        {
            normal = Vec3.Normalize(inNormal);
            distance = -Vec3.Dot(inNormal, inPoint);
            area = distance;
        }

        /// <summary>
        /// Crea un plano.
        /// </summary>
        /// <param name="inNormal"></param>
        /// <param name="distance"></param>
        public Plane(Vec3 inNormal, float distance)
        {
            normal = Vec3.Normalize(inNormal);
            this.distance = distance;
            area = distance;
        }

        /// <summary>
        /// Crea un plano.
        /// </summary>
        /// <param name="vecA"></param>
        /// <param name="vecB"></param>
        /// <param name="vecC"></param>
        public Plane(Vec3 vecA, Vec3 vecB, Vec3 vecC)
        {
            normal = Vec3.Normalize(Vec3.Cross(vecB-vecA, vecC-vecA));
            distance = -Vec3.Dot(normal, vecA);
            
            // El producto cruz entre 2 vectores te da un vector cuya magnitud es el area de un paralelogramo y la mitad del mismo es un triangulo que lo forma
            //area = Vec3.Cross(vecA - vecB, vecA - vecC).magnitude * .5f; 
            area = Vec3.Cross(vecB - vecA, vecC - vecA).magnitude * .5f; 
        }

        /// <summary>
        /// Establece un plano utilizando un punto que se encuentra dentro de él,
        /// junto con una normal para orientarlo.
        /// </summary>
        /// <param name="inNormal"> La normal del plano.</param>
        /// <param name="inPoint"> Un punto que se encuentra en el plano.</param>
        public void SetNormalAndPosition(Vec3 inNormal, Vec3 inPoint)
        {
            normal = Vec3.Normalize(inNormal);
            distance = -Vec3.Dot(inNormal, inPoint);
        }

        /// <summary>
        /// Establece un plano utilizando tres puntos que se encuentran dentro de él.
        /// Los puntos giran en el sentido de las agujas del reloj cuando miras hacia abajo en la superficie superior del plano.
        /// </summary>
        /// <param name="vectA"> Primer punto en el sentido de las agujas del reloj.</param>
        /// <param name="vecB"> Segundo punto en el sentido de las agujas del reloj.</param>
        /// <param name="vecC"> Tercer punto en el sentido de las agujas del reloj.</param>
        public void Set3Points(Vec3 vectA, Vec3 vecB, Vec3 vecC)
        {
            normal = Vec3.Normalize(Vec3.Cross(vecB - vectA, vecC - vectA));
            distance = -Vec3.Dot(normal, vectA);
        }

        /// <summary>
        /// Hace que el plano mire en la dirección opuesta.
        /// </summary>
        public void Flip()
        {
            normal = -normal;
            distance = -distance;
        }

        /// <summary>
        /// Devuelve una copia del plano que mira en la dirección opuesta.
        /// </summary>
        public Plane flipped => new Plane(-normal, -distance);

        
        /// <summary>
        /// Mueve el plano en el espacio, tomando como referencia un vector.
        /// </summary>
        /// <param name="translation">El desplazamiento en el espacio para mover el plano</param>
        public void Translate(Vec3 translation) => distance += Vec3.Dot(normal, translation);

        /// <summary>
        /// Devuelve una copia del plano con la posicion modificada.
        /// </summary>
        /// <param name="plane">Plano que se mueve en el espacio.</param>
        /// <param name="translation">El desplazamiento para mover el plano.</param>
        /// <returns>The translated plane.</returns>
        public static Plane Translate(Plane plane, Vec3 translation) =>
            new Plane(plane.Normal, plane.Distance += Vec3.Dot(plane.Normal, translation));

        /// <summary>
        /// Devuelve el punto mas cercano de un plano en base a una posicion dada.
        /// </summary>
        /// <param name="point">Punto que se projecta en el plano</param>
        /// <returns>Un punto en el plano que está más cerca de una posicion.</returns>
        public Vec3 ClosestPointOnPlane(Vec3 point)
        {
            float dot = Vec3.Dot(normal, point) + distance;
            return point - normal * dot;
        }

        /// <summary>
        /// Devuelve una distancia con signo del plano al punto.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float GetDistanceToPoint(Vec3 point) => Vec3.Dot(normal, point) + distance;

        /// <summary>
        /// Chequea si hay un punto del lado positivo del plano.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool GetSide(Vec3 point) => (double)Vec3.Dot(normal, point) + (double)distance > 0.0;

        /// <summary>
        /// Chequea si estan los 2 puntos del mismo lado del plano.
        /// </summary>
        /// <param name="inPt0"></param>
        /// <param name="inPt1"></param>
        /// <returns></returns>
        public bool SameSide(Vec3 inPt0, Vec3 inPt1)
        {
            float distanceToPoint1 = GetDistanceToPoint(inPt0);
            float distanceToPoint2 = GetDistanceToPoint(inPt1);

            return (double)distanceToPoint1 > 0.0 && (double)distanceToPoint2 > 0.0 ||
                   (double)distanceToPoint1 <= 0.0 && (double)distanceToPoint2 <= 0.0;
        }

        public override string ToString() => $"normal:{normal}, distance:{distance}";
    }
}