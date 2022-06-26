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

        #endregion
        #region Constructors

        public MainForm()
        {
            InitializeComponent();
            Initialize();
            _gameTime = new GameTime();
            Keyboard = new Keyboard();
            _timer = new Timer
            {
                Interval = 1000 / 30 // 60 PFS (el intervalo no siempre se respeta en winforms)
            };
            _timer.Tick += (sender, e) =>
            {
                var now = DateTime.Now;

                _gameTime.FrameMilliseconds = (int)(now - _gameTime.FrameDate).TotalMilliseconds;
                _gameTime.FrameDate = now;
                Application.DoEvents();
                Update(_gameTime);  // ejecuta logica propia del juego
                Keyboard.Clear();

                using (DrawHandler drawHandler = new DrawHandler(Canvas.Width, Canvas.Height))
                {
                    Draw(drawHandler);    // Actualiza la imagen en cada cuadro
                    Canvas.Image = drawHandler.BaseImage; // asigna la imagen del nuevo cuadro al picture box
                }
            };
        }

        #endregion
        #region Properties

        /// <summary>
        /// Recursos graficos del juego
        /// </summary>
        public Resources Resources { get; set; }
        /// <summary>
        /// C
        /// </summary>
        public MapHandler MapHandler { get; set; }
        /// <summary>
        /// Informacion de teclas precionadas
        /// </summary>
        protected Keyboard Keyboard { get; set; }

        #endregion
        #region Methods

        /// <summary>
        /// Carga los recursos graficos del juego
        /// </summary>
        private void Initialize()
        {
            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");

            Resources = new Resources()
            {
                SpriteSheet = LoadImage($"{directory}/TileSet.png"),
                Map = JsonConvert.DeserializeObject<Map>(LoadText($"{directory}/Level_1_1.json"))
            };
            Canvas.BackColor = ColorTranslator.FromHtml(Resources.Map.BackgroundColor);
            InitializeMap();
        }

        /// <summary>
        /// Carga el mapa
        /// </summary>
        private void InitializeMap()
        {
            MapHandler = new MapHandler(Resources, Canvas.Size);
            MapHandler.Restart += (obj, e) => InitializeMap(); // reinicia el mapa
        }

        /// <summary>
        /// Carga un texto
        /// </summary>
        /// <param name="path">ruta del archivo a cargar</param>
        /// <returns></returns>
        private string LoadText(string path)
        {
            try
            {
                using (var sr = new StreamReader(path))
                {
                    return sr.ReadToEnd().Trim();
                }
            }
            catch
            {
                MessageBox.Show("Load File Error\n" + path);

                return null;
            }
        }

        /// <summary>
        /// Carga una imagen 
        /// </summary>
        /// <param name="path">ruta de la imagen a cargar</param>
        /// <returns></returns>
        private Image LoadImage(string path)
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

        private void Update(GameTime gameTime)
        {
            MapHandler.Update(gameTime);
        }

        /// <summary>
        /// Dibuja la grilla
        /// </summary>
        /// <param name="drawHandler"></param>
        private void Draw(DrawHandler drawHandler)
        {
            MapHandler.Draw(drawHandler);
        }

        private void MainFormKeyDown(object sender, KeyEventArgs e)
        {
            Keyboard.SetKey(e.KeyData);

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
            _timer.Start(); // inicia el juego
        }

        #endregion
    }
}
