// -----------------------------------------------------------------------
// <copyright file="GraphTravelling.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Nintenlord.Graph
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class GraphTraversal
    {
        public static IEnumerable<TNode> BreadthFirstTraversal<TNode>(
            this IGraph<TNode> graph, 
            TNode startNode)
        {
            HashSet<TNode> travelledNodes = new HashSet<TNode>();

            Queue<TNode> queue = new Queue<TNode>(graph.NodeCount);

            queue.Enqueue(startNode);

            while (queue.Count > 0)
            {
                var nextNode = queue.Dequeue();
                
                yield return nextNode;                

                foreach (var node in graph.GetNeighbours(nextNode))
                {
                    if (!travelledNodes.Contains(node))
                    {
                        queue.Enqueue(node);
                    }
                }
            }
        }

        #region Depth first traversal

        public enum DepthFirstTraversalOrdering
        {
            PreOrdering,
            PostOrdering
        }

        public static IEnumerable<TNode> DepthFirstTraversal<TNode>(
            this IGraph<TNode> graph,
            TNode startNode,
            DepthFirstTraversalOrdering ordering)
        {
            HashSet<TNode> travelledNodes = new HashSet<TNode>();

            switch (ordering)
            {
                case DepthFirstTraversalOrdering.PreOrdering:
                    return DepthFirstTraversalVisitPreOrder(graph, startNode, travelledNodes);
                case DepthFirstTraversalOrdering.PostOrdering:
                    return DepthFirstTraversalVisitPostOrder(graph, startNode, travelledNodes);
                default:
                    throw new ArgumentException();
            }
        }

        private static IEnumerable<TNode> DepthFirstTraversalVisitPreOrder<TNode>(
            IGraph<TNode> graph, TNode node, HashSet<TNode> travelledNodes)
        {
            travelledNodes.Add(node);

            yield return node;

            foreach (var neighbours in graph.GetNeighbours(node))
            {
                foreach (var travelledNode in DepthFirstTraversalVisitPreOrder(graph, neighbours, travelledNodes))
                {
                    yield return travelledNode;
                }
            }
        }

        private static IEnumerable<TNode> DepthFirstTraversalVisitPostOrder<TNode>(
            IGraph<TNode> graph, TNode node, HashSet<TNode> travelledNodes)
        {
            travelledNodes.Add(node);

            foreach (var neighbours in graph.GetNeighbours(node))
            {
                foreach (var travelledNode in DepthFirstTraversalVisitPostOrder(graph, neighbours, travelledNodes))
                {
                    yield return travelledNode;
                }
            }

            yield return node;
        }


        public static IEnumerable<TNode> DepthFirstTraversalAllNodes<TNode>(
            this IGraph<TNode> graph,
            DepthFirstTraversalOrdering ordering)
        {
            HashSet<TNode> travelledNodes = new HashSet<TNode>();

            switch (ordering)
            {
                case DepthFirstTraversalOrdering.PreOrdering:
                    foreach (var node in graph)
                    {
                        if (!travelledNodes.Contains(node))
                        {
                            foreach (var node2 in DepthFirstTraversalVisitPreOrder(graph, node, travelledNodes))
                            {
                                yield return node2;
                            }
                        }

                    }
                    break;
                case DepthFirstTraversalOrdering.PostOrdering:
                    foreach (var node in graph)
                    {
                        if (!travelledNodes.Contains(node))
                        {
                            foreach (var node2 in DepthFirstTraversalVisitPostOrder(graph, node, travelledNodes))
                            {
                                yield return node2;
                            }
                        }
                    }
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        #endregion
    }
}
