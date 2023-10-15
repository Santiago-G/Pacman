using LePacman.Screens.MapEditor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Bson;
using Pacman;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.XPath;
using static LePacman.Screens.MapEditor.MapEditor;

namespace LePacman.Screens.MainGame
{
    public class MainGame : Screen
    {
        public static Texture2D spriteSheet;
        private static SpriteFont HeaderFonts;

        public static List<Point> PortalCoord = new List<Point>();

        public static WallTileVisual[,] wallGrid = new WallTileVisual[29, 32];
        public static PelletTileVisual[,] pelletGrid = new PelletTileVisual[28, 31];

        private static int elroy1 = 2112;
        private static int elroy2 = 1999;

        #region Entities

        public static Ghost[] ghosts;

        public static Pacman pacman;
        private static GhostChamber ghostChamber;

        #endregion

        public static GhostStates currentState;

        static int currPeriodIndex;
        static TimeSpan[] ScatterPeriods;
        static TimeSpan[] ChasePeriods;

        private TimeSpan currentPeriod
        {
            get
            {
                if (currentState == GhostStates.Chase)
                {
                    return ChasePeriods[currPeriodIndex];
                }

                return ScatterPeriods[currPeriodIndex];
            }
        }
        static TimeSpan currFrightPeriod;

        static TimeSpan ScatterChaseTimer;
        static TimeSpan FrightTimer;
        

        private int score;
        private static int pelletTarget = 0;
        private int targetScore;

        private static int LevelCounter = 0;

        static Vector2 offset = new Vector2(110, 75);
        static float tileSize;

        #region Debug Sprites
        private static Sprite debugBlinky;
        private static Sprite debugInky;
        private static Sprite debugPinky;
        private static Sprite debugClyde;
        #endregion

        private PelletTileVisual pacmanPosition => pelletGrid[pacman.GridPosition.X, pacman.GridPosition.Y];

        public MainGame(Point Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            size = Size;
            PelletGrid.Instance.offset = offset;
        }

        public override void LoadContent(ContentManager Content)
        {
            spriteSheet = Content.Load<Texture2D>("PacmanMainGameSpriteSheet");
            HeaderFonts = Content.Load<SpriteFont>("mainGameHeader");

            debugInky = new Sprite(Content.Load<Texture2D>("debugSprite"), new Vector2(-100), Color.Blue);
            debugPinky = new Sprite(Content.Load<Texture2D>("debugSprite"), new Vector2(-100), Color.Pink);
            debugBlinky = new Sprite(Content.Load<Texture2D>("debugSprite"), new Vector2(-100), Color.Red);
            debugClyde = new Sprite(Content.Load<Texture2D>("debugSprite"), new Vector2(-100), Color.Brown);
        }

        #region Loading the Map
        private static void portalLogic(SavedMap map)
        {
            foreach (var portalPair in map.Portals)
            {
                Point currPortalCoord = portalPair.firstPortal.firstTile.Cord;
                PortalCoord.Add(new Point(Math.Clamp((currPortalCoord.X - 1), 0, 28), Math.Clamp((currPortalCoord.Y - 1), 0, 32)));

                currPortalCoord = portalPair.firstPortal.secondTile.Cord;
                PortalCoord.Add(new Point(Math.Clamp((currPortalCoord.X - 1), 0, 28), Math.Clamp((currPortalCoord.Y - 1), 0, 32)));

                currPortalCoord = portalPair.secondPortal.firstTile.Cord;
                PortalCoord.Add(new Point(Math.Clamp((currPortalCoord.X - 1), 0, 28), Math.Clamp((currPortalCoord.Y - 1), 0, 32)));

                currPortalCoord = portalPair.secondPortal.secondTile.Cord;
                PortalCoord.Add(new Point(Math.Clamp((currPortalCoord.X - 1), 0, 28), Math.Clamp((currPortalCoord.Y - 1), 0, 32)));
            }
        }

