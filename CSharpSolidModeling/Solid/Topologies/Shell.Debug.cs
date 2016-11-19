using System.Collections.Generic;

namespace Solid
{
    // Euler-Poincare' formula
    // 　V - E + F - (L - F) - 2(S - G) = 0

    partial class Shell
    {
        public HashSet<Vertex> _VertexSet
        {
            get {
                var vertices = new HashSet<Vertex>();

                var loops = _LoopSet;
                foreach (var l in loops) {
                    if (l.Isolated != null) {
                        if (!vertices.Contains( l.Isolated ))
                            vertices.Add( l.Isolated );
                    } else {
                        foreach (var v in l.VerticesRing) {
                            if (!vertices.Contains( v ))
                                vertices.Add( v );
                        }
                    }
                }
                return vertices;
            }
        }

        public HashSet<Edge> _EdgeSet
        {
            get {
                var edges = new HashSet<Edge>();

                var loops = _LoopSet;
                foreach (var l in loops) {
                    if (l.Isolated != null)
                        continue;
                    foreach (var e in l.EdgesRing) {
                        if (!edges.Contains( e ))
                            edges.Add( e );
                    }
                }
                return edges;
            }
        }

        public HashSet<Loop> _LoopSet
        {
            get {
                var loops = new HashSet<Loop>();

                foreach (var f in this.Faces) {
                    if (!loops.Contains( f.Frame ))
                        loops.Add( f.Frame );
                    if (!f.HasHoles)
                        continue;
                    foreach (var h in f.Holes) {
                        if (!loops.Contains( h ))
                            loops.Add( h );
                    }
                }
                return loops;
            }
        }

        public string _GetEulerPoincareFormula()
        {
            int countVertices = this._VertexSet.Count;
            int countEdges = this._EdgeSet.Count;
            int countLoops = this._LoopSet.Count;
            int countFaces = this.faces.Count;

            return
                "V - E + F - (L - F) = " +
                countVertices.ToString() + " - " +
                countEdges.ToString() + " + " +
                countFaces.ToString() + " - (" +
                countLoops.ToString() + " - " +
                countFaces.ToString() + ") = " +
                (countVertices - countEdges + countFaces - (countLoops - countFaces)).ToString() +
                " (2S = 2)";
        }
    }
}
