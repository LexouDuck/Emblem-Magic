// -----------------------------------------------------------------------
// <copyright file="NeighbourhoodGraph.cs" company="">
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
    public sealed class NeighbourhoodGraph<TNode> : IEditableGraph<TNode>
    {
        Dictionary<TNode, List<TNode>> neighbours;

        public NeighbourhoodGraph(IEnumerable<TNode> nodes)
        {
            this.neighbours = new Dictionary<TNode, List<TNode>>();

            foreach (var node in nodes)
            {
                this.neighbours[node] = new List<TNode>();
            }
        }

        public NeighbourhoodGraph(IEnumerable<KeyValuePair<TNode, IEnumerable<TNode>>> neighbours)
        {
            this.neighbours = new Dictionary<TNode, List<TNode>>();

            foreach (var pair in neighbours)
            {
                this.neighbours[pair.Key] = new List<TNode>(pair.Value);
            }
        }


        #region IEditableGraph<TNode> Members

        public void SetEdge(TNode from, TNode to)
        {
            neighbours[from].Add(to);
        }

        public void RemoveEdge(TNode from, TNode to)
        {
            neighbours[from].Remove(to);
        }
        
        public bool this[TNode from, TNode to]
        {
            get
            {
                return neighbours[from].Contains(to);
            }
            set
            {
                if (value)
                {
                    neighbours[from].Add(to);
                }
                else
                {
                    neighbours[from].Remove(to);
                }
            }
        }

        #endregion

        #region IGraph<TNode> Members

        public int NodeCount
        {
            get { return neighbours.Count; }
        }

        public IEnumerable<TNode> GetNeighbours(TNode node)
        {
            return neighbours[node];
        }

        public bool IsEdge(TNode from, TNode to)
        {
            return neighbours[from].Contains(to);
        }

        #endregion

        #region IEnumerable<TNode> Members

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
