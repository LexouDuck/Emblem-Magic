using GBA;
using System;

namespace EmblemMagic.FireEmblem
{
    public class FE6 : Game
    {
        public FE6(GameVersion version, bool clean, bool expanded) : base(version, clean, expanded) { }
        
        public static string GameID(GameVersion version)
        {
            switch (version)
            {
                case GameVersion.JAP: return "FIREEMBLEM6" + '\u0000' + "AFEJ01";
                default: throw new Exception("Invalid game version.");
            }
        }
        public static uint Checksum(GameVersion version)
        {
            switch (version)
            {
                case GameVersion.JAP: return 0xD38763E1;
                default: throw new Exception("Invalid game version.");
            }
        }
        public static uint DefaultFileSize(GameVersion version)
        {
            switch (version)
            {
                case GameVersion.JAP: return 0x00800000;
                default: throw new Exception("Invalid game version.");
            }
        }

        override public string GetIdentifier()
        {
            return "FE6" + (char)Version;
        }
        override public Range[] GetDefaultFreeSpace()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Range[]
                    {
                        new Range(0x7E9940, 0x7E9FEF),
                        new Range(0x7EA640, 0x7EABEF),
                        new Range(0x7FB980, 0x7FBFEF),
                        new Range(0x7FF0B0, 0x7FFFFF)
                    };
                case GameVersion.USA: return new Range[]
                    {
                        new Range(0x817A00, 0xA00000),
                        new Range(0xA297B0, 0xB00000),
                        new Range(0xB013F0, 0x1000000)
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
                new Repoint("Map Sprite Move Array", Address_MapSpriteMoveArray()),
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

                new Repoint("Small World Map Palette", Address_WorldMap()[0]),
                new Repoint("Small World Map Tileset", Address_WorldMap()[1]),
                new Repoint("Large World Map Palette", Address_WorldMap()[2]),
                new Repoint("Large World Map Tileset", Address_WorldMap()[3]),

                new Repoint("Title Screen MG/FG Palette", Address_TitleScreen()[0]),
                new Repoint("Title Screen MG/FG Tileset", Address_TitleScreen()[1]),
                new Repoint("Title Screen FG Tileset",    Address_TitleScreen()[2]),
                new Repoint("Title Screen MG TSA",        Address_TitleScreen()[3]),
                new Repoint("Title Screen BG Tileset",    Address_TitleScreen()[4]),
                new Repoint("Title Screen BG Palette",    Address_TitleScreen()[5]),
            };
        }



        override public Pointer Address_ClassArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x60A0E8);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_ChapterArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x6637A4);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_MapDataArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x664398);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_TextArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x0F635C);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_HuffmanTree()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x0F300C);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_HuffmanTreeRoot()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x0F6358);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_Font_Menu()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x59027C);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_Font_Bubble()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x5A8204);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_TextBubbleTileset()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x2E4394);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_TextBubblePalette()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x2E4630);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_MusicArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x3994D8);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_PortraitArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x66074C);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_MapTerrainNames()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x0);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_MapSpriteIdleArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x662C14);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_MapSpriteMoveArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x6649B4);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_MapSpritePalettes()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x100968);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_DialogBackgroundArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x5C4484);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_BattleBackgroundArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x607504);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_CutsceneScreenArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x68A998);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_ItemIconTileset()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xF9D80);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_ItemIconPalette()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xFED80);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_CharacterPaletteArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x7FC008);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_BattleAnimationArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x6A0008);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_SpellAnimationArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x5D0DA0);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_BattlePlatformArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x7EA008);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer[] Address_BattleScreenFrame()
        {
            switch (Version)
            {
                case GameVersion.JAP:
                    return new Pointer[]
                    {
                        new Pointer(0x1125E0), // Tileset
                        new Pointer(0x112CD4), // Palette
                        new Pointer(0x112968), // TSA
                        new Pointer(0x1127F0), // Enemy Name BG
                        new Pointer(0x112840), // Enemy Weapon BG
                        new Pointer(0x1128AC), // Player Name BG
                        new Pointer(0x1128FC), // Player Weapon BG
                    };
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer[] Address_WorldMap()
        {
            switch (Version)
            {
                case GameVersion.JAP:
                    return new Pointer[4]
                    {
                        new Pointer(0x68C2E0), // small map palette
                        new Pointer(0x68C2DC), // small map graphics
                        new Pointer(0x68C2E8), // large map palette
                        new Pointer(0x68C2E4), // large map graphics array
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
                        new Pointer(0x35CB90), // MG/FG palettes
                        new Pointer(0x35CC90), // MG/FG graphics
                        new Pointer(0x35E6DC), // FG graphics
                        new Pointer(0x35F3A8), // MG TSA (the sword uses 3 palettes)
                        new Pointer(0x35FC3C), // BG graphics
                        new Pointer(0x364418), // BG palette
                    };
                default: throw new Exception("Invalid game version.");
            }
        }
    }
}
