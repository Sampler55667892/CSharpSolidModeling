namespace Mathematics.Geometry
{
    // Rectangle には (Vector3D) Center を持たせない << 姿勢行列は外部に持たせる

    /// <summary>
    /// 長方形
    /// </summary>
    public struct Rectangle
    {
        #region Fields

        public Vector2d Size;

        #endregion  // Fields

        #region Properties

        public double Area => Size.X * Size.Y;

        #endregion  // Properties

        #region Methods

        public Rectangle( double sizeX, double sizeY )
        {
            this.Size = new Vector2d( sizeX, sizeY );
        }

        public Rectangle( Vector2d size )
        {
            this.Size = size;
        }

        #endregion  // Methods
    }
}
