using System.Collections.Generic;

namespace Solid
{
    /// <summary>
    /// 位相要素シェル
    /// </summary>
    public partial class Shell
    {
        #region Properties

        /// <summary>
        /// フェイスを列挙します
        /// </summary>
        public IEnumerable<Face> Faces => faces;

        #endregion  // Properties

        #region Methods

        /// <summary>
        /// フェイスについて(相互)接続します (Shell -> Face, Face -> Shell)
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public bool Connect( Face f )
        {
            if (faces == null)
                faces = new HashSet<Face>();
            f.Host = this;

            return faces.Add( f );
        }

        /// <summary>
        /// フェイスの接続を切ります
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public bool Disconnect( Face f )
        {
            if (faces == null)
                return false;
            f.Host = null;

            return faces.Remove( f );
        }

        /// <summary>
        /// インスタンスを生成します (継承用)
        /// </summary>
        /// <returns></returns>
        public virtual Shell New() => new Shell();

        protected Shell()
        {
        }

        #endregion  // Methods

        #region Fields

        HashSet<Face> faces;

        #endregion  // Fields
    }
}
