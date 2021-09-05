using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Utility.Primitives
{
    public struct CharacterPair : IEquatable<CharacterPair>
    {
        public readonly char First;
        public readonly char Second;

        public CharacterPair(char first, char second)
        {
            this.First = first;
            this.Second = second;
        }

        public static bool operator ==(CharacterPair a, CharacterPair b)
        {
            return a.First == b.First && a.Second == b.Second;
        }

        public static bool operator !=(CharacterPair a, CharacterPair b)
        {
            return !(a == b);
        }

        public bool Equals(CharacterPair other)
        {
            return other == this;
        }

        public override bool Equals(object obj)
        {
            if (obj is CharacterPair)
            {
                return this.Equals((CharacterPair)obj);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return (First << 16) + Second;
        }
    }
}
