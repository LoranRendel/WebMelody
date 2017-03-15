using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;

using System.ComponentModel.DataAnnotations;

namespace WebMelodyCore.Models
{
    public class Piece
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DateGenerated { get; set; }
        public string UserIP { get; set; }

        public Piece()
        {
            this.DateGenerated = DateTime.UtcNow;
        }
    }
}