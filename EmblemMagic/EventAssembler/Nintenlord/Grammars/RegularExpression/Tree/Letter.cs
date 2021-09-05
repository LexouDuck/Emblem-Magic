// -----------------------------------------------------------------------
// <copyright file="Letter.cs" company="">
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
    public sealed class Letter<TLetter> : IRegExExpressionTree<TLetter>
    {
        public readonly TLetter LetterToMatch;

        public Letter(TLetter letter)
        {
            this.LetterToMatch = letter;
        }

        #region IRegExExpressionTree<T> Members

        public RegExNodeTypes Type
        {
            get { return RegExNodeTypes.Letter; }
        }

        #endregion

        #region ITree<TLetter> Members

        public IEnumerable<IRegExExpressionTree<TLetter>> GetChildren()
        {
            yield break;
        }

        #endregion
    }
}
