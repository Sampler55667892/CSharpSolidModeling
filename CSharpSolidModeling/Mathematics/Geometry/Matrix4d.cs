using System;

namespace Mathematics.Geometry
{
    // ベクトルとの乗算は Matrix * Vector の順で，Vector は (4 [行] * 1 [列]) とみなす
        // [ X0 Y0 Z0 W0 ][ a ] = [ X0 a + Y0 b + Z0 c + W0 ]
        // [ X1 Y1 Z1 W1 ][ b ]   [ X1 a + Y1 b + Z1 c + W1 ]
        // [ X2 Y2 Z2 W2 ][ c ]   [ X2 a + Y2 b + Z2 c + W2 ]
        // [ 0  0  0  1  ][ 1 ]   [ 1                       ]
    /// <summary>
    /// 4次の正方行列
    /// </summary>
    public struct Matrix4d
    {
        #region Fields

        double[,] contents;

        #endregion  // Fields

        #region Properties

        /// <summary>
        /// 単位行列
        /// </summary>
        public static Matrix4d Identity =>
            new Matrix4d( new double[ 4, 4 ] {
                {1, 0, 0, 0},
                {0, 1, 0, 0},
                {0, 0, 1, 0},
                {0, 0, 0, 1}
            } );

        /// <summary>
        /// 零行列
        /// </summary>
        public static Matrix4d Zero =>
            new Matrix4d( new double[ 4, 4 ] {
                {0, 0, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0}
            } );

