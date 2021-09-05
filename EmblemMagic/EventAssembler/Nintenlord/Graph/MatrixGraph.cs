// -----------------------------------------------------------------------
// <copyright file="EditableGraph.cs" company="">
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
    /// Graph where adjacency is implemented as matrix.
    /// </summary>
    public sealed class MatrixGraph : IEditableGraph<int>
    {
        bool[,] neighbours;

        public MatrixGraph(int amountOfNodes)
        {
            neighbours = new bool[amountOfNodes, amountOfNodes];
            this.NodeCount = amountOfNodes;
        }
        
        #region IEditableGraph<int> Members

        public bool this[int from, int to]
        {
            get
            {
                return neighbours[from, to];
            }
            set
            {
                neighbours[from, to] = value;
            }
        }

        public void RemoveEdge(int from, int to)
        {
            neighbours[from, to] = false;
        }

        public void SetEdge(int from, int to)
        {
            neighbours[from, to] = true;
        }
        
        #endregion

        #region IGraph<TNode> Members

        public int NodeCount
        {
            get;
            private set;
        }

        public IEnumerable<int> GetNeighbours(int node)
        {
            for (int i = 0; i < NodeCount; i++)
            {
                if (neighbours[node, i])
                {
                    yield return i;
                }
            }
        }

        public bool IsEdge(int from, int to)
        {
            return neighbours[from, to];
        }

        #endregion

        #region IEnumerable<TNode> Members

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < NodeCount; i++)
            {
                yield return i;
            }
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
