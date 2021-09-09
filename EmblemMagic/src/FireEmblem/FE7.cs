using System;
using System.Collections.Generic;
using GBA;
using Magic;

namespace EmblemMagic.FireEmblem
{
    public abstract class FE7 : FireEmblem.Game
    {
        //public FE7(GameRegion region) : base(GameType.FE7, region) { }

        public override GameType Type => GameType.FE7;
        public override String Name => "Fire Emblem 7";

        public static Game FromRegion(GameRegion version)
        {
            switch (version)
            {
                case GameRegion.JAP: return new FE7J();
                case GameRegion.USA: return new FE7U();
                case GameRegion.EUR: return new FE7E();
            }
            throw new Exception("Invalid game ROM region (" + version + ")");
        }
    }



    public class FE7J : FE7
    {
        //public FE7J() : base(GameRegion.JAP) { }

        public override GameRegion Region => GameRegion.JAP;

        public override String Identifier => "FE7J";
        public override String ID => "FIREEMBLEM7\0AE7J01";
        public override UInt32 Checksum => 0xF0C10E72;
        public override UInt32 FileSize => 0x01000000;
        public override Magic.Range[] FreeSpace => new Magic.Range[]
        {
            new Magic.Range(0xDCC200, 0xE00000),
            new Magic.Range(0xFA5100, 0xFC0000),
            new Magic.Range(0xFD2E00, 0xFD8000),
            new Magic.Range(0xFE4000, 0xFFF000),
            new Magic.Range(0xFDC000, 0xFE0000),
        };
        public override Dictionary<String, Pointer> Addresses => new()
        {
            ["Class Array"]             = new Pointer(0x0),//Address_ClassArray()
            ["Chapter Array"]           = new Pointer(0xD62110),//Address_ChapterArray()
            ["Map Data Array"]          = new Pointer(0xD648F4),//Address_MapDataArray()

            ["Text Array"]              = new Pointer(0xBBB370),//Address_TextArray()
            ["Huffman Tree"]            = new Pointer(0xBB72E0),//Address_HuffmanTree()
            ["Huffman Tree Root"]       = new Pointer(0xBBB36C),//Address_HuffmanTreeRoot()

            ["Menu Font"]               = new Pointer(0xBC1FEC),//Address_Font_Menu()
            ["Text Bubble Font"]        = new Pointer(0xBDC134),//Address_Font_Bubble()
            ["Text Bubble Tileset"]     = new Pointer(0x4027B0),//Address_TextBubbleTileset()
            ["Text Bubble Palette"]     = new Pointer(0x402A4C),//Address_TextBubblePalette()

            ["Music Array"]             = new Pointer(0x6EA8D0),//Address_MusicArray()

            ["Portrait Array"]          = new Pointer(0xD5E23C),//Address_PortraitArray()

            ["Map Terrain Names"]       = new Pointer(0x0),//Address_MapTerrainNames()

            ["Map Sprite Idle Array"]   = new Pointer(0xD613B8),//Address_MapSpriteIdleArray()
            ["Map Sprite Move Array"]   = new Pointer(0xD650B4),//Address_MapSpriteMoveArray()
            ["Map Sprite Palettes"]     = new Pointer(0x1900E8),//Address_MapSpritePalettes()

            ["Dialog Background Array"] = new Pointer(0xC00798),//Address_DialogBackgroundArray()
            ["Battle Background Array"] = new Pointer(0xC4BD70),//Address_BattleBackgroundArray()
            ["Cutscene Screen Array"  ] = new Pointer(0xDB793C),//Address_CutsceneScreenArray()

            ["Item Icon Tileset"]       = new Pointer(0x0),//Address_ItemIconTileset()
            ["Item Icon Palette"]       = new Pointer(0x0),//Address_ItemIconPalette()

            ["Character Palette Array"] = new Pointer(0xFD8008),//Address_CharacterPaletteArray()
            ["Battle Animation Array"]  = new Pointer(0xE00008),//Address_BattleAnimationArray()

            ["Spell Animation Array"]   = new Pointer(0xC1071C),//Address_SpellAnimationArray()

            ["Battle Platform Array"]   = new Pointer(0xFC0008),//Address_BattlePlatformArray()

            ["Battle Screen Tileset" ]  = new Pointer(0x1DE528),//Address_BattleScreenFrame()[0]
            ["Battle Screen Palettes"]  = new Pointer(0x1DEC14),//Address_BattleScreenFrame()[1]
            ["Battle Screen TSA"     ]  = new Pointer(0x1DE8A8),//Address_BattleScreenFrame()[2]
            ["Battle Screen L Name"  ]  = new Pointer(0x1DE730),//Address_BattleScreenFrame()[3]
            ["Battle Screen L Weapon"]  = new Pointer(0x1DE780),//Address_BattleScreenFrame()[4]
            ["Battle Screen R Name"  ]  = new Pointer(0x1DE7EC),//Address_BattleScreenFrame()[5]
            ["Battle Screen R Weapon"]  = new Pointer(0x1DE83C),//Address_BattleScreenFrame()[6]

            ["Small World Map Tileset"] = new Pointer(0x60A8EC),//Address_WorldMap()[0]
            ["Small World Map Palette"] = new Pointer(0x60A86C),//Address_WorldMap()[1]
            ["Small World Map TSA"    ] = new Pointer(0x60F964),//Address_WorldMap()[2]
            ["Large World Map Tileset"] = new Pointer(0xDB1074),//Address_WorldMap()[3]
            ["Large World Map Palette"] = new Pointer(0x5AE7BC),//Address_WorldMap()[4]
            ["Large World Map TSA"    ] = new Pointer(0xDB10A4),//Address_WorldMap()[5]

            ["Title Screen BG Palette"] = new Pointer(0x6B73E0),//Address_TitleScreen()[0]
            ["Title Screen BG Tileset"] = new Pointer(0x6B7400),//Address_TitleScreen()[1]
            ["Title Screen MG Palette"] = new Pointer(0x6BB6E8),//Address_TitleScreen()[2]
            ["Title Screen MG Tileset"] = new Pointer(0x6BB708),//Address_TitleScreen()[3]
            ["Title Screen MG TSA"    ] = new Pointer(0x6BBF90),//Address_TitleScreen()[4]
            ["Title Screen FG Palette"] = new Pointer(0x6BC444),//Address_TitleScreen()[5]
            ["Title Screen FG Tileset"] = new Pointer(0x6BC4E4),//Address_TitleScreen()[6]
        };
    }



