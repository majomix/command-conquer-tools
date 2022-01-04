using System;
using NDesk.Options;

namespace CommandConquerTextTool
{
    public class Program
    {
        static void Main(string[] args)
        {
            var export = true;
            string compactDir = string.Empty;
            string locFile = string.Empty;
            string txtFile = string.Empty;

            var options = new OptionSet()
                .Add("import", value => export = false)
                .Add("loc=", value => locFile = value)
                .Add("compact=", value => compactDir = value)
                .Add("txt=", value => txtFile = value);

            options.Parse(Environment.GetCommandLineArgs());

            var converter = new TextConverter();

            if (compactDir != string.Empty)
            {
                converter.CompactLanguages(compactDir);
            }
            else if (export)
            {
                converter.LoadLocFile(locFile);
                converter.WriteTextFile(txtFile);
            }
            else
            {
                converter.LoadTextFile(txtFile);
                converter.WriteLocFile(locFile);
            }
        }
    }
}
