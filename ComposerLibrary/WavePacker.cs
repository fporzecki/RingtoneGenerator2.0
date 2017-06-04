using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComposerLibrary
{
    public class WavePacker
    {
        private static WavePacker _instance;
        private WavePacker(){}
        public static WavePacker Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WavePacker();
                }
                return _instance;
            }
        }

        public MemoryStream PackFile(short[] data)
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream, Encoding.ASCII);
            var dataLength = data.Length * 2;

            // filetype definition
            writer.Write(Encoding.ASCII.GetBytes("RIFF"));
            writer.Write(data.Length);
            writer.Write(Encoding.ASCII.GetBytes("WAVE"));

            // content interpretation definition
            writer.Write(Encoding.ASCII.GetBytes("fmt "));
            writer.Write(16);
            writer.Write((short)1); //  PCM
            writer.Write((short)1); //  mono
            writer.Write(44100);    //  sample rate
            writer.Write(44100 * 16 / 8);   //  byte rate
            writer.Write((short)2); //  bytes per sample
            writer.Write((short)16);    //  bits per sample

            // data
            writer.Write(Encoding.ASCII.GetBytes("data"));
            writer.Write(dataLength);   //  length of the data

            var newData = new byte[dataLength];
            for (int i = 0; i < dataLength; i++)
            {
                newData[i] = 0;
            }

            Buffer.BlockCopy(data, 0, newData, 0, newData.Length);
            writer.Write(newData);

            return stream;
        }

        public void WriteFile(MemoryStream ms, string filename)
        {
            using (var fs = new FileStream(filename, FileMode.Create))
            {
                ms.WriteTo(fs);
            }
        }
    }
}
