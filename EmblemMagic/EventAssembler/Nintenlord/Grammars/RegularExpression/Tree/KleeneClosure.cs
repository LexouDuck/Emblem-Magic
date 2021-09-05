// -----------------------------------------------------------------------
// <copyright file="KleeneClosure.cs" company="">
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
    public sealed class KleeneClosure<TLetter> : IRegExExpressionTree<TLetter>
    {
        readonly IRegExExpressionTree<TLetter> toRepeat;

        public KleeneClosure(IRegExExpressionTree<TLetter> toRepeat)
        {
            this.toRepeat = toRepeat;
        }

        #region IRegExExpressionTree<TLetter> Members

        public RegExNodeTypes Type
        {
            get { return RegExNodeTypes.KleeneClosure; }
        }

        #endregion

        #region ITree<TLetter> Members

        public IEnumerable<IRegExExpressionTree<TLetter>> GetChildren()
        {
            yield return toRepeat;
        }

        #endregion
    }
}
