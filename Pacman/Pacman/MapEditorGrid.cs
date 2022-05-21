using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman
{
    public class MapEditorGrid
    {
        MapEditorVisualTile[,] Tiles;

        MapEditorGrid(Point gridSize, Point tileSize, Vector2 position)
        {
            for (int y = 0; y < gridSize.Y; y++)
            {
                for (int x = 0; x < gridSize.X; x++)
                {

                    float realPositionX = x * tileSize.X + position.X;
                    float realPositionY = y * tileSize.Y + position.Y;

                    Tiles[y, x] = new MapEditorVisualTile(MapEditorVisualTile.NormalSprite, new Point(y, x), Color.White);

                    for (int i = 0; i < offsets.Length; i++)
                    {
                        tiles[y, x].Neighbors[i].Index = new Point(y + offsets[i].Y, x + offsets[i].X);
                    }
                }
            }
        }
        public void SetUpGrid()
        {
            
        }

        public void LoadGrid()
        {
            
        }

        public void Update()
        { 
            
        }

        public void Draw(SpriteBatch batch)
        {

        }
    }
}
