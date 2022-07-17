using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;


namespace Pacman
{
    public class pixelData : AbstractData<Point>
    { 
        public override Point[] Neighbors { get; set; }

        public pixelData()
        {
            Neighbors = new Point[8];
        }
    }
}
