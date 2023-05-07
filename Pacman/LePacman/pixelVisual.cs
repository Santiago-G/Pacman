using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Pacman
{
    public class pixelVisual : abstractVisual
    {
        #region Textures

        public static Texture2D EmptySprite;
        public static Texture2D HLEmptySprite;

        public static Texture2D PelletSprite;
        public static Texture2D HLPelletSprite;

        public static Texture2D PowerPelletSprite;
        public static Texture2D HLPowerPelletSprite;

        public static Texture2D NBemptySprite;
        public static Texture2D NBpelletSprite;
        public static Texture2D NBpowerPelletSprite;
        #endregion

        public static MapEditorPixelGrid Grid;

        public pixelData Data { get; set; } = new pixelData();

        protected override AbstractData data { get => Data; set { Data = (pixelData)value; } }

        public pixelVisual(Texture2D Image, Point Cord, Color Tint, Vector2 Offset, Vector2 Scale, Vector2 Origin, float Rotation, SpriteEffects spriteEffects) : base(Image, Cord, Tint, Offset, Scale, Origin, Rotation, spriteEffects)
        {

        }

        public pixelVisual(pixelData dataTile, Vector2 offset) : base(dataTile, offset)
        {
            
        }

        public bool isPacmanTile = false;

        public override void UpdateStates(bool setDefault = false)
        {
            
            switch (TileStates)
            {
                case States.Empty:
                    CurrentImage = EmptySprite;
                    PrevImage = HLEmptySprite;
                    break;
                case States.Pellet:
                    CurrentImage = PelletSprite;
                    PrevImage = HLPelletSprite;
                    break;
                case States.PowerPellet:
                    CurrentImage = PowerPelletSprite;
                    PrevImage = HLPowerPelletSprite;
                    break;
                case States.Fruit:
                    CurrentImage = NBemptySprite;
                    PrevImage = NBemptySprite;
                    break;
                case States.Occupied:
                    CurrentImage = NBemptySprite;
                    PrevImage = NBemptySprite;
                    break;
                case States.Pacman:
                    CurrentImage = NBemptySprite;
                    PrevImage = CurrentImage;
                    break;
            }

            if (!setDefault)
            {
                Texture2D bucket = CurrentImage;
                CurrentImage = PrevImage;
                PrevImage = bucket;
            }
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            if (Hitbox.Contains(ms.Position))
            {
                if (ms.LeftButton == ButtonState.Pressed) 
                {
                    if (TileStates != States.Occupied && TileStates != States.Pacman)
                    {
                        switch (MapEditor.selectedTileType)
                        {
                            case SelectedType.Default:
                                break;
                            case SelectedType.Pellet:
                                TileStates = States.Pellet;
                                break;
                            case SelectedType.PowerPellet:
                                TileStates = States.PowerPellet;
                                break;
                            case SelectedType.Eraser:
                                TileStates = States.Empty;
                                break;
                        }
                    }
                    else if (TileStates == States.Pacman && MapEditor.selectedTileType == SelectedType.Eraser)
                    {
                        Grid.RemovePacman(Grid.PosToIndex(ms.Position.ToVector2()));
                    }
                }

                if (MapEditor.selectedTileType == SelectedType.AltEraser)
                {
                    TileStates = States.Empty;
                }

                UpdateStates();
            }
            else
            {
                CurrentImage = PrevImage;
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(CurrentImage, Position, null, Tint, Rotation, Origin, Scale, SpriteEffects, 0);
        }
    }
}
