namespace MarioBros.Engine
{
    /// <summary>
    /// Direccion hacia donde mira el personaje
    /// </summary>
    public enum Direction
    {
        Right,
        Left
    }

    public enum BoxState
    {
        Normal,
        Empty
    }

    public enum GameState
    {
        Playing, // juego en ejecucion
        Dying, // muestra la animacion de mario muriendo
        Wining, // muestra la animacion de mario ganando
    }

    // Diferentes tipos de acciones que puede realizar el personaje
    public enum MarioAction 
    {
        Idle,
        Walk,
        Die,
        Flag,
        Jump,
        Falling,
        Stop,
    }

    public enum GoombaState
    {
        Normal,
        Dying
    }
}
