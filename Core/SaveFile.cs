using System;
using System.CodeDom;
using System.IO;
using System.Linq;
using System.Text;
using SaveEditor.Utilities;

namespace SaveEditor.Core
{
    public class SaveFile
    {
        public uint checksum { get; private set; }
        public byte[] data { get; private set; }

        private readonly Reader reader_;
        private readonly Writer writer_;

        public SaveFile(uint fileChecksum, byte[] fileData)
        {
            checksum = fileChecksum;
            data = fileData;

            Stream stream = Stream.Synchronized(new MemoryStream(data));
            reader_ = new Reader(stream);
            writer_ = new Writer(stream);
        }

        public string GetName()
        {
            return Encoding.Default.GetString(reader_.ReadBytes(6, 0x80));
        }

        public void WriteName(string name)
        {
            writer_.WriteBytes(Encoding.ASCII.GetBytes(name.PadRight(6, (char)0x00).Substring(0, 6)), 0x80);
        }

        public void EnableArrows()
        {
            //Enables Light Arrows bitFlag which is stored at 0x32F in the savegame slot using a bitmask. Does nothing if already enabled
            writer_.WriteByte((byte)(reader_.ReadByte(0x2A8) | 0x8), 0x2A8);
        }

    }
}