using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MarioBros.Core
{
    /// <summary>
    /// Clase con la logica de dibujado
    /// </summary>
    public class DrawHandler : IDisposable
    {
        #region Constructores

        public DrawHandler(int width, int height)
        {
            BaseImage = new Bitmap(width, height);
            Graphics = Graphics.FromImage(BaseImage);
            Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Imagen base sobre la cual se dibujan las demas imagenes
        /// </summary>
        public Image BaseImage { get; private set; }

        /// <summary>
        /// Clase con funciones de dibujado
        /// </summary>
        private Graphics Graphics { get; set; }

        #endregion
        #region Methods

        /// <summary>
        /// Dibuja una imagen en pantalla
        /// </summary>
        /// <param name="image">Imagen a dibujar</param>
        /// <param name="position">Posicion de la imagen en pantalla</param>
        public void Draw(Image image, Point position)
        {
            Graphics.DrawImage(image, position.X, position.Y, image.Width, image.Height);
        }

        /// <summary>
        /// Dibuja una imagen en pantalla
        /// </summary>
        /// <param name="image">Imagen a dibujar</param>
        /// <param name="rectangle">porcion de la imagen a dibuar</param>
        /// <param name="position">Posicion de la imagen en pantalla</param>
        /// <param name="flip">Rota la imagen horizontalmente</param>
        public void Draw(Image image, Rectangle rectangle, Point position)
        {
            Graphics.DrawImage(image, position.X, position.Y, rectangle, GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Dibuja una imagen en pantalla
        /// </summary>
        /// <param name="image">Imagen a dibujar</param>
        /// <param name="rectangle">porcion de la imagen a dibuar</param>
        /// <param name="x">Posicion en x de la imagen en pantalla</param>
        /// <param name="y">Posicion en y de la imagen en pantalla</param>
        /// <param name="flip">Rota la imagen horizontalmente</param>
        public void Draw(Image image, Rectangle rectangle, int x, int y, bool flipH = false)
        {
            if (flipH)
            {
                var _image = new Bitmap(rectangle.Width, rectangle.Height); // obtengo la imagen del rectangulo

                using (var _graphics = Graphics.FromImage(_image))
                {
                    _graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    _graphics.DrawImage(image, 0, 0, rectangle, GraphicsUnit.Pixel);
                }

                _image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                Graphics.DrawImage(_image, x, y);
            }
            else
            {
                Graphics.DrawImage(image, x, y, rectangle, GraphicsUnit.Pixel);
            }
        }

        #endregion
        #region IDisposable

        public void Dispose()
        {
            Graphics.Dispose();
            BaseImage = null;
        }

        #endregion
    }
}
