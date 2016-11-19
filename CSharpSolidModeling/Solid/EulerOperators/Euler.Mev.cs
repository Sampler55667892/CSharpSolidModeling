using System.Diagnostics;
using Mathematics.Geometry;

namespace Solid
{
    // dV = 1, dE = 1

    internal class Mev
    {
        #region SpurEv

        public Vertex SpurEv( Vector3d vPos, Vertex startV, Edge prevLE, Edge prevRE, out Edge newE )
        {
            var endV = Archetype.NewVertex();
            endV.Position = vPos;
            newE = Archetype.NewEdge();
            newE.ConnectStart( startV );
            newE.ConnectEnd( endV );

            if (startV.Isolated != null) {
                // 　　　　　　 ■→
                // ◇  ==>  ◇――→●
                // 　　　　　 ←■

                var lHalf = new HalfEdge { HostLoop = startV.Isolated };
                var rHalf = new HalfEdge { HostLoop = startV.Isolated };
                newE.ConnectLeft( lHalf );
                newE.ConnectRight( rHalf );

                // Make link of halfedge
                lHalf.Next =
                lHalf.Prev = rHalf;
                rHalf.Next =
                rHalf.Prev = lHalf ;
                // Link : loop -> halfedge
                lHalf.HostLoop.First = lHalf;
                // Reset Isolated
                startV.Isolated.Isolated = null;
                startV.Isolated = null;
            } else {
                Debug.Assert( prevLE != null && prevRE != null && prevLE.Contains( startV ) );

                if (prevLE == prevRE) {
                    var lHalf = new HalfEdge { HostLoop = prevLE.Right.HostLoop };
                    var rHalf = new HalfEdge { HostLoop = prevLE.Right.HostLoop };
                    newE.ConnectLeft( lHalf );
                    newE.ConnectRight( rHalf );

                    lHalf.Next = rHalf;
                    rHalf.Prev = lHalf ;

                    // Halfedge はそのリンクが自然になるようにつなげる
                    if (prevLE.Start == startV) {
                        // 　　Ｒ→　　　　　 　Ｒ→　　■→
                        // ○←――◇　==>　○←――◇――→●
                        // 　←Ｌ　　　　　　 ←Ｌ　　←■
                        // Make link of halfedge
                        lHalf.Prev = prevLE.Right;
                        prevLE.Right.Next = lHalf;
                        rHalf.Next = prevLE.Left;
                        prevLE.Left.Prev = rHalf;
                    } else {    // prevLE.End == startV
                        // 　　Ｌ→　　　　　　 Ｌ→　　■→
                        // ○――→◇　==>　○――→◇――→●
                        // 　←Ｒ　　　　　　 ←Ｒ　　←■
                        // Make link of halfedge
                        lHalf.Prev = prevLE.Left;
                        prevLE.Left.Next = lHalf;
                        rHalf.Next = prevLE.Right;
                        prevLE.Right.Prev = rHalf;
                    }
                } else {
                    Debug.Assert( prevRE.Contains( startV ) );

                    var lHalf = new HalfEdge();
                    var rHalf = new HalfEdge();
                    newE.ConnectLeft( lHalf );
                    newE.ConnectRight( rHalf );

                    lHalf.Next = rHalf;
                    rHalf.Prev = lHalf;

                    if (prevRE.Start == startV) {
                        // 　　　　　　Ｌ→
                        // ○―――◇――→○
                        // 　　　　　←Ｒ
                        lHalf.HostLoop =
                        rHalf.HostLoop = prevRE.Left.HostLoop;
                        if (prevLE.Start == startV) {
                            // 　　　　　　　　　　   　　　　　●
                            // 　　　　　　　　　　   　　　　↑↑
                            // 　　　　　　　　　　   　　　　■｜■
                            // 　　　　　　　　　　   　　　　　｜↓
                            // 　　Ｒ→　　Ｌ→　　   　　　Ｒ→｜　Ｌ→
                            // ○←――◇――→○　==>　○←――◇――→○
                            // 　←Ｌ　　←Ｒ　　　   　　←Ｌ　　←Ｒ
                            // Make link of halfedge
                            lHalf.Prev = prevLE.Right;
                            prevLE.Right.Next = lHalf;
                            rHalf.Next = prevRE.Left;
                            prevRE.Left.Prev = rHalf;
                        } else {
                            // 　　　　　　　　　　   　　　　　●
                            // 　　　　　　　　　　   　　　　↑↑
                            // 　　　　　　　　　　   　　　　■｜■
                            // 　　　　　　　　　　   　　　　　｜↓
                            // 　　Ｌ→　　Ｌ→　　   　　　Ｌ→｜　Ｌ→
                            // ○――→◇――→○　==>　○――→◇――→○
                            // 　←Ｒ　　←Ｒ　　　   　　←Ｒ　　←Ｒ
                            // Make link of halfedge
                            lHalf.Prev = prevLE.Left;
                            prevLE.Left.Next = lHalf;
                            rHalf.Next = prevRE.Left;
                            prevRE.Left.Prev = rHalf;
                        }
                    } else {
                        // 　　　　　　Ｒ→
                        // ○―――◇←――○
                        // 　　　　　←Ｌ
                        lHalf.HostLoop =
                        rHalf.HostLoop = prevRE.Right.HostLoop;
                        if (prevLE.Start == startV) {
                            // 　　　　　　　　　　   　　　　　●
                            // 　　　　　　　　　　   　　　　↑↑
                            // 　　　　　　　　　　   　　　　■｜■
                            // 　　　　　　　　　　   　　　　　｜↓
                            // 　　Ｒ→　　Ｒ→　　   　　　Ｒ→｜　Ｒ→
                            // ○←――◇←――○　==>　○←――◇←――○
                            // 　←Ｌ　　←Ｌ　　　   　　←Ｌ　　←Ｌ
                            // Make link of halfedge
                            lHalf.Prev = prevLE.Right;
                            prevLE.Right.Next = lHalf;
                            rHalf.Next = prevRE.Right;
                            prevRE.Right.Prev = rHalf;
                        } else {
                            // 　　　　　　　　　　   　　　　　●
                            // 　　　　　　　　　　   　　　　↑↑
                            // 　　　　　　　　　　   　　　　■｜■
                            // 　　　　　　　　　　   　　　　　｜↓
                            // 　　Ｌ→　　Ｒ→　　   　　　Ｌ→｜　Ｒ→
                            // ○――→◇←――○　==>　○――→◇←――○
                            // 　←Ｒ　　←Ｌ　　　   　　←Ｒ　　←Ｌ
                            // Make link of halfedge
                            lHalf.Prev = prevLE.Left;
                            prevLE.Left.Next = lHalf;
                            rHalf.Next = prevRE.Right;
                            prevRE.Right.Prev = rHalf;
                        }
                    }
                }
            }

            return endV;
        }

