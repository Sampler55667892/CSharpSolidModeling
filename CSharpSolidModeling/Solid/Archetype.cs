namespace Solid
{
    public static class Archetype
    {
        #region Methods

        public static void Set( Shell shell, Face face, Loop loop, Edge edge, Vertex vertex )
        {
            shellArchetype  = shell ;
            faceArchetype   = face  ;
            loopArchetype   = loop  ;
            edgeArchetype   = edge  ;
            vertexArchetype = vertex;
        }

        public static Shell NewShell() => shellArchetype.New();

        public static Face NewFace() => faceArchetype.New();

        public static Loop NewLoop() => loopArchetype.New();

        public static Edge NewEdge() => edgeArchetype.New();

        public static Vertex NewVertex() => vertexArchetype.New();

        #endregion  // Methods

        #region Fields

        static Shell shellArchetype;
        static Face faceArchetype;
        static Loop loopArchetype;
        static Edge edgeArchetype;
        static Vertex vertexArchetype;

        #endregion  // Fields
    }
}
