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

        }

        public bool AddVertex(Vertex<T> vertex)
        {

            return false;
        }

        public bool RemoveVertex(Vertex<T> vertex)
        {

            return false;
        }

        public bool AddEdge(Vertex<T> a, Vertex<T> b)
        {

            return false;
        }

        public bool RemoveEdge(Vertex<T> a, Vertex<T> b)
        {

            return false;
        }
    }
}
