// -----------------------------------------------------------------------
// <copyright file="RegExNodeTypes.cs" company="">
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
    /// Types or regular expression elements
    /// </summary>
    public enum RegExNodeTypes
    {
        /// <summary>
        /// One letter
        /// </summary>
        Letter,
        /// <summary>
        /// A word with no letters in it
        /// </summary>
        EmptyWord,
        /// <summary>
        /// No words
        /// </summary>
        Empty,
        /// <summary>
        /// All finite repetitions of a word
        /// </summary>
        KleeneClosure,
        /// <summary>
        /// Either first word or second word
        /// </summary>
        Choise,
        /// <summary>
        /// Concatenation of two words
        /// </summary>
        Concatenation,
    }
}
