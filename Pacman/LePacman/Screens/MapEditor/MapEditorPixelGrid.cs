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
using LePacman.Screens.MapEditor.Pathfinding;
using System.Diagnostics.Metrics;
using System.Runtime.Serialization.Formatters;
using Microsoft.Xna.Framework.Input;
using System.IO;
using static Microsoft.Xna.Framework.Graphics.SpriteFont;
using Pacman;

namespace LePacman.Screens.MapEditor
{
    public class MapEditorPixelGrid
    {
        Vector2 Position;

        public pixelVisual[,] Tiles;
        public List<pixelVisual> FilledTiles = new List<pixelVisual>();

        public pixelVisual[] FruitTiles = new pixelVisual[2];

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

            float gridXf = Pos.X / Tiles[0, 0].Hitbox.Width;
            float gridYf = Pos.Y / Tiles[0, 0].Hitbox.Height;

            if (gridXf < 0)
            {
                gridXf = -1;
            }
            if (gridYf < 0)
            {
                gridYf = -1;
            }

            int gridX = (int)gridXf;
            int gridY = (int)gridYf;

            Point retPoint = new Point(gridX, gridY);
            if (!IsValid(retPoint)) return new Point(-1);
            return retPoint;
        }

        private bool IsValid(Point gridIndex)
        {
            return gridIndex.X >= 0 && gridIndex.X < Tiles.GetLength(0) && gridIndex.Y >= 0 && gridIndex.Y < Tiles.GetLength(1);
        }

        #region Pacman
        public void AddPacman(Point index)
        {
            MapEditor.selectedPacman = false;
            MapEditor.pacmanPlaced = true;
            Point[] offsets = new Point[] { new Point(1, 1), new Point(0, 1), new Point(0, -1), new Point(1, -1) };

            Tiles[index.X, index.Y].TileStates = States.Pacman;
            Tiles[index.X + 1, index.Y].TileStates = States.Pacman;

            Tiles[index.X, index.Y].UpdateStates();
            Tiles[index.X + 1, index.Y].UpdateStates();

            pacmanTileIndex.Add(index);
            pacmanTileIndex.Add(new Point(index.X + 1, index.Y));

            pacmanOrigin = index;
        }
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

        #endregion

        #region Pellet/Pacman Validation Checks
        private bool ghostChamberExitCheck(Point position)
        {
            if ((int)Tiles[position.X, position.Y].TileStates > 3 || (int)Tiles[position.X + 1, position.Y].TileStates > 3)
            {
                return false;
            }
            return true;
        }

        public int getWeight(pixelVisual ab, pixelVisual ba)
        {
            int abWeight = (int)ab.TileStates;
            int baWeight = (int)ba.TileStates;

            if (ba.isPacmanTile)
            {
                baWeight = 0;
            }
            if (ab.isPacmanTile)
            {
                abWeight = 0;
            }

            if (abWeight < (int)States.Occupied && baWeight < (int)States.Occupied)
            {
                return -1;
            }

            return short.MaxValue;
        }


