using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman
{
    public abstract class MapEditorDataTile
    {
        public Point Cord;

        public (Point Index, bool isWall)[] Neighbors = new (Point, bool)[8];

        public Color Tint;

        public Vector2 Origin;

        public float Rotation;

        public Vector2 Scale;

        public SpriteEffects SpriteEffects;
    }
}
