using System;
using System.Collections.Generic;
using System.Drawing;

namespace MarioBros.Engine
{
    public class MapHandler
    {
        #region Fields

        private Size _canvasSize;
        private RectangleF _canvasRec;
        private readonly float _gravity = 2f;

        #endregion
        #region Events

        public event EventHandler Restart;

        #endregion
        #region Constructors

        public MapHandler(Resources resources, Size canvasSize)
        {
            _canvasSize = canvasSize;
            LayerTiles = new LayerTiles(resources, canvasSize);
            LayerObstacles = new LayerObstacles(resources);
            LayerObjects = new LayerObjects(resources, canvasSize);

            for (var i = 0; i < LayerObjects.MapObjects.Count; i++)
            {
                // atacha el evento de cambio de posiscion de los objetos en el mapa
                LayerObjects.MapObjects[i].MapPositionChanged += ObjectsMapPositionChanged;
            }

            LayerObjects.Mario.Died += MarioDied;
            State = GameState.Playing;

            // actualiza la posicion inicial de los objetos
            UpdateObjectPosition(); 
        }

        #endregion
        #region Properties

        /// <summary>
        /// Estado del juego en ejecucion
        /// </summary>
        public GameState State { get; set; }
        /// <summary>
        /// Informacion grafica del mapa
        /// </summary>
        public LayerTiles LayerTiles { get; set; }
        /// <summary>
        /// Informacion de obstaculos del mapa
        /// </summary>
        public LayerObstacles LayerObstacles { get; set; }
        /// <summary>
        /// Objetos del mapa
        /// </summary>
        public LayerObjects LayerObjects { get; set; }

        #endregion
        #region Methods

        private void UpdatePlaying(GameTime gameTime)
        {
            _canvasRec = new RectangleF(LayerTiles.Position.X, LayerTiles.Position.Y, _canvasSize.Width, _canvasSize.Height);

            var lstRemove = new List<BaseEntity>();

            for (var i = 0; i < LayerObjects.MapObjects.Count; i++)
            {
                if (LayerObjects.MapObjects[i] is IGravity)
                {
                    LayerObjects.MapObjects[i].Velocity = new PointF(LayerObjects.MapObjects[i].Velocity.X, LayerObjects.MapObjects[i].Velocity.Y + _gravity);
                    LayerObjects.MapObjects[i].MapPosition = new PointF(LayerObjects.MapObjects[i].MapPosition.X, LayerObjects.MapObjects[i].MapPosition.Y + LayerObjects.MapObjects[i].Velocity.Y);
                    // al actualizar la posicion del mapa en Y, se validan las coliciones y se ajusta la posicion en caso de ser necesario
                }

                LayerObjects.MapObjects[i].Update(gameTime);

                if (LayerObjects.MapObjects[i].Velocity.X != 0)
                {
                    if (LayerObjects.MapObjects[i].MapPositionRec.IntersectsWith(_canvasRec)) // los objetos se mueven solo si estan a la vista
                    {
                        LayerObjects.MapObjects[i].MapPosition = new PointF(LayerObjects.MapObjects[i].MapPosition.X + LayerObjects.MapObjects[i].Velocity.X, LayerObjects.MapObjects[i].MapPosition.Y);
                    }
                }

                if (LayerObjects.MapObjects[i].Removing)
                {
                    lstRemove.Add(LayerObjects.MapObjects[i]);
                }
            };

            for (var i = 0; i < lstRemove.Count; i++)
            {
                // remueve de la lista los objetos descartados
                LayerObjects.MapObjects.Remove(lstRemove[i]); 
            }

            for (var i = 0; i < LayerObjects.MapObjectsNew.Count; i++)
            {
                // se agregan los objetos nuevos creados en el transcurso del juego, en este ejemplo las monedas
                LayerObjects.MapObjects.Add(LayerObjects.MapObjectsNew[i]);
            }

            LayerObjects.MapObjectsNew.Clear();
            MoveCharacter();
            UpdateObjectPosition();

            if (LayerObjects.Mario.MapPosition.X + (LayerObjects.Mario.SourceRectangle.Width / 2) >= LayerObjects.Flag.X)
            {
                LayerObjects.Mario.ActionState = MarioAction.Flag;
                LayerObjects.Mario.MapPosition = new PointF(LayerObjects.Flag.X - 10, LayerObjects.Mario.MapPosition.Y);
                LayerObjects.Mario.Velocity = new PointF(0, 0);
                State = GameState.Wining;
                // si mario llega a la bandera cambia el estado del juego
            }
        }

        private void UpdateDying()
        {
            // muestra la animacion de mario muriendo
            LayerObjects.Mario.Velocity = new PointF(LayerObjects.Mario.Velocity.X, LayerObjects.Mario.Velocity.Y + _gravity);
            LayerObjects.Mario.MapPosition = new PointF(LayerObjects.Mario.MapPosition.X, LayerObjects.Mario.MapPosition.Y + LayerObjects.Mario.Velocity.Y);
            
            // al actualizar la posicion del mapa en Y, se validan las coliciones y se ajusta la posicion en caso de ser necesario
            UpdateObjectPosition(LayerObjects.Mario); // actualiza la posicion en el mapa solo de mario 
        }

