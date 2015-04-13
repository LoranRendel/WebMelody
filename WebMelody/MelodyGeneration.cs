using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Melody;
using WaveGenerator;
using System.IO;
using System.Text;
namespace WebMelody
{
    public class MelodyGeneration
    {
        static uint Samplerate = 8000;
        static BitDepth Bitness = BitDepth.Bit8;
        static ushort Channels = 1;
        static int LengthLimit = 1000;

        public static string Generate(string melody)
        {
            MemoryStream fileStream = new MemoryStream();
            WaveFile melodyFile = new WaveFile(Samplerate, Bitness, Channels, fileStream);
            SoundGenerator generator = new SoundGenerator(melodyFile);           
            NotationTranstalor.Song song = new NotationTranstalor.Song(string.Empty, string.Empty, melody);
            if (song.Length > MelodyGeneration.LengthLimit)
            {
                var ex = new ArgumentException("The melody is too long.");
                ex.Data.Add("Error", GenerationError.TooLong);
                throw ex;
            }
            if (song == NotationTranstalor.Song.Empty)
            {
                var ex = new ArgumentException("Incorrect notation");
                ex.Data.Add("Error", GenerationError.IncorrectNotation);
                throw ex;
            }
            //Generation
            double startPhase = 0d;
            foreach(var note in song.Notes)           
                startPhase = generator.AddSimpleTone(note.Frequency, note.Duration, startPhase, 0.4, true);
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
    } 
    public enum GenerationError
    {
        IncorrectNotation,
        TooLong,
        General,
        Empty
    }
}