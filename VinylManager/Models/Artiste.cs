using SQLiteNetExtensions.Attributes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Models
{
    public class Artiste
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100), Unique (Name = "IDX_Artiste"), NotNull]
        public string Nom { get; set; }

        [MaxLength(100)]
        public string Nationalite { get; set; }

        public int singleCounter { get; set; }

        public int quatreTitresCounter { get; set; }

        public int trenteTroisTitresCounter { get; set; }

        public int interprete { get; set; }
        
        public int auteur { get; set; }

        public int compositeur { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Singles> Singles { get; set; }

    }
}
