using System;
using System.Drawing;

namespace MarioBros.Core
{
    public class PositionEventArgs : EventArgs
    {
        #region Constructors

        public PositionEventArgs(PointF current, PointF previous)
        {
            Current = current;
            Previous = previous;
        }

        #endregion
        #region Properties

        public PointF Current { get; set; }
        public PointF Previous { get; set; }

        #endregion
    }
}
