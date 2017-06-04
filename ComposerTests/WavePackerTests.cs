using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace ComposerTests
{
    [TestClass]
    public class WavePackerTests
    {
        public MemoryStream GetFile(int milliseconds)
        {
            var samples = ComposerLibrary.SampleGenerator.Instance
                .GenerateSamples(milliseconds, 440.0);
            var file = ComposerLibrary.WavePacker.Instance
                .PackFile(samples.ToArray());
            file.Seek((long)0, SeekOrigin.Begin);

            return file;
        }

        [TestMethod]
        public void StreamShouldStartWithRIFF()
        {
            var file = GetFile(2000);
            var bucket = new byte[4];
            file.Read(bucket, 0, 4);
            var first4Chars = System.Text.Encoding.ASCII.GetString(bucket);

            Assert.AreEqual("RIFF", first4Chars);
        }

        [TestMethod]
        public void FileSizeCorrectness()
        {
            var formatOverhead = 44.0;
            var audioLengths = new short[4] { 2000, 50, 1500, 3000 };
            var files = new System.Collections.Generic
                .List<Tuple<double, MemoryStream>>();
            for (int i = 0; i < audioLengths.Length; i++)
            {
                files.Add(new Tuple<double, MemoryStream>(audioLengths[i],
                    GetFile(audioLengths[i])));
            }

            foreach (var file in files)
            {
                Assert.AreEqual(
                    (file.Item1 / 1000.0) * 44100.0 * 2.0 + formatOverhead,
                    file.Item2.Length);
            }
        }
    }
}
