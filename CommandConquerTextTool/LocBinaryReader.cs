using System;
using System.IO;
using System.Text;

namespace CommandConquerTextTool
{
    public class LocBinaryReader : BinaryReader
    {
        public LocBinaryReader(Stream input) : base(input) { }

        public LocFile ReadDataFile()
        {
            var locFile = new LocFile();

            locFile.NumberOfEntries = ReadInt32();

            for (var i = 0; i < locFile.NumberOfEntries; i++)
            {
                var descriptor = new StringDescriptor();

                descriptor.KeyHash = ReadUInt32();
                descriptor.ValueLength = ReadInt32();
                descriptor.KeyLength = ReadInt32();

                locFile.Entries.Add(descriptor);
            }

            for (var i = 0; i < locFile.NumberOfEntries; i++)
            {
                var descriptor = locFile.Entries[i];

                descriptor.Value = Encoding.Unicode.GetString(ReadBytes(descriptor.ValueLength * 2));
            }

            for (var i = 0; i < locFile.NumberOfEntries; i++)
            {
                var descriptor = locFile.Entries[i];

                descriptor.Key = Encoding.ASCII.GetString(ReadBytes(descriptor.KeyLength));
            }

            return locFile;
        }
    }


}
