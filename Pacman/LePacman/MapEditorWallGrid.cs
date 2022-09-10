﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;


namespace Pacman
{
    public class MapEditorWallGrid
    {
        Vector2 Position;

        public wallVisual[,] Tiles;
        public List<wallVisual> FilledTiles = new List<wallVisual>();

        Point[] offsets = new Point[] { new Point(-1, -1), new Point(0, -1), new Point(1, -1), new Point(1, 0), new Point(1, 1), new Point(0, 1), new Point(-1, 1), new Point(-1, 0) };

        #region Functions

        #region Adding/Removing Walls
        public void addWall(Vector2 MousePosition)
        {
            //check if it's out of bounds

            Point index = PosToIndex(MousePosition);

            if (index == new Point(-1) || Tiles[index.Y, index.X].TileStates == States.Occupied || Tiles[index.Y, index.X].WallState == WallStates.GhostChamber) return;

            recursiveAddWall(Tiles[index.Y, index.X]);
        }
        public void removeWall(Vector2 MousePosition)
        {
            Point index = PosToIndex(MousePosition);

            if (index == new Point(-1) || Tiles[index.Y, index.X].TileStates != States.Wall) return;

            if (Tiles[index.Y, index.X].WallState == WallStates.GhostChamber)
            {
                removeGhostChamber();
                return;
            }

            Tiles[index.Y, index.X].TileStates = States.Empty;
            Tiles[index.Y, index.X].WallState = WallStates.Empty;

            recursiveAddWall(Tiles[index.Y, index.X], true);
        }
        private void recursiveAddWall(wallVisual currentTile, bool remove = false)
        {
            if (!remove)
            {
                bool updated = UpdateWall(currentTile.Position);

                if (!updated) return;

                currentTile.TileStates = States.Wall;
                currentTile.UpdateStates();
            }

            foreach (var neighbor in currentTile.Neighbors)
            {
                if (neighbor.isWall)
                {
                    recursiveAddWall(Tiles[neighbor.Index.Y, neighbor.Index.X]);
                }
            }
        }
        private bool UpdateWall(Vector2 Position)
        {
            return UpdateWall(PosToIndex(Position));
        }
        private bool UpdateWall(Point tileIndex)
        {
            if (tileIndex == new Point(-1))
            {
                return false;
            }
            //y, x

            wallVisual currentTile = Tiles[tileIndex.Y, tileIndex.X];

            WallStates oldState = currentTile.WallState;

            for (int i = 0; i < offsets.Length; i++)
            {
                var newPosition = new Point(tileIndex.X + offsets[i].X, tileIndex.Y + offsets[i].Y);

                currentTile.Neighbors[i].isWall = IsValid(newPosition) && Tiles[newPosition.Y, newPosition.X].TileStates == States.Wall;
                currentTile.Neighbors[i].Index = newPosition;
            }

            //0, 1, 2
            //7, *, 3
            //6, 5, 4

            if (currentTile.Neighbors[1].isWall && currentTile.Neighbors[3].isWall && currentTile.Neighbors[5].isWall && currentTile.Neighbors[7].isWall && currentTile.Neighbors[0].isWall && currentTile.Neighbors[2].isWall && currentTile.Neighbors[4].isWall && currentTile.Neighbors[6].isWall)
            {
                Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.InteriorWall;
                return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
            }

            if ((!currentTile.Neighbors[1].isWall && !currentTile.Neighbors[3].isWall && !currentTile.Neighbors[5].isWall && !currentTile.Neighbors[7].isWall))
            {
                Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.LoneWall;
                return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
            }

            if (currentTile.Neighbors[1].isWall && currentTile.Neighbors[3].isWall && currentTile.Neighbors[5].isWall && currentTile.Neighbors[7].isWall && !currentTile.Neighbors[0].isWall && !currentTile.Neighbors[2].isWall && !currentTile.Neighbors[4].isWall && !currentTile.Neighbors[6].isWall)
            {
                //interior center
                Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.InteriorCorner;
                return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
            }

            if (currentTile.Neighbors[3].isWall && currentTile.Neighbors[7].isWall)
            {
                //horizontal
                if (!currentTile.Neighbors[1].isWall && !currentTile.Neighbors[5].isWall)
                {
                    //middle horiz
                    Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.Horiz;
                    return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                }
                else
                {
                    //Crosses (T)
                    if (!currentTile.Neighbors[0].isWall && !currentTile.Neighbors[2].isWall && !currentTile.Neighbors[4].isWall && !currentTile.Neighbors[6].isWall)
                    {
                        if (currentTile.Neighbors[1].isWall)
                        {
                            //Bottom T
                            Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.BottomCross;
                            return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                        }
                        //Top T
                        Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.TopCross;
                        return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                    }

                    //Top Edge
                    if (currentTile.Neighbors[6].isWall && currentTile.Neighbors[5].isWall && currentTile.Neighbors[4].isWall)
                    {
                        if (currentTile.Neighbors[1].isWall && currentTile.Neighbors[2].isWall)
                        {
                            Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.TopLeftInteriorFilledCorner;
                            return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                        }
                        if (currentTile.Neighbors[0].isWall && currentTile.Neighbors[1].isWall)
                        {
                            Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.TopRightInteriorFilledCorner;
                            return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                        }

                        Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.TopEdge;
                        return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                    }

                    //Bottom Edge
                    if (currentTile.Neighbors[0].isWall && currentTile.Neighbors[1].isWall && currentTile.Neighbors[2].isWall)
                    {
                        if (currentTile.Neighbors[5].isWall && currentTile.Neighbors[6].isWall)
                        {
                            Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.BottomRightInteriorFilledCorner;
                            return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                        }
                        if (currentTile.Neighbors[5].isWall && currentTile.Neighbors[4].isWall)
                        {
                            Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.BottomLeftInteriorFilledCorner;
                            return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                        }

                        Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.BottomEdge;
                        return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                    }
                }
            }
            if (!currentTile.Neighbors[1].isWall && !currentTile.Neighbors[5].isWall)
            {
                if (currentTile.Neighbors[7].isWall)
                {
                    //right horiz
                    Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.HorizRightEnd;
                    return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                }
                //left horiz
                Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.HorizLeftEnd;
                return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
            }

            if (currentTile.Neighbors[1].isWall && currentTile.Neighbors[5].isWall)
            {
                if (!currentTile.Neighbors[7].isWall && !currentTile.Neighbors[3].isWall)
                {
                    //middle verti
                    Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.Verti;
                    return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                }
                else
                {
                    //Crosses (T)
                    if (!currentTile.Neighbors[0].isWall && !currentTile.Neighbors[2].isWall && !currentTile.Neighbors[4].isWall && !currentTile.Neighbors[6].isWall)
                    {
                        if (currentTile.Neighbors[7].isWall)
                        {
                            //Right T
                            Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.RightCross;
                            return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                        }

                        Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.LeftCross;
                        return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                    }

                    //Left Edge
                    if (currentTile.Neighbors[2].isWall && currentTile.Neighbors[3].isWall && currentTile.Neighbors[4].isWall)
                    {
                        Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.LeftEdge;
                        return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                    }
                    //Right Edge
                    if (currentTile.Neighbors[0].isWall && currentTile.Neighbors[7].isWall && currentTile.Neighbors[6].isWall)
                    {
                        Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.RightEdge;
                        return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                    }
                }

            }
            if (!currentTile.Neighbors[7].isWall && !currentTile.Neighbors[3].isWall)
            {
                if (currentTile.Neighbors[1].isWall)
                {
                    //bottom horiz
                    Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.VertiBottomEnd;
                    return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                }
                //top horiz
                Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.VertiTopEnd;
                return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
            }

            //Bottom Left Corner
            if (currentTile.Neighbors[1].isWall && currentTile.Neighbors[3].isWall)
            {
                if (currentTile.Neighbors[2].isWall)
                {
                    //corner without 2nd wall
                    Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.BottomLeftCornerFilled;
                    return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                }

                Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.BottomLeftCorner;
                return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
            }

            //Bottom Right Corner
            if (currentTile.Neighbors[1].isWall && currentTile.Neighbors[7].isWall)
            {
                if (currentTile.Neighbors[0].isWall)
                {
                    //corner without 2nd wall
                    Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.BottomRightCornerFilled;
                    return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                }

                Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.BottomRightCorner;
                return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
            }

            //Top Right Corner
            if (currentTile.Neighbors[7].isWall && currentTile.Neighbors[5].isWall)
            {
                if (currentTile.Neighbors[6].isWall)
                {
                    //corner without 2nd wall
                    Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.TopRightCornerFilled;
                    return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                }

                Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.TopRightCorner;
                return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
            }

            //Top Left Corner
            if (currentTile.Neighbors[3].isWall && currentTile.Neighbors[5].isWall)
            {
                if (currentTile.Neighbors[4].isWall)
                {
                    Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.TopLeftCornerFilled;
                    return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
                }

                Tiles[tileIndex.Y, tileIndex.X].WallState = WallStates.TopLeftCorner;
                return oldState != Tiles[tileIndex.Y, tileIndex.X].WallState;
            }

            //cross

            return false;
            //else if (neighboringWalls[1] && neighboringWalls[3] && neighboringWalls[5] && neighboringWalls[7])
            //{
            //    Tiles[tileIndex.Y, tileIndex.X].wallStates = MapEditorTile.WallStates.InteriorCorner;
            //}
        }
        #endregion

