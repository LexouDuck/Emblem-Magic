using GBA;
using Magic;

namespace GBA
{
    /// <summary>
    /// This represents the data for a sound sample to be used by an instrument
    /// </summary>
    public struct Sample
    {
        public Sample(Pointer address)
        {
            Data = Core.ReadData(address, 16);
            Data = Core.ReadData(address, 16 + (System.Int32)PCM_Size);
        }

        System.Byte[] Data;

        public System.Byte[] ToBytes()
        {
            return Data;
        }


        /// <summary>
        /// Whether or not this sample is played on loop
        /// </summary>
        public System.Boolean Looped
        {
            get
            {
                return (Data[3] == 0x40);
            }
        }
        /// <summary>
        /// The pitch adjustment for this sound sample. Pitch = 1024 * SampleRate
        /// </summary>
        public System.UInt32 Pitch
        {
            get
            {
                return Data.GetUInt32(4, true);
            }
        }
        /// <summary>
        /// The relative loop starting point for this sample
        /// </summary>
        public System.UInt32 LoopStart
        {
            get
            {
                return Data.GetUInt32(8, true);
            }
        }
        /// <summary>
        /// The size of this sound sample
        /// </summary>
        public System.UInt32 PCM_Size
        {
            get
            {
                return Data.GetUInt32(12, true);
            }
        }
        /// <summary>
        /// The actual sample wave, in signed 8-bit standard Pulse Code Modulation data
        /// </summary>
        public System.Byte[] PCM_Data
        {
            get
            {
                return Data.GetBytes(16);
            }
        }
    }
}
