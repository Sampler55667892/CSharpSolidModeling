namespace Mathematics.Geometry
{
    // 正規・・・大きさが1
    // 直交・・・互いにベクトルの直積が0
    // 基底・・・(e_{x}, e_{y}, e_{z}) が互いに線型独立で
    // 　全ての点を (e_{x}, e_{y}, e_{z}) を使って一意に表現できる

    /// <summary>
    /// 正規直交基底
    /// </summary>
    public class OrthonormalBasis
    {
        #region Fields

        /// <summary>
        /// 基底上の原点
        /// </summary>
        public Vector3d Origin;

        Vector3d ex;
        Vector3d ey;
        Vector3d ez;
        static OrthonormalBasis standard;

        #endregion  // Fields

        #region Properties

        /// <summary>
        /// 基底上のX軸
        /// </summary>
        public Vector3d Ex => ex;

        /// <summary>
        /// 基底上のY軸
        /// </summary>
        public Vector3d Ey => ey;

        /// <summary>
        /// 基底上のZ軸
        /// </summary>
        public Vector3d Ez => ez;

        /// <summary>
        /// 標準的な正規直交規定 (原点 (0, 0, 0), X軸 (1, 0, 0), Y軸 (0, 1, 0), Z軸 (0, 0, 1))
        /// </summary>
        public static OrthonormalBasis Standard
        {
            get {
                if (standard == null) {
                    standard = new OrthonormalBasis( Vector3d.Zero );
                    standard.SetByXY( Vector3d.UnitX, Vector3d.UnitY );
                }
                return standard;
            }
        }

        /// <summary>
        /// 基底が有効か (正規性と直交性を持つか) 否かを判定します
        /// </summary>
        public bool IsValid
        {
            get {
                // 正規性のチェック
                var isNormalized =
                    Tolerance.IsIgnorable( ex.Norm - 1 ) &&
                    Tolerance.IsIgnorable( ey.Norm - 1 ) &&
                    Tolerance.IsIgnorable( ez.Norm - 1 );
                if (!isNormalized)
                    return false;

                // 直交性のチェック
                var isOrthogonal =
                    Tolerance.IsIgnorable( Vector3d.DotProduct( ex, ey ) ) &&
                    Tolerance.IsIgnorable( Vector3d.DotProduct( ey, ez ) ) &&
                    Tolerance.IsIgnorable( Vector3d.DotProduct( ez, ex ) );
                return isOrthogonal;
            }
        }

        /// <summary>
        /// 座標変換行列 : 変換元は ex = (1, 0, 0), ey = (0, 1, 0), ez = (0, 0, 1) で原点 (0, 0, 0)．
        ///  変換先は Ex, Ey, Ez で原点 Origin．
        /// </summary>
        public Matrix4d CoordinateTransformation =>
            new Matrix4d(
                new double [ 4, 4 ] {
                    { ex.X, ey.X, ez.X, Origin.X },
                    { ex.Y, ey.Y, ez.Y, Origin.Y },
                    { ex.Z, ey.Z, ez.Z, Origin.Z },
                    {    0,    0,    0,        1 }
                } );

        #endregion  // Properties

        #region Methods

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OrthonormalBasis() :
            this( Vector3d.Zero )
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="origin">基底の原点</param>
        public OrthonormalBasis( Vector3d origin )
        {
            this.Origin = origin;
        }

        /// <summary>
        /// X軸とY軸から設定します
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="ey"></param>
        public void SetByXY( Vector3d ex, Vector3d ey ) =>
            Set( ex, ey, Vector3d.CrossProduct( ex, ey ) );

        /// <summary>
        /// Y軸とZ軸から設定します
        /// </summary>
        /// <param name="ey"></param>
        /// <param name="ez"></param>
        public void SetByYZ( Vector3d ey, Vector3d ez ) =>
            Set( Vector3d.CrossProduct( ey, ez ), ey, ez );

        /// <summary>
        /// Z軸とX軸から設定します
        /// </summary>
        /// <param name="ez"></param>
        /// <param name="ex"></param>
        public void SetByZX( Vector3d ez, Vector3d ex ) =>
            Set( ex, Vector3d.CrossProduct( ez, ex ), ez );

        void Set( Vector3d ex, Vector3d ey, Vector3d ez )
        {
            ex.Normalize();
            ey.Normalize();
            ez.Normalize();

            this.ex = ex;
            this.ey = ey;
            this.ez = ez;
        }

        // point = (1, 0, 0) x + (0, 1, 0) y + (0, 0, 1) z + (0, 0, 0)
        //       = e_{x} x' + e_{y} y' + e_{z} z' + o'
        // point - o' = e_{x} x' + e_{y} y' + e_{z} z'
        // 直交性より (e_{y} | e_{x}) = (e_{z} | e_{x}) = 0 だから
        //   x' = ((point - o') | e_{x})
        
        /// <summary>
        /// 基底上の X 成分を取得します
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public double GetXComponent( Vector3d point ) => Vector3d.DotProduct( point - Origin, Ex );

        /// <summary>
        /// 基底上の Y 成分を取得します
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public double GetYComponent( Vector3d point ) => Vector3d.DotProduct( point - Origin, Ey );

        /// <summary>
        /// 基底上の Z 成分を取得します
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public double GetZComponent( Vector3d point ) => Vector3d.DotProduct( point - Origin, Ez );

        #endregion  // Methods
    }
}
