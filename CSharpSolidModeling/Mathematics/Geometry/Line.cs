namespace Mathematics.Geometry
{
    /// <summary>
    /// (無限)直線
    /// </summary>
    public struct Line
    {
        #region Fields

        public Vector3d Origin;
        public Vector3d Vector;

        #endregion  // Fields

        #region Properties

        /// <summary>
        /// X軸正方向に方向ベクトルを持つ直線
        /// </summary>
        public static Line XLine => new Line( Vector3d.Zero, Vector3d.UnitX );
        /// <summary>
        /// Y軸正方向に方向ベクトルを持つ直線
        /// </summary>
        public static Line YLine => new Line( Vector3d.Zero, Vector3d.UnitY );
        /// <summary>
        /// Z軸正方向に方向ベクトルを持つ直線
        /// </summary>
        public static Line ZLine => new Line( Vector3d.Zero, Vector3d.UnitZ );

        #endregion  // Properties

        #region Methods

        public Line( Vector3d origin, Vector3d vector )
        {
            this.Origin = origin;
            this.Vector = vector;
        }

        public Line( LineSegment lineSeg )
        {
            this.Origin = lineSeg.StartPoint;
            this.Vector = lineSeg.EndPoint - lineSeg.StartPoint;
        }

        public static Line FromStartEnd( Vector3d start, Vector3d end ) =>
            new Line( start, end - start );

        #region 幾何計算

        public Vector3d? GetIntersection( Plane plane )
        {
            // 直線と平面が平行かどうか (直線の方向ベクトルと平面の法線ベクトルが直交するかどうか)
            var v_dot_n = Vector3d.DotProduct( Vector, plane.Normal );
            if (Tolerance.IsIgnorable( v_dot_n ))
                return null;

            // 直線の方程式 x = o_{l} + b V
            // 平面の方程式 (x - o_{p} | N) = 0
            // 両者を連立させて，
            // 　(o_{l} + b V - o_{p} | N) = 0
            // 　(o_{l} - o_{p} | N) + b (V | N) = 0
            // (V | N) != 0 ならば，
            // 　b = (o_{p} - o_{l} | N) / (V | N)
            // 直線の式に代入し，交点 x_{c} は
            // 　x_{c} = o_{l} + ((o_{p} - o_{l} | N) / (V | N)) V

            var factor =
                Vector3d.DotProduct( plane.Origin - Origin, plane.Normal ) / v_dot_n;
            var intersection =
                Origin + factor * Vector;

            return intersection;
        }

        /// <summary>
        /// 2つの直線の交点を取得します
        /// [契約] 平行な関係にある場合 (重なる場合・端点でつながる場合含む) は交点はなしと考えます
        /// </summary>
        /// <param name="line"></param>
        /// <param name="areParallel">平行かどうか(true -> 平行, false -> 非平行)</param>
        /// <returns></returns>
        public Vector3d? GetIntersection( Line line, out bool areParallel )
        {
            double aMin, bMin;
            var distance = GetDistance( line, out aMin, out bMin, out areParallel );
            if (areParallel || !Tolerance.IsIgnorable( distance ))
                return null;
            return Origin + aMin * Vector;

            /*
            // 平行でない -> ねじれの位置になければ, 平面が1つ決まる
            var points = new Vector3d[] {
                Origin,
                Origin + Vector,
                line.Origin,
                line.Origin + line.Vector
            };
            var plane = Plane.From3Points( points[ 0 ], points[ 1 ], points[ 2 ] );
            if (!plane.IsOn( points[ 3 ] ))
                return null;

            // 4点が1つの平面に乗る -> 2つの直線はねじれの位置にはない
            // 平面上へと座標変換して2次元で交点計算する (精度がよい)
            // 　l1 : p = o1 + a v1
            // 　l2 : q = o2 + b v2
            // 　o1 + a v1 = o2 + b v2
            // v2 に直交するベクトルを構成し (v2 を 90度回転する) 内積で未知数を1つ減らし求める
            // 計算後，逆座標変換して3次元上の点を得る
            */
        }

        /// <summary>
        /// 2つの直線の最短距離を取得します
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public double GetDistance( Line line )
        {
            // l1 : Origin + a Vector
            // l2 : line.Origin + b line.Vector
            // 2直線が最接近する際のl1上のパラメータ aMin, l2上のパラメータ bMin
            double aMin, bMin;
            bool areParallel;
            return GetDistance( line, out aMin, out bMin, out areParallel );
        }

        double GetDistance( Line line, out double aMin, out double bMin, out bool areParallel )
        {
            aMin =
            bMin = 0;

            // 2直線が平行の場合 -> 点と射影点との距離で測る
            areParallel = AreParallel( line );
            if (areParallel)
                return GetDistance( line.Origin );

            // 最小2乗法を応用する
            // 　f(a, b) = Norm( (p + a v1) - (q + b v2) )^{2}　を最小にする (a, b) を求める
            // 　f(a, b) を展開して整理し \partial f / \partial a = 0 かつ \partial f / \partial b = 0
            // 　の条件から次の連立方程式が得られる：
            // 　[ Norm( v1 )^{2}   -(v1 | v2)     ][ a ] = [ (q - p | v1) ]
            // 　[ -(v1 | v2)       Norm( v2 )^{2} ][ b ]   [ (p - q | v2) ]
            // 　左辺の行列式が正則でないなら2直線は平行

            var dotProdV1V2 = Vector3d.DotProduct( Vector, line.Vector );
            var v1Norm = Vector.Norm;
            var v2Norm = line.Vector.Norm;
            var matrix =
                new Matrix2d( new double[ 2, 2 ] {
                    {v1Norm * v1Norm, -dotProdV1V2   },
                    {-dotProdV1V2   , v2Norm * v2Norm}
                });
            var param =
                matrix.InverseMatrix *
                new Vector2d( Vector3d.DotProduct( line.Origin - Origin, Vector ),
                              Vector3d.DotProduct( Origin - line.Origin, line.Vector ) );
            aMin = param.X;
            bMin = param.Y;

            return ((Origin + aMin * Vector) - (line.Origin + bMin * line.Vector)).Norm;
        }

        /// <summary>
        /// 2つの直線が平行かどうかを判定します
        /// </summary>
        /// <param name="line"></param>
        /// <returns>true -> 平行, false -> 平行でない</returns>
        public bool AreParallel( Line line ) =>
            Vector.AreParallel( line.Vector );

        /// <summary>
        /// 点が直線上に乗っているかどうかを判定します
        /// </summary>
        /// <param name="point"></param>
        /// <returns>true -> 乗っている, false -> 乗っていない</returns>
        public bool IsOn( Vector3d point ) =>
            Tolerance.IsIgnorable( GetDistance( point ) );

        /// <summary>
        /// 点と直線との (点と点を直線へと垂直に射影した点との) 距離を取得します
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public double GetDistance( Vector3d point )
        {
            // let p' : projected point
            //　 p' = q + a v　(v != 0)
            // point と p' との Euclid 距離 d は
            // 　d = Sqrt( Norm( point - p' )^{2} )
            // 式を展開して簡略化した結果は次のようになる
            // 　d = Norm( v * (point - q) ) / Norm( v )

            return Vector3d.CrossProduct( Vector, point - Origin ).Norm / Vector.Norm;
        }

        /// <summary>
        /// 点を直線へと垂直に射影した点を取得します
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector3d GetProjectedPoint( Vector3d point )
        {
            double projectedParameter;
            return GetProjectedPoint( point, out projectedParameter );
        }

        internal Vector3d GetProjectedPoint( Vector3d point, out double projectedParameter )
        {
            // let p' : projected point
            // 　p' = point + b n
            // 　l : q + a v　(v != 0)
            // 両者を連立して, 
            // 　(p' =) point + b n = q + a v
            // 両辺に v を内積として作用させる．(v | n) = 0 (直交) に注意し
            // 　(v | point) = (v | q) + a(v | v)
            // v != 0 より (v | v) != 0 で
            // 　a = ((v | point) - (v | q)) / (v | v)
            // 従って p' は
            // 　p' = q + a v = q + ((v | point) - (v | q))/(v | v) v

            projectedParameter =
                (Vector3d.DotProduct( Vector, point ) - Vector3d.DotProduct( Vector, Origin )) /
                Vector3d.DotProduct( Vector, Vector );
            return Origin + projectedParameter * Vector;
        }

        #endregion // 幾何計算

        #endregion  // Methods
    }
}