    public class FE7U : FE7
    {
        //public FE7U() : base(GameRegion.USA) { }

        public override GameRegion Region => GameRegion.USA;

        public override String Identifier => "FE7U";
        public override String ID => "FIREEMBLEME\0AE7E01";
        public override UInt32 Checksum => 0x2A524221;
        public override UInt32 FileSize => 0x01000000;
        public override Magic.Range[] FreeSpace => new Magic.Range[]
        {
            new Magic.Range(0xD00000, 0xD90000),
            new Magic.Range(0xFA5000, 0xFC0000),
            new Magic.Range(0xFD3E00, 0xFD9000),
            new Magic.Range(0xFDC000, 0xFE0000),
            new Magic.Range(0xFE3100, 0xFFD000),
            new Magic.Range(0xFFE000, 0xFFF140),
        };
        public override Dictionary<String, Pointer> Addresses => new()
        {
            ["Class Array"]             = new Pointer(0xBE015C),//Address_ClassArray()
            ["Chapter Array"]           = new Pointer(0xC9A200),//Address_ChapterArray()
            ["Map Data Array"]          = new Pointer(0xC9C9C8),//Address_MapDataArray()

            ["Text Array"]              = new Pointer(0xB808AC),//Address_TextArray()
            ["Huffman Tree"]            = new Pointer(0xB7D71C),//Address_HuffmanTree()
            ["Huffman Tree Root"]       = new Pointer(0xB808A8),//Address_HuffmanTreeRoot()

            ["Menu Font"]               = new Pointer(0xB896B0),//Address_Font_Menu()
            ["Text Bubble Font"]        = new Pointer(0xB8B5B0),//Address_Font_Bubble()
            ["Text Bubble Tileset"]     = new Pointer(0x3FBD34),//Address_TextBubbleTileset()
            ["Text Bubble Palette"]     = new Pointer(0x3FBFD0),//Address_TextBubblePalette()

            ["Music Array"]             = new Pointer(0x69D6E0),//Address_MusicArray()

            ["Portrait Array"]          = new Pointer(0xC96584),//Address_PortraitArray() // Elibian nights 0x1877E6C

            ["Map Terrain Names"]       = new Pointer(0xBE50E8),//Address_MapTerrainNames()

            ["Map Sprite Idle Array"]   = new Pointer(0xC99700),//Address_MapSpriteIdleArray()
            ["Map Sprite Move Array"]   = new Pointer(0xC9D174),//Address_MapSpriteMoveArray()
            ["Map Sprite Palettes"]     = new Pointer(0x194594),//Address_MapSpritePalettes()

            ["Dialog Background Array"] = new Pointer(0xB91588),//Address_DialogBackgroundArray()
            ["Battle Background Array"] = new Pointer(0xBDCA64),//Address_BattleBackgroundArray()
            ["Cutscene Screen Array"  ] = new Pointer(0xCED888),//Address_CutsceneScreenArray()

            ["Item Icon Tileset"]       = new Pointer(0xC5EA4),//Address_ItemIconTileset()
            ["Item Icon Palette"]       = new Pointer(0xCBEA4),//Address_ItemIconPalette()

            ["Character Palette Array"] = new Pointer(0xFD8008),//Address_CharacterPaletteArray()
            ["Battle Animation Array"]  = new Pointer(0xE00008),//Address_BattleAnimationArray()

            ["Spell Animation Array"]   = new Pointer(0xBA13D0),//Address_SpellAnimationArray()

            ["Battle Platform Array"]   = new Pointer(0xFC0008),//Address_BattlePlatformArray()

            ["Battle Screen Tileset" ]  = new Pointer(0x1D88B4),//Address_BattleScreenFrame()[0]
            ["Battle Screen Palettes"]  = new Pointer(0x1D8FA0),//Address_BattleScreenFrame()[1]
            ["Battle Screen TSA"     ]  = new Pointer(0x1D8C34),//Address_BattleScreenFrame()[2]
            ["Battle Screen L Name"  ]  = new Pointer(0x1D8ABC),//Address_BattleScreenFrame()[3]
            ["Battle Screen L Weapon"]  = new Pointer(0x1D8B0C),//Address_BattleScreenFrame()[4]
            ["Battle Screen R Name"  ]  = new Pointer(0x1D8B78),//Address_BattleScreenFrame()[5]
            ["Battle Screen R Weapon"]  = new Pointer(0x1D8BC8),//Address_BattleScreenFrame()[6]

            ["Small World Map Tileset"] = new Pointer(0x5D0AC0),//Address_WorldMap()[0]
            ["Small World Map Palette"] = new Pointer(0x5D0A40),//Address_WorldMap()[1]
            ["Small World Map TSA"    ] = new Pointer(0x5D5B38),//Address_WorldMap()[2]
            ["Large World Map Tileset"] = new Pointer(0xCE7818),//Address_WorldMap()[3]
            ["Large World Map Palette"] = new Pointer(0x574990),//Address_WorldMap()[4]
            ["Large World Map TSA"    ] = new Pointer(0xCE7848),//Address_WorldMap()[5]

            ["Title Screen BG Palette"] = new Pointer(0x66AF6C),//Address_TitleScreen()[0]
            ["Title Screen BG Tileset"] = new Pointer(0x66AF8C),//Address_TitleScreen()[1]
            ["Title Screen MG Palette"] = new Pointer(0x66F274),//Address_TitleScreen()[2]
            ["Title Screen MG Tileset"] = new Pointer(0x66F294),//Address_TitleScreen()[3]
            ["Title Screen MG TSA"    ] = new Pointer(0x66FB1C),//Address_TitleScreen()[4]
            ["Title Screen FG Palette"] = new Pointer(0x66FCE0),//Address_TitleScreen()[5]
            ["Title Screen FG Tileset"] = new Pointer(0x66FD80),//Address_TitleScreen()[6]
        };
    }



