using System;
using System.Collections.Generic;
using GBA;
using Magic;

namespace KirbyMagic.Kirby
{
    public abstract class KND : Kirby.Game
    {
        //public KND(GameRegion region) : base(GameType.KND, region) { }

        public override GameType Type => GameType.KND;
        public override String Name => "Kirby: Nightmare in Dreamland";

        public static Game FromRegion(GameRegion version)
        {
            switch (version)
            {
                case GameRegion.JAP: return new KNDJ();
                case GameRegion.USA: return new KNDU();
                case GameRegion.EUR: return new KNDE();
            }
            throw new Exception("Invalid game ROM region (" + version + ")");
        }
    }


    
    public class KNDJ : KND
    {
        public override GameRegion Region => GameRegion.JAP;

        public override String Identifier => "KNDJ";
        public override String ID => "AGB KIRBY DXA7KJ01";
        public override UInt32 Checksum => 0x7FE05A37;
        public override UInt32 FileSize => 0x00800000;
        public override Magic.Range[] FreeSpace => new Magic.Range[]
        {
            // TODO
        };
        public override Dictionary<String, Pointer> Addresses => new()
        {
            ["Text Array"]              = new Pointer(0x0),
            ["Menu Font"]               = new Pointer(0x0),

            ["Music Array"]             = new Pointer(0x0),

            ["Enemy Array"]             = new Pointer(0x0),

            ["Level Background Array"]  = new Pointer(0x0),
            ["Level Map Data Array"]    = new Pointer(0x0),

            ["World Background Array"]  = new Pointer(0x0),
            ["World Map Data Array"]    = new Pointer(0x0),

            ["Cutscene Screen Array"]   = new Pointer(0x0),

            ["Item Icon Tileset"]       = new Pointer(0x0),
            ["Item Icon Palette"]       = new Pointer(0x0),

            ["Title Screen FG Palette"] = new Pointer(0x0),
            ["Title Screen FG Tileset"] = new Pointer(0x0),
            ["Title Screen FG TSA"]     = new Pointer(0x0),
            ["Title Screen BG Palette"] = new Pointer(0x0),
            ["Title Screen BG Tileset"] = new Pointer(0x0),
            ["Title Screen BG TSA"]     = new Pointer(0x0),
        };
    }



    public class KNDU : KND
    {
        public override GameRegion Region => GameRegion.USA;

        public override String Identifier => "KNDU";
        public override String ID => "AGB KIRBY DXA7KE01";
        public override UInt32 Checksum => 0x20EF3F64;
        public override UInt32 FileSize => 0x00800000;
        public override Magic.Range[] FreeSpace => new Magic.Range[]
        {
            // TODO
        };
        public override Dictionary<String, Pointer> Addresses => new()
        {
            ["Text Array"]              = new Pointer(0x0),
            ["Menu Font"]               = new Pointer(0x0),

            ["Music Array"]             = new Pointer(0x0),

            ["Enemy Array"]             = new Pointer(0x0),

            ["Level Background Array"]  = new Pointer(0x0),
            ["Level Map Data Array"]    = new Pointer(0x0),

            ["World Background Array"]  = new Pointer(0x0),
            ["World Map Data Array"]    = new Pointer(0x0),

            ["Cutscene Screen Array"]   = new Pointer(0x0),

            ["Item Icon Tileset"]       = new Pointer(0x0),
            ["Item Icon Palette"]       = new Pointer(0x0),

            ["Title Screen FG Palette"] = new Pointer(0x0),
            ["Title Screen FG Tileset"] = new Pointer(0x0),
            ["Title Screen FG TSA"]     = new Pointer(0x0),
            ["Title Screen BG Palette"] = new Pointer(0x0),
            ["Title Screen BG Tileset"] = new Pointer(0x0),
            ["Title Screen BG TSA"]     = new Pointer(0x0),
        };
    }



    public class KNDE : KND
    {
        public override GameRegion Region => GameRegion.EUR;

        public override String Identifier => "KNDE";
        public override String ID => "AGB KIRBY DXA7KP01";
        public override UInt32 Checksum => 0x3B7A7477;
        public override UInt32 FileSize => 0x01000000;
        public override Magic.Range[] FreeSpace => new Magic.Range[]
        {
            // TODO
        };
        public override Dictionary<String, Pointer> Addresses => new()
        {
            ["Text Array"]              = new Pointer(0x0),
            ["Menu Font"]               = new Pointer(0x0),

            ["Music Array"]             = new Pointer(0x0),

            ["Enemy Array"]             = new Pointer(0x0),

            ["Level Background Array"]  = new Pointer(0x0),
            ["Level Map Data Array"]    = new Pointer(0x0),

            ["World Background Array"]  = new Pointer(0x0),
            ["World Map Data Array"]    = new Pointer(0x0),

            ["Cutscene Screen Array"]   = new Pointer(0x0),

            ["Item Icon Tileset"]       = new Pointer(0x0),
            ["Item Icon Palette"]       = new Pointer(0x0),

            ["Title Screen FG Palette"] = new Pointer(0x0),
            ["Title Screen FG Tileset"] = new Pointer(0x0),
            ["Title Screen FG TSA"]     = new Pointer(0x0),
            ["Title Screen BG Palette"] = new Pointer(0x0),
            ["Title Screen BG Tileset"] = new Pointer(0x0),
            ["Title Screen BG TSA"]     = new Pointer(0x0),
        };
    }
}
