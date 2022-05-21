using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman
{
    public static class Extensions
    {
        public static IEnumerable<MapEditorVisualTile> Flatten(this MapEditorVisualTile[,] tiles)
        {
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    yield return tiles[y, x];
                }
            }
        }
    }
}
