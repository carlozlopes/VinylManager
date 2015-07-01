using SQLiteNetExtensions.Attributes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Models
{
    class QuatreTitreTitres
    {
        [ForeignKey(typeof(QuatreTitres)), NotNull]
        public int QuatreTitresId { get; set; }

        [ForeignKey(typeof(Titre)), NotNull]
        public int FaceId { get; set; }
    }
}
