using System.Collections.Generic;

namespace Solid
{
    /// <summary>
    /// 位相要素フェイス
    /// </summary>
    public class Face
    {
        #region Properties

        /// <summary>
        /// ホスト (シェル)
        /// </summary>
        public Shell Host
        {
            get;
            internal set;
        }

        /// <summary>
        /// フレーム (ループ)
        /// </summary>
        public Loop Frame
        {
            get;
            internal set;
        }

        /// <summary>
        /// ホールがあるか？ (true -> ある, false -> ない)
        /// </summary>
        public bool HasHoles =>
            holes != null && holes.Count > 0;

        /// <summary>
        /// ホールを列挙します
        /// </summary>
        public IEnumerable<Loop> Holes => holes;

        #endregion  // Properties

        #region Methods

        /// <summary>
        /// フレームについて(相互)接続します (Face -> Loop, Loop -> Face)
        /// </summary>
        /// <param name="frame"></param>
        public void ConnectFrame( Loop frame )
        {
            this.Frame = frame;
            frame.Host = this;
        }

        /// <summary>
        /// ホールをつなげます
        /// </summary>
        /// <param name="hole"></param>
        /// <returns></returns>
        public bool ConnectHole( Loop hole )
        {
            if (holes == null)
                holes = new HashSet<Loop>();
            hole.Host = this;

            return holes.Add( hole );
        }

        /// <summary>
        /// インスタンスを生成します (継承用)
        /// </summary>
        /// <returns></returns>
        public virtual Face New() => new Face();

        protected Face()
        {
        }

        #endregion  // Methods

        #region Fields

        HashSet<Loop> holes;

        #endregion  // Fields
    }
}
