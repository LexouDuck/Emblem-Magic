using System;
namespace Nintenlord.Graph
{
    /// <summary>
    /// A graph where edges can be set and removed.
    /// </summary>
    /// <typeparam name="TNode">Type of nodes.</typeparam>
    public interface IEditableGraph<TNode> : IGraph<TNode>
    {
        bool this[TNode from, TNode to] { get; set; }
        void RemoveEdge(TNode from, TNode to);
        void SetEdge(TNode from, TNode to);
    }
}
