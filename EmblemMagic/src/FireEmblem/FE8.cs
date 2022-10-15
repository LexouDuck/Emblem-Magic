using System;
using System.Collections.Generic;
using GBA;
using Magic;

namespace EmblemMagic.FireEmblem
{
    public abstract class FE8 : FireEmblem.Game
    {
        //public FE8(GameRegion region) : base(GameType.FE8, region) { }

        public override GameType Type => GameType.FE8;
        public override String Name => "Fire Emblem 8";

        public static Game FromRegion(GameRegion version)
        {
            switch (version)
            {
                case GameRegion.JAP: return new FE8J();
                case GameRegion.USA: return new FE8U();
                case GameRegion.EUR: return new FE8E();
            }
            throw new Exception("Invalid game ROM region (" + version + ")");
        }
    }



    public class FE8J : FE8
    {
        //public FE8J() : base(GameRegion.JAP) { }

        public override GameRegion Region => GameRegion.JAP;

        public override String Identifier => "FE8J";
        public override String ID => "FIREEMBLEM8\0BE8J01";
        public override UInt32 Checksum => 0x9D76826F;
        public override UInt32 FileSize => 0x01000000;
        public override Magic.Range[] FreeSpace => new Magic.Range[]
        {
            new Magic.Range(0xBC3A00, 0xC00000),
            new Magic.Range(0xE47200, 0xEE0000),
            new Magic.Range(0xEF3000, 0xEF8000),
            new Magic.Range(0xEFB300, 0xFE0000),
            new Magic.Range(0xFE4000, 0xFFF000),
        };
        public override Dictionary<String, Pointer> Addresses => new Dictionary<String, Pointer>()
        {
            ["Class Array"]               = new Pointer(0x0),//Address_ClassArray()
            ["Chapter Array"]             = new Pointer(0x904E1C),//Address_ChapterArray()
            ["Map Data Array"]            = new Pointer(0x907BC8),//Address_MapDataArray()

            ["Text Array"]                = new Pointer(0x14D08C),//Address_TextArray()
            ["Huffman Tree"]              = new Pointer(0x14929C),//Address_HuffmanTree()
            ["Huffman Tree Root"]         = new Pointer(0x14D088),//Address_HuffmanTreeRoot()

            ["Menu Font"]                 = new Pointer(0x57994C),//Address_Font_Menu()
            ["Text Bubble Font"]          = new Pointer(0x593ECC),//Address_Font_Bubble()
            ["Text Bubble Tileset"]       = new Pointer(0xA5A35C),//Address_TextBubbleTileset()
            ["Text Bubble Palette"]       = new Pointer(0xA5A5F8),//Address_TextBubblePalette()

            ["Music Array"]               = new Pointer(0x214120),//Address_MusicArray()

            ["Portrait Array"]            = new Pointer(0x90111C),//Address_PortraitArray()

            ["Map Terrain Names"]         = new Pointer(0x0),//Address_MapTerrainNames()

            ["Map Sprite Idle Array"]     = new Pointer(0x903E0C),//Address_MapSpriteIdleArray()
            ["Map Sprite Move Array"]     = new Pointer(0xA13488),//Address_MapSpriteMoveArray()
            ["Map Sprite Palettes"]       = new Pointer(0x5C7340),//Address_MapSpritePalettes()

            ["Dialog Background Array"]   = new Pointer(0x9CD958),//Address_DialogBackgroundArray()
            ["Battle Background Array"]   = new Pointer(0x7AACC8),//Address_BattleBackgroundArray()
            ["Cutscene Screen Array"  ]   = new Pointer(0xAC0524),//Address_CutsceneScreenArray()

            ["Item Icon Tileset"]         = new Pointer(0x0),//Address_ItemIconTileset()
            ["Item Icon Palette"]         = new Pointer(0x0),//Address_ItemIconPalette()

            ["Character Palette Array"]   = new Pointer(0xEF8008),//Address_CharacterPaletteArray()
            ["Battle Animation Array"]    = new Pointer(0xC00008),//Address_BattleAnimationArray()

            ["Spell Animation Array"]     = new Pointer(0x5FF000),//Address_SpellAnimationArray()

            ["Battle Platform Array"]     = new Pointer(0xEE0008),//Address_BattlePlatformArray()

            ["Battle Screen Tileset" ]    = new Pointer(0x85616C),//Address_BattleScreenFrame()[0] // Tileset
            ["Battle Screen Palettes"]    = new Pointer(0x856AB0),//Address_BattleScreenFrame()[1] // Palette
            ["Battle Screen TSA"     ]    = new Pointer(0x856664),//Address_BattleScreenFrame()[2] // TSA
            ["Battle Screen L Name"  ]    = new Pointer(0x8564D4),//Address_BattleScreenFrame()[3] // Enemy Name BG
            ["Battle Screen L Weapon"]    = new Pointer(0x85654C),//Address_BattleScreenFrame()[4] // Enemy Weapon BG
            ["Battle Screen R Name"  ]    = new Pointer(0x85659C),//Address_BattleScreenFrame()[5] // Player Name BG
            ["Battle Screen R Weapon"]    = new Pointer(0x856614),//Address_BattleScreenFrame()[6] // Player Weapon BG

            ["Mini World Map Tileset" ]   = new Pointer(0xB26A6C),//Address_WorldMap()[0] // mini map graphics
            ["Mini World Map Palette" ]   = new Pointer(0xB2715C),//Address_WorldMap()[1] // mini map palette
            ["Small World Map Tileset"]   = new Pointer(0xB1E9B8),//Address_WorldMap()[2] // small map graphics
            ["Small World Map Palette"]   = new Pointer(0xB23D3C),//Address_WorldMap()[3] // small map palette
            ["Small World Map TSA"    ]   = new Pointer(0xB237EC),//Address_WorldMap()[4] // small map TSA
            ["Large World Map Tileset"]   = new Pointer(0xB085F8),//Address_WorldMap()[5] // large map graphics
            ["Large World Map Palette"]   = new Pointer(0xB1B278),//Address_WorldMap()[6] // large map palette
            ["Large World Map TSA"    ]   = new Pointer(0xB1B2F8),//Address_WorldMap()[7] // large map TSA

            ["Title Screen BG Palette"  ] = new Pointer(0xB43988),//Address_TitleScreen()[0]
            ["Title Screen BG Tileset 1"] = new Pointer(0xB3FCF4),//Address_TitleScreen()[1]
            ["Title Screen BG Tileset 2"] = new Pointer(0xB41C9C),//Address_TitleScreen()[2]
            ["Title Screen BG TSA"      ] = new Pointer(0xB43424),//Address_TitleScreen()[3]
            ["Title Screen MG Palette"  ] = new Pointer(0xB44B20),//Address_TitleScreen()[4]
            ["Title Screen MG Tileset"  ] = new Pointer(0xB439A8),//Address_TitleScreen()[5]
            ["Title Screen MG TSA"      ] = new Pointer(0xB44838),//Address_TitleScreen()[6]
            ["Title Screen FG Palette"  ] = new Pointer(0xB4678C),//Address_TitleScreen()[7]
            ["Title Screen FG Tileset 1"] = new Pointer(0xB44B40),//Address_TitleScreen()[8]
            ["Title Screen FG Tileset 2"] = new Pointer(0xB45958),//Address_TitleScreen()[9]
        };
    }



