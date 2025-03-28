#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace JohaToolkit.UnityEngine.DataStructures.Graph
{
    public class Graph
    {
        public List<Vertex> Vertices = new();
        public List<Edge> Edges = new();

        public Vertex AddVertex(string identifier, float posX, float posY, float posZ)
        {
            Vertex? searchVertex = GetVertex(identifier);
            if (searchVertex != null)
            {
                return searchVertex;
            }
            Vertex newVertex = new Vertex(identifier, posX, posY, posZ);
            Vertices.Add(newVertex);
            return newVertex;
        }

        public Edge AddEdge(Vertex start, Vertex end, float weight, bool isOneWay)
        {
            Edge? searchEdge = GetEdge(start, end);
            if (searchEdge != null)
            {
                return searchEdge;
            }
            Edge newEdge = new Edge(start, end, weight, isOneWay);
            Edges.Add(newEdge);
            return newEdge;
        }

        public Edge? GetEdge(Vertex vertexA, Vertex vertexB)
        {
            foreach (Edge edge in Edges)
            {
                if((edge.VertexA == vertexA || edge.VertexA == vertexB) && (edge.VertexB == vertexA || edge.VertexB == vertexB))
                    return edge;
            }
            return null;
        }

        public Vertex? GetVertex(string identifier)
        {
            foreach (Vertex vertex in Vertices)
            {
                if (vertex.Identifier.Equals(identifier))
                    return vertex;
            }
            return null;
        }

        public Edge[] GetEdges(Vertex vertex)
        {
            return Edges.Where(e => (e.VertexA == vertex || e.VertexB == vertex && !e.IsOneWay)).ToArray();
        }

        public Edge[] GetEdges(string identifier)
        {
            return Edges.Where(e => (e.VertexA.Identifier.Equals(identifier) || e.VertexB.Identifier.Equals(identifier) && !e.IsOneWay)).ToArray();
        }

        public Vertex[] GetNeighbours(Vertex vertex)
        {
            return Edges.Where(e => (e.VertexA == vertex)).Select(e => e.VertexB)
                .Concat(Edges.Where(e => (e.VertexB == vertex && !e.IsOneWay)).Select(e => e.VertexA))
                .ToArray();
        }

        public void Prim()
        {
            List<Vertex> markedVertices = new();
            List<Edge> newEdges = new();
            List<Edge> minEdges = new();
            markedVertices.Add(Vertices[0]);
            minEdges.AddRange(GetEdges(markedVertices[0]));

            while(markedVertices.Count < Vertices.Count)
            {
                minEdges = minEdges.Distinct().OrderBy(e => e.Weight).ToList();
                for(int i = 0; i < minEdges.Count; i++)
                {
                    if (!markedVertices.Contains(minEdges[i].VertexA))
                    {
                        markedVertices.Add(minEdges[i].VertexA);
                        newEdges.Add(minEdges[i]);
                        minEdges.AddRange(GetEdges(minEdges[i].VertexA));
                        break;
                    }
                    else if (!markedVertices.Contains(minEdges[i].VertexB))
                    {
                        markedVertices.Add(minEdges[i].VertexB);
                        newEdges.Add(minEdges[i]);
                        minEdges.AddRange(GetEdges(minEdges[i].VertexB));
                        break;
                    }
                }
            }

            Edges = newEdges;
        }

        public bool CanReach(Vertex start, Vertex destination)
        {
            // DSF
            List<Vertex> visited = new();
            visited.Add(start);
            return CanReachRecursive(start, destination, visited);
        }

        private bool CanReachRecursive(Vertex start, Vertex destination, List<Vertex> visited)
        {
            if(start == destination) 
                return true;
            Vertex[] neighbours = GetNeighbours(start).Except(visited).ToArray();
            
            foreach (Vertex neighbour in neighbours)
            {
                if (visited.Contains(neighbour))
                    continue;
                visited.Add(neighbour);
                if(CanReachRecursive(neighbour, destination, visited))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
