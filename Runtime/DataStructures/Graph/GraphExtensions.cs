using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JohaToolkit.UnityEngine.Utility;
using UnityEngine;

namespace JohaToolkit.UnityEngine.DataStructures.Graph
{
    public static class GraphExtensions
    {
        /// <summary>
        /// Determines whether the graph contains the specified node using the default equality comparer.
        /// </summary>
        /// <typeparam name="TNode">The node type.</typeparam>
        /// <typeparam name="TEdge">The edge type.</typeparam>
        /// <param name="graph">The graph instance.</param>
        /// <param name="node">The node to check for.</param>
        /// <returns>True if the node exists in the graph; otherwise, false.</returns>
        public static bool ContainsNode<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode node) where TEdge : IEdge<TNode>
        {
            return ContainsNode(graph, node, EqualityComparer<TNode>.Default);
        }

        /// <summary>
        /// Determines whether the graph contains the specified node using a custom equality comparer.
        /// </summary>
        /// <typeparam name="TNode">The node type.</typeparam>
        /// <typeparam name="TEdge">The edge type.</typeparam>
        /// <param name="graph">The graph instance.</param>
        /// <param name="node">The node to check for.</param>
        /// <param name="comparer">The equality comparer to use.</param>
        /// <returns>True if the node exists in the graph; otherwise, false.</returns>
        public static bool ContainsNode<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode node, IEqualityComparer<TNode> comparer) where TEdge : IEdge<TNode>
        {
            return graph.GetNodes().Contains(node, comparer);
        }
        
        public static IEdge<TNode> GetEdge<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode from, TNode to, IEqualityComparer<TNode> comparer, bool ignoreDirectionality) where TEdge : IEdge<TNode>
        {
            return graph.GetEdges().FirstOrDefault(e => 
                comparer.Equals(e.GetFrom(), from) && comparer.Equals(e.GetTo(), to)
                || ((ignoreDirectionality || e.GetIsBidirectional()) && comparer.Equals(e.GetFrom(), to) && comparer.Equals(e.GetTo(), from))
                );
        }

        /// <summary>
        /// Returns all edges connected to the specified node using the default equality comparer.
        /// </summary>
        /// <typeparam name="TNode">The node type.</typeparam>
        /// <typeparam name="TEdge">The edge type.</typeparam>
        /// <param name="graph">The graph instance.</param>
        /// <param name="node">The node to find edges for.</param>
        /// <returns>An enumerable of edges connected to the node.</returns>
        public static IEnumerable<TEdge> GetEdgesWithNode<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode node) where TEdge : IEdge<TNode>
        {
            return GetEdgesWithNode(graph, node, EqualityComparer<TNode>.Default);
        }

        /// <summary>
        /// Returns all edges connected to the specified node using a custom equality comparer.
        /// </summary>
        /// <typeparam name="TNode">The node type.</typeparam>
        /// <typeparam name="TEdge">The edge type.</typeparam>
        /// <param name="graph">The graph instance.</param>
        /// <param name="node">The node to find edges for.</param>
        /// <param name="comparer">The equality comparer to use.</param>
        /// <returns>An enumerable of edges connected to the node.</returns>
        public static IEnumerable<TEdge> GetEdgesWithNode<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode node, IEqualityComparer<TNode> comparer) where TEdge : IEdge<TNode>
        {
            return graph.GetEdges().Where(e => comparer.Equals(e.GetFrom(), node) || comparer.Equals(e.GetTo(), node));
        }

        /// <summary>
        /// Determines whether the graph contains the specified edge using the default equality comparer.
        /// </summary>
        /// <typeparam name="TNode">The node type.</typeparam>
        /// <typeparam name="TEdge">The edge type.</typeparam>
        /// <param name="graph">The graph instance.</param>
        /// <param name="edge">The edge to check for.</param>
        /// <returns>True if the edge exists in the graph; otherwise, false.</returns>
        public static bool ContainsEdge<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TEdge edge) where TEdge : IEdge<TNode>
        {
            return ContainsEdge(graph, edge, EqualityComparer<TEdge>.Default);
        }

        /// <summary>
        /// Determines whether the graph contains the specified edge using a custom equality comparer.
        /// </summary>
        /// <typeparam name="TNode">The node type.</typeparam>
        /// <typeparam name="TEdge">The edge type.</typeparam>
        /// <param name="graph">The graph instance.</param>
        /// <param name="edge">The edge to check for.</param>
        /// <param name="comparer">The equality comparer to use.</param>
        /// <returns>True if the edge exists in the graph; otherwise, false.</returns>
        public static bool ContainsEdge<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TEdge edge, IEqualityComparer<TEdge> comparer) where TEdge : IEdge<TNode>
        {
            return graph.GetEdges().Contains(edge, comparer);
        }

        /// <summary>
        /// Returns all neighbors of the specified node using the default equality comparer.
        /// </summary>
        /// <typeparam name="TNode">The node type.</typeparam>
        /// <typeparam name="TEdge">The edge type.</typeparam>
        /// <param name="graph">The graph instance.</param>
        /// <param name="node">The node to find neighbors for.</param>
        /// /// <param name="ignoreDirectionality">
        /// If true, considers all edges as bidirectional; if false, respects the directionality of edges.
        /// </param>
        /// <returns>An enumerable of neighboring nodes.</returns>
        public static IEnumerable<TNode> GetNeighbors<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode node, bool ignoreDirectionality = true) where TEdge : IEdge<TNode>
        {
            return graph.GetNeighbors(node, EqualityComparer<TNode>.Default, ignoreDirectionality);
        }

        /// <summary>
        /// Returns all neighbors of the specified node using a custom equality comparer.
        /// </summary>
        /// <typeparam name="TNode">The node type.</typeparam>
        /// <typeparam name="TEdge">The edge type.</typeparam>
        /// <param name="graph">The graph instance.</param>
        /// <param name="node">The node to find neighbors for.</param>
        /// <param name="comparer">The equality comparer to use for node comparison.</param>
        /// <param name="ignoreDirectionality">
        /// If true, considers all edges as bidirectional; if false, respects the directionality of edges.
        /// </param>
        /// <returns>An enumerable of neighboring nodes.</returns>
        public static IEnumerable<TNode> GetNeighbors<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode node, IEqualityComparer<TNode> comparer, bool ignoreDirectionality = true) where TEdge : IEdge<TNode>
        {
            TEdge[] edges = GetEdgesWithNode(graph, node, comparer).ToArray();
            return edges
                .Where(e => (ignoreDirectionality || e.GetIsBidirectional()) || comparer.Equals(e.GetFrom(), node))
                .Select(e => e.GetFrom())
                .Concat(edges.Select(e => e.GetTo()))
                .Where(n => !comparer.Equals(n, node)).Distinct(comparer);
        }

        /// <summary>
        /// Removes all nodes from the graph that do not have any connected edges.
        /// </summary>
        /// <typeparam name="TNode">The node type.</typeparam>
        /// <typeparam name="TEdge">The edge type.</typeparam>
        /// <param name="graph">The graph instance.</param>
        /// <returns>True if all nodes were removed successfully; otherwise, false.</returns>
        public static bool RemoveNodesWithoutEdges<TNode, TEdge>(this IGraph<TNode, TEdge> graph) where TEdge : IEdge<TNode>
        {
            TNode[] nodesToRemove = graph.GetNodes().Where(node => !graph.GetEdgesWithNode(node).Any()).ToArray();
            bool failed = false;
            for (int i = nodesToRemove.Length - 1; i >= 0; i--)
            {
                TNode node = nodesToRemove[i];
                bool result = graph.RemoveNode(node);
                failed = failed || !result;
            }

            return !failed;
        }

        public static (TNode[] path, float cost)[] Dijkstra<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode start, bool ignoreDirectionality, int depth) where TEdge : IEdge<TNode>
        {
            return Dijkstra(graph, start, EqualityComparer<TNode>.Default, ignoreDirectionality, depth);
        }
        
        public static (TNode[] path, float cost)[] Dijkstra<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode start, 
            IEqualityComparer<TNode> comparer, bool ignoreDirectionality, int maxDepth) where TEdge : IEdge<TNode>
        {
            if (!graph.ContainsNode(start, comparer))
            {
                Debug.LogError($"Graph does not contain node {start}");
                return Array.Empty<(TNode[] path, float cost)>();
            }
            
            Dictionary<TNode, float> costs = new(comparer);
            Dictionary<TNode, TNode> parents = new(comparer);
            Dictionary<TNode, int> depths = new(comparer);
            HashSet<TNode> visited = new(comparer);

            Comparer<TNode> comparerByDepth = Comparer<TNode>.Create((a, b) =>
            {
                int cmp = depths[a].CompareTo(depths[b]);
                return cmp != 0 ? cmp
                    // Ensure uniqueness
                    : comparer.GetHashCode(a).CompareTo(comparer.GetHashCode(b));
            });

            SortedSet<TNode> queue = new(comparerByDepth);

            queue.Add(start);
            costs[start] = 0;
            parents[start] = default;
            depths[start] = 0;

            while (queue.Count > 0)
            {
                TNode current = queue.Min;
                queue.Remove(current);

                if (depths[current] >= maxDepth)
                    break;
                
                if (!visited.Add(current))
                    continue;

                foreach (TNode neighbor in graph.GetNeighbors(current, comparer, ignoreDirectionality))
                {
                    if (visited.Contains(neighbor))
                        continue;

                    IEdge<TNode> edge = graph.GetEdge(current, neighbor, comparer, ignoreDirectionality);
                    if (edge == null)
                        continue;

                    float newCost = costs[current] + edge.GetWeight();
                    if (costs.TryGetValue(neighbor, out float oldCost) && !(newCost < oldCost)) 
                        continue;
                    
                    costs[neighbor] = newCost;
                    parents[neighbor] = current;
                    depths[neighbor] = depths[current] + 1;
                    queue.Add(neighbor);
                }
            }

            // Build paths for all reachable nodes except the start
            List<(TNode[] path, float cost)> paths = new();
            foreach ((TNode node, float value) in costs)
            {
                if (comparer.Equals(node, start))
                    continue;

                Stack<TNode> pathStack = new();
                
                TNode current = node;
                
                while (current != null)
                {
                    pathStack.Push(current);
                    current = parents[current];
                }
                paths.Add((pathStack.ToArray(), value));
            }

            return paths.ToArray();
        }
        
        private class DijkstraNode<TNode>
        {
            public TNode Node;
            public DijkstraNode<TNode> Parent;
            public float Cost;
        }
    }
}