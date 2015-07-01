using SQLiteNetExtensions.Attributes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Models
{
    public class TrenteTroisToursTitres
    {
        [ForeignKey(typeof(TrenteTroisTours)), NotNull]
        public int TrenteTroisToursId { get; set; }

        [ForeignKey(typeof(Titre)), NotNull]
        public int FaceId { get; set; }
    }
}
