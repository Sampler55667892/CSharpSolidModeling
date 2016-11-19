using Solid;

namespace SolidModeling
{
    public sealed class ELoop : Loop
    {
        #region Methods

        public override Loop New() => new ELoop();

        internal ELoop() :
            base()
        {
        }

        #endregion  // Methods
    }
}
