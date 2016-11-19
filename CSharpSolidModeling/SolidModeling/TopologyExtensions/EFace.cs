using System.Collections.Generic;
using Mathematics.Geometry;
using Solid;

namespace SolidModeling
{
    public sealed class EFace : Face
    {
        #region Properties

        /// <summary>
        /// 幾何形状
        /// </summary>
        public Polygon Geometry
        {
            get {
                var frameLoop = base.Frame;
                var points = new List<Vector3d>();

                foreach (var v in frameLoop.VerticesRing)
                    points.Add( v.Position );

                return new Polygon { Points = points.ToArray() };
            }
        }

        #endregion  // Properties

        #region Methods

        public override Face New() => new EFace();

        internal EFace() :
            base()
        {
        }

        #endregion  // Methods
    }
}
