using System;

namespace Skeleton.Model
{
    public class FileTableEntry
    {
        public UInt16 IsEncrypted { get; set; }
        public UInt32 FileNameHash { get; set; }
        public UInt32 NumberOfEntry { get; set; }
        public Int32 DataSize { get; set; }
        public UInt32 DataOffset { get; set; }
        public Int16 FileNameTableIndex { get; set; }
        public string Import { get; set; }
    }
}
