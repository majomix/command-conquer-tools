using System;
using System.IO;
using System.Text;

namespace Skeleton.Model
{
    internal class SkeletonBinaryWriter : BinaryWriter
    {
        public SkeletonBinaryWriter(Stream stream)
            : base(stream) { }

        public void WriteMegaFile(MegaFile megaFile)
        {
            Write(megaFile.Signature1);
            Write(megaFile.Signature2);
            Write(megaFile.DataStart);
            Write(megaFile.NumberOfFileNames);
            Write(megaFile.NumberOfFileTableEntries);
            Write(megaFile.FileNamesLength);

            var fileNameTableStartPosition = BaseStream.Position;

            for (var i = 0; i < megaFile.NumberOfFileNames; i++)
            {
                Write((Int16)megaFile.FileNames[i].Length);
                Write(Encoding.ASCII.GetBytes(megaFile.FileNames[i]));
            }

            megaFile.FileNamesLength = (int)(BaseStream.Position - fileNameTableStartPosition);

            for (var i = 0; i < megaFile.NumberOfFileTableEntries; i++)
            {
                Write(megaFile.FileTableEntries[i].IsEncrypted);
                Write(megaFile.FileTableEntries[i].FileNameHash);
                Write(megaFile.FileTableEntries[i].NumberOfEntry);
                Write(megaFile.FileTableEntries[i].DataSize);
                Write(megaFile.FileTableEntries[i].DataOffset);
                Write(megaFile.FileTableEntries[i].FileNameTableIndex);
            }

            megaFile.DataStart = (int)BaseStream.Position;
        }
    }
}