        #region Ghost Chamber

        public bool CanPlaceGC(Point index)
        {
            if (index != new Point(-1))
            {
                int x = index.Y;
                int y = index.X;

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        if (Tiles[x + i, y + j].TileStates != States.Empty)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            return false;
        }
        public void PlaceGhostChamber(Point index)
        {
            if (index != new Point(-1))
            {
                int x = index.X;
                int y = index.Y;

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        Tiles[x + i, y + j].WallState = WallStates.GhostChamber;
                        Tiles[x + i, y + j].TileStates = States.Wall;
                        Tiles[x + i, y + j].UpdateStates();
                    }
                }
            }
        }

        private void removeGhostChamber()
        {
            foreach (var tile in Tiles)
            {
                if (tile.WallState == WallStates.GhostChamber)
                {
                    tile.TileStates = States.Empty;
                    tile.WallState = WallStates.Empty;
                    tile.UpdateStates();
                }
            }

            MapEditor.ghostChamberMS.Position = new Vector2(-300);
            MapEditor.ghostChamberButton.Tint = Color.White;
            MapEditor.selectedGhostChamber = false;
            MapEditor.ghostChamberPlaced = false;
        }

        #endregion

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

            Game1.WindowText = $"GridX: {gridX}, GridY: {gridY}, Offset: {Pos}";

