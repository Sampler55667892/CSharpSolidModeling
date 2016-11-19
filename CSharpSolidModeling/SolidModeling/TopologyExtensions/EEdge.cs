using Mathematics.Geometry;
using Solid;

namespace SolidModeling
{
    public sealed class EEdge : Edge
    {
        #region Properties

        /// <summary>
        /// 幾何形状
        /// </summary>
        public Line Geometry => Line.FromStartEnd( base.Start.Position, base.End.Position );

        #endregion  // Properties

        #region Methods

        public override Edge New() => new EEdge();

        internal EEdge() :
            base()
        {
        }

        #endregion  // Methods
    }
}
