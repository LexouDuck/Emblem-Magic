using GBA;
using System;

namespace EmblemMagic.FireEmblem
{
    public class FE7 : Game
    {
        public FE7(GameVersion version, bool clean, bool expanded) : base(version, clean, expanded) { }
        
        public static string GameID(GameVersion version)
        {
            switch (version)
            {
                case GameVersion.JAP: return "FIREEMBLEM7" + '\u0000' + "AE7J01";
                case GameVersion.USA: return "FIREEMBLEME" + '\u0000' + "AE7E01";
                case GameVersion.EUR: return "FIREEMBLEMY" + '\u0000' + "AE7Y01";
                default: throw new Exception("Invalid game version.");
            }
        }
        public static uint Checksum(GameVersion version)
        {
            switch (version)
            {
                case GameVersion.JAP: return 0xF0C10E72;
                case GameVersion.USA: return 0x2A524221;
                case GameVersion.EUR: return 0x9DECC754;
                default: throw new Exception("Invalid game version.");
            }
        }
        public static uint DefaultFileSize(GameVersion version)
        {
            switch (version)
            {
                case GameVersion.JAP: return 0x01000000;
                case GameVersion.USA: return 0x01000000;
                case GameVersion.EUR: return 0x01000000;
                default: throw new Exception("Invalid game version.");
            }
        }

        override public string GetIdentifier()
        {
            return "FE7" + (char)Version;
        }
        override public Range[] GetDefaultFreeSpace()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Range[]
                    {
                        new Range(0xDCC200, 0xE00000),
                        new Range(0xFA5100, 0xFC0000),
                        new Range(0xFD2E00, 0xFD8000),
                        new Range(0xFE4000, 0xFFF000),
                        new Range(0xFDC000, 0xFE0000)
                    };
                case GameVersion.USA: return new Range[]
                    {
                        new Range(0xD00000, 0xD90000),
                        new Range(0xFA5000, 0xFC0000),
                        new Range(0xFD3E00, 0xFD9000),
                        new Range(0xFDC000, 0xFE0000),
                        new Range(0xFE3100, 0xFFD000),
                        new Range(0xFFE000, 0xFFF140)
                    };
                case GameVersion.EUR: return new Range[]
                    {

                    };
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Repoint[] GetDefaultPointers()
        {
            return new Repoint[]
            {
                new Repoint("Class Array", Address_ClassArray()),
                new Repoint("Chapter Array", Address_ChapterArray()),
                new Repoint("Map Data Array", Address_MapDataArray()),

                new Repoint("Text Array", Address_TextArray()),
                new Repoint("Huffman Tree", Address_HuffmanTree()),
                new Repoint("Huffman Tree Root", Address_HuffmanTreeRoot()),

                new Repoint("Menu Font", Address_Font_Menu()),
                new Repoint("Text Bubble Font", Address_Font_Bubble()),
                new Repoint("Text Bubble Tileset", Address_TextBubbleTileset()),
                new Repoint("Text Bubble Palette", Address_TextBubblePalette()),

                new Repoint("Music Array", Address_MusicArray()),

                new Repoint("Portrait Array", Address_PortraitArray()),

                new Repoint("Map Terrain Names", Address_MapTerrainNames()),

                new Repoint("Map Sprite Idle Array", Address_MapSpriteIdleArray()),
                new Repoint("Map Sprite Walk Array", Address_MapSpriteWalkArray()),
                new Repoint("Map Sprite Palettes", Address_MapSpritePalettes()),

                new Repoint("Dialog Background Array", Address_DialogBackgroundArray()),
                new Repoint("Battle Background Array", Address_BattleBackgroundArray()),
                new Repoint("Cutscene Screen Array",   Address_CutsceneScreenArray()),

                new Repoint("Item Icon Tileset", Address_ItemIconTileset()),
                new Repoint("Item Icon Palette", Address_ItemIconPalette()),

                new Repoint("Character Palette Array", Address_CharacterPaletteArray()),
                new Repoint("Battle Animation Array", Address_BattleAnimationArray()),

                new Repoint("Spell Animation Array", Address_SpellAnimationArray()),

                new Repoint("Battle Platform Array", Address_BattlePlatformArray()),

                new Repoint("Battle Screen Tileset",  Address_BattleScreenFrame()[0]),
                new Repoint("Battle Screen Palettes", Address_BattleScreenFrame()[1]),
                new Repoint("Battle Screen TSA",      Address_BattleScreenFrame()[2]),
                new Repoint("Battle Screen L Name",   Address_BattleScreenFrame()[3]),
                new Repoint("Battle Screen L Weapon", Address_BattleScreenFrame()[4]),
                new Repoint("Battle Screen R Name",   Address_BattleScreenFrame()[5]),
                new Repoint("Battle Screen R Weapon", Address_BattleScreenFrame()[6]),

                new Repoint("Small World Map Tileset", Address_WorldMap()[0]),
                new Repoint("Small World Map Palette", Address_WorldMap()[1]),
                new Repoint("Small World Map TSA",     Address_WorldMap()[2]),
                new Repoint("Large World Map Tileset", Address_WorldMap()[3]),
                new Repoint("Large World Map Palette", Address_WorldMap()[4]),
                new Repoint("Large World Map TSA",     Address_WorldMap()[5]),

                new Repoint("Title Screen BG Palette", Address_TitleScreen()[0]),
                new Repoint("Title Screen BG Tileset", Address_TitleScreen()[1]),
                new Repoint("Title Screen MG Palette", Address_TitleScreen()[2]),
                new Repoint("Title Screen MG Tileset", Address_TitleScreen()[3]),
                new Repoint("Title Screen MG TSA",     Address_TitleScreen()[4]),
                new Repoint("Title Screen FG Palette", Address_TitleScreen()[5]),
                new Repoint("Title Screen FG Tileset", Address_TitleScreen()[6]),
            };
        }



