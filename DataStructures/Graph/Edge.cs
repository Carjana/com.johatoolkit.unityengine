﻿namespace JohaToolkit.Unity.DataStructures.Graph
{
    public class Edge
    {
        public float Weight;
        public Vertex VertexA;
        public Vertex VertexB;
        public bool IsOneWay;
        public Edge(Vertex vertexA, Vertex vertexB, float weight, bool isOneWay)
        {
            Weight = weight;
            VertexA = vertexA;
            VertexB = vertexB;
            IsOneWay = isOneWay;
        }
    }
}
