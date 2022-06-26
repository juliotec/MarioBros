using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MarioBros.Core
{
    public class LayerTiles : Sprite
    {
        #region Constructors

        public LayerTiles(Resources resources, Size canvasSize)
        {
            List<int> columns;

            Position = Point.Empty;
            Image = resources.SpriteSheet;
            TileSize = new Size(resources.Map.TileWidth, resources.Map.TileHeight);
            Size = new Size(resources.Map.Width, resources.Map.Height);

            // Carga los tiles del mapa
            Tiles = new Dictionary<int, Rectangle>();
            Size tileSetSize = new Size(resources.SpriteSheet.Width / TileSize.Width, resources.SpriteSheet.Height / TileSize.Height);

            for (var i = 0; i < tileSetSize.Height; i++)
            {
                for (var j = 0; j < tileSetSize.Width; j++)
                {
                    Tiles.Add((tileSetSize.Width * i) + j + 1, new Rectangle(j * TileSize.Width, i * TileSize.Height, TileSize.Width, TileSize.Height));
                }
            }

            // Carga matriz de tiles
            var layerTiles = resources.Map.Layers.FirstOrDefault(x => x.Name == "Tiles");

            Matrix = new int[layerTiles.Height, layerTiles.Width];

            for (var i = 0; i < layerTiles.Height; i++)
            {
                columns = layerTiles.Data.Skip(i * layerTiles.Width).Take(layerTiles.Width).ToList();

                for (var j = 0; j < layerTiles.Width; j++)
                {
                    Matrix[i, j] = Convert.ToInt32(columns[j]);
                }
            }

            // Tamaño en cantidad de celdas que son visibles en la pantalla
            var viewPortSize = new Size((int)Math.Ceiling(canvasSize.Width / (float)TileSize.Width), (int)Math.Ceiling(canvasSize.Height / (float)TileSize.Height));
            
            ViewPort = new Size(Math.Min(viewPortSize.Width + 1, Size.Width), Math.Min(viewPortSize.Height, Size.Height));
        }

        #endregion
        #region Properties

        /// <summary>
        /// Diccionario con los tiles asociados a su respectivo ID
        /// </summary>
        private Dictionary<int, Rectangle> Tiles { get; set; }
        /// <summary>
        /// Matriz con la informacion grafica del mapa
        /// </summary>
        private int[,] Matrix { get; set; }
        /// <summary>
        /// Tamaño de la pantalla en celdas (celdas del mapa visibles)
        /// </summary>
        private Size ViewPort { get; set; }
        /// <summary>
        /// Tamaño del mapa en cantidad de celdas
        /// </summary>
        public Size Size { get; private set; }
        /// <summary>
        /// Tamaño de cada tile del mapa en pixeles
        /// </summary>
        public Size TileSize { get; private set; }

        #endregion
        #region Methods
        public override void Draw(DrawHandler drawHandler)
        {
            var startX = (int)Math.Floor((float)Position.X / TileSize.Width); // coordenada en x de la primera celda a dibujar
            var startY = (int)Math.Floor((float)Position.Y / TileSize.Height); // coordenada en y de la primera celda a dibujar

            for (var i = startX; i < startX + ViewPort.Width; i++)
            {
                for (var j = startY; j < startY + ViewPort.Height; j++)
                {
                    if (i >= 0 && i < Size.Width)
                    {
                        if (Matrix[j, i] != 0)
                        {
                            drawHandler.Draw(Image, Tiles[Matrix[j, i]], new Point((int)(i * TileSize.Width - Position.X), (int)(j * TileSize.Height - Position.Y)));
                        }
                    }
                }
            }
        }

        #endregion
    }
}
