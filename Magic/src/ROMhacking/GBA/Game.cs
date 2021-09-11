using System;
using System.Collections.Generic;
using System.Text;
using Compression;
using Magic;

namespace GBA
{
    /// <summary>
    /// The child classes of Game store known info about game ROMs in their vanilla state
    /// </summary>
    public abstract class Game<T> : IGame where T : Enum
    {
        /// <summary>
        /// The address/offset in the ROM at which the AGB game ID is located
        /// </summary>
        public const UInt32 ID_ADDRESS = 0x0000A0;
        /// <summary>
        /// The amount of bytes that the game identifier fits into
        /// </summary>
        public const UInt32 ID_LENGTH  = 18;

        
        
        /*
        private readonly T             type;
        private readonly GameRegion    region;
        private readonly String        name;
        private readonly String        identifier;
        private readonly String        id;
        private readonly UInt32        checksum;
        private readonly UInt32        filesize;
        private readonly Magic.Range[] freespace;
        private readonly Dictionary<String, Pointer> addresses;
        */
        public abstract T             Type                    {get;}//=> type;      
        public abstract GameRegion    Region                  {get;}//=> region;    
        public abstract String        Name                    {get;}//=> name;      
        public abstract String        Identifier              {get;}//=> identifier;
        public abstract String        ID                      {get;}//=> id;        
        public abstract UInt32        Checksum                {get;}//=> checksum;  
        public abstract UInt32        FileSize                {get;}//=> filesize;  
        public abstract Magic.Range[] FreeSpace               {get;}//=> freespace; 
        public abstract Dictionary<String, Pointer> Addresses {get;}//=> addresses; 

        /*
        public static Game<T> FromROM(DataManager ROM)
        {
            Byte[] id_data = Core.ReadData(new Pointer(ID_ADDRESS), 18);
            String id = new String(Encoding.ASCII.GetChars(id_data));
            GameRegion game_region;
            T game_type;
            GetTypeAndRegion(id, out game_type, out game_region);
            return new Game<T>();
        }
        */
        public static void GetTypeAndRegion(String id, out T game_type, out GameRegion game_region)
        {
            foreach (GameRegion region in Enum.GetValues(typeof(GameRegion)))
            {
                foreach (T type in Enum.GetValues(typeof(T)))
                {
                    if (Game<T>.GetGameID(type, region).Equals(id))
                    {
                        game_type = type;
                        game_region = region;
                        return;
                    }
                }
            }
            throw new Exception("The Kirby game ID could not be identified.");
        }

        public static String GetGameID(T type, GameRegion region)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// Returns an array of 'Repoint's describing the addresses of different core assets and arrays for the game
        /// </summary>
        public Repoint[] GetDefaultPointers()
        {
            List<Repoint> result = new();
            foreach (KeyValuePair<String, Pointer> item in this.Addresses)
            {
                result.Add(new Repoint(item.Key, item.Value));
            }
            return result.ToArray();
        }



        override public String ToString()
        {
            return (this.Name + " (" + this.Region + ")");
        }
        override public Boolean Equals(Object other)
        {
            if (!(other is Game<T>)) return false;
            Game<T> game = (Game<T>)other;
            return (Region == game.Region)
                && (this.GetType() == game.GetType());
        }
        override public Int32 GetHashCode()
        {
            return (this.GetType().GetHashCode() ^ Identifier.GetHashCode() ^ Checksum.GetHashCode());
        }
    }
}