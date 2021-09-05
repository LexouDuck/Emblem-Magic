// -----------------------------------------------------------------------
// <copyright file="DictionaryBasedGraph.cs" company="">
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
    public sealed class DictionaryBasedGraph<TNode> : IGraph<TNode>
    {
        readonly IDictionary<TNode, IEnumerable<TNode>> neighbours;

        public DictionaryBasedGraph(IDictionary<TNode, IEnumerable<TNode>> neighbours)
        {
            this.neighbours = neighbours;
        }

        #region IGraph<T> Members

        public int NodeCount
        {
            get { return neighbours.Count; }
        }

        public IEnumerable<TNode> GetNeighbours(TNode node)
        {
            return neighbours[node];
        }

        public bool IsEdge(TNode node1, TNode node2)
        {
            return neighbours[node1].Contains(node2);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<TNode> GetEnumerator()
        {
            return neighbours.Keys.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
