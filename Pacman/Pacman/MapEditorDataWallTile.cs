using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman
{
    public class MapEditorDataWallTile : MapEditorDataTile
    {
        public WallTileStates TileStates = WallTileStates.Empty;
        public WallStates WallStates = WallStates.notAWall;
    }
}
