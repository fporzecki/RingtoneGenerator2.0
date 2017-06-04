using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ComposerLibrary
{
    public class ComposerParsing
    {
        private static ComposerParsing _instance;
        private ComposerParsing() { }
        public static ComposerParsing Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ComposerParsing();
                }
                return _instance;
            }
        }

        public enum MeasureFraction
        {
            Full = 1,
            Half = 2,
            Quarter = 4,
            Eighth = 8,
            Sixteenth = 16,
            Thirtyseconth = 32
        }

        public enum Octave
        {
            One,
            Two,
            Three,
            Rest = -1
        }

        public enum Note
        {
            A,
            ASharp,
            B,
            C,
            CSharp,
            D,
            DSharp,
            E,
            F,
            FSharp,
            G,
            GSharp,
            Rest = -1
        }

        public struct Length
        {
            public MeasureFraction Fraction { get; set; }
            public bool Extended { get; set; }
            public float Tempo { get; set; }

            public Length(MeasureFraction fraction, bool extended, float tempo)
            {
                Fraction = fraction;
                Extended = extended;
                Tempo = tempo;
            }
        }

        public struct Sound
        {
            public Note Note { get; set; }
            public Octave Octave { get; set; }

            public Sound(Note note, Octave octave)
            {
                Note = note;
                Octave = octave;
            }
        }

        public struct Token
        {
            public Length Length { get; set; }
            public Sound Sound { get; set; }

            public Token(Length length, Sound sound)
            {
                Length = length;
                Sound = sound;
            }
        }

        public Length ParseMeasureFraction(string score)
        {
            var fractionPattern = @"\A(16|1|2|4|8|32)";
            var fractionExtendedPattern = @"\A\b(1.|2.|4.|8.|16.|32.)";

            if (Regex.Match(score, fractionExtendedPattern).Success)
            {
                var match = Regex.Match(score, fractionExtendedPattern).Value;

                switch (match)
                {
                    case "1.":
                        {
                            return new Length(
                                MeasureFraction.Full, true, 0.0f);
                        }
                    case "2.":
                        {
                            return new Length
                                (MeasureFraction.Half, true, 0.0f);
                        }
                    case "4.":
                        {
                            return new Length(
                                MeasureFraction.Quarter, true, 0.0f);
                        }
                    case "8.":
                        {
                            return new Length(
                                MeasureFraction.Eighth, true, 0.0f);
                        }
                    case "16.":
                        {
                            return new Length(
                                MeasureFraction.Sixteenth, true, 0.0f);
                        }
                    case "32.":
                        {
                            return new Length(
                                MeasureFraction.Thirtyseconth, true, 0.0f);
                        }
                    default:
                        break;
                }
            }

            if (Regex.Match(score, fractionPattern).Success)
            {
                var match = int.Parse(Regex
                    .Match(score, fractionPattern).Value);

                switch (match)
                {
                    case 1:
                        {
                            return new Length(
                                MeasureFraction.Full, false, 0.0f);
                        }
                    case 2:
                        {
                            return new Length(
                                MeasureFraction.Half, false, 0.0f);
                        }
                    case 4:
                        {
                            return new Length(
                                MeasureFraction.Quarter, false, 0.0f);
                        }
                    case 8:
                        {
                            return new Length(
                                MeasureFraction.Eighth, false, 0.0f);
                        }
                    case 16:
                        {
                            return new Length(
                                MeasureFraction.Sixteenth, false, 0.0f);
                        }
                    case 32:
                        {
                            return new Length(
                                MeasureFraction.Thirtyseconth, false, 0.0f);
                        }
                    default:
                        break;
                }
            }

            throw new ArgumentException(
                "The passed score does not fit the criteria");
        }

        public bool SharpableNote(string score)
        {
            if (Regex.Match(score, "#").Success) return true;

            return false;
        }

        public Note ParseNote(string score)
        {
            var sharpablePattern = @"a|c|d|f|g";
            var pattern = @"b|e";
            var restPattern = @"-";
            if (Regex.Match(score, restPattern).Success)
                return Note.Rest;

            if (Regex.Match(score, sharpablePattern).Success)
            {
                var match = Regex.Match(score, sharpablePattern).Value;
                if (SharpableNote(score))
                {
                    switch (match)
                    {
                        case "a":
                            return Note.ASharp;
                        case "c":
                            return Note.CSharp;
                        case "d":
                            return Note.DSharp;
                        case "f":
                            return Note.FSharp;
                        case "g":
                            return Note.GSharp;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (match)
                    {
                        case "a":
                            return Note.A;
                        case "c":
                            return Note.C;
                        case "d":
                            return Note.D;
                        case "f":
                            return Note.F;
                        case "g":
                            return Note.G;
                        default:
                            break;
                    }
                }
            }

            if(Regex.Match(score, pattern).Success)
            {
                var match = Regex.Match(score, pattern).Value;
                switch(match)
                {
                    case "b":
                        return Note.B;
                    case "e":
                        return Note.E;
                    default:
                        break;
                }
            }

            throw new ArgumentException(
                "The passed score does not fit the criteria");
        }

        public Octave ParseOctave(string score)
        {
            var pattern = @"(1|2|3|-)\Z";
            //score = score.Remove(0, 2);
            if(Regex.Match(score, pattern).Success)
            {
                var match = Regex.Match(score, pattern).Value;
                switch (match)
                {
                    case "1":
                        return Octave.One;
                    case "2":
                        return Octave.Two;
                    case "3":
                        return Octave.Three;
                    case "-":
                        return Octave.Rest;
                    default:
                        break;
                }
            }

            throw new ArgumentException(
                "The passed score does not fit the criteria");
        }

        public Sound CombineSoundData(string score)
        {
            var note = ParseNote(score);
            var octave = ParseOctave(score);

            return new Sound(note, octave);
        }

        public Token CreateNoteToken(string score, float tempo)
        {
            var length = ParseMeasureFraction(score);
            length.Tempo = tempo;

            var sound = CombineSoundData(score);

            return new Token(length, sound);
        }

        public List<Token> ParseScore(string score, float tempo)
        {
            var splitTokens = new List<Token>();
            var splits = score.Split(new[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries);
            foreach (var split in splits)
            {
                splitTokens.Add(CreateNoteToken(split, tempo));
            }

            return splitTokens;
        }

        public double DurationFromToken(Token token)
        {
            var secondsPerBeat = 60.0 / token.Length.Tempo;
            switch (token.Length.Fraction)
            {
                case MeasureFraction.Full:
                    return 4.0 * 1000.0 * secondsPerBeat;
                case MeasureFraction.Half:
                    return 2.0 * 1000.0 * secondsPerBeat;
                case MeasureFraction.Quarter:
                    return 1000.0 * secondsPerBeat;
                case MeasureFraction.Eighth:
                    return (1.0 / 2.0) * 1000.0 * secondsPerBeat;
                case MeasureFraction.Sixteenth:
                    return (1.0 / 4.0) * 1000.0 * secondsPerBeat;
                case MeasureFraction.Thirtyseconth:
                    return (1.0 / 8.0) * 1000.0 * secondsPerBeat;
                default:
                    break;
            }

            throw new ArgumentException();
        }

        public int NoteIndex(Note note, Octave octave, List<Note> notes)
        {
            var noteIndex = notes.IndexOf(note);
            return noteIndex + (int)octave * 12;
        }

        public int SemitonesBetween(Token lower, Token upper)
        {
            var noteSequence = new List<Note>()
            {
                Note.A, Note.ASharp, Note.B, Note.C, Note.CSharp,
                Note.D, Note.DSharp, Note.E, Note.F, Note.FSharp,
                Note.G, Note.GSharp
            };

            return NoteIndex(upper.Sound.Note, upper.Sound.Octave, noteSequence)
                - NoteIndex(lower.Sound.Note, lower.Sound.Octave, noteSequence);
        }

        public double Frequency(Token token)
        {
            if (token.Sound.Note == Note.Rest &&
                token.Sound.Octave == Octave.Rest) return 0.0;
            
            var tempToken = new Token(new Length(MeasureFraction.Full,
                true, 120.0f), new Sound(Note.A, Octave.One));
            var gap = SemitonesBetween(tempToken, token);
            
            return 220.0 * Math.Pow((Math.Pow(2.0, (1.0 / 12.0))), gap); 
        }
    }
}