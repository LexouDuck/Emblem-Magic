using Compression;
using GBA;
using System;
using System.Text;

namespace EmblemMagic.FireEmblem
{
    public enum GameVersion
    {
        JAP = 'J',
        USA = 'U',
        EUR = 'E'
    }



    /// <summary>
    /// The child classes of this 
    /// </summary>
    public abstract class Game
    {
        /// <summary>
        /// The address in the ROM at which the game ID is located
        /// </summary>
        const uint ID_ADDRESS = 0x0000A0;

        /// <summary>
        /// Tells whether or not this ROM is an unmodified Fire Emblem ROM
        /// </summary>
        public Boolean IsClean { get; private set; }
        /// <summary>
        /// Tells whether or not the filesize has been changed
        /// </summary>
        public Boolean Expanded { get; private set; }
        /// <summary>
        /// Which version of the game this is - JAP, USA or EUR
        /// </summary>
        public GameVersion Version { get; private set; }



        /// <summary>
        /// Creates a FE6/FE7/FE8 instance from the given parameters
        /// </summary>
        public Game(GameVersion version, bool clean, bool expanded)
        {
            Version = version;
            IsClean = clean;
            Expanded = expanded;
        }

        /// <summary>
        /// This creates a GameType instance by checking the current ROM
        /// </summary>
        public static Game FromROM(DataManager ROM)
        {
            byte[] id_data = Core.ReadData(new Pointer(ID_ADDRESS), 0x12);
            String id = new String(Encoding.ASCII.GetChars(id_data));

            GameVersion version = 0;
            char game = ' ';
            foreach (GameVersion region in Enum.GetValues(typeof(GameVersion)))
            {
                if (region == GameVersion.JAP)
                if (id.Equals(FE6.GameID(region))) { game = '6'; version = region; break; }
                if (id.Equals(FE7.GameID(region))) { game = '7'; version = region; break; }
                if (id.Equals(FE8.GameID(region))) { game = '8'; version = region; break; }
            }

            bool clean;
            bool expanded;
            switch (game)
            {
                case '6':
                    expanded = (Core.CurrentROMSize == FE6.DefaultFileSize(version));
                    clean = (CRC32.GetChecksum(ROM.FileData) == FE6.Checksum(version));
                    break;
                case '7':
                    expanded = (Core.CurrentROMSize == FE7.DefaultFileSize(version));
                    clean = (CRC32.GetChecksum(ROM.FileData) == FE7.Checksum(version));
                    break;
                case '8':
                    expanded = (Core.CurrentROMSize == FE8.DefaultFileSize(version));
                    clean = (CRC32.GetChecksum(ROM.FileData) == FE8.Checksum(version));
                    break;
                default: throw new Exception("The Fire Emblem game ID could not be identified.");
            }

            switch (game)
            {
                case '6': return new FE6(version, clean, expanded);
                case '7': return new FE7(version, clean, expanded);
                case '8': return new FE8(version, clean, expanded);
                default: return null;
            }
        }



        override public string ToString()
        {
            if (this is FE6) return "Fire Emblem 6" + " (" + Version + ") " + " - " + (IsClean ? "Clean" : "Hacked") + " ROM";
            if (this is FE7) return "Fire Emblem 7" + " (" + Version + ") " + " - " + (IsClean ? "Clean" : "Hacked") + " ROM";
            if (this is FE8) return "Fire Emblem 8" + " (" + Version + ") " + " - " + (IsClean ? "Clean" : "Hacked") + " ROM";
            return "Unknown Game";
        }
        override public bool Equals(object other)
        {
            if (!(other is Game)) return false;
            Game game = (Game)other;
            return (Version == game.Version)
                && (this.GetType() == game.GetType());
        }
        override public int GetHashCode()
        {
            return this.GetType().GetHashCode() ^ Version.GetHashCode() ^ IsClean.GetHashCode() ^ Expanded.GetHashCode();
        }



        /// <summary>
        /// Returns a string identifier of this Fire Emblem game : "FE6J", "FE7U", or "FE8E", etc
        /// </summary>
        public abstract string GetIdentifier();
        /// <summary>
        /// Returns an array of ranges describing known free space for a clean ROM of the given version
        /// </summary>
        public abstract Range[] GetDefaultFreeSpace();
        /// <summary>
        /// Returns an array of 'Repoint's describing the addresses of different core assets and arrays for the game
        /// </summary>
        public abstract Repoint[] GetDefaultPointers();



