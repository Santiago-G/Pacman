using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman
{
    public class pixelVisual : abstractVisual<Point>
    {
        #region Textures

        public static Texture2D EmptySprite;
        public static Texture2D HLEmptySprite;

        public static Texture2D PelletSprite;
        public static Texture2D HLPelletSprite;

        public static Texture2D PowerPelletSprite;
        public static Texture2D HLPowerPelletSprite;

        #endregion

        public override data
        //override data

        public pixelVisual(Texture2D Image, Point Cord, Color Tint, Vector2 Offset, Vector2 Scale, Vector2 Origin, float Rotation, SpriteEffects spriteEffects) : base(Image, Cord, Tint, Offset, Scale, Origin, Rotation, spriteEffects)
        {
        }

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


                    break;
            }

            if (setDefault)
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


                        default:
                            break;
                    }
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
