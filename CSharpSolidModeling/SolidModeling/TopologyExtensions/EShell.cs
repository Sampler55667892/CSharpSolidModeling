using Mathematics.Geometry;
using Solid;

namespace SolidModeling
{
    public sealed class EShell : Shell
    {
        #region Methods

        /// <summary>
        /// インスタンスを生成します (継承用)
        /// </summary>
        /// <returns></returns>
        public override Shell New() => new EShell();

        internal EShell() :
            base()
        {
        }

        /// <summary>
        /// 各頂点の座標を行列変換します
        /// </summary>
        /// <param name="matrix"></param>
        public void Transform( Matrix4d matrix )
        {
            var vSet = base._VertexSet;
            if (vSet != null) {
                // 明示的に型指定 (var だとコンパイルエラー)
                foreach (EVertex v in vSet)
                    v.Transform( matrix );
            }
        }

        #endregion  // Methods
    }
}
