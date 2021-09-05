// -----------------------------------------------------------------------
// <copyright file="ColouredGraph.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Nintenlord.Graph.Colouring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IColouredGraph<TNode, out TColour> : IGraph<TNode>
    {
        IEnumerable<TColour> GetColours();
    }

    public interface IVertexColouring<TNode, out TColour> : IColouredGraph<TNode, TColour>
    {
        TColour this[TNode node] { get; }
    }

    public interface IEdgeColouring<TNode, out TColour> : IColouredGraph<TNode, TColour>
    {
        TColour this[TNode startNode, TNode endNode] { get; }
    }
}
