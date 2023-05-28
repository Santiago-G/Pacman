using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using MonoGame.Extended;
using System.Net.Security;
using LePacman;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using Pacman;
using System.IO;

namespace LePacman.Screens.MapEditor
{
    public class MapEditor : Screen
    {        
        public record struct SavedMap
        {
            public string Name { get; set; }

            public pixelData[] PixelTiles { get; set; }
            public wallData[] WallTiles { get; set; }
            public PortalPair[] Portals { get; set; }

            public SavedMap(string name, pixelData[] pixelTiles, wallData[] wallTiles, PortalPair[] portals)
            {
                Name = name;
                PixelTiles = pixelTiles;
                WallTiles = wallTiles;
                Portals = portals;
            }
        }

        static public SelectedType selectedTileType = SelectedType.Default;
        private SelectedType prevSelctedTileType;
        bool eraserSelected = false;

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

        Texture2D fruitButtonSprite;
        Texture2D selectedFruitButtonSprite;
        public static Button fruitButton;
        public static Image fruitIcon;

        Texture2D[] fruitImages;
        int fruitIndex;

        Texture2D switchButtonSprite;
        Button switchGridButton;

        //Left one
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

        public static bool isFruitPlaced = false;
        public static bool selectedFruit = false;

        Texture2D portalButtonSprite;
        Button generatePortalButton;

        Texture2D errorBackground;
        Texture2D errorBackgroundTwo;
        Texture2D errorBackgroundThree;
        SpriteFont errorHeaderFont;
        SpriteFont errorBodyFont;

        TimeSpan fruitTarget = new TimeSpan();

        public MapEditor(Point Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
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
            //HLswitchButtonSprite = Content.Load<Texture2D>("HLswitchButton");
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

            portalButtonSprite = Content.Load<Texture2D>("portalButton");
            generatePortalButton = new Button(portalButtonSprite, new Vector2(1000, 900), Color.White);
            objects.Add(generatePortalButton);

            fruitButtonSprite = Content.Load<Texture2D>("fruitButton");
            selectedFruitButtonSprite = Content.Load<Texture2D>("selectedFruitButton");
            fruitButton = new Button(fruitButtonSprite, new Vector2(1100, 600), Color.White);

            fruitImages = new Texture2D[5] { Content.Load<Texture2D>("cherryFruit"), Content.Load<Texture2D>("strawberryFruit"),
                Content.Load<Texture2D>("orangeFruit"), Content.Load<Texture2D>("appleFruit"), Content.Load<Texture2D>("melonFruit") };


            fruitIcon = new Image(fruitImages[0], new Vector2(-1000), Color.White);
            objects.Add(fruitButton);
            objects.Add(fruitIcon);

            errorBackground = Content.Load<Texture2D>("outerWallErrorMSGBG");
            errorBackgroundTwo = Content.Load<Texture2D>("errorBackgroundTwo");
            errorBackgroundThree = Content.Load<Texture2D>("errorBackgroundThree");
            errorBackground = Content.Load<Texture2D>("errorBackgroundThree");
            errorHeaderFont = Content.Load<SpriteFont>("ErrorHeaderFont");
            errorBodyFont = Content.Load<SpriteFont>("ErrorBodyText");

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

            WallVisual.NBemptySprite = pixelVisual.NBemptySprite;

            WallVisual.EmptySprite = Content.Load<Texture2D>("mapEditorTile");
            WallVisual.HLEmptySprite = Content.Load<Texture2D>("EnlargeBorderTile");

            WallVisual.LoneWallSprite = Content.Load<Texture2D>("loneWall");
            WallVisual.InteriorWallSprite = Content.Load<Texture2D>("InteriorWall");

            WallVisual.MiddleWallSprite = Content.Load<Texture2D>("singleMiddleWall");
            WallVisual.SingleWallEnd = Content.Load<Texture2D>("singleWallEnd");

            WallVisual.CornerWallFilledTile = Content.Load<Texture2D>("CornerWallsFilled");

            WallVisual.CornerWallTile = WallVisual.CornerWallFilledTile;

            WallVisual.EdgeSprite = Content.Load<Texture2D>("EdgeTile");

            WallVisual.InteriorCrossSprite = Content.Load<Texture2D>("Cross");

            WallVisual.SingleCrossSprite = WallVisual.EdgeSprite;

            WallVisual.InteriorFilledCorner = Content.Load<Texture2D>("InteriorFilledCorner");

            WallVisual.MiddleOuterWall = Content.Load<Texture2D>("singleMiddleOuterWall");
            WallVisual.CornerOuterWall = Content.Load<Texture2D>("cornerOuterWall");
            WallVisual.SingleIntersectingOuterWall = Content.Load<Texture2D>("singleIntersectingOuterWall");
            WallVisual.MiddleIntersectingOuterWall = Content.Load<Texture2D>("middleIntersectingOuterWall");
            WallVisual.EdgeIntersectingOuterWall = Content.Load<Texture2D>("edgeIntersectingOuterWall");
            WallVisual.Edge2IntersectingOuterWall = Content.Load<Texture2D>("edge2IntersectingOuterWall");

            WallVisual.emptyWallError = Content.Load<Texture2D>("errorEmptyTile");
            WallVisual.singleOWError = Content.Load<Texture2D>("middleOWError");

            pixelGridSize = new Point(wallGridSize.X - 1, wallGridSize.Y - 1);
            pixelGridOffest = new Vector2(wallGridOffest.X + pixelVisual.EmptySprite.Width / 2, wallGridOffest.Y + pixelVisual.EmptySprite.Height / 2);

            WallGrid = new MapEditorWallGrid(wallGridSize, new Point(WallVisual.EmptySprite.Width, WallVisual.EmptySprite.Height), wallGridOffest);
            WallVisual.Grid = WallGrid;

            PelletGrid = new MapEditorPixelGrid(pixelGridSize, new Point(pixelVisual.EmptySprite.Width, pixelVisual.EmptySprite.Height), pixelGridOffest);
            pixelVisual.Grid = PelletGrid;
        }

