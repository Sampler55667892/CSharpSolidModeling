namespace Mathematics.Geometry
{
    /// <summary>
    /// (有限)線分
    /// </summary>
    public struct LineSegment
    {
        #region Fields

        public Vector3d StartPoint;
        public Vector3d EndPoint;

        #endregion  // Fields

        #region Properties

        public Vector3d Vector => EndPoint - StartPoint;

        public double Length => Vector.Norm;

        #endregion  // Properties

        #region Methods

        public LineSegment( Vector3d startPoint, Vector3d endPoint )
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }

        #region 幾何計算

        public Vector3d? GetIntersection( Plane plane )
        {
            if (Tolerance.IsIgnorable( Length ))
                return null;

            // 直線と平面との交差判定
            var line = new Line( StartPoint, Vector );
            var intersection = line.GetIntersection( plane );
            if (!intersection.HasValue)
                return null;

            // 交点が StartPoint と EndPoint の間にあるか否かを判定する
            // 直線の方程式は x = o + b v  =>  b = ((x - o) | v) / (v | v)
            var factor =
                Vector3d.DotProduct( intersection.Value - StartPoint, Vector ) /
                Vector3d.DotProduct( Vector, Vector );
            if (factor < 0d || 1d < factor)
                return null;

            return intersection.Value;
        }

        /// <summary>
        /// 線分同士の交点を取得します
        /// [契約] 平行な関係にある場合 (重なる場合・端点でつながる場合含む) は交点はなしと考えます
        /// </summary>
        /// <param name="lineSeg"></param>
        /// <returns></returns>
        public Vector3d? GetIntersection( LineSegment lineSeg )
        {
            // 直線同士の交点計算
            var line1 = new Line( this );
            var line2 = new Line( lineSeg );

            bool areParallel;
            var intersection = line1.GetIntersection( line2, out areParallel );
            if (!intersection.HasValue)
                return null;

            // 交点が両方の線分上にある場合，かつそのときのみ交点ありと判定する
            if (IsOn( intersection.Value ) && lineSeg.IsOn( intersection.Value ))
                return intersection;

            return null;
        }

        /// <summary>
        /// 2つの線分が平行かどうかを判定します
        /// </summary>
        /// <param name="line"></param>
        /// <returns>true -> 平行, false -> 平行でない</returns>
        public bool AreParallel( LineSegment lineSeg ) => Vector.AreParallel( lineSeg.Vector );

        /// <summary>
        /// 点が線分上に乗っているかどうかを判定します
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsOn( Vector3d point )
        {
            double projectedParameter;
            var projectedPoint = new Line( this ).GetProjectedPoint( point, out projectedParameter );
            // 点と射影点との距離が誤差を除いて 0 か？
            if (!Tolerance.IsIgnorable( (point - projectedPoint).Norm ))
                return false;
            // 射影点が線分上にあるか？
            return 0d <= projectedParameter && projectedParameter <= 1d;
        }

        #endregion // 幾何計算

        #endregion  // Methods
    }
}
