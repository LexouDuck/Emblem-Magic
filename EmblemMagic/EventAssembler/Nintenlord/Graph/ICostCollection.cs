using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Graph
{
    public interface ICostCollection<in TNode>
    {
        /// <summary>
        /// Gets or sets the nodes cost.
        /// </summary>
        /// <param name="node">Node whose value.</param>
        /// <throws>KeyNotFoundException</throws>
        /// <returns>Cost of the node.</returns>
        int this[TNode node] { get; set; }

        /// <summary>
        /// Tries to get the value of the node.
        /// </summary>
        /// <param name="node">Node which value is attempted to take.</param>
        /// <param name="value">Value of the node if result is true.</param>
        /// <returns>True if node is contained in this, else false.</returns>
        bool TryGetValue(TNode node, out int value);

        /// <summary>
        /// Checks if node is in this collection.
        /// </summary>
        /// <param name="node">Node to check.</param>
        /// <returns>True if this collection contains the node, else false.</returns>
        bool ContainsValue(TNode node);

        /// <summary>
        /// Signals that the collection will no longer be used.
        /// </summary>
        void Release();
    }
}
