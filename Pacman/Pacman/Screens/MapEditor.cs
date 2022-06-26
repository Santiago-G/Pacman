using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pacman.Dummies;

namespace Pacman
{
    public class MapEditor : Screen
    {
        static public SelectedType selectedTileType = SelectedType.Default;

        public MapEditorGrid TileGrid;
        public MapEditorGrid PixelGrid;
        Vector2 GridOffest = new Vector2(40, 90);
        Point GridSize = new Point(29, 32);

        Image mapEditorImage;
        Texture2D mapEditorSprite;

        Texture2D pelletButtonSprite;
        Texture2D selectedPelletSprite;
        Button pelletButton;

        Texture2D eraserButtonSprite;
        Texture2D selectedEraserSprite;
        Button eraserButton;

        Texture2D powerPelletButtonSprite;
        Texture2D selectedPowerPelletSprite;
        Button powerPelletButton;

        Texture2D wallButtonSprite;
        Texture2D selectedWallSprite;
        Button wallButton;

        public MapEditor((int width, int height) Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            size = Size;
        }

        //enum that changes states when selecting walls, pelets, ect...

        public override void LoadContent(ContentManager Content)
        {
            mapEditorSprite = Content.Load<Texture2D>("mapEditorText");
            mapEditorImage = new Image(mapEditorSprite, new Vector2(800 - mapEditorSprite.Width / 2, 10), Color.White);
            objects.Add(mapEditorImage);

            pelletButtonSprite = Content.Load<Texture2D>("UnselectedPelletButton");
            selectedPelletSprite = Content.Load<Texture2D>("PelletButton");
            pelletButton = new Button(pelletButtonSprite, new Vector2(1000, 200), Color.White);
            objects.Add(pelletButton);

            eraserButtonSprite = Content.Load<Texture2D>("eraserButton");
            selectedEraserSprite = Content.Load<Texture2D>("selectedEraserButton");
            eraserButton = new Button(eraserButtonSprite, new Vector2(1000, 300), Color.White);
            objects.Add(eraserButton);

            powerPelletButtonSprite = Content.Load<Texture2D>("powerPelletButton");
            selectedPowerPelletSprite = Content.Load<Texture2D>("selectedPowerPellet");
            powerPelletButton = new Button(powerPelletButtonSprite, new Vector2(1000, 400), Color.White);
            objects.Add(powerPelletButton);

            wallButtonSprite = Content.Load<Texture2D>("wallButton");
            selectedWallSprite = Content.Load<Texture2D>("selectedWallButton");
            wallButton = new Button(wallButtonSprite, new Vector2(1000, 500), Color.White);
            objects.Add(wallButton);

            //ADD THING THAT IF SELECTED ONLY DRAWS OVER BLANK TILES

            MapEditorVisualWallTile.NormalSprite = Content.Load<Texture2D>("mapEditorTile");
            MapEditorVisualWallTile.NormalEnlargedBorder = Content.Load<Texture2D>("EnlargeBorderTile");
            MapEditorVisualWallTile.PelletSprite = Content.Load<Texture2D>("mapEditorTile2");
            MapEditorVisualWallTile.PelletEnlargedBorder = Content.Load<Texture2D>("enlargedPelletTile");
            MapEditorVisualWallTile.PowerPelletSprite = Content.Load<Texture2D>("powerPelletSprite");
            MapEditorVisualWallTile.PowerPelletEnlargedBorder = Content.Load<Texture2D>("enlargedPowerPelletSprite");

            MapEditorVisualWallTile.LoneWallTile = Content.Load<Texture2D>("loneWall"); 
            MapEditorVisualWallTile.InteriorWall = Content.Load<Texture2D>("InteriorWall");

            MapEditorVisualWallTile.MiddleWallPiece = Content.Load<Texture2D>("singleMiddleWall");
            MapEditorVisualWallTile.SingleWallEnd = Content.Load<Texture2D>("singleWallEnd");

            MapEditorVisualWallTile.CornerWallTile = Content.Load<Texture2D>("CornerWalls");
            
            MapEditorVisualWallTile.CornerWallFilledTile = Content.Load<Texture2D>("CornerWallsFilled");

            MapEditorVisualWallTile.EdgeTile = Content.Load<Texture2D>("EdgeTile");

            MapEditorVisualWallTile.InteriorCross = Content.Load<Texture2D>("Cross");

            MapEditorVisualWallTile.SingleCross = Content.Load<Texture2D>("SingleCross");

            MapEditorVisualWallTile.InteriorFilledCorner = Content.Load<Texture2D>("InteriorFilledCorner");

            TileGrid = new MapEditorGrid(GridSize, new Point(MapEditorVisualWallTile.NormalSprite.Width, MapEditorVisualWallTile.NormalSprite.Height), GridOffest);
            PixelGrid = new MapEditorGrid(new Point(29, 32), new Point(MapEditorVisualWallTile.));
        }

