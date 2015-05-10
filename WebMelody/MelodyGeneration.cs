using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using Melody;
using WaveGenerator;
using System.IO;
using System.Text;

using System.Xml;
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
            generator.Volume = 0.3;
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

        public static IEnumerable<Piece> ReadExamples(HttpApplication webapp)
        {
            string pathToList = webapp.Server.MapPath("~/App_Data/tunes.xml");
            XDocument examples = null;      
            try
            {
               examples = XDocument.Load(pathToList);
            }
            catch
            {
                return null;
            }
            var pieces = examples.Root.Descendants("piece");         
            return pieces.Select(piece => new Piece()
            { 
                Title = piece.Attribute("name").Value,
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