using System;
using System.Drawing;

namespace MarioBros.Engine
{
    public class Box : BaseEntity, IGravity
    {
        #region Fields

        private PointF? _originalPosition;

        #endregion
        #region Constructors

        public Box(Resources resources, BaseObject obj)
        {
            if (resources.Map == null || resources.SpriteSheet == null)
            {
                throw new NullReferenceException();
            }

            var _recSize = new Size(resources.Map.TileWidth, resources.Map.TileHeight);

            Image = resources.SpriteSheet;            
            SourceRecNormal = CreateRectangles(_recSize, new Point(320, 0), new Point(320, 64), new Point(320, 128), new Point(320, 64), new Point(320, 0));
            SourceRecEmpty = CreateRectangles(_recSize, new Point(224, 64));
            State = BoxState.Normal;
            FPS = 8;
            _originalPosition = new PointF(obj.X, (int)obj.Y - resources.Map.TileHeight);
            MapPosition = _originalPosition.Value;
        }
        #endregion
        #region Events

        public event EventHandler? DropCoin;

        #endregion
        #region Properties

        private Rectangle[]? SourceRecNormal { get; set; }
        private Rectangle[]? SourceRecEmpty { get; set; }

        public BoxState State
        {
            get { return _state; }
            set
            {
                _state = value;
                SourceRectangles = value == BoxState.Normal ? SourceRecNormal : SourceRecEmpty;
                ResetAnimation();
            }
        }
        private BoxState _state;

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

                if (State == BoxState.Normal)
                {
                    State = BoxState.Empty;
                    DropCoin?.Invoke(this, EventArgs.Empty);
                    Velocity = new PointF(Velocity.X, -10);
                }
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

        #endregion
    }
}
