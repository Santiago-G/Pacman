using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Pacman
{
    public abstract class MapEditorVisualTile : Sprite
    {
        public Texture2D CurrentImage;
        public override Texture2D Image => CurrentImage;

        public MapEditorDataTile Data { get; }

        public Point Cord
        {
            get
            {
                return Data.Cord;
            }
            set
            {
                Data.Cord = value;
            }
        }

        public (Point Index, bool isWall)[] Neighbors => Data.Neighbors;

        private Vector2 offset;
        public override Vector2 Position
        {
            get
            {
                return new Vector2(Cord.Y * CurrentImage.Width, Cord.X * CurrentImage.Height) + offset;
            }
        }
        public override Color Tint
        {
            get
            {
                return Data.Tint;
            }
            set
            {
                Data.Tint = value;
            }
        }

        public override Vector2 Origin { get => Data.Origin; set => Data.Origin = value; }

        public override float Rotation { get => Data.Rotation; set => Data.Rotation = value; }

        public override SpriteEffects SpriteEffects { get => Data.SpriteEffects; set => Data.SpriteEffects = value; }

        public override Vector2 Scale { get => Data.Scale; set => Data.Scale = value; }

        public MapEditorVisualTile(Texture2D image, Point cord, Color tint, Vector2 offset, Vector2 scale, Vector2 origin, float rotation, SpriteEffects spriteEffects, MapEditorDataTile data) : base(image, Vector2.Zero, tint, scale, origin, rotation, spriteEffects)
        {
            Cord = cord;
            this.offset = offset;
            Data = data;
        }

        public MapEditorVisualTile(MapEditorDataTile dataTile, Vector2 offset) : base(null, new Vector2(0), dataTile.Tint)
        {
            Data = dataTile;
            this.offset = offset;
            UpdateStates(true);
        }

        public abstract void UpdateStates(bool setDefault = false);
    }
}
