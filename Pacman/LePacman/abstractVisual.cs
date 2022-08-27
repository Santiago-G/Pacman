using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Pacman
{
    // public abstract class abstractVisual<T, D> : Sprite where D : abstractData<T>
    // : abstractVisual<T, pixelData<T>>
    public abstract class abstractVisual<T> : Sprite
    {
        public Texture2D CurrentImage;
        public Texture2D PrevImage;

        //shift click like in gimp to fill in whatever is selected
        //also have a fill all

        protected abstract AbstractData<T> data { get; set; }

        public States TileStates
        {
            get { return data.TileStates; }
            set { data.TileStates = value; }
        }
        public Point Cord
        {
            get { return data.cord; }
            set { data.cord = value; }
        }

        public override Color Tint
        {
            get
            {
                return data.tint;
            }
            set
            {
                data.tint = value;
            }
        }
        public Vector2 Offset
        {
            get { return data.offset; }
            set { data.offset = value; }
        }
        public override Vector2 Scale
        {
            get { return data.scale; }
            set { data.scale = value; }
        }
        public override Vector2 Origin
        {
            get { return data.origin; }
            set { data.origin = value; }
        }
        public override float Rotation
        {
            get { return data.rotation; }
            set { data.rotation = value; }
        }
        public override SpriteEffects SpriteEffects
        {
            get { return data.spriteEffects; }
            set { data.spriteEffects = value; }
        }

        public override Rectangle Hitbox => new Rectangle((int)(Position.X - Origin.X * Scale.X), (int)(Position.Y - Origin.Y * Scale.Y), (int)(CurrentImage.Width * Scale.X), (int)(CurrentImage.Height * Scale.Y));

        public T[] Neighbors 
        { 
            get { return data.Neighbors; } 
            set { data.Neighbors = value; }
        }


        //public (Point Index, bool isWall)[] Neighbors => ((Point, bool))Data.Neighbors;

        public override Vector2 Position
        {
            get
            {
                return new Vector2(Cord.Y * CurrentImage.Width, Cord.X * CurrentImage.Height) + Offset;
            } 
        }


        public abstractVisual(Texture2D image, Point cord, Color tint, Vector2 offset, Vector2 scale, Vector2 origin, float rotation, SpriteEffects spriteEffects) : base(image, Vector2.Zero, tint, scale, origin, rotation, spriteEffects)
        {
            CurrentImage = image;
            PrevImage = CurrentImage;
            Cord = cord;
            Offset = offset;         
        }

        public abstractVisual(AbstractData<T> dataTile, Vector2 offset) : base(null, new Vector2(0), dataTile.tint)
        {
            data = dataTile;
            Offset = offset;
            UpdateStates(true);
        }

        //fix both images when i hover over them

        public abstract void UpdateStates(bool setDefault = false);
       
        public abstract override void Update(GameTime gameTime);

        public abstract override void Draw(SpriteBatch batch);
    }
}
