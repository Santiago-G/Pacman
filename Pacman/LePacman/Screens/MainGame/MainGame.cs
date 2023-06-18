using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        WallTileVisual test2;

        private static WallTileVisual[,] wallGrid = new WallTileVisual[29, 32];

        WallStates[] iLoveTesting = new WallStates[] { WallStates.Empty, WallStates.LoneWall, WallStates.Horiz, WallStates.HorizLeftEnd, WallStates.HorizRightEnd, WallStates.Verti, WallStates.VertiTopEnd, WallStates.VertiBottomEnd, WallStates.TopLeftCorner, WallStates.TopRightCorner, WallStates.BottomRightCorner, WallStates.BottomLeftCorner, WallStates.TopEdge, WallStates.RightEdge, WallStates.BottomEdge, WallStates.LeftEdge, WallStates.Interior, WallStates.OuterVerti, WallStates.OuterHoriz, WallStates.TopLeftCornerOW, WallStates.TopRightCornerOW };

        public MainGame(Point Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            size = Size;
        }

        public override void LoadContent(ContentManager Content)
        {
            spriteSheet = Content.Load<Texture2D>("PacmanMainGameSpriteSheet");
            test2 = new WallTileVisual(new Vector2(100), Color.White, iLoveTesting[10], new Vector2(4));


            //objects.Add(test2);
        }

        public static void LoadMap() 
        {
            int x = 0;
            int y = 0;
            Vector2 offset = new Vector2(100, 100);

            SavedMap map = SaveMap.currentMap;

            for (int i = 0; i < map.WallTiles.Length; i++)
            {
                if (x == wallGrid.GetLength(0))
                {
                    x = 0;
                    y++;
                }

                wallGrid[x, y] = new WallTileVisual(new Vector2(offset.X + x*9, offset.Y + y*9), Color.White, map.WallTiles[i].WS, new Vector2(1));
                x++;
            }
        }

        public override void Update(GameTime gameTime)
        {
            

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

            base.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
        }
    }
}
