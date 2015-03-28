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
        public static string Generate(string melody)
        {
            MemoryStream fileStream = new MemoryStream();
            WaveFile melodyFile = new WaveFile(16000, BitDepth.Bit16, 1, fileStream);
            SoundGenerator generator = new SoundGenerator(melodyFile);           
            NotationTranstalor.Song song = new NotationTranstalor.Song(string.Empty, string.Empty, melody);
            if (song.Length > 1000 * 60 * 2)
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
                startPhase = generator.AddSimpleTone(note.Frequency, note.Duration, startPhase, 1, true);
            generator.Save();        
            fileStream.Position = 0;
            string dataUrl = string.Format("data:audio/wav;base64,{0}", Convert.ToBase64String(fileStream.ToArray()));
            return dataUrl;  
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