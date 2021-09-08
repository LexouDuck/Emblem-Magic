using GBA;
using System;
using Magic;

namespace KirbyMagic.Kirby
{
    public class KND : Game
    {
        public KND(GameVersion version, bool clean, bool expanded) : base(version, clean, expanded) { }
        
        public static string GameID(GameVersion version)
        {
            switch (version)
            {
                case GameVersion.JAP: return "AGB KIRBY DXA7KJ01";
                case GameVersion.USA: return "AGB KIRBY DXA7KE01";
                case GameVersion.EUR: return "AGB KIRBY DXA7KP01";
                default: throw new Exception("invalid game version given.");
            }
        }
        public static uint Checksum(GameVersion version)
        {
            switch (version)
            {
                case GameVersion.JAP: return 0x7FE05A37;
                case GameVersion.USA: return 0x20EF3F64;
                case GameVersion.EUR: return 0x3B7A7477;
                default: throw new Exception("Invalid game version.");
            }
        }
        public static uint DefaultFileSize(GameVersion version)
        {
            switch (version)
            {
                case GameVersion.JAP: return 0x00800000;
                case GameVersion.USA: return 0x00800000;
                case GameVersion.EUR: return 0x01000000;
                default: throw new Exception("Invalid game version.");
            }
        }

        override public string GetIdentifier()
        {
            return "KND" + (char)Version;
        }
        override public Magic.Range[] GetDefaultFreeSpace()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Magic.Range[]
                    {
                        // TODO
                    };
                case GameVersion.USA: return new Magic.Range[]
                    {
                        // TODO
                    };
                case GameVersion.EUR: return new Magic.Range[]
                    {
                        // TODO
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

                new Repoint("Mini World Map Tileset",  Address_WorldMap()[0]),
                new Repoint("Mini World Map Palette",  Address_WorldMap()[1]),
                new Repoint("Small World Map Tileset", Address_WorldMap()[2]),
                new Repoint("Small World Map Palette", Address_WorldMap()[3]),
                new Repoint("Small World Map TSA",     Address_WorldMap()[4]),
                new Repoint("Large World Map Tileset", Address_WorldMap()[5]),
                new Repoint("Large World Map Palette", Address_WorldMap()[6]),
                new Repoint("Large World Map TSA",     Address_WorldMap()[7]),

                new Repoint("Title Screen BG Palette",   Address_TitleScreen()[0]),
                new Repoint("Title Screen BG Tileset 1", Address_TitleScreen()[1]),
                new Repoint("Title Screen BG Tileset 2", Address_TitleScreen()[2]),
                new Repoint("Title Screen BG TSA",       Address_TitleScreen()[3]),
                new Repoint("Title Screen MG Palette",   Address_TitleScreen()[4]),
                new Repoint("Title Screen MG Tileset",   Address_TitleScreen()[5]),
                new Repoint("Title Screen MG TSA",       Address_TitleScreen()[6]),
                new Repoint("Title Screen FG Palette",   Address_TitleScreen()[7]),
                new Repoint("Title Screen FG Tileset 1", Address_TitleScreen()[8]),
                new Repoint("Title Screen FG Tileset 2", Address_TitleScreen()[9]),
            };
        }



