using Mathematics.Geometry;

namespace Solid
{
    // Euler-Poincare' formula
    // 　V - E + F - (L - F) - 2(S - G) = 0

    // MVFS, MHKFS, MHGKF
    // MEV, MEF, MEKH
    // and Duals

    /// <summary>
    /// Euler操作
    /// </summary>
    public class EulerOperator
    {
        #region Methods

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EulerOperator()
        {
            mvfs = new Mvfs();
            mev = new Mev();
            mef = new Mef();
        }

        public Shell Mvfs( Vector3d position, out Vertex newVertex, out Loop newLoop, out Face newFace ) =>
            mvfs.Do( position, out newVertex, out newLoop, out newFace );

        #region MEV

        public Vertex MevSpurEv( Vector3d vertexPosition, Vertex startVertex, Edge prevLeftEdge,
            Edge prevRightEdge, out Edge newEdge ) =>
            mev.SpurEv( vertexPosition, startVertex, prevLeftEdge, prevRightEdge, out newEdge );

        public Vertex MevSplitE( Vector3d vertexPosition, Edge splitTargetEdge, bool newVertexIsEnd,
            out Edge newEdge ) =>
            mev.SplitE( vertexPosition, splitTargetEdge, newVertexIsEnd, out newEdge );

        // MEV_SplitV...

        #endregion  // MEV

        public Face Mef( Vertex startVertex, Vertex endVertex, Edge prevLeftEdge, Edge prevRightEdge,
            Edge nextLeftEdge, Edge nextRightEdge, out Edge newEdge, out Loop oldLoop, out Loop newLoop ) =>
            mef.Do( startVertex, endVertex, prevLeftEdge, prevRightEdge, nextLeftEdge, nextRightEdge,
                out newEdge, out oldLoop, out newLoop );

        #endregion  // Methods

        #region Fields

        Mvfs mvfs;
        Mev mev;
        Mef mef;

        #endregion  // Fields
    }
}