        public (List<pixelVisual> invalidPellets, bool pacManValid) longJacket(MapEditorWallGrid wallGrid)
        {
            Graph graph = new Graph();

            HashSet<Vertex> targets = new HashSet<Vertex>();

            Point startingPoint = new Point(wallGrid.ghostChamberTiles[3, 0].Cord.X - 1, wallGrid.ghostChamberTiles[3, 0].Cord.Y - 2);

            if (!ghostChamberExitCheck(startingPoint))
            {
                throw new Exception("uh no ghost chamber, make sure you got one mannnnn");
            }

            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    Tiles[x, y].Tint = Color.White;
                    Vertex vertex = new Vertex(Tiles[x, y].Cord);

                    if (Tiles[x, y].TileStates < States.Occupied && Tiles[x, y].TileStates != States.Empty)
                    {
                        if (Tiles[x, y].TileStates == States.Pacman)
                        {
                            vertex.isPacman = true;
                        }
                        targets.Add(vertex);
                    }
                    else if (Tiles[x, y].TileStates >= States.Occupied)
                    {
                        if (Tiles[x, y].isPacmanTile)
                        {
                            vertex.isPacman = true;
                            targets.Add(vertex);
                        }
                        else
                        {
                            vertex.isWall = true;
                        }
                    }
                    graph.AddVertex(vertex);
                }
            }

            Tiles[startingPoint.X, startingPoint.Y].Tint = Color.Red;

            #region Creating Edges
            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    int i = x * Tiles.GetLength(1) + y;


                    if (x != 0)//&& !invalidConnect(Tiles[x, y], Tiles[x - 1, y]))
                    {
                        //no left
                        graph.AddEdge(graph.vertices[i], graph.vertices[i - Tiles.GetLength(1)], getWeight(Tiles[x, y], Tiles[x - 1, y]));
                    }
                    if (x != Tiles.GetLength(0) - 1)// !invalidConnect(Tiles[x, y], Tiles[x + 1, y]))
                    {
                        //no right
                        graph.AddEdge(graph.vertices[i], graph.vertices[i + Tiles.GetLength(1)], getWeight(Tiles[x, y], Tiles[x + 1, y]));
                    }
                    if (y != 0)// && !invalidConnect(Tiles[x, y], Tiles[x, y - 1]))
                    {
                        //no up
                        graph.AddEdge(graph.vertices[i], graph.vertices[i - 1], getWeight(Tiles[x, y], Tiles[x, y - 1]));
                    }
                    if (y != Tiles.GetLength(1) - 1)// && !invalidConnect(Tiles[x, y], Tiles[x, y + 1]))
                    {
                        //no right
                        graph.AddEdge(graph.vertices[i], graph.vertices[i + 1], getWeight(Tiles[x, y], Tiles[x, y + 1]));
                    }
                }
            }

            addPortalEdge(wallGrid, graph);
            //indPacman(wallGrid, graph, targets);

            //the graph should have 3590 edges
            #endregion

            //Graph is Y, X

            Vertex startingVertex = graph.vertices[startingPoint.X * Tiles.GetLength(1) + startingPoint.Y];
            Tiles[startingVertex.Value.X, startingVertex.Value.Y].Tint = Color.Red;

            (List<Vertex> vertices, bool pacmanValid) pathfinderResult = Pathfinders.otherDijkstra(graph, startingVertex, targets);

            List<Vertex> invalidPelletTiles = pathfinderResult.vertices;
            List<pixelVisual> leInvalidPellets = new List<pixelVisual>();

            foreach (var invalidTiles in invalidPelletTiles)
            {
                leInvalidPellets.Add(Tiles[invalidTiles.Value.X, invalidTiles.Value.Y]);
                leInvalidPellets[leInvalidPellets.Count - 1].Tint = Color.Red;
            }

            return (leInvalidPellets, pathfinderResult.pacmanValid);
        }

        private void addPortalEdge(MapEditorWallGrid wallGrid, Graph graph)
        {
            int count = 0;
            foreach (var portalPair in wallGrid.Portals)
            {
                count++;
                // int i = x * Tiles.GetLength(1) + y;
                Point firstPortalTile = Tiles[Math.Clamp(portalPair.firstPortal.secondTile.Cord.X, 0, Tiles.GetLength(0) - 1), Math.Clamp(portalPair.firstPortal.secondTile.Cord.Y, 0, Tiles.GetLength(1) - 1)].Cord;
                Point secondPortalTile = Tiles[Math.Clamp(portalPair.secondPortal.secondTile.Cord.X, 0, Tiles.GetLength(0) - 1), Math.Clamp(portalPair.secondPortal.secondTile.Cord.Y, 0, Tiles.GetLength(1) - 1)].Cord;

                int firstPortalVertexIndex = firstPortalTile.X * Tiles.GetLength(1) + firstPortalTile.Y;
                int secondPortalVertexIndex = secondPortalTile.X * Tiles.GetLength(1) + secondPortalTile.Y;

                graph.AddEdge(graph.vertices[firstPortalVertexIndex], graph.vertices[secondPortalVertexIndex], getWeight(Tiles[firstPortalTile.X, firstPortalTile.Y], Tiles[secondPortalTile.X, secondPortalTile.Y]));
                graph.AddEdge(graph.vertices[secondPortalVertexIndex], graph.vertices[firstPortalVertexIndex], getWeight(Tiles[secondPortalTile.X, secondPortalTile.Y], Tiles[firstPortalTile.X, firstPortalTile.Y]));
            }
        }
        #endregion

        public void addFruit(Point index)
        {
            MapEditor.fruitButton.Tint = Color.Gray;
            MapEditor.selectedFruit = false;
            MapEditor.isFruitPlaced = true;

            Tiles[index.X, index.Y].TileStates = States.Fruit;
            Tiles[index.X + 1, index.Y].TileStates = States.Fruit;

            Tiles[index.X, index.Y].UpdateStates();
            Tiles[index.X + 1, index.Y].UpdateStates();

            FruitTiles[0] = Tiles[index.X, index.Y];
            FruitTiles[1] = Tiles[index.X + 1, index.Y];
        }

        public void RemoveFruit(Point index)
        {
            FruitTiles[0].TileStates = States.Empty;
            FruitTiles[0].UpdateStates();
            FruitTiles[1].TileStates = States.Empty;
            FruitTiles[1].UpdateStates();

            FruitTiles = new pixelVisual[2];

            MapEditor.fruitButton.Tint = Color.White;
            MapEditor.fruitIcon.Position = new Vector2(-1000);

            MapEditor.selectedFruit = false;
            MapEditor.isFruitPlaced = false;
        }

        public void LoadGrid(List<pixelData> TileList)
        {
            Tiles = TileList.Select(x => new pixelVisual(x, Position)).Expand(new Point(Tiles.GetLength(0), Tiles.GetLength(1)));

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
                    case States.Fruit:
                        FilledTiles.Add(tile);
                        break;
                    case States.Pacman:
                        if (Tiles[pacmanOrigin.X, pacmanOrigin.Y] == tile)
                        {
                            tile.CurrentImage = pixelVisual.NBemptySprite;
                            FilledTiles.Add(tile);

                            Tiles[pacmanOrigin.X + 1, pacmanOrigin.Y].CurrentImage = pixelVisual.NBemptySprite;
                            FilledTiles.Add(Tiles[pacmanOrigin.X + 1, pacmanOrigin.Y]);
                        }
                        break;
                }
            }
        }

        public void GoInFocus(List<WallVisual> wallTiles)
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

                Tiles[leftX, topY].isPacmanTile = tile.TileStates == States.Pacman;
                Tiles[leftX, y].isPacmanTile = tile.TileStates == States.Pacman;
                Tiles[x, y].isPacmanTile = tile.TileStates == States.Pacman;
                Tiles[x, topY].isPacmanTile = tile.TileStates == States.Pacman;
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
