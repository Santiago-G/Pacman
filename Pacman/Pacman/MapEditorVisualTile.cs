using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Pacman
{
    public class MapEditorVisualTile : Sprite
    {           
        public Texture2D CurrentImage;

        #region Textures
        [JsonIgnore]
        public Texture2D prevImage;

        public static Texture2D NormalSprite;
        public static Texture2D NormalEnlargedBorder;

        public static Texture2D PelletSprite;
        public static Texture2D PelletEnlargedBorder;

        public static Texture2D PowerPelletSprite;
        public static Texture2D PowerPelletEnlargedBorder;

        public static Texture2D LoneWallTile;

        public static Texture2D InteriorWall;

        public static Texture2D MiddleWallPiece;

        public static Texture2D SingleWallEnd;

        public static Texture2D CornerWallTile;

        public static Texture2D CornerWallFilledTile;

        public static Texture2D EdgeTile;

        public static Texture2D InteriorCross;

        public static Texture2D SingleCross;

        public static Texture2D InteriorFilledCorner;


        #endregion

        //shift click like in gimp to fill in whatever is selected
        //also have a fill all

        public MapEditorDataTile Data { get; } = new MapEditorDataTile();

        public States TileStates
        {
            get
            {
                return Data.TileStates;
            }
            set
            {
                Data.TileStates = value;
            }
        }
        public WallStates WallStates
        {
            get
            {
                return Data.WallStates;
            }
            set
            {
                Data.WallStates = value;
            }
        }

        public Point Cord
        {
            get
            {
                return Data.Cord;
            }
            set
            {
                Data.Cord = value;
            }
        }

        public (Point Index, bool isWall)[] Neighbors => Data.Neighbors;

        private Vector2 offset;
        public override Vector2 Position
        {
            get
            {
                return new Vector2(Cord.Y * CurrentImage.Width, Cord.X * CurrentImage.Height) + offset;
            }
        }
        public override Color Tint
        {
            get
            {
                return Data.Tint;
            }
            set
            {
                Data.Tint = value;
            }
        }

        public MapEditorVisualTile(Texture2D image, Point cord, Color tint, Vector2 offset, Vector2 scale, Vector2 origin, float rotation, SpriteEffects spriteEffects) : base(image, Vector2.Zero, tint, scale, origin, rotation, spriteEffects)
        {
            CurrentImage = NormalSprite;
            prevImage = CurrentImage;
            Cord = cord;
            this.offset = offset;
        }

        public MapEditorVisualTile(MapEditorDataTile dataTile, Vector2 offset) : base(null, new Vector2(0), dataTile.Tint)
        {
            Data = dataTile;
            this.offset = offset;
            UpdateStates(true);
        }

        //fix both images when i hover over them

        public bool IsClicked()
        {
            return false;
        }

        public void UpdateStates(bool setDefault=false)
        {
            if (setDefault)
            {
                switch (TileStates)
                {
                    case States.Empty:
                        CurrentImage = NormalSprite;
                        prevImage = NormalEnlargedBorder;
                        break;
                    case States.Pellet:
                        CurrentImage = PelletSprite;
                        prevImage = PelletEnlargedBorder;
                        break;
                    case States.PowerPellet:
                        CurrentImage = PowerPelletSprite;
                        prevImage = PowerPelletEnlargedBorder;
                        break;
                    case States.Wall:

                        UpdateWalls();

                        break;
                }
            }
            else 
            {
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
                    case States.PowerPellet:
                        prevImage = PowerPelletSprite;
                        CurrentImage = PowerPelletEnlargedBorder;
                        break;
                    case States.Wall:
                        UpdateWalls();

                        break;
                }
            }      
        }

        public void UpdateWalls()
        {
            switch (WallStates)
            {
                case WallStates.LoneWall:
                    CurrentImage = LoneWallTile;
                    prevImage = LoneWallTile;
                    break;


                case WallStates.Horiz:
                    CurrentImage = MiddleWallPiece;
                    prevImage = MiddleWallPiece;

                    Rotation = (float)(Math.PI * .5);
                    break;
                case WallStates.HorizLeftEnd:
                    CurrentImage = SingleWallEnd;
                    prevImage = SingleWallEnd;

                    Rotation = (float)(Math.PI * 1.5);
                    break;
                case WallStates.HorizRightEnd:
                    CurrentImage = SingleWallEnd;
                    prevImage = SingleWallEnd;

                    Rotation = (float)(Math.PI * .5);
                    break;


                case WallStates.Verti:
                    CurrentImage = MiddleWallPiece;
                    prevImage = MiddleWallPiece;

                    Rotation = 0;
                    break;
                case WallStates.VertiTopEnd:
                    CurrentImage = SingleWallEnd;
                    prevImage = SingleWallEnd;

                    Rotation = 0;
                    break;
                case WallStates.VertiBottomEnd:
                    CurrentImage = SingleWallEnd;
                    prevImage = SingleWallEnd;

                    Rotation = (float)(Math.PI * 1);
                    break;


                case WallStates.TopLeftCorner:
                    CurrentImage = CornerWallTile;
                    prevImage = CornerWallTile;

                    Rotation = 0;
                    break;
                case WallStates.TopRightCorner:
                    CurrentImage = CornerWallTile;
                    prevImage = CornerWallTile;

                    Rotation = (float)(Math.PI * .5);
                    break;
                case WallStates.BottomRightCorner:
                    CurrentImage = CornerWallTile;
                    prevImage = CornerWallTile;

                    Rotation = (float)(Math.PI * 1);
                    break;
                case WallStates.BottomLeftCorner:
                    CurrentImage = CornerWallTile;
                    prevImage = CornerWallTile;

                    Rotation = (float)(Math.PI * 1.5);
                    break;


                case WallStates.TopLeftCornerFilled:
                    CurrentImage = CornerWallFilledTile;
                    prevImage = CornerWallFilledTile;

                    Rotation = 0;
                    break;
                case WallStates.TopRightCornerFilled:
                    CurrentImage = CornerWallFilledTile;
                    prevImage = CornerWallFilledTile;

                    Rotation = (float)(Math.PI * .5);
                    break;
                case WallStates.BottomRightCornerFilled:
                    CurrentImage = CornerWallFilledTile;
                    prevImage = CornerWallFilledTile;

                    Rotation = (float)(Math.PI * 1);
                    break;
                case WallStates.BottomLeftCornerFilled:
                    CurrentImage = CornerWallFilledTile;
                    prevImage = CornerWallFilledTile;

                    Rotation = (float)(Math.PI * 1.5);
                    break;


                case WallStates.TopEdge:
                    CurrentImage = EdgeTile;
                    prevImage = EdgeTile;

                    Rotation = 0;
                    break;
                case WallStates.RightEdge:
                    CurrentImage = EdgeTile;
                    prevImage = EdgeTile;

                    Rotation = (float)(Math.PI * .5);
                    break;
                case WallStates.BottomEdge:
                    CurrentImage = EdgeTile;
                    prevImage = EdgeTile;

                    Rotation = (float)(Math.PI * 1);
                    break;
                case WallStates.LeftEdge:
                    CurrentImage = EdgeTile;
                    prevImage = EdgeTile;

                    Rotation = (float)(Math.PI * 1.5);
                    break;

                case WallStates.TopCross:
                    CurrentImage = SingleCross;
                    prevImage = SingleCross;

                    Rotation = 0;
                    break;
                case WallStates.RightCross:
                    CurrentImage = SingleCross;
                    prevImage = SingleCross;

                    Rotation = (float)(Math.PI * .5);
                    break;
                case WallStates.BottomCross:
                    CurrentImage = SingleCross;
                    prevImage = SingleCross;

                    Rotation = (float)(Math.PI * 1);
                    break;
                case WallStates.LeftCross:
                    CurrentImage = SingleCross;
                    prevImage = SingleCross;

                    Rotation = (float)(Math.PI * 1.5);
                    break;


                case WallStates.InteriorWall:
                    CurrentImage = InteriorWall;
                    prevImage = InteriorWall;
                    break;
                case WallStates.InteriorCorner:
                    CurrentImage = InteriorCross;
                    prevImage = InteriorCross;
                    break;

                case WallStates.TopLeftInteriorFilledCorner:
                    CurrentImage = InteriorFilledCorner;
                    prevImage = InteriorFilledCorner;

                    Rotation = 0;
                    break;
                case WallStates.TopRightInteriorFilledCorner:
                    CurrentImage = InteriorFilledCorner;
                    prevImage = InteriorFilledCorner;

                    Rotation = (float)(Math.PI * .5);
                    break;
                case WallStates.BottomRightInteriorFilledCorner:
                    CurrentImage = InteriorFilledCorner;
                    prevImage = InteriorFilledCorner;

                    Rotation = (float)(Math.PI * 1);
                    break;
                case WallStates.BottomLeftInteriorFilledCorner:
                    CurrentImage = InteriorFilledCorner;
                    prevImage = InteriorFilledCorner;

                    Rotation = (float)(Math.PI * 1.5);
                    break;



                case WallStates.notAWall:
                    break;
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
                        case SelectedType.Pellet:
                            TileStates = States.Pellet;
                            break;
                        case SelectedType.PowerPellet:
                            TileStates = States.PowerPellet;
                            break;
                        case SelectedType.Eraser:
                            if (TileStates != States.Wall)
                            {
                                TileStates = States.Empty;
                            }
                            break;
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
                CurrentImage = prevImage;
            }

        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(CurrentImage, Position, null, Tint, Rotation, Origin, Scale, SpriteEffects, 0);
            //batch.Draw(Game1.Pixel, Hitbox, Color.Red * 0.3f);
        }
    }
}
