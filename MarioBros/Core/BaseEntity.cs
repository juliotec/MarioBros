using System;
using System.Drawing;

namespace MarioBros.Core
{
    public class BaseEntity : Sprite
    {
        #region Fields
        
        private int _index = 0; // numero que indica el cuadro actual de la animacion que se dibujara
        private int _milisec = 0; // variable auxiliar que suma los milisegundos transcurridos en cada iteracion del metodo update 

        #endregion
        #region Events

        public event EventHandler<PositionEventArgs> MapPositionChanged;

        #endregion
        #region Properties

        /// <summary>
        /// Lista de rectangulos que componen la animacion actual
        /// </summary>
        protected Rectangle[] SourceRectangles { get; set; }

        /// <summary>
        /// Direccion hacia donde mira el personaje
        /// </summary>
        protected Direction DirectionState { get; set; }

        /// <summary>
        /// <summary>
        /// Velocidad del objeto al desplazase, ajusta la posicicion de este en el escenario
        /// </summary>
        public PointF Velocity { get; set; }

        /// <summary>
        /// Indica la velocidad en "cuadros por segundo" que tendran las animaciones del personaje
        /// </summary>
        public int FPS { get; set; } = 1;

        /// <summary>
        /// Rectangulo que ocupa el personaje en el mapa
        /// </summary>
        public RectangleF MapPositionRec { get; private set; }

        /// <summary>
        /// Indica que el objeto sera removido del mapa
        /// </summary>
        public bool Removing { get; set; }

        /// <summary>
        /// Rectangulo a dibujar en el cuadro actual
        /// </summary>
        public Rectangle SourceRectangle { get { return SourceRectangles[_index]; } }

        /// <summary>
        /// Posision del objeto en el mapa
        /// </summary>
        public virtual PointF MapPosition
        {
            get { return _mapPosition; }
            set
            {
                var positionEventArgs = new PositionEventArgs(value, _mapPosition);

                _mapPosition = value;

                if (positionEventArgs.Previous != positionEventArgs.Current)
                {
                    if (SourceRectangles != null)
                    {
                        MapPositionRec = new RectangleF(value.X, value.Y, SourceRectangle.Width, SourceRectangle.Height);
                    }

                    MapPositionChanged?.Invoke(this, positionEventArgs);
                }
                // desencadena el evento indicando que el objeto se desplazo en el mapa
            }
        }
        private PointF _mapPosition;

        #endregion
        #region Methods

        /// <summary>
        /// Reinicia la animacion
        /// </summary>
        protected void ResetAnimation()
        {
            _index = 0;
        }

        /// <summary>
        /// Crea la coleccion de rectangulos que componen una animacion
        /// </summary>
        /// <param name="size">tamaño de los rectangulos</param>
        /// <param name="locations">ubicacion de los rectangulos</param>
        /// <returns></returns>
        protected Rectangle[] CreateRectangles(Size size, params Point[] locations)
        {
            var rect = new Rectangle[locations.Length];

            for (var i = 0; i < locations.Length; i++)
            {
                rect[i] = new Rectangle(locations[i], size);
            }

            return rect;
        }

        public virtual void Update(GameTime gameTime)
        {
            Animation(gameTime);
        }

        /// <summary>
        /// Valida la colicion del con otro objeto
        /// </summary>
        /// <param name="obj"></param>
        public virtual void CheckCollision(BaseEntity obj, PointF prevPosition)
        {
        }

        /// <summary>
        /// Realiza la animacion del personaje
        /// </summary>
        /// <param name="gameTime"></param>
        public void Animation(GameTime gameTime)
        {
            _milisec += gameTime.FrameMilliseconds; // contador de tiempo en milisegundos (variable auxiliar)

            if (_milisec >= 1000 / FPS) // se calcula la demora que tendra el paso de cada cuadro de la animacion
            {
                _milisec = 0;

                if (_index < SourceRectangles.Length - 1)
                {
                    _index++; // si el el estado actual posee mas cuadros, adelanto 1
                }
                else
                {
                    _index = 0; // si el el estado actual NO posee mas cuadros, vuelvo al primero para hacer una animacion ciclica
                }
            }
        }

        /// <summary>
        /// Ajusta la posicion del objeto en el mapa sin desencadenar el evento
        /// </summary>
        /// <param name="x">ajuste en X</param>
        /// <param name="y">ajuste en Y</param>
        public void FixMapPosition(float x, float y)
        {
            _mapPosition = new PointF(_mapPosition.X + x, _mapPosition.Y + y);
        }

        #endregion
        #region Sprite

        public override void Draw(DrawHandler drawHandler)
        {
            drawHandler.Draw(Image, SourceRectangle, (int)Position.X, (int)Position.Y, DirectionState == Direction.Left);
        }

        #endregion
    }
}
