using System;
using System.Text;
using Compression;
using GBA;
using Magic;

namespace KirbyMagic.Kirby
{
    public enum GameType
    {
        Invalid = 0,
        KND,
        KAM,
    }



    public abstract class Game : GBA.Game<GameType>
    {
        /// <summary>
        /// Creates a Kirby Game instance from the given parameters
        /// </summary>
        public static Game FromTypeAndRegion(GameType type, GameRegion region)
        {
            switch (type)
            {
                case GameType.KND: return KND.FromRegion(region);
                case GameType.KAM: return KAM.FromRegion(region);
            }
            return null;
        }

        /// <summary>
        /// This creates a GameType instance by checking the current ROM
        /// </summary>
        public static Game FromROM()
        {
            Byte[] id_data = Core.ReadData(new Pointer(ID_ADDRESS), (Int32)ID_LENGTH);
            String id = new String(Encoding.ASCII.GetChars(id_data));
            
            GameRegion region = GameRegion.Invalid;
            GameType   game   = GameType.Invalid;
            foreach (GameRegion r in Enum.GetValues(typeof(GameRegion)))
            {
                if (r == GameRegion.Invalid) continue;
                if (id.Equals(KND.FromRegion(r).ID)) { game = GameType.KND; region = r; break; }
                if (id.Equals(KAM.FromRegion(r).ID)) { game = GameType.KAM; region = r; break; }
            }
#if DEBUG
            UI.ShowMessage(Util.IntToHex(CRC32.GetChecksum(Core.App.ROM.FileData)));
#endif
            return Game.FromTypeAndRegion(game, region);
        }
    }
}