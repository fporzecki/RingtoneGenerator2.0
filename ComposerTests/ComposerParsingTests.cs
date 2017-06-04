using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerTests
{
    [TestClass]
    public class ComposerParsingTests
    {
        [TestMethod]
        public void ShouldReturnProperFractionMeasurement()
        {
            var score1 = "32.#d3";
            var resultLength = ComposerLibrary.ComposerParsing.Instance
                .ParseMeasureFraction(score1);

            Assert.AreEqual(resultLength.Fraction, 
                ComposerLibrary.ComposerParsing.MeasureFraction.Thirtyseconth);
            Assert.IsTrue(resultLength.Extended);

            var score2 = "32#d3";
            resultLength = ComposerLibrary.ComposerParsing.Instance
                .ParseMeasureFraction(score2);

            Assert.AreEqual(resultLength.Fraction,
                ComposerLibrary.ComposerParsing.MeasureFraction.Thirtyseconth);
            Assert.IsFalse(resultLength.Extended);

            var score3 = "1#d3";
            resultLength = ComposerLibrary.ComposerParsing.Instance
                .ParseMeasureFraction(score3);

            Assert.AreEqual(resultLength.Fraction,
                ComposerLibrary.ComposerParsing.MeasureFraction.Full);
            Assert.IsFalse(resultLength.Extended);
        }

        [TestMethod]
        public void NoteIsSharpable()
        {
            var score1 = "32.#d3";
            Assert.IsTrue(ComposerLibrary.ComposerParsing.Instance
                .SharpableNote(score1));

            var score2 = "32.d3";
            Assert.IsFalse(ComposerLibrary.ComposerParsing.Instance
                .SharpableNote(score2));
        }

        [TestMethod]
        public void ProperNoteParsing()
        {
            var score1 = "32.#d3";
            var noteResult = ComposerLibrary.ComposerParsing.Instance
                .ParseNote(score1);
            Assert.AreEqual(noteResult, 
                ComposerLibrary.ComposerParsing.Note.DSharp);

            var score2 = "32.a3";
            noteResult = ComposerLibrary.ComposerParsing.Instance
                .ParseNote(score2);
            Assert.AreEqual(noteResult,
                ComposerLibrary.ComposerParsing.Note.A);
        }

        [TestMethod]
        public void ProperOctaveParsing()
        {
            var score1 = "32.#d3";
            var octaveResult = ComposerLibrary.ComposerParsing.Instance
                .ParseOctave(score1);
            Assert.AreEqual(octaveResult, 
                ComposerLibrary.ComposerParsing.Octave.Three);

            var score2 = "32.#d2";
            octaveResult = ComposerLibrary.ComposerParsing.Instance
                .ParseOctave(score2);
            Assert.AreEqual(octaveResult,
                ComposerLibrary.ComposerParsing.Octave.Two);

            var score3 = "32.#d1";
            octaveResult = ComposerLibrary.ComposerParsing.Instance
                .ParseOctave(score3);
            Assert.AreEqual(octaveResult,
                ComposerLibrary.ComposerParsing.Octave.One);
        }

        [TestMethod]
        public void ProperSoundConstructionRestIncluded()
        {
            var score1 = "32.#d3";
            var sound = ComposerLibrary.ComposerParsing.Instance
                .CombineSoundData(score1);
            Assert.AreEqual(sound.Note, 
                ComposerLibrary.ComposerParsing.Note.DSharp);
            Assert.AreEqual(sound.Octave, 
                ComposerLibrary.ComposerParsing.Octave.Three);

            var score2 = "32.-";
            var sound2 = ComposerLibrary.ComposerParsing.Instance
                .CombineSoundData(score2);
            Assert.AreEqual(sound2.Note, 
                ComposerLibrary.ComposerParsing.Note.Rest);
            Assert.AreEqual(sound2.Octave, 
                ComposerLibrary.ComposerParsing.Octave.Rest);
        }

        [TestMethod]
        public void ProperTokenCreation()
        {
            var score1 = "32.#d3";
            var token = ComposerLibrary.ComposerParsing.Instance
                .CreateNoteToken(score1, 120.0f);

            Assert.AreEqual(token.Length.Fraction,
                ComposerLibrary.ComposerParsing.MeasureFraction.Thirtyseconth);
            Assert.IsTrue(token.Length.Extended);
            Assert.AreEqual(token.Sound.Note,
                ComposerLibrary.ComposerParsing.Note.DSharp);
            Assert.AreEqual(token.Sound.Octave,
                ComposerLibrary.ComposerParsing.Octave.Three);
        }

        [TestMethod]
        public void ShouldParseASimpleScore()
        {
            var score = "32.#d3 16-";
            var tokens = ComposerLibrary.ComposerParsing.Instance
                .ParseScore(score, 120.0f);
            Assert.AreEqual(tokens[0].Length.Fraction,
                ComposerLibrary.ComposerParsing.MeasureFraction.Thirtyseconth);
            Assert.IsTrue(tokens[0].Length.Extended);
            Assert.AreEqual(tokens[0].Sound.Note,
                ComposerLibrary.ComposerParsing.Note.DSharp);
            Assert.AreEqual(tokens[0].Sound.Octave,
                ComposerLibrary.ComposerParsing.Octave.Three);

            Assert.AreEqual(tokens[1].Length.Fraction,
                ComposerLibrary.ComposerParsing.MeasureFraction.Sixteenth);
            Assert.IsFalse(tokens[1].Length.Extended);
            Assert.AreEqual(tokens[1].Sound.Note,
                ComposerLibrary.ComposerParsing.Note.Rest);
            Assert.AreEqual(tokens[1].Sound.Octave,
                ComposerLibrary.ComposerParsing.Octave.Rest);
        }

        [TestMethod]
        public void ProperFrequencyIsReturned()
        {
            var expectedFrequency = 440;
            var score = "32.a2";
            var token = ComposerLibrary.ComposerParsing.Instance
                .ParseScore(score, 120.0f);
            var frequency = ComposerLibrary.ComposerParsing.Instance
                .Frequency(token[0]);

            Assert.AreEqual(expectedFrequency, (int)frequency);

            expectedFrequency = 220;
            score = "32.a1";
            token = ComposerLibrary.ComposerParsing.Instance
                .ParseScore(score, 120.0f);
            frequency = ComposerLibrary.ComposerParsing.Instance
                .Frequency(token[0]);

            Assert.AreEqual(expectedFrequency, (int)frequency);

            expectedFrequency = 880;
            score = "32.a3";
            token = ComposerLibrary.ComposerParsing.Instance
                .ParseScore(score, 120.0f);
            frequency = ComposerLibrary.ComposerParsing.Instance
                .Frequency(token[0]);

            Assert.AreEqual(expectedFrequency, (int)frequency);
        }
    }
}
