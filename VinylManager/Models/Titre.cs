using SQLiteNetExtensions.Attributes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Models
{
    public class Titre
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100), Unique(Name="IDX_FACE"), NotNull]
        public string Nom { get; set; }

        [MaxLength(10), Unique(Name = "IDX_FACE")]
        public string Annee { get; set; }

        [ManyToMany(typeof(SinglesTitres))]
        public List<Singles> Singles { get; set; }

        [ManyToMany(typeof(TrenteTroisToursTitres))]
        public List<TrenteTroisTours> TrenteTroisTours { get; set; }
    }
}