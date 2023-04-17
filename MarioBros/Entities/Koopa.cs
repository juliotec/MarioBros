using System;
using System.Drawing;
using MarioBros.Engine;

namespace MarioBros.Entities
{
    public class Koopa : BaseEntity, IGravity
    {
        #region Constructors

        public Koopa(Resources resources, BaseObject obj)
        {
            if (resources.Map == null || resources.SpriteSheet == null)
            {
                throw new NullReferenceException();
            }

            Image = resources.SpriteSheet;
            SourceRecNormal = CreateRectangles(new Size(resources.Map.TileWidth, resources.Map.TileHeight * 2), new Point(224, 384), new Point(256, 384));
            SourceRectangles = SourceRecNormal;
            Velocity = new PointF(-2, 0);
            FPS = 6;
            MapPosition = new PointF(obj.X, obj.Y - resources.Map.TileHeight);
        }

        #endregion
        #region Properties

        private Rectangle[]? SourceRecNormal { get; set; }

        #endregion
    }
}
