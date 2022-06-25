using System;
using System.Drawing;
using MarioBros.Entities;

namespace MarioBros.Core
{
    public class Mario : Base, IGravity
    {
        #region Fields

        #endregion
        #region Events

        public event EventHandler Died;

        #endregion
        #region Constructors

        public Mario(Resources resources, BaseObject obj)
        {
            var _recSize = new Size(resources.Map.TileWidth, resources.Map.TileHeight);

            base.Image = resources.SpriteSheet;
            SourceRecSmallStand = base.CreateRectangles(_recSize, new Point(320, 640));
            SourceRecSmallStop = base.CreateRectangles(_recSize, new Point(384, 640));
            SourceRecSmallWalk = base.CreateRectangles(_recSize, new Point(416, 640), new Point(320, 672), new Point(352, 672));
            SourceRecSmallJump = base.CreateRectangles(_recSize, new Point(384, 672));
            SourceRecSmallDead = base.CreateRectangles(_recSize, new Point(352, 640));
            SourceRecSmallFlag = base.CreateRectangles(_recSize, new Point(320, 704));
            ActionState = MarioAction.Idle;
            DirectionState = Direction.Right;
            FPS = 12;
            Velocity = PointF.Empty;
            MapPosition = new PointF(obj.X, (int)obj.Y - resources.Map.TileHeight);
        }
        #endregion
        #region Properties

        private Rectangle[] SourceRecSmallStop { get; set; }
        private Rectangle[] SourceRecSmallRun { get; set; }
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
            if (ActionState == MarioAction.Idle)
                SourceRectangles = SourceRecSmallStand;
            else if (ActionState == MarioAction.Walk)
                SourceRectangles = SourceRecSmallWalk;
            else if (ActionState == MarioAction.Stop)
                SourceRectangles = SourceRecSmallStop;
            else if (ActionState == MarioAction.Jump || ActionState == MarioAction.Falling)
                SourceRectangles = SourceRecSmallJump;
            else if (ActionState == MarioAction.Die)
                SourceRectangles = SourceRecSmallDead;
            else if (ActionState == MarioAction.Flag)
                SourceRectangles = SourceRecSmallFlag;

            ResetAnimation();
        }
        /// <summary>
        /// Interaccion con el personaje
        /// </summary>
        public void MoveCharacter()
        {
            if (ActionState == MarioAction.Die)
                return;

            float _aceleration = 0.2f;
            float _maxAceleration = 6f;

            #region TURBO
            if (Elements.Keyboard.Turbo) // duplico la velocidad cuando el personaje corre
            {
                _aceleration *= 2;
                _maxAceleration *= 2;
            }
            else if (Math.Abs(Velocity.X) > _maxAceleration)
            {
                // si dejo de usar turbo, desacelero
                Velocity = (Velocity.X < 0 ? new PointF(Velocity.X + _aceleration, Velocity.Y) : new PointF(Velocity.X - _aceleration, Velocity.Y));
                Velocity = new PointF((float)Math.Round(Velocity.X, 1), (float)Math.Round(Velocity.Y, 1)); // correccion
            }
            FPS = Math.Abs(Velocity.X) <= _maxAceleration / 2 ? 6 : 12; // acelero la animacion dependiendo la velocidad
            if (Elements.Keyboard.Turbo)
                FPS *= 2;
            #endregion

            #region RIGHT
            if (Elements.Keyboard.Right) // desplzaimiento hacia la derecha
            {
                if (Velocity.X < _maxAceleration)
                    Velocity = new PointF(Velocity.X + _aceleration, Velocity.Y);

                if ((ActionState != MarioAction.Jump && ActionState != MarioAction.Falling))
                {
                    if (DirectionState != Direction.Right)
                        DirectionState = Direction.Right;

                    if (Velocity.X <= 0)
                    {
                        if (ActionState != MarioAction.Stop)
                            ActionState = MarioAction.Stop;
                    }
                    else if (ActionState != MarioAction.Walk)
                        ActionState = MarioAction.Walk;
                }
            }
            #endregion

            #region LEFT
            if (Elements.Keyboard.Left) // desplamiento hacia la izquierda
            {
                if (Velocity.X > -_maxAceleration)
                    Velocity = new PointF(Velocity.X - _aceleration, Velocity.Y);

                if ((ActionState != MarioAction.Jump && ActionState != MarioAction.Falling))
                {
                    if (DirectionState != Direction.Left)
                        DirectionState = Direction.Left;

                    if (Velocity.X > 0)
                    {
                        if (ActionState != MarioAction.Stop)
                            ActionState = MarioAction.Stop;
                    }
                    else if (ActionState != MarioAction.Walk)
                        ActionState = MarioAction.Walk;
                }
            }
            #endregion

            #region JUMP
            if (Elements.Keyboard.Jump && (ActionState != MarioAction.Jump && ActionState != MarioAction.Falling))
            {
                ActionState = MarioAction.Jump;
                float _jAaceleration = Elements.Keyboard.Turbo ? 24 : 20;
                Velocity = new PointF(Velocity.X, -_jAaceleration);
            }

            if (ActionState == MarioAction.Falling && Velocity.Y == 0)
                ActionState = Velocity.X != 0 ? MarioAction.Walk : MarioAction.Stop;

            if (ActionState == MarioAction.Jump && Velocity.Y >= 0)
                ActionState = MarioAction.Falling;
            #endregion

            #region STOP WALK
            if (ActionState != MarioAction.Jump && !Elements.Keyboard.Right && !Elements.Keyboard.Left) // deja de caminar
            {
                float _velX = (Velocity.X > 0 ? -(_aceleration * 2) : Velocity.X < 0 ? (_aceleration * 2) : 0) + Velocity.X;
                if (Math.Abs(Velocity.X) < (_aceleration * 2)) _velX = 0;

                Velocity = new PointF((float)Math.Round(_velX, 2), Velocity.Y);
            }
            #endregion

            #region IDLE
            if (ActionState != MarioAction.Jump && ActionState != MarioAction.Falling && ActionState != MarioAction.Idle && Velocity.X == 0) // retorna a estado de espera
                ActionState = MarioAction.Idle;
            #endregion
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

        #region Update
        public override void Update(GameTime gameTime)
        {
            MoveCharacter();

            base.Update(gameTime);
        }
        #endregion

        #region Draw
        public override void Draw(DrawHandler drawHandler)
        {
            drawHandler.Draw(base.Image, base.SourceRectangle, (int)base.Position.X, (int)base.Position.Y, DirectionState == Direction.Left);
        }
        #endregion
    }
    public enum MarioAction // Diferentes tipos de acciones que puede realizar el personaje
    {
        Idle,   
        Walk,   
        Die,    
        Flag,   
        Jump,  
        Falling,
        Stop,   
    }
}
