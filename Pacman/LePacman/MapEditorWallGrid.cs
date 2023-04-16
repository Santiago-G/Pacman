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
using System.Diagnostics.Metrics;
using System.Runtime.Serialization.Formatters;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Pacman
{
    //record class Animal<T> (string name, int age, T coffee);
    public record struct PortalPair(Portal firstPortal, Portal secondPortal);

    public record struct Portal(WallVisual firstTile, WallVisual secondTile);

    public class MapEditorWallGrid
    {
        Vector2 Position;
        
        public WallVisual[,] Tiles;
        public List<WallVisual> FilledTiles = new List<WallVisual>();
        public List<PortalPair> Portals = new List<PortalPair>();
        //1 Portal Pair = 2 Portals = 4 Tiles
        public WallVisual[,] ghostChamberTiles = new WallVisual[7,4];
        
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
        private void recursiveAddWall(WallVisual currentTile, bool remove = false)
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

            WallVisual currentTile = Tiles[tileIndex.X, tileIndex.Y];
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
                foreach (var neighbor in currentTile.Neighbors)
                {
                    if (IsValid(neighbor) && Tiles[neighbor.X, neighbor.Y].OuterWallNeighborCount(Tiles, false) > 2)
                    {
                        currentTile.WallState = WallStates.Empty;
                        return false;
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
                        ghostChamberTiles[i, j] = Tiles[x + i, y + j];
                        Tiles[x + i, y + j].UpdateStates();
                    }
                }
            }
        }

        private void removeGhostChamber()
        {
            ghostChamberTiles = new WallVisual[7,4];

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
            Tiles = TileList.Select(x => new WallVisual(x, Position)).Expand(new Point(Tiles.GetLength(0), Tiles.GetLength(1)));
            bool foundGhostChamber = false;

            foreach (var tile in Tiles)
            {
                if (!foundGhostChamber && tile.TileStates == States.GhostChamber)
                {
                    MapEditor.ghostChamberMS.Position = new Vector2(tile.Position.X - 13, tile.Position.Y - 13);
                    foundGhostChamber = true;
                    MapEditor.ghostChamberPlaced = true;

                    for (int x = 0; x < 7; x++)
                    {
                        for (int y = 0; y < 4; y++)
                        {
                            ghostChamberTiles[x, y] = Tiles[tile.Cord.X + x, tile.Cord.Y + y];
                        }
                    }
                }
                tile.UpdateStates(true);
            }
        }

        #region OuterWall/Portal functions
        private List<WallVisual> findInvalidTiles(WallVisual startingTile)
        {
            List<WallVisual> invalidTiles = new List<WallVisual>();

            int directionX = 0;
            int directionY = 0;
            WallVisual currentTile = startingTile;
            //Go Down

            if (startingTile.Cord.Y != 0 && Tiles[startingTile.Cord.X, startingTile.Cord.Y - 1].WallState.HasFlag(WallStates.OuterWall))
            {
                directionY = 1;
            }
            else if (startingTile.Cord.Y != Tiles.GetLength(1) - 1 && Tiles[startingTile.Cord.X, startingTile.Cord.Y + 1].WallState.HasFlag(WallStates.OuterWall))
            {
                directionY = -1;
            }
            else if (startingTile.Cord.X != 0 && Tiles[startingTile.Cord.X - 1, startingTile.Cord.Y].WallState.HasFlag(WallStates.OuterWall))
            {
                directionX = 1;
            }
            else if (startingTile.Cord.X != Tiles.GetLength(0) - 1 && Tiles[startingTile.Cord.X + 1, startingTile.Cord.Y].WallState.HasFlag(WallStates.OuterWall))
            {
                directionX = -1;
            }

            if (startingTile.WallState.HasFlag(WallStates.OuterWall))
            {
                currentTile = Tiles[startingTile.Cord.X + directionY, startingTile.Cord.Y + directionX];
                int temp = directionX;
                directionX = directionY;
                directionY = temp;
            }

            if ((directionX != 0 && directionY != 0) || (directionX == 0 && directionY == 0))
            {
                invalidTiles.Add(startingTile);
                return invalidTiles;
            }

            while (true)
            {
                invalidTiles.Add(currentTile);

                currentTile = Tiles[currentTile.Cord.X + directionX, currentTile.Cord.Y + directionY];

                if (((currentTile.Cord.X == Tiles.GetLength(0) - 1 || currentTile.Cord.X == 0) && directionX != 0) || ((currentTile.Cord.Y == Tiles.GetLength(1) - 1 || currentTile.Cord.Y == 0) && directionY != 0))
                {
                    invalidTiles.Clear();
                    invalidTiles.Add(startingTile);
                    return invalidTiles;
                }

                if (currentTile.WallState.HasFlag(WallStates.OuterWall)) break;
            }

            return invalidTiles;
        }

        private bool isOnTheBorder(Point position)
        {
            return position.X == 0 || position.Y == 0 || position.X == Tiles.GetLength(0) - 1 || position.Y == Tiles.GetLength(1) - 1;
        }

        private (string, List<WallVisual>) PortalValityCheck(WallVisual startingTile, bool isHoriz)
        {
            int directionX = 0;
            int directionY = 0;

            string errorMSG = "Portals must extend to the edge of the map";
            List<WallVisual> invalidTiles = new List<WallVisual>();
            WallVisual currTile = startingTile;
            int gapsFound = 0;
            bool inGap = false;

            //There should only be one gap.
            //Bottom

            if (isHoriz) { directionY = 1; }
            else { directionX = 1; }

            //I love one liners
            currTile = Tiles[(startingTile.Cord.X - directionY * 2) * directionY, (startingTile.Cord.Y - directionX * 2) * directionX];

            for (int i = 0; i < 2; i++)
            {
                while (true)
                {
                    if (!currTile.WallState.HasFlag(WallStates.OuterWall))
                    {
                        if (!inGap)
                        {
                            gapsFound++;
                            inGap = true;
                        }

                        if (gapsFound >= 2)
                        {
                            invalidTiles.Add(currTile);
                        }
                    }
                    else if (inGap)
                    {
                        inGap = false;
                    }

                    if (currTile.Cord.X >= Tiles.GetLength(0) - 1 || currTile.Cord.Y >= Tiles.GetLength(1) - 1) { break; }

                    currTile = Tiles[currTile.Cord.X + directionX, currTile.Cord.Y + directionY];
                }

                gapsFound = 0;
                inGap = false;
                //I really love one liners
                currTile = Tiles[(startingTile.Cord.X + directionY) * directionY, (startingTile.Cord.Y + directionX) * directionX];
            }

            if (invalidTiles.Count > 0)
            {
                return (errorMSG, invalidTiles);
            }

            return ("Lazing On A Sunday Afteroon", null);
        }

        private void DuplicatePortalCheck(List<List<WallVisual>> list, List<WallVisual> item)
        {
            WallVisual temp = item[0];

            foreach (var portal in list)
            {
                foreach (var tile in portal)
                {
                    if (tile == temp)
                    {
                        return;
                    }
                }
            }

            list.Add(item);
        }

        public List<(string ErrorMsg, List<WallVisual> InvalidTiles)> FindInvalidOuterWalls()
        {
            Portals.Clear();

            #region Set Up
            graph.Clear();

            List<(string, List<WallVisual>)> result = new List<(string, List<WallVisual>)>();
            HashSet<Vertex> outsideWalls = new HashSet<Vertex>();
            List<Vertex> foundWalls = new List<Vertex>();
            //HashSet<Vertex> possiblePortals = new HashSet<Vertex>();
            bool containsOuterWalls = false;
            string currErrorMsg = "";


            foreach (var tile in Tiles)
            {
                tile.Tint = Color.White;
                Vertex newVert = new Vertex(tile.Cord);
                graph.AddVertex(newVert);
                if (tile.WallState.HasFlag(WallStates.OuterWall))
                {
                    containsOuterWalls = true;
                    newVert.isOuterWall = true;
                    outsideWalls.Add(newVert);
                }
            }

            if (!containsOuterWalls)
            {
                return result;
            }

            #region Creating Edges
            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    int i = x * Tiles.GetLength(1) + y;

                    if (x == 13 && y == 11)
                    {
                        ;
                    }

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

            #endregion

            Vertex startingVertex = (Pathfinders.Dijkstra(graph, graph.vertices[0], outsideWalls, out _, out _, 1)).First();

            foundWalls = Pathfinders.Dijkstra(graph, startingVertex, outsideWalls, out List<Point> jumps, out Vertex lastVertex);

            List<List<WallVisual>> possiblePortals = new List<List<WallVisual>>();

            if (jumps.Count == 1)
            {
                currErrorMsg = "Invalid! No counterpart found";

                result.Add((currErrorMsg, findInvalidTiles(Tiles[jumps.First().X, jumps.First().Y])));
            }

            //find possible portals and return invalid gaps
            foreach (var item in foundWalls)
            {
                if (!Tiles[item.Value.X, item.Value.Y].WallState.HasFlag(WallStates.OuterWall))
                {
                    List<WallVisual> portal = findInvalidTiles(Tiles[item.Value.X, item.Value.Y]);

                    if (portal.Count != 0)
                    {
                        if (portal.Count == 2)
                        {
                            DuplicatePortalCheck(possiblePortals, portal);
                        }
                        else
                        {
                            result.Add(("working on it", portal));
                        }
                    }
                }
            }

            //check if the portals are valid

            foreach (var portal in possiblePortals)
            {
                if (portal.Count != 2)
                {
                    ;
                    continue;
                }

                (string ErrorMSG, List<WallVisual>) portalValidity = PortalValityCheck(portal[0], portal[0].Cord.Y == portal[1].Cord.Y);

                if (portalValidity.ErrorMSG != "Lazing On A Sunday Afteroon")
                {
                    result.Add(portalValidity);
                }
            }

            if (result.Count == 0)
            {
                foreach (var portal in possiblePortals)
                {
                    Point newPortalPos = new Point(1);
                    Point newPortalPos2 = new Point(1);

                    if (portal[0].Cord.X == 0)
                    {
                        newPortalPos.X = Tiles.GetLength(0) - 1;
                        newPortalPos.Y = portal[0].Cord.Y;
                        newPortalPos2 = new Point(newPortalPos.X, portal[1].Cord.Y);
                    }
                    else if (portal[0].Cord.X == Tiles.GetLength(0) - 1)
                    {
                        newPortalPos.X = 0;
                        newPortalPos.Y = portal[0].Cord.Y;
                        newPortalPos2 = new Point(newPortalPos.X, portal[1].Cord.Y);
                    }
                    else if (portal[0].Cord.Y == 0)
                    {
                        newPortalPos.X = portal[0].Cord.X;
                        newPortalPos.Y = Tiles.GetLength(1) - 1;
                        newPortalPos2 = new Point(portal[1].Cord.X, newPortalPos.Y);
                    }
                    else
                    {
                        newPortalPos.X = portal[0].Cord.X;
                        newPortalPos.Y = 0;
                        newPortalPos2 = new Point(portal[1].Cord.X, newPortalPos.Y);
                    }


                    Portals.Add(new (new (portal[0], portal[1]), new (Tiles[newPortalPos.X, newPortalPos.Y], Tiles[newPortalPos2.X, newPortalPos2.Y])));
                    //Portals.Add(new WallVisual[][] { new WallVisual[] { portal[0], portal[1] },
                    //                                 new WallVisual[] {Tiles[newPortalPos.X, newPortalPos.Y], Tiles[newPortalPos2.X, newPortalPos2.Y] } });
                }
            }

            return result;
        }
        #endregion

        public int getWeight(WallVisual ab, WallVisual ba)
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

        public List<WallVisual> GetFilledTiles()
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
            Position = position + new Vector2(WallVisual.EmptySprite.Width / 2, WallVisual.EmptySprite.Height / 2);

            Tiles = new WallVisual[gridSize.X, gridSize.Y];

            for (int x = 0; x < gridSize.X; x++)
            {
                for (int y = 0; y < gridSize.Y; y++)
                {
                    Tiles[x, y] = new WallVisual(WallVisual.EmptySprite, new Point(x, y), Color.White, Position, Vector2.One, new Vector2(WallVisual.EmptySprite.Width / 2f, WallVisual.EmptySprite.Height / 2f), 0f, SpriteEffects.None);

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
