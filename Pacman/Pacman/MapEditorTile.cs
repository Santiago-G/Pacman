﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman
{
    public class MapEditorTile : Sprite
    {

        public enum States
        {
            Empty,
            Pellet,
            PowerPellet,
            Fruit,
            Wall
        }

        public Texture2D CurrentImage;
        private Texture2D prevImage;

        public static Texture2D NormalSprite;
        public static Texture2D NormalEnlargedBorder;

        public static Texture2D PelletSprite;
        public static Texture2D PelletEnlargedBorder;

        private Texture2D WallTile; //there will be a lot of these

        public Vector2 Position;
        public Color Tint;

        //shift click like in gimp to fill in wehatever is selected
        //also have a fill all.

        //powerpellet

        public Rectangle Hitbox { get => new Rectangle((int)Position.X, (int)Position.Y, CurrentImage.Width, CurrentImage.Height); set { } }

        public States TileStates = States.Empty;

        public Vector2 Cord;

        public MapEditorTile(Texture2D image, Vector2 position, Color tint) : base(image, position, tint)
        {
            CurrentImage = NormalSprite;
            prevImage = CurrentImage;
            Position = position;
            Tint = tint;
        }

        public bool IsClicked()
        {
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            //Try replacing switches with dictionaries

            if (Hitbox.Contains(ms.Position))
            {
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    switch (MapEditor.selectedTileType)
                    {
                        case SelectedType.Default:
                            break;
                        case SelectedType.Pellet:
                            break;
                        case SelectedType.PowerPellet:
                            break;
                        default:
                            break;
                    }
                    TileStates = States.Pellet;
                }

                switch (TileStates)
                {
                    case States.Empty:
                        prevImage = NormalSprite;
                        CurrentImage = NormalEnlargedBorder;
                        break;
                    case States.Pellet:
                        prevImage = PelletSprite;
                        CurrentImage = PelletEnlargedBorder;
                        break;
                }
            }
            else
            {
                CurrentImage = prevImage;
            }

        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(CurrentImage, Position, Tint);
        }
    }
}