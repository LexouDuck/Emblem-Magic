// -----------------------------------------------------------------------
// <copyright file="RegExExpressionTree.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Nintenlord.Grammars.RegularExpression.Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Nintenlord.Collections.Trees;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IRegExExpressionTree<TLetter> : ITree<IRegExExpressionTree<TLetter>>
    {
        RegExNodeTypes Type { get; }
    }
}
