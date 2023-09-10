using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pacman;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace LePacman.Screens.MainGame
{
    public abstract class Ghost : Entity
    {
        private bool shifty = false;
        private EntityStates defaultGhost;
        protected override EntityStates EntityState
        {
            get
            {
                if (shifty) 
                {
                    return defaultGhost | currDirection ^ EntityStates.Shifty;
                }

                return defaultGhost | currDirection;
            }
        }

        protected virtual Point currTargetTile { get; set; }

        protected Point scatterTarget;

        public static GhostStates currGhostState;

        protected EntityStates pendingDirection;


        static Graph<Point> graph = new Graph<Point>();

        //the change between ghost states is gonna be in MainGame based on a timer and if pacman ate power pellet

        public Ghost(Vector2 Position, Color Tint, Vector2 Scale, EntityStates DefaultGhost, Point Coord) : base(Position, Tint, Scale, Coord)
        {
            defaultSize = new Point(14);
            maxSpeed = PelletGrid.Instance.Pacman.maxSpeed * 1.05;
            ļSpeed = PelletGrid.Instance.Pacman.maxSpeed * 1.25;
            animate = true;

            defaultGhost = DefaultGhost;

            animationLimit = TimeSpan.FromMilliseconds(100);
        }

        public static void LoadGrid()
        {
            foreach (var tile in PelletGrid.Instance.gridTiles)
            {
                graph.AddVertex(new Vertex<Point>(tile.coord));
            }

            PelletTileVisual[,] tiles = PelletGrid.Instance.gridTiles;

            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    int i = x * tiles.GetLength(1) + y;

                    if (tiles[x, y].currentState == States.Occupied)
                    {
                        continue;
                    }

                    if (x != 0 && tiles[x - 1, y].currentState != States.Occupied)
                    {
                        //no left
                        graph.AddEdge(graph.Vertices[i], graph.Vertices[i - tiles.GetLength(1)]);
                        if (graph.Vertices[i - tiles.GetLength(1)].value != new Point(x - 1, y)) 
                            ;
                        //PelletGrid[]
                    }
                    if (x != tiles.GetLength(0) - 1 && tiles[x + 1, y].currentState != States.Occupied)
                    {
                        //no right
                        Console.WriteLine(tiles[x + 1, y].currentState);
                        graph.AddEdge(graph.Vertices[i], graph.Vertices[i + tiles.GetLength(1)]);
                        if (graph.Vertices[i + tiles.GetLength(1)].value != new Point(x + 1, y))
                            ;
                    }
                    if (y != 0 && tiles[x, y - 1].currentState != States.Occupied)
                    {
                        //no up
                        Console.WriteLine(tiles[x, y - 1].currentState);
                        graph.AddEdge(graph.Vertices[i], graph.Vertices[i - 1]);
                        if (graph.Vertices[i - 1].value != new Point(x, y - 1))
                            ;
                    }
                    if (y != tiles.GetLength(1) - 1 && tiles[x, y + 1].currentState != States.Occupied)
                    {
                        //no down
                        Console.WriteLine(tiles[x, y + 1].currentState);
                        graph.AddEdge(graph.Vertices[i], graph.Vertices[i + 1]);
                        if (graph.Vertices[i + 1].value != new Point(x, y + 1))
                            ;
                    }
                }
            }

            foreach (var edge in graph.Edges)
            {                
                if (edge.EndingPoint.value == new Point(9))
                {
                    ;
                }
            }
        }

        //they're thing for movement should be a stack of directions that get popped into pending direction
        //this stack should be updated every movement tick (timer)


        //use best first search for pathfinding

        protected EntityStates PointToDirection(Point point)
        {
            if (point == new Point(0, -1))
            {
                return EntityStates.Up;
            }
            if (point == new Point(1, 0))
            {
                return EntityStates.Right;
            }
            if (point == new Point(0, 1))
            {
                return EntityStates.Down;
            }

            return EntityStates.Left;
        }

        override protected void FunnyChair()
        {
            PelletTileVisual[,] tiles = PelletGrid.Instance.gridTiles;
            int currIndex = localPos.X * tiles.GetLength(1) + localPos.Y;

            if (graph.Vertices[currIndex].NeighborCount == 2)
            {
                Point neighborTile = graph.Vertices[currIndex].Neighbors[0].EndingPoint.value;

                if (neighborTile != localPos - directions[currDirection])
                {
                    pendingDirection = PointToDirection(graph.Vertices[currIndex].Neighbors[0].EndingPoint.value - localPos);
                }
                else
                {
                    pendingDirection = PointToDirection(graph.Vertices[currIndex].Neighbors[1].EndingPoint.value - localPos);
                }
            }
            else
            {
                //banned from going up: (12, 11); (15, 11); (12, 23); (15, 23)

                float shortestDistance = 100000;

                foreach (var neighbor in graph.Vertices[currIndex].Neighbors)
                {
                    PelletTileVisual currNeighborTile = tiles[neighbor.EndingPoint.value.X, neighbor.EndingPoint.value.Y];

                    if (currNeighborTile.coord == localPos - directions[currDirection]) { continue; }

                    float currSLDistance = Vector2.Distance(currNeighborTile.Position, currTargetTile.ToVector2());
                    if (currSLDistance < shortestDistance)
                    {
                        shortestDistance = currSLDistance;
                        pendingDirection = PointToDirection(neighbor.EndingPoint.value - localPos);
                    }
                }
            }

            currDirection = pendingDirection;
        }

        protected override void AnimationLogic()
        {
            if (animationTimer > animationLimit)
            {
                shifty = !shifty;

                animationTimer = TimeSpan.Zero;
            }
        }

        public abstract void Update(GameTime gameTime);
    }
}
