using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Graph.PathFinding
{
    public class DijkstraBasedHeurestic<T> : IHeurestic<T>, IDisposable
    {
        T goal;
        IDictionary<T, int> costs;

        public DijkstraBasedHeurestic(T goal, IWeighedGraph<T> map,
            IEqualityComparer<T> comparer)
        {
            this.goal = goal;
            costs = Dijkstra_algorithm.GetCosts(goal, map, comparer);

        }
        #region IHeurestic<Tile> Members

        public T Goal
        {
            get { return goal; }
        }

        public int GetCostEstimate(T from)
        {
            return costs[from];
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            costs.Clear();
        }

        #endregion
    }

}