    public class FE8U : FE8
    {
        //public FE8U() : base(GameRegion.USA) { }

        public override GameRegion Region => GameRegion.USA;

        public override String Identifier => "FE8U";
        public override String ID => "FIREEMBLEM2EBE8E01";
        public override UInt32 Checksum => 0xA47246AE;
        public override UInt32 FileSize => 0x01000000;
        public override Magic.Range[] FreeSpace => new Magic.Range[]
        {
            new Magic.Range(0xB2A610, 0xC00000),
            new Magic.Range(0xE47200, 0xEE0000),
            new Magic.Range(0xEF3000, 0xEF8000),
            new Magic.Range(0xEFB000, 0xFDFD00),
            new Magic.Range(0xFE4000, 0xFFF000),
        };
        public override Dictionary<String, Pointer> Addresses => new Dictionary<String, Pointer>()
        {
            ["Class Array"]               = new Pointer(0x807110),//Address_ClassArray()
            ["Chapter Array"]             = new Pointer(0x8B0890),//Address_ChapterArray()
            ["Map Data Array"]            = new Pointer(0x8B363C),//Address_MapDataArray()

            ["Text Array"]                = new Pointer(0x15D48C),//Address_TextArray()
            ["Huffman Tree"]              = new Pointer(0x15A72C),//Address_HuffmanTree()
            ["Huffman Tree Root"]         = new Pointer(0x15D488),//Address_HuffmanTreeRoot()

            ["Menu Font"]                 = new Pointer(0x58C7EC),//Address_Font_Menu()
            ["Text Bubble Font"]          = new Pointer(0x58F6F4),//Address_Font_Bubble()
            ["Text Bubble Tileset"]       = new Pointer(0x9E8238),//Address_TextBubbleTileset()
            ["Text Bubble Palette"]       = new Pointer(0x9E84D4),//Address_TextBubblePalette()

            ["Music Array"]               = new Pointer(0x224470),//Address_MusicArray()

            ["Portrait Array"]            = new Pointer(0x8ACBC4),//Address_PortraitArray()

            ["Map Terrain Names"]         = new Pointer(0x80D374),//Address_MapTerrainNames()

            ["Map Sprite Idle Array"]     = new Pointer(0x8AF880),//Address_MapSpriteIdleArray()
            ["Map Sprite Move Array"]     = new Pointer(0x9A2E00),//Address_MapSpriteMoveArray()
            ["Map Sprite Palettes"]       = new Pointer(0x59EE20),//Address_MapSpritePalettes()

            ["Dialog Background Array"]   = new Pointer(0x95DD1C),//Address_DialogBackgroundArray()
            ["Battle Background Array"]   = new Pointer(0x75A794),//Address_BattleBackgroundArray()
            ["Cutscene Screen Array"  ]   = new Pointer(0xA3CCEC),//Address_CutsceneScreenArray()

            ["Item Icon Tileset"]         = new Pointer(0x5926F4),//Address_ItemIconTileset()
            ["Item Icon Palette"]         = new Pointer(0x5996F4),//Address_ItemIconPalette()

            ["Character Palette Array"]   = new Pointer(0xEF8008),//Address_CharacterPaletteArray()
            ["Battle Animation Array"]    = new Pointer(0xC00008),//Address_BattleAnimationArray()

            ["Spell Animation Array"]     = new Pointer(0x5D4E60),//Address_SpellAnimationArray()

            ["Battle Platform Array"]     = new Pointer(0xEE0008),//Address_BattlePlatformArray()

            ["Battle Screen Tileset" ]    = new Pointer(0x801C14),//Address_BattleScreenFrame()[0] // Tileset
            ["Battle Screen Palettes"]    = new Pointer(0x802558),//Address_BattleScreenFrame()[1] // Palette
            ["Battle Screen TSA"     ]    = new Pointer(0x80210C),//Address_BattleScreenFrame()[2] // TSA
            ["Battle Screen L Name"  ]    = new Pointer(0x801F7C),//Address_BattleScreenFrame()[3] // Enemy Name BG
            ["Battle Screen L Weapon"]    = new Pointer(0x801FF4),//Address_BattleScreenFrame()[4] // Enemy Weapon BG
            ["Battle Screen R Name"  ]    = new Pointer(0x802044),//Address_BattleScreenFrame()[5] // Player Name BG
            ["Battle Screen R Weapon"]    = new Pointer(0x8020BC),//Address_BattleScreenFrame()[6] // Player Weapon BG

            ["Mini World Map Tileset" ]   = new Pointer(0xAA1280),//Address_WorldMap()[0] // mini map graphics
            ["Mini World Map Palette" ]   = new Pointer(0xAA188C),//Address_WorldMap()[1] // mini map palette
            ["Small World Map Tileset"]   = new Pointer(0xA99140),//Address_WorldMap()[2] // small map graphics
            ["Small World Map Palette"]   = new Pointer(0xA9E4C4),//Address_WorldMap()[3] // small map palette
            ["Small World Map TSA"    ]   = new Pointer(0xA9DF74),//Address_WorldMap()[4] // small map TSA
            ["Large World Map Tileset"]   = new Pointer(0xA83364),//Address_WorldMap()[5] // large map graphics
            ["Large World Map Palette"]   = new Pointer(0xA95FE4),//Address_WorldMap()[6] // large map palette
            ["Large World Map TSA"    ]   = new Pointer(0xA96064),//Address_WorldMap()[7] // large map TSA

            ["Title Screen BG Palette"  ] = new Pointer(0xAAB3F4),//Address_TitleScreen()[0]
            ["Title Screen BG Tileset 1"] = new Pointer(0xAA7760),//Address_TitleScreen()[1]
            ["Title Screen BG Tileset 2"] = new Pointer(0xAA9708),//Address_TitleScreen()[2]
            ["Title Screen BG TSA"      ] = new Pointer(0xAAAE90),//Address_TitleScreen()[3]
            ["Title Screen MG Palette"  ] = new Pointer(0xAAC58C),//Address_TitleScreen()[4]
            ["Title Screen MG Tileset"  ] = new Pointer(0xAAB414),//Address_TitleScreen()[5]
            ["Title Screen MG TSA"      ] = new Pointer(0xAAC2A4),//Address_TitleScreen()[6]
            ["Title Screen FG Palette"  ] = new Pointer(0xAADB68),//Address_TitleScreen()[7]
            ["Title Screen FG Tileset 1"] = new Pointer(0xAAC5AC),//Address_TitleScreen()[8]
            ["Title Screen FG Tileset 2"] = new Pointer(0xAACEDC),//Address_TitleScreen()[9]
        };
    }