        #endregion  // SpurEv

        #region SplitE

        public Vertex SplitE( Vector3d vPos, Edge targetE, bool newVIsEnd, out Edge newE )
        {
            var middleV = Archetype.NewVertex();
            middleV.Position = vPos;
            newE = Archetype.NewEdge();

            var lHalf = new HalfEdge { HostLoop = targetE.Left .HostLoop };
            var rHalf = new HalfEdge { HostLoop = targetE.Right.HostLoop };

            newE.ConnectLeft( lHalf );
            newE.ConnectRight( rHalf );

            if (newVIsEnd) {
                // 　　Ｌ→　　　   　　　■→　　Ｌ→
                // ◇―――→○　==>　◇――→●――→○
                // 　←Ｒ　　　　   　　←■　　←Ｒ
                var originalStart = targetE.Start;
                originalStart.Disconnect( targetE );
                targetE.ConnectStart( middleV );
                newE.ConnectStart( originalStart );
                newE.ConnectEnd( middleV );
                // Make link of halfedge
                lHalf.Prev = targetE.Left.Prev;    // Left
                targetE.Left.Prev.Next = lHalf;
                targetE.Left.Prev = lHalf;
                lHalf.Next = targetE.Left;
                rHalf.Next = targetE.Right.Next;    // Right
                targetE.Right.Next.Prev = rHalf;
                targetE.Right.Next = rHalf;
                rHalf.Prev = targetE.Right;
            } else {
                // 　　Ｌ→　　　   　　　Ｌ→　　■→
                // ◇―――→○　==>　◇――→●――→○
                // 　←Ｒ　　　　   　　←Ｒ　　←■
                var originalEnd = targetE.End;
                originalEnd.Disconnect( targetE );
                targetE.ConnectEnd( middleV );
                newE.ConnectStart( middleV );
                newE.ConnectEnd( originalEnd );
                // Make link of halfedge
                lHalf.Next = targetE.Left.Next;    // Left
                targetE.Left.Next.Prev = lHalf;
                targetE.Left.Next = lHalf;
                lHalf.Prev = targetE.Left;
                rHalf.Prev = targetE.Right.Prev;    // Right
                targetE.Right.Prev.Next = rHalf;
                targetE.Right.Prev = rHalf;
                rHalf.Next = targetE.Right;
            }

            return middleV;
        }

        #endregion SplitE
    }
}
