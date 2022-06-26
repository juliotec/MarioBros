using System;
using System.Drawing;

namespace MarioBros.Core
{
    public class Mario : BaseEntity, IGravity
    {
        #region Events

        public event EventHandler Died;

        #endregion
        #region Constructors

        public Mario(Resources resources, BaseObject obj)
        {
            var recSize = new Size(resources.Map.TileWidth, resources.Map.TileHeight);

            Image = resources.SpriteSheet;
            SourceRecSmallStand = CreateRectangles(recSize, new Point(320, 640));
            SourceRecSmallStop = CreateRectangles(recSize, new Point(384, 640));
            SourceRecSmallWalk = CreateRectangles(recSize, new Point(416, 640), new Point(320, 672), new Point(352, 672));
            SourceRecSmallJump = CreateRectangles(recSize, new Point(384, 672));
            SourceRecSmallDead = CreateRectangles(recSize, new Point(352, 640));
            SourceRecSmallFlag = CreateRectangles(recSize, new Point(320, 704));
            ActionState = MarioAction.Idle;
            DirectionState = Direction.Right;
            FPS = 12;
            Velocity = PointF.Empty;
            MapPosition = new PointF(obj.X, obj.Y - resources.Map.TileHeight);
        }

        #endregion
        #region Properties

        private Rectangle[] SourceRecSmallStop { get; set; }
        private Rectangle[] SourceRecSmallStand { get; set; }
        private Rectangle[] SourceRecSmallWalk { get; set; }
        private Rectangle[] SourceRecSmallJump { get; set; }
        private Rectangle[] SourceRecSmallDead { get; set; }
        private Rectangle[] SourceRecSmallFlag { get; set; }

        public MarioAction ActionState
        {
            get { return _actionState; }
            set
            {
                _actionState = value;
                SetSourceRectangle();
            }
        }
        private MarioAction _actionState;

        /// <summary>
        /// Direccion hacia donde mira el personaje
        /// </summary>
        new public Direction DirectionState
        {
            get { return _directionState; }
            set
            {
                _directionState = value;
                SetSourceRectangle();
            }
        }
        private Direction _directionState;

        #endregion
        #region Methods

        /// <summary>
        /// Setea el valor del array de rectangulos que se va a dibujar
        /// </summary>
        private void SetSourceRectangle()
        {
            // dependiendo el estado nuevo del perosnaje reasigna el array de rectangulo que corresponda a SourceRectangle para que se dibuje correctamente
            switch (ActionState)
            {
                case MarioAction.Idle:
                    SourceRectangles = SourceRecSmallStand;
                    break;
                case MarioAction.Walk:
                    SourceRectangles = SourceRecSmallWalk;
                    break;
                case MarioAction.Stop:
                    SourceRectangles = SourceRecSmallStop;
                    break;
                case MarioAction.Jump:
                case MarioAction.Falling:
                    SourceRectangles = SourceRecSmallJump;
                    break;
                case MarioAction.Die:
                    SourceRectangles = SourceRecSmallDead;
                    break;
                case MarioAction.Flag:
                    SourceRectangles = SourceRecSmallFlag;
                    break;
            };
            
            ResetAnimation();
        }

        public override void Update(GameTime gameTime)
        {
            MoveCharacter();

            base.Update(gameTime);
        }

        public override void Draw(DrawHandler drawHandler)
        {
            drawHandler.Draw(Image, SourceRectangle, (int)Position.X, (int)Position.Y, DirectionState == Direction.Left);
        }

