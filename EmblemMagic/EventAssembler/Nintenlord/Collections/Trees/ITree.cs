using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Collections.Trees
{
    public interface ITree<out TChild>
    {
        IEnumerable<TChild> GetChildren();
    }

}