    public class FE7E : FE7
    {
        //public FE7E() : base(GameRegion.EUR) { }

        public override GameRegion Region => GameRegion.EUR;

        public override String Identifier => "FE7E";
        public override String ID => "FIREEMBLEMY\0AE7Y01";
        public override UInt32 Checksum => 0x9DECC754;
        public override UInt32 FileSize => 0x01000000;
        public override Magic.Range[] FreeSpace => new Magic.Range[]
        {
            // TODO
        };
        public override Dictionary<String, Pointer> Addresses => new()
        {
            ["Class Array"]             = new Pointer(0x0),//Address_ClassArray()
            ["Chapter Array"]           = new Pointer(0xD612B0),//Address_ChapterArray()
            ["Map Data Array"]          = new Pointer(0xD63A78),//Address_MapDataArray()

            ["Text Array"]              = new Pointer(0xC397A0),//Address_TextArray()
            ["Huffman Tree"]            = new Pointer(0xC34E30),//Address_HuffmanTree()
            ["Huffman Tree Root"]       = new Pointer(0xC3979C),//Address_HuffmanTreeRoot()

            ["Menu Font"]               = new Pointer(0xC4C90C),//Address_Font_Menu()
            ["Text Bubble Font"]        = new Pointer(0xC4F6F4),//Address_Font_Bubble()
            ["Text Bubble Tileset"]     = new Pointer(0x1E4810),//Address_TextBubbleTileset()
            ["Text Bubble Palette"]     = new Pointer(0x1E4AAC),//Address_TextBubblePalette()

            ["Music Array"]             = new Pointer(0x67F200),//Address_MusicArray()

            ["Portrait Array"]          = new Pointer(0xD5D634),//Address_PortraitArray()

            ["Map Terrain Names"]       = new Pointer(0x0),//Address_MapTerrainNames()

            ["Map Sprite Idle Array"]   = new Pointer(0xD607B0),//Address_MapSpriteIdleArray()
            ["Map Sprite Move Array"]   = new Pointer(0xD64224),//Address_MapSpriteMoveArray()
            ["Map Sprite Palettes"]     = new Pointer(0x0D13D0),//Address_MapSpritePalettes()

            ["Dialog Background Array"] = new Pointer(0xC556CC),//Address_DialogBackgroundArray()
            ["Battle Background Array"] = new Pointer(0xCA3B14),//Address_BattleBackgroundArray()
            ["Cutscene Screen Array"  ] = new Pointer(0xDB4CB0),//Address_CutsceneScreenArray()

            ["Item Icon Tileset"]       = new Pointer(0x0),//Address_ItemIconTileset()
            ["Item Icon Palette"]       = new Pointer(0x0),//Address_ItemIconPalette()

            ["Character Palette Array"] = new Pointer(0xFD8008),//Address_CharacterPaletteArray()
            ["Battle Animation Array"]  = new Pointer(0xE00008),//Address_BattleAnimationArray()

            ["Spell Animation Array"]   = new Pointer(0xC68540),//Address_SpellAnimationArray()

            ["Battle Platform Array"]   = new Pointer(0xFC0008),//Address_BattlePlatformArray()

            ["Battle Screen Tileset" ]  = new Pointer(0x2BE6DC),//Address_BattleScreenFrame()[0]
            ["Battle Screen Palettes"]  = new Pointer(0x2BEDC8),//Address_BattleScreenFrame()[1]
            ["Battle Screen TSA"     ]  = new Pointer(0x2BEA5C),//Address_BattleScreenFrame()[2]
            ["Battle Screen L Name"  ]  = new Pointer(0x2BE8E4),//Address_BattleScreenFrame()[3]
            ["Battle Screen L Weapon"]  = new Pointer(0x2BE934),//Address_BattleScreenFrame()[4]
            ["Battle Screen R Name"  ]  = new Pointer(0x2BE9A0),//Address_BattleScreenFrame()[5]
            ["Battle Screen R Weapon"]  = new Pointer(0x2BE9F0),//Address_BattleScreenFrame()[6]

            ["Small World Map Tileset"] = new Pointer(0x64DB88),//Address_WorldMap()[0]
            ["Small World Map Palette"] = new Pointer(0x64DB08),//Address_WorldMap()[1]
            ["Small World Map TSA"    ] = new Pointer(0x652C00),//Address_WorldMap()[2]
            ["Large World Map Tileset"] = new Pointer(0xDAEC40),//Address_WorldMap()[3]
            ["Large World Map Palette"] = new Pointer(0x5F1A58),//Address_WorldMap()[4]
            ["Large World Map TSA"    ] = new Pointer(0xDAEC70),//Address_WorldMap()[5]

            ["Title Screen BG Palette"] = new Pointer(0x1C7424),//Address_TitleScreen()[0]
            ["Title Screen BG Tileset"] = new Pointer(0x1C7444),//Address_TitleScreen()[1]
            ["Title Screen MG Palette"] = new Pointer(0x1CB72C),//Address_TitleScreen()[2]
            ["Title Screen MG Tileset"] = new Pointer(0x1CB74C),//Address_TitleScreen()[3]
            ["Title Screen MG TSA"    ] = new Pointer(0x1CBFD4),//Address_TitleScreen()[4]
            ["Title Screen FG Palette"] = new Pointer(0x1CC198),//Address_TitleScreen()[5]
            ["Title Screen FG Tileset"] = new Pointer(0x1CC238),//Address_TitleScreen()[6]
        };
    }
}
