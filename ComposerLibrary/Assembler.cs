using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComposerLibrary
{
    public class Assembler
    {
        private static Assembler _instance;
        private Assembler() { }
        public static Assembler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Assembler();
                }
                return _instance;
            }
        }
        public List<short> TokenToSound(ComposerParsing.Token token)
        {
            var duration = ComposerParsing.Instance.DurationFromToken(token);
            var frequency = ComposerParsing.Instance.Frequency(token);

            return SampleGenerator.Instance.GenerateSamples(duration, frequency);
        }

        public List<short> Assemble(List<ComposerParsing.Token> tokens)
        {
            var output = new List<short>();
            foreach (var token in tokens)
            {
                var sounds = TokenToSound(token);
                foreach (var sound in sounds)
                {
                    output.Add(sound);
                }
            }

            return output;
        }

        public void AssembleToPackedStream(string score, float tempo, 
            string filename)
        {
            var tokens = ComposerParsing.Instance.ParseScore(score, tempo);
            var sounds = Assemble(tokens).ToArray();
            var ms = WavePacker.Instance.PackFile(sounds);
            WavePacker.Instance.WriteFile(ms, filename);
        }
    }
}