        override public Pointer Address_ClassArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x0);
                case GameVersion.USA: return new Pointer(0xBE015C);
                case GameVersion.EUR: return new Pointer(0x0);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_ChapterArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xD62110);
                case GameVersion.USA: return new Pointer(0xC9A200);
                case GameVersion.EUR: return new Pointer(0xD612B0);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_MapDataArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xD648F4);
                case GameVersion.USA: return new Pointer(0xC9C9C8);
                case GameVersion.EUR: return new Pointer(0xD63A78);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_TextArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xBBB370);
                case GameVersion.USA: return new Pointer(0xB808AC);
                case GameVersion.EUR: return new Pointer(0xC397A0);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_HuffmanTree()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xBB72E0);
                case GameVersion.USA: return new Pointer(0xB7D71C);
                case GameVersion.EUR: return new Pointer(0xC34E30);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_HuffmanTreeRoot()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xBBB36C);
                case GameVersion.USA: return new Pointer(0xB808A8);
                case GameVersion.EUR: return new Pointer(0xC3979C);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_Font_Menu()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xBC1FEC);
                case GameVersion.USA: return new Pointer(0xB896B0);
                case GameVersion.EUR: return new Pointer(0xC4C90C);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_Font_Bubble()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xBDC134);
                case GameVersion.USA: return new Pointer(0xB8B5B0);
                case GameVersion.EUR: return new Pointer(0xC4F6F4);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_TextBubbleTileset()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x4027B0);
                case GameVersion.USA: return new Pointer(0x3FBD34);
                case GameVersion.EUR: return new Pointer(0x1E4810);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_TextBubblePalette()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x402A4C);
                case GameVersion.USA: return new Pointer(0x3FBFD0);
                case GameVersion.EUR: return new Pointer(0x1E4AAC);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_MusicArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x6EA8D0);
                case GameVersion.USA: return new Pointer(0x69D6E0);
                case GameVersion.EUR: return new Pointer(0x67F200);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_PortraitArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xD5E23C);
                case GameVersion.USA: return new Pointer(0xC96584); // Elibian nights 0x1877E6C
                case GameVersion.EUR: return new Pointer(0xD5D634);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_MapTerrainNames()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x0);
                case GameVersion.USA: return new Pointer(0xBE50E8);
                case GameVersion.EUR: return new Pointer(0x0);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_MapSpriteIdleArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xD613B8);
                case GameVersion.USA: return new Pointer(0xC99700);
                case GameVersion.EUR: return new Pointer(0xD607B0);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_MapSpriteWalkArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xD650B4);
                case GameVersion.USA: return new Pointer(0xC9D174);
                case GameVersion.EUR: return new Pointer(0xD64224);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_MapSpritePalettes()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x1900E8);
                case GameVersion.USA: return new Pointer(0x194594);
                case GameVersion.EUR: return new Pointer(0x0D13D0);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_DialogBackgroundArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xC00798);
                case GameVersion.USA: return new Pointer(0xB91588);
                case GameVersion.EUR: return new Pointer(0xC556CC);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_BattleBackgroundArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xC4BD70);
                case GameVersion.USA: return new Pointer(0xBDCA64);
                case GameVersion.EUR: return new Pointer(0xCA3B14);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_CutsceneScreenArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xDB793C);
                case GameVersion.USA: return new Pointer(0xCED888);
                case GameVersion.EUR: return new Pointer(0xDB4CB0);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_ItemIconTileset()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x0);
                case GameVersion.USA: return new Pointer(0xC5EA4);
                case GameVersion.EUR: return new Pointer(0x0);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_ItemIconPalette()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x0);
                case GameVersion.USA: return new Pointer(0xCBEA4);
                case GameVersion.EUR: return new Pointer(0x0);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_CharacterPaletteArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xFD8008);
                case GameVersion.USA: return new Pointer(0xFD8008);
                case GameVersion.EUR: return new Pointer(0xFD8008);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_BattleAnimationArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xE00008);
                case GameVersion.USA: return new Pointer(0xE00008);
                case GameVersion.EUR: return new Pointer(0xE00008);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_SpellAnimationArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xC1071C);
                case GameVersion.USA: return new Pointer(0xBA13D0);
                case GameVersion.EUR: return new Pointer(0xC68540);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_BattlePlatformArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xFC0008);
                case GameVersion.USA: return new Pointer(0xFC0008);
                case GameVersion.EUR: return new Pointer(0xFC0008);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer[] Address_BattleScreenFrame()
        {
            switch (Version)
            {
                case GameVersion.JAP:
                    return new Pointer[7]
                    {
                        new Pointer(0x1DE528), // Tileset
                        new Pointer(0x1DEC14), // Palette
                        new Pointer(0x1DE8A8), // TSA
                        new Pointer(0x1DE730), // Enemy Name BG
                        new Pointer(0x1DE780), // Enemy Weapon BG
                        new Pointer(0x1DE7EC), // Player Name BG
                        new Pointer(0x1DE83C), // Player Weapon BG
                    };
                case GameVersion.USA:
                    return new Pointer[7]
                    {
                        new Pointer(0x1D88B4), // Tileset
                        new Pointer(0x1D8FA0), // Palette
                        new Pointer(0x1D8C34), // TSA
                        new Pointer(0x1D8ABC), // Enemy Name BG
                        new Pointer(0x1D8B0C), // Enemy Weapon BG
                        new Pointer(0x1D8B78), // Player Name BG
                        new Pointer(0x1D8BC8), // Player Weapon BG
                    };
                case GameVersion.EUR:
                    return new Pointer[7]
                    {
                        new Pointer(0x2BE6DC), // Tileset
                        new Pointer(0x2BEDC8), // Palette
                        new Pointer(0x2BEA5C), // TSA
                        new Pointer(0x2BE8E4), // Enemy Name BG
                        new Pointer(0x2BE934), // Enemy Weapon BG
                        new Pointer(0x2BE9A0), // Player Name BG
                        new Pointer(0x2BE9F0), // Player Weapon BG
                    };
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer[] Address_WorldMap()
        {
            switch (Version)
            {
                case GameVersion.JAP:
                    return new Pointer[6]
                    {
                        new Pointer(0x60A8EC), // small map graphics
                        new Pointer(0x60A86C), // small map palette
                        new Pointer(0x60F964), // small map TSA
                        new Pointer(0xDB1074), // large map graphics
                        new Pointer(0x5AE7BC), // large map palette
                        new Pointer(0xDB10A4)  // large map TSA
                    };
                case GameVersion.USA:
                    return new Pointer[6]
                    {
                        new Pointer(0x5D0AC0), // small map graphics
                        new Pointer(0x5D0A40), // small map palette
                        new Pointer(0x5D5B38), // small map TSA
                        new Pointer(0xCE7818), // large map graphics
                        new Pointer(0x574990), // large map palette
                        new Pointer(0xCE7848)  // large map TSA
                    };
                case GameVersion.EUR:
                    return new Pointer[6]
                    {
                        new Pointer(0x64DB88), // small map graphics
                        new Pointer(0x64DB08), // small map palette
                        new Pointer(0x652C00), // small map TSA
                        new Pointer(0xDAEC40), // large map graphics
                        new Pointer(0x5F1A58), // large map palette
                        new Pointer(0xDAEC70)  // large map TSA
                    };
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer[] Address_TitleScreen()
        {
            switch (Version)
            {
                case GameVersion.JAP:
                    return new Pointer[]
                    {
                        new Pointer(0x6B73E0), // background palette
                        new Pointer(0x6B7400), // background tileset
                        new Pointer(0x6BB6E8), // middlelayer palette
                        new Pointer(0x6BB708), // middlelayer tileset
                        new Pointer(0x6BBF90), // middlelayer TSA
                        new Pointer(0x6BC444), // foreground palette
                        new Pointer(0x6BC4E4), // foreground tileset
                    };
                case GameVersion.USA:
                    return new Pointer[]
                    {
                        new Pointer(0x66AF6C), // background palette
                        new Pointer(0x66AF8C), // background tileset
                        new Pointer(0x66F274), // middlelayer palette
                        new Pointer(0x66F294), // middlelayer tileset
                        new Pointer(0x66FB1C), // middlelayer TSA
                        new Pointer(0x66FCE0), // foreground palette
                        new Pointer(0x66FD80), // foreground tileset
                    };
                case GameVersion.EUR:
                    return new Pointer[]
                    {
                        new Pointer(0x1C7424), // background palette
                        new Pointer(0x1C7444), // background tileset
                        new Pointer(0x1CB72C), // middlelayer palette
                        new Pointer(0x1CB74C), // middlelayer tileset
                        new Pointer(0x1CBFD4), // middlelayer TSA
                        new Pointer(0x1CC198), // foreground palette
                        new Pointer(0x1CC238), // foreground tileset
                    };
                default: throw new Exception("Invalid game version.");
            }
        }
    }
}
