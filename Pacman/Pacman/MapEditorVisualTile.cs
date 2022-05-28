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
        private Texture2D prevImage;

        public static Texture2D NormalSprite;
        public static Texture2D NormalEnlargedBorder;

        public static Texture2D PelletSprite;
        public static Texture2D PelletEnlargedBorder;

        public static Texture2D PowerPelletSprite;
        public static Texture2D PowerPelletEnlargedBorder;

        public static Texture2D LoneWallTile;

        public static Texture2D InteriorWall;

        public static Texture2D HorizWallTile;
        public static Texture2D HorizLeftWallTile;
        public static Texture2D HorizRightWallTile;

        public static Texture2D VertiWallTile;
        public static Texture2D VertiTopWallTile;
        public static Texture2D VertiBottomWallTile;

        public static Texture2D TopLeftWallTile;
        public static Texture2D TopRightWallTile;
        public static Texture2D BottomRightWallTile;
        public static Texture2D BottomLeftWallTile;

        public static Texture2D TopLeftFilledWallTile;
        public static Texture2D TopRightFilledWallTile;
        public static Texture2D BottomRightFilledWallTile;
        public static Texture2D BottomLeftFilledWallTile;

        public static Texture2D TopEdge;
        public static Texture2D RightEdge;
        public static Texture2D BottomEdge;
        public static Texture2D LeftEdge;

        public static Texture2D InteriorCross;

        public static Texture2D TopCross;
        public static Texture2D RightCross;
        public static Texture2D BottomCross;
        public static Texture2D LeftCross;

        #endregion

        //shift click like in gimp to fill in whatever is selected
        //also have a fill all

        public Rectangle Hitbox { get => new Rectangle((int)Position.X, (int)Position.Y, CurrentImage.Width, CurrentImage.Height); }

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

        public MapEditorVisualTile(Texture2D image, Point cord, Color tint, Vector2 offset) : base(image, Vector2.Zero, tint)
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
                    CurrentImage = HorizWallTile;
                    prevImage = HorizWallTile;
                    break;
                case WallStates.HorizLeftEnd:
                    CurrentImage = HorizLeftWallTile;
                    prevImage = HorizLeftWallTile;
                    break;
                case WallStates.HorizRightEnd:
                    CurrentImage = HorizRightWallTile;
                    prevImage = HorizRightWallTile;
                    break;

                case WallStates.Verti:
                    CurrentImage = VertiWallTile;
                    prevImage = VertiWallTile;
                    break;
                case WallStates.VertiTopEnd:
                    CurrentImage = VertiTopWallTile;
                    prevImage = VertiTopWallTile;
                    break;
                case WallStates.VertiBottomEnd:
                    CurrentImage = VertiBottomWallTile;
                    prevImage = VertiBottomWallTile;
                    break;

                case WallStates.TopLeftCorner:
                    CurrentImage = TopLeftWallTile;
                    prevImage = TopLeftWallTile;
                    break;
                case WallStates.TopRightCorner:
                    CurrentImage = TopRightWallTile;
                    prevImage = TopRightWallTile;
                    break;
                case WallStates.BottomRightCorner:
                    CurrentImage = BottomRightWallTile;
                    prevImage = BottomRightWallTile;
                    break;
                case WallStates.BottomLeftCorner:
                    CurrentImage = BottomLeftWallTile;
                    prevImage = BottomLeftWallTile;
                    break;

                case WallStates.TopLeftCornerFilled:
                    CurrentImage = TopLeftFilledWallTile;
                    prevImage = TopLeftFilledWallTile;
                    break;
                case WallStates.TopRightCornerFilled:
                    CurrentImage = TopRightFilledWallTile;
                    prevImage = TopRightFilledWallTile;
                    break;
                case WallStates.BottomRightCornerFilled:
                    CurrentImage = BottomRightFilledWallTile;
                    prevImage = BottomRightFilledWallTile;
                    break;
                case WallStates.BottomLeftCornerFilled:
                    CurrentImage = BottomLeftFilledWallTile;
                    prevImage = BottomLeftFilledWallTile;
                    break;

                case WallStates.TopEdge:
                    CurrentImage = TopEdge;
                    prevImage = TopEdge;
                    break;
                case WallStates.RightEdge:
                    CurrentImage = RightEdge;
                    prevImage = RightEdge;
                    break;
                case WallStates.BottomEdge:
                    CurrentImage = BottomEdge;
                    prevImage = BottomEdge;
                    break;
                case WallStates.LeftEdge:
                    CurrentImage = LeftEdge;
                    prevImage = LeftEdge;
                    break;

                case WallStates.TopCross:
                    CurrentImage = TopCross;
                    prevImage = TopCross;
                    break;
                case WallStates.RightCross:
                    CurrentImage = RightCross;
                    prevImage = RightCross;
                    break;
                case WallStates.BottomCross:
                    CurrentImage = BottomCross;
                    prevImage = BottomCross;
                    break;
                case WallStates.LeftCross:
                    CurrentImage = LeftCross;
                    prevImage = LeftCross;
                    break;


                case WallStates.InteriorWall:
                    CurrentImage = InteriorWall;
                    prevImage = InteriorWall;
                    break;
                case WallStates.InteriorCorner:
                    CurrentImage = InteriorCross;
                    prevImage = InteriorCross;
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
                            TileStates = States.Empty;
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
            batch.Draw(CurrentImage, Position, Tint);
        }
    }
}
