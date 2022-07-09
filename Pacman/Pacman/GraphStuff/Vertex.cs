using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    public class Vertex<T>
    {
        public int Value { get; set; }
        public List<Edge<T>> Neighbors { get; set; }

        public int NeighborCount => Neighbors.Count;

        public float DistanceFromStart;
        public Vertex<T> Founder;
        public bool Visited;

        public Vector2 Cord;

        public bool isWall;

        public float FinalDistance;

        //A*
        public Vertex(int value, float finalDis, Vector2 cord, bool iswall)
        {
            Neighbors = new List<Edge<T>>();
            Value = value;

            DistanceFromStart = int.MaxValue;
            Founder = null;
            Visited = false;

            FinalDistance = finalDis;
            Cord = cord;

            isWall = iswall;
        }
    }
}
