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

        #endregion
    }
}
