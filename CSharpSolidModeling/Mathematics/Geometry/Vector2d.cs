using System;

namespace Mathematics.Geometry
{
    public struct Vector2d
    {
        #region Properties

        public double X;
        public double Y;

        public double Norm => Math.Sqrt( X * X + Y * Y );

        public static Vector2d Zero => new Vector2d( 0, 0 );

        public static Vector2d UnitX => new Vector2d( 1, 0 );

        public static Vector2d UnitY => new Vector2d( 0, 1 );

        #endregion  // Properties

        #region Methods

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2d( double x, double y )
        {
            this.X = x;
            this.Y = y;
        }

        #region operators

        public static Vector2d operator +( Vector2d v0, Vector2d v1 ) =>
            new Vector2d( v0.X + v1.X, v0.Y + v1.Y );

        public static Vector2d operator -( Vector2d v0, Vector2d v1 ) =>
            new Vector2d( v0.X - v1.X, v0.Y - v1.Y );

        public static Vector2d operator *( Vector2d v, double a ) =>
            new Vector2d( v.X * a, v.Y * a );

        public static Vector2d operator /( Vector2d v, double a ) =>
            new Vector2d( v.X / a, v.Y / a );

        public static Vector2d operator *( double a, Vector2d v ) =>
            new Vector2d( a * v.X, a * v.Y );

        public static Vector2d operator -( Vector2d v ) =>
            new Vector2d( -v.X, -v.Y );

        public static bool operator ==( Vector2d v0, Vector2d v1 ) =>
            Tolerance.IsIgnorable( v0.X - v1.X ) &&
            Tolerance.IsIgnorable( v0.Y - v1.Y );

        public static bool operator !=( Vector2d v0, Vector2d v1 ) => !(v0 == v1);

        public override bool Equals( object obj ) => base.Equals( obj );

        public override int GetHashCode() => base.GetHashCode();

        #endregion // operators

        public void Normalize()
        {
            var norm = Norm;
            if (Tolerance.IsIgnorable( norm ))
                return;

            X = X / norm;
            Y = Y / norm;
        }

        public static double DotProduct( Vector2d v0, Vector2d v1 ) =>
            v0.X * v1.X + v0.Y * v1.Y;

        public double GetDistance( Vector2d point ) =>
            (this - point).Norm;

        // (v1 | v2) が 0 なら直角
        public bool AreOrthogonal( Vector2d v ) =>
            Tolerance.IsIgnorable( DotProduct( this, v ) );

        // Norm( v1 )^{2} Norm( v2 )^{2} - (v1 | v2)^{2} が 0 なら平行
        public bool AreParallel( Vector2d v )
        {
            return Tolerance.IsIgnorable(
                (Norm * Norm) * (v.Norm * v.Norm) -
                DotProduct( this, v ) * DotProduct( this, v ));
        }

        public override string ToString() => $"({X.ToString()}, {Y.ToString()})";

        #endregion  // Methods
    }
}
