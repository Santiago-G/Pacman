using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Pacman
{
    public class MapEditorPixelGrid
    {
        Vector2 Position;

        public pixelVisual[,] Tiles;
        public List<pixelVisual> FilledTiles = new List<pixelVisual>();

        Point[] offsets = new Point[] { new Point(-1, -1), new Point(0, -1), new Point(1, -1), new Point(1, 0), new Point(1, 1), new Point(0, 1), new Point(-1, 1), new Point(-1, 0) };

        public MapEditorPixelGrid(Point gridSize, Point tileSize, Vector2 position)
        {
            Position = position + new Vector2(pixelVisual.EmptySprite.Width / 2, pixelVisual.EmptySprite.Height / 2);

            Tiles = new pixelVisual[gridSize.Y, gridSize.X];

            for (int y = 0; y < gridSize.Y; y++)
            {
                for (int x = 0; x < gridSize.X; x++)
                {
                    //new Vector2(MapEditorVisualTile.NormalSprite.Width /2, MapEditorVisualTile.NormalSprite.Height/2)
                    Tiles[y, x] = new pixelVisual(pixelVisual.EmptySprite, new Point(y, x), Color.White, Position, Vector2.One, new Vector2(pixelVisual.EmptySprite.Width / 2f, pixelVisual.EmptySprite.Height / 2f), 0f, SpriteEffects.None);

                    for (int i = 0; i < offsets.Length; i++)
                    {
                        Tiles[y, x].Data.Neighbors[i] = new Point(y + offsets[i].Y, x + offsets[i].X);
                    }
                }
            }
        }

        public void LoadGrid(List<pixelData> TileList)
        {
            Tiles = TileList.Select(x => new pixelVisual(x, Position)).Expand(new Point(Tiles.GetLength(1), Tiles.GetLength(0)));

            foreach (var tile in Tiles)
            {
                tile.UpdateStates(true);
                ;
            }
        }

        public void GoTransparent()
        {
            FilledTiles.Clear();

            foreach (var tile in Tiles)
            {
                switch (tile.TileStates)
                {
                    case States.Empty:
                        tile.CurrentImage = pixelVisual.NBemptySprite;
                        break;
                    case States.Occupied:
                        tile.CurrentImage = pixelVisual.NBemptySprite;
                        break;
                    case States.Pellet:
                        tile.CurrentImage = pixelVisual.NBpelletSprite;
                        FilledTiles.Add(tile);
                        break;
                    case States.PowerPellet:
                        tile.CurrentImage = pixelVisual.NBpowerPelletSprite;
                        FilledTiles.Add(tile);
                        break;
                }
            }
        }

        public void GoInFocus(List<wallVisual> wallTiles)
        {
            foreach (var tile in Tiles)
            {
                if (tile.TileStates == States.Occupied)
                {
                    tile.TileStates = States.Empty;
                }

                tile.UpdateStates();
            }

            foreach (var tile in wallTiles)
            {
                int leftX = Math.Max(tile.Cord.X - 1, 0);
                int topY = Math.Max(tile.Cord.Y - 1, 0);
                int y = Math.Min(tile.Cord.Y, 27);
                int x = Math.Min(tile.Cord.X, 30);

                Tiles[leftX, topY].TileStates = States.Occupied;
                Tiles[leftX, y].TileStates = States.Occupied;
                Tiles[x, y].TileStates = States.Occupied;
                Tiles[x, topY].TileStates = States.Occupied;

                Tiles[leftX, topY].UpdateStates();
                Tiles[leftX, y].UpdateStates();
                Tiles[x, y].UpdateStates();
                Tiles[x, topY].UpdateStates();
            }
        }

        public List<pixelVisual> GetFilledTiles()
        {
            FilledTiles.Clear();

            foreach (var tile in Tiles)
            {
                if (tile.TileStates != States.Empty && tile.TileStates != States.Occupied)
                {
                    FilledTiles.Add(tile);
                }
            }

            return FilledTiles;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var tile in Tiles)
            {
                tile.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (var tile in Tiles)
            {
                tile.Draw(batch);
            }
        }
    }
}
