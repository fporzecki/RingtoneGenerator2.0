using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ComposerTests
{
    [TestClass]
    public class SampleGeneratorTests
    {
        [TestMethod]
        public void AmountOfSamplesTests()
        {
            var milliseconds = 2000.0;
            var frequency = 440.0;
            var samples = ComposerLibrary.SampleGenerator.Instance
                .GenerateSamples(milliseconds, frequency);
            
            Assert.AreEqual(88200, samples.Count);
        }

        [TestMethod]
        public void SamplesAreInRangeTest()
        {
            var milliseconds = 2000.0;
            var frequency = 440.0;
            var samples = ComposerLibrary.SampleGenerator.Instance
                .GenerateSamples(milliseconds, frequency);

            foreach (var sample in samples)
            {
                Assert.IsTrue(sample > -1 * ComposerLibrary.SampleGenerator
                    .Instance.SixteenBitSampleLimit
                    && sample < ComposerLibrary.SampleGenerator
                    .Instance.SixteenBitSampleLimit);
            }
        }

        [TestMethod]
        public void SamplesShouldBeZeroWhenGenerating0Hz()
        {
            var milliseconds = 2000.0;
            var frequency = 0.0;
            var samples = ComposerLibrary.SampleGenerator.Instance
                .GenerateSamples(milliseconds, frequency);

            var expectedSamples = new List<short>();

            foreach (var sample in samples)
            {
                expectedSamples.Add(0);
            }
            CollectionAssert.AreEqual(expectedSamples, samples);
        }
    }
}