        /// <summary>
        /// Interaccion con el personaje
        /// </summary>
        public void MoveCharacter()
        {
            if (ActionState == MarioAction.Die)
            {
                return;
            }

            var aceleration = 0.2f;
            var maxAceleration = 6f;

            // TURBO
            // duplico la velocidad cuando el personaje corre
            if (Keyboard.IsTurbo) 
            {
                aceleration *= 2;
                maxAceleration *= 2;
            }
            else if (Math.Abs(Velocity.X) > maxAceleration)
            {
                // si dejo de usar turbo, desacelero
                Velocity = Velocity.X < 0 ? new PointF(Velocity.X + aceleration, Velocity.Y) : new PointF(Velocity.X - aceleration, Velocity.Y);
                Velocity = new PointF((float)Math.Round(Velocity.X, 1), (float)Math.Round(Velocity.Y, 1)); // correccion
            }

            FPS = Math.Abs(Velocity.X) <= maxAceleration / 2 ? 6 : 12; // acelero la animacion dependiendo la velocidad

            if (Keyboard.IsTurbo)
            {
                FPS *= 2;
            }

            // RIGHT
            // desplzaimiento hacia la derecha
            if (Keyboard.IsRight)
            {
                if (Velocity.X < maxAceleration)
                {
                    Velocity = new PointF(Velocity.X + aceleration, Velocity.Y);
                }

                if (ActionState != MarioAction.Jump && ActionState != MarioAction.Falling)
                {
                    if (DirectionState != Direction.Right)
                    {
                        DirectionState = Direction.Right;
                    }

                    if (Velocity.X <= 0)
                    {
                        if (ActionState != MarioAction.Stop)
                        {
                            ActionState = MarioAction.Stop;
                        }
                    }
                    else if (ActionState != MarioAction.Walk)
                    {
                        ActionState = MarioAction.Walk;
                    }
                }
            }

            // LEFT
            // desplamiento hacia la izquierda
            if (Keyboard.IsLeft)
            {
                if (Velocity.X > -maxAceleration)
                {
                    Velocity = new PointF(Velocity.X - aceleration, Velocity.Y);
                }

                if (ActionState != MarioAction.Jump && ActionState != MarioAction.Falling)
                {
                    if (DirectionState != Direction.Left)
                    {
                        DirectionState = Direction.Left;
                    }

                    if (Velocity.X > 0)
                    {
                        if (ActionState != MarioAction.Stop)
                        {
                            ActionState = MarioAction.Stop;
                        }
                    }
                    else if (ActionState != MarioAction.Walk)
                    {
                        ActionState = MarioAction.Walk;
                    }
                }
            }

            // JUMP
            if (Keyboard.IsJump && ActionState != MarioAction.Jump && ActionState != MarioAction.Falling)
            {
                ActionState = MarioAction.Jump;
                Velocity = new PointF(Velocity.X, -(Keyboard.IsTurbo ? 24 : 20));
            }

            if (ActionState == MarioAction.Falling && Velocity.Y == 0)
            {
                ActionState = Velocity.X != 0 ? MarioAction.Walk : MarioAction.Stop;
            }

            if (ActionState == MarioAction.Jump && Velocity.Y >= 0)
            {
                ActionState = MarioAction.Falling;
            }

            // STOP WALK
            // deja de caminar
            if (ActionState != MarioAction.Jump && !Keyboard.IsRight && !Keyboard.IsLeft)
            {
                float velX;
                
                if (Math.Abs(Velocity.X) < (aceleration * 2))
                {
                    velX = 0;
                }
                else
                {
                    velX = (Velocity.X > 0 ? -(aceleration * 2) : Velocity.X < 0 ? (aceleration * 2) : 0) + Velocity.X;
                }                

                Velocity = new PointF((float)Math.Round(velX, 2), Velocity.Y);
            }

            // IDLE
            // retorna a estado de espera
            if (ActionState != MarioAction.Jump && ActionState != MarioAction.Falling && ActionState != MarioAction.Idle && Velocity.X == 0)
            {
                ActionState = MarioAction.Idle;
            }
        }

        /// <summary>
        /// Mata a mario bros
        /// </summary>
        public void Kill()
        {
            ActionState = MarioAction.Die; // cambia el estado para mostrar el sprite correspondiente
            Velocity = new PointF(Velocity.X, -20); // cambia velocidad para mostrar el salto de muerte
            Died(this, EventArgs.Empty); // notifica al controlador del juego que mario murio para cambiar el estado del juego
        }

        #endregion
    }
}
