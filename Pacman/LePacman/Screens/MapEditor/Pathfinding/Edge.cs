using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LePacman.Screens.MapEditor.Pathfinding
{
    public class Edge
    {
        public Vertex Start { get; set; }
        public Vertex End { get; set; }
        public float Weight { get; set; }

        public Edge(Vertex startingPoint, Vertex endingPoint, float distance)
        {
            Start = startingPoint;
            End = endingPoint;

            Weight = distance;
        }
    }
}

