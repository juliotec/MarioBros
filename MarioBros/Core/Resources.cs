using System.Drawing;

namespace MarioBros.Core
{
    /// <summary>
    /// Clase que carga los recursos del juego
    /// </summary>
    public class Resources
    {
        #region Properties

        /// <summary>
        /// Bloque de la grilla Vacio
        /// </summary>
        public Image SpriteSheet { get; set; }
        public Map Map { get; set; }

        #endregion
    }
}
