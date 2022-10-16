using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pacman.Dummies;
using System.Security.Cryptography.X509Certificates;

namespace Pacman
{
    public class MapEditor : Screen
    {
        static public SelectedType selectedTileType = SelectedType.Default;

        public static GridStates currentGridState = GridStates.WallGrid;

        public MapEditorPixelGrid PelletGrid;
        public MapEditorWallGrid WallGrid;

        static Vector2 wallGridOffest = new Vector2(40, 90);
        static Point wallGridSize = new Point(29, 32);

        Vector2 pixelGridOffest;
        Point pixelGridSize;

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

        Texture2D outerWallButtonSprite;
        Texture2D selectedOuterWallSprite;
        Button outerWallButton;

        Texture2D switchButtonSprite;
        Texture2D HLswitchButtonSprite;
        Button switchGridButton;

        Texture2D ghostChamberTexture;
        public static Button ghostChamberButton;
        public static Image ghostChamberMS;
        public static bool selectedGhostChamber = false;
        public static bool ghostChamberPlaced = false;

        Texture2D pmPlacementSprite;
        public static Button pacmanPlacementButton;
        public static Image pacmanTileIcon;
        public static bool selectedPacman = false;
        public static bool pacmanPlaced = false;


        public MapEditor((int width, int height) Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            size = Size;
        }

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
            eraserButton = new Button(eraserButtonSprite, new Vector2(1000, 600), Color.White);
            objects.Add(eraserButton);

            powerPelletButtonSprite = Content.Load<Texture2D>("powerPelletButton");
            selectedPowerPelletSprite = Content.Load<Texture2D>("selectedPowerPellet");
            powerPelletButton = new Button(powerPelletButtonSprite, new Vector2(1000, 300), Color.White);
            objects.Add(powerPelletButton);

            wallButtonSprite = Content.Load<Texture2D>("wallButton");
            selectedWallSprite = Content.Load<Texture2D>("selectedWallButton");
            wallButton = new Button(wallButtonSprite, new Vector2(1000, 400), Color.White);
            objects.Add(wallButton);

            outerWallButtonSprite = Content.Load<Texture2D>("outerWallButton");
            selectedOuterWallSprite = Content.Load<Texture2D>("selectedOuterWallButton");
            outerWallButton = new Button(outerWallButtonSprite, new Vector2(1000, 500), Color.White);
            objects.Add(outerWallButton);

            switchButtonSprite = Content.Load<Texture2D>("switchButton");
            HLswitchButtonSprite = Content.Load<Texture2D>("HLswitchButton");
            switchGridButton = new Button(switchButtonSprite, new Vector2(830, 90), Color.White);
            objects.Add(switchGridButton);

            ghostChamberTexture = Content.Load<Texture2D>("pacManGhostChamber");
            ghostChamberButton = new Button(ghostChamberTexture, new Vector2(1000, 700), Color.White);
            ghostChamberMS = new Image(ghostChamberTexture, new Vector2(-200), Color.White, new Vector2(1), Vector2.Zero, 0f, SpriteEffects.None);
            objects.Add(ghostChamberButton);
            objects.Add(ghostChamberMS);

            pmPlacementSprite = Content.Load<Texture2D>("pacManButton");
            pacmanTileIcon = new Image(Content.Load<Texture2D>("pacManTileImage"), new Vector2(-200), Color.White);
            pacmanPlacementButton = new Button(pmPlacementSprite, new Vector2(1000, 850), Color.White);
            objects.Add(pacmanPlacementButton);
            objects.Add(pacmanTileIcon);


            //ADD THING THAT IF SELECTED ONLY DRAWS OVER BLANK TILES

            pixelVisual.EmptySprite = Content.Load<Texture2D>("emptyPelletTile");
            pixelVisual.HLEmptySprite = Content.Load<Texture2D>("HLEmptyPelletTile");
            pixelVisual.PelletSprite = Content.Load<Texture2D>("mapEditorTile2");
            pixelVisual.HLPelletSprite = Content.Load<Texture2D>("enlargedPelletTile");
            pixelVisual.PowerPelletSprite = Content.Load<Texture2D>("powerPelletSprite");
            pixelVisual.HLPowerPelletSprite = Content.Load<Texture2D>("enlargedPowerPelletSprite");

            pixelVisual.NBemptySprite = Content.Load<Texture2D>("emptyTile");
            pixelVisual.NBpelletSprite = Content.Load<Texture2D>("NBpixelTile");
            pixelVisual.NBpowerPelletSprite = Content.Load<Texture2D>("NBpowerPelletTile");

            wallVisual.NBemptySprite = pixelVisual.NBemptySprite;

            wallVisual.EmptySprite = Content.Load<Texture2D>("mapEditorTile");
            wallVisual.HLEmptySprite = Content.Load<Texture2D>("EnlargeBorderTile");

