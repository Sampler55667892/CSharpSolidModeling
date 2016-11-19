using System.Collections.Generic;
using System.Linq;

namespace Solid
{
    // dE = 1, dL = 1, dF = 1

    internal class Mef
    {
        // TODO : Tie up the loose ends (Hole loop)
        public Face Do( Vertex startV, Vertex endV, Edge prevLE, Edge prevRE, Edge nextLE, Edge nextRE,
            out Edge newE, out Loop oldL, out Loop newL )
        {
            newE = null;
            oldL =
            newL = null;

            // 分離可能なループを探す
            var separableL = FindSeparableLoop( prevLE, prevRE, nextLE, nextRE );
            if (separableL == null)
                throw new System.Exception( "[MEF.cs/Do] 分離可能なループが見つかりませんでした" );
            oldL = separableL;

            var hostShell = separableL.Host.Host;

            newE = Archetype.NewEdge();
            newE.ConnectStart( startV );
            newE.ConnectEnd( endV );

            var newF = Archetype.NewFace();
            newL = Archetype.NewLoop();

            // Link
            hostShell.Connect( newF );
            newF.ConnectFrame( newL );

            // ハーフエッジの Link を編集
            EditLinksOfHalfEdges( newE, prevLE, prevRE, nextLE, nextRE );

            // ループの更新 (Left を新規にする)
            newE.Left.HostLoop = newL;
            newE.Right.HostLoop = separableL;
            UpdateNewLoopLinks( newE.Left, newL );

            // ホールがある場合はホールループの振分けを幾何演算を使って行う
            //...

            return newF;
        }

        Loop FindSeparableLoop( Edge prevLE, Edge prevRE, Edge nextLE, Edge nextRE )
        {
            // prev 側のループを集める
            var prevLoops = new HashSet<Loop>();
            foreach (var loop in new Loop[] { prevLE.Left.HostLoop, prevLE.Right.HostLoop,
                prevRE.Left.HostLoop, prevRE.Right.HostLoop }) {
                prevLoops.Add( loop );
            }

            // next 側のループを集める
            var nextLoops = new HashSet<Loop>();
            foreach (var loop in new Loop[] { nextLE.Left.HostLoop, nextLE.Right.HostLoop,
                nextRE.Left.HostLoop, nextRE.Right.HostLoop }) {
                nextLoops.Add( loop );
            }

            // 共通するループを特定する
            prevLoops.IntersectWith( nextLoops );
            if (prevLoops.Count != 1)
                return null;

            return prevLoops.First();
        }

        #region EditLinksOfHalfEdges

        void EditLinksOfHalfEdges( Edge newE, Edge prevLE, Edge prevRE, Edge nextLE, Edge nextRE )
        {
            var lHalf = new HalfEdge();
            var rHalf = new HalfEdge();
            newE.ConnectLeft( lHalf );
            newE.ConnectRight( rHalf );

            // 始点側
                // 　　　◇
                // 　　↑↑
                // 　　■｜■
                // 　　　｜↓
                // ―――○―――
            EditLinksOfHalfEdges_PrevSide( newE.Start, lHalf, rHalf, prevLE, prevRE );

            // 終点側
                // ―――◇―――
                // 　　↑↑
                // 　　■｜■
                // 　　　｜↓
                // 　　　○
            EditLinksOfHalfEdges_NextSide( newE.End, lHalf, rHalf, nextLE, nextRE );
        }

