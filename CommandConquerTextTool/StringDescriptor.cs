using System;

namespace CommandConquerTextTool
{
    public class StringDescriptor
    {
        public UInt32 KeyHash { get; set; }
        public Int32 ValueLength { get; set; }
        public Int32 KeyLength { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
