using System;
using System.Collections.Generic;

namespace Mathematics.Geometry
{
    /// <summary>
    /// 多角形
    /// </summary>
    public struct Polygon
    {
        #region Fields

        /// <summary>
        /// 点列
        /// [契約] 各点は1つの平面に乗ると前提します
        /// </summary>
        public Vector3d[] Points;

        #endregion  // Fields

        #region Properties

        /// <summary>
        /// 面積
        /// </summary>
        public double Area => Math.Abs( SignedArea );

        /// <summary>
        /// 符号付の面積
        /// </summary>
        public double SignedArea
        {
            get {
                if (Points == null || Points.Length <= 2)
                    return 0;

                // Procedure
                //  (1) 正規直交基底の計算
                //  (2) 面積の計算

                // (1)
                    // 正規直交基底を取って (x, y) 成分を計算する
                    // 　r = (r | e_{x}) x + (r | e_{y}) y + (r | e_{z}) z
                var basis = ComputeOrthonormalBasis();
                var x_sequence = new double[ Points.Length ];
                var y_sequence = new double[ Points.Length ];
                for (var i = 0; i < Points.Length; ++i) {
                    x_sequence[ i ] = basis.GetXComponent( Points[ i ] );
                    y_sequence[ i ] = basis.GetYComponent( Points[ i ] );
                }

                // (2)
                    // Green's theorem を多角形領域に適用した結果より
                    // 　Area(D) = (1/2) * sum_{i = 0}^{N - 1}(x_{i}y_{i + 1} - x_{i + 1}y_{i})
                var signedArea = 0d;
                for (var i = 0; i < Points.Length; ++i) {
                    var i_next = (i + 1) % Points.Length;
                    signedArea +=
                        x_sequence[ i ] * y_sequence[ i_next ] - x_sequence[ i_next ] * y_sequence[ i ];
                }

                return signedArea / 2d;
            }
        }

        /// <summary>
        /// 法線ベクトル
        /// </summary>
        public Vector3d Normal
        {
            get {
                // Procedure
                //  (1) 正規直交基底の Z 成分の計算
                //  (2) 面積の符合による方向の決定

                // (1)
                var basis = ComputeOrthonormalBasis();

                // (2)
                Vector3d normal = basis.Ez;
                if (SignedArea < 0)
                    normal = -normal;

                return normal;
            }
        }

        public bool IsValid => Points != null && Points.Length > 2;

