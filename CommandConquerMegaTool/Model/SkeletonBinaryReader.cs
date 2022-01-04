using System.IO;
using System.Text;

namespace Skeleton.Model
{
    internal class SkeletonBinaryReader : BinaryReader
    {
        public SkeletonBinaryReader(FileStream fileStream)
            : base(fileStream) { }

        public MegaFile ReadStructure()
        {
            var megaFile = new MegaFile();

            megaFile.Signature1 = ReadInt32();
            megaFile.Signature2 = ReadInt32();
            megaFile.DataStart = ReadInt32();
            megaFile.NumberOfFileNames = ReadInt32();
            megaFile.NumberOfFileTableEntries = ReadInt32();
            megaFile.FileNamesLength = ReadInt32();

            for (var i = 0; i < megaFile.NumberOfFileNames; i++)
            {
                var fileNameLength = ReadInt16();
                megaFile.FileNames.Add(Encoding.ASCII.GetString(ReadBytes(fileNameLength)));
            }

            for (var i = 0; i < megaFile.NumberOfFileTableEntries; i++)
            {
                var fileTableEntry = new FileTableEntry();

                fileTableEntry.IsEncrypted = ReadUInt16();
                fileTableEntry.FileNameHash = ReadUInt32();
                fileTableEntry.NumberOfEntry = ReadUInt32();
                fileTableEntry.DataSize = ReadInt32();
                fileTableEntry.DataOffset = ReadUInt32();
                fileTableEntry.FileNameTableIndex = ReadInt16();

                megaFile.FileTableEntries.Add(fileTableEntry);
            }

            return megaFile;
        }
    }
}