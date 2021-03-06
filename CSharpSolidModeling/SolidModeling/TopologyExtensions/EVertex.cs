﻿using System.Linq;
using Mathematics.Geometry;
using Solid;

namespace SolidModeling
{
    public sealed class EVertex : Vertex
    {
        #region Methods

        public override Vertex New() => new EVertex();

        internal EVertex() :
            base()
        {
        }

        /// <summary>
        /// (this, v) の2つのバーテックスを持つエッジを見付けます
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Edge FindEdge( Vertex v ) =>
            base.Linked.FirstOrDefault( e => v == e.GetOppositeVertex( this ) );

        /// <summary>
        /// 頂点の座標を行列変換します
        /// </summary>
        /// <param name="matrix"></param>
        public void Transform( Matrix4d matrix ) =>
            base.Position = matrix * base.Position;

        #endregion  // Methods
    }
}
