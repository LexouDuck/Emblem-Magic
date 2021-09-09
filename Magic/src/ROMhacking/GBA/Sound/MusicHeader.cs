using Magic;

namespace GBA
{
    /// <summary>
    /// This reads from a "song header" as it's called - can be a legit song or SFX
    /// </summary>
    public class MusicHeader
    {
        Pointer Address;

        public MusicHeader(Pointer address)
        {
            Address = address;
        }

        /// <summary>
        /// the total amount of tracks for this song
        /// </summary>
        public System.Byte TrackAmount
        {
            get
            {
                return Core.ReadByte(Address);
            }
        }
        /// <summary>
        /// No one knooooows
        /// </summary>
        public System.Byte Unknown
        {
            get
            {
                return Core.ReadByte(Address + 1);
            }
        }
        /// <summary>
        /// Song Priority is handled as follows :
        /// - On PSG channels, the note with the highest priority will play
        /// - On direct sound channels, if there is no free channel anymore, the notes with the highest priority will continue to play
        ///   while the lower priority ones will be ignored or silenced out to make room for high priority notes.
        /// In the case where the priority is equal, the track number comes into play.
        /// Lower tracks numbers always takes the priority on higher track numbers.
        /// </summary>
        public System.Byte Priority
        {
            get
            {
                return Core.ReadByte(Address + 2);
            }
        }
        /// <summary>
        /// Reverberation - Echo feed back (affects all DirectSound channels)
        /// If bit 7 is set, reverb setting is set to this value whenever the song starts to play, overwriting any previous value.
        /// If bit 7 is clear nothing is changed when the song starts to play, and the previously used reverb value is kept.
        /// </summary>
        public System.Byte Reverb
        {
            get
            {
                return Core.ReadByte(Address + 3);
            }
        }
        public Pointer Instruments
        {
            get
            {
                return Core.ReadPointer(Address + 4);
            }
        }
        public Pointer[] Tracks
        {
            get
            {
                Pointer[] result = new Pointer[TrackAmount];
                for (System.Int32 i = 0; i < result.Length; i++)
                {
                    result[i] = Core.ReadPointer(Address + 8 + i * 4);
                }
                return result;
            }
        }
    }
}