        override public Pointer Address_ClassArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x0);
                case GameVersion.USA: return new Pointer(0x807110);
                case GameVersion.EUR: return new Pointer(0x0);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_ChapterArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x904E1C);
                case GameVersion.USA: return new Pointer(0x8B0890);
                case GameVersion.EUR: return new Pointer(0xFACB2C);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_MapDataArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x907BC8);
                case GameVersion.USA: return new Pointer(0x8B363C);
                case GameVersion.EUR: return new Pointer(0xFAF8D8);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_TextArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x14D08C);
                case GameVersion.USA: return new Pointer(0x15D48C);
                case GameVersion.EUR: return new Pointer(0x35B8AC);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_HuffmanTree()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x14929C);
                case GameVersion.USA: return new Pointer(0x15A72C);
                case GameVersion.EUR: return new Pointer(0x356004);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_HuffmanTreeRoot()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x14D088);
                case GameVersion.USA: return new Pointer(0x15D488);
                case GameVersion.EUR: return new Pointer(0x35B8A8);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_Font_Menu()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x57994C);
                case GameVersion.USA: return new Pointer(0x58C7EC);
                case GameVersion.EUR: return new Pointer(0x798340);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_Font_Bubble()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x593ECC);
                case GameVersion.USA: return new Pointer(0x58F6F4);
                case GameVersion.EUR: return new Pointer(0x79B248);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_TextBubbleTileset()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xA5A35C);
                case GameVersion.USA: return new Pointer(0x9E8238);
                case GameVersion.EUR: return new Pointer(0xA8E72C);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_TextBubblePalette()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xA5A5F8);
                case GameVersion.USA: return new Pointer(0x9E84D4);
                case GameVersion.EUR: return new Pointer(0xA8E9C8);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_MusicArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x214120);
                case GameVersion.USA: return new Pointer(0x224470);
                case GameVersion.EUR: return new Pointer(0x42FFB0);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_PortraitArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x90111C);
                case GameVersion.USA: return new Pointer(0x8ACBC4);
                case GameVersion.EUR: return new Pointer(0xFA8E60);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_MapTerrainNames()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x0);
                case GameVersion.USA: return new Pointer(0x80D374);
                case GameVersion.EUR: return new Pointer(0x0);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_MapSpriteIdleArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x903E0C);
                case GameVersion.USA: return new Pointer(0x8AF880);
                case GameVersion.EUR: return new Pointer(0xFABB1C);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_MapSpriteMoveArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xA13488);
                case GameVersion.USA: return new Pointer(0x9A2E00);
                case GameVersion.EUR: return new Pointer(0xA45B04);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_MapSpritePalettes()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x5C7340);
                case GameVersion.USA: return new Pointer(0x59EE20);
                case GameVersion.EUR: return new Pointer(0x7B13E4);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_DialogBackgroundArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x9CD958);
                case GameVersion.USA: return new Pointer(0x95DD1C);
                case GameVersion.EUR: return new Pointer(0x1059FAC);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_BattleBackgroundArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x7AACC8);
                case GameVersion.USA: return new Pointer(0x75A794);
                case GameVersion.EUR: return new Pointer(0x99AFC8);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_CutsceneScreenArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xAC0524);
                case GameVersion.USA: return new Pointer(0xA3CCEC);
                case GameVersion.EUR: return new Pointer(0x10BDE10);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_ItemIconTileset()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x0);
                case GameVersion.USA: return new Pointer(0x5926F4);
                case GameVersion.EUR: return new Pointer(0x0);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_ItemIconPalette()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x0);
                case GameVersion.USA: return new Pointer(0x5996F4);
                case GameVersion.EUR: return new Pointer(0x0);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_CharacterPaletteArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xEF8008);
                case GameVersion.USA: return new Pointer(0xEF8008);
                case GameVersion.EUR: return new Pointer(0xEF8008);
                default: throw new Exception("Invalid game version.");
            }
        }
        override public Pointer Address_BattleAnimationArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xC00008);
                case GameVersion.USA: return new Pointer(0xC00008);
                case GameVersion.EUR: return new Pointer(0xC00008);
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer Address_SpellAnimationArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0x5FF000);
                case GameVersion.USA: return new Pointer(0x5D4E60);
                case GameVersion.EUR: return new Pointer(0x8153CC);
                default: throw new Exception("Invalid game version.");
            }
        }
        
        override public Pointer Address_BattlePlatformArray()
        {
            switch (Version)
            {
                case GameVersion.JAP: return new Pointer(0xEE0008);
                case GameVersion.USA: return new Pointer(0xEE0008);
                case GameVersion.EUR: return new Pointer(0xEE0008);
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
                        new Pointer(0x85616C), // Tileset
                        new Pointer(0x856AB0), // Palette
                        new Pointer(0x856664), // TSA
                        new Pointer(0x8564D4), // Enemy Name BG
                        new Pointer(0x85654C), // Enemy Weapon BG
                        new Pointer(0x85659C), // Player Name BG
                        new Pointer(0x856614), // Player Weapon BG
                    };
                case GameVersion.USA:
                    return new Pointer[7]
                    {
                        new Pointer(0x801C14), // Tileset
                        new Pointer(0x802558), // Palette
                        new Pointer(0x80210C), // TSA
                        new Pointer(0x801F7C), // Enemy Name BG
                        new Pointer(0x801FF4), // Enemy Weapon BG
                        new Pointer(0x802044), // Player Name BG
                        new Pointer(0x8020BC), // Player Weapon BG
                    };
                case GameVersion.EUR:
                    return new Pointer[7]
                    {
                        new Pointer(0xA42448), // Tileset
                        new Pointer(0xA42D8C), // Palette
                        new Pointer(0xA42940), // TSA
                        new Pointer(0xA427B0), // Enemy Name BG
                        new Pointer(0xA42828), // Enemy Weapon BG
                        new Pointer(0xA42878), // Player Name BG
                        new Pointer(0xA428F0), // Player Weapon BG
                    };
                default: throw new Exception("Invalid game version.");
            }
        }

        override public Pointer[] Address_WorldMap()
        {
            switch (Version)
            {
                case GameVersion.JAP:
                    return new Pointer[8]
                    {
                        new Pointer(0xB26A6C), // mini map graphics
                        new Pointer(0xB2715C), // mini map palette
                        new Pointer(0xB1E9B8), // small map graphics
                        new Pointer(0xB23D3C), // small map palette
                        new Pointer(0xB237EC), // small map TSA
                        new Pointer(0xB085F8), // large map graphics
                        new Pointer(0xB1B278), // large map palette
                        new Pointer(0xB1B2F8)  // large map TSA
                    };
                case GameVersion.USA:
                    return new Pointer[8]
                    {
                        new Pointer(0xAA1280), // mini map graphics
                        new Pointer(0xAA188C), // mini map palette
                        new Pointer(0xA99140), // small map graphics
                        new Pointer(0xA9E4C4), // small map palette
                        new Pointer(0xA9DF74), // small map TSA
                        new Pointer(0xA83364), // large map graphics
                        new Pointer(0xA95FE4), // large map palette
                        new Pointer(0xA96064)  // large map TSA
                    };
                case GameVersion.EUR:
                    return new Pointer[8]
                    {
                        new Pointer(0x10C005C), // mini map graphics pointer array
                        new Pointer(0x112F9F0), // mini map palette
                        new Pointer(0x111CB34), // small map graphics
                        new Pointer(0x1121EB8), // small map palette
                        new Pointer(0x1121968), // small map TSA
                        new Pointer(0x110553C), // large map graphics
                        new Pointer(0x11181BC), // large map palette
                        new Pointer(0x111823C)  // large map TSA
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
                        new Pointer(0xB43988), // BG palette
                        new Pointer(0xB3FCF4), // BG tileset 1
                        new Pointer(0xB41C9C), // BG tileset 2
                        new Pointer(0xB43424), // BG TSA
                        new Pointer(0xB44B20), // MG palette
                        new Pointer(0xB439A8), // MG tileset
                        new Pointer(0xB44838), // MG TSA
                        new Pointer(0xB4678C), // FG palette
                        new Pointer(0xB44B40), // FG tileset 1
                        new Pointer(0xB45958), // FG tileset 2
                    };
                case GameVersion.USA:
                    return new Pointer[]
                    {
                        new Pointer(0xAAB3F4), // BG palette
                        new Pointer(0xAA7760), // BG tileset 1
                        new Pointer(0xAA9708), // BG tileset 2
                        new Pointer(0xAAAE90), // BG TSA
                        new Pointer(0xAAC58C), // MG palette
                        new Pointer(0xAAB414), // MG tileset
                        new Pointer(0xAAC2A4), // MG TSA
                        new Pointer(0xAADB68), // FG palette
                        new Pointer(0xAAC5AC), // FG tileset 1
                        new Pointer(0xAACEDC), // FG tileset 2
                    };
                case GameVersion.EUR:
                    return new Pointer[]
                    {
                        new Pointer(0xB1C5EC), // BG palette
                        new Pointer(0xB18958), // BG tileset 1
                        new Pointer(0xB1A900), // BG tileset 2
                        new Pointer(0xB1C088), // BG TSA
                        new Pointer(0xB1D784), // MG palette
                        new Pointer(0xB1C60C), // MG tileset
                        new Pointer(0xB1D49C), // MG TSA
                        new Pointer(0xB21F04), // FG palette
                        new Pointer(0xB1D7A4), // FG tileset 1
                        new Pointer(0xB1E0D4), // FG tileset 2
                    };
                default: throw new Exception("Invalid game version.");
            }
        }
    }
}
