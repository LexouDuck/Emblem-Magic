using System;
using System.Collections.Generic;
using GBA;
using Magic;

namespace EmblemMagic.FireEmblem
{
    public abstract class FE6 : FireEmblem.Game
    {
        //public FE6(GameRegion region) : base(GameType.FE6, region) { }

        public override GameType Type => GameType.FE6;
        public override String Name => "Fire Emblem 6";

        public static Game FromRegion(GameRegion version)
        {
            switch (version)
            {
                case GameRegion.JAP: return new FE6J();
              //case GameRegion.USA: return new FE6U();
              //case GameRegion.EUR: return new FE6E();
            }
            throw new Exception("Invalid game ROM region (" + version + ")");
        }
    }



    public class FE6J : FE6
    {
        //public FE6J() : base(GameRegion.JAP) { }

        public override GameRegion Region => GameRegion.JAP;

        public override String Identifier => "FE6J";
        public override String ID => "FIREEMBLEM6\0AFEJ01";
        public override UInt32 Checksum => 0xD38763E1;
        public override UInt32 FileSize => 0x00800000;
        public override Magic.Range[] FreeSpace => new Magic.Range[]
        {
            new Magic.Range(0x7E9940, 0x7E9FEF),
            new Magic.Range(0x7EA640, 0x7EABEF),
            new Magic.Range(0x7FB980, 0x7FBFEF),
            new Magic.Range(0x7FF0B0, 0x7FFFFF),
        };
        /* FE6U fan translation patch
        {
            new Magic.Range(0x817A00, 0xA00000),
            new Magic.Range(0xA297B0, 0xB00000),
            new Magic.Range(0xB013F0, 0x1000000)
        };
        */
        public override Dictionary<String, Pointer> Addresses => new()
        {
            ["Class Array"]             = new Pointer(0x60A0E8),
            ["Chapter Array"]           = new Pointer(0x6637A4),
            ["Map Data Array"]          = new Pointer(0x664398),

            ["Text Array"]              = new Pointer(0x0F635C),
            ["Huffman Tree"]            = new Pointer(0x0F300C),
            ["Huffman Tree Root"]       = new Pointer(0x0F6358),

            ["Menu Font"]               = new Pointer(0x59027C),
            ["Text Bubble Font"]        = new Pointer(0x5A8204),
            ["Text Bubble Tileset"]     = new Pointer(0x2E4394),
            ["Text Bubble Palette"]     = new Pointer(0x2E4630),

            ["Music Array"]             = new Pointer(0x3994D8),

            ["Portrait Array"]          = new Pointer(0x66074C),

            ["Map Terrain Names"]       = new Pointer(0x0),// TODO

            ["Map Sprite Idle Array"]   = new Pointer(0x662C14),
            ["Map Sprite Move Array"]   = new Pointer(0x6649B4),
            ["Map Sprite Palettes"]     = new Pointer(0x100968),

            ["Dialog Background Array"] = new Pointer(0x5C4484),
            ["Battle Background Array"] = new Pointer(0x607504),
            ["Cutscene Screen Array"]   = new Pointer(0x68A998),

            ["Item Icon Tileset"]       = new Pointer(0x0F9D80),
            ["Item Icon Palette"]       = new Pointer(0x0FED80),

            ["Character Palette Array"] = new Pointer(0x7FC008),
            ["Battle Animation Array"]  = new Pointer(0x6A0008),

            ["Spell Animation Array"]   = new Pointer(0x5D0DA0),

            ["Battle Platform Array"]   = new Pointer(0x7EA008),
            
            ["Battle Screen Tileset"]   = new Pointer(0x1125E0),
            ["Battle Screen Palettes"]  = new Pointer(0x112CD4),
            ["Battle Screen TSA"]       = new Pointer(0x112968),
            ["Battle Screen L Name"]    = new Pointer(0x1127F0),
            ["Battle Screen L Weapon"]  = new Pointer(0x112840),
            ["Battle Screen R Name"]    = new Pointer(0x1128AC),
            ["Battle Screen R Weapon"]  = new Pointer(0x1128FC),

            ["Small World Map Palette"] = new Pointer(0x68C2E0),
            ["Small World Map Tileset"] = new Pointer(0x68C2DC),
            ["Large World Map Palette"] = new Pointer(0x68C2E8),
            ["Large World Map Tileset"] = new Pointer(0x68C2E4),

            ["Title Screen MG/FG Palettes"] = new Pointer(0x35CB90),
            ["Title Screen MG/FG Tileset"]  = new Pointer(0x35CC90),
            ["Title Screen FG Tileset"]     = new Pointer(0x35E6DC),
            ["Title Screen MG TSA"]         = new Pointer(0x35F3A8),
            ["Title Screen BG Tileset"]     = new Pointer(0x35FC3C),
            ["Title Screen BG Palette"]     = new Pointer(0x364418),
        };
    }
}
