using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman
{
    public static class Extensions
    {
        public static IEnumerable<T> Flatten<T>(this T[,] tiles)
        {
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    yield return tiles[y, x];
                }
            }
        }

        public static T[,] Expand<T>(this IEnumerable<T> items, Point size)
        {
            T[,] expandedItems = new T[size.Y, size.X];

            int x = 0;
            int y = 0;

            foreach (var item in items)
            {
                if (x >= size.X)
                {
                    x = 0;
                    y++;
                }

                expandedItems[y, x] = item;
                x++;
            }

            return expandedItems;
        }

        //public static Texture2D Flip()
        //{
            
        //}

        //This was not endorsed by Michael!
        public static T[,] Expand<T>(this IEnumerable<T> items, int width)
        {
            T[] arr = items.ToArray();

            return Expand(items, new Point(width, arr.Length/width));
        }
    }
}
