namespace CustomMath
{
    public struct BoundingBox
    {
        private Vec3 center;
        private Vec3 extends;

        public BoundingBox(Vec3 center, Vec3 size)
        {
            this.center = center;
            extends = size * .5f;
        }

        public Vec3 Center
        {
            get => center;
            set => center = value;
        }

        public Vec3 Size
        {
            get => extends * 2;
            set => extends = value * .5f;
        }

        public Vec3 Extends
        {
            get => extends;
            set => extends = value;
        }

        public Vec3 Min => center - extends;

        public Vec3 Max => center + extends;

        public bool Contains(Vec3 point) => Min.x < point.x && Min.y < point.y && Min.z < point.z && point.x < Max.x && point.y < Max.y && point.z < Max.z;
    }
}
