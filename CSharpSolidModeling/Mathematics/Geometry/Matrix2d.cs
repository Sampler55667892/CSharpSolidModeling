using System;

namespace Mathematics.Geometry
{
    // ベクトルとの乗算は Matrix * Vector の順で，Vector は (2 [行] * 1 [列]) とみなす
    //   [ X0 Y0 ][ a ] = [ X0 a + Y0 b ]
    //   [ X1 Y1 ][ b ]   [ X1 a + Y1 b ]
    /// <summary>
    /// 2次の正方行列
    /// </summary>
    public struct Matrix2d
    {
        #region Fields

        double[,] contents;

        #endregion  // Fields

        #region Properties

        /// <summary>
        /// 単位行列
        /// </summary>
        public static Matrix2d Identity =>
            new Matrix2d( new double[ 2, 2 ] {
                {1, 0},
                {0, 1}
            } );

        /// <summary>
        /// 零行列
        /// </summary>
        public static Matrix2d Zero =>
            new Matrix2d( new double[ 2, 2 ] {
                {0, 0},
                {0, 0}
            } );

        // x' = scale.x * x
        // y' = scale.y * y
        /// <summary>
        /// 拡大用の行列
        /// [注意] 拡大中心点は原点 (0, 0)
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Matrix2d Scale( Vector2d scale ) =>
            new Matrix2d( new double[ 2, 2 ] {
                {scale.X,       0},
                {      0, scale.Y}
            } );

        /// <summary>
        /// 回転行列
        /// [注意] 回転中心点は原点 (0, 0)
        /// </summary>
        /// <param name="angleRadian"></param>
        /// <returns></returns>
        public static Matrix2d Rotate( double angleRadian )
        {
            // x' = cos (th + a) = cos th cos a - sin th sin a = (cos a) x - (sin a) y
            // y' = sin (th + a) = sin th cos a + cos th sin a = (sin a) x + (cos a) y

            double cos_a = Math.Cos( angleRadian );
            double sin_a = Math.Sin( angleRadian );
            return
                new Matrix2d( new double[ 2, 2 ] {
                    {cos_a, -sin_a},
                    {sin_a,  cos_a}
                } );
        }

        /// <summary>
        /// 行列が単位行列か否かを判定します
        /// </summary>
        public bool IsIdentity
        {
            get {
                for (var i = 0; i < 2; ++i) {
                    for (var j = 0; j < 2; ++j) {
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
                for (var i = 0; i < 2; ++i) {
                    for (var j = 0; j < 2; ++j) {
                        if (!Tolerance.IsIgnorable( contents[ i, j ] ))
                            return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 行列式
        /// </summary>
        public double Determinant => M11 * M22 - M12 * M21;

        /// <summary>
        /// 正則かどうか？
        /// (true -> 正則, false -> 正則でない)
        /// </summary>
        public bool IsRegular => !Tolerance.IsIgnorable( Determinant );

        /// <summary>
        /// 逆行列
        /// (正則でない場合は零行列を返します)
        /// </summary>
        public Matrix2d InverseMatrix
        {
            get {
                double det = Determinant;
                if (Tolerance.IsIgnorable( det ))
                    return Zero;

                // [a b]^{-1} = 1/(ad - bc) [ d -b]
                // [c d]                    [-c  a]
                return new Matrix2d( new double[ 2, 2 ] {
                    { M22 / det, -M12 / det},
                    {-M21 / det,  M11 / det}
                } );
            }
        }

        public bool IsValid => contents != null && contents.Length == 4;

        public double M11 => contents[ 0, 0 ];
        public double M12 => contents[ 0, 1 ];
        public double M21 => contents[ 1, 0 ];
        public double M22 => contents[ 1, 1 ];

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
        public Matrix2d( double[,] contents )
        {
            this.contents = new double[ 2, 2 ];
            SetContents( contents );
        }

        #region operators

        public static Matrix2d operator +( Matrix2d m0, Matrix2d m1 ) =>
            new Matrix2d( new double[ 2, 2 ] {
                {m0.M11 + m1.M11, m0.M12 + m1.M12},
                {m0.M21 + m1.M21, m0.M22 + m1.M22}
            } );

        public static Matrix2d operator -( Matrix2d m0, Matrix2d m1 ) =>
            new Matrix2d( new double[ 2, 2 ] {
                {m0.M11 - m1.M11, m0.M12 - m1.M12},
                {m0.M21 - m1.M21, m0.M22 - m1.M22}
            } );
        
        public static Matrix2d operator *( Matrix2d m0, Matrix2d m1 )
        {
            // [m11  m12][n11  n12]
            // [m21  m22][n21  n22]

            var contents = new double[ 2, 2 ];

            for (var i = 0; i < 2; ++i) {
                for (var j = 0; j < 2; ++j) {
                    contents[ i, j ] = 0;
                    for (var k = 0; k < 2; ++k)
                        contents[ i, j ] += m0[ i, k ] * m1[ k, j ];
                }
            }
            return new Matrix2d( contents );
        }

        public static Vector2d operator *( Matrix2d m, Vector2d v ) =>
            new Vector2d( m.M11 * v.X + m.M12 * v.Y,
                          m.M21 * v.X + m.M22 * v.Y );

        public static Matrix2d operator *( Matrix2d m, double a ) =>
            new Matrix2d( new double[ 2, 2 ] {
                {m.M11 * a, m.M12 * a},
                {m.M21 * a, m.M22 * a}
            } );

        public static Matrix2d operator *( double a, Matrix2d m ) =>
            new Matrix2d( new double[ 2, 2 ] {
                {a * m.M11, a * m.M12},
                {a * m.M21, a * m.M22}
            } );

        public static Matrix2d operator -( Matrix2d m ) =>
            new Matrix2d( new double[ 2, 2 ] {
                {-m.M11, -m.M12},
                {-m.M21, -m.M22}
            } );

        #endregion // operators

        bool SetContents( double[,] contents )
        {
            if (contents == null)
                return false;

            // 簡易サイズチェック
            if (contents.Length != 4)
                return false;

            // 例えば contents が 1 * 4, 4 * 1 の配列の場合は例外を飛ばす
            for (var i = 0; i < 2; ++i) {
                for (var j = 0; j < 2; ++j)
                    this.contents[ i, j ] = contents[ i, j ];
            }
            return true;
        }

        #endregion  // Methods
    }
}
