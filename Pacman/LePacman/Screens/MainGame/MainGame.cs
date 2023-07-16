using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LePacman.Screens.MapEditor.MapEditor;

namespace LePacman.Screens.MainGame
{
    public class MainGame : Screen
    {
        public static Texture2D spriteSheet;
        PelletTileVisual test2;

        TimeSpan TimeSpantestDuration;
        TimeSpan limit = TimeSpan.FromSeconds(1);
        int timeThing = 0;

        private static WallTileVisual[,] wallGrid = new WallTileVisual[29, 32];
        private static PelletTileVisual[,] pelletGrid = new PelletTileVisual[28, 31];

        public static Pacman lePacman;
        private static GhostChamber ghostChamber;

        public MainGame(Point Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            size = Size;
        }

        public override void LoadContent(ContentManager Content)
        {
            spriteSheet = Content.Load<Texture2D>("PacmanMainGameSpriteSheet");
        }

        public static void LoadMap(float size) 
        {
            int x = 0;
            int y = 0;
            float tileSize = size * WallTileVisual.defaultSize;
            Vector2 offset = new Vector2(110, 20);

            Vector2 pacmanPos = new Vector2(-1);
            Vector2 ghostChamberPos = new Vector2(-1);

            SavedMap map = SaveMap.currentMap;

            for (int i = 0; i < map.WallTiles.Length; i++)
            {
                if (x == wallGrid.GetLength(0))
                {
                    x = 0;
                    y++;
                }

                wallGrid[x, y] = new WallTileVisual(new Vector2(offset.X + x*tileSize, offset.Y + y* tileSize), Color.White, map.WallTiles[i].WS, map.WallTiles[i].C, new Vector2(size));
                
                if (map.WallTiles[i].TS == States.Pacman)
                {
                    pacmanPos = wallGrid[x, y].Position - new Vector2(tileSize / 2);
                }
                else if (map.WallTiles[i].TS == States.GhostChamber && ghostChamberPos == new Vector2(-1))
                {
                    ghostChamberPos = (wallGrid[x, y].Position + new Vector2(size * 70, size * 40) / 2) - new Vector2(tileSize / 2);
                }

                x++;
            }

            x = 0; y = 0; offset += new Vector2(tileSize/2);

            for (int i = 0; i < map.PixelTiles.Length; i++)
            {
                if (x == pelletGrid.GetLength(0))
                {
                    x = 0;
                    y++;
                }

                pelletGrid[x, y] = new PelletTileVisual(new Vector2(offset.X + x*tileSize, offset.Y + y*tileSize), Color.White, map.PixelTiles[i].TS, map.PixelTiles[i].C, new Vector2(size));

                if (map.PixelTiles[i].TS == States.Pacman && pacmanPos == new Vector2(-1))
                {
                    pacmanPos = pelletGrid[x, y].Position + new Vector2(tileSize/2, 0);
                }

                x++;
            }

            lePacman = new Pacman(pacmanPos, Color.White, new Vector2(size * 1.4f));
            ghostChamber = new GhostChamber(ghostChamberPos, Color.White, new Vector2(size));
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

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

            lePacman.Draw(spriteBatch);

            base.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
        }
    }
}
