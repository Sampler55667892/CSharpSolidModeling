namespace Solid
{
    // 　　　○
    // 　　／
    // 　○
    // ↑↑
    // Ｌ｜Ｒ
    // 　｜↓
    // 　○

    internal class HalfEdge
    {
        internal Edge HostEdge;
        internal Loop HostLoop;

        internal HalfEdge Next;
        internal HalfEdge Prev;
    }
}
