using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nintenlord.Collections;
using Nintenlord.Graph;

namespace Nintenlord.Graph.PathFinding
{
    //http://www.policyalmanac.org/games/aStarTutorial.htm
    //http://theory.stanford.edu/~amitp/GameProgramming/AStarComparison.html#S1

    public static class A_Star
    {
        public static List<TNode> GetPath<TNode>(TNode start, TNode goal,
            IWeighedGraph<TNode> map, IHeurestic<TNode> heurestics)
        {
            return GetPath(start, goal, map, heurestics, EqualityComparer<TNode>.Default);
        }

        public static List<TNode> GetPath<TNode>(TNode start, TNode goal,
            IWeighedGraph<TNode> map, IHeurestic<TNode> heurestics, IEqualityComparer<TNode> nodeComparer)
        {
            IPriorityQueue<int, TNode> open = 
                new SkipListPriorityQueue<int, TNode>(10);
            HashSet<TNode> closed = new HashSet<TNode>(nodeComparer);
            ICostCollection<TNode> gCosts = map.GetTempCostCollection();
            ICostCollection<TNode> hCosts = map.GetTempCostCollection();
            IDictionary<TNode, TNode> parents = new Dictionary<TNode, TNode>(nodeComparer);

            open.Enqueue(start, 0);
            gCosts[start] = 0;
            hCosts[start] = 0;
            while (open.Count > 0 && !nodeComparer.Equals(open.Peek(),goal))
            {
                TNode current = open.Dequeue();
                closed.Add(current);

                foreach (TNode neighbour in map.GetNeighbours(current))
                {
                    int gCost = gCosts[current] + map.GetMovementCost(current, neighbour);
                    int oldGcost;
                    if (gCosts.TryGetValue(neighbour, out oldGcost) && gCost < oldGcost)
                    {//If we found a better route to neighbour 
                        int hCost = hCosts[neighbour];
                        open.Remove(neighbour, oldGcost + hCost);
                        closed.Remove(neighbour);

                        gCosts[neighbour] = gCost;
                        open.Enqueue(neighbour, gCost + hCost);
                        parents[neighbour] = current;
                        
                    }
                    else if (!closed.Contains(neighbour) && !open.Contains(neighbour))
                    {//If we got here the first time
                        int hCost = heurestics.GetCostEstimate(neighbour);
                        hCosts[neighbour] = hCost;
                        
                        gCosts[neighbour] = gCost;
                        open.Enqueue(neighbour, gCost + hCost);
                        parents[neighbour] = current;
                    }
                }
            }

            gCosts.Release();
            hCosts.Release();

            if (open.Count == 0)//No path exists
            {
                return new List<TNode>();
            }

            TNode last = open.Dequeue();
            open.Clear();

            List<TNode> result = new List<TNode>();
            while (parents.ContainsKey(last))
            {
                result.Add(last);
                last = parents[last];
            }
            
            result.Add(last);
            result.Reverse();
            return result;
        }
    }
}
