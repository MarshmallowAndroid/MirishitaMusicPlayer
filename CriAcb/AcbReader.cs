using CriUtf;
using CriAwb;
using System;
using System.Collections.Generic;
using System.IO;

namespace CriAcb
{
    public class AcbReader
    {
        private readonly Stream innerStream;
        private readonly uint awbOffset;
        private readonly uint awbLength;

        public AcbReader(Stream acbStream)
        {
            innerStream = acbStream;
            acbStream.Position = 0;

            UtfTable utfTable = new UtfTable(acbStream);
            if (utfTable.Name != "Header")
                throw new InvalidDataException("No Header table.");
            
            if (!utfTable.QueryData("AwbFile", out ValueData awbValueData))
                throw new InvalidDataException("No embedded AWB file.");

            awbOffset = awbValueData.Offset;
            awbLength = awbValueData.Length;
        }

        public AwbReader GetAwb()
        {
            return new AwbReader(new IsolatedStream(innerStream, awbOffset, awbLength), true);
        }

        private static UtfTable header;

        private static UtfTable cueNameTable;
        private static UtfTable cueTable;
        private static UtfTable sequenceTable;
        private static UtfTable trackTable;
        private static UtfTable trackCommandTable;
        private static UtfTable synthTable;
        private static UtfTable waveformTable;

        private static bool hasCommandTable;
        private static bool hasTrackEventTable;

        private static int targetWaveID;
        private static bool isEmbedded;

        private static string cueNameName;
        private static int cueNameIndex;

        private static int sequenceDepth;
        private static int synthDepth;

        private static List<string> names;

        private static bool LoadWaveform(int index)
        {
            waveformTable = header.OpenSubtable("WaveformTable");

            if (!waveformTable.QueryUInt16("Id", out ushort id, index))
            {
                if (isEmbedded)
                {
                    if (!waveformTable.QueryUInt16("MemoryAwbId", out id, index)) return false;
                }
                else
                {
                    if (!waveformTable.QueryUInt16("StreamAwbId", out id, index)) return false;
                }
            }
            if (!waveformTable.QueryByte("Streaming", out byte streaming, index)) return false;

            if (id != targetWaveID) return true;
            if ((isEmbedded && streaming == 1) || (!isEmbedded && streaming == 0)) return true;

            if (!names.Contains(cueNameName)) names.Add(cueNameName);

            //names.Add(cueNameName);

            return true;
        }

        private static bool LoadSynth(int index)
        {
            synthTable = header.OpenSubtable("SynthTable");
            if (!synthTable.QueryByte("Type", out byte type, index)) return false;
            if (!synthTable.QueryData("ReferenceItems", out ValueData referenceItems, index)) return false;

            synthDepth++;

            int count = (int)referenceItems.Length / 4;
            for (int i = 0; i < count; i++)
            {
                ushort itemType = ReadUInt16BE(synthTable, referenceItems.Offset + (i * 4));
                ushort itemIndex = ReadUInt16BE(synthTable, referenceItems.Offset + (i * 4) + 2);

                switch (itemType)
                {
                    case 0:
                        count = 0;
                        break;
                    case 1:
                        if (!LoadWaveform(itemIndex)) return false;
                        break;
                    case 2:
                        if (!LoadSynth(itemIndex)) return false;
                        break;
                    case 3:
                        if (!LoadSequence(itemIndex)) return false;
                        break;
                    default:
                        count = 0;
                        break;
                }
            }

            synthDepth--;

            return true;
        }

        private static bool LoadCommandTlv(ValueData command)
        {
            uint offset = command.Offset;
            uint maxOffset = offset + command.Length;

            while (offset < maxOffset)
            {
                ushort tlvCode = ReadUInt16BE(trackCommandTable, offset);
                byte tlvSize = ReadByte(trackCommandTable, offset + 2);
                offset += 2 + 1;

                switch (tlvCode)
                {
                    case 2000:
                    case 2003:
                        if (tlvSize < 4)
                        {
                            Console.WriteLine("TLV with unknown size");
                            break;
                        }

                        ushort tlvType = ReadUInt16BE(trackCommandTable, offset);
                        ushort tlvIndex = ReadUInt16BE(trackCommandTable, offset + 2);

                        switch (tlvType)
                        {
                            case 2:
                                if (!LoadSynth(tlvIndex)) return false;
                                break;
                            case 3:
                                if (!LoadSequence(tlvIndex)) return false;
                                break;
                            default:
                                maxOffset = 0;
                                break;
                        }
                        break;
                    default:
                        break;
                }

                offset += tlvSize;
            }

            return true;
        }

