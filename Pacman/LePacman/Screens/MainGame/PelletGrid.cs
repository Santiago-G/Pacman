using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LePacman.Screens.MapEditor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Bson;
using Pacman;
using System;
using System.Collections.Generic;

namespace LePacman.Screens.MainGame
{
    internal class PelletGrid
    {
        private PelletGrid() { }
        public static PelletGrid Instance { get; } = new PelletGrid();
        public Vector2 offset;
        public float tileSize;

        public Vector2 CoordToPostion(Point Coord)
        {
            return new Vector2(offset.X + Coord.X * tileSize, offset.Y + Coord.Y * tileSize);
        }

        //public static Vector2 CoordToPostion(Point Coord)
        //{
        //    return new Vector2(offset.X + Coord.X * tileSize, offset.Y + Coord.Y * tileSize);
        //}
        //how should i format it/what do i put in the singleton (pacman, ghosts, ect)



        public Pacman Pacman;
        public Point pacmanPos => Pacman.currPelletTile.coord;
    }
}
