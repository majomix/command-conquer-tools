using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Force.Crc32;

namespace Skeleton.Model
{
    internal class SkeletonEditor
    {
        public MegaFile MegaFile { get; set; }

        public void LoadFileStructure(SkeletonBinaryReader reader)
        {
            MegaFile = reader.ReadStructure();
        }

        public void ExtractFile(FileTableEntry entry, string directory, SkeletonBinaryReader reader)
        {
            var name = MegaFile.FileNames[entry.FileNameTableIndex];
            var compoundName = directory + @"\" + name;

            Directory.CreateDirectory(Path.GetDirectoryName(compoundName));

            using (var fileStream = File.Open(compoundName, FileMode.Create))
            {
                using (var writer = new BinaryWriter(fileStream))
                {
                    reader.BaseStream.Seek(entry.DataOffset, SeekOrigin.Begin);
                    writer.Write(reader.ReadBytes(entry.DataSize));
                }
            }
        }

        public void SaveDataEntry(SkeletonBinaryReader reader, SkeletonBinaryWriter writer)
        {

        }

        public void SaveIndex(SkeletonBinaryWriter writer)
        {

        }

        public void WriteStubMegaFile(SkeletonBinaryWriter writer, List<string> fileNames, List<FileTableEntry> entries)
        {
            MegaFile = new MegaFile
            {
                FileNames = fileNames,
                NumberOfFileNames = fileNames.Count,
                FileTableEntries = entries,
                NumberOfFileTableEntries = entries.Count
            };

            writer.WriteMegaFile(MegaFile);
        }

        public void WriteDataEntry(SkeletonBinaryWriter writer, FileTableEntry entry, string fileNameToHash)
        {
            entry.DataOffset = (uint)writer.BaseStream.Position;

            var bytes = File.ReadAllBytes(entry.Import);

            var nameBytes = Encoding.ASCII.GetBytes(fileNameToHash);

            entry.DataSize = bytes.Length;
            entry.FileNameHash = Crc32Algorithm.Compute(nameBytes, 0, nameBytes.Length);

            writer.Write(bytes);
        }

        public void FinalizeMegaFile(SkeletonBinaryWriter writer)
        {
            var orderedFileTable = MegaFile.FileTableEntries.OrderBy(entry => entry.FileNameHash).ToList();

            for (var i = 0; i < orderedFileTable.Count; i++)
            {
                orderedFileTable[i].NumberOfEntry = (uint)i;
            }

            MegaFile.FileTableEntries = orderedFileTable;

            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            writer.WriteMegaFile(MegaFile);
        }
    }
}