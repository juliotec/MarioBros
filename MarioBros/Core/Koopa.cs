﻿using Game.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioBros.Elements.Objects
{
    public class Koopa : Base
    {
        #region Constructor
        public Koopa(Elements.Resources resources, Data.BaseObject obj)
        {
            base.Image = resources.SpriteSheet;

            //Size _recSize = new Size(resources.Map_Data.tilewidth, resources.Map_Data.tileheight);
            SourceRec_Normal = base.Create_Rectangles(new Size(resources.Map_Data.Tilewidth, resources.Map_Data.Tileheight * 2), 
                new Point(224, 384), 
                new Point(256, 384)
            );

            SourceRectangles = SourceRec_Normal;
            Velocity = new PointF(-2, 0);
            FPS = 6;

            MapPosition = new PointF(obj.X, (int)obj.Y - resources.Map_Data.Tileheight);
        }
        #endregion

        #region Properties
        private Rectangle[] SourceRec_Normal { get; set; }
        #endregion

    }
}
