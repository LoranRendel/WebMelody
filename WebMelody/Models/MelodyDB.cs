using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebMelody.Models
{
    public class MelodyDB:DbContext
    {
        public DbSet<Piece> Pieces {get;set;}

        public MelodyDB(string connectionStringName):base(connectionStringName)
        {

        }
    }
}