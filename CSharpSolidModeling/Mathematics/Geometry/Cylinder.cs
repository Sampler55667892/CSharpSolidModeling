namespace Mathematics.Geometry
{
    // Cylinder には (Vector3D) Center を持たせない << 姿勢行列は外部に持たせる

    /// <summary>
    /// 円柱
    /// </summary>
    public struct Cylinder
    {
        #region Fields

        public double Radius;

        public double Height;

        #endregion  // Fields

        #region Properties

        public double Volume => (2d * System.Math.PI * Radius) * Height;

        #endregion  // Properties

        #region Methods

        public Cylinder( double radius, double height )
        {
            this.Radius = radius;
            this.Height = height;
        }

        #endregion  // Methods
    }
}
