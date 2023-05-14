using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LePacman.Screens.MapEditor.Pathfinding
{
    public class Graph
    {
        public List<Vertex> vertices;
        private List<Edge> edges;

        public int VertexCount => vertices.Count;

        public Graph()
        {
            vertices = new List<Vertex>();
            edges = new List<Edge>();
        }

        public void AddVertex(Vertex vertex)
        {
            if (vertex == null || vertex.NeighborCount > 0)
            {
                return;
            }

            vertices.Add(vertex);
        }

        public bool RemoveVertex(Vertex vertex)
        {
            if (vertices.Contains(vertex))
            {
                for (int i = 0; i < vertex.NeighborCount; i++)
                {
                    RemoveEdge(vertex, vertex.Neighbors[i].End);
                }

                vertex.Neighbors.Clear();

                for (int i = 0; i < edges.Count; i++)
                {
                    if (edges[i].End == vertex)
                    {
                        RemoveEdge(edges[i].Start, vertex);
                    }
                }

                vertices.Remove(vertex);
                return true;
            }

            return false;
        }

        public bool AddEdge(Vertex a, Vertex b, float weight)
        {
            if (a == null || b == null || GetEdge(a, b) != null)
            {
                throw new Exception("Eins zwei drei alle");
            }

            Edge newEdge = new Edge(a, b, weight);

            a.Neighbors.Add(newEdge);
            edges.Add(newEdge);

            return true;
        }

        public bool RemoveEdge(Vertex a, Vertex b)
        {
            Edge edgeToRemove = GetEdge(a, b);

            if (edgeToRemove != null)
            {
                a.Neighbors.Remove(edgeToRemove);
                edges.Remove(edgeToRemove);

                return true;
            }

            return false;
        }

        public Edge GetEdge(Vertex a, Vertex b)
        {
            if (a != null && b != null && a.Neighbors.Exists((currNeighbor) => currNeighbor.End == b))
            {
                return a.Neighbors.First((currNeighbor) => currNeighbor.End == b);
            }

            return null;
        }

        public void Clear()
        {
            vertices = new List<Vertex>();
            edges = new List<Edge>();
        }
    }
}
