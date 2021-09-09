using System;
using System.Collections.Generic;
using GBA;

namespace Magic
{
    public enum GameRegion
    {
        Invalid = 0,
        JAP = 'J',
        USA = 'U',
        EUR = 'E',
    }



    public interface IGame
    {
        /// <summary>
        /// Which game this is (typically a Magic app can romhack several games which share engines)
        /// </summary>
        //public Enum Type { get; }

        /// <summary>
        /// Which version of the game this is - JAP, USA or EUR
        /// </summary>
        public GameRegion Region { get; }

        /// <summary>
        /// The full name of this game (not counting region/version)
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Gets the 4-character string identifier of this game ROM
        /// </summary>
        public String Identifier { get; }

        /// <summary>
        /// Gets the identifier used within the ROM to identify the game
        /// </summary>
        public String ID { get; }

        /// <summary>
        /// Gets the CRC32 checksum of the default version of the current ROM (according to game and version)
        /// </summary>
        public UInt32 Checksum { get; }

        /// <summary>
        /// Gets the default file size of the current ROM (according to game and version)
        /// </summary>
        public UInt32 FileSize { get; }

        /// <summary>
        /// Gets the array of ranges describing known free space for a clean ROM of the given version
        /// </summary>
        public Magic.Range[] FreeSpace { get; }

        /// <summary>
        /// Gets a set of labeled pointers, which describe the inner game data locations
        /// </summary>
        public Dictionary<String, Pointer> Addresses { get; }

        /// <summary>
        /// Returns an array of 'Repoint's describing the addresses of different core assets and arrays for the game
        /// </summary>
        public Repoint[] GetDefaultPointers();
    }
}