            wallVisual.LoneWallSprite = Content.Load<Texture2D>("loneWall");
            wallVisual.InteriorWallSprite = Content.Load<Texture2D>("InteriorWall");

            wallVisual.MiddleWallSprite = Content.Load<Texture2D>("singleMiddleWall");
            wallVisual.SingleWallEnd = Content.Load<Texture2D>("singleWallEnd");

            wallVisual.CornerWallFilledTile = Content.Load<Texture2D>("CornerWallsFilled");

            wallVisual.CornerWallTile = wallVisual.CornerWallFilledTile;

            wallVisual.EdgeSprite = Content.Load<Texture2D>("EdgeTile");

            wallVisual.InteriorCrossSprite = Content.Load<Texture2D>("Cross");

            //wallVisual.SingleCrossSprite = Content.Load<Texture2D>("SingleCross");
            wallVisual.SingleCrossSprite = wallVisual.EdgeSprite;

            wallVisual.InteriorFilledCorner = Content.Load<Texture2D>("InteriorFilledCorner");

            wallVisual.MiddleOuterWall = Content.Load<Texture2D>("singleMiddleOuterWall");
            wallVisual.CornerOuterWall = Content.Load<Texture2D>("cornerOuterWall");
            wallVisual.SingleIntersectingOuterWall = Content.Load<Texture2D>("singleIntersectingOuterWall");

            pixelGridSize = new Point(wallGridSize.X - 1, wallGridSize.Y - 1);
            pixelGridOffest = new Vector2(wallGridOffest.X + pixelVisual.EmptySprite.Width / 2, wallGridOffest.Y + pixelVisual.EmptySprite.Height / 2);

            WallGrid = new MapEditorWallGrid(wallGridSize, new Point(wallVisual.EmptySprite.Width, wallVisual.EmptySprite.Height), wallGridOffest);
            wallVisual.Grid = WallGrid;

            PelletGrid = new MapEditorPixelGrid(pixelGridSize, new Point(pixelVisual.EmptySprite.Width, pixelVisual.EmptySprite.Height), pixelGridOffest);
            pixelVisual.Grid = PelletGrid;
        }

        MouseState prevms;
        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            KeyboardState kb = Keyboard.GetState();

            /* TO DO LIST
             * 
             * IMPORTANT
             * ---------
             * 
             * Fix outer walls "infecting" normal walls
             * 
             * Fix the pacman sprite not spawning when loading a map
             * 
             * Do Portals
             * Do Fruits
             * 
             * Make sure you check if all the outside walls are valid when the user is done making the map.
             * 
             * For the ghosts, once you place the ghost chamber?, make sure they can reach every empty tile in the map via DFS or BFS.
             *
             * Before you can "save" the map, make sure it fills out a list of requirements (like having a ghost chamber.)
             * 
             * Quality of Life
             * ---------------
             * 
             * KEYBINDS (And have right click be delete)
             * Update textures and placement
             * If you hover over a button, a small info box tells you the name and what it does. General Info
             * Button that toggles drawing over objects
             * Shift Click like in GIMP
             * Maybe have a finished view? (without grayed out tiles and borders)
             * 
             */

            #region Saving and Loading

            if (kb.IsKeyDown(Keys.Space))
            {
                GridStates prevCurrGridState = currentGridState;

                PelletGrid.GoInFocus(WallGrid.FilledTiles);
                WallGrid.GoInFocus(PelletGrid.GetFilledTiles());

                string stringifiedPellets = JsonConvert.SerializeObject(PelletGrid.Tiles.Flatten().Select(tile => tile.Data));
                System.IO.File.WriteAllText("SavedPelletMap.json", stringifiedPellets);

                string stringifiedWalls = JsonConvert.SerializeObject(WallGrid.Tiles.Flatten().Select(tile => tile.Data));
                System.IO.File.WriteAllText("SavedWallMap.json", stringifiedWalls);

                if (prevCurrGridState == GridStates.PixelGrid)
                {
                    WallGrid.GoTransparent();
                }
                else
                {
                    PelletGrid.GoTransparent();
                }

                currentGridState = prevCurrGridState;
                ;
            }

