using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerTests
{
    [TestClass]
    public class AssemblerTests
    {
        [TestMethod]
        public void ResultsShouldBeOfCorrectLength()
        {
            var score = "2#d3 2- 2- 8#d3 4c2 4c2 8c1 2- 4c1";
            var tokens = ComposerLibrary.ComposerParsing.Instance
                .ParseScore(score, 120.0f);
            var samples = ComposerLibrary.Assembler.Instance
                .Assemble(tokens);
            var expectedSamples = 6.0 * 44100.0;

            Assert.AreEqual(expectedSamples, samples.Count);
        }
    }
}
