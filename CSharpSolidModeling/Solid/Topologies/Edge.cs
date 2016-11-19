namespace Solid
{
    /// <summary>
    /// 位相要素エッジ
    /// </summary>
    public class Edge
    {
        #region Properties

        /// <summary>
        /// 始点
        /// </summary>
        public Vertex Start;

        /// <summary>
        /// 終点
        /// </summary>
        public Vertex End;

        internal HalfEdge Left ;
        internal HalfEdge Right;

        #endregion  // Properties

        #region Methods

        /// <summary>
        /// インスタンスを生成します (継承用)
        /// </summary>
        /// <returns></returns>
        public virtual Edge New() => new Edge();

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        protected Edge()
        {
        }

        /// <summary>
        /// 始点について(相互)接続します
        /// </summary>
        /// <param name="start"></param>
        public void ConnectStart( Vertex start )
        {
            this.Start = start;
            start.Connect( this );
        }

        /// <summary>
        /// 終点について(相互)接続します
        /// </summary>
        /// <param name="end"></param>
        public void ConnectEnd( Vertex end )
        {
            this.End = end;
            end.Connect( this );
        }

        /// <summary>
        /// 左ハーフエッジについて(相互)接続します
        /// </summary>
        /// <param name="left"></param>
        internal void ConnectLeft( HalfEdge left )
        {
            this.Left = left;
            left.HostEdge = this;
        }

        /// <summary>
        /// 右ハーフエッジについて(相互)接続します
        /// </summary>
        /// <param name="right"></param>
        internal void ConnectRight( HalfEdge right )
        {
            this.Right = right;
            right.HostEdge = this;
        }

        #region メモ : GetStartVertex / GetEndVertex

        // Rule : 面上では稜線の向きは反時計周りが正
        // 下図では Start -> End が反時計周りになるためにはその稜線はループ B に属して
        // いる必要がある．そのとき，LeftHalfEdge が注目しているループと同じである必要がある．

        // ───○───
        // 　　↑↑
        // 　B □｜□ A
        // 　　　｜↓
        // ───○───

        #endregion  // メモ : GetStartVertex / GetEndVertex

        /// <summary>
        /// 指定ループから見た場合の始点を取得します
        /// </summary>
        /// <param name="loop"></param>
        /// <returns></returns>
        public Vertex GetStartVertex( Loop loop )
        {
            if (Left.HostLoop == loop) {
                // ───○───
                // 　　↑↑
                // 　B ■｜□ A
                // 　　　｜↓
                // ───●───
                return Start;
            } else if (Right.HostLoop == loop) {
                // ───●───
                // 　　↑｜
                // 　B □｜■ A
                // 　　　↓↓
                // ───○───
                return End;
            }
            return null;
        }

        /// <summary>
        /// 指定ループから見た場合の終点を取得します
        /// </summary>
        /// <param name="loop"></param>
        /// <returns></returns>
        public Vertex GetEndVertex( Loop loop )
        {
            if (Left.HostLoop == loop) {
                // ───●───
                // 　　↑↑
                // 　B ■｜□ A
                // 　　　｜↓
                // ───○───
                return End;
            } else if (Right.HostLoop == loop) {
                // ───○───
                // 　　↑｜
                // 　B □｜■ A
                // 　　　↓↓
                // ───●───
                return Start;
            }
            return null;
        }

        /// <summary>
        /// 指定頂点の逆の頂点を取得します (頂点が始点なら終点，終点なら始点を取得します)
        /// (前提：指定頂点は始点 or 終点と前提します)
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public Vertex GetOppositeVertex( Vertex vertex )
        {
            if (vertex == Start)
                return End;
            else if (vertex == End)
                return Start;
            return null;
        }

        public bool Contains( Vertex vertex ) =>
            Start == vertex || End == vertex;

        #endregion  // Methods
    }
}
