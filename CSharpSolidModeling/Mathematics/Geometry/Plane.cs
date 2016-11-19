using System.Collections.Generic;

namespace Mathematics.Geometry
{
    /// <summary>
    /// 平面
    /// </summary>
    public struct Plane
    {
        #region Fields

        /// <summary>
        /// 平面上の1点
        /// </summary>
        public Vector3d Origin;
        /// <summary>
        /// 法線
        /// </summary>
        public Vector3d Normal;

        #endregion  // Fields

        #region Properties

        /// <summary>
        /// X軸正方向に法線ベクトルを持つ平面
        /// </summary>
        public static Plane XPlane => new Plane( Vector3d.Zero, Vector3d.UnitX );

        /// <summary>
        /// Y軸正方向に法線ベクトルを持つ平面
        /// </summary>
        public static Plane YPlane => new Plane( Vector3d.Zero, Vector3d.UnitY );

        /// <summary>
        /// Z軸正方向に法線ベクトルを持つ平面
        /// </summary>
        public static Plane ZPlane => new Plane( Vector3d.Zero, Vector3d.UnitZ );

        #endregion  // Properties

        #region Methods

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="normal"></param>
        public Plane( Vector3d origin, Vector3d normal )
        {
            this.Origin = origin;
            this.Normal = normal;
        }

        /// <summary>
        /// コンストラクタ
        /// [契約] 三角形の3点はそれぞれ異なる座標を持つと前提します
        /// </summary>
        /// <param name="triangle"></param>
        public Plane( Triangle triangle ) :
            this( triangle.Point0, triangle.Point1, triangle.Point2 )
        {
        }

        /// <summary>
        /// コンストラクタ
        /// [契約] 多角形の点数は3点以上で，各点はそれぞれ異なる座標を持つと前提します
        /// </summary>
        /// <param name="polygon"></param>
        public Plane( Polygon polygon )
        {
            Vector3d v1, v2;
            if (polygon.Get2VectorsWhichAreLinearIndependent( out v1, out v2 )) {
                var p1 = polygon.Points[ 0 ];
                var p2 = p1 + v1;
                var p3 = p1 + v2;

                var normal = Vector3d.CrossProduct( p2 - p1, p3 - p1 );
                normal.Normalize();

                this.Origin = p1;
                this.Normal = normal;
            } else {
                this.Origin = polygon.Points[ 0 ];
                this.Normal = Vector3d.Zero;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// [契約] 3点はそれぞれ異なる座標を持つと前提します
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        public Plane( Vector3d p1, Vector3d p2, Vector3d p3 )
        {
            var normal = Vector3d.CrossProduct( p2 - p1, p3 - p1 );
            normal.Normalize();

            this.Origin = p1;
            this.Normal = normal;
        }

        #region 幾何計算

        public Vector3d? GetIntersection( Line line ) => line.GetIntersection( this );

        public Vector3d? GetIntersection( LineSegment lineSegment ) => lineSegment.GetIntersection( this );

        public Vector3d[] GetIntersections( Triangle triangle )
        {
            // 3本の線分と平面との交差判定に還元する
            var intersections = new HashSet<string>();

            var lineSegments =
                new [] {
                    new LineSegment( triangle.Point0, triangle.Point1 ),
                    new LineSegment( triangle.Point1, triangle.Point2 ),
                    new LineSegment( triangle.Point2, triangle.Point0 ) };

            var result = new List<Vector3d>();

            for (var i = 0; i < lineSegments.Length; ++i) {
                var tempIntersection = GetIntersection( lineSegments[ i ] );
                if (tempIntersection.HasValue) {
                    var key = Approximation.ToApproximatedString( tempIntersection.Value );
                    if (!intersections.Contains( key )) {
                        intersections.Add( key );
                        result.Add( tempIntersection.Value );
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// 点が平面上に乗るかどうかを判定します
        /// </summary>
        /// <param name="point"></param>
        /// <returns>true -> 平面上に乗る, false -> 平面上に乗らない</returns>
        public bool IsOn( Vector3d point )
        {
            // 直線と平面との交点計算
            var line = new Line( point, Normal );
            var intersection = line.GetIntersection( this );
            if (!intersection.HasValue)
                return false;
            // 交点と入力点との距離計算
            return Tolerance.IsIgnorable( (intersection.Value - point).Norm );
        }

        #endregion // 幾何計算

        #endregion  // Methods
    }
}
