using System.Drawing;

namespace MarioBros.Core
{
    /// <summary>
    /// Sprite que se dibujara en pantalla
    /// </summary>
    public class Sprite
    {
        #region Constructors
        public Sprite()
        {
            Visible = true;
        }

        /// <summary>
        /// Instancia al Sprite a dibujar
        /// </summary>
        /// <param name="image">Imagen a dibujar</param>
        /// <param name="position">Posicion en pantalla donde se dibujara</param>
        public Sprite(Image image, Point position)
        {
            Image = image;
            Position = position;
            Visible = true;
        }
        #endregion
        #region Properties

        /// <summary>
        /// Imagen a dibujar
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// Posicion en pantalla donde se dibujara la imagen
        /// </summary>
        public PointF Position { get; set; }

        /// <summary>
        /// Determina si se debe dibujar o no la imagen
        /// </summary>
        public bool Visible { get; set; }

        #endregion
        #region Methods

        /// <summary>
        /// Dibuja todos los sprites en pantalla
        /// </summary>
        /// <param name="baseImage">Imagen base a donde se dibujara</param>
        /// <param name="g">Clase con metodos de dibujado</param>
        public virtual void Draw(DrawHandler drawHandler)
        {
            if (Visible)
            {
                drawHandler.Draw(Image, new Point((int)Position.X, (int)Position.Y));
            }
        }

        #endregion
    }
}
