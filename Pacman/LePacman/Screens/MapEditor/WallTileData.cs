using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman;
using System;
using System.Collections.Generic;
using System.Text;



namespace LePacman.Screens.MapEditor
{
    public class WallTileData : AbstractData
    {
        public WallStates WallState = WallStates.Empty;

        public override Point[] Neighbors { get; set; }

        public WallTileData()
        {
            Neighbors = new Point[8];
        }
    }
}
