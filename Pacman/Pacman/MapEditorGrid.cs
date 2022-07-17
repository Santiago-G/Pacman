using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Pacman
{
    public class MapEditorGrid
    {
        /*
        Vector2 Position;

        public MapEditorVisualTile[,] Tiles;
        Point[] offsets = new Point[] { new Point(-1, -1), new Point(0, -1), new Point(1, -1), new Point(1, 0), new Point(1, 1), new Point(0, 1), new Point(-1, 1), new Point(-1, 0) };

        #region Functions

        public void addWall(Vector2 MousePosition)
        {
            //check if it's out of bounds

            Point index = PosToIndex(MousePosition);

            if (index == new Point(-1)) return;

            recursiveAddWall(Tiles[index.Y, index.X]);
        }

        public void removeWall(Vector2 MousePosition)
        {
            Point index = PosToIndex(MousePosition);

            if (index == new Point(-1) || Tiles[index.Y, index.X].TileStates != States.Wall) return;

            Tiles[index.Y, index.X].TileStates = States.Empty;
            Tiles[index.Y, index.X].WallStates = WallStates.notAWall;

            recursiveAddWall(Tiles[index.Y, index.X], true);
        }

        private void recursiveAddWall(MapEditorVisualTile currentTile, bool remove = false)
        {
            if (!remove)
            {
                bool updated = UpdateWall(currentTile.Position);

                if (!updated) return;
            }

            currentTile.UpdateWalls();

            foreach (var neighbor in currentTile.Neighbors)
            {
                if (neighbor.isWall)
                {
                    recursiveAddWall(Tiles[neighbor.Index.Y, neighbor.Index.X]);
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

            Game1.WindowText = $"GridX: {gridX}, GridY: {gridY}, Offset: {Pos}";

            Point retPoint = new Point(gridX, gridY);
            if (!IsValid(retPoint)) return new Point(-1);
            return retPoint;
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

            MapEditorVisualTile currentTile = Tiles[tileIndex.Y, tileIndex.X];

            WallStates oldState = currentTile.WallStates;

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
                Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.InteriorWall;
                return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
            }

            if ((!currentTile.Neighbors[1].isWall && !currentTile.Neighbors[3].isWall && !currentTile.Neighbors[5].isWall && !currentTile.Neighbors[7].isWall))
            {
                Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.LoneWall;
                return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
            }

            if (currentTile.Neighbors[1].isWall && currentTile.Neighbors[3].isWall && currentTile.Neighbors[5].isWall && currentTile.Neighbors[7].isWall && !currentTile.Neighbors[0].isWall && !currentTile.Neighbors[2].isWall && !currentTile.Neighbors[4].isWall && !currentTile.Neighbors[6].isWall)
            {
                //interior center
                Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.InteriorCorner;
                return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
            }

            if (currentTile.Neighbors[3].isWall && currentTile.Neighbors[7].isWall)
            {
                //horizontal
                if (!currentTile.Neighbors[1].isWall && !currentTile.Neighbors[5].isWall)
                {
                    //middle horiz
                    Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.Horiz;
                    return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                }
                else
                {
                    //Crosses (T)
                    if (!currentTile.Neighbors[0].isWall && !currentTile.Neighbors[2].isWall && !currentTile.Neighbors[4].isWall && !currentTile.Neighbors[6].isWall)
                    {
                        if (currentTile.Neighbors[1].isWall)
                        {
                            //Bottom T
                            Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.BottomCross;
                            return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                        }
                        //Top T
                        Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.TopCross;
                        return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                    }

                    //Top Edge
                    if (currentTile.Neighbors[6].isWall && currentTile.Neighbors[5].isWall && currentTile.Neighbors[4].isWall)
                    {
                        if (currentTile.Neighbors[1].isWall && currentTile.Neighbors[2].isWall)
                        {
                            Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.TopLeftInteriorFilledCorner;
                            return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                        }
                        if (currentTile.Neighbors[0].isWall && currentTile.Neighbors[1].isWall)
                        {
                            Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.TopRightInteriorFilledCorner;
                            return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                        }

                        Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.TopEdge;
                        return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                    }

                    //Bottom Edge
                    if (currentTile.Neighbors[0].isWall && currentTile.Neighbors[1].isWall && currentTile.Neighbors[2].isWall)
                    {
                        if (currentTile.Neighbors[5].isWall && currentTile.Neighbors[6].isWall)
                        {
                            Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.BottomRightInteriorFilledCorner;
                            return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                        }
                        if (currentTile.Neighbors[5].isWall && currentTile.Neighbors[4].isWall)
                        {
                            Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.BottomLeftInteriorFilledCorner;
                            return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                        }

                        Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.BottomEdge;
                        return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                    }
                }
            }
            if (!currentTile.Neighbors[1].isWall && !currentTile.Neighbors[5].isWall)
            {
                if (currentTile.Neighbors[7].isWall)
                {
                    //right horiz
                    Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.HorizRightEnd;
                    return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                }
                //left horiz
                Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.HorizLeftEnd;
                return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
            }

            if (currentTile.Neighbors[1].isWall && currentTile.Neighbors[5].isWall)
            {
                if (!currentTile.Neighbors[7].isWall && !currentTile.Neighbors[3].isWall)
                {
                    //middle verti
                    Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.Verti;
                    return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                }
                else
                {
                    //Crosses (T)
                    if (!currentTile.Neighbors[0].isWall && !currentTile.Neighbors[2].isWall && !currentTile.Neighbors[4].isWall && !currentTile.Neighbors[6].isWall)
                    {
                        if (currentTile.Neighbors[7].isWall)
                        {
                            //Right T
                            Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.RightCross;
                            return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                        }

                        Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.LeftCross;
                        return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                    }

                    //Left Edge
                    if (currentTile.Neighbors[2].isWall && currentTile.Neighbors[3].isWall && currentTile.Neighbors[4].isWall)
                    {
                        Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.LeftEdge;
                        return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                    }
                    //Right Edge
                    if (currentTile.Neighbors[0].isWall && currentTile.Neighbors[7].isWall && currentTile.Neighbors[6].isWall)
                    {
                        Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.RightEdge;
                        return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                    }
                }

            }
            if (!currentTile.Neighbors[7].isWall && !currentTile.Neighbors[3].isWall)
            {
                if (currentTile.Neighbors[1].isWall)
                {
                    //bottom horiz
                    Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.VertiBottomEnd;
                    return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                }
                //top horiz
                Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.VertiTopEnd;
                return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
            }

            //Bottom Left Corner
            if (currentTile.Neighbors[1].isWall && currentTile.Neighbors[3].isWall)
            {
                if (currentTile.Neighbors[2].isWall)
                {
                    //corner without 2nd wall
                    Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.BottomLeftCornerFilled;
                    return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                }

                Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.BottomLeftCorner;
                return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
            }

            //Bottom Right Corner
            if (currentTile.Neighbors[1].isWall && currentTile.Neighbors[7].isWall)
            {
                if (currentTile.Neighbors[0].isWall)
                {
                    //corner without 2nd wall
                    Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.BottomRightCornerFilled;
                    return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                }

                Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.BottomRightCorner;
                return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
            }

            //Top Right Corner
            if (currentTile.Neighbors[7].isWall && currentTile.Neighbors[5].isWall)
            {
                if (currentTile.Neighbors[6].isWall)
                {
                    //corner without 2nd wall
                    Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.TopRightCornerFilled;
                    return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                }

                Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.TopRightCorner;
                return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
            }

            //Top Left Corner
            if (currentTile.Neighbors[3].isWall && currentTile.Neighbors[5].isWall)
            {
                if (currentTile.Neighbors[4].isWall)
                {
                    Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.TopLeftCornerFilled;
                    return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
                }

                Tiles[tileIndex.Y, tileIndex.X].WallStates = WallStates.TopLeftCorner;
                return oldState != Tiles[tileIndex.Y, tileIndex.X].WallStates;
            }

            //cross

            return false;
            //else if (neighboringWalls[1] && neighboringWalls[3] && neighboringWalls[5] && neighboringWalls[7])
            //{
            //    Tiles[tileIndex.Y, tileIndex.X].wallStates = MapEditorTile.WallStates.InteriorCorner;
            //}
        }

        private bool IsValid(Point gridIndex)
        {
            return gridIndex.X >= 0 && gridIndex.X < Tiles.GetLength(1) && gridIndex.Y >= 0 && gridIndex.Y < Tiles.GetLength(0);
        }
        #endregion

        public MapEditorGrid(Point gridSize, Point tileSize, Vector2 position)
        {
            Position = position + new Vector2(MapEditorVisualTile.NormalSprite.Width / 2, MapEditorVisualTile.NormalSprite.Height / 2);

            Tiles = new MapEditorVisualTile[gridSize.Y, gridSize.X];

            for (int y = 0; y < gridSize.Y; y++)
            {
                for (int x = 0; x < gridSize.X; x++)
                {
                    //new Vector2(MapEditorVisualTile.NormalSprite.Width /2, MapEditorVisualTile.NormalSprite.Height/2)
                    Tiles[y, x] = new MapEditorVisualTile(MapEditorVisualTile.NormalSprite, new Point(y, x), Color.White, Position, Vector2.One, new Vector2(MapEditorVisualTile.NormalSprite.Width / 2f, MapEditorVisualTile.NormalSprite.Height / 2f), 0f, SpriteEffects.None);

                    for (int i = 0; i < offsets.Length; i++)
                    {
                        Tiles[y, x].Neighbors[i].Index = new Point(y + offsets[i].Y, x + offsets[i].X);
                    }
                }
            }
        }
        public void SetUpGrid()
        {
            
        }

        public void LoadGrid(List<MapEditorDataTile> TileList)
        {
            Tiles = TileList.Select(x => new MapEditorVisualTile(x, Position)).Expand(new Point(Tiles.GetLength(1), Tiles.GetLength(0)));

            foreach (var tile in Tiles)
            {
                tile.UpdateStates();
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
        */
    }
}
