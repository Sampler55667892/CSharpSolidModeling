using Mathematics.Geometry;

namespace Solid
{
    // dV = 1, dF = 1, dL = 1, dS = 1

    internal class Mvfs
    {
        public Shell Do( Vector3d position, out Vertex newVertex, out Loop newLoop, out Face newFace )
        {
            // インスタンス生成
            var newShell = Archetype.NewShell();
            newFace = Archetype.NewFace();
            newLoop = Archetype.NewLoop();
            newVertex = Archetype.NewVertex();

            // 座標設定
            newVertex.Position = position;

            // リンク
            newShell.Connect( newFace );
            newFace.ConnectFrame( newLoop );
            newLoop.Isolated = newVertex;
            newVertex.Isolated = newLoop;

            return newShell;
        }
    }
}
