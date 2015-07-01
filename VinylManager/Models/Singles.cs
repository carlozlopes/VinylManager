using SQLiteNetExtensions.Attributes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Models
{
    public class Singles
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100), NotNull]
        public string Nom { get; set; }

        [ForeignKey(typeof(Artiste)), Unique(Name = "IDX_SINGLES"), NotNull]
        public int ArtisteId { get; set; }

        [Unique(Name = "IDX_SINGLES"), NotNull]
        public int FaceAId { get; set; }

        [Unique(Name = "IDX_SINGLES")]
        public int FaceBId { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public Artiste Artiste { get; set; }

        [ManyToMany(typeof(SinglesTitres))]
        public List<Titre> Faces { get; set; }
    }
}