        MouseState prevms;

        private void deselectWallButtons()
        {
            selectedTileType = SelectedType.Default;

            wallButton.Image = wallButtonSprite;
            pelletButton.Image = pelletButtonSprite;
            eraserButton.Image = eraserButtonSprite;
            powerPelletButton.Image = powerPelletButtonSprite;
            outerWallButton.Image = outerWallButtonSprite;

            if (selectedPacman)
            {
                pacmanPlacementButton.Image = pmPlacementSprite;
                pacmanPlacementButton.Tint = Color.White;
                pacmanTileIcon.Position = new Vector2(-200);
                selectedPacman = false;
            }
        }

        private void deselectPelletButtons()
        {
            selectedTileType = SelectedType.Default;

            fruitButton.Image = fruitButtonSprite;
            pelletButton.Image = pelletButtonSprite;
            eraserButton.Image = eraserButtonSprite;
            powerPelletButton.Image = powerPelletButtonSprite;
            wallButton.Image = wallButtonSprite;
        }


        private void duplicatePortalCheck()
        {
            bool dupeFound = false;
            Point currPortalTileCoord;

            for (int i = 0; i < WallGrid.Portals.Count - 1; i++)
            {
                currPortalTileCoord = WallGrid.Portals[i].firstPortal.firstTile.Cord;
                for (int j = i + 1; j < WallGrid.Portals.Count; j++)
                {
                    if (currPortalTileCoord == WallGrid.Portals[j].firstPortal.firstTile.Cord) { dupeFound = true; }
                    else if (currPortalTileCoord == WallGrid.Portals[j].firstPortal.secondTile.Cord) { dupeFound = true; }
                    else if (currPortalTileCoord == WallGrid.Portals[j].secondPortal.firstTile.Cord) { dupeFound = true; }
                    else if (currPortalTileCoord == WallGrid.Portals[j].secondPortal.secondTile.Cord) { dupeFound = true; }

                    if (dupeFound)
                    {
                        WallGrid.Portals.Remove(WallGrid.Portals[j]);
                    }
                }
            }
        }

        private bool ValidityChecks()
        {
            //Check if outer walls are valid*
            var outerWallCheckResult = WallGrid.FindInvalidOuterWalls();

            foreach (var error in outerWallCheckResult)
            {
                PopUpManager.Instance.EnqueuePopUp(new ErrorPopUp(errorBackground, new Point(500), error.InvalidTiles.First().Position, errorHeaderFont, errorBodyFont, "Error!", error.ErrorMsg, error.InvalidTiles));
                deselectWallButtons();
                return false;
            }

            duplicatePortalCheck();

            if (!ghostChamberPlaced)
            {
                PopUpManager.Instance.EnqueuePopUp(new ErrorPopUp(errorBackgroundTwo, new Point(500), new Vector2(1090, 740), errorHeaderFont, errorBodyFont, "Error!", "No Ghost Chamber found!", new List<WallVisual>()));
                return false;
            }

            WallGrid.Tiles[WallGrid.ghostChamberTiles[3, 0].Cord.X, WallGrid.ghostChamberTiles[3, 0].Cord.Y - 1].Tint = Color.Red;
            switchGrids();
            switchGrids();

            var pelletCheckResult = PelletGrid.longJacket(WallGrid);

            if (!pelletCheckResult.pacManValid)
            {
                PopUpManager.Instance.EnqueuePopUp(new ErrorPopUp(errorBackgroundThree, new Point(500), new Vector2(1020, 700), errorHeaderFont, errorBodyFont, "Error!", "No Pacman found!", new List<WallVisual>()));
                return false;
            }

            foreach (var error in pelletCheckResult.invalidPellets)
            {
                PopUpManager.Instance.EnqueuePopUp(new ErrorPopUp(errorBackground, new Point(500), error.Position, errorHeaderFont, errorBodyFont, "Error!", "Pacman and the Ghosts cannot reach these tiles!", new List<WallVisual>()));
                return false;
            }

            return true;
        }

        private void FinishMap()
        {
            if (!ValidityChecks()) { return; }

            SavedMap newMap = new SavedMap("CrackedActor", PelletGrid.Tiles.Flatten().Select(tile => tile.Data).ToArray(),
                WallGrid.Tiles.Flatten().Select(tile => tile.Data).ToArray(), WallGrid.Portals.ToArray());

            SaveMap.currentMap = newMap;

            ScreenManagerPM.Instance.screens[GameStates.SaveMap].background = ScreenManagerPM.Instance.GetBackgroundImage();
            ScreenManagerPM.Instance.ChangeScreens(GameStates.SaveMap);


            string SerializedMap100PercentTrustmebro = "hi";
            string MapName = "bob";
            //Have 3 files: Map1, Map2, Map3.  Write the map to the corrisponding file
            File.WriteAllText(MapName + ".json", SerializedMap100PercentTrustmebro);

            //Give the option between Save & Load, or just Save
        }

        public void switchGrids()
        {
            if (!pacmanPlaced)
            {
                selectedPacman = false;
                pacmanPlacementButton.Tint = Color.White;
                pacmanTileIcon.Position = new Vector2(-200);
            }
            if (!ghostChamberPlaced)
            {
                selectedGhostChamber = false;
                ghostChamberMS.Position = new Vector2(-300);
            }
            if (!isFruitPlaced)
            {
                selectedFruit = false;
                fruitIcon.Position = new Vector2(-1000);
            }

            if (currentGridState == GridStates.PixelGrid)
            {
                currentGridState = GridStates.WallGrid;
                PelletGrid.GoTransparent();
                WallGrid.GoInFocus(PelletGrid.FilledTiles);

                pelletButton.Tint = Color.Gray;
                powerPelletButton.Tint = Color.Gray;
                wallButton.Tint = Color.White;
                outerWallButton.Tint = Color.White;
                ghostChamberButton.Tint = Color.White;

                if (!isFruitPlaced)
                {
                    fruitButton.Tint = Color.Gray;
                }
            }
            else
            {
                currentGridState = GridStates.PixelGrid;
                WallGrid.GoTransparent();
                PelletGrid.GoInFocus(WallGrid.FilledTiles);

                wallButton.Tint = Color.Gray;
                ghostChamberButton.Tint = Color.Gray;
                outerWallButton.Tint = Color.Gray;
                powerPelletButton.Tint = Color.White;
                pelletButton.Tint = Color.White;

                if (!isFruitPlaced)
                {
                    fruitButton.Tint = Color.White;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            KeyboardState kb = Keyboard.GetState();

            //CircleF outer wall breaks

            /* TO DO LIST
             * 
             * IMPORTANT
             * ---------
             *
             * You can save up to 3 different maps. 
             *
             * FIX certain chars from not appearing in the font
             * 

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


            if (ms.RightButton == ButtonState.Pressed && !eraserSelected && !selectedPacman)
            {
                eraserSelected = true;
                prevSelctedTileType = selectedTileType;
                selectedTileType = SelectedType.AltEraser;
                eraserButton.Image = selectedEraserSprite;
            }

            if (eraserSelected && ms.RightButton != ButtonState.Pressed)
            {
                eraserSelected = false;
                selectedTileType = prevSelctedTileType;
                eraserButton.Image = eraserButtonSprite;
            }

            //Switching Grids
            if (switchGridButton.IsClicked(ms))
            {
                switchGrids();
            }

            if (currentGridState == GridStates.WallGrid)
            {
                if (selectedGhostChamber)
                {
                    Point index = WallGrid.PosToIndex(ms.Position.ToVector2());
                    int concord = 0;

                    if (index != new Point(-1) && index.X < wallGridSize.X - 6 && index.Y < wallGridSize.Y - 3)
                    {
                        ghostChamberMS.Position = new Vector2(WallGrid.Tiles[index.X, index.Y].Position.X - pixelVisual.EmptySprite.Width / 2, WallGrid.Tiles[index.X, index.Y].Position.Y - pixelVisual.EmptySprite.Height / 2);
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
                            WallGrid.PlaceGhostChamber(new Point(index.X, index.Y));
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
                            deselectWallButtons();

                            selectedTileType = SelectedType.Wall;
                            wallButton.Image = selectedWallSprite;

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
                            deselectWallButtons();

                            selectedTileType = SelectedType.OuterWall;
                            outerWallButton.Image = selectedOuterWallSprite;

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
                        deselectWallButtons();

                        selectedGhostChamber = true;
                        ghostChamberButton.Tint = Color.Gray;
                    }

                    if (selectedPacman)
                    {
                        Point index = WallGrid.PosToIndex(ms.Position.ToVector2());

                        if (index != new Point(-1))
                        {
                            if (index.X == wallGridSize.X - 1)
                            {
                                index.X--;
                            }
                            if (index.Y == wallGridSize.Y - 1)
                            {
                                index.Y--;
                            }

                            //make a small pacman image, and have it follow "lock" the mouse
                            pacmanTileIcon.Position = new Vector2(WallGrid.Tiles[index.X, index.Y].Position.X - pixelVisual.EmptySprite.Width / 2, WallGrid.Tiles[index.X, index.Y].Position.Y - pixelVisual.EmptySprite.Height / 2);

                            if (ms.LeftButton == ButtonState.Pressed && WallGrid.Tiles[index.X, index.Y].TileStates == States.Empty && WallGrid.Tiles[index.X, index.Y + 1].TileStates == States.Empty && WallGrid.Tiles[index.X + 1, index.Y].TileStates == States.Empty)
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
                    else if (selectedTileType == SelectedType.AltEraser)
                    {
                        WallGrid.removeWall(new Vector2(ms.Position.X, ms.Position.Y));
                    }

                    if (generatePortalButton.IsClicked(ms))
                    {
                        FinishMap();
                    }
                }
            }
            else
            {
                if (pelletButton.IsClicked(ms))
                {
                    if (selectedTileType != SelectedType.Pellet)
                    {
                        deselectPelletButtons();

                        selectedTileType = SelectedType.Pellet;
                        pelletButton.Image = selectedPelletSprite;
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
                        deselectPelletButtons();

                        selectedTileType = SelectedType.PowerPellet;
                        powerPelletButton.Image = selectedPowerPelletSprite;
                    }
                    else
                    {
                        selectedTileType = SelectedType.Default;
                        powerPelletButton.Image = powerPelletButtonSprite;
                    }
                }

                else if (fruitButton.IsClicked(ms) && !isFruitPlaced)
                {
                    if (selectedTileType != SelectedType.Fruit)
                    {
                        deselectPelletButtons();
                        fruitIndex = Random.Shared.Next(0, 5);
                        fruitIcon.Image = fruitImages[fruitIndex];

                        selectedTileType = SelectedType.Fruit;
                        fruitButton.Image = selectedFruitButtonSprite;
                        selectedFruit = true;
                    }
                    else
                    {
                        selectedTileType = SelectedType.Default;
                        fruitButton.Image = fruitButtonSprite;
                        selectedFruit = false;
                    }
                }

                if (selectedPacman)
                {
                    Point index = PelletGrid.PosToIndex(ms.Position.ToVector2());

                    if (index != new Point(-1))
                    {
                        if (index.X == wallGridSize.X - 2)
                        {
                            index.X--;
                        }
                        if (index.Y == wallGridSize.Y - 2)
                        {
                            index.Y--;
                        }

                        pacmanTileIcon.Position = new Vector2(PelletGrid.Tiles[index.X, index.Y].Position.X - pacmanTileIcon.Image.Width / 4, PelletGrid.Tiles[index.X, index.Y].Position.Y - pacmanTileIcon.Image.Height / 2);
                        // pacmanTileIcon.Position = new Vector2(PelletGrid.Tiles[index.X, index.Y].Position.X - pixelVisual.EmptySprite.Width / 2, PelletGrid.Tiles[index.X, index.Y].Position.Y - pixelVisual.EmptySprite.Height / 2);

                        if (ms.LeftButton == ButtonState.Pressed && PelletGrid.Tiles[index.X, index.Y].TileStates == States.Empty)
                        {
                            if (PelletGrid.Tiles[index.X + 1, index.Y].TileStates == States.Empty)
                            {
                                PelletGrid.AddPacman(index);
                            }
                        }
                    }
                }

                if (selectedFruit)
                {
                    Point index = PelletGrid.PosToIndex(ms.Position.ToVector2());
                    if (index != new Point(-1))
                    {
                        if (index.X == wallGridSize.X - 2)
                        {
                            index.X--;
                        }
                        if (index.Y == wallGridSize.Y - 2)
                        {
                            index.Y--;
                        }

                        fruitIcon.Position = new Vector2(PelletGrid.Tiles[index.X, index.Y].Position.X - fruitIcon.Image.Width / 4, PelletGrid.Tiles[index.X, index.Y].Position.Y - fruitIcon.Image.Height / 2);

                        if (ms.LeftButton == ButtonState.Pressed)
                        {
                            if (PelletGrid.Tiles[index.X, index.Y].TileStates == States.Empty && PelletGrid.Tiles[index.X + 1, index.Y].TileStates == States.Empty)
                            {
                                PelletGrid.addFruit(index);
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
                        deselectPelletButtons();
                        deselectWallButtons();

                        selectedTileType = SelectedType.Eraser;
                        eraserButton.Image = selectedEraserSprite;

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

            if (isFruitPlaced)
            {
                fruitTarget += gameTime.ElapsedGameTime;

                if (fruitTarget >= TimeSpan.FromMilliseconds(200))
                {
                    fruitIcon.Image = fruitImages[Random.Shared.Next(0, 5)];
                    fruitTarget = TimeSpan.Zero;
                }
            }

            prevms = ms;

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
