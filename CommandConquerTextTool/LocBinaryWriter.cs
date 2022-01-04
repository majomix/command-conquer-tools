using System.IO;
using System.Linq;
using System.Text;

namespace CommandConquerTextTool
{
    public class LocBinaryWriter : BinaryWriter
    {
        public LocBinaryWriter(FileStream fileStream) : base(fileStream) { }

        public void Write(LocFile locFile)
        {
            Write(locFile.Entries.Count);

            var orderedEntries = locFile.Entries.OrderBy(e => e.KeyHash).ToList();

            foreach (var entry in orderedEntries)
            {
                Write(entry.KeyHash);
                Write(entry.ValueLength);
                Write(entry.KeyLength);
            }

            foreach (var entry in orderedEntries)
            {
                Write(Encoding.Unicode.GetBytes(entry.Value));
            }

            foreach (var entry in orderedEntries)
            {
                Write(Encoding.ASCII.GetBytes(entry.Key));
            }
        }
    }
}
