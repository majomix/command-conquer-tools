using System;
using System.Collections.Generic;

namespace CommandConquerTextTool
{
    public class LocFile
    {
        public Int32 NumberOfEntries { get; set; }

        public List<StringDescriptor> Entries { get; set; }

        public LocFile()
        {
            Entries = new List<StringDescriptor>();
        }
    }
}
