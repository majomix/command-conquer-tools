using System;
using System.Collections.Generic;

namespace Skeleton.Model
{
    public class MegaFile
    {
        public Int32 Signature1 { get; set; }
        public Int32 Signature2 { get; set; }
        public Int32 DataStart { get; set; }
        public Int32 NumberOfFileNames { get; set; }
        public Int32 NumberOfFileTableEntries { get; set; }
        public Int32 FileNamesLength { get; set; }
        public List<string> FileNames { get; set; }
        public List<FileTableEntry> FileTableEntries { get; set; }

        public MegaFile()
        {
            FileNames = new List<string>();
            FileTableEntries = new List<FileTableEntry>();
            Signature1 = -1;
            Signature2 = 1065185444;
            DataStart = -1;
            NumberOfFileNames = -1;
            NumberOfFileTableEntries = -1;
            FileNamesLength = -1;
        }
    }
}
