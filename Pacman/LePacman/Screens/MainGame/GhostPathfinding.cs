using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LePacman.Screens.MainGame
{
    public class Vertex<T>
    {
        public T value { get; set; }
        public List<Edge<T>> Neighbors { get; set; }

        public int NeighborCount => Neighbors.Count;
        public Vertex(T Value)
        {
            value = Value;
            Neighbors = new List<Edge<T>>();
        }
    }

    public class Edge<T>
    {
        public Vertex<T> StartingPoint { get; set; }
        public Vertex<T> EndingPoint { get; set; }

        public Edge(Vertex<T> startingPoint, Vertex<T> endingPoint)
        {
            StartingPoint = startingPoint;
            EndingPoint = endingPoint;
        }
    }

    public class Graph<T>
    {
        public List<Vertex<T>> Vertices { get;}
        public List<Edge<T>> Edges { get;}

        public int VertexCount => Vertices.Count;
        public Graph()
        {
            Vertices = new List<Vertex<T>>();
            Edges = new List<Edge<T>>();
        }

        public bool AddVertex(Vertex<T> vertex)
        {
            if (vertex == null || vertex.NeighborCount > 0)
            {
                return false;
            }

            Vertices.Add(vertex);
            return true;
        }

        public bool RemoveVertex(Vertex<T> vertex)
        {
            ;
            return false;
        }

        public bool AddEdge(Vertex<T> a, Vertex<T> b)
        {
            if (a == null || b == null) 
            {
                return false; 
            }

            Edge<T> edgey = new Edge<T>(a, b);

            Edges.Add(edgey);
            a.Neighbors.Add(new Edge<T>(a, b));

            return true;
        }

        public bool RemoveEdge(Vertex<T> a, Vertex<T> b)
        {
            if (a == null || b == null)
            {
                return false;
            }

            ;

            return false;
        }
    }

}
