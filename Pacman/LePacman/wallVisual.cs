using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman
{
    public class wallVisual : abstractVisual
    {
        public static MapEditorWallGrid Grid;

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


        public static Texture2D MiddleOuterWall;

        public static Texture2D CornerOuterWall;

        public static Texture2D SingleIntersectingOuterWall;
        public static Texture2D MiddleIntersectingOuterWall;
        public static Texture2D EdgeIntersectingOuterWall;
        public static Texture2D Edge2IntersectingOuterWall;
        #endregion        

        public wallData Data { get; set; } = new wallData();

        public int OuterWallNeighborCount(wallVisual[,] Tiles, bool countDiaganals)
        {
            int numOfNeighboringOWs = 0;
            int maxCount = Neighbors.Length;

            if (!countDiaganals) maxCount = 4;

            if (/*isValidPostion(Cord, Tiles) &&*/ WallState.HasFlag(WallStates.OuterWall))
            {
                for (int i = 0; i < maxCount; i++)
                {
                    if (!isValidPostion(Neighbors[i], Tiles)) continue;

                    if (Tiles[Neighbors[i].X, Neighbors[i].Y].WallState.HasFlag(WallStates.OuterWall))
                    {
                        numOfNeighboringOWs += 1;
                    }
                }
            }

            return numOfNeighboringOWs;
        }

        private bool isValidPostion(Point Cord, wallVisual[,] Tiles)
        {
            return isValidPostion(Cord.ToVector2(), Tiles);
        }
        private bool isValidPostion(Vector2 Cord, wallVisual[,] Tiles)
        {
            return !(Cord.X < 0 || Cord.Y < 0 || Cord.X >= Tiles.GetLength(0) || Cord.Y >= Tiles.GetLength(1));
        }

        protected override AbstractData data { get => Data; set { Data = (wallData)value; } }

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
            if (Data.TileStates == States.Empty)
            {
                CurrentImage = EmptySprite;
                PrevImage = HLEmptySprite;
            }
            else if (Data.TileStates == States.Error)
            {
                //me when i
            }
            else if (Data.TileStates == States.Occupied || Data.TileStates == States.GhostChamber || Data.TileStates == States.Pacman)
            {
                CurrentImage = NBemptySprite;
                PrevImage = NBemptySprite;
            }
            else
            {
                switch (Data.WallState)
                {
                    case WallStates.Empty:
                        CurrentImage = EmptySprite;
                        Rotation = 0;
                        PrevImage = HLEmptySprite;
                        break;

                    case WallStates.LoneWall:
                        CurrentImage = LoneWallSprite;
                        Rotation = 0;
                        break;

                    #region Horizontals
                    case WallStates.Horiz:
                        CurrentImage = MiddleWallSprite;
                        Rotation = (float)(Math.PI * .5);
                        PrevImage = MiddleWallSprite;
                        break;
                    case WallStates.HorizLeftEnd:
                        CurrentImage = SingleWallEnd;
                        Rotation = (float)(Math.PI * 1.5);
                        break;
                    case WallStates.HorizRightEnd:
                        CurrentImage = SingleWallEnd;
                        Rotation = (float)(Math.PI * .5);
                        break;
                    #endregion

                    #region Verticals
                    case WallStates.Verti:
                        CurrentImage = MiddleWallSprite;
                        Rotation = 0;
                        break;
                    case WallStates.VertiTopEnd:
                        CurrentImage = SingleWallEnd;
                        Rotation = 0;
                        break;
                    case WallStates.VertiBottomEnd:
                        CurrentImage = SingleWallEnd;
                        Rotation = (float)(Math.PI * 1);
                        break;
                    #endregion

                    #region Corners
                    case WallStates.TopLeftCorner:
                        CurrentImage = CornerWallTile;
                        Rotation = 0;
                        break;
                    case WallStates.TopRightCorner:
                        CurrentImage = CornerWallTile;
                        Rotation = (float)(Math.PI * .5);
                        break;
                    case WallStates.BottomRightCorner:
                        CurrentImage = CornerWallTile;
                        Rotation = (float)(Math.PI * 1);
                        break;
                    case WallStates.BottomLeftCorner:
                        CurrentImage = CornerWallTile;
                        Rotation = (float)(Math.PI * 1.5);
                        break;
                    #endregion

                    #region Edges
                    case WallStates.TopEdge:
                        CurrentImage = EdgeSprite;
                        Rotation = 0;
                        break;
                    case WallStates.RightEdge:
                        CurrentImage = EdgeSprite;
                        Rotation = (float)(Math.PI * .5);
                        break;
                    case WallStates.BottomEdge:
                        CurrentImage = EdgeSprite;
                        Rotation = (float)(Math.PI * 1);
                        break;
                    case WallStates.LeftEdge:
                        CurrentImage = EdgeSprite;
                        Rotation = (float)(Math.PI * 1.5);
                        break;
                    #endregion

                    case WallStates.Interior:
                        CurrentImage = InteriorWallSprite;
                        Rotation = 0;
                        break;

                    #region Outer Walls
                    case WallStates.OuterWall:
                        CurrentImage = LoneWallSprite;
                        Rotation = 0;
                        break;

                    #region Verticals
                    case WallStates.OuterUp:
                        CurrentImage = MiddleOuterWall;
                        Rotation = 0;
                        break;
                    case WallStates.OuterDown:
                        CurrentImage = MiddleOuterWall;
                        Rotation = 0;
                        break;
                    case WallStates.OuterVerti:
                        CurrentImage = MiddleOuterWall;
                        Rotation = 0;
                        break;
                    #endregion

                    #region Horizontals
                    case WallStates.OuterLeft:
                        CurrentImage = MiddleOuterWall;
                        Rotation = (float)(Math.PI * 1.5);
                        break;
                    case WallStates.OuterRight:
                        CurrentImage = MiddleOuterWall;
                        Rotation = (float)(Math.PI * 1.5);
                        break;
                    case WallStates.OuterHoriz:
                        CurrentImage = MiddleOuterWall;
                        Rotation = (float)(Math.PI * 1.5);
                        break;
                    #endregion

                    #region Corners
                    case WallStates.TopLeftCornerOW:
                        CurrentImage = CornerOuterWall;
                        Rotation = 0;
                        break;
                    case WallStates.TopRightCornerOW:
                        CurrentImage = CornerOuterWall;
                        Rotation = (float)(Math.PI * .5);
                        break;
                    case WallStates.BottomRightCornerOW:
                        CurrentImage = CornerOuterWall;
                        Rotation = (float)(Math.PI * 1);
                        break;
                    case WallStates.BottomLeftCornerOW:
                        CurrentImage = CornerOuterWall;
                        Rotation = (float)(Math.PI * 1.5);
                        break;
                    #endregion

                    #region OuterWalls intersecting with normal walls
                    case WallStates.TopIntersectingOW:
                        CurrentImage = SingleIntersectingOuterWall;
                        Rotation = (float)(Math.PI * .5);
                        break;
                    case WallStates.RightIntersectingOW:
                        CurrentImage = SingleIntersectingOuterWall;
                        Rotation = (float)(Math.PI * 1);
                        break;
                    case WallStates.BottomIntersectingOW:
                        CurrentImage = SingleIntersectingOuterWall;
                        Rotation = (float)(Math.PI * 1.5);
                        break;
                    case WallStates.LeftIntersectingOW:
                        CurrentImage = SingleIntersectingOuterWall;
                        Rotation = 0;
                        break;


                    case WallStates.TopLeftIntersectingOW:
                        CurrentImage = EdgeIntersectingOuterWall;
                        Rotation = (float)(Math.PI * .5);
                        break;
                    case WallStates.TopRightIntersectingOW:
                        CurrentImage = Edge2IntersectingOuterWall;
                        Rotation = (float)(Math.PI * .5);
                        break;
                    ///
                    case WallStates.RightTopIntersectingOW:
                        CurrentImage = EdgeIntersectingOuterWall;
                        Rotation = (float)(Math.PI * 1);
                        break;
                    case WallStates.RightBottomIntersectingOW:
                        CurrentImage = Edge2IntersectingOuterWall;
                        Rotation = (float)(Math.PI * 1);
                        break;
                    ///
                    case WallStates.BottomLeftIntersectingOW:
                        CurrentImage = Edge2IntersectingOuterWall;
                        Rotation = (float)(Math.PI * 1.5);
                        break;
                    case WallStates.BottomRightIntersectingOW:
                        CurrentImage = EdgeIntersectingOuterWall;
                        Rotation = (float)(Math.PI * 1.5);
                        break;
                    ///
                    case WallStates.LeftTopIntersectingOW:
                        CurrentImage = Edge2IntersectingOuterWall;
                        Rotation = 0;
                        break;
                    case WallStates.LeftBottomIntersectingOW:
                        CurrentImage = EdgeIntersectingOuterWall;
                        Rotation = 0;
                        break;

                    case WallStates.MiddleTopIntersectingOW:
                        CurrentImage = MiddleIntersectingOuterWall;
                        Rotation = (float)(Math.PI * .5);
                        break;
                    case WallStates.MiddleRightIntersectingOW:
                        CurrentImage = MiddleIntersectingOuterWall;
                        Rotation = (float)(Math.PI * 1);
                        break;
                    case WallStates.MiddleBottomIntersectingOW:
                        CurrentImage = MiddleIntersectingOuterWall;
                        Rotation = (float)(Math.PI * 1.5);
                        break;
                    case WallStates.MiddleLeftIntersectingOW:
                        CurrentImage = MiddleIntersectingOuterWall;
                        Rotation = 0;
                        break;
                        #endregion
                        #endregion
                }
            }

            if (PrevImage == null)
            {
                PrevImage = CurrentImage;
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
