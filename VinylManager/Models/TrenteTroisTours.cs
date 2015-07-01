using SQLiteNetExtensions.Attributes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Models
{
    public class TrenteTroisTours
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100), Unique(Name = "IDX_33t"), NotNull]
        public string Nom { get; set; }

        [ForeignKey(typeof(Artiste)), Unique(Name = "IDX_33t"), NotNull]
        public int ArtisteId { get; set; }

        [ManyToOne]
        public Artiste Artiste { get; set; }

        [ManyToMany(typeof(TrenteTroisToursTitres))]
        public List<Titre> Faces { get; set; }
    }
}
