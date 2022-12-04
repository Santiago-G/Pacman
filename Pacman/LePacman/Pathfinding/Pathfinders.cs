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

        public static HashSet<Vertex> Dijkstra(Graph graph, Vertex Start, HashSet<Vertex> End, int length = -1)
        {
            int numOfJumps = 0;

            if (length == -1)
            {
                length = End.Count;
            }
            HashSet<Vertex> foundVertices = new HashSet<Vertex>();
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
                    numOfJumps++;
                }

                currVertex = PriorityQueue.Pop();
                if (currVertex.Visited) 
                {
                    continue;
                }

                if (currVertex.isOuterWall || currVertex.Value.X == 15 || currVertex.Value.X == 16) 
                {
                    Console.WriteLine($"Position: {currVertex.Value}, Distance from start: {currVertex.DistanceFromStart}");
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

            /* WHATS WRONG WITH THE PATHFINDING

                Distances from start are the same when they should not be, causing multiple paths across the portals/gaps
                likely reasons: incorrectly getting reset/set                
            */

            //loop through every outer wall
            //go up their founders (adding them to found as you do) until you reach a known point
            //at the beginning known point is start
            //after the first time, every vertex you've already visited as you go up the founders is known (because they already led back to start)
            int i = 0;
            foreach (var wall in End)
            {
                i++;
                Vertex currVer = wall;
                while (currVer != null && !foundVertices.Contains(currVer))
                {
                    foundVertices.Add(currVer);
                    currVer = currVer.Founder;
                }
            }

            if (numOfJumps == 1)
            {
                throw new Exception("Airbag 2");
            }
            //fix the gap next to start thing


            return foundVertices;
        }
    }
}
