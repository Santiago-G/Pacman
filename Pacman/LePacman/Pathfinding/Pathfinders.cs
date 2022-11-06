using System;
using System.Collections.Generic;
using System.Linq;
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

        public static HashSet<Vertex> Dijkstra(Graph graph, Vertex Start, HashSet<Vertex> End)
        {
            HashSet<Vertex> foundVertices = new HashSet<Vertex>();
            int endsFound = 0;

            BinaryHeap<Vertex> PriorityQueue = new BinaryHeap<Vertex>(DijkstraComparer);

            foreach (var vertex in graph.vertices)
            {
                vertex.Visited = false;
                vertex.DistanceFromStart = int.MaxValue;
                vertex.Founder = null;
            }

            Start.DistanceFromStart = 0;

            PriorityQueue.Insert(Start);

            while (endsFound < End.Count)
            {
                currVertex = PriorityQueue.Pop();

                if (currVertex.Visited == false)
                {

                    foreach (var Neighbor in currVertex.Neighbors)
                    {
                        float tentativeDistance = currVertex.DistanceFromStart + Neighbor.Weight;

                        if (tentativeDistance < Neighbor.End.DistanceFromStart)
                        {
                            Neighbor.End.DistanceFromStart = tentativeDistance;
                            Neighbor.End.Founder = currVertex;
                            Neighbor.End.Visited = false;
                        }

                        if (!Neighbor.End.Visited)
                        {
                            PriorityQueue.Insert(Neighbor.End);
                            Neighbor.End.inBinaryHeap = true;
                        }
                    }

                    currVertex.Visited = true;

                    if (End.Contains(currVertex))
                    {
                        endsFound++;
                        foundVertices.Add(currVertex);
                    }
                }
            }

            return End;
        }
    }
}
