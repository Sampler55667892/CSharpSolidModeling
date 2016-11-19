namespace Mathematics.Geometry
{
    /// <summary>
    /// 三角形
    /// </summary>
    public struct Triangle
    {
        #region Fields

        public Vector3d Point0;
        public Vector3d Point1;
        public Vector3d Point2;

        #endregion  // Fields

        #region Properties

        /// <summary>
        /// 縮退しているか否かを判定します (true -> 縮退している, false -> それ以外)
        /// </summary>
        public bool IsDegenerate => Tolerance.IsIgnorable( Area );

        /// <summary>
        /// 面積
        /// </summary>
        public double Area
        {
            get {
                // ベクトルの外積の大きさは，2つのベクトルが成す平行四辺形の面積に等しい
                var crossProduct = Vector3d.CrossProduct( Point1 - Point0, Point2 - Point0 );
                return crossProduct.Norm / 2.0;
            }
        }

        /// <summary>
        /// 法線ベクトル
        ///  [契約] 三角形が縮退している場合は計算結果を保証しません
        /// </summary>
        public Vector3d NormalVector
        {
            get {
                var normal = Plane.Normal;
                normal.Normalize();
                return normal;
            }
        }

        /// <summary>
        /// 三角形が成す平面を取得します
        ///  [契約] 三角形が縮退している場合は計算結果を保証しません
        /// </summary>
        public Plane Plane =>
            new Plane { Origin = Point0,
                        Normal = Vector3d.CrossProduct( Point1 - Point0, Point2 - Point0 ) };

        #endregion  // Properties

        #region Methods

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="point0"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        public Triangle( Vector3d point0, Vector3d point1, Vector3d point2 )
        {
            this.Point0 = point0;
            this.Point1 = point1;
            this.Point2 = point2;
        }

        #region 幾何計算

        /// <summary>
        /// 点が三角形の境界or内部に含まれるかを判定します
        /// [契約] 点は三角形が成す平面上に乗ると前提します
        /// </summary>
        /// <param name="point">点</param>
        /// <returns></returns>
        public bool IsInOrOn( Vector3d point ) =>
            new Polygon { Points = new Vector3d[] { Point0, Point1, Point2 } }.IsInOrOn( point );

        /// <summary>
        /// (境界込みで) 直線と三角形との交点が存在するか否かを判定します
        /// (注意：直線は半直線ではありません)
        /// </summary>
        /// <param name="line">直線</param>
        /// <param name="intersection">交点</param>
        /// <returns>true -> 交点 (三角形に対し IN or ON) が存在する, false -> 存在しない</returns>
        public bool Intersects( Line line, out Vector3d? intersection )
        {
            // 直線と平面との交点計算 と 点と三角形との包含判定に還元する
            intersection = line.GetIntersection( this.Plane );
            if (intersection.HasValue)
                return IsInOrOn( intersection.Value );
            return false;
        }

        #endregion  // 幾何計算

        #endregion  // Methods
    }
}
