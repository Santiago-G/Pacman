using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman
{
    public class wallVisual : abstractVisual<(Point Index, bool isWall)>
    {

        #region Textures

        public static Texture2D EmptySprite;
        public static Texture2D HLEmptySprite;

        public static Texture2D LoneWallSprite;

        public static Texture2D InteriorWallSprite;

        public static Texture2D MiddleWallSprite;

        public static Texture2D SingleWallEnd;

        public static Texture2D CornerWallTile;

        public static Texture2D CornerWallFilledTile;

        public static Texture2D EdgeSprite;

        public static Texture2D InteriorCrossSprite;

        public static Texture2D SingleCrossSprite;

        public static Texture2D InteriorFilledCorner;

        #endregion        

        public wallData Data { get; set; }

        protected override AbstractData<(Point, bool)> data { get => Data; set { Data = (wallData)value; } }

        public WallStates WallState
        {
            get { return Data.WallState; }
            set { Data.WallState = value;}
        }

        public wallVisual(Texture2D Image, Point Cord, Color Tint, Vector2 Offset, Vector2 Scale, Vector2 Origin, float Rotation, SpriteEffects spriteEffects) : base(new wallData(), Image, Cord, Tint, Offset, Scale, Origin, Rotation, spriteEffects)
        {

        }

        public wallVisual(wallData dataTile, Vector2 offset) : base(dataTile, offset)
        {
            
        }

        public override void UpdateStates(bool setDefault = false)
        {
            if (data.TileStates == States.Empty)
            {
                CurrentImage = EmptySprite;
                PrevImage = HLEmptySprite;
            }
            else 
            {
                switch (((wallData)data).WallState)
                {
                    case WallStates.LoneWall:
                        CurrentImage = LoneWallSprite;
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.Horiz:
                        CurrentImage = MiddleWallSprite;
                        PrevImage = MiddleWallSprite;
                        break;
                    case WallStates.HorizLeftEnd:
                        CurrentImage = SingleWallEnd;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.HorizRightEnd:
                        CurrentImage = SingleWallEnd;
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.Verti:
                        CurrentImage = MiddleWallSprite;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.VertiTopEnd:
                        CurrentImage = SingleWallEnd;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.VertiBottomEnd:
                        CurrentImage = SingleWallEnd;
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.TopLeftCorner:
                        CurrentImage = CornerWallTile;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.TopRightCorner:
                        CurrentImage = CornerWallTile;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.BottomRightCorner:
                        CurrentImage = CornerWallTile;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.BottomLeftCorner:
                        CurrentImage = CornerWallTile;
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.TopLeftCornerFilled:
                        CurrentImage = CornerWallFilledTile;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.TopRightCornerFilled:
                        CurrentImage = CornerWallFilledTile;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.BottomRightCornerFilled:
                        CurrentImage = CornerWallFilledTile;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.BottomLeftCornerFilled:
                        CurrentImage = CornerWallFilledTile;
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.TopEdge:
                        CurrentImage = EdgeSprite;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.RightEdge:
                        CurrentImage = EdgeSprite;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.BottomEdge:
                        CurrentImage = EdgeSprite;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.LeftEdge:
                        CurrentImage = EdgeSprite;
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.TopCross:
                        CurrentImage = SingleCrossSprite;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.RightCross:
                        CurrentImage = SingleCrossSprite;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.BottomCross:
                        CurrentImage = SingleCrossSprite;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.LeftCross:
                        CurrentImage = SingleCrossSprite;
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.InteriorWall:
                        CurrentImage = InteriorWallSprite;
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.InteriorCorner:
                        CurrentImage = InteriorCrossSprite;
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.TopLeftInteriorFilledCorner:
                        CurrentImage = InteriorFilledCorner;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.TopRightInteriorFilledCorner:
                        CurrentImage = InteriorFilledCorner;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.BottomRightInteriorFilledCorner:
                        CurrentImage = InteriorFilledCorner;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.BottomLeftInteriorFilledCorner:
                        CurrentImage = InteriorFilledCorner;
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.Empty:
                        CurrentImage = EmptySprite;
                        PrevImage = HLEmptySprite;
                        break;
                }
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

            //Try replacing switches with dictionaries

            if (Hitbox.Contains(ms.Position))
            {
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    switch (MapEditor.selectedTileType)
                    {
                        case SelectedType.Default:
                            break;
                        //case SelectedType.Eraser:
                        //    // if (TileStates != States.Wall)
                        //    {
                        //        TileStates = States.Empty;
                        //    }
                            //break;
                        case SelectedType.Wall:
                            TileStates = States.Wall;
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
