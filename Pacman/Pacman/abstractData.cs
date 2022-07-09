using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    public abstract class abstractData
    {
        //Texture2D image, Point cord, Color tint, Vector2 offset, Vector2 scale, Vector2 origin, float rotation, SpriteEffects spriteEffects
        public States TileStates = States.Empty;
        public Point cord;

        //check if i can do obj array
        //public (Point Index, bool isWall)[] Neighbors = new (Point, bool)[8];
        public object [] Neighbors = new object[8];

        public Color tint;
        public Vector2 offset;
        public Vector2 scale;
        public Vector2 origin;
        public float rotation;
        public SpriteEffects spriteEffects;
    }
}
