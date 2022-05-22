using Microsoft.Xna.Framework;
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

            int index = 0;
            foreach (var item in items)
            {
                index++;

                if ()
                { 
                }
            }

            return expandedItems;
        }

        //This was not endorsed by Michael!
        public static T[,] Expand<T>(this IEnumerable<T> items, int width)
        {
            T[] arr = items.ToArray();

            return Expand(items, new Point(width, arr.Length/width));
        }
    }
}
