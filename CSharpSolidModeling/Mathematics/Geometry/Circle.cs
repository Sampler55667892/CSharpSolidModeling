namespace Mathematics.Geometry
{
    // Circle には (Vector3D) Center を持たせない << 姿勢行列は外部に持たせる

    /// <summary>
    /// 円
    /// </summary>
    public struct Circle
    {
        #region Fields

        public double Radius;

        #endregion  // Fields

        #region Methods

        public Circle( double radius )
        {
            this.Radius = radius;
        }

        #endregion  // Methods
    }
}
