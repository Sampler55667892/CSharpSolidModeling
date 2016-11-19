using System;

namespace Mathematics.Geometry
{
    // Sphere には (Vector3D) Center を持たせない << 姿勢行列は外部に持たせる

    /// <summary>
    /// 球体
    /// </summary>
    public struct Sphere
    {
        #region Fields

        public double Radius;

        #endregion  // Fields

        #region Properties

        public double Volume => (4.0 * Math.PI * Radius * Radius * Radius) / 3.0;

        public double SurfaceArea => 4.0 * Math.PI * Radius * Radius;

        #endregion  // Properties

        #region Methods

        public Sphere( double radius )
        {
            this.Radius = radius;
        }

        // zenith (天頂角), azimuth (方位角)
        // X軸では azimuth は 0, Z軸では zenith は 0
        public static Vector3d GetPosition( double radius, double zenithRadian, double azimuthRadian )
        {
            var rho = radius * Math.Sin( zenithRadian );
            return
                new Vector3d( rho * Math.Cos( azimuthRadian ),
                              rho * Math.Sin( azimuthRadian ),
                              radius * Math.Cos( zenithRadian ) );
        }

        #endregion  // Methods
    }
}
