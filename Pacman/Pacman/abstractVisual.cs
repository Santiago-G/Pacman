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

        public abstract abstractData<T> Data { get; set; }

        public States TileStates
        {
            get { return Data.TileStates; }
            set { Data.TileStates = value; }
        }
        public Point Cord
        {
            get { return Data.cord; }
            set { Data.cord = value; }
        }

        public override Color Tint
        {
            get
            {
                return Data.tint;
            }
            set
            {
                Data.tint = value;
            }
        }
        public Vector2 Offset
        {
            get { return Data.offset; }
            set { Data.offset = value; }
        }
        public override Vector2 Scale
        {
            get { return Data.scale; }
            set { Data.scale = value; }
        }
        public override Vector2 Origin
        {
            get { return Data.origin; }
            set { Data.origin = value; }
        }
        public override float Rotation
        {
            get { return Data.rotation; }
            set { Data.rotation = value; }
        }
        public override SpriteEffects SpriteEffects
        {
            get { return Data.spriteEffects; }
            set { Data.spriteEffects = value; }
        }


        //public (Point Index, bool isWall)[] Neighbors => ((Point, bool))Data.Neighbors;

        public override Vector2 Position
        {
            get
            {
                return new Vector2(Cord.Y * CurrentImage.Width, Cord.X * CurrentImage.Height) + Offset;
            }
        }


        public abstractVisual(abstractData<T> data, Texture2D image, Point cord, Color tint, Vector2 offset, Vector2 scale, Vector2 origin, float rotation, SpriteEffects spriteEffects) : base(image, Vector2.Zero, tint, scale, origin, rotation, spriteEffects)
        {
            Data = data;
            CurrentImage = image;
            PrevImage = CurrentImage;
            Cord = cord;
            Offset = offset;
        }

        public abstractVisual(abstractData<T> dataTile, Vector2 offset) : base(null, new Vector2(0), dataTile.tint)
        {
            Data = dataTile;
            Offset = offset;
            UpdateStates(true);
        }

        //fix both images when i hover over them

        public abstract void UpdateStates(bool setDefault = false);
       
        public abstract override void Update(GameTime gameTime);

        public abstract override void Draw(SpriteBatch batch);
    }
}