        private static bool LoadTrackEventCommand(int index)
        {
            trackTable = header.OpenSubtable("TrackTable");
            if (!trackTable.QueryUInt16("EventIndex", out ushort eventIndex, index)) return false;

            if (eventIndex == 65535) return true;

            if (hasTrackEventTable)
            {
                trackCommandTable = header.OpenSubtable("TrackEventTable");
                if (!trackCommandTable.QueryData("Command", out ValueData command, eventIndex)) return false;

                if (!LoadCommandTlv(command)) return false;
            }

            return true;
        }

        private static bool LoadSequence(int index)
        {
            sequenceTable = header.OpenSubtable("SequenceTable");
            if (!sequenceTable.QueryUInt16("NumTracks", out ushort numTracks, index)) return false;
            if (!sequenceTable.QueryData("TrackIndex", out ValueData trackIndex, index)) return false;

            if (numTracks * 2 > trackIndex.Length) return false;

            sequenceDepth++;

            for (int i = 0; i < numTracks; i++)
            {
                ushort trackIndexValue = ReadUInt16BE(sequenceTable, trackIndex.Offset + (i * 2));

                if (!LoadTrackEventCommand(trackIndexValue)) return false;
            }

            sequenceDepth--;

            return true;
        }

        private static bool LoadCue(int index)
        {
            cueTable = header.OpenSubtable("CueTable");
            if (!cueTable.QueryByte("ReferenceType", out byte referenceType, index)) return false;
            if (!cueTable.QueryUInt16("ReferenceIndex", out ushort referenceIndex, index)) return false;

            switch (referenceType)
            {
                case 0x1:
                    if (!LoadWaveform(referenceIndex)) return false;
                    break;
                case 0x2:
                    if (!LoadSynth(referenceIndex)) return false;
                    break;
                case 0x3:
                    if (!LoadSequence(referenceIndex)) return false;
                    break;
                default:
                    break;
            }

            return true;
        }

        private static bool LoadCueName(int index)
        {
            if (!cueNameTable.QueryString("CueName", out string cueName, index)) return false;
            if (!cueNameTable.QueryUInt16("CueIndex", out ushort cueIndex, index)) return false;

            cueNameName = cueName;
            cueNameIndex = cueIndex;

            if (!LoadCue(cueIndex)) return false;

            return true;
        }

        public List<string> LoadWaveName(int waveID, bool embeddedAwb = false)
        {
            innerStream.Position = 0;

            header = new UtfTable(innerStream);
            if (header.Name != "Header")
                throw new InvalidDataException("No Header table.");

            hasCommandTable = header.QueryData("CommandTable", out ValueData _);
            hasTrackEventTable = header.QueryData("TrackEventTable", out ValueData _);

            names = new List<string>();

            targetWaveID = waveID;
            isEmbedded = embeddedAwb;

            cueNameTable = header.OpenSubtable("CueNameTable");
            for (int i = 0; i < cueNameTable.Rows; i++)
            {
                if (!LoadCueName(i)) break;
            }

            return names;
        }

        private static ushort ReadUInt16BE(UtfTable utfTable, long offset)
        {
            byte[] trackIndexValueBytes = utfTable.GetData(offset, sizeof(ushort));
            Array.Reverse(trackIndexValueBytes);
            return BitConverter.ToUInt16(trackIndexValueBytes, 0);
        }

        private static byte ReadByte(UtfTable utfTable, long offset)
        {
            return utfTable.GetData(offset, sizeof(byte))[0];
        }
    }
}
