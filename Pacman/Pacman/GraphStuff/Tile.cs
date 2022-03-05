using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman.GraphStuff
{
    public class Tile : Sprite
    {
        public enum States
        {
            Empty,
            Wall,
        }

        public Texture2D CurrentImage;
        private Texture2D ogImage;

        public static Texture2D NormalImage;
        public static Texture2D MapEditorTile;
        public static Texture2D EnlargedBorder;
        public static Texture2D PelletTile;
        private Texture2D WallTile; //there will be a lot of these

        public Vector2 Position;
        public Color Tint;

        public bool isMapEditorTile;
        public bool isWall;

        public Rectangle Hitbox { get => new Rectangle((int)Position.X, (int)Position.Y, CurrentImage.Width, CurrentImage.Height); set { } }

        public States TileStates = States.Empty;

        public Vector2 Cord;

        public Tile(Texture2D image, Vector2 position, Color tint, bool ismapEditorTile) : base(image, position, tint)
        {
            if (ismapEditorTile)
            {
                CurrentImage = MapEditorTile;
                ogImage = MapEditorTile;
            }
            else 
            {
                CurrentImage = NormalImage;
                ogImage = NormalImage;
            }
            Position = position;
            isMapEditorTile = ismapEditorTile;
            Tint = tint;
        }

        public bool IsClicked()
        {
            return false;
        }


        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            if (isMapEditorTile && Hitbox.Intersects(new Rectangle(ms.Position, new Point(1))))
            {
                CurrentImage = EnlargedBorder;
            }
            else 
            {
                CurrentImage = ogImage;
            }
        }

        public override void Draw(SpriteBatch batch)
        {

            batch.Draw(CurrentImage, Position, Tint);

        }
    }
}
