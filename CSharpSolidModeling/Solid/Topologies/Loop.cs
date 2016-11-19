using System.Collections.Generic;

namespace Solid
{
    /// <summary>
    /// 位相要素ループ
    /// </summary>
    public class Loop
    {
        #region Properties

        /// <summary>
        /// ホスト (フェイス)
        /// </summary>
        public Face Host
        {
            get;
            internal set;
        }

        /// <summary>
        /// エッジの環を列挙します
        /// </summary>
        public IEnumerable<Edge> EdgesRing
        {
            get {
                var looper = First;
                do {
                    yield return looper.HostEdge;
                    looper = looper.Next;
                } while (looper != First);
            }
        }

        /// <summary>
        /// 頂点の環を列挙します
        /// </summary>
        public IEnumerable<Vertex> VerticesRing
        {
            get {
                if (Isolated != null)
                    yield return Isolated;
                else {
                    var looper = First;
                    var vStart = looper.HostEdge.GetStartVertex( this );
                    yield return vStart;

                    var vNext = vStart;
                    do {
                        vNext = looper.HostEdge.GetOppositeVertex( vNext );
                        yield return vNext;
                        looper = looper.Next;
                    } while (looper.Next != First);
                }
                yield break;
            }
        }

        #endregion  // Properties

        #region Methods

        /// <summary>
        /// インスタンスを生成します (継承用)
        /// </summary>
        /// <returns></returns>
        public virtual Loop New() => new Loop();

        protected Loop()
        {
        }

        #endregion  // Methods

        #region Fields

        internal HalfEdge First;

        internal Vertex Isolated;

        #endregion  // Fields
    }
}
