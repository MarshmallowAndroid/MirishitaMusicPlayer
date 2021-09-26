using CriUtf.BinaryReaderExtensions;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace CriUtf
{
    [Flags]
    public enum ColumnFlag : byte
    {
        Name = 0x10,
        Default = 0x20,
        Row = 0x40
    }

    public enum ColumnType : byte
    {
        Byte = 0x00,
        SByte = 0x01,
        UInt16 = 0x02,
        Int16 = 0x03,
        UInt32 = 0x04,
        Int32 = 0x05,
        UInt64 = 0x06,
        Int64 = 0x07,
        Float = 0x08,
        Double = 0x09,
        String = 0x0a,
        VLData = 0x0b,
        UInt128 = 0x0c,
        Undefined = 0xFF
    }

    public class UtfTableColumn
    {
        public ColumnFlag Flag { get; internal set; }
        public ColumnType Type { get; internal set; }
        public string Name { get; internal set; }
        public uint Offset { get; internal set; }
    }

    public class ValueData
    {
        public uint Offset { get; set; }
        public uint Length { get; set; }

        public override string ToString()
        {
            return $"Offset: {Offset}, Length: {Length}";
        }
    }

    public class UtfTableQueryResult
    {
        public bool Found { get; internal set; }
        public ColumnType Type { get; internal set; }
        public object Value { get; internal set; }
    }

    /*
     * Crappy C# port from the vgmstream project
     */
    public class UtfTable
    {
        private readonly BinaryReader binaryReader;

        private readonly uint tableOffset;
        private readonly uint tableSize;
        private readonly ushort version;
        private readonly ushort rowsOffset;
        private readonly uint stringsOffset;
        private readonly uint dataOffset;
        private readonly uint nameOffset;
        //private readonly ushort columns;
        private readonly ushort rowWidth;
        //private readonly uint Rows;
        private readonly uint stringsSize;

        private readonly uint schemaOffset = 0x20;
        //UtfTableColumn[] Schema;

        private readonly byte[] stringTable;

        public UtfTable(Stream utfTableStream)
        {
            binaryReader = new BinaryReader(utfTableStream);

            tableOffset = (uint)binaryReader.BaseStream.Position;

            if (!binaryReader.ReadChars(4).SequenceEqual("@UTF".ToCharArray()))
                throw new InvalidDataException("Invalid magic.");

            tableSize = binaryReader.ReadUInt32BE() + 0x08;
            version = binaryReader.ReadUInt16BE();
            rowsOffset = (ushort)(binaryReader.ReadUInt16BE() + 0x08);
            stringsOffset = binaryReader.ReadUInt32BE() + 0x08;
            dataOffset = binaryReader.ReadUInt32BE() + 0x08;
            nameOffset = binaryReader.ReadUInt32BE();
            Columns = binaryReader.ReadUInt16BE();
            rowWidth = binaryReader.ReadUInt16BE();
            Rows = binaryReader.ReadUInt32BE();

            stringsSize = dataOffset - stringsOffset;

            if (tableOffset + tableSize > utfTableStream.Length)
                throw new InvalidDataException("Table size exceeds bounds of file.");
            if (rowsOffset > tableSize || stringsOffset > tableSize || dataOffset > tableSize)
                throw new InvalidDataException("Offsets out of bounds.");
            if (stringsSize <= 0 || nameOffset > stringsSize)
                throw new InvalidDataException("Invalid string table size.");
            if (Columns <= 0)
                throw new InvalidDataException("Table has no columns.");

            binaryReader.BaseStream.Position = tableOffset + stringsOffset;

            stringTable = new byte[stringsSize];
            if (binaryReader.Read(stringTable, 0, (int)stringsSize) != stringsSize)
                throw new InvalidDataException("Failed to read string table.");

            binaryReader.BaseStream.Position = tableOffset + this.schemaOffset;

            uint valueSize = 0;
            uint columnOffset = 0;
            uint schemaOffset = tableOffset + this.schemaOffset;

            Schema = new UtfTableColumn[Columns];

            for (int i = 0; i < Columns; i++)
            {
                binaryReader.BaseStream.Position = schemaOffset;

                byte info = binaryReader.ReadByte();
                uint nameOffset = binaryReader.ReadUInt32BE();

                schemaOffset += 0x1 + 0x4;

                Schema[i] = new UtfTableColumn()
                {
                    Flag = (ColumnFlag)(info & 0xF0),
                    Type = (ColumnType)(info & 0x0F),
                    Name = "",
                    Offset = 0
                };

                switch (Schema[i].Type)
                {
                    case ColumnType.Byte:
                    case ColumnType.SByte:
                        valueSize = 0x01;
                        break;
                    case ColumnType.UInt16:
                    case ColumnType.Int16:
                        valueSize = 0x02;
                        break;
                    case ColumnType.UInt32:
                    case ColumnType.Int32:
                    case ColumnType.Float:
                    case ColumnType.String:
                        valueSize = 0x04;
                        break;
                    case ColumnType.UInt64:
                    case ColumnType.Int64:
                    case ColumnType.VLData:
                        valueSize = 0x08;
                        break;
                    default:
                        throw new InvalidDataException("Unknown column type.");
                }

                if (Schema[i].Flag.HasFlag(ColumnFlag.Name))
                {
                    Schema[i].Name = GetStringFromTable(nameOffset);
                }

                if (Schema[i].Flag.HasFlag(ColumnFlag.Default))
                {
                    Schema[i].Offset = schemaOffset - (tableOffset + this.schemaOffset);
                    schemaOffset += valueSize;
                }

                if (Schema[i].Flag.HasFlag(ColumnFlag.Row))
                {
                    Schema[i].Offset = columnOffset;
                    columnOffset += valueSize;
                }
            }

            Name = GetStringFromTable(nameOffset);
        }

        public ushort Columns { get; }

        public uint Rows { get; }

        public UtfTableColumn[] Schema { get; }

        public string Name { get; }

        public UtfTable OpenSubtable(string tableName)
        {
            if (!QueryData(tableName, out ValueData tableValueData))
                throw new ArgumentException("Subtable does not exist.");
            binaryReader.BaseStream.Position = tableValueData.Offset;
            return new UtfTable(binaryReader.BaseStream);
        }

        public UtfTableQueryResult Query(string columnName, int row)
        {
            UtfTableQueryResult result = new UtfTableQueryResult
            {
                Found = false
            };

            for (int i = 0; i < Columns; i++)
            {
                UtfTableColumn column = Schema[i];
                uint dataOffset;

                if (column.Name == string.Empty || !column.Name.Equals(columnName)) continue;

                result.Found = true;
                result.Type = column.Type;

                if (column.Flag.HasFlag(ColumnFlag.Default))
                    dataOffset = tableOffset + schemaOffset + column.Offset;
                else if (column.Flag.HasFlag(ColumnFlag.Row))
                    dataOffset = (uint)(tableOffset + rowsOffset + row * rowWidth + column.Offset);
                else
                    dataOffset = 0;

                if (dataOffset == 0)
                {
                    result.Value = null;
                    break;
                }

                binaryReader.BaseStream.Position = dataOffset;

                switch (column.Type)
                {
                    case ColumnType.Byte:
                        result.Value = binaryReader.ReadByte();
                        break;
                    case ColumnType.SByte:
                        result.Value = binaryReader.ReadSByte();
                        break;
                    case ColumnType.UInt16:
                        result.Value = binaryReader.ReadUInt16BE();
                        break;
                    case ColumnType.Int16:
                        result.Value = binaryReader.ReadInt16BE();
                        break;
                    case ColumnType.UInt32:
                        result.Value = binaryReader.ReadUInt32BE();
                        break;
                    case ColumnType.Int32:
                        result.Value = binaryReader.ReadInt32BE();
                        break;
                    case ColumnType.UInt64:
                        result.Value = binaryReader.ReadUInt64BE();
                        break;
                    case ColumnType.Int64:
                        result.Value = binaryReader.ReadInt64BE();
                        break;
                    case ColumnType.Float:
                        result.Value = binaryReader.ReadSingleBE();
                        break;
                    case ColumnType.String:
                        uint stringOffset = binaryReader.ReadUInt32BE();
                        if (stringOffset > stringsSize)
                            throw new InvalidDataException("Invalid string offset.");

                        result.Value = GetStringFromTable(stringOffset);
                        break;
                    case ColumnType.VLData:
                        result.Value = new ValueData()
                        {
                            Offset = binaryReader.ReadUInt32BE(),
                            Length = binaryReader.ReadUInt32BE()
                        };
                        break;
                    default:
                        throw new InvalidDataException("Unknown column type.");
                }

                break;
            }

            return result;
        }

        public bool QueryValue(string columnName, int row, ColumnType type, out object value)
        {
            UtfTableQueryResult result = Query(columnName, row);

            switch (result.Type)
            {
                case ColumnType.Byte:
                    value = (byte)0;
                    break;
                case ColumnType.SByte:
                    value = (sbyte)0;
                    break;
                case ColumnType.UInt16:
                    value = (ushort)0;
                    break;
                case ColumnType.Int16:
                    value = (short)0;
                    break;
                case ColumnType.UInt32:
                    value = (uint)0;
                    break;
                case ColumnType.Int32:
                    value = (int)0;
                    break;
                case ColumnType.UInt64:
                    value = (ulong)0;
                    break;
                case ColumnType.Int64:
                    value = (long)0;
                    break;
                case ColumnType.Float:
                    value = (float)0;
                    break;
                case ColumnType.String:
                    value = "";
                    break;
                case ColumnType.VLData:
                    value = new ValueData()
                    {
                        Offset = 0,
                        Length = 0
                    };
                    break;
                default:
                    throw new InvalidDataException("Unknown column type.");
            }

            if (result.Value != null) value = result.Value;

            if (result == null || !result.Found || result.Type != type)
                return false;

            return true;
        }

        public bool QuerySByte(string columnName, out sbyte value, int row = 0)
        {
            bool hasColumn = QueryValue(columnName, row, ColumnType.SByte, out object objectValue);
            if (hasColumn) value = (sbyte)objectValue;
            else value = 0;
            return hasColumn;
        }

        public bool QueryByte(string columnName, out byte value, int row = 0)
        {
            bool hasColumn = QueryValue(columnName, row, ColumnType.Byte, out object objectValue);
            if (hasColumn) value = (byte)objectValue;
            else value = 0;
            return hasColumn;
        }

        public bool QueryInt16(string columnName, out short value, int row = 0)
        {
            bool hasColumn = QueryValue(columnName, row, ColumnType.Int16, out object objectValue);
            if (hasColumn) value = (short)objectValue;
            else value = 0;
            return hasColumn;
        }

        public bool QueryUInt16(string columnName, out ushort value, int row = 0)
        {
            bool hasColumn = QueryValue(columnName, row, ColumnType.UInt16, out object objectValue);
            if (hasColumn) value = (ushort)objectValue;
            else value = 0;
            return hasColumn;
        }

        public bool QueryInt32(string columnName, out int value, int row = 0)
        {
            bool hasColumn = QueryValue(columnName, row, ColumnType.Int32, out object objectValue);
            if (hasColumn) value = (int)objectValue;
            else value = 0;
            return hasColumn;
        }

        public bool QueryUInt32(string columnName, out uint value, int row = 0)
        {
            bool hasColumn = QueryValue(columnName, row, ColumnType.UInt32, out object objectValue);
            if (hasColumn) value = (uint)objectValue;
            else value = 0;
            return hasColumn;
        }

        public bool QueryInt64(string columnName, out long value, int row = 0)
        {
            bool hasColumn = QueryValue(columnName, row, ColumnType.Int64, out object objectValue);
            if (hasColumn) value = (long)objectValue;
            else value = 0;
            return hasColumn;
        }

        public bool QueryUInt64(string columnName, out ulong value, int row = 0)
        {
            bool hasColumn = QueryValue(columnName, row, ColumnType.UInt64, out object objectValue);
            if (hasColumn) value = (ulong)objectValue;
            else value = 0;
            return hasColumn;
        }

        public bool QueryString(string columnName, out string value, int row = 0)
        {
            bool hasColumn = QueryValue(columnName, row, ColumnType.String, out object objectValue);
            if (hasColumn) value = (string)objectValue;
            else value = string.Empty;
            return hasColumn;
        }


        public bool QueryData(string columnName, out ValueData value, int row = 0)
        {
            bool hasColumn = QueryValue(columnName, row, ColumnType.VLData, out object objectValue);
            if (hasColumn)
            {
                value = (ValueData)objectValue;
                value.Offset += tableOffset + dataOffset;
            }
            else value = null;
            return hasColumn;
        }

        public byte[] GetData(long offset, uint length)
        {
            long restore = binaryReader.BaseStream.Position;
            binaryReader.BaseStream.Position = offset;
            byte[] data = binaryReader.ReadBytes((int)length);
            binaryReader.BaseStream.Position = restore;
            return data;
        }

        //public ushort ReadUInt16BE(long offset)
        //{
        //    long restore = binaryReader.BaseStream.Position;
        //    binaryReader.BaseStream.Position = offset;
        //    ushort value = binaryReader.ReadUInt16BE();
        //    binaryReader.BaseStream.Position = restore;
        //    return value;
        //}

        private string GetStringFromTable(uint offset)
        {
            if (offset > stringsSize)
                throw new InvalidDataException("Invalid string offset.");

            StringBuilder stringBuilder = new StringBuilder();

            int i = 0;
            while (i < stringsSize)
            {
                stringBuilder.Append((char)stringTable[offset + i++]);
                if (stringTable[offset + i] == '\0') break;
            }

            return stringBuilder.ToString();
        }
    }
}
