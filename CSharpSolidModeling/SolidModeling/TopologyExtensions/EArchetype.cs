using Solid;

namespace SolidModeling
{
    public static class EArchetype
    {
        #region Methods

        /// <summary>
        /// SolidModeling.dll を使う場合はアプリケーションの最初で1度だけ呼出して下さい
        /// </summary>
        public static void Initialize() =>
            Archetype.Set(
                new EShell(), new EFace(), new ELoop(), new EEdge(), new EVertex() );

        #endregion  // Methods
    }
}
