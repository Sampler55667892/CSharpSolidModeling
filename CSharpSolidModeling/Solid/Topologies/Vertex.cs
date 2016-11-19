using System.Collections.Generic;
using Mathematics.Geometry;

namespace Solid
{
    /// <summary>
    /// 位相要素バーテックス
    /// </summary>
    public class Vertex
    {
        #region Properties

        /// <summary>
        /// バーテックスに接続しているエッジを列挙します
        /// </summary>
        public IEnumerable<Edge> Linked => this.linked;

        /// <summary>
        /// バーテックスに接続しているエッジの数を取得します
        /// </summary>
        public int CountLinked =>
            linked == null ? 0 : linked.Count;

        #endregion  // Properties

        #region Methods

        /// <summary>
        /// エッジを接続します
        /// </summary>
        /// <param name="e"></param>
        /// <returns>true -> エッジが接続した, false -> 既に接続していた</returns>
        public bool Connect( Edge e )
        {
            if (linked == null)
                linked = new List<Edge>();
            else if (linked.Contains( e ))
                return false;

            linked.Add( e );
            return true;
        }

        /// <summary>
        /// エッジの接続を切ります
        /// </summary>
        /// <param name="e"></param>
        /// <returns>true -> エッジの接続が切れた, false -> エッジの接続が既に切れていた</returns>
        public bool Disconnect( Edge e )
        {
            if (linked == null)
                return false;
            return linked.Remove( e );
        }

        /// <summary>
        /// インスタンスを生成します (継承用)
        /// </summary>
        /// <returns></returns>
        public virtual Vertex New() => new Vertex();

        protected Vertex()
        {
        }

        #endregion  // Methods

        #region Fields

        /// <summary>
        /// 位置
        /// </summary>
        public Vector3d Position;

        internal Loop Isolated;

        List<Edge> linked;

        #endregion  // Fields
    }
}
