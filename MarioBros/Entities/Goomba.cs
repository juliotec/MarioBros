using System;
using System.Drawing;
using MarioBros.Engine;

namespace MarioBros.Entities
{
    public class Goomba : BaseEntity, IGravity
    {
        #region Fields

        private GoombaState _state;
        private int _miliseconsDying;

        #endregion
        #region Constructors

        public Goomba(Resources resources, BaseObject obj)
        {
            if (resources.Map == null || resources.SpriteSheet == null)
            {
                throw new NullReferenceException();
            }

            var recSize = new Size(resources.Map.TileWidth, resources.Map.TileHeight);

            Image = resources.SpriteSheet;
            SourceRecNormal = CreateRectangles(recSize, new Point(0, 480), new Point(32, 480));
            SourceRecDying = CreateRectangles(recSize, new Point(64, 480));
            MapPosition = new PointF(obj.X, obj.Y - resources.Map.TileHeight);
            Velocity = new PointF(-2, 0);
            FPS = 6;
            State = GoombaState.Normal;
        }

        #endregion
        #region Properties

        private Rectangle[]? SourceRecNormal { get; set; }
        private Rectangle[]? SourceRecDying { get; set; }

        public GoombaState State
        {
            get { return _state; }
            set
            {
                _state = value;
                ResetAnimation();
                SourceRectangles = value == GoombaState.Normal ? SourceRecNormal : SourceRecDying;
            }
        }

        #endregion
        #region BaseEntity

        public override void CheckCollision(BaseEntity obj, PointF prevPosition)
        {
            if (State == GoombaState.Dying)
            {
                return;
            }

            if (obj is Mario mario)
            {
                if (new PointF(obj.MapPosition.X - prevPosition.X, obj.MapPosition.Y - prevPosition.Y).Y != 0) // si existe solo colicion vertical
                {
                    State = GoombaState.Dying;
                    Velocity = PointF.Empty;
                    mario.ActionState = MarioAction.Jump;
                    mario.Velocity = new PointF(obj.Velocity.X, -15); // rebota mario bros
                }
                else
                {
                    mario.Kill(); // asesina a mario
                }
            }
            else if (obj is Box || obj is Brick)
            {
                MapPosition = new PointF(MapPosition.X, obj.MapPosition.Y - obj.SourceRectangle.Height);
            }
            else
            {
                obj.Velocity = new PointF(-obj.Velocity.X, obj.Velocity.Y); // cambienza a caminar en direccion contraria
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (State == GoombaState.Dying)
            {
                // contador antes de desaparecer
                _miliseconsDying += gameTime.FrameMilliseconds;

                if (_miliseconsDying >= 1000)
                {
                    Removing = true;
                }
            }

            base.Update(gameTime);
        }

        #endregion
    }
}