            if (kb.IsKeyDown(Keys.M))
            {
                if (System.IO.File.Exists("SavedPelletMap.json"))
                {
                    string pelletContent = System.IO.File.ReadAllText("SavedPelletMap.json");
                    string wallContent = System.IO.File.ReadAllText("SavedWallMap.json");

                    List<pixelData> flattenedPelletTiles = JsonConvert.DeserializeObject<List<pixelData>>(pelletContent);
                    List<wallData> flattenedWallTiles = JsonConvert.DeserializeObject<List<wallData>>(wallContent);
                    //tiles = flattenedTiles.Select(x => new MapEditorVisualTile(.To2DArray(/*maybe needs parameters*/);
                    //Create grid based on this two d array
                    PelletGrid.LoadGrid(flattenedPelletTiles);
                    WallGrid.LoadGrid(flattenedWallTiles);

                    if (currentGridState == GridStates.PixelGrid)
                    {
                        PelletGrid.GoTransparent();
                        PelletGrid.GoInFocus(WallGrid.FilledTiles);

                        WallGrid.GoTransparent();
                    }
                    else
                    {
                        WallGrid.GoTransparent();
                        WallGrid.GoInFocus(PelletGrid.FilledTiles);

                        PelletGrid.GoTransparent();
                    }
                }
            }
            #endregion


            //Switching Grids
            if (switchGridButton.IsClicked(ms))
            {
                if (!pacmanPlaced)
                {
                    selectedPacman = false;
                    pacmanPlacementButton.Tint = Color.White;
                    pacmanTileIcon.Position = new Vector2(-200);
                }

                if (currentGridState == GridStates.PixelGrid)
                {
                    currentGridState = GridStates.WallGrid;
                    PelletGrid.GoTransparent();
                    WallGrid.GoInFocus(PelletGrid.FilledTiles);

                    pelletButton.Tint = Color.Gray;
                    powerPelletButton.Tint = Color.Gray;
                    wallButton.Tint = Color.White;
                    ghostChamberButton.Tint = Color.White;
                }
                else
                {
                    currentGridState = GridStates.PixelGrid;
                    WallGrid.GoTransparent();
                    PelletGrid.GoInFocus(WallGrid.FilledTiles);

                    wallButton.Tint = Color.Gray;
                    ghostChamberButton.Tint = Color.Gray;
                    powerPelletButton.Tint = Color.White;
                    pelletButton.Tint = Color.White;
                }
            }


