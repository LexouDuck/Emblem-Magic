using System;
using System.Collections.Generic;
using GBA;
using Magic;

namespace KirbyMagic.Kirby
{
    public abstract class KAM : Kirby.Game
    {
        //public KAM(GameRegion region) : base(GameType.KAM, region) { }

        public override GameType Type => GameType.KAM;
        public override String Name => "Kirby and the Amazing Mirror";

        public static Game FromRegion(GameRegion version)
        {
            switch (version)
            {
                case GameRegion.JAP: return new KAMJ();
                case GameRegion.USA: return new KAMU();
                case GameRegion.EUR: return new KAME();
            }
            throw new Exception("Invalid game ROM region (" + version + ")");
        }
    }


    
    public class KAMJ : KAM
    {
        public override GameRegion Region => GameRegion.JAP;

        public override String Identifier => "KAMJ";
        public override String ID => "AGB KIRBY AMB8KJ01";
        public override UInt32 Checksum => 0x683D772C;
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

    public class KAMU : KAM
    {
        public override GameRegion Region => GameRegion.USA;

        public override String Identifier => "KAMU";
        public override String ID => "AGB KIRBY AMB8KE01";
        public override UInt32 Checksum => 0x9F2A3048;
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

    public class KAME : KAM
    {
        public override GameRegion Region => GameRegion.EUR;

        public override String Identifier => "KAME";
        public override String ID => "AGB KIRBY AMB8KP01";
        public override UInt32 Checksum => 0x4F07C618;
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