        /// <summary>
        /// 行列が単位行列か否かを判定します
        /// </summary>
        public bool IsIdentity
        {
            get {
                for (int i = 0; i < 4; ++i) {
                    for (int j = 0; j < 4; ++j) {
                        if (i == j) {
                            if (!Tolerance.IsIgnorable( contents[ i, j ] - 1 ))
                                return false;
                        } else {
                            if (!Tolerance.IsIgnorable( contents[ i, j ] ))
                                return false;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 行列が零行列か否かを判定します
        /// </summary>
        public bool IsZero
        {
            get {
                for (int i = 0; i < 4; ++i) {
                    for (int j = 0; j < 4; ++j) {
                        if (!Tolerance.IsIgnorable( contents[ i, j ] ))
                            return false;
                    }
                }
                return true;
            }
        }

        public bool IsValid => contents != null && contents.Length == 16;

        public double M11 => contents[ 0, 0 ];
        public double M12 => contents[ 0, 1 ];
        public double M13 => contents[ 0, 2 ];
        public double M14 => contents[ 0, 3 ];
        public double M21 => contents[ 1, 0 ];
        public double M22 => contents[ 1, 1 ];
        public double M23 => contents[ 1, 2 ];
        public double M24 => contents[ 1, 3 ];
        public double M31 => contents[ 2, 0 ];
        public double M32 => contents[ 2, 1 ];
        public double M33 => contents[ 2, 2 ];
        public double M34 => contents[ 2, 3 ];
        public double M41 => contents[ 3, 0 ];
        public double M42 => contents[ 3, 1 ];
        public double M43 => contents[ 3, 2 ];
        public double M44 => contents[ 3, 3 ];

        /// <summary>
        /// インデクサ
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public double this[ int i, int j ] => contents[ i, j ];

        #endregion  // Properties

        #region Methods

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="contents"></param>
        public Matrix4d( double[,] contents )
        {
            this.contents = new double[ 4, 4 ];
            SetContents( contents );
        }

        /// <summary>
        /// 平行移動用の行列
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Matrix4d Translate( Vector3d vector )
        {
            // x' = x + vector.x
            // y' = y + vector.y
            // z' = z + vector.z
            return
                new Matrix4d( new double[ 4, 4 ] {
                    {1, 0, 0, vector.X},
                    {0, 1, 0, vector.Y},
                    {0, 0, 1, vector.Z},
                    {0, 0, 0, 1       }
                } );
        }

        /// <summary>
        /// 拡大用の行列
        /// [注意] 拡大中心点は原点 (0, 0, 0)
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Matrix4d Scale( Vector3d scale )
        {
            // x' = scale.x * x
            // y' = scale.y * y
            // z' = scale.z * z
            return
                new Matrix4d( new double[ 4, 4 ] {
                    {scale.X,       0,       0, 0},
                    {      0, scale.Y,       0, 0},
                    {      0,       0, scale.Z, 0},
                    {      0,       0,       0, 1},
                } );
        }

        // TODO : 効率化 >> 手計算をして行列の掛け算をなくす
        /// <summary>
        /// 拡大用の行列
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static Matrix4d Scale( Vector3d scale, Vector3d origin )
        {
            // (0, 0, 0) に移動して拡大し origin に戻る
            return
                Translate( -origin ) * Scale( scale ) * Translate( origin );
        }

        /// <summary>
        /// Euler 角による回転行列 (回転軸は X軸)
        /// [注意] 回転中心点は原点 (0, 0, 0)
        /// </summary>
        /// <param name="angleRadian"></param>
        /// <returns></returns>
        public static Matrix4d RotateX( double angleRadian )
        {
            // y' = (cos a) y - (sin a) z
            // z' = (sin a) y + (cos a) z
            // x' = x

            var cosA = Math.Cos( angleRadian );
            var sinA = Math.Sin( angleRadian );
            return
                new Matrix4d( new double[ 4, 4 ] {
                    {1,    0,     0, 0},
                    {0, cosA, -sinA, 0},
                    {0, sinA,  cosA, 0},
                    {0,    0,     0, 1}
                } );
        }

        /// <summary>
        /// Euler 角による回転行列 (回転軸は Y軸)
        /// [注意] 回転中心点は原点 (0, 0, 0)
        /// </summary>
        /// <param name="angleRadian"></param>
        /// <returns></returns>
        public static Matrix4d RotateY( double angleRadian )
        {
            // z' = (cos a) z - (sin a) x
            // x' = (sin a) z + (cos a) x
            // y' = y

            var cosA = Math.Cos( angleRadian );
            var sinA = Math.Sin( angleRadian );
            return
                new Matrix4d( new double[ 4, 4 ] {
                    { cosA, 0, sinA, 0},
                    {    0, 1,    0, 0},
                    {-sinA, 0, cosA, 0},
                    {    0, 0,    0, 1}
                } );
        }

        /// <summary>
        /// Euler 角による回転行列 (回転軸は Z軸)
        /// [注意] 回転中心点は原点 (0, 0, 0)
        /// </summary>
        /// <param name="angleRadian"></param>
        /// <returns></returns>
        public static Matrix4d RotateZ( double angleRadian )
        {
            // [ cos( th + a ) ] = [ cos th cos a - sin th sin a ] = [ cos a  -sin a ] * [ cos th ]
            // [ sin( th + a ) ]   [ sin th cos a + cos th sin a ]   [ sin a   cos a ]   [ sin th ]

            // x' = (cos a) x - (sin a) y
            // y' = (sin a) x + (cos a) y
            // z' = z

            var cosA = Math.Cos( angleRadian );
            var sinA = Math.Sin( angleRadian );
            return
                new Matrix4d( new double[ 4, 4 ] {
                    {cosA, -sinA, 0, 0},
                    {sinA,  cosA, 0, 0},
                    {   0,     0, 1, 0},
                    {   0,     0, 0, 1}
                } );
        }

        // TODO : 効率化 >> 手計算をして行列の掛け算をなくす
        /// <summary>
        /// Euler 角による回転行列 (回転軸は X軸)
        /// </summary>
        /// <param name="angleRadian"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static Matrix4d RotateX( double angleRadian, Vector3d origin )
        {
            // (0, 0, 0) に移動して回転し origin に戻る
            return
                Translate( -origin ) * RotateX( angleRadian ) * Translate( origin );
        }

        // TODO : 効率化 >> 手計算をして行列の掛け算をなくす
        /// <summary>
        /// Euler 角による回転行列 (回転軸は Y軸)
        /// </summary>
        /// <param name="angleRadian"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static Matrix4d RotateY( double angleRadian, Vector3d origin )
        {
            return
                Translate( -origin ) * RotateY( angleRadian ) * Translate( origin );
        }

        // TODO : 効率化 >> 手計算をして行列の掛け算をなくす
        /// <summary>
        /// Euler 角による回転行列 (回転軸は Z軸)
        /// </summary>
        /// <param name="angleRadian"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static Matrix4d RotateZ( double angleRadian, Vector3d origin )
        {
            return
                Translate( -origin ) * RotateZ( angleRadian ) * Translate( origin );
        }

        // TODO : Rotate (回転軸, 回転中心点の指定)
//        public static Matrix4d Rotate( double angleRadian, Vector3D origin, Vector3D axis )
        //...

        #region operators

        public static Matrix4d operator +( Matrix4d m0, Matrix4d m1 ) =>
            new Matrix4d( new double[ 4, 4 ] {
                {m0.M11 + m1.M11, m0.M12 + m1.M12, m0.M13 + m1.M13, m0.M14 + m1.M14},
                {m0.M21 + m1.M21, m0.M22 + m1.M22, m0.M23 + m1.M23, m0.M24 + m1.M24},
                {m0.M31 + m1.M31, m0.M32 + m1.M32, m0.M33 + m1.M33, m0.M34 + m1.M34},
                {m0.M41 + m1.M41, m0.M42 + m1.M42, m0.M43 + m1.M43, m0.M44 + m1.M44},
            } );

        public static Matrix4d operator -( Matrix4d m0, Matrix4d m1 ) =>
            new Matrix4d( new double[ 4, 4 ] {
                {m0.M11 - m1.M11, m0.M12 - m1.M12, m0.M13 - m1.M13, m0.M14 - m1.M14},
                {m0.M21 - m1.M21, m0.M22 - m1.M22, m0.M23 - m1.M23, m0.M24 - m1.M24},
                {m0.M31 - m1.M31, m0.M32 - m1.M32, m0.M33 - m1.M33, m0.M34 - m1.M34},
                {m0.M41 - m1.M41, m0.M42 - m1.M42, m0.M43 - m1.M43, m0.M44 - m1.M44},
            } );
        
        public static Matrix4d operator *( Matrix4d m0, Matrix4d m1 )
        {
            // [m11  m12  m13  m14][n11  n12  n13  n14]
            // [m21  m22  m23  m24][n21  n22  n23  n24]
            // [m31  m32  m33  m34][n31  n32  n33  n34]
            // [m41  m42  m43  m44][n41  n42  n43  n44]

            var contents = new double[ 4, 4 ];

            for (int i = 0; i < 4; ++i) {
                for (int j = 0; j < 4; ++j) {
                    contents[ i, j ] = 0;
                    for (int k = 0; k < 4; ++k)
                        contents[ i, j ] += m0[ i, k ] * m1[ k, j ];
                }
            }
            return new Matrix4d( contents );
        }

        public static Vector4d operator *( Matrix4d m, Vector4d v ) =>
            new Vector4d( m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z + m.M14 * v.W ,
                          m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z + m.M24 * v.W ,
                          m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z + m.M34 * v.W ,
                          m.M41 * v.X + m.M42 * v.Y + m.M43 * v.Z + m.M44 * v.W );

        public static Vector3d operator *( Matrix4d m, Vector3d v )
        {
            var vect = m * new Vector4d( v );
            return new Vector3d( vect.X, vect.Y, vect.Z );
        }

        public static Matrix4d operator *( Matrix4d m, double a ) =>
            new Matrix4d( new double[ 4, 4 ] {
                {m.M11 * a, m.M12 * a, m.M13 * a, m.M14 * a},
                {m.M21 * a, m.M22 * a, m.M23 * a, m.M24 * a},
                {m.M31 * a, m.M32 * a, m.M33 * a, m.M34 * a},
                {m.M41 * a, m.M42 * a, m.M43 * a, m.M44 * a},
            } );

        public static Matrix4d operator *( double a, Matrix4d m ) =>
            new Matrix4d( new double[ 4, 4 ] {
                {a * m.M11, a * m.M12, a * m.M13, a * m.M14},
                {a * m.M21, a * m.M22, a * m.M23, a * m.M24},
                {a * m.M31, a * m.M32, a * m.M33, a * m.M34},
                {a * m.M41, a * m.M42, a * m.M43, a * m.M44},
            } );

        public static Matrix4d operator -( Matrix4d m ) =>
            new Matrix4d( new double[ 4, 4 ] {
                {-m.M11, -m.M12, -m.M13, -m.M14},
                {-m.M21, -m.M22, -m.M23, -m.M24},
                {-m.M31, -m.M32, -m.M33, -m.M34},
                {-m.M41, -m.M42, -m.M43, -m.M44},
            } );

        #endregion // operators

        bool SetContents( double[,] contents )
        {
            if (contents == null)
                return false;

            // 簡易サイズチェック
            if (contents.Length != 16)
                return false;

            // 例えば contents が 1 * 16, 2 * 8 の配列の場合は例外を飛ばす
            for (int i = 0; i < 4; ++i) {
                for (int j = 0; j < 4; ++j)
                    this.contents[ i, j ] = contents[ i, j ];
            }
            return true;
        }

        #endregion  // Methods
    }
}
