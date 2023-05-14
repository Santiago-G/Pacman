using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;


namespace LePacman.Screens.MapEditor
{
    public class pixelData : AbstractData
    {
        public override Point[] Neighbors { get; set; }

        public pixelData()
        {
            Neighbors = new Point[8];
        }
    }
}
