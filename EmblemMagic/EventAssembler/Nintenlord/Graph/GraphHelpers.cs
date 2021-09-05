// -----------------------------------------------------------------------
// <copyright file="GraphHelpers.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Nintenlord.Graph
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Nintenlord.Utility;
    using Nintenlord.Collections.Trees;
    using Nintenlord.Collections;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class GraphHelpers
    {
        public static bool HasCycle<TNode>(this IGraph<TNode> graph, out List<TNode> cycle)
        {
            throw new NotImplementedException();

            var DFS = graph.DepthFirstTraversalAllNodes(
                GraphTraversal.DepthFirstTraversalOrdering.PostOrdering);

            //var tree = graph.DepthFirstSearch();

        }

        /// <summary>
        /// Returns the topological sorting of cycleless graph
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static IEnumerable<TNode> TopologicalSort<TNode>(this IGraph<TNode> graph)
        {
            return graph.DepthFirstTraversalAllNodes(
                GraphTraversal.DepthFirstTraversalOrdering.PostOrdering)
                .Reverse();
        }

        /// <summary>
        /// Returns all nodes in a directed graph with no edges pointing to it.
        /// </summary>
        /// <typeparam name="TNode">Type of nodes</typeparam>
        /// <param name="graph">Graph with nodes</param>
        /// <returns>List of top nodes</returns>
        public static List<TNode> GetTopNodes<TNode>(this IGraph<TNode> graph)
        {
            List<TNode> nodes = graph.ToList();

            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                if (graph.Any(node => graph.IsEdge(node, nodes[i])))
                {
                    nodes.RemoveAt(i);
                }
            }

            return nodes;
        }

        public static IEnumerable<Tuple<TNode, TNode>> GetEdges<TNode>(this IGraph<TNode> graph)
        {
            return from node in graph 
                   from neighbour in graph.GetNeighbours(node) 
                   select Tuple.Create(node, neighbour);
        }

        public static bool IsConnected<TNode>(this IGraph<TNode> graph)
        {
            if (graph.NodeCount == 0)
                return true;

            var transpose = graph.GetTranspose();

            var dfs = transpose.DepthFirstTraversalAllNodes(
                GraphTraversal.DepthFirstTraversalOrdering.PostOrdering).ToList();

            int index = 0;

            while (index < graph.NodeCount)
            {
                int i;
                for (i = index + 1; i < graph.NodeCount; i++)
                {
                    if (graph.IsEdge(dfs[index], dfs[i]))
                    {
                        i = index;
                        break;
                    }
                }
                if (i == graph.NodeCount)
                    break;
            }

            return index == graph.NodeCount - 1;//return if reached top of transpose
        }

        public static TransposeGraph<TNode> GetTranspose<TNode>(this IGraph<TNode> graph)
        {
            return new TransposeGraph<TNode>(graph);
        }
    }
}
