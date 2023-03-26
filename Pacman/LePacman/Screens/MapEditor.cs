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

namespace Pacman
{
    public class MapEditor : Screen
    {
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

        Texture2D switchButtonSprite;
        Button switchGridButton;

        //Left one
        WallVisual ghostChamberEntrance;
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

        Texture2D portalButtonSprite;
        Button generatePortalButton;

        Texture2D errorBackground;
        SpriteFont errorHeaderFont;
        SpriteFont errorBodyFont;





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



            errorBackground = Content.Load<Texture2D>("outerWallErrorMSGBG");
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

            pelletButton.Image = pelletButtonSprite;
            eraserButton.Image = eraserButtonSprite;
            powerPelletButton.Image = powerPelletButtonSprite;
            wallButton.Image = wallButtonSprite;
        }


        private void ValidityChecks()
        {
            //Check if outer walls are valid*
            var result = WallGrid.FindInvalidOuterWalls();

            foreach (var error in result)
            {
                PopUpManager.Instance.EnqueuePopUp(new ErrorPopUp(errorBackground, new Point(500), error.InvalidTiles.First().Position, errorHeaderFont, errorBodyFont, "Error!", error.ErrorMsg, error.InvalidTiles));
                deselectWallButtons();
                return;
            }

            foreach (var portals in WallGrid.Portals)
            {
                portals.firstPortal.firstTile.Tint = Color.Red;
                portals.firstPortal.secondTile.Tint = Color.Red;
                portals.secondPortal.firstTile.Tint = Color.Red;
                portals.secondPortal.secondTile.Tint = Color.Red;
            }
            //WallGrid.Tiles[WallGrid.ghostChamberTiles[3, 0].Cord.X, WallGrid.ghostChamberTiles[3, 0].Cord.Y - 1].Tint = Color.Red;
            switchGrids();
            switchGrids();

            PelletGrid.longJacket(WallGrid);

            //start the PF at 3,0
            //Pathfind for the ghost, and check if they can reach every pellet.
            //Also keep track if they encounter Pacman
            //If everything above is true, everything is valid.
            //Make sure ghosts can use portals.

            //Pellet tiles are occupied tiles.
            
            
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
             * Fix pacman, its broken
             * 
             * Keep track of the number of portals (and their position you should be doing this anyways)
             * and do another pathfinder starting in the middle. Make it targets the outer edge the map. If it reaches more outer tiles than
             * there are portals, give an error
             * 
             * I'm done. Just use try catch for all the other edge cases
             * Find other edge cases, I fixed the UI aspect of things. 
             * 
             * FIX certain chars from not appearing in the font
             * 
             * 
             * 
             * Fix saving and loading
             * Do Delete Keybind
             * 
             * Do Portals. Portals are 2 tiles wide and there can be multiple.
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
                        ValidityChecks();
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
