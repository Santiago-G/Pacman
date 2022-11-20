using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LePacman.Pathfinding
{
    public class Vertex
    {
        public Point Value { get; set; }
        public List<Edge> Neighbors { get; set; }

        public int NeighborCount => Neighbors.Count;

        public float DistanceFromStart;
        public Vertex Founder;
        public bool Visited;

        public bool isOuterWall = false;
        public bool inBinaryHeap;

        //Dijkstra
        public Vertex(Point Position)
        {
            Neighbors = new List<Edge>();
            Value = Position;

            DistanceFromStart = short.MaxValue;
            Founder = null;
            Visited = false;
        }
    }
}
