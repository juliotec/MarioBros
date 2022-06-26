using System.Drawing;
using MarioBros.Core;

namespace MarioBros.Entities
{
    public class Koopa : BaseEntity
    {
        #region Constructors

        public Koopa(Resources resources, BaseObject obj)
        {
            Image = resources.SpriteSheet;
            SourceRecNormal = CreateRectangles(new Size(resources.Map.TileWidth, resources.Map.TileHeight * 2), new Point(224, 384), new Point(256, 384));
            SourceRectangles = SourceRecNormal;
            Velocity = new PointF(-2, 0);
            FPS = 6;
            MapPosition = new PointF(obj.X, (int)obj.Y - resources.Map.TileHeight);
        }

        #endregion
        #region Properties

        private Rectangle[] SourceRecNormal { get; set; }

        #endregion

    }
}
