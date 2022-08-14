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


        public static Texture2D NBemptySprite;
        public static Texture2D NBloneWallSprite;
        public static Texture2D NBinteriorWallSprite;
        public static Texture2D NBmiddleWallSprite;
        public static Texture2D NBsingleWallEnd;
        public static Texture2D NBcornerWallTile;
        public static Texture2D NBcornerWallFilledTile;
        public static Texture2D NBedgeSprite;
        public static Texture2D NBinteriorCrossSprite;
        public static Texture2D NBsingleCrossSprite;
        public static Texture2D NBinteriorFilledCorner;

        public static Texture2D OccupiedSprite;
        #endregion        

        public wallData Data { get; set; } = new wallData();

        protected override AbstractData<(Point, bool)> data { get => Data; set { Data = (wallData)value; } }

        public WallStates WallState
        {
            get { return Data.WallState; }
            set { Data.WallState = value; }
        }

        public wallVisual(Texture2D Image, Point Cord, Color Tint, Vector2 Offset, Vector2 Scale, Vector2 Origin, float Rotation, SpriteEffects spriteEffects) : base(Image, Cord, Tint, Offset, Scale, Origin, Rotation, spriteEffects)
        {
            ;
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
            else if (data.TileStates == States.Occupied)
            {
                CurrentImage = OccupiedSprite;
                PrevImage = OccupiedSprite;
            }
            else
            {
                switch (((wallData)data).WallState)
                {
                    case WallStates.LoneWall:
                        CurrentImage = LoneWallSprite;
                        Rotation = 0;
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.Horiz:
                        CurrentImage = MiddleWallSprite;
                        Rotation = (float)(Math.PI * .5);
                        PrevImage = MiddleWallSprite;
                        break;
                    case WallStates.HorizLeftEnd:
                        CurrentImage = SingleWallEnd;
                        Rotation = (float)(Math.PI * 1.5);
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.HorizRightEnd:
                        CurrentImage = SingleWallEnd;
                        Rotation = (float)(Math.PI * .5);
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.Verti:
                        CurrentImage = MiddleWallSprite;
                        Rotation = 0;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.VertiTopEnd:
                        CurrentImage = SingleWallEnd;
                        Rotation = 0;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.VertiBottomEnd:
                        CurrentImage = SingleWallEnd;
                        Rotation = (float)(Math.PI * 1);
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.TopLeftCorner:
                        CurrentImage = CornerWallTile;
                        Rotation = 0;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.TopRightCorner:
                        CurrentImage = CornerWallTile;
                        Rotation = (float)(Math.PI * .5);
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.BottomRightCorner:
                        CurrentImage = CornerWallTile;
                        Rotation = (float)(Math.PI * 1);
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.BottomLeftCorner:
                        CurrentImage = CornerWallTile;
                        Rotation = (float)(Math.PI * 1.5);
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.TopLeftCornerFilled:
                        CurrentImage = CornerWallFilledTile;
                        Rotation = 0;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.TopRightCornerFilled:
                        CurrentImage = CornerWallFilledTile;
                        Rotation = (float)(Math.PI * .5);
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.BottomRightCornerFilled:
                        CurrentImage = CornerWallFilledTile;
                        Rotation = (float)(Math.PI * 1);
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.BottomLeftCornerFilled:
                        CurrentImage = CornerWallFilledTile;
                        Rotation = (float)(Math.PI * 1.5);
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.TopEdge:
                        CurrentImage = EdgeSprite;
                        Rotation = 0;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.RightEdge:
                        CurrentImage = EdgeSprite;
                        Rotation = (float)(Math.PI * .5);
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.BottomEdge:
                        CurrentImage = EdgeSprite;
                        Rotation = (float)(Math.PI * 1);
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.LeftEdge:
                        CurrentImage = EdgeSprite;
                        Rotation = (float)(Math.PI * 1.5);
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.TopCross:
                        CurrentImage = SingleCrossSprite;
                        Rotation = 0;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.RightCross:
                        CurrentImage = SingleCrossSprite;
                        Rotation = (float)(Math.PI * .5);
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.BottomCross:
                        CurrentImage = SingleCrossSprite;
                        Rotation = (float)(Math.PI * 1);
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.LeftCross:
                        CurrentImage = SingleCrossSprite;
                        Rotation = (float)(Math.PI * 1.5);
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.InteriorWall:
                        CurrentImage = InteriorWallSprite;
                        Rotation = 0;
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.InteriorCorner:
                        CurrentImage = InteriorCrossSprite;
                        Rotation = 0;
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.TopLeftInteriorFilledCorner:
                        CurrentImage = InteriorFilledCorner;
                        Rotation = 0;
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.TopRightInteriorFilledCorner:
                        CurrentImage = InteriorFilledCorner;
                        Rotation = (float)(Math.PI * .5);
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.BottomRightInteriorFilledCorner:
                        CurrentImage = InteriorFilledCorner;
                        Rotation = (float)(Math.PI * 1);
                        PrevImage = CurrentImage;
                        break;
                    case WallStates.BottomLeftInteriorFilledCorner:
                        CurrentImage = InteriorFilledCorner;
                        Rotation = (float)(Math.PI * 1.5);
                        PrevImage = CurrentImage;
                        break;

                    case WallStates.Empty:
                        CurrentImage = EmptySprite;
                        Rotation = 0;
                        PrevImage = HLEmptySprite;
                        break;
                }
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

            //Try replacing switches with dictionaries

            if (Hitbox.Contains(ms.Position))
            {
                if (ms.LeftButton == ButtonState.Pressed && TileStates != States.Occupied)
                {
                    //switch (MapEditor.selectedTileType)
                    //{
                    //    //case SelectedType.Eraser:
                    //    //    if (TileStates != States.Wall)
                    //    //    {
                    //    //        TileStates = States.Empty;
                    //    //    }
                    //    //    else
                    //    //    {
                    //    //        Map
                    //    //    }
                    //    //    break;
                    //    case SelectedType.Wall:
                    //        TileStates = States.Wall;
                    //        break;
                    //}

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
