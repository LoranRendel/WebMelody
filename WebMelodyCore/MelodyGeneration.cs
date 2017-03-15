using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Simue;
using WaveGenerator;
using System.IO;
using System.Text;
using WebMelodyCore.Models;
using System.Xml;
namespace WebMelodyCore
{
    public class MelodyGeneration
    {
        static uint Samplerate = 8000;
        static BitDepth Bitness = BitDepth.Bit8;
        static ushort Channels = 1;
        static int LengthLimit = 1000*120;
        public static string Generate(string melody)
        {
            SimueCompiler compiler = new SimueCompiler();
            var result = compiler.Parse(compiler.Tokenize(melody));
            if (result.Errors.Count != 0)
            {
                var ex = new ArgumentException("Incorrect notation");
                ex.Data.Add("Error", GenerationError.IncorrectNotation);
                throw ex;
            }
            if (result.Song.Length > MelodyGeneration.LengthLimit)
            {
                var ex = new ArgumentException("The melody is too long.");
                ex.Data.Add("Error", GenerationError.TooLong);
                throw ex;
            }
            MemoryStream fileStream = new MemoryStream();
            WaveFile melodyFile = new WaveFile(Samplerate, Bitness, Channels, fileStream);
            SoundGenerator generator = new SoundGenerator(melodyFile);
            generator.Volume = 0.3;
            Song song = result.Song;
            //Generation
            foreach(var note in song.Notes)
                generator.AddSimpleTone(note.Frequency, note.Duration);
            generator.Save();
            fileStream.Position = 0;
            string dataUrl = string.Format("data:audio/wav;base64,{0}", Convert.ToBase64String(fileStream.ToArray()));
            return dataUrl;
        }
        public static void Config(uint samplerate, BitDepth bitness, ushort channels, int lengthLimit)
        {
            MelodyGeneration.Samplerate = samplerate;
            MelodyGeneration.Bitness = bitness;
            MelodyGeneration.Channels = channels;
            MelodyGeneration.LengthLimit = lengthLimit;
        }
        public static IEnumerable<Piece> ReadExamples(string pathToExampleFile)
        {           
            XDocument examples = null;
            try
            {
               examples = XDocument.Load(pathToExampleFile);
            }
            catch
            {
                return null;
            }
            var pieces = examples.Root.Descendants("piece");
            return pieces.Select(piece => new Piece()
            {
                Title = piece.Attribute("name")?.Value,
                Text = piece.Value
            });
        }
    }
    public enum GenerationError
    {
        IncorrectNotation,
        TooLong,
        General,
        Empty
    }
}