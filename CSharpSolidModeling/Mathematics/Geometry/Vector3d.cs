using System;

namespace Mathematics.Geometry
{
    public struct Vector3d
    {
        #region Properties

        public double X;
        public double Y;
        public double Z;

        public double Norm => Math.Sqrt( X * X + Y * Y + Z * Z );

        public static Vector3d Zero => new Vector3d( 0, 0, 0 );

        public static Vector3d UnitX => new Vector3d( 1, 0, 0 );

        public static Vector3d UnitY => new Vector3d( 0, 1, 0 );

        public static Vector3d UnitZ => new Vector3d( 0, 0, 1 );

        #endregion  // Properties

        #region Methods

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3d( double x, double y, double z )
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="z"></param>
        public Vector3d( Vector2d vector, double z )
        {
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = z;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="vector"></param>
        public Vector3d( Vector4d vector )
        {
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = vector.Z;
        }

        #region operators

        public static Vector3d operator +( Vector3d v0, Vector3d v1 ) =>
            new Vector3d( v0.X + v1.X, v0.Y + v1.Y, v0.Z + v1.Z );

        public static Vector3d operator -( Vector3d v0, Vector3d v1 ) =>
            new Vector3d( v0.X - v1.X, v0.Y - v1.Y, v0.Z - v1.Z );

        public static Vector3d operator *( Vector3d v, double a ) =>
            new Vector3d( v.X * a, v.Y * a, v.Z * a );

        public static Vector3d operator /( Vector3d v, double a ) =>
            new Vector3d( v.X / a, v.Y / a, v.Z / a );

        public static Vector3d operator *( double a, Vector3d v ) =>
            new Vector3d( a * v.X, a * v.Y, a * v.Z );

        public static Vector3d operator -( Vector3d v ) =>
            new Vector3d( -v.X, -v.Y, -v.Z );

        public static bool operator ==( Vector3d v0, Vector3d v1 )
        {
            return
                Tolerance.IsIgnorable( v0.X - v1.X ) &&
                Tolerance.IsIgnorable( v0.Y - v1.Y ) &&
                Tolerance.IsIgnorable( v0.Z - v1.Z );
        }

        public static bool operator !=( Vector3d v0, Vector3d v1 ) => !(v0 == v1);

        public override bool Equals( object obj ) => base.Equals( obj );

        public override int GetHashCode() => base.GetHashCode();

        #endregion // operator

        public void Normalize()
        {
            var norm = Norm;
            if (Tolerance.IsIgnorable( norm ))
                return;

            X = X / norm;
            Y = Y / norm;
            Z = Z / norm;
        }

        /// <summary>
        /// 内積を計算します
        /// </summary>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <returns></returns>
        public static double DotProduct( Vector3d v0, Vector3d v1 )
        {
            return v0.X * v1.X + v0.Y * v1.Y + v0.Z * v1.Z;
        }

        /// <summary>
        /// 外積を計算します
        /// </summary>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <returns></returns>
        public static Vector3d CrossProduct( Vector3d v0, Vector3d v1 )
        {
            // v0 × v1 =
            // 　|v0.X  v0.Y  v0.Z|
            // 　|v1.X  v1.Y  v1.Z|
            // 　|i     j     k   |
            return
                new Vector3d( v0.Y * v1.Z - v0.Z * v1.Y ,
                              v0.Z * v1.X - v0.X * v1.Z ,
                              v0.X * v1.Y - v0.Y * v1.X );
        }

        /// <summary>
        /// スカラー三重積を計算します
        /// </summary>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double ScalarTripletProduct( Vector3d v0, Vector3d v1, Vector3d v2 ) =>
            DotProduct( v0, CrossProduct( v1, v2 ) );

        /// <summary>
        /// ベクトル三重積を計算します
        /// </summary>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3d VectorTripletProduct( Vector3d v0, Vector3d v1, Vector3d v2 ) =>
            CrossProduct( v0, CrossProduct( v1, v2 ) );

        public double GetDistance( Vector3d point ) => (this - point).Norm;

        // (v1 | v2) が 0 なら直角
        public bool AreOrthogonal( Vector3d v ) =>
            Tolerance.IsIgnorable(
                DotProduct( this, v ) );

        // Norm( v1 )^{2} Norm( v2 )^{2} - (v1 | v2)^{2} が 0 なら平行
        public bool AreParallel( Vector3d v )
        {
            return Tolerance.IsIgnorable(
                (Norm * Norm) * (v.Norm * v.Norm) -
                DotProduct( this, v ) * DotProduct( this, v ));
        }

        public override string ToString() =>
            $"({X.ToString()}, {Y.ToString()}, {Z.ToString()})";

        #endregion  // Methods
    }
}
