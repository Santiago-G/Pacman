﻿using LePacman.Screens.MapEditor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Bson;
using Pacman;
using System;
using System.Collections.Generic;
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

        static TimeSpan ScatterChaseTimer;

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

            LoadNewLevel();
        }

        #endregion

        private static void LoadNewLevel()
        {
            LevelCounter++;

            switch (LevelCounter)
            {
                case 1:

                    ScatterPeriods = new TimeSpan[4] { TimeSpan.FromSeconds(7), TimeSpan.FromSeconds(7), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5) };
                    ChasePeriods = new TimeSpan[3] { TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(20) };

                    pacman.normalSpeed = pacman.maxSpeed * 1.2;
                    pacman.frightSpeed = pacman.maxSpeed * 1.1;
                    Ghost.normalSpeed = pacman.maxSpeed * 1.25;
                    Ghost.frightSpeed = pacman.maxSpeed * 1.5;

                    break;
                case 2:
                    ScatterPeriods = new TimeSpan[4] { TimeSpan.FromSeconds(7), TimeSpan.FromSeconds(7), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1 / 60) };
                    ChasePeriods = new TimeSpan[3] { TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(1033) };

                    pacman.normalSpeed = pacman.maxSpeed * 1.1;
                    pacman.frightSpeed = pacman.maxSpeed * 1.05;
                    Ghost.normalSpeed = pacman.maxSpeed * 1.15;
                    Ghost.frightSpeed = pacman.maxSpeed * 1.45;
                    break;
                case 5:
                    ScatterPeriods = new TimeSpan[4] { TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1 / 60) };
                    ChasePeriods = new TimeSpan[3] { TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(1037) };

                    pacman.normalSpeed = pacman.maxSpeed;
                    pacman.frightSpeed = pacman.maxSpeed;
                    Ghost.normalSpeed = pacman.maxSpeed * 1.05;
                    Ghost.frightSpeed = pacman.maxSpeed * 1.4;
                    break;
                case 21:

                    pacman.normalSpeed = pacman.maxSpeed * 1.1;
                    pacman.frightSpeed = pacman.maxSpeed * 5;
                    Ghost.frightSpeed = pacman.maxSpeed * .3;
                    break;
            }

            currPeriodIndex = 0;
            ScatterChaseTimer = TimeSpan.Zero;

            currentState = GhostStates.Scatter;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            if (currentState != GhostStates.Frightened) 
            {
                ScatterChaseTimer += gameTime.ElapsedGameTime;
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

                pacmanPosition.currentState = States.Empty;
                score += 10;
                targetScore++;
            }
            if (pacmanPosition.currentState == States.PowerPellet)
            {
                currentState = GhostStates.Frightened;

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