    public class FE8E : FE8
    {
        //public FE8E() : base(GameRegion.EUR) { }

        public override GameRegion Region => GameRegion.EUR;

        public override String Identifier => "FE8E";
        public override String ID => "FIREEMBLEM2PBE8P01";
        public override UInt32 Checksum => 0xB3005195;
        public override UInt32 FileSize => 0x02000000;
        public override Magic.Range[] FreeSpace => new Magic.Range[]
        {
            new Magic.Range(0x1147000, 0x1FE0000),
            new Magic.Range(0x1FE3800, 0x1FFFF00),
        };
        public override Dictionary<String, Pointer> Addresses => new Dictionary<String, Pointer>()
        {
            ["Class Array"]               = new Pointer(0x0),//Address_ClassArray()
            ["Chapter Array"]             = new Pointer(0xFACB2C),//Address_ChapterArray()
            ["Map Data Array"]            = new Pointer(0xFAF8D8),//Address_MapDataArray()

            ["Text Array"]                = new Pointer(0x35B8AC),//Address_TextArray()
            ["Huffman Tree"]              = new Pointer(0x356004),//Address_HuffmanTree()
            ["Huffman Tree Root"]         = new Pointer(0x35B8A8),//Address_HuffmanTreeRoot()

            ["Menu Font"]                 = new Pointer(0x798340),//Address_Font_Menu()
            ["Text Bubble Font"]          = new Pointer(0x79B248),//Address_Font_Bubble()
            ["Text Bubble Tileset"]       = new Pointer(0xA8E72C),//Address_TextBubbleTileset()
            ["Text Bubble Palette"]       = new Pointer(0xA8E9C8),//Address_TextBubblePalette()

            ["Music Array"]               = new Pointer(0x42FFB0),//Address_MusicArray()

            ["Portrait Array"]            = new Pointer(0xFA8E60),//Address_PortraitArray()

            ["Map Terrain Names"]         = new Pointer(0x0),//Address_MapTerrainNames()

            ["Map Sprite Idle Array"]     = new Pointer(0xFABB1C),//Address_MapSpriteIdleArray()
            ["Map Sprite Move Array"]     = new Pointer(0xA45B04),//Address_MapSpriteMoveArray()
            ["Map Sprite Palettes"]       = new Pointer(0x7B13E4),//Address_MapSpritePalettes()

            ["Dialog Background Array"]   = new Pointer(0x1059FAC),//Address_DialogBackgroundArray()
            ["Battle Background Array"]   = new Pointer(0x99AFC8),//Address_BattleBackgroundArray()
            ["Cutscene Screen Array"  ]   = new Pointer(0x10BDE10),//Address_CutsceneScreenArray()

            ["Item Icon Tileset"]         = new Pointer(0x0),//Address_ItemIconTileset()
            ["Item Icon Palette"]         = new Pointer(0x0),//Address_ItemIconPalette()

            ["Character Palette Array"]   = new Pointer(0xEF8008),//Address_CharacterPaletteArray()
            ["Battle Animation Array"]    = new Pointer(0xC00008),//Address_BattleAnimationArray()

            ["Spell Animation Array"]     = new Pointer(0x8153CC),//Address_SpellAnimationArray()

            ["Battle Platform Array"]     = new Pointer(0xEE0008),//Address_BattlePlatformArray()

            ["Battle Screen Tileset" ]    = new Pointer(0xA42448),//Address_BattleScreenFrame()[0] // Tileset
            ["Battle Screen Palettes"]    = new Pointer(0xA42D8C),//Address_BattleScreenFrame()[1] // Palette
            ["Battle Screen TSA"     ]    = new Pointer(0xA42940),//Address_BattleScreenFrame()[2] // TSA
            ["Battle Screen L Name"  ]    = new Pointer(0xA427B0),//Address_BattleScreenFrame()[3] // Enemy Name BG
            ["Battle Screen L Weapon"]    = new Pointer(0xA42828),//Address_BattleScreenFrame()[4] // Enemy Weapon BG
            ["Battle Screen R Name"  ]    = new Pointer(0xA42878),//Address_BattleScreenFrame()[5] // Player Name BG
            ["Battle Screen R Weapon"]    = new Pointer(0xA428F0),//Address_BattleScreenFrame()[6] // Player Weapon BG

            ["Mini World Map Tileset" ]   = new Pointer(0x10C005C),//Address_WorldMap()[0] // mini map graphics pointer array
            ["Mini World Map Palette" ]   = new Pointer(0x112F9F0),//Address_WorldMap()[1] // mini map palette
            ["Small World Map Tileset"]   = new Pointer(0x111CB34),//Address_WorldMap()[2] // small map graphics
            ["Small World Map Palette"]   = new Pointer(0x1121EB8),//Address_WorldMap()[3] // small map palette
            ["Small World Map TSA"    ]   = new Pointer(0x1121968),//Address_WorldMap()[4] // small map TSA
            ["Large World Map Tileset"]   = new Pointer(0x110553C),//Address_WorldMap()[5] // large map graphics
            ["Large World Map Palette"]   = new Pointer(0x11181BC),//Address_WorldMap()[6] // large map palette
            ["Large World Map TSA"    ]   = new Pointer(0x111823C),//Address_WorldMap()[7] // large map TSA

            ["Title Screen BG Palette"  ] = new Pointer(0xB1C5EC),//Address_TitleScreen()[0]
            ["Title Screen BG Tileset 1"] = new Pointer(0xB18958),//Address_TitleScreen()[1]
            ["Title Screen BG Tileset 2"] = new Pointer(0xB1A900),//Address_TitleScreen()[2]
            ["Title Screen BG TSA"      ] = new Pointer(0xB1C088),//Address_TitleScreen()[3]
            ["Title Screen MG Palette"  ] = new Pointer(0xB1D784),//Address_TitleScreen()[4]
            ["Title Screen MG Tileset"  ] = new Pointer(0xB1C60C),//Address_TitleScreen()[5]
            ["Title Screen MG TSA"      ] = new Pointer(0xB1D49C),//Address_TitleScreen()[6]
            ["Title Screen FG Palette"  ] = new Pointer(0xB21F04),//Address_TitleScreen()[7]
            ["Title Screen FG Tileset 1"] = new Pointer(0xB1D7A4),//Address_TitleScreen()[8]
            ["Title Screen FG Tileset 2"] = new Pointer(0xB1E0D4),//Address_TitleScreen()[9]
        };
    }
}
