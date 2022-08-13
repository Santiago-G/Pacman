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

        public static Texture2D NBemptySprite;
        public static Texture2D NBpelletSprite;
        public static Texture2D NBpowerPelletSprite;

        public static Texture2D OccupiedSprite;
        #endregion

        public pixelData Data { get; set; } = new pixelData();

        protected override AbstractData<Point> data { get => Data; set { Data = (pixelData)value; } }

        public pixelVisual(Texture2D Image, Point Cord, Color Tint, Vector2 Offset, Vector2 Scale, Vector2 Origin, float Rotation, SpriteEffects spriteEffects) : base(Image, Cord, Tint, Offset, Scale, Origin, Rotation, spriteEffects)
        {

        }

        public pixelVisual(pixelData dataTile, Vector2 offset) : base(dataTile, offset)
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
                case States.NoBackground:
                    ;
                    break;
                case States.Occupied:
                    CurrentImage = OccupiedSprite;
                    PrevImage = OccupiedSprite;
                    break;
            }

            if (!setDefault)
            {
                Texture2D bucket = CurrentImage;
                CurrentImage = PrevImage;
                PrevImage = bucket;
            }
            else 
            {
                ;
            }
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            if (Hitbox.Contains(ms.Position))
            {
                if (ms.LeftButton == ButtonState.Pressed && TileStates != States.Occupied)
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
