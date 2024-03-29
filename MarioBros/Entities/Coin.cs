﻿using System;
using System.Drawing;
using MarioBros.Engine;

namespace MarioBros.Entities
{
    public class Coin : BaseEntity, IGravity
    {
        #region Fields

        private PointF? _originalPosition;

        #endregion
        #region Constructors

        public Coin(Resources resources)
        {
            if (resources.Map == null || resources.SpriteSheet == null)
            {
                throw new NullReferenceException();
            }

            var _recSize = new Size(resources.Map.TileWidth, resources.Map.TileHeight);

            Image = resources.SpriteSheet;
            SourceRectangles = CreateRectangles(_recSize, new Point(256, 192), new Point(288, 192), new Point(320, 192), new Point(352, 192));
            Velocity = new PointF(0, -20);
            FPS = 6;
        }

        #endregion
        #region BaseEntity

        public override void Update(GameTime gameTime)
        {
            if (_originalPosition == null)
            {
                _originalPosition = MapPosition;
            }

            if (MapPosition.Y > _originalPosition.Value.Y)
            {
                Removing = true;
            }

            base.Update(gameTime);
        }

        #endregion
    }
}
