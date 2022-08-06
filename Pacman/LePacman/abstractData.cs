using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    public abstract class AbstractData<T>
    {
        //Texture2D image, Point cord, Color tint, Vector2 offset, Vector2 scale, Vector2 origin, float rotation, SpriteEffects spriteEffects
        public virtual States TileStates { get; set; }
        public virtual Point cord { get; set; }

        //check if i can do obj array
        //public (Point Index, bool isWall)[] Neighbors = new (Point, bool)[8];
        public virtual T[] Neighbors { get; set; }

        public virtual Color tint { get; set; }
        public virtual Vector2 offset { get; set; }
        public virtual Vector2 scale { get; set; }
        public virtual Vector2 origin { get; set; }
        public virtual float rotation { get; set; }
        public virtual SpriteEffects spriteEffects { get; set; }
    }
}
