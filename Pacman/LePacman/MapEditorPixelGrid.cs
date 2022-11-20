using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Pacman
{
    public class MapEditorPixelGrid
    {
        Vector2 Position;

        public pixelVisual[,] Tiles;
        public List<pixelVisual> FilledTiles = new List<pixelVisual>();

        private List<Point> pacmanTileIndex = new List<Point>();
        Point pacmanOrigin;

        Point[] offsets = new Point[] { new Point(-1, -1), new Point(0, -1), new Point(1, -1), new Point(1, 0), new Point(1, 1), new Point(0, 1), new Point(-1, 1), new Point(-1, 0) };

        public MapEditorPixelGrid(Point gridSize, Point tileSize, Vector2 position)
        {
            Position = position + new Vector2(pixelVisual.EmptySprite.Width / 2, pixelVisual.EmptySprite.Height / 2);

            Tiles = new pixelVisual[gridSize.X, gridSize.Y];

            for (int x = 0; x < gridSize.X; x++)
            {
                for (int y = 0; y < gridSize.Y; y++)
                {
                    //new Vector2(MapEditorVisualTile.NormalSprite.Width /2, MapEditorVisualTile.NormalSprite.Height/2)
                    Tiles[x, y] = new pixelVisual(pixelVisual.EmptySprite, new Point(x, y), Color.White, Position, Vector2.One, new Vector2(pixelVisual.EmptySprite.Width / 2f, pixelVisual.EmptySprite.Height / 2f), 0f, SpriteEffects.None);

                    for (int i = 0; i < offsets.Length; i++)
                    {
                        Tiles[x, y].Data.Neighbors[i] = new Point(x + offsets[i].X, y + offsets[i].Y);
                    }
                }
            }
        }

        public Point PosToIndex(Vector2 Pos)
        {
            Pos.X -= Position.X - Tiles[0, 0].Origin.X * Tiles[0, 0].Scale.X;
            Pos.Y -= Position.Y - Tiles[0, 0].Origin.Y * Tiles[0, 0].Scale.Y;

            //check if the float is valid, set it to floats

            float gridXf = (Pos.X / Tiles[0, 0].Hitbox.Width);
            float gridYf = (Pos.Y / Tiles[0, 0].Hitbox.Height);

            if (gridXf < 0)
            {
                gridXf = -1;
            }
            if (gridYf < 0)
            {
                gridYf = -1;
            }

            int gridX = (int)(gridXf);
            int gridY = (int)(gridYf);

            Point retPoint = new Point(gridX, gridY);
            if (!IsValid(retPoint)) return new Point(-1);
            return retPoint;
        }

        private bool IsValid(Point gridIndex)
        {
            return gridIndex.X >= 0 && gridIndex.X < Tiles.GetLength(1) && gridIndex.Y >= 0 && gridIndex.Y < Tiles.GetLength(0);
        }

        #region Pacman
        public void AddPacman(Point index)
        {
            MapEditor.selectedPacman = false;
            MapEditor.pacmanPlaced = true;
            Point[] offsets = new Point[] { new Point(1, 1), new Point(0, 1), new Point(0, -1), new Point(1, -1) };

            Tiles[index.X, index.Y].TileStates = States.Pacman;
            Tiles[index.X, index.Y + 1].TileStates = States.Pacman;

            Tiles[index.X, index.Y].UpdateStates();
            Tiles[index.X, index.Y + 1].UpdateStates();

            pacmanTileIndex.Add(index);
            pacmanTileIndex.Add(new Point(index.X + 1, index.Y));

            pacmanOrigin = index;

            foreach (var offset in offsets)
            {
                if (Tiles[index.X + offset.X, index.Y + offset.Y].TileStates == States.Empty)
                {
                    Tiles[index.X + offset.X, index.Y + offset.Y].TileStates = States.Pacman;
                    Tiles[index.X + offset.X, index.Y + offset.Y].UpdateStates();

                    pacmanTileIndex.Add(new Point(index.X + offset.X, index.Y + offset.Y));
                }
            }
        }

        #endregion

        public void RemovePacman(Point gridIndex)
        {
            foreach (var index in pacmanTileIndex)
            {
                if (Tiles[index.X, index.Y].TileStates == States.Pacman)
                {
                    Tiles[index.X, index.Y].TileStates = States.Empty;
                    Tiles[index.X, index.Y].UpdateStates();
                }
            }

            pacmanOrigin = new Point(-1);

            pacmanTileIndex.Clear();

            MapEditor.pacmanTileIcon.Position = new Vector2(-200);
            MapEditor.pacmanPlacementButton.Tint = Color.White;
            MapEditor.selectedPacman = false;
            MapEditor.pacmanPlaced = false;
        }

        public void LoadGrid(List<pixelData> TileList)
        {
            Tiles = TileList.Select(x => new pixelVisual(x, Position)).Expand(new Point(Tiles.GetLength(1), Tiles.GetLength(0)));

            foreach (var tile in Tiles)
            {
                tile.UpdateStates(true);
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
                    case States.Pacman:
                        if (Tiles[pacmanOrigin.X, pacmanOrigin.Y] == tile)
                        {
                            tile.CurrentImage = pixelVisual.NBemptySprite;
                            FilledTiles.Add(tile);

                            Tiles[pacmanOrigin.X, pacmanOrigin.Y + 1].CurrentImage = pixelVisual.NBemptySprite;
                            FilledTiles.Add(Tiles[pacmanOrigin.X, pacmanOrigin.Y + 1]);
                        }
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
                int y = Math.Min(tile.Cord.Y, 30);
                int x = Math.Min(tile.Cord.X, 27);

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