        private void UpdateWinning(GameTime gameTime)
        {
            LayerObjects.Mario.Velocity = new PointF(LayerObjects.Mario.Velocity.X, LayerObjects.Mario.Velocity.Y + _gravity);
            LayerObjects.Mario.MapPosition = new PointF(LayerObjects.Mario.MapPosition.X + LayerObjects.Mario.Velocity.X, LayerObjects.Mario.MapPosition.Y);
            LayerObjects.Mario.MapPosition = new PointF(LayerObjects.Mario.MapPosition.X, LayerObjects.Mario.MapPosition.Y + LayerObjects.Mario.Velocity.Y);
            LayerObjects.Mario.Animation(gameTime);

            if (LayerObjects.Mario.ActionState == MarioAction.Flag && (LayerObjects.Mario.MapPosition.Y + LayerObjects.Mario.SourceRectangle.Height) == (LayerObjects.Flag.Y + LayerObjects.Flag.Height))
            {
                LayerObjects.Mario.ActionState = MarioAction.Walk;
                LayerObjects.Mario.Velocity = new PointF(6, 0);
            }

            UpdateObjectPosition(LayerObjects.Mario); // actualiza la posicion en el mapa solo de mario 

            if (LayerObjects.Mario.MapPosition.X >= ((LayerTiles.Size.Width - 1) * LayerTiles.TileSize.Width))
            {
                Restart(null, EventArgs.Empty); // reinicia el mapa
            }
        }

        /// <summary>
        /// Actualiza la posicion en pantalla de los objetos del mapa
        /// </summary>
        private void UpdateObjectPosition(BaseEntity mapObject = null)
        {
            var objects = mapObject != null ? new List<BaseEntity>() { mapObject } : LayerObjects.MapObjects;

            for (var i = 0; i < objects.Count; i++)
            {
                objects[i].Position = new PointF(objects[i].MapPosition.X - LayerTiles.Position.X, objects[i].MapPosition.Y - LayerTiles.Position.Y);
            }
        }

        /// <summary>
        /// Desplazamiento del personaje en el escenario
        /// </summary>
        private void MoveCharacter()
        {
            if (LayerObjects.Mario.Position.X > _canvasSize.Width / 2)
            {
                var maxPositionWidth = LayerTiles.Size.Width * LayerTiles.TileSize.Width - _canvasSize.Width;

                LayerTiles.Position = new PointF(LayerTiles.Position.X + (float)(LayerObjects.Mario.Position.X - _canvasSize.Width / 2f), LayerTiles.Position.Y);

                if (LayerTiles.Position.X > maxPositionWidth)
                {
                    // limita el desplazamiento del hasta el borde del mapa
                    LayerTiles.Position = new PointF(maxPositionWidth, LayerTiles.Position.Y); 
                }

                UpdateObjectPosition();
            }
            else if (LayerObjects.Mario.Position.X < 0)
            {
                // evita que el personaje se salga del margen izquierdo de la pantalla
                LayerObjects.Mario.MapPosition = new PointF(LayerTiles.Position.X, LayerObjects.Mario.MapPosition.Y);
            }
        }

        private void ObjectsMapPositionChanged(object sender, PositionEventArgs e)
        {
            if (State == GameState.Playing || State == GameState.Wining)
            {
                // valida la colicion del objeto con las celdas bloqueadas del mapa
                LayerObstacles.ValidColition((BaseEntity)sender, e.Previous);

                // valida la colicion del objeto con otros objetos 
                LayerObjects.ValidColition((BaseEntity)sender, e.Previous);
            }
        }

        private void MarioDied(object sender, EventArgs e)
        {
            State = GameState.Dying; // al detectar que mario murio, cambia el estado del juego
        }

        /// <summary>
        /// Dibuja la grilla
        /// </summary>
        /// <param name="drawHandler"></param>
        public void Draw(DrawHandler drawHandler)
        {
            LayerTiles.Draw(drawHandler);
            LayerObjects.Draw(drawHandler);
        }

        public void Update(GameTime gameTime)
        {
            switch (State)
            {
                case GameState.Playing:
                    UpdatePlaying(gameTime);
                    break;
                case GameState.Dying:
                    UpdateDying();
                    break;
                case GameState.Wining:
                    UpdateWinning(gameTime);
                    break;
            }

            if (LayerObjects.Mario.Position.Y > (_canvasSize.Height * 2))
            {
                Restart(null, EventArgs.Empty);
                // si mario se cae a un poso o completa el mapa, se reinicia 
            }
        }

        #endregion
    }
}
