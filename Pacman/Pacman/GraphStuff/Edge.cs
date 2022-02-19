using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman
{
    public class Edge<T>
    {
        public Vertex<T> Start { get; set; }
        public Vertex<T> End { get; set; }
        public float Weight { get; set; }

        public Edge(Vertex<T> startingPoint, Vertex<T> endingPoint, float distance)
        {
            Start = startingPoint;
            End = endingPoint;

            Weight = distance;
        }
    }
}
