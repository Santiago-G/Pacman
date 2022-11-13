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

            if (index == new Point(-1) || Tiles[index.Y, index.X].TileStates == States.Occupied || Tiles[index.Y, index.X].TileStates == States.GhostChamber || Tiles[index.Y, index.X].TileStates == States.Pacman) return;

            recursiveAddWall(Tiles[index.Y, index.X]);
        }
        public void removeWall(Vector2 MousePosition)
        {
            Point index = PosToIndex(MousePosition);

            if (index == new Point(-1)) return;

            if (Tiles[index.Y, index.X].TileStates != States.Wall)
            {
                if (Tiles[index.Y, index.X].TileStates == States.GhostChamber)
                {
                    removeGhostChamber();
                }

                else if (Tiles[index.Y, index.X].TileStates == States.Pacman)
                {
                    RemovePacman(index);
                }

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
                if (IsValid(neighbor) && Tiles[neighbor.Y, neighbor.X].TileStates == States.Wall)
                {
                    recursiveAddWall(Tiles[neighbor.Y, neighbor.X]);
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

                if (IsValid(newPosition) && Tiles[newPosition.Y, newPosition.X].TileStates == States.Wall)
                {
                    currentTile.WallState |= (WallStates)Math.Pow(2, i + 1);
                }
            }

            if (currentTile.WallState.HasFlag(WallStates.TopIntersectingOW) || currentTile.WallState.HasFlag(WallStates.RightIntersectingOW) || currentTile.WallState.HasFlag(WallStates.BottomIntersectingOW) || currentTile.WallState.HasFlag(WallStates.LeftIntersectingOW))
            {
                for (int i = 4; i < offsets.Length; i++)
                {
                    var newPosition = new Point(tileIndex.X + offsets[i].X, tileIndex.Y + offsets[i].Y);

                    currentTile.Neighbors[i] = newPosition;

                    if (IsValid(newPosition) && Tiles[newPosition.Y, newPosition.X].TileStates == States.Wall)
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
            Tiles[index.Y, index.X].TileStates = States.Pacman;
            pacmanTileIndex.Add(index);
            pacmanOrigin = index;

            Tiles[index.Y, index.X + 1].TileStates = States.Pacman;
            pacmanTileIndex.Add(new Point(index.X + 1, index.Y));

            Tiles[index.Y + 1, index.X + 1].TileStates = States.Pacman;
            pacmanTileIndex.Add(new Point(index.X + 1, index.Y + 1));

            Tiles[index.Y + 1, index.X].TileStates = States.Pacman;
            pacmanTileIndex.Add(new Point(index.X, index.Y + 1));
        }

        private void RemovePacman(Point index)
        {
            foreach (var Index in pacmanTileIndex)
            {
                Tiles[Index.Y, Index.X].TileStates = States.Empty;
                Tiles[Index.Y, Index.X].UpdateStates();
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
            return gridIndex.X >= 0 && gridIndex.X < Tiles.GetLength(1) && gridIndex.Y >= 0 && gridIndex.Y < Tiles.GetLength(0);
        }

        public void LoadGrid(List<wallData> TileList)
        {
            Tiles = TileList.Select(x => new wallVisual(x, Position)).Expand(new Point(Tiles.GetLength(1), Tiles.GetLength(0)));
            bool foundGhostChamber = false;

            foreach (var tile in Tiles)
            {
                if (!foundGhostChamber && tile.TileStates == States.GhostChamber)
                {
                    MapEditor.ghostChamberMS.Position = new Vector2(tile.Position.X - 13, tile.Position.Y - 13);
                    foundGhostChamber = true;
                }
                tile.UpdateStates(true);
            }
        }

        public bool OuterWallsValidity()
        {
            graph.Clear();

            //load graph

            HashSet<Vertex> outsideWalls = new HashSet<Vertex>();

            HashSet<Vertex> foundWalls = new HashSet<Vertex>();

            foreach (var tile in Tiles)
            {
                Vertex newVert = new Vertex(tile.Cord);
                graph.AddVertex(newVert);
                if (tile.WallState.HasFlag(WallStates.OuterWall))
                {
                    outsideWalls.Add(newVert);
                }
            }


            int j;
            for (int y = 0; y < Tiles.GetLength(0); y++)
            {
                for (int x = 0; x < Tiles.GetLength(1); x++)
                {
                    int i = y * Tiles.GetLength(1) + x;

                    if (x != 0)
                    {
                        //no left
                        graph.AddEdge(graph.vertices[i], graph.vertices[i - 1], getWeight(Tiles[y, x], Tiles[y, x - 1]));
                    }
                    if (x != Tiles.GetLength(1) - 1)
                    {
                        //no right
                        graph.AddEdge(graph.vertices[i], graph.vertices[i + 1], getWeight(Tiles[y, x], Tiles[y, x + 1]));
                    }
                    if (y != 0)
                    {
                        //no up
                        graph.AddEdge(graph.vertices[i], graph.vertices[i - Tiles.GetLength(1)], getWeight(Tiles[y, x], Tiles[y - 1, x]));
                    }
                    if (y != Tiles.GetLength(0) - 1)
                    {
                        //no right
                        graph.AddEdge(graph.vertices[i], graph.vertices[i + Tiles.GetLength(1)], getWeight(Tiles[y, x], Tiles[y + 1, x]));
                    }
                }
            }
            //the graph should have 3590 edges
            //Vertex endpoint = Pathfinders.Dijkstra(graph, Pathfinders.Dijkstra(graph, graph.vertices[0], outsideWalls, 1), outsideWalls);
            //Vertex currVertex = endpoint;

            foundWalls = Pathfinders.Dijkstra(graph, (Pathfinders.Dijkstra(graph, graph.vertices[0], outsideWalls, 1)).First(), outsideWalls);

            //while (currVertex.Founder != null)
            //{
            //    Tiles[currVertex.Value.X, currVertex.Value.Y].Tint = Color.Red;
            //    currVertex = currVertex.Founder;
            //}

            foreach (var item in foundWalls)
            {
                if (Tiles[item.Value.X, item.Value.Y].TileStates != States.Wall) 
                {
                    ;
                }

                Tiles[item.Value.X, item.Value.Y].Tint = Color.Red;
            }

            return false;
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

            Tiles = new wallVisual[gridSize.Y, gridSize.X];

            for (int y = 0; y < gridSize.Y; y++)
            {
                for (int x = 0; x < gridSize.X; x++)
                {
                    Tiles[y, x] = new wallVisual(wallVisual.EmptySprite, new Point(y, x), Color.White, Position, Vector2.One, new Vector2(wallVisual.EmptySprite.Width / 2f, wallVisual.EmptySprite.Height / 2f), 0f, SpriteEffects.None);

                    for (int i = 0; i < offsets.Length; i++)
                    {
                        Tiles[y, x].Data.Neighbors[i] = new Point(y + offsets[i].Y, x + offsets[i].X);
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
