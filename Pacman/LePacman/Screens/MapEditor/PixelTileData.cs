using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;


namespace LePacman.Screens.MapEditor
{
    public class PixelTileData : AbstractData
    {
        public override Point[] Neighbors { get; set; }

        public PixelTileData()
        {
            Neighbors = new Point[8];
        }
    }
}
