using System;
using System.Collections.Generic;

namespace GBA
{
    public class OAM_Array
    {
        public OAM this[Int32 index]
        {
            get
            {
                return Sprites[index];
            }
            set
            {
                Sprites[index] = value;
            }
        }

        /// <summary>
        /// The actual OAM array that this class is a wrapper for.
        /// </summary>
        public List<OAM> Sprites;
        /// <summary>
        /// The transformation data in this OAM array, for any OAM entries that are affine sprites
        /// </summary>
        public List<OAM_Affine> Affines;



        /// <summary>
        /// Creates an empty OAM_Array
        /// </summary>
        public OAM_Array()
        {
            Sprites = new List<OAM>();
            Affines = new List<OAM_Affine>();
        }
        /// <summary>
        /// Creates an OAM_Array from the given array
        /// </summary>
        public OAM_Array(List<OAM> array)
        {
            Sprites = array;
            Affines = new List<OAM_Affine>();
        }
        /// <summary>
        /// Reads OAM data starting at 'offset', stopping at the first terminator encountered
        /// </summary>
        public OAM_Array(Byte[] data, UInt32 offset)
        {
            Sprites = new List<OAM>();
            Affines = new List<OAM_Affine>();

            Byte[] buffer = new Byte[12];
            Boolean loadAffineData = true;
            for (UInt32 i = offset; i < data.Length; i += 12)
            {
                if (data[i] == 0x00)
                {
                    loadAffineData = false;
                    Array.Copy(data, i, buffer, 0, 12);
                    Sprites.Add(new OAM(buffer));
                }
                else
                {
                    for (Int32 j = 1; j < 12; j++)
                    {
                        if (data[i + j] != 0x00)
                        {
                            if (loadAffineData)
                            {
                                Array.Copy(data, i, buffer, 0, 12);
                                Affines.Add(new OAM_Affine(buffer));
                                goto Continue;
                            }
                            else throw new Exception("Invalid terminator read in OAM.");
                        }
                    }
                    break;
                }
                Continue: continue;
            }
        }



        /// <summary>
        /// Returns a byte array of this OAM array
        /// </summary>
        public Byte[] ToBytes()
        {
            Byte[] result = new Byte[Affines.Count * OAM.LENGTH + Sprites.Count * OAM.LENGTH + OAM.LENGTH];
            Int32 index = 0;
            for (Int32 i = 0; i < Affines.Count; i++)
            {
                Array.Copy(Affines[i].ToBytes(), 0, result, index, OAM.LENGTH);
                index += OAM.LENGTH;
            }
            for (Int32 i = 0; i < Sprites.Count; i++)
            {
                Array.Copy(Sprites[i].ToBytes(), 0, result, index, OAM.LENGTH);
                index += OAM.LENGTH;
            }
            result[result.Length - OAM.LENGTH] = 0x01; // make terminator
            return result;
        }
        /// <summary>
        /// Merges the different OAM arrays given into one big byte array, each array separated by a terminator
        /// </summary>
        public static Byte[] Merge(List<OAM_Array> frameOAMs)
        {
            List<Byte> result = new List<Byte>();
            foreach (OAM_Array oam in frameOAMs)
            {
                result.AddRange(oam.ToBytes());
            }
            return result.ToArray();
        }
    }
}
