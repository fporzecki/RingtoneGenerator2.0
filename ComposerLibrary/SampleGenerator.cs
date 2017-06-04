using System;
using System.Collections.Generic;
using System.Linq;

namespace ComposerLibrary
{
    public class SampleGenerator
    {
        private static SampleGenerator _instance;
        private SampleGenerator()
        {
            SampleRate = 44100.0;
            SixteenBitSampleLimit = 32767.0;
            Volume = 0.8;
        }
        public static SampleGenerator Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new SampleGenerator();
                }
                return _instance;
            }
        }
        public double SampleRate { get; set; }
        public double SixteenBitSampleLimit { get; set; }
        public double Volume { get; set; }
        
        public List<short> GenerateSamples(double milliseconds,
            double frequency)
        {
            var numOfSamples = milliseconds / 1000.0 * SampleRate;

            var requiredSamples = new List<double>();
            for (int i = 1; i <= numOfSamples; i++)
            {
                requiredSamples.Add((double)i);
            }

            var result = new List<short>();
            foreach (var sample in requiredSamples)
            {
                result.Add(ToAmplitude(sample, frequency));
            }

            return result.ToList();
        }

        public short ToAmplitude(double requiredSample, double frequency)
        {
            var result = requiredSample * 2.0 * Math.PI * frequency / SampleRate;
            result = Math.Sin(result);
            result *= SixteenBitSampleLimit * Volume;
            return (short)result;
        }
    }
}
