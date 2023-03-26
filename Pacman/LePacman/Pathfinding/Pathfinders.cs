using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LePacman.Pathfinding
{
    public class Pathfinders
    {

        public Queue<Vertex> queue = new Queue<Vertex>();
        static Vertex currVertex;

        static Comparer<Vertex> DijkstraComparer = Comparer<Vertex>.Create((Vertex a, Vertex b) =>
        {
            if (a.DistanceFromStart > b.DistanceFromStart)
            {
                return 1;
            }
            else if (a.DistanceFromStart < b.DistanceFromStart)
            {
                return -1;
            }
            else
            {
                return 0;
            }

        });

        public static List<Vertex> Dijkstra(Graph graph, Vertex Start, HashSet<Vertex> End, out List<Point> jumps, out Vertex lastIndex, int length = -1)
        {
            jumps = new List<Point>();

            if (length == -1)
            {
                length = End.Count;
            }
            HashSet<Vertex> foundVertices = new HashSet<Vertex>();
            List<Vertex> orderedVertices = new List<Vertex>();
            int endsFound = 0;

            BinaryHeap<Vertex> PriorityQueue = new BinaryHeap<Vertex>(DijkstraComparer);

            foreach (var vertex in graph.vertices)
            {
                vertex.Visited = false;
                vertex.DistanceFromStart = float.PositiveInfinity;
                vertex.Founder = null;
                vertex.inBinaryHeap = false;
            }

            Start.DistanceFromStart = 0;

            PriorityQueue.Insert(Start);

            // setup ^

            while (endsFound < length)
            {
                if (currVertex != null && (MathHelper.Distance(currVertex.Value.X, PriorityQueue.Peek().Value.X) > 1.1 || MathHelper.Distance(currVertex.Value.Y, PriorityQueue.Peek().Value.Y) > 1.1))
                {
                    jumps.Add(new Point(currVertex.Value.X, currVertex.Value.Y));
                }

                currVertex = PriorityQueue.Pop();
                if (currVertex.Visited) 
                {
                    continue;
                }

                foreach (var Neighbor in currVertex.Neighbors)
                {
                    float tentativeDistance = currVertex.DistanceFromStart + Neighbor.Weight;

                    if (!Neighbor.End.Visited && tentativeDistance < Neighbor.End.DistanceFromStart)
                    {
                        Neighbor.End.DistanceFromStart = tentativeDistance;
                        Neighbor.End.Founder = currVertex;
                        PriorityQueue.Insert(Neighbor.End);
                        Neighbor.End.inBinaryHeap = true;
                    }
                }

                currVertex.Visited = true;

                if (End.Contains(currVertex))
                {
                    endsFound++;
                }
            }

            lastIndex = currVertex;

            if (length > 1)
            {
                int i = 0;
                foreach (var wall in End)
                {
                    i++;
                    Vertex currVer = wall;
                    while (currVer != null && !foundVertices.Contains(currVer))
                    {
                        foundVertices.Add(currVer);
                        orderedVertices.Add(currVer);
                        currVer = currVer.Founder;
                    }
                }
            }
            else
            {
                orderedVertices.Add(currVertex);
                return orderedVertices;
            }

            return orderedVertices;
        }

        public static void BreadthFirstSearch(Graph graph, Vertex Start, HashSet<Vertex> End)
        {

        }

    }
}