        public static void LoadMap(float size)
        {
            int x = 0;
            int y = 0;
            tileSize = size * WallTileVisual.defaultSize;
            PelletGrid.Instance.tileSize = tileSize;
            PelletGrid.Instance.gridTiles = pelletGrid;

            bool isPacmanFound = false;

            Vector2 pacmanPos = new Vector2(-1);
            Point pacmanCoord = new Point(-1);

            Vector2 gcPos = new Vector2(-1);
            Point blinkyCoord = new Point(-1);

            SavedMap map = SaveMap.currentMap;

            for (int i = 0; i < map.WallTiles.Length; i++)
            {
                if (x == wallGrid.GetLength(0))
                {
                    x = 0;
                    y++;
                }

                wallGrid[x, y] = new WallTileVisual(new Vector2(offset.X + x * tileSize, offset.Y + y * tileSize), Color.White, map.WallTiles[i].WS, map.WallTiles[i].C, new Vector2(size));

                if (!isPacmanFound && map.WallTiles[i].TS == States.Pacman)
                {
                    isPacmanFound = true;
                    pacmanPos = wallGrid[x, y].Position - new Vector2(tileSize / 2);
                    pacmanCoord = wallGrid[x, y].coord - new Point(1);

                    Point pacmanIndex = new Point(i % (pelletGrid.GetLength(0) + 1), i / (pelletGrid.GetLength(0) + 1));
                    ;

                    for (int pacy = -1; pacy < 2; pacy++)
                    {
                        for (int pacx = -1; pacx < 2; pacx++)
                        {
                            map.PixelTiles[((pacmanIndex.Y + pacy) * pelletGrid.GetLength(0)) + pacmanIndex.X + pacx].TS = States.Pacman;
                        }
                    }
                }
                else if (map.WallTiles[i].TS == States.GhostChamber && gcPos == new Vector2(-1))
                {
                    gcPos = (wallGrid[x, y].Position + new Vector2(size * 70, size * 40) / 2) - new Vector2(tileSize / 2);
                    blinkyCoord = new Point(wallGrid[x, y].coord.X + 2, wallGrid[x, y].coord.Y - 2);
                }

                x++;
            }

            x = 0; y = 0; offset += new Vector2(tileSize / 2);

            for (int i = 0; i < map.PixelTiles.Length; i++)
            {
                if (x == pelletGrid.GetLength(0))
                {
                    x = 0;
                    y++;
                }

                pelletGrid[x, y] = new PelletTileVisual(new Vector2(offset.X + x * tileSize, offset.Y + y * tileSize), Color.White, map.PixelTiles[i].TS, map.PixelTiles[i].C, new Vector2(size));

                switch (map.PixelTiles[i].TS)
                {
                    case States.Pellet:
                        pelletTarget++;
                        break;
                    case States.PowerPellet:
                        pelletTarget++;
                        break;
                    case States.Pacman:
                        if (pacmanPos == new Vector2(-1))
                        {
                            pacmanPos = pelletGrid[x, y].Position + new Vector2(tileSize / 2, 0);
                            pacmanCoord = pelletGrid[x, y].coord + new Point(1, 0);
                        }
                        break;
                }

                x++;
            }

            portalLogic(map);

            pacman = new Pacman(pacmanPos, Color.White, new Vector2(size * 1.4f), pacmanCoord);
            ghostChamber = new GhostChamber(gcPos, Color.White, new Vector2(size));

            ghosts = new Ghost[4]
            {
                PelletGrid.Instance.Blinky = new Blinky(new Vector2(gcPos.X, gcPos.Y - tileSize*3), Color.White, new Vector2(size * 1.4f), blinkyCoord),
                PelletGrid.Instance.Inky = new Inky(new Vector2(gcPos.X - tileSize*2, gcPos.Y ), Color.White, new Vector2(size * 1.4f), new Point(blinkyCoord.X - 2, blinkyCoord.Y)),
                PelletGrid.Instance.Pinky = new Pinky(new Vector2(gcPos.X, gcPos.Y ), Color.White, new Vector2(size * 1.4f), new Point(blinkyCoord.X + 2, blinkyCoord.Y)),
                PelletGrid.Instance.Clyde = new Clyde(new Vector2(gcPos.X + tileSize*2, gcPos.Y ), Color.White, new Vector2(size * 1.4f), new Point(blinkyCoord.X + 2, blinkyCoord.Y - 3))
            };
            Ghost.LoadGrid();

            LoadLevels();
            LoadNewLevel();
        }

