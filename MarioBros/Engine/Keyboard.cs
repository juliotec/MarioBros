using System.Windows.Forms;

namespace MarioBros.Engine
{
    /// <summary>
    /// Teclas habilitadas en el juego
    /// </summary>
    public class Keyboard
    {
        #region Fields

        public static bool IsLeft { get; set; }
        public static bool IsRight { get; set; }
        public static bool IsTurbo { get; set; }
        public static bool IsJump { get; set; }
        public bool Left { get; private set; }
        public bool Right { get; private set; }
        public bool Up { get; private set; }
        public bool Down { get; private set; }
        public bool Space { get; private set; }

        #endregion
        #region Methods

        public void SetKey(Keys key)
        {
            switch (key)
            {
                case Keys.Left:
                    Left = true;
                    break;
                case Keys.Right:
                    Right = true;
                    break;
                case Keys.Up:
                    Up = true;
                    break;
                case Keys.Down:
                    Down = true;
                    break;
                case Keys.Space:
                    Space = true;
                    break;
            }
        }

        /// <summary>
        /// Limpia las teclas seleccionada
        /// </summary>
        public void Clear()
        {
            Left = false;
            Right = false;
            Up = false;
            Down = false;
            Space = false;
        }

        #endregion
    }
}
