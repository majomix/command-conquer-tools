using System.IO;
using System.Linq;
using System.Text;
using Force.Crc32;

namespace CommandConquerTextTool
{
    public class TextConverter
    {
        private LocFile _locFile;

        public void CompactLanguages(string directory)
        {
            LocFile english;
            LocFile german;
            LocFile polish;

            using (var file = File.Open(directory + "MASTERTEXTFILE_EN-US.LOC", FileMode.Open))
            using (var reader = new LocBinaryReader(file))
            {
                english = reader.ReadDataFile();
            }

            using (var file = File.Open(directory + "MASTERTEXTFILE_DE-DE.LOC", FileMode.Open))
            using (var reader = new LocBinaryReader(file))
            {
                german = reader.ReadDataFile();
            }

            using (var file = File.Open(directory + "MASTERTEXTFILE_PL-PL.LOC", FileMode.Open))
            using (var reader = new LocBinaryReader(file))
            {
                polish = reader.ReadDataFile();
            }

            using (var stream = File.Open(directory + "MASTERTEXTFILE.csv", FileMode.Create))
            using (var writer = new StreamWriter(stream, Encoding.Unicode))
            {
                foreach (var entry in english.Entries.OrderBy(e => e.Key))
                {
                    var englishValue = entry.Value.Replace("\r\n", "\\n").Replace("\n", "\\n").Replace("\t", "\\t");
                    var germanValue = german.Entries.Single(e => e.Key == entry.Key).Value.Replace("\r\n", "\\n").Replace("\n", "\\n").Replace("\t", "\\t");
                    var polishValue = polish.Entries.Single(e => e.Key == entry.Key).Value.Replace("\r\n", "\\n").Replace("\n", "\\n").Replace("\t", "\\t");

                    writer.WriteLine($"{entry.Key}\t{englishValue}\t{englishValue}\t{polishValue}\t{germanValue}");
                }
            }
        }

        public void LoadLocFile(string path)
        {
            using (var file = File.Open(path, FileMode.Open))
            using (var reader = new LocBinaryReader(file))
            {
                _locFile = reader.ReadDataFile();
            }
        }

        public void WriteTextFile(string path)
        {
            using (var stream = File.Open(path, FileMode.Create))
            using (var writer = new StreamWriter(stream, Encoding.Unicode))
            {
                foreach (var entry in _locFile.Entries.OrderBy(e => e.Key))
                {
                    writer.WriteLine($"{entry.Key}\t{entry.Value.Replace("\n", "\\n").Replace("\t", "\\t")}");
                }
            }
        }

        public void LoadTextFile(string path)
        {
            _locFile = new LocFile();

            var lines = File.ReadAllLines(path, Encoding.Unicode);

            foreach (var line in lines)
            {

                string[] tokens = line.Split(new[] {'\t'}, 2);

                if (tokens.Length != 2)
                    continue;

                var key = tokens[0];
                var value = tokens[1].Replace("\\n", "\n").Replace("\\t", "\t");
                var keyHash = Crc32Algorithm.Compute(Encoding.ASCII.GetBytes(key), 0, key.Length);

                var descriptor = new StringDescriptor
                {
                    Key = key,
                    KeyLength = key.Length,
                    KeyHash = keyHash,
                    Value = value,
                    ValueLength = value.Length
                };

                _locFile.Entries.Add(descriptor);
            }
        }

        public void WriteLocFile(string path)
        {
            using (var fileStream = File.Open(path, FileMode.Create))
            {
                using (var writer = new LocBinaryWriter(fileStream))
                {
                    writer.Write(_locFile);
                }
            }
        }

        //private string ConvertGameStringToText()
        //{

        //}

        //private string ConvertTextToGameString()
        //{

        //}
    }
}