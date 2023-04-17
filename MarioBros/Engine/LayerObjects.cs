using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MarioBros.Entities;

namespace MarioBros.Engine
{
    public class LayerObjects : Sprite
    {
        #region Fields

        private Size _canvasSize;

        #endregion
        #region Constructors

        public LayerObjects(Resources resources, Size canvasSize)
        {
            if (resources.Map == null || resources.SpriteSheet == null || resources.Map.Layers == null)
            {
                throw new NullReferenceException();
            }

            var objects = resources.Map.Layers.First(x => x.Name == "Objects").Objects;

            _canvasSize = canvasSize;
            Image = resources.SpriteSheet;
            MapObjectsNew = new List<BaseEntity>();
            MapObjects = new List<BaseEntity>();

            for (var i = 0; i < objects?.Count; i++)
            {
                switch (Convert.ToInt32(objects[i].Type))
                {
                    case 1:
                        Mario = new Mario(resources, objects[i]);
                        break;
                    case 2:
                        var box = new Box(resources, objects[i]);

                        box.DropCoin += (sender, e) =>
                        {
                            MapObjectsNew.Add(new Coin(resources)
                            {
                                MapPosition = box.MapPosition
                            }); 
                        };
                        MapObjects.Add(box);
                        break;
                    case 3:
                        MapObjects.Add(new Brick(resources, objects[i]));
                        break;
                    //case 4:
                    //    // hongo de vida
                    //    break;
                    case 5:
                        Flag = new Rectangle(objects[i].X, objects[i].Y - objects[i].Height, objects[i].Width, objects[i].Height);
                        break;
                    case 6:
                        MapObjects.Add(new Goomba(resources, objects[i]));
                        break;
                    case 7:
                        MapObjects.Add(new Koopa(resources, objects[i]));
                        break;
                }
            };

            if (Mario != null)
            {
                MapObjects.Add(Mario); // lo agrega como ultimo objeto del mapa
            }
        }

        #endregion
        #region Properties

        /// <summary>
        /// Personaje jugable mario bros
        /// </summary>
        public Mario? Mario { get; private set; }
        /// <summary>
        /// Ubicacion de la Bandera
        /// </summary>
        public Rectangle Flag { get; set; }
        /// <summary>
        /// Objetos del mapa
        /// </summary>
        public List<BaseEntity>? MapObjects { get; set; }
        /// <summary>
        /// Objetos nuevos que se deben agregar al mapa
        /// </summary>
        public List<BaseEntity>? MapObjectsNew { get; set; }

        #endregion
        #region Methods

        public void ValidColition(BaseEntity obj, PointF prevPosition)
        {
            var objects = MapObjects?.Where(x => !x.Equals(obj) && x.MapPositionRec.IntersectsWith(obj.MapPositionRec)).ToList();

            for (var i = 0; i < objects?.Count; i++)
            {
                objects[i].CheckCollision(obj, prevPosition);
            }
        }

        public void Update(GameTime gameTime)
        {
            for (var i = 0; i < MapObjects?.Count; i++)
            {
                if (MapObjects[i].Position.X + MapObjects[i].SourceRectangle.Width > 0 && MapObjects[i].Position.X <= _canvasSize.Width) // solo actualizo el objeto si se ve en pantalla
                {
                    MapObjects[i].Update(gameTime);
                }
            }
        }

        #endregion
        #region Sprite

        public override void Draw(DrawHandler drawHandler)
        {
            for (var i = 0; i < MapObjects?.Count; i++)
            {
                if (MapObjects[i].Position.X + MapObjects[i].SourceRectangle.Width > 0 && MapObjects[i].Position.X <= _canvasSize.Width) // solo dibujo el objeto si se ve en pantalla
                {
                    MapObjects[i].Draw(drawHandler);
                }
            }
        }

        #endregion
    }
}
