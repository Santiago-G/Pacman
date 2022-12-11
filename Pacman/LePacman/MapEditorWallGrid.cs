using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Net.NetworkInformation;
using System.Diagnostics.SymbolStore;
using System.Runtime.ExceptionServices;
using LePacman.Pathfinding;

namespace Pacman
{
    public class MapEditorWallGrid
    {
        Vector2 Position;

        public wallVisual[,] Tiles;
        public List<wallVisual> FilledTiles = new List<wallVisual>();
        private List<Point> pacmanTileIndex = new List<Point>();
        private Graph graph = new Graph();

        Point pacmanOrigin;

        Point[] offsets = new Point[] { new Point(0, -1), new Point(0, 1), new Point(-1, 0), new Point(1, 0), new Point(1, -1), new Point(1, 1), new Point(-1, 1), new Point(-1, -1) };

        #region Functions

        #region Adding/Removing Walls
        public void addWall(Vector2 MousePosition)
        {
            //check if it's out of bounds

            Point index = PosToIndex(MousePosition);

            if (index == new Point(-1) || Tiles[index.X, index.Y].TileStates == States.Occupied || Tiles[index.X, index.Y].TileStates == States.GhostChamber || Tiles[index.X, index.Y].TileStates == States.Pacman) return;

            recursiveAddWall(Tiles[index.X, index.Y]);
        }
        public void removeWall(Vector2 MousePosition)
        {
            Point index = PosToIndex(MousePosition);

            if (index == new Point(-1)) return;

            if (Tiles[index.X, index.Y].TileStates != States.Wall)
            {
                if (Tiles[index.X, index.Y].TileStates == States.GhostChamber)
                {
                    removeGhostChamber();
                }

                else if (Tiles[index.X, index.Y].TileStates == States.Pacman)
                {
                    RemovePacman(index);
                }

                return;
            }

            Tiles[index.X, index.Y].TileStates = States.Empty;
            Tiles[index.X, index.Y].WallState = WallStates.Empty;

            recursiveAddWall(Tiles[index.X, index.Y], true);
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
                if (IsValid(neighbor) && Tiles[neighbor.X, neighbor.Y].TileStates == States.Wall)
                {
                    recursiveAddWall(Tiles[neighbor.X, neighbor.Y]);
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

            wallVisual currentTile = Tiles[tileIndex.X, tileIndex.Y];
            WallStates oldState = currentTile.WallState;

            if (MapEditor.selectedTileType == SelectedType.OuterWall || oldState.HasFlag(WallStates.OuterWall))
            {
                if (!oldState.HasFlag(WallStates.isWall) || oldState.HasFlag(WallStates.OuterWall))
                {
                    currentTile.WallState = WallStates.OuterWall;
                }
                else return false;
            }
            else currentTile.WallState = WallStates.isWall;


            for (int i = 0; i < offsets.Length / 2; i++)
            {
                var newPosition = new Point(tileIndex.X + offsets[i].X, tileIndex.Y + offsets[i].Y);

                currentTile.Neighbors[i] = newPosition;

                if (IsValid(newPosition) && Tiles[newPosition.X, newPosition.Y].TileStates == States.Wall)
                {
                    currentTile.WallState |= (WallStates)Math.Pow(2, i + 1);
                }
            }

            if (currentTile.WallState.HasFlag(WallStates.OuterWall))
            {
                int numOfOuterWalls = 0;
                foreach (var neighbor in currentTile.Neighbors)
                {
                    numOfOuterWalls = 0;
                    if (Tiles[neighbor.X, neighbor.Y].WallState.HasFlag(WallStates.OuterWall))
                    {
                        foreach (var item in Tiles[neighbor.X, neighbor.Y].Neighbors)
                        {
                            if (Tiles[item.X, item.Y].WallState.HasFlag(WallStates.OuterWall))
                            {
                                numOfOuterWalls++;
                            }
                        }

                        if (numOfOuterWalls > 3)
                        {
                            fix this
                            currentTile.WallState = WallStates.Empty;
                            return false;
                        }
                    }
                }
            }

            if (currentTile.WallState.HasFlag(WallStates.TopIntersectingOW) || currentTile.WallState.HasFlag(WallStates.RightIntersectingOW) || currentTile.WallState.HasFlag(WallStates.BottomIntersectingOW) || currentTile.WallState.HasFlag(WallStates.LeftIntersectingOW))
            {
                for (int i = 4; i < offsets.Length; i++)
                {
                    var newPosition = new Point(tileIndex.X + offsets[i].X, tileIndex.Y + offsets[i].Y);

                    currentTile.Neighbors[i] = newPosition;

                    if (IsValid(newPosition) && Tiles[newPosition.X, newPosition.Y].TileStates == States.Wall)
                    {
                        currentTile.WallState |= (WallStates)Math.Pow(2, i + 1);
                    }
                }
            }

            if (currentTile.WallState == WallStates.OuterWall)
            {
                currentTile.WallState = WallStates.OuterHoriz;
            }

            return oldState != currentTile.WallState;
        }

        #endregion

        #region Ghost Chamber

        public bool CanPlaceGC(Point index)
        {
            if (index != new Point(-1))
            {
                int x = index.X;
                int y = index.Y;

                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 4; j++)
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

                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        Tiles[x + i, y + j].TileStates = States.GhostChamber;
                        Tiles[x + i, y + j].UpdateStates();
                    }
                }
            }
        }

        private void removeGhostChamber()
        {
            foreach (var tile in Tiles)
            {
                if (tile.TileStates == States.GhostChamber)
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

        #region Pacman 
        public void AddPacman(Point index)
        {
            MapEditor.selectedPacman = false;
            MapEditor.pacmanPlaced = true;
            Tiles[index.X, index.Y].TileStates = States.Pacman;
            pacmanTileIndex.Add(index);
            pacmanOrigin = index;

            Tiles[index.X, index.Y + 1].TileStates = States.Pacman;
            pacmanTileIndex.Add(new Point(index.X + 1, index.Y));

            Tiles[index.X + 1, index.Y + 1].TileStates = States.Pacman;
            pacmanTileIndex.Add(new Point(index.X + 1, index.Y + 1));

            Tiles[index.X + 1, index.Y].TileStates = States.Pacman;
            pacmanTileIndex.Add(new Point(index.X, index.Y + 1));
        }

        private void RemovePacman(Point index)
        {
            foreach (var Index in pacmanTileIndex)
            {
                Tiles[Index.X, Index.Y].TileStates = States.Empty;
                Tiles[Index.X, Index.Y].UpdateStates();
            }

            pacmanOrigin = new Point(-1);

            pacmanTileIndex.Clear();

            MapEditor.pacmanTileIcon.Position = new Vector2(-200);
            MapEditor.pacmanPlacementButton.Tint = Color.White;
            MapEditor.selectedPacman = false;
            MapEditor.pacmanPlaced = false;
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

            // Game1.WindowText = $"GridX: {gridX}, GridY: {gridY}, Offset: {Pos}";

            Point retPoint = new Point(gridX, gridY);
            if (!IsValid(retPoint)) return new Point(-1);
            return retPoint;
        }

        private bool IsValid(Point gridIndex)
        {
            return gridIndex.X >= 0 && gridIndex.X < Tiles.GetLength(0) && gridIndex.Y >= 0 && gridIndex.Y < Tiles.GetLength(1);
        }

        public void LoadGrid(List<wallData> TileList)
        {
            Tiles = TileList.Select(x => new wallVisual(x, Position)).Expand(new Point(Tiles.GetLength(0), Tiles.GetLength(1)));
            bool foundGhostChamber = false;

            foreach (var tile in Tiles)
            {
                if (!foundGhostChamber && tile.TileStates == States.GhostChamber)
                {
                    MapEditor.ghostChamberMS.Position = new Vector2(tile.Position.X - 13, tile.Position.Y - 13);
                    foundGhostChamber = true;
                    MapEditor.ghostChamberPlaced = true;
                }
                tile.UpdateStates(true);
            }
        }

        public bool OuterWallsValidity()
        {
            graph.Clear();

            HashSet<Vertex> outsideWalls = new HashSet<Vertex>();
            List<Vertex> foundWalls = new List<Vertex>();
            HashSet<Vertex> possiblePortals = new HashSet<Vertex>();

            foreach (var tile in Tiles)
            {
                Vertex newVert = new Vertex(tile.Cord);
                graph.AddVertex(newVert);
                if (tile.WallState.HasFlag(WallStates.OuterWall))
                {
                    newVert.isOuterWall = true;
                    outsideWalls.Add(newVert);
                }
            }

            #region Creating Edges
            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    int i = x * Tiles.GetLength(1) + y;

                    if (x != 0)
                    {
                        //no left
                        graph.AddEdge(graph.vertices[i], graph.vertices[i - Tiles.GetLength(1)], getWeight(Tiles[x, y], Tiles[x - 1, y]));
                    }
                    if (x != Tiles.GetLength(0) - 1)
                    {
                        //no right
                        graph.AddEdge(graph.vertices[i], graph.vertices[i + Tiles.GetLength(1)], getWeight(Tiles[x, y], Tiles[x + 1, y]));
                    }
                    if (y != 0)
                    {
                        //no up
                        graph.AddEdge(graph.vertices[i], graph.vertices[i - 1], getWeight(Tiles[x, y], Tiles[x, y - 1]));
                    }
                    if (y != Tiles.GetLength(1) - 1)
                    {
                        //no right
                        graph.AddEdge(graph.vertices[i], graph.vertices[i + 1], getWeight(Tiles[x, y], Tiles[x, y + 1]));
                    }
                }
            }

            //the graph should have 3590 edges
            #endregion

            foundWalls = Pathfinders.Dijkstra(graph, (Pathfinders.Dijkstra(graph, graph.vertices[0], outsideWalls, out _, out _, 1)).First(), outsideWalls, out int numOfJumps, out Vertex lastVertex);

            
            if (numOfJumps == 1)
            {
                return false;
            }
            else if (numOfJumps == 0)
            {
                bool noGapNearStart = false;
                foreach (var neighbor in lastVertex.Neighbors)
                {
                    //7 is the min amount for a loop
                    if (neighbor.End.DistanceFromStart < lastVertex.DistanceFromStart - 7)
                    {
                        noGapNearStart = true;
                        break;
                    }
                }

                if (!noGapNearStart)
                {
                    return false;
                }
            }
            

            //check if there are gaps of 1 / 3 or more. 
            int gapNum = 0;
            foreach (var item in foundWalls)
            {
                if (!Tiles[item.Value.X, item.Value.Y].WallState.HasFlag(WallStates.OuterWall))
                {
                    gapNum++;
                    possiblePortals.Add(item);
                    Tiles[item.Value.X, item.Value.Y].Tint = Color.Green;
                }
                else 
                {
                    Tiles[item.Value.X, item.Value.Y].Tint = Color.Red;
                    if (gapNum != 0 && gapNum != 2) 
                        return false;
                    gapNum = 0;
                }

                
            }

            if (gapNum != 0 && gapNum != 2) 
                return false;

            foreach (var item in possiblePortals)
            {
                int direction = -1;
                int targetX;            
                
                targetX = item.Value.X == Tiles.GetLength(0) - 1 ? 0 : (direction = 1) * Tiles.GetLength(0) - 1;
                ;

                wallVisual currTile = Tiles[item.Value.X, item.Value.Y];
                while (currTile.Cord.X != targetX)
                {
                    currTile = Tiles[currTile.Cord.X + direction, currTile.Cord.Y];

                    if (currTile.WallState.HasFlag(WallStates.OuterWall))
                    {
                        return false;
                    }
                }
                ;
                if (!(Tiles[currTile.Cord.X, currTile.Cord.Y - 1].WallState.HasFlag(WallStates.OuterWall)) && !(Tiles[currTile.Cord.X, currTile.Cord.Y + 1].WallState.HasFlag(WallStates.OuterWall)))
                {
                    return false;
                }
            }

            return true;
        }

        public int getWeight(wallVisual ab, wallVisual ba)
        {
            if (ab.WallState.HasFlag(WallStates.OuterWall) && ba.WallState.HasFlag(WallStates.OuterWall))
            {
                return -1;
            }

            return short.MaxValue;
        }

        #region Switching Grids
        public void GoTransparent()
        {
            FilledTiles.Clear();

            foreach (var tile in Tiles)
            {
                if (tile.TileStates == States.Empty || tile.TileStates == States.Occupied)
                {
                    tile.CurrentImage = pixelVisual.NBemptySprite;
                }
                else
                {
                    FilledTiles.Add(tile);
                }
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
                if (tile.TileStates == States.Occupied)
                {
                    tile.TileStates = States.Empty;
                }

                tile.UpdateStates();
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
        #endregion

        #endregion

        public MapEditorWallGrid(Point gridSize, Point tileSize, Vector2 position)
        {
            Position = position + new Vector2(wallVisual.EmptySprite.Width / 2, wallVisual.EmptySprite.Height / 2);

            Tiles = new wallVisual[gridSize.X, gridSize.Y];

            for (int x = 0; x < gridSize.X; x++)
            {
                for (int y = 0; y < gridSize.Y; y++)
                {
                    Tiles[x, y] = new wallVisual(wallVisual.EmptySprite, new Point(x, y), Color.White, Position, Vector2.One, new Vector2(wallVisual.EmptySprite.Width / 2f, wallVisual.EmptySprite.Height / 2f), 0f, SpriteEffects.None);

                    for (int i = 0; i < offsets.Length; i++)
                    {
                        Tiles[x, y].Data.Neighbors[i] = new Point(x + offsets[i].X, y + offsets[i].Y);
                    }
                }
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
