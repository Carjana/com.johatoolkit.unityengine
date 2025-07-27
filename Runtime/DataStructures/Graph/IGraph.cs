#nullable enable
using System;
using System.Collections.Generic;

namespace JohaToolkit.UnityEngine.DataStructures.Graph
{
    public interface IGraph<TNode, TEdge>
    where TEdge : IEdge<TNode>
    {
        public IList<TNode> GetNodes();
        public IList<TEdge> GetEdges();
        
        public int NodeCount => GetNodes().Count;
        public int EdgeCount => GetEdges().Count;
        
        public bool AddNode(TNode node);
        public bool RemoveNode(TNode node);
        public bool AddEdge(TNode from, TNode to, float weight, out TEdge edge, bool bidirectional = true);
        public bool RemoveEdge(TEdge edge);

        public void Clear();
    }

    public interface IEdge<TNode>
    {
        public TNode GetFrom();
        public TNode GetTo();
        public float GetWeight() => 1;
        public bool GetIsBidirectional() => true;
    }
}