        #endregion
        record struct LevelInfo(double[] ScatterTimes, int[] ChasePeriods, int FrightPeriod, double PacmanSpeed, double FrightPacManSpeed, double GhostSpeed, double FrightGhostSpeed, int Elroy1Dots, int Elroy2Dots, double Elroy1Speed, double Elroy2Speed, int FrightTime, int NumbOfFlashes);

        static LevelInfo[] Levels = new LevelInfo[21];

        private static void LoadLevels()
        {
            double pacmanMaxSpeed = pacman.maxSpeed.TotalMilliseconds;

            Levels[0] = new(ScatterTimes: new double[] { 7, 7, 5, 5 }, ChasePeriods: new int[] { 20, 20, 20 }, FrightPeriod: 6, //Times
                PacmanSpeed: pacmanMaxSpeed * 1.2, pacmanMaxSpeed * 1.1, pacmanMaxSpeed * 1.25, pacmanMaxSpeed * 1.5,           //Speeds
                pelletTarget - 20, pelletTarget - 10, pacmanMaxSpeed * 1.2, pacmanMaxSpeed * 1.15, 6, 5);                        //Elroy && Frightend Stuff


            Levels[1] = new(ScatterTimes: new double[] { 7, 7, 5, 1/60 }, ChasePeriods: new int[] { 20, 20, 1033 }, FrightPeriod: 6,
                PacmanSpeed: pacmanMaxSpeed * 1.1, pacmanMaxSpeed * 1.05, pacmanMaxSpeed * 1.15, pacmanMaxSpeed * 1.45,
                pelletTarget - 30, pelletTarget - 15, pacmanMaxSpeed * 1.1, pacmanMaxSpeed * 1.05, 5, 5);

            Levels[2] = Levels[1];
            Levels[2].Elroy1Dots -= 10; Levels[2].Elroy2Dots -= 5; Levels[2].FrightTime -= 1;

            Levels[3] = Levels[2];
            Levels[3].FrightTime -= 1;

            Levels[4] = new(ScatterTimes: new double[] { 5, 5, 5, 1 / 60 }, ChasePeriods: new int[] { 20, 20, 1037 }, FrightPeriod: 6,
                PacmanSpeed: pacmanMaxSpeed, pacmanMaxSpeed, pacmanMaxSpeed * 1.05, pacmanMaxSpeed * 1.4,
                pelletTarget - 40, pelletTarget - 20, pacmanMaxSpeed, pacmanMaxSpeed * .95, 2, 5);

            int elroyCount = 0;
            int currentElroy = Levels[4].Elroy1Dots;

            for (int i = 5; i < Levels.Length - 1; i++)
            {
                Levels[i] = Levels[4];

                if (elroyCount % 3 == 0)
                {
                    currentElroy -= 10 * (Math.Clamp(i, 5, 11)/5);
                }

                Levels[i].Elroy1Dots = currentElroy;
                Levels[i].Elroy2Dots = currentElroy / 2;

                elroyCount++;
            }

            Levels[17].Elroy1Dots = pelletTarget - 100; Levels[17].Elroy2Dots = pelletTarget - 50;

            Levels[5].FrightTime = Levels[9].FrightTime = 5;
            Levels[6].FrightTime = Levels[7].FrightTime = Levels[10].FrightTime = 2;
            Levels[8].FrightTime = Levels[11].FrightTime = Levels[12].FrightTime = Levels[14].FrightTime = Levels[15].FrightTime = Levels[17].FrightTime = 1;
            Levels[13].FrightTime = 3;

            Levels[8].NumbOfFlashes = Levels[11].NumbOfFlashes = Levels[12].NumbOfFlashes = Levels[14].NumbOfFlashes = Levels[15].NumbOfFlashes = Levels[17].NumbOfFlashes = 3;
            
           feafeaf //do the last level (21), i bookmarked the website on chrome
           // and remove the fright stuff for levels 17 && 19-21
        }

        private static void LoadNewLevel()
        {
            LevelCounter++;

            var currentLevel = Levels[LevelCounter];

            ScatterPeriods = new TimeSpan[4];
            for (int i = 0; i < ScatterPeriods.Length; ScatterPeriods[i] = TimeSpan.FromSeconds(currentLevel.ScatterTimes[i++]))

                ChasePeriods = new TimeSpan[3];
            for (int i = 0; i < ChasePeriods.Length; ChasePeriods[i] = TimeSpan.FromSeconds(currentLevel.ChasePeriods[i++]))

                currFrightPeriod = TimeSpan.FromSeconds(currentLevel.FrightPeriod);

            currPeriodIndex = 0;
            ScatterChaseTimer = TimeSpan.Zero;
            FrightTimer = TimeSpan.Zero;

            currentState = GhostStates.Scatter;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            if (currentState != GhostStates.Frightened) 
            {
                ScatterChaseTimer += gameTime.ElapsedGameTime;
            }
            else
            {
                FrightTimer += gameTime.ElapsedGameTime;

                if (FrightTimer >= currFrightPeriod)
                {
                    ;
                    //ChangeModes(ghoststat)
                }
            }

            if (ScatterChaseTimer > currentPeriod)
            {
                if (currentState == GhostStates.Chase)
                {
                    currentState = GhostStates.Scatter;
                    currPeriodIndex++;
                }
                else
                {
                    currentState = GhostStates.Chase;
                }

                ScatterChaseTimer = TimeSpan.Zero;
            }


            pacman.Update(gameTime);
            //ghosts[0].Update(gameTime);
            foreach (var ghost in ghosts)
            {
                ghost.Update(gameTime);
            }

            if (pacmanPosition.currentState == States.Pellet)
            {
                pacman.freezeFrameCounter++;
                pacmanPosition.currentState = States.Empty;
                score += 10;
                targetScore++;
            }
            if (pacmanPosition.currentState == States.PowerPellet)
            {
                ChangeModes(GhostStates.Frightened);

                pacman.freezeFrameCounter += 3;

                pacmanPosition.currentState = States.Empty;
                score += 50;
                targetScore++;
            }
            else
            {
                //pacman.ļSpeed = pacman.maxSpeed * .8;
            }

            debugInky.Position = ghosts[1].currTargetTile.ToVector2();
            debugPinky.Position = ghosts[2].currTargetTile.ToVector2();
            debugBlinky.Position = ghosts[0].currTargetTile.ToVector2();
            debugClyde.Position = ghosts[3].currTargetTile.ToVector2();
            base.Update(gameTime);
        }

        private void ChangeModes(GhostStates newState )
        {
            switch (newState)
            {
                case GhostStates.Chase:
                    break;
                case GhostStates.Scatter:
                    break;
                case GhostStates.Frightened:
                    EnterFrightened();
                    break;
            }
        }

        private void EnterFrightened()
        {
            if (currentState == GhostStates.Frightened)
            {
                //just reset the timer.

                return;
            }

            currentState = GhostStates.Frightened;

            foreach (var ghost in ghosts)
            {
                ghost.ļSpeed = Ghost.frightSpeed;
                ghost.ReverseDirection();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (var tile in wallGrid)
            {
                tile.Draw(spriteBatch);
            }

            foreach (var tile in pelletGrid)
            {
                tile.Draw(spriteBatch);
            }

            ghostChamber.Draw(spriteBatch);

            pacman.Draw(spriteBatch);


            foreach (var ghost in ghosts)
            {
                ghost.Draw(spriteBatch);
            }

            debugInky.Draw(spriteBatch);
            debugPinky.Draw(spriteBatch);
            debugBlinky.Draw(spriteBatch);
            debugClyde.Draw(spriteBatch);

            spriteBatch.DrawString(HeaderFonts, "High Score", new Vector2(size.X / 2 - HeaderFonts.MeasureString("HighScore").X / 2, 0), Color.White);
            spriteBatch.DrawString(HeaderFonts, score.ToString(), new Vector2(size.X / 2, 27), Color.White);

            base.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
        }
    }
}
