using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MarioBros.Engine
{
    public class LayerObstacles
    {
        #region Constructors

        public LayerObstacles(Resources resources)
        {
            var layerTiles = resources.Map.Layers.FirstOrDefault(x => x.Name == "Obstacles");
            List<int> columns;

            TileSize = new Size(resources.Map.TileWidth, resources.Map.TileHeight);
            Size = new Size(resources.Map.Width, resources.Map.Height);            
            Matrix = new bool[layerTiles.Height, layerTiles.Width];

            for (var i = 0; i < layerTiles.Height; i++)
            {
                columns = layerTiles.Data.Skip(i * layerTiles.Width).Take(layerTiles.Width).ToList();

                for (var j = 0; j < layerTiles.Width; j++)
                {
                    Matrix[i, j] = Convert.ToInt32(columns[j]) != 0;
                }
            }
        }

        #endregion
        #region Properties

        /// <summary>
        /// Tamaño de cada tile del mapa en pixeles
        /// </summary>
        private Size TileSize { get; set; }
        /// <summary>
        /// Matriz con la informacion de obstaculos
        /// </summary>
        private bool[,] Matrix { get; set; }
        /// <summary>
        /// Tamaño del mapa en cantidad de celdas
        /// </summary>
        public Size Size { get; private set; }

        #endregion
        #region Methods

        private PointF GetPositionAdjust(RectangleF colArea, PointF difPosition)
        {
            float _x =
                difPosition.X < 0 ? colArea.Width :
                difPosition.X > 0 ? -colArea.Width :
                0;

            float _y =
                difPosition.Y < 0 ? colArea.Height :
                difPosition.Y > 0 ? -colArea.Height :
                0;

            return new PointF(_x, _y);
        }

        private bool HasTileColition(int row, int col)
        {
            if (col < 0 || col >= Size.Width)
            {
                return true;
            }

            if (row < 0 || row >= Size.Height)
            {
                return false;
            }

            return Matrix[row, col];
        }

        public void ValidColition(BaseEntity obj, PointF prevPosition)
        {
            if (obj == null)
            {
                return;
            }

            //rectangulo del elemento dentro del mapa
            var objRectangle = obj.MapPositionRec;
            var tileXMin = (int)Math.Floor(objRectangle.X / TileSize.Width);
            var tileXMax = (int)Math.Ceiling((objRectangle.X + obj.SourceRectangle.Width) / TileSize.Width) - 1;
            var tileYMin = (int)Math.Floor(objRectangle.Y / TileSize.Height);
            var tileYMax = (int)Math.Ceiling((objRectangle.Y + obj.SourceRectangle.Height) / TileSize.Height) - 1;

            for (var i = tileYMin; i <= tileYMax; i++)
            {
                for (var j = tileXMin; j <= tileXMax; j++)
                {
                    if (HasTileColition(i, j))
                    {
                        var area = RectangleF.Intersect(objRectangle, new RectangleF(j * (float)TileSize.Width, i * (float)TileSize.Height, TileSize.Width, TileSize.Height));

                        if (area.Width == 0 && area.Height == 0)
                        {
                            continue;
                        }

                        var difPosition = new PointF(obj.MapPosition.X - prevPosition.X, obj.MapPosition.Y - prevPosition.Y); // diferencia entre la posicion actual y anterior
                        var adjPosition = GetPositionAdjust(area, difPosition);

                        if (difPosition.Y != 0) // si existe solo colicion vertical
                        {
                            obj.FixMapPosition(0, adjPosition.Y);
                            obj.Velocity = new PointF(obj.Velocity.X, 0);

                            break;
                        }

                        if (difPosition.X != 0) // si existe colicion horizontal
                        {
                            obj.FixMapPosition(adjPosition.X, 0);

                            if (obj is Mario)
                            {
                                obj.Velocity = new PointF(0, obj.Velocity.Y);
                            }
                            else
                            {
                                obj.Velocity = new PointF(-obj.Velocity.X, obj.Velocity.Y);
                            }

                            break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
