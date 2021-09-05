// -----------------------------------------------------------------------
// <copyright file="Choise.cs" company="">
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
    public sealed class Choise<TLetter> : IRegExExpressionTree<TLetter>
    {
        readonly IRegExExpressionTree<TLetter> firstChoise;
        readonly IRegExExpressionTree<TLetter> secondChoise;

        public Choise(
            IRegExExpressionTree<TLetter> firstChoise, 
            IRegExExpressionTree<TLetter> secondChoise)
        {
            this.firstChoise = firstChoise;
            this.secondChoise = secondChoise;
        }

        #region IRegExExpressionTree<TLetter> Members

        public RegExNodeTypes Type
        {
            get { return RegExNodeTypes.Choise; }
        }

        #endregion

        #region ITree<IRegExExpressionTree<TLetter>> Members

        public IEnumerable<IRegExExpressionTree<TLetter>> GetChildren()
        {
            yield return firstChoise;
            yield return secondChoise;
        }

        #endregion
    }
}
