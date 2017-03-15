using System.Collections.Generic;

namespace WebMelodyCore.Models
{
    public class NonJSGenerateMelody
    {
        public string Melody { get; set; }
        public string DataUrl { get; set; }
        public IEnumerable<Piece> Examples { get; set; }
    }
}