        MouseState prevms;
        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.Space))
            {
                string stringified = JsonConvert.SerializeObject(TileGrid.Tiles.Flatten().Select(tile => tile.Data));
                System.IO.File.WriteAllText("SavedMap.json", stringified);
            }

            if (kb.IsKeyDown(Keys.M))
            {
                string content = System.IO.File.ReadAllText("SavedMap.json");
                List<MapEditorDataWallTile> flattenedTiles = JsonConvert.DeserializeObject<List<MapEditorDataWallTile>>(content);
                TileGrid.LoadGrid(flattenedTiles);
                ;
            }

            TileGrid.Update(gameTime);

            if (pelletButton.IsClicked(ms))
            {
                if (selectedTileType != SelectedType.Pellet)
                {
                    selectedTileType = SelectedType.Pellet;
                    pelletButton.Image = selectedPelletSprite;

                    eraserButton.Image = eraserButtonSprite;
                    powerPelletButton.Image = powerPelletButtonSprite;
                    wallButton.Image = wallButtonSprite;
                }
                else
                {
                    selectedTileType = SelectedType.Default;
                    pelletButton.Image = pelletButtonSprite;
                }
            }

            else if (eraserButton.IsClicked(ms))
            {
                if (selectedTileType != SelectedType.Eraser)
                {
                    selectedTileType = SelectedType.Eraser;
                    eraserButton.Image = selectedEraserSprite;

                    pelletButton.Image = pelletButtonSprite;
                    powerPelletButton.Image = powerPelletButtonSprite;
                    wallButton.Image = wallButtonSprite;
                }
                else
                {
                    selectedTileType = SelectedType.Default;
                    eraserButton.Image = eraserButtonSprite;
                }
            }

            else if (powerPelletButton.IsClicked(ms))
            {
                if (selectedTileType != SelectedType.PowerPellet)
                {
                    selectedTileType = SelectedType.PowerPellet;
                    powerPelletButton.Image = selectedPowerPelletSprite;

                    pelletButton.Image = pelletButtonSprite;
                    eraserButton.Image = eraserButtonSprite;
                    wallButton.Image = wallButtonSprite;
                }
                else
                {
                    selectedTileType = SelectedType.Default;
                    powerPelletButton.Image = powerPelletButtonSprite;
                }
            }

            else if (wallButton.IsClicked(ms))
            {
                if (selectedTileType != SelectedType.Wall)
                {
                    selectedTileType = SelectedType.Wall;
                    wallButton.Image = selectedWallSprite;

                    pelletButton.Image = pelletButtonSprite;
                    eraserButton.Image = eraserButtonSprite;
                    powerPelletButton.Image = powerPelletButtonSprite;
                }
                else
                {
                    selectedTileType = SelectedType.Default;
                    wallButton.Image = wallButtonSprite;
                }
            }

            if (selectedTileType == SelectedType.Wall)
            {
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    TileGrid.addWall(new Vector2(ms.Position.X, ms.Position.Y));
                }
            }

            if (selectedTileType == SelectedType.Eraser)
            {
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    TileGrid.removeWall(new Vector2(ms.Position.X, ms.Position.Y));
                }
            }

            prevms = ms;

            Game1.WindowText = $"{TileGrid.PosToIndex(new Vector2(ms.Position.X, ms.Position.Y))}, Raw MS {ms.Position}";

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            TileGrid.Draw(spriteBatch);

            spriteBatch.Draw(MapEditorVisualWallTile.NormalSprite, new Vector2(40, 80),Color.White);

            base.Draw(spriteBatch);
        }
    }
}