            if (currentGridState == GridStates.WallGrid)
            {
                if (selectedGhostChamber)
                {
                    Point index = WallGrid.PosToIndex(ms.Position.ToVector2());
                    int concord = 0;

                    if (index != new Point(-1) && index.X < wallGridSize.X - 6 && index.Y < wallGridSize.Y - 3)
                    {
                        ghostChamberMS.Position = new Vector2(WallGrid.Tiles[index.Y, index.X].Position.X - pixelVisual.EmptySprite.Width / 2, WallGrid.Tiles[index.Y, index.X].Position.Y - pixelVisual.EmptySprite.Height / 2);
                        concord = 1;
                    }
                    else
                    {
                        ghostChamberMS.Position = new Vector2(ms.Position.X, ms.Position.Y);
                        concord = 0;
                    }

                    if (ms.LeftButton == ButtonState.Released)
                    {
                        selectedGhostChamber = false;

                        if (concord > 0 && WallGrid.CanPlaceGC(index))
                        {
                            //check if theres things inside it.
                            ghostChamberPlaced = true;
                            WallGrid.PlaceGhostChamber(new Point(index.Y, index.X));
                        }
                        else
                        {
                            ghostChamberButton.Tint = Color.White;
                            ghostChamberMS.Position = new Vector2(-200);
                        }
                    }
                }
                else
                {
                    if (wallButton.IsClicked(ms))
                    {
                        if (selectedTileType != SelectedType.Wall)
                        {
                            selectedTileType = SelectedType.Wall;
                            wallButton.Image = selectedWallSprite;

                            pelletButton.Image = pelletButtonSprite;
                            eraserButton.Image = eraserButtonSprite;
                            powerPelletButton.Image = powerPelletButtonSprite;
                            outerWallButton.Image = outerWallButtonSprite;

                            if (!pacmanPlaced)
                            {
                                selectedPacman = false;
                                pacmanPlacementButton.Tint = Color.White;
                                pacmanTileIcon.Position = new Vector2(-200);
                            }
                        }
                        else
                        {
                            selectedTileType = SelectedType.Default;
                            wallButton.Image = wallButtonSprite;
                        }
                    }

                    if (outerWallButton.IsClicked(ms))
                    {
                        if (selectedTileType != SelectedType.OuterWall)
                        {
                            selectedTileType = SelectedType.OuterWall;
                            outerWallButton.Image = selectedOuterWallSprite;

                            wallButton.Image = wallButtonSprite;
                            pelletButton.Image = pelletButtonSprite;
                            eraserButton.Image = eraserButtonSprite;
                            powerPelletButton.Image = powerPelletButtonSprite;

                            if (!pacmanPlaced)
                            {
                                selectedPacman = false;
                                pacmanPlacementButton.Tint = Color.White;
                                pacmanTileIcon.Position = new Vector2(-200);
                            }
                        }
                        else 
                        {
                            selectedTileType = SelectedType.Default;
                            outerWallButton.Image = outerWallButtonSprite;
                        }
                    }

                    if (!ghostChamberPlaced && ghostChamberButton.IsClicked(ms))
                    {
                        selectedTileType = SelectedType.Default;
                        wallButton.Image = wallButtonSprite;
                        eraserButton.Image = eraserButtonSprite;
                        outerWallButton.Image = outerWallButtonSprite;

                        selectedGhostChamber = true;
                        ghostChamberButton.Tint = Color.Gray;
                    }

                    if (selectedPacman)
                    {
                        Point index = WallGrid.PosToIndex(ms.Position.ToVector2());

                        if (index != new Point(-1))
                        {
                            //make a small pacman image, and have it follow "lock" the mouse
                            pacmanTileIcon.Position = new Vector2(WallGrid.Tiles[index.Y, index.X].Position.X - pixelVisual.EmptySprite.Width / 2, WallGrid.Tiles[index.Y, index.X].Position.Y - pixelVisual.EmptySprite.Height / 2);

                            if (ms.LeftButton == ButtonState.Pressed && WallGrid.Tiles[index.Y, index.X].TileStates == States.Empty && WallGrid.Tiles[index.Y, index.X + 1].TileStates == States.Empty && WallGrid.Tiles[index.Y + 1, index.X].TileStates == States.Empty)
                            {
                                WallGrid.AddPacman(index);
                            }
                        }
                    }

                    if (selectedTileType == SelectedType.Wall || selectedTileType == SelectedType.OuterWall)
                    {
                        if (ms.LeftButton == ButtonState.Pressed)
                        {
                            WallGrid.addWall(new Vector2(ms.Position.X, ms.Position.Y));
                        }
                    }
                    else if (selectedTileType == SelectedType.Eraser)
                    {
                        if (ms.LeftButton == ButtonState.Pressed)
                        {
                            WallGrid.removeWall(new Vector2(ms.Position.X, ms.Position.Y));
                        }
                    }
                }
            }
            else
            {
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

                if (selectedPacman)
                {
                    Point index = PelletGrid.PosToIndex(ms.Position.ToVector2());

                    if (index != new Point(-1))
                    {
                        pacmanTileIcon.Position = new Vector2(PelletGrid.Tiles[index.Y, index.X].Position.X - pacmanTileIcon.Image.Width / 4, PelletGrid.Tiles[index.Y, index.X].Position.Y - pacmanTileIcon.Image.Height / 2);

                        if (ms.LeftButton == ButtonState.Pressed && PelletGrid.Tiles[index.Y, index.X].TileStates == States.Empty)
                        {

                            if (PelletGrid.Tiles[index.Y, index.X + 1].TileStates == States.Empty)
                            {
                                PelletGrid.AddPacman(index);
                            }
                        }
                    }
                }
            }

            if (!selectedGhostChamber)
            {
                if (eraserButton.IsClicked(ms))
                {
                    if (selectedTileType != SelectedType.Eraser)
                    {
                        selectedTileType = SelectedType.Eraser;
                        eraserButton.Image = selectedEraserSprite;

                        pelletButton.Image = pelletButtonSprite;
                        powerPelletButton.Image = powerPelletButtonSprite;
                        wallButton.Image = wallButtonSprite;

                        if (!pacmanPlaced)
                        {
                            selectedPacman = false;
                            pacmanPlacementButton.Tint = Color.White;
                            pacmanTileIcon.Position = new Vector2(-200);
                        }
                    }
                    else
                    {
                        selectedTileType = SelectedType.Default;
                        eraserButton.Image = eraserButtonSprite;
                    }
                }

                if (!pacmanPlaced && pacmanPlacementButton.IsClicked(ms))
                {
                    if (selectedPacman)
                    {
                        selectedPacman = !selectedPacman;
                        pacmanPlacementButton.Tint = Color.White;
                        pacmanTileIcon.Position = new Vector2(-200);
                    }
                    else
                    {
                        selectedTileType = SelectedType.Default;
                        wallButton.Image = wallButtonSprite;
                        eraserButton.Image = eraserButtonSprite;

                        selectedPacman = true;
                        pacmanPlacementButton.Tint = Color.Gray;
                    }
                }
            }

            prevms = ms;

            Game1.WindowText = $"{WallGrid.PosToIndex(new Vector2(ms.Position.X, ms.Position.Y))}, Raw MS {ms.Position}";

            base.Update(gameTime);
            if (currentGridState == GridStates.WallGrid)
            {
                WallGrid.Update(gameTime);
            }
            else
            {
                PelletGrid.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            WallGrid.Draw(spriteBatch);
            PelletGrid.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
