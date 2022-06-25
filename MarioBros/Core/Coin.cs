using System.Drawing;
using MarioBros.Entities;

namespace MarioBros.Core
{
    public class Coin : Base, IGravity
    {
        #region Fields

        private PointF? _originalPosition;

        #endregion
        #region Constructors

        public Coin(Resources resources)
        {
            var _recSize = new Size(resources.Map.TileWidth, resources.Map.TileHeight);

            base.Image = resources.SpriteSheet;
            SourceRectangles = base.CreateRectangles(_recSize, new Point(256, 192), new Point(288, 192), new Point(320, 192), new Point(352, 192));
            Velocity = new PointF(0, -20);
            FPS = 6;
        }

        #endregion
        #region Base

        public override void Update(GameTime gameTime)
        {
            if (_originalPosition == null)
            {
                _originalPosition = MapPosition;
            }

            if (MapPosition.Y > _originalPosition.Value.Y)
            {
                Removing = true;
            }

            base.Update(gameTime);
        }

        #endregion
    }
}
