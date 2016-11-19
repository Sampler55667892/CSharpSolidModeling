namespace Mathematics.Geometry
{
    // Cuboid には (Vector3D) Center を持たせない << 姿勢行列は外部に持たせる

    /// <summary>
    /// 直方体
    /// </summary>
    public struct Cuboid
    {
        #region Fields

        public Vector3d Size;

        #endregion  // Fields

        #region Properties

        public double Volume => Size.X * Size.Y * Size.Z;

        #endregion  // Properties

        #region Methods

        public Cuboid( double sizeX, double sizeY, double sizeZ )
        {
            this.Size = new Vector3d( sizeX, sizeY, sizeZ );
        }

        public Cuboid( Vector3d size )
        {
            this.Size = size;
        }

        #endregion  // Methods
    }
}
