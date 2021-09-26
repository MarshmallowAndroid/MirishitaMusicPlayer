using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CriUtf.BinaryReaderExtensions
{
    public static class BinaryReaderExtensions
    {
        public static short ReadInt16BE(this BinaryReader binaryReader)
        {
            ushort le = binaryReader.ReadUInt16();
            return (short)ReverseEndianness(le);
        }

        public static ushort ReadUInt16BE(this BinaryReader binaryReader)
        {
            ushort le = binaryReader.ReadUInt16();
            return ReverseEndianness(le);
        }

        public static int ReadInt32BE(this BinaryReader binaryReader)
        {
            uint le = binaryReader.ReadUInt32();
            return (int)ReverseEndianness(le);
        }

        public static uint ReadUInt32BE(this BinaryReader binaryReader)
        {
            uint le = binaryReader.ReadUInt32();
            return ReverseEndianness(le);
        }
        public static long ReadInt64BE(this BinaryReader binaryReader)
        {
            ulong le = binaryReader.ReadUInt64();
            return (long)ReverseEndianness(le);
        }

        public static ulong ReadUInt64BE(this BinaryReader binaryReader)
        {
            ulong le = binaryReader.ReadUInt64();
            return ReverseEndianness(le);
        }

        public static float ReadSingleBE(this BinaryReader binaryReader)
        {
            float le = binaryReader.ReadSingle();
            byte[] floatBytes = BitConverter.GetBytes(le);
            byte[] reversed = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                reversed[i] = floatBytes[3 - i];
            }
            return BitConverter.ToSingle(reversed, 0);
        }

        private static ushort ReverseEndianness(ushort value)
        {
            return (ushort)((value << 8) | (value >> 8));
        }

        private static uint ReverseEndianness(uint value)
        {
            value = ((value & 0x00FF00FFu) << 8) | ((value & 0xFF00FF00u) >> 8);
            return (value & 0x0000FFFFu) << 16 | ((value & 0xFFFF0000u) >> 16);
        }

        private static ulong ReverseEndianness(ulong value)
        {
            value = ((value & 0x00FF00FF00FF00FFu) << 8) | ((value & 0xFF00FF00FF00FF00u) >> 8);
            value = ((value & 0x0000FFFF0000FFFFu) << 16) | ((value & 0xFFFF0000FFFF0000u) >> 16);
            return value << 32 | value >> 32;
        }
    }
}