        void EditLinksOfHalfEdges_PrevSide( Vertex startV, HalfEdge lHalf, HalfEdge rHalf,
            Edge prevLE, Edge prevRE )
        {
            if (prevLE == prevRE) {
                if (prevLE.End == startV) {
                    // 　◇
                    // ↑↑
                    // ■｜■
                    // 　｜↓
                    // 　○
                    // ↑↑
                    // Ｌ｜Ｒ
                    // 　｜↓
                    lHalf.Prev = prevLE.Left;
                    prevLE.Left.Next = lHalf;
                    rHalf.Next = prevLE.Right;
                    prevLE.Right.Prev = rHalf;
                } else if (prevLE.Start == startV) {
                    // 　◇
                    // ↑↑
                    // ■｜■
                    // 　｜↓
                    // 　○
                    // ↑｜
                    // Ｌ｜Ｒ
                    // 　↓↓
                    lHalf.Prev = prevLE.Right;
                    prevLE.Right.Next = lHalf;
                    rHalf.Next = prevLE.Left;
                    prevLE.Left.Prev = rHalf;
                } else
                    throw new System.Exception( "[MEF.cs/EditLinksOfHalfEdges_PrevSide] Link がおかしい" );
            } else {
                if (prevLE.End == startV && prevRE.Start == startV) {
                    // 　　　◇
                    // 　　↑↑
                    // 　　■｜■
                    // 　Ｌ→｜↓Ｌ→
                    // ――→○――→
                    lHalf.Prev = prevLE.Left;
                    prevLE.Left.Next = lHalf;
                    rHalf.Next = prevRE.Left;
                    prevRE.Left.Prev = rHalf;
                } else if (prevLE.End == startV && prevRE.End == startV) {
                    // 　　　◇
                    // 　　↑↑
                    // 　　■｜■
                    // 　Ｌ→｜↓Ｒ→
                    // ――→○←――
                    lHalf.Prev = prevLE.Left;
                    prevLE.Left.Next = lHalf;
                    rHalf.Next = prevRE.Right;
                    prevRE.Right.Prev = rHalf;
                } else if (prevLE.Start == startV && prevRE.Start == startV) {
                    // 　　　◇
                    // 　　↑↑
                    // 　　■｜■
                    // 　Ｒ→｜↓Ｌ→
                    // ←――○――→
                    lHalf.Prev = prevLE.Right;
                    prevLE.Right.Next = lHalf;
                    rHalf.Next = prevRE.Left;
                    prevRE.Left.Prev = rHalf;
                } else if (prevLE.Start == startV && prevRE.End == startV) {
                    // 　　　◇
                    // 　　↑↑
                    // 　　■｜■
                    // 　Ｒ→｜↓Ｒ→
                    // ←――○←――
                    lHalf.Prev = prevLE.Right;
                    prevLE.Right.Next = lHalf;
                    rHalf.Next = prevRE.Right;
                    prevRE.Right.Prev = rHalf;
                } else
                    throw new System.Exception( "[MEF.cs/EditLinksOfHalfEdges_PrevSide] Link がおかしい" );
            }
        }

        void EditLinksOfHalfEdges_NextSide( Vertex endV, HalfEdge lHalf, HalfEdge rHalf,
            Edge nextLE, Edge nextRE )
        {
            if (nextLE == nextRE) {
                if (nextLE.Start == endV) {
                    // ↑↑
                    // Ｌ｜Ｒ
                    // 　｜↓
                    // 　◇
                    // ↑↑
                    // ■｜■
                    // 　｜↓
                    // 　○
                    lHalf.Next = nextLE.Left;
                    nextLE.Left.Prev = lHalf;
                    rHalf.Prev = nextLE.Right;
                    nextLE.Right.Next = rHalf;
                } else if (nextLE.End == endV) {
                    // ↑｜
                    // Ｒ｜Ｌ
                    // 　↓↓
                    // 　◇
                    // ↑↑
                    // ■｜■
                    // 　｜↓
                    // 　○
                    lHalf.Next = nextLE.Right;
                    nextLE.Right.Prev = lHalf;
                    rHalf.Prev = nextLE.Left;
                    nextLE.Left.Next = rHalf;
                } else
                    throw new System.Exception( "[MEF.cs/EditLinksOfHalfEdges_NextSide] Link がおかしい" );
            } else {
                if (nextLE.End == endV && nextRE.Start == endV) {
                    // ――→◇――→
                    // ←Ｒ↑↑←Ｒ
                    // 　　■｜■
                    // 　　　｜↓
                    // 　　　○
                    lHalf.Next = nextLE.Right;
                    nextLE.Right.Prev = lHalf;
                    rHalf.Prev = nextRE.Right;
                    nextRE.Right.Next = rHalf;
                } else if (nextLE.End == endV && nextRE.End == endV) {
                    // ――→◇←――
                    // ←Ｒ↑↑←Ｌ
                    // 　　■｜■
                    // 　　　｜↓
                    // 　　　○
                    lHalf.Next = nextLE.Right;
                    nextLE.Right.Prev = lHalf;
                    rHalf.Prev = nextRE.Left;
                    nextRE.Left.Next = rHalf;
                } else if (nextLE.Start == endV && nextRE.Start == endV) {
                    // ←――◇――→
                    // ←Ｌ↑↑←Ｒ
                    // 　　■｜■
                    // 　　　｜↓
                    // 　　　○
                    lHalf.Next = nextLE.Left;
                    nextLE.Left.Prev = lHalf;
                    rHalf.Prev = nextRE.Right;
                    nextRE.Right.Next = rHalf;
                } else if (nextLE.Start == endV && nextRE.End == endV) {
                    // ←――◇←――
                    // ←Ｌ↑↑←Ｌ
                    // 　　■｜■
                    // 　　　｜↓
                    // 　　　○
                    lHalf.Next = nextLE.Left;
                    nextLE.Left.Prev = lHalf;
                    rHalf.Prev = nextRE.Left;
                    nextRE.Left.Next = rHalf;
                } else
                    throw new System.Exception( "[MEF.cs/EditLinksOfHalfEdges_NextSide] Link がおかしい" );
            }
        }

        #endregion  // EditLinksOfHalfEdges

        void UpdateNewLoopLinks( HalfEdge first, Loop newL )
        {
            newL.First = first;

            var current = first;
            do {
                current = current.Next;
                current.HostLoop = newL;
            } while (current != first);
        }
    }
}
