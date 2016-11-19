using Mathematics.Geometry;
using Solid;

namespace SolidModeling
{
    public static class SolidModeler
    {
        #region Methods

        /// <summary>
        /// 直方体を生成します
        /// </summary>
        /// <remarks>
        /// 原点は (0, 0, 0)
        /// </remarks>
        /// <param name="size"></param>
        /// <returns></returns>
        public static EShell Cuboid( Vector3d size )
        {
            var o = new EulerOperator();

            // 　　　v4────v7
            // 　　／｜　　　／｜
            // 　／　｜　　／　｜
            // v5─e7──v6　　e10 　Ｚ
            // ｜　　｜　｜　　｜　　｜
            // ｜　　v0─｜e3─v3　　・─Ｙ
            // e6　／　　e8　／　　／
            // ｜／　　　｜／　　Ｘ
            // v1─e1──v2

            Vertex v0;
            Loop l;
            Face f;
            var shell = o.Mvfs( new Vector3d( 0, 0, 0 ), out v0, out l, out f );

            Edge e0;
            var v1 = o.MevSpurEv( new Vector3d( size.X, 0, 0 ), v0, null, null, out e0 );

            Edge e1;
            var v2 = o.MevSpurEv( new Vector3d( size.X, size.Y, 0 ), v1, e0, e0, out e1 );

            Edge e2;
            var v3 = o.MevSpurEv( new Vector3d( 0, size.Y, 0 ), v2, e1, e1, out e2 );

            Edge e3;
            Loop ol, nl;
            var f0 = o.Mef( v0, v3, e0, e0, e2, e2, out e3, out ol, out nl );

            Edge e4;
            var v4 = o.MevSpurEv( new Vector3d( 0, 0, size.Z ), v0, e3, e0, out e4 );

            Edge e5;
            var v5 = o.MevSpurEv( new Vector3d( size.X, 0, size.Z ), v4, e4, e4, out e5 );

            // 　e1　　　　e0
            // ←―― v1 ←――
            // 　　 ↑↑
            // 　　 Ｌ｜Ｒ
            // 　　　 ｜↓
            // 　　　 v5
            // 　　　 ↑
            // 　　　 ｜e5
            // 　　　 ｜
            Edge e6;
            var f1 = o.Mef( v5, v1, e5, e5, e1, e0, out e6, out ol, out nl );

            Edge e7;
            var v6 = o.MevSpurEv( new Vector3d( size.X, size.Y, size.Z ), v5, e5, e6, out e7 );

            // 　e2　　　　e1
            // ←―― v2 ←――
            // 　　 ↑↑
            // 　　 Ｌ｜Ｒ
            // 　　　 ｜↓
            // 　　　 v6
            // 　　　 ↑
            // 　　　 ｜e7
            // 　　　 ｜
            Edge e8;
            var f2 = o.Mef( v6, v2, e7, e7, e2, e1, out e8, out ol, out nl );

            Edge e9;
            var v7 = o.MevSpurEv( new Vector3d( 0, size.Y, size.Z ), v6, e7, e8, out e9 );

            // 　e3　　　　e2
            // ←―― v3 ←――
            // 　　 ↑↑
            // 　　 Ｌ｜Ｒ
            // 　　　 ｜↓
            // 　　　 v7
            // 　　　 ↑
            // 　　　 ｜e9
            // 　　　 ｜
            Edge e10;
            var f3 = o.Mef( v7, v3, e9, e9, e3, e2, out e10, out ol, out nl );

            // 　e5　　　　e4
            // ←―― v4 ←――
            // 　　 ↑↑
            // 　　 Ｌ｜Ｒ
            // 　e9　 ｜↓ e10
            // ――→ v7 ――→
            Edge e11;
            var f4 = o.Mef( v7, v4, e9, e10, e5, e4, out e11, out ol, out nl );

            return shell as EShell;
        }

        #endregion  // Methods
    }
}