        /// <summary>
        /// 各点が1つの平面に乗るか？
        /// (true -> 1つの平面に乗る, false -> 1つの平面に乗らない点が1つ以上存在する)
        /// </summary>
        public bool IsPlanar
        {
            get {
                if (Points.Length < 3)
                    return false;
                if (Points.Length == 3)
                    return true;
                var plane = new Plane( this );
                for (int i = 3; i < Points.Length; ++i) {
                    if (!plane.IsOn( Points[ i ] ))
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 同じ座標の点が2つ以上存在するか？
        /// (true -> 2つ以上存在する, false -> 存在しない(全ての点は異なる座標))
        /// </summary>
        public bool ExistsSamePoints
        {
            get {
                var approximatedPointSet = new HashSet<string>();
                for (int i = 0; i < Points.Length; ++i) {
                    var approximatedPoint = Approximation.ToApproximatedString( Points[ i ] );
                    if (!approximatedPointSet.Add( approximatedPoint ))
                        return true;
                }
                return false;
            }
        }

        #endregion  // Properties

        #region Methods

        OrthonormalBasis ComputeOrthonormalBasis()
        {
            var xVector = Points[ 1 ] - Points[ 0 ];
            var zVector = Vector3d.CrossProduct( xVector, Points[ Points.Length - 1 ] - Points[ 0 ] );
            var basis = new OrthonormalBasis( Points[ 0 ] );
            basis.SetByZX( zVector, xVector );

            return basis;
        }

        #region 幾何計算

        /// <summary>
        /// 点が多角形の境界or内部に含まれるかを判定します
        /// [契約] 点は多角形が成す平面上に乗ると前提します
        /// </summary>
        /// <param name="point">点</param>
        /// <returns></returns>
        public bool IsInOrOn( Vector3d point )
        {
            // 角度の総和法
            // 　\sum_{i} theta_{i} = 2 \pi => in
            // 　\sum_{i} theta_{i} = 0 => out

            // Procedure
            //  (1) 各頂点へのベクトルの計算
            //  (2) 各角度の計算

            // (1)
            var vectors = new Vector3d[ Points.Length ];
            for (var i = 0; i < Points.Length; ++i) {
                vectors[ i ] = Points[ i ] - point;
                if (Tolerance.IsIgnorable( vectors[ i ].Norm ))
                    return true;
            }

            // (2)
            // (r_{i} | r_{i + 1}) = |r_{i}| |r_{i + 1}| cos theta
            // 　theta は鋭角の方
            var normal = this.Normal;
            var sumAngle = 0d;
            for (var i = 0; i < Points.Length; ++i) {
                var i_next = (i + 1) % Points.Length;
                var angle  =
                    Math.Acos(
                        Vector3d.DotProduct( vectors[ i ], vectors[ i_next ] ) /
                        (vectors[ i ].Norm * vectors[ i_next ].Norm) );
                // vectors[ i ]×vectors[ i_next ] と normal の方向を比較し，
                // 鋭角となる向きが逆行するかどうかを判定
                if (Vector3d.ScalarTripletProduct( normal, vectors[ i ], vectors[ i_next ] ) < 0)
                    angle = -angle;
                sumAngle += angle;
            }

            var isIn = sumAngle > Math.PI;
            return isIn;
        }

        /// <summary>
        /// 線分と多角形(の境界線分)の交点を取得します
        /// [契約] 線分と多角形が1つの平面上に乗っている
        /// [注意] 互いに平行な線分同士 (両者が重なる場合含む) では交点は考えません
        /// </summary>
        /// <param name="lineSegment"></param>
        /// <returns></returns>
        public List<Vector3d> GetPlanarIntersections( LineSegment lineSegment )
        {
            if (!IsValid)
                return null;

            var intersections = new List<Vector3d>();
            var dicPoints = new HashSet<string>();

            for (var i = 0; i < Points.Length; ++i) {
                var polygonSegment =
                    new LineSegment( Points[ i ],
                                     Points[ (i + 1) % Points.Length ] );
                var intersection =
                    polygonSegment.GetIntersection( lineSegment );
                if (!intersection.HasValue)
                    continue;
                var codedIntersection = Approximation.ToApproximatedString( intersection.Value );
                if (dicPoints.Add( codedIntersection ))
                    intersections.Add( intersection.Value );
            }

            return intersections;
        }

        /// <summary>
        /// 線型独立な2つのベクトル v1,v2 を取得します
        /// [注意] 戻り値が true の場合 Points[ 0 ], Points[ 0 ] + v1, Points[ 0 ] + v2 は
        /// 1つの直線上には乗りません
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>true -> 線型独立な2つのベクトルが存在, false -> 存在しない</returns>
        public bool Get2VectorsWhichAreLinearIndependent( out Vector3d v1, out Vector3d v2 )
        {
            v1 =
            v2 = Vector3d.Zero;

            if (Points == null || Points.Length < 3)
                return false;

            for (var i = 1; i < Points.Length; ++i) {
                var v = Points[ i ] - Points[ 0 ];
                if (v.Norm == 0)
                    continue;
                if (v1.Norm == 0)
                    v1 = v;
                else {
                    // v1 はセット済み
                    if (!v1.AreParallel( v )) {
                        v2 = v;
                        break;
                    }
                }
            }
            return v1.Norm > 0 && v2.Norm > 0;
        }

        #endregion  // 幾何計算

        #endregion  // Methods
    }
}
