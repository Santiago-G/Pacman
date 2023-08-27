using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LePacman.Screens.MainGame
{
    public abstract class Ghost : Entity
    {
        protected Point currTargetTile;
        protected Point scatterTarget;

        public static GhostStates currGhostState;

        protected Directions pendingDirection;

        Graph<Point> graph = new Graph<Point>();

        //the change between ghost states is gonna be in MainGame based on a timer and if pacman ate power pellet

        public Ghost(Vector2 Position, Color Tint, Vector2 Scale, EntityStates EntityState, Point Coord, Point ScatterTarget) : base(Position, Tint, Scale, EntityState, Coord)
        {
            defaultSize = new Point(14);
            maxSpeed = PelletGrid.Instance.Pacman.maxSpeed * 1.05;
            ļSpeed = PelletGrid.Instance.Pacman.maxSpeed * 1.25;
            scatterTarget = ScatterTarget;

            animationLimit = TimeSpan.FromMilliseconds(100);

            loadGrid();
            ;
        }

        void loadGrid()
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
                    }
                    if (x != tiles.GetLength(0) - 1 && tiles[x + 1, y].currentState != States.Occupied)
                    {
                        //no right
                        graph.AddEdge(graph.Vertices[i], graph.Vertices[i + tiles.GetLength(1)]);
                    }
                    if (y != 0 && tiles[x, y - 1].currentState != States.Occupied)
                    {
                        //no up
                        graph.AddEdge(graph.Vertices[i], graph.Vertices[i - 1]);
                    }
                    if (y != tiles.GetLength(1) - 1 && tiles[x, y + 1].currentState != States.Occupied)
                    {
                        //no down
                        graph.AddEdge(graph.Vertices[i], graph.Vertices[i + 1]);
                    }
                }
            }

            //foreach (var item in graph.Vertices)
            //{
            //    if (item.NeighborCount > 0)
            //    {
            //        PelletGrid.Instance.gridTiles[item.value.X, item.value.Y].currentState = States.Debug;
            //    }
            //}
        }

        //they're thing for movement should be a stack of directions that get popped into pending direction
        //this stack should be updated every movement tick (timer)


        //use best first search for pathfinding

        protected Directions pointToDirection(Point point)
        {
            if (point == new Point(0, -1))
            {
                return Directions.Up;
            }
            if (point == new Point(1, 0))
            {
                return Directions.Right;
            }
            if (point == new Point(0, 1))
            {
                return Directions.Down;
            }

            return Directions.Left;
        }

        protected void evaluateRoute()
        {
            PelletTileVisual[,] tiles = PelletGrid.Instance.gridTiles;
            int currIndex = localPos.X * tiles.GetLength(1) + localPos.Y;

            Directions bannedDirec = Directions.Up;
            switch (currDirection)
            {
                case Directions.Up:
                    bannedDirec = Directions.Down;
                    break;
                case Directions.Down:
                    bannedDirec = Directions.Up;
                    break;
                case Directions.Left:
                    bannedDirec = Directions.Right;
                    break;
                case Directions.Right:
                    bannedDirec = Directions.Left;
                    break;
            }

            //i = x * tiles.GetLength(1) + y;
            if (graph.Vertices[currIndex].NeighborCount == 2)
            {
                Point neighborTile = graph.Vertices[currIndex].Neighbors[0].EndingPoint.value;

                if (neighborTile != localPos - directions[currDirection])
                {
                    pendingDirection = currDirection;
                }
                else
                {
                    pendingDirection = pointToDirection(graph.Vertices[currIndex].Neighbors[1].EndingPoint.value - localPos);
                }

            }
            else
            {
                //banned from going up: (12, 11); (15, 11); (12, 23); (15, 23)

                float shortestStraightLineDistance = 22111122;
                for (int i = 0; i < graph.Vertices[currIndex].NeighborCount; i++)
                {
                    PelletTileVisual currNeighborTile = tiles[graph.Vertices[currIndex].Neighbors[i].EndingPoint.value.X, graph.Vertices[currIndex].Neighbors[i].EndingPoint.value.Y];

                    if (currNeighborTile.coord == localPos - directions[currDirection]) { continue; }
                    //distance formula from the origin of currNegh to target tile
                    float currSLDistance = Math.dist

                    //from origin
                    
                }
            }




        }

        protected void Bre()
        {

        }

        public abstract void Update(GameTime gameTime);
    }
}