        /// <summary>
        /// Returns the address at which the array of unit classes is located for this ROM
        /// </summary>
        public abstract Pointer Address_ClassArray();
        /// <summary>
        /// Returns the address at which the chapter array is located for this ROM
        /// </summary>
        public abstract Pointer Address_ChapterArray();
        /// <summary>
        /// Returns the address at which the big map/event data pointer array is located for this ROM
        /// </summary>
        public abstract Pointer Address_MapDataArray();

        /// <summary>
        /// Returns the address at which the text array is located for this ROM
        /// </summary>
        public abstract Pointer Address_TextArray();
        /// <summary>
        /// Returns the address at which the huffman tree begins for this ROM
        /// </summary>
        public abstract Pointer Address_HuffmanTree();
        /// <summary>
        /// Returns the address at which the huffman tree ends for this ROM
        /// </summary>
        public abstract Pointer Address_HuffmanTreeRoot();

        /// <summary>
        /// Returns the address at which the menu font for this ROM is
        /// </summary>
        public abstract Pointer Address_Font_Menu();
        /// <summary>
        /// Returns the address at which the dialogue text bubble font for this ROM is
        /// </summary>
        public abstract Pointer Address_Font_Bubble();
        /// <summary>
        /// Returns the address at which the text bubble graphics is for this game
        /// </summary>
        public abstract Pointer Address_TextBubbleTileset();
        /// <summary>
        /// Returns the address at which the text bubble palette is for this game
        /// </summary>
        public abstract Pointer Address_TextBubblePalette();

        /// <summary>
        /// Returns the address at which the array of songs is for this game
        /// </summary>
        public abstract Pointer Address_MusicArray();

        /// <summary>
        /// Returns the address at which the portrait array is located for this ROM
        /// </summary>
        public abstract Pointer Address_PortraitArray();

        /// <summary>
        /// Returns the address at which the map tileset terrain name text index array is located in this ROM
        /// </summary>
        public abstract Pointer Address_MapTerrainNames();

        /// <summary>
        /// Returns the address of the first (standing) map sprite array
        /// </summary>
        public abstract Pointer Address_MapSpriteIdleArray();
        /// <summary>
        /// Returns the address of the second (walking) map sprite array
        /// </summary>
        /// <returns></returns>
        public abstract Pointer Address_MapSpriteWalkArray();
        /// <summary>
        /// Returns the address at which the 4 palettes for map sprites is located in this ROM
        /// </summary>
        public abstract Pointer Address_MapSpritePalettes();

        /// <summary>
        /// Returns the address at which the array of backgrounds for this FE game is located
        /// </summary>
        public abstract Pointer Address_DialogBackgroundArray();
        /// <summary>
        /// Returns the address at which the array of battle backgrounds for this game is located
        /// </summary>
        public abstract Pointer Address_BattleBackgroundArray();
        /// <summary>
        /// Returns the address at which the array of cutscene screens for this game is located
        /// </summary>
        public abstract Pointer Address_CutsceneScreenArray();

        /// <summary>
        /// Returns the address at which the item icon graphics are for this game
        /// </summary>
        public abstract Pointer Address_ItemIconTileset();
        /// <summary>
        /// Returns the address at which the item icon palette is for this game
        /// </summary>
        public abstract Pointer Address_ItemIconPalette();

        /// <summary>
        /// Returns the address at which the list of character in-battle palettes is
        /// </summary>
        public abstract Pointer Address_CharacterPaletteArray();
        /// <summary>
        /// Returns the address at which the battle animation array starts in this FE game
        /// </summary>
        public abstract Pointer Address_BattleAnimationArray();

        /// <summary>
        /// Returns the address at which the spell animations start in this game
        /// </summary>
        public abstract Pointer Address_SpellAnimationArray();

        /// <summary>
        /// Returns the address at which the array of platforms for the battle screen is
        /// </summary>
        public abstract Pointer Address_BattlePlatformArray();
        /// <summary>
        /// Returns the pointers for the battle screen frame
        /// </summary>
        public abstract Pointer[] Address_BattleScreenFrame();

        /// <summary>
        /// Returns the pointers for both the small world map and large world map (and mini if FE8)
        /// </summary>
        public abstract Pointer[] Address_WorldMap();

        /// <summary>
        /// Returns the pointers for the graphics that make up the title screen
        /// </summary>
        public abstract Pointer[] Address_TitleScreen();
    }
}