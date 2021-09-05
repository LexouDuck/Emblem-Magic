using System;
using System.Collections.Generic;
using System.IO;

namespace Compression
{
    public static class UPS
    {
        public static void WriteFile(string path,
            byte[] oldFile, UInt64 oldFileSize, UInt32 oldChecksum,
            byte[] newFile, UInt64 newFileSize, UInt32 newChecksum)
        {
            List<UInt64> changedOffsets = new List<UInt64>();
            List<byte[]> XOR_bytes = new List<byte[]>();
            UInt64 maxSize = Math.Max(oldFileSize, newFileSize);

            for (UInt64 i = 0; i < maxSize; i++)
            {
                byte x = i < oldFileSize ? oldFile[i] : (byte)0x00;
                byte y = i < newFileSize ? newFile[i] : (byte)0x00;

                if (x != y)
                {
                    changedOffsets.Add(i);
                    List<byte> temp = new List<byte>();
                    while (x != y && i < maxSize)
                    {
                        temp.Add((byte)(x ^ y));
                        i++;
                        x = i < oldFileSize ? oldFile[i] : (byte)0x00;
                        y = i < newFileSize ? newFile[i] : (byte)0x00;
                    }
                    XOR_bytes.Add(temp.ToArray());
                }
            }

            File.WriteAllBytes(path, ToBytes(
                oldFileSize, oldChecksum,
                newFileSize, newChecksum,
                changedOffsets.ToArray(), XOR_bytes.ToArray()));
        }

        static byte[] ToBytes(
            UInt64 oldFileSize, UInt32 oldChecksum,
            UInt64 newFileSize, UInt32 newChecksum,
            UInt64[] changed, byte[][] XOR_bytes)
        {
            List<byte> file = new List<byte>();
            file.Add((byte)'U');
            file.Add((byte)'P');
            file.Add((byte)'S');
            file.Add((byte)'1');
            file.AddRange(VLQ.Encode(oldFileSize));
            file.AddRange(VLQ.Encode(newFileSize));

            for (int i = 0; i < changed.LongLength; i++)
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
