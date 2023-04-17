using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using MarioBros.Engine;

namespace MarioBros
{
    public partial class MainForm : Form
    {
        #region Fields

        private readonly GameTime _gameTime;
        /// <summary>
        /// Timer que refresca la imagen del juego
        /// </summary>
        private readonly Timer _timer;
        /// <summary>
        /// Recursos graficos del juego
        /// </summary>
        private readonly Resources? _resources;
        /// <summary>
        /// Manejador del Mapa
        /// </summary>
        private MapHandler? _mapHandler;

        #endregion
        #region Constructors

        public MainForm()
        {
            InitializeComponent();

            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");

            _resources = new Resources()
            {
                SpriteSheet = LoadImage($"{directory}/TileSet.png"),
                Map = JsonConvert.DeserializeObject<Map>(LoadText($"{directory}/Level_1_1.json"))
            };

            if (_resources.Map == null || _resources.Map.BackgroundColor == null)
            {
                throw new NullReferenceException();
            }

            _pictureBox.BackColor = ColorTranslator.FromHtml(_resources.Map.BackgroundColor);

            InitializeMap();

            _gameTime = new GameTime();
            _timer = new Timer
            {
                Interval = 1000 / 30 // 60 FPS (el intervalo no siempre se respeta en winforms)
            };
            _timer.Tick += TimerTick;
        }

        #endregion
        #region Methods

        /// <summary>
        /// Carga un texto
        /// </summary>
        /// <param name="path">ruta del archivo a cargar</param>
        /// <returns></returns>
        private static string LoadText(string path)
        {
            try
            {
                return new StreamReader(path).ReadToEnd().Trim();
            }
            catch
            {
                MessageBox.Show("Load File Error\n" + path);

                return string.Empty;
            }
        }

        /// <summary>
        /// Carga una imagen 
        /// </summary>
        /// <param name="path">ruta de la imagen a cargar</param>
        /// <returns></returns>
        private static Image? LoadImage(string path)
        {
            try
            {
                return Image.FromFile(path);
            }
            catch
            {
                MessageBox.Show("Load File Error\n" + path);

                return null;
            }
        }

        /// <summary>
        /// Carga el mapa
        /// </summary>
        private void InitializeMap()
        {
            if (_resources == null)
            {
                throw new NullReferenceException();
            }

            _mapHandler = new MapHandler(_resources, _pictureBox.Size);
            _mapHandler.Restart += (obj, e) => InitializeMap(); // reinicia el mapa
        }

        private void Update(GameTime gameTime)
        {
            if (_mapHandler == null)
            {
                throw new NullReferenceException();
            }

            _mapHandler.Update(gameTime);
        }

        /// <summary>
        /// Dibuja la grilla
        /// </summary>
        /// <param name="drawHandler"></param>
        private void Draw(DrawHandler drawHandler)
        {
            if (_mapHandler == null)
            {
                throw new NullReferenceException();
            }

            _mapHandler.Draw(drawHandler);
        }

        private void TimerTick(object? sender, EventArgs e)
        {
            var now = DateTime.Now;
            using var drawHandler = new DrawHandler(_pictureBox.Width, _pictureBox.Height);

            _gameTime.FrameMilliseconds = (int)(now - _gameTime.FrameDate).TotalMilliseconds;
            _gameTime.FrameDate = now;
            Application.DoEvents();
            Update(_gameTime);  // ejecuta logica propia del juego
            Draw(drawHandler);  // actualiza la imagen en cada cuadro
            _pictureBox.Image = drawHandler.BaseImage; // asigna la imagen del nuevo cuadro al picture box
        }

        private void MainFormKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    Keyboard.IsLeft = true;
                    break;
                case Keys.Right:
                    Keyboard.IsRight = true;
                    break;
                case Keys.Z:
                    Keyboard.IsTurbo = true;
                    break;
                case Keys.X:
                    Keyboard.IsJump = true;
                    break;
            }
        }

        private void MainFormKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    Keyboard.IsLeft = false;
                    break;
                case Keys.Right:
                    Keyboard.IsRight = false;
                    break;
                case Keys.Z:
                    Keyboard.IsTurbo = false;
                    break;
                case Keys.X:
                    Keyboard.IsJump = false;
                    break;
            }
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            _timer.Start(); // inicia el juego
        }

        #endregion
    }
}