            Point retPoint = new Point(gridX, gridY);
            if (!IsValid(retPoint)) return new Point(-1);
            return retPoint;
        }

        private bool IsValid(Point gridIndex)
        {
            return gridIndex.X >= 0 && gridIndex.X < Tiles.GetLength(1) && gridIndex.Y >= 0 && gridIndex.Y < Tiles.GetLength(0);
        }

        public void LoadGrid(List<wallData> TileList)
        {
            Tiles = TileList.Select(x => new wallVisual(x, Position)).Expand(new Point(Tiles.GetLength(1), Tiles.GetLength(0)));
            bool foundGhostChamber = false;

            foreach (var tile in Tiles)
            {
                if (!foundGhostChamber && tile.WallState == WallStates.GhostChamber)
                {
                    MapEditor.ghostChamberMS.Position = new Vector2(tile.Position.X - 13, tile.Position.Y - 13);
                    foundGhostChamber = true;
                }
                tile.UpdateStates(true);
            }
        }

        #endregion

        public MapEditorWallGrid(Point gridSize, Point tileSize, Vector2 position)
        {
            Position = position + new Vector2(wallVisual.EmptySprite.Width / 2, wallVisual.EmptySprite.Height / 2);

            Tiles = new wallVisual[gridSize.Y, gridSize.X];

            for (int y = 0; y < gridSize.Y; y++)
            {
                for (int x = 0; x < gridSize.X; x++)
                {
                    Tiles[y, x] = new wallVisual(wallVisual.EmptySprite, new Point(y, x), Color.White, Position, Vector2.One, new Vector2(wallVisual.EmptySprite.Width / 2f, wallVisual.EmptySprite.Height / 2f), 0f, SpriteEffects.None);

                    for (int i = 0; i < offsets.Length; i++)
                    {
                        Tiles[y, x].Data.Neighbors[i].Item1 = new Point(y + offsets[i].Y, x + offsets[i].X);
                    }
                }
            }
        }

        public void GoTransparent()
        {
            FilledTiles.Clear();

            foreach (var tile in Tiles)
            {
                if (tile.TileStates == States.Empty || tile.TileStates == States.Occupied)
                {
                    tile.CurrentImage = pixelVisual.NBemptySprite;
                    tile.PrevImage = pixelVisual.NBemptySprite;
                }
                else
                {
                    FilledTiles.Add(tile);
                }

                tile.TileStates = States.NoBackground;
            }
        }

        public List<wallVisual> GetFilledTiles()
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

        public void GoInFocus(List<pixelVisual> pixelTiles)
        {
            foreach (var tile in Tiles)
            {
                if (tile.CurrentImage != wallVisual.NBemptySprite && tile.CurrentImage != wallVisual.EmptySprite || tile.WallState == WallStates.GhostChamber)
                {
                    tile.TileStates = States.Wall;
                }
                else
                {
                    tile.TileStates = States.Empty;
                }

                tile.UpdateStates();

                if (tile.CurrentImage == pixelVisual.NBemptySprite)
                {
                    if (tile.WallState != WallStates.GhostChamber)
                    {
                        tile.WallState = WallStates.Empty;
                    }
                }

            }

            foreach (var tile in pixelTiles)
            {
                Tiles[tile.Cord.X, tile.Cord.Y].TileStates = States.Occupied;
                Tiles[tile.Cord.X, tile.Cord.Y + 1].TileStates = States.Occupied;
                Tiles[tile.Cord.X + 1, tile.Cord.Y + 1].TileStates = States.Occupied;
                Tiles[tile.Cord.X + 1, tile.Cord.Y].TileStates = States.Occupied;

                Tiles[tile.Cord.X, tile.Cord.Y].UpdateStates();
                Tiles[tile.Cord.X, tile.Cord.Y + 1].UpdateStates();
                Tiles[tile.Cord.X + 1, tile.Cord.Y + 1].UpdateStates();
                Tiles[tile.Cord.X + 1, tile.Cord.Y].UpdateStates();
            }
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