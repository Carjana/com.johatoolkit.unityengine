#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using JohaToolkit.UnityEngine.Utility;
using UnityEngine;

namespace JohaToolkit.UnityEngine.DataStructures.Graph
{
    public class JoHaEdge<TNode> : IEdge<TNode>
    {
        public TNode From;
        public TNode To;
        public float Weight;
        public bool IsBidirectional;
        public JoHaEdge(TNode from, TNode to, float weight, bool isBidirectional = true)
        {
            From = from;
            To = to;
            Weight = weight;
            IsBidirectional = isBidirectional;
        }
        
        public TNode GetFrom() => From;
        public TNode GetTo() => To;
        
        public float GetWeight() => Weight;
        public bool GetIsBidirectional() => IsBidirectional;
    }
    
    public class JoHaGraph<TNode> : IGraph<TNode, JoHaEdge<TNode>>
    {
        protected readonly List<TNode> Nodes = new();
        protected readonly List<JoHaEdge<TNode>> Edges = new();
        public IList<TNode> GetNodes() => Nodes;
        public IList<JoHaEdge<TNode>> GetEdges() => Edges;

        protected bool IsCashedNeighboursDirty = true;

        protected class NodeConnection
        {
            public TNode Node;
            public bool Reachable;

            public NodeConnection(TNode node, bool reachable)
            {
                Node = node;
                Reachable = reachable;
            }
        }
        
        protected readonly Dictionary<TNode, List<NodeConnection>> CashedNeighbours = new();
        public FuncEqualityComparer<TNode> NodeEqualityComparer { get; protected set; }
        public FuncEqualityComparer<JoHaEdge<TNode>> EdgeEqualityComparer { get; protected set; }
        
        public JoHaGraph(Func<TNode, TNode, bool> nodeEqualityComparer)
        {
            NodeEqualityComparer = new FuncEqualityComparer<TNode>(nodeEqualityComparer);
            EdgeEqualityComparer = new FuncEqualityComparer<JoHaEdge<TNode>>((e1, e2) =>
            {
                if (!e1.IsBidirectional && !e2.IsBidirectional)
                {
                    // If both edges are not bidirectional, compare them as directed edges
                    return Mathf.Approximately(e1.Weight, e2.Weight) && nodeEqualityComparer.Invoke(e1.From, e2.From) && nodeEqualityComparer.Invoke(e1.To, e2.To);
                }

                if (e1.IsBidirectional && e2.IsBidirectional)
                {
                    // If both edges are bidirectional, compare them as undirected edges
                    return Mathf.Approximately(e1.Weight, e2.Weight)
                           &&
                           (
                               (nodeEqualityComparer.Invoke(e1.From, e2.From) &&
                                nodeEqualityComparer.Invoke(e1.To, e2.To))
                               ||
                               (nodeEqualityComparer.Invoke(e1.From, e2.To) &&
                                nodeEqualityComparer.Invoke(e1.To, e2.From)
                               )
                           );
                }

                return false;
            });
        }
        
        public JoHaGraph(Func<TNode, TNode, bool> nodeEqualityComparer, Func<JoHaEdge<TNode>, JoHaEdge<TNode>, bool> edgeEqualityComparer)
        {
            NodeEqualityComparer = new FuncEqualityComparer<TNode>(nodeEqualityComparer);
            EdgeEqualityComparer = new FuncEqualityComparer<JoHaEdge<TNode>>(edgeEqualityComparer);
        }

        public bool AddNode(TNode node)
        {
            if (this.ContainsNode(node, NodeEqualityComparer))
            {
                return false;
            }
            Nodes.Add(node);
            IsCashedNeighboursDirty = true;
            return true;
        }

        public bool RemoveNode(TNode node)
        {
            if (!this.ContainsNode(node, NodeEqualityComparer))
            {
                return false;
            }

            Nodes.Remove(node);

            JoHaEdge<TNode>[] edges = this.GetEdgesWithNode(node).ToArray();

            for (int i = edges.Length - 1; i >= 0; i--)
            {
                RemoveEdge(edges[i]);
            }

            IsCashedNeighboursDirty = true;
            
            return true;
        }

        public bool AddEdge(TNode from, TNode to, float weight, out JoHaEdge<TNode> edge, bool bidirectional = true)
        {
            edge = new JoHaEdge<TNode>(from, to, weight, bidirectional);
            if (this.ContainsEdge(edge, EdgeEqualityComparer))
                return false;

            AddNode(from);
            AddNode(to);
            
            Edges.Add(edge);
            IsCashedNeighboursDirty = true;
            return true;
        }

        public bool RemoveEdge(JoHaEdge<TNode> edge)
        {
            if (!this.ContainsEdge(edge, EdgeEqualityComparer))
            {
                return false;
            }

            Edges.Remove(edge);
            IsCashedNeighboursDirty = true;
            return true;
        }

        public void Clear()
        {
            Nodes.Clear();
            Edges.Clear();
            CashedNeighbours.Clear();
            IsCashedNeighboursDirty = true;
        }
        
        public TNode[] GetNeighbors(TNode node, bool ignoreDirectionality = true)
        {
            if (IsCashedNeighboursDirty)
            {
                RecalculateCashedNeighbours();
            }

            return CashedNeighbours.TryGetValue(node, out List<NodeConnection>? neighbours)
                ? neighbours.Where(n => ignoreDirectionality || n.Reachable).Select(n => n.Node).ToArray()
                : new TNode[] { };
        }

        private void RecalculateCashedNeighbours()
        {
            CashedNeighbours.Clear();

            foreach (TNode node in Nodes)
            {
                CashedNeighbours[node] = new List<NodeConnection>();
            }

            foreach (JoHaEdge<TNode>? edge in Edges)
            {
                UpdateCashedNeighbours(edge);
            }
            
            IsCashedNeighboursDirty = false;
        }

        private void UpdateCashedNeighbours(JoHaEdge<TNode> edge)
        {
            if (!CashedNeighbours[edge.From].Select(t => t.Node).Contains(edge.To, NodeEqualityComparer))
            {
                CashedNeighbours[edge.From].Add(new NodeConnection(edge.To, true));
            }
            else
            {
                // Update the reachability status if the edge is bidirectional
                NodeConnection existingConnection = CashedNeighbours[edge.From].Find(t => NodeEqualityComparer.Equals(t.Node, edge.To));
                existingConnection.Reachable = true;
            }
                
            if (!CashedNeighbours[edge.To].Select(t => t.Node).Contains(edge.From, NodeEqualityComparer))
            {
                CashedNeighbours[edge.To].Add(new NodeConnection(edge.From, edge.IsBidirectional));
            }
            else
            {
                // Update the reachability status if the edge is bidirectional
                NodeConnection existingConnection = CashedNeighbours[edge.To].Find(t => NodeEqualityComparer.Equals(t.Node, edge.From));
                existingConnection.Reachable = existingConnection.Reachable || edge.IsBidirectional;
            }
        }
    }
}