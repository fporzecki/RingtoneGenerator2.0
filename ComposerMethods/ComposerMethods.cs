using System;
using Microsoft.VisualBasic;

namespace ComposerMethods
{
    public static class ComposerMethods
    {
        public static void PackSounds(string score, string filename, float bpm)
        {
            if (score == null || score == "")
                throw new ArgumentException("There's no score to produce!");
            if (filename == null || filename == "")
                throw new ArgumentException("There's no filename!");
            if (Strings.Right(filename, 4) != ".wav")
                filename += ".wav";

            ComposerLibrary.Assembler.Instance
                .AssembleToPackedStream(score, bpm, filename);
        }
    }
}
