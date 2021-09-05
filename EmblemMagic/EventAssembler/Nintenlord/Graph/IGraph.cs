// -----------------------------------------------------------------------
// <copyright file="IGraph.cs" company="">
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
    /// A graph of nodes and edges.
    /// </summary>
    public interface IGraph<TNode> : IEnumerable<TNode>
    {
        int NodeCount { get; }
        IEnumerable<TNode> GetNeighbours(TNode node);
        bool IsEdge(TNode from, TNode to);
    }
}
