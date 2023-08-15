using LePacman.Screens.MapEditor;
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

        TimeSpan powerPelletTimer;
        TimeSpan powerPelletLimit = TimeSpan.FromMilliseconds(500);
        int timeThing = 0;

        public static WallTileVisual[,] wallGrid = new WallTileVisual[29, 32];
        public static PelletTileVisual[,] pelletGrid = new PelletTileVisual[28, 31];

        private int score;
        private static int pelletTarget = 0;
        private int targetScore;

        public static Ghost[] ghosts;

        public static Pacman pacman;
        private static GhostChamber ghostChamber;

        static Vector2 offset = new Vector2(110, 75);
        static float tileSize;


        public MainGame(Point Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            size = Size;
        }

        public override void LoadContent(ContentManager Content)
        {
            spriteSheet = Content.Load<Texture2D>("PacmanMainGameSpriteSheet");
            HeaderFonts = Content.Load<SpriteFont>("mainGameHeader");
        }

        public static Vector2 CoordToPostion(Point Coord)
        {
            return new Vector2(offset.X + Coord.X * tileSize, offset.Y + Coord.Y * tileSize);
        }

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

                if (map.WallTiles[i].TS == States.Pacman)
                {
                    pacmanPos = wallGrid[x, y].Position - new Vector2(tileSize / 2);
                    pacmanCoord = wallGrid[x, y].coord - new Point(1);
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
                        pelletTarget ++;
                        break;
                    case States.PowerPellet:
                        pelletTarget ++;
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
                new Blinky(new Vector2(gcPos.X, gcPos.Y - tileSize*3), Color.White, new Vector2(size * 1.4f), blinkyCoord),
                new Inky(new Vector2(gcPos.X - tileSize*2, gcPos.Y ), Color.White, new Vector2(size * 1.4f), new Point(blinkyCoord.X - 2, blinkyCoord.Y + 3)),
                new Pinky(new Vector2(gcPos.X, gcPos.Y ), Color.White, new Vector2(size * 1.4f), new Point(blinkyCoord.X, blinkyCoord.Y + 3)),
                new Clyde(new Vector2(gcPos.X + tileSize*2, gcPos.Y ), Color.White, new Vector2(size * 1.4f), new Point(blinkyCoord.X + 2, blinkyCoord.Y + 3))
            };
            pelletGrid[ghosts[0].PendingLocation.X, ghosts[0].PendingLocation.Y].currentState = States.PowerPellet;
            pelletGrid[ghosts[0].PendingLocation.X, ghosts[0].PendingLocation.Y].Tint = Color.Red;

            pelletGrid[ghosts[1].PendingLocation.X, ghosts[1].PendingLocation.Y].currentState = States.PowerPellet;
            pelletGrid[ghosts[1].PendingLocation.X, ghosts[1].PendingLocation.Y].Tint = Color.Cyan;

            pelletGrid[ghosts[2].PendingLocation.X, ghosts[2].PendingLocation.Y].currentState = States.PowerPellet;
            pelletGrid[ghosts[2].PendingLocation.X, ghosts[2].PendingLocation.Y].Tint = Color.Pink;

            pelletGrid[ghosts[3].PendingLocation.X, ghosts[3].PendingLocation.Y].currentState = States.PowerPellet;
            pelletGrid[ghosts[3].PendingLocation.X, ghosts[3].PendingLocation.Y].Tint = Color.Orange;

        }

        private PelletTileVisual pacmanPosition => pelletGrid[pacman.PendingLocation.X, pacman.PendingLocation.Y];

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            pacman.Update(gameTime);

            if (pacmanPosition.currentState == States.Pellet)
            {
                pacman.ļSpeed = pacman.maxSpeed * 1.81;
                pacmanPosition.currentState = States.Empty;
                score += 10;
                targetScore++;
            }
            if (pacmanPosition.currentState == States.PowerPellet)
            {
                pacman.ļSpeed = pacman.maxSpeed * 1.67;
                pacmanPosition.currentState = States.Empty;
                score += 50;
                targetScore++;
            }
            else
            {
                pacman.ļSpeed = pacman.maxSpeed * .8;
            }


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
                /////     ghost.Draw(spriteBatch);
            }

            spriteBatch.DrawString(HeaderFonts, "High Score", new Vector2(size.X / 2 - HeaderFonts.MeasureString("HighScore").X/2, 0), Color.White);
            spriteBatch.DrawString(HeaderFonts, score.ToString(), new Vector2(size.X / 2, 27), Color.White);

            base.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
        }
    }
}
