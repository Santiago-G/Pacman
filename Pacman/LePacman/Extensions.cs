using LePacman.Screens.MapEditor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static LePacman.Screens.MapEditor.MapEditor;

namespace Pacman
{
    public static class Extensions
    {
        public static IEnumerable<T> Flatten<T>(this T[,] tiles)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    yield return tiles[x, y];
                }
            }
        }

        public static WallTileInfo[] SaveWallData(this WallTileData[] tiles)
        {
            WallTileInfo[] savedMapTiles = new WallTileInfo[tiles.Length];

            for (int i = 0; i < tiles.Length; i++)
            {
                savedMapTiles[i] = new WallTileInfo(tiles[i].TileStates, tiles[i].cord, tiles[i].Neighbors, tiles[i].WallState);
            }

            return savedMapTiles;
        }

        public static PelletTileInfo[] SavePelletData(this PixelTileData[] tiles)
        {
            PelletTileInfo[] savedMapTiles = new PelletTileInfo[tiles.Length];

            for (int i = 0; i < tiles.Length; i++)
            {
                savedMapTiles[i] = new PelletTileInfo(tiles[i].TileStates, tiles[i].cord, tiles[i].Neighbors);
            }

            return savedMapTiles;
        }

        public static PortalPairData ToPortalData(this PortalPair portals) 
        {
            return new PortalPairData(new PortalData(portals.firstPortal.firstTile.Cord, portals.firstPortal.secondTile.Cord),
                new PortalData(portals.secondPortal.firstTile.Cord, portals.secondPortal.secondTile.Cord));
        }
        public static PortalPairData[] ToPortalDataArray(this PortalPair[] portals)
        {
            PortalPairData[] newPortals = new PortalPairData[portals.Length];

            for (int i = 0; i < portals.Length; i++) 
            {
                newPortals[i] = portals[i].ToPortalData();
            }

            return newPortals;
        }

        public static T[,] Expand<T>(this IEnumerable<T> items, Point size)
        {
            T[,] expandedItems = new T[size.X, size.Y];

            int x = 0;
            int y = 0;

            foreach (var item in items)
            {
                if (x >= size.X)
                {
                    x = 0;
                    y++;
                }

                expandedItems[x, y] = item;
                x++;
            }

            return expandedItems;
        }

        //This was not endorsed by Michael!
        public static T[,] Expand<T>(this IEnumerable<T> items, int width)
        {
            T[] arr = items.ToArray();

            return Expand(items, new Point(width, arr.Length / width));
        }
    }
}
