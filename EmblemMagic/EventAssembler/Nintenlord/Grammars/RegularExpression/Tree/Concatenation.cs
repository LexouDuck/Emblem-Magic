// -----------------------------------------------------------------------
// <copyright file="Concatenation.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Nintenlord.Grammars.RegularExpression.Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public sealed class Concatenation<TLetter> : IRegExExpressionTree<TLetter>
    {
        readonly IRegExExpressionTree<TLetter> first;
        readonly IRegExExpressionTree<TLetter> second;

        public Concatenation(
            IRegExExpressionTree<TLetter> first,
            IRegExExpressionTree<TLetter> second)
        {
            this.first = first;
            this.second = second;
        }

        #region IRegExExpressionTree<TLetter> Members

        public RegExNodeTypes Type
        {
            get { return RegExNodeTypes.Concatenation; }
        }

        #endregion

        #region ITree<IRegExExpressionTree<TLetter>> Members

        public IEnumerable<IRegExExpressionTree<TLetter>> GetChildren()
        {
            yield return first;
            yield return second;
        }

        #endregion
    }
}
