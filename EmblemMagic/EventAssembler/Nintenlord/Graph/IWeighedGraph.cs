using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.Graph;

namespace Nintenlord.Graph
{
    public interface IWeighedGraph<TNode> : IGraph<TNode>
    {
        int GetMovementCost(TNode from, TNode to);

        ICostCollection<TNode> GetTempCostCollection();
    }
}
