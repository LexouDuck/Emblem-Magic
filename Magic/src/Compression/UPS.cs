using System;
using System.Collections.Generic;
using System.IO;

namespace Compression
{
    public static class UPS
    {
        public static void WriteFile(String path,
            Byte[] oldFile, UInt64 oldFileSize, UInt32 oldChecksum,
            Byte[] newFile, UInt64 newFileSize, UInt32 newChecksum)
        {
            List<UInt64> changedOffsets = new List<UInt64>();
            List<Byte[]> XOR_bytes = new List<Byte[]>();
            UInt64 maxSize = Math.Max(oldFileSize, newFileSize);

            for (UInt64 i = 0; i < maxSize; i++)
            {
                Byte x = i < oldFileSize ? oldFile[i] : (Byte)0x00;
                Byte y = i < newFileSize ? newFile[i] : (Byte)0x00;

                if (x != y)
                {
                    changedOffsets.Add(i);
                    List<Byte> temp = new List<Byte>();
                    while (x != y && i < maxSize)
                    {
                        temp.Add((Byte)(x ^ y));
                        i++;
                        x = i < oldFileSize ? oldFile[i] : (Byte)0x00;
                        y = i < newFileSize ? newFile[i] : (Byte)0x00;
                    }
                    XOR_bytes.Add(temp.ToArray());
                }
            }

            File.WriteAllBytes(path, ToBytes(
                oldFileSize, oldChecksum,
                newFileSize, newChecksum,
                changedOffsets.ToArray(), XOR_bytes.ToArray()));
        }

        static Byte[] ToBytes(
            UInt64 oldFileSize, UInt32 oldChecksum,
            UInt64 newFileSize, UInt32 newChecksum,
            UInt64[] changed, Byte[][] XOR_bytes)
        {
            List<Byte> file = new List<Byte>();
            file.Add((Byte)'U');
            file.Add((Byte)'P');
            file.Add((Byte)'S');
            file.Add((Byte)'1');
            file.AddRange(VLQ.Encode(oldFileSize));
            file.AddRange(VLQ.Encode(newFileSize));

            for (Int32 i = 0; i < changed.LongLength; i++)
            {
                UInt64 relativeOffset = changed[i];
                if (i != 0)
                {
                    relativeOffset -= changed[i - 1] + (UInt64)XOR_bytes[i - 1].Length + 1;
                }
                file.AddRange(VLQ.Encode(relativeOffset));
                file.AddRange(XOR_bytes[i]);
                file.Add(0x00);
            }

            file.AddRange(BitConverter.GetBytes(oldChecksum));
            file.AddRange(BitConverter.GetBytes(newChecksum));
            file.AddRange(BitConverter.GetBytes(CRC32.GetChecksum(file.ToArray())));
            return file.ToArray();
        }
    }
}
