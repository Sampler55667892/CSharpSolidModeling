using System;

namespace Mathematics.Geometry
{
    public struct Vector4d
    {
        #region Properties

        public double X;
        public double Y;
        public double Z;
        public double W;

        public double Norm => Math.Sqrt( X * X + Y * Y + Z * Z + W * W );

        public static Vector4d Zero => new Vector4d( 0, 0, 0, 0 );

        #endregion  // Properties

        #region Methods

        public Vector4d( double x, double y, double z, double w )
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public Vector4d( Vector3d vector )
        {
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = vector.Z;
            this.W =        1;
        }

        #region operators

        public static Vector4d operator +( Vector4d v0, Vector4d v1 ) =>
            new Vector4d( v0.X + v1.X, v0.Y + v1.Y, v0.Z + v1.Z, v0.W + v1.W );

        public static Vector4d operator -( Vector4d v0, Vector4d v1 ) =>
            new Vector4d( v0.X - v1.X, v0.Y - v1.Y, v0.Z - v1.Z, v0.W - v1.W );

        public static Vector4d operator *( Vector4d v, double a ) =>
            new Vector4d( v.X * a, v.Y * a, v.Z * a, v.W * a );

        public static Vector4d operator /( Vector4d v, double a ) =>
            new Vector4d( v.X / a, v.Y / a, v.Z / a, v.W / a );

        public static Vector4d operator *( double a, Vector4d v ) =>
            new Vector4d( a * v.X, a * v.Y, a * v.Z, a * v.W );

        public static Vector4d operator -( Vector4d v ) =>
            new Vector4d( -v.X, -v.Y, -v.Z, -v.W );

        public static bool operator ==( Vector4d v0, Vector4d v1 )
        {
            return
                Tolerance.IsIgnorable( v0.X - v1.X ) &&
                Tolerance.IsIgnorable( v0.Y - v1.Y ) &&
                Tolerance.IsIgnorable( v0.Z - v1.Z ) &&
                Tolerance.IsIgnorable( v0.W - v1.W );
        }

        public static bool operator !=( Vector4d v0, Vector4d v1 ) => !(v0 == v1);

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
            Z = Z / norm;
            W = W / norm;
        }

        public static double DotProduct( Vector4d v0, Vector4d v1 ) =>
            v0.X * v1.X + v0.Y * v1.Y + v0.Z * v1.Z + v0.W * v1.W;

        public double GetDistance( Vector4d point ) =>
            (this - point).Norm;

        public override string ToString() =>
            $"({X.ToString()}, {Y.ToString()}, {Z.ToString()}, {W.ToString()})";

        #endregion  // Methods
    }
}
