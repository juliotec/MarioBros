using System;
using System.Drawing;

namespace MarioBros.Engine
{
    public class Brick : BaseEntity, IGravity
    {
        #region Fields

        private PointF? _originalPosition;

        #endregion
        #region Constructors

        public Brick(Resources resources, BaseObject obj)
        {
            if (resources.Map == null || resources.SpriteSheet == null)
            {
                throw new NullReferenceException();
            }

            var _recSize = new Size(resources.Map.TileWidth, resources.Map.TileHeight);

            Image = resources.SpriteSheet;
            SourceRecNormal = CreateRectangles(_recSize, new Point(224, 0));
            SourceRectangles = SourceRecNormal;
            _originalPosition = new PointF(obj.X, (int)obj.Y - resources.Map.TileHeight);
            MapPosition = _originalPosition.Value;
        }

        #endregion
        #region Properties

        private Rectangle[]? SourceRecNormal { get; set; }

        public override PointF MapPosition
        {
            get => base.MapPosition;
            set => base.MapPosition = _originalPosition == null ? default : new PointF(value.X, Math.Min(value.Y, _originalPosition.Value.Y));
        }

        #endregion
        #region BaseEntity

        public override void CheckCollision(BaseEntity obj, PointF prevPosition)
        {
            var difPosition = new PointF(obj.MapPosition.X - prevPosition.X, obj.MapPosition.Y - prevPosition.Y); // diferencia entre la posicion actual y anterior

            if (difPosition.Y > 0)
            {
                obj.Velocity = new PointF(obj.Velocity.X, 0);
                obj.MapPosition = new PointF(obj.MapPosition.X, MapPosition.Y - obj.SourceRectangle.Height);
            }
            else if (difPosition.Y < 0)
            {
                obj.Velocity = new PointF(obj.Velocity.X, 0);
                obj.MapPosition = new PointF(obj.MapPosition.X, MapPosition.Y + obj.SourceRectangle.Height);

                Velocity = new PointF(Velocity.X, -10);
            }
            else if (difPosition.X > 0)
            {
                obj.Velocity = new PointF(0, obj.Velocity.Y);
                obj.MapPosition = new PointF(MapPosition.X - obj.SourceRectangle.Width, obj.MapPosition.Y);
            }
            else if (difPosition.X < 0)
            {
                obj.Velocity = new PointF(0, obj.Velocity.Y);
                obj.MapPosition = new PointF(MapPosition.X + obj.SourceRectangle.Width, obj.MapPosition.Y);
            }
        }
    }
    #endregion
}
