using SQLiteNetExtensions.Attributes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Models
{
    public class QuatreTitres
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100), NotNull]
        public string Nom { get; set; }

        [ForeignKey(typeof(Artiste)), Unique(Name = "IDX_4T"), NotNull]
        public int ArtisteId { get; set; }

        [Unique(Name = "IDX_4T"), NotNull]
        public int FaceA1Id { get; set; }

        [Unique(Name = "IDX_4T"), NotNull]
        public int FaceA2Id { get; set; }

        [Unique(Name = "IDX_4T"), NotNull]
        public int FaceB1Id { get; set; }

        [Unique(Name = "IDX_4T"), NotNull]
        public int FaceB2Id { get; set; }

        [ManyToOne]
        public Artiste Artiste { get; set; }

        [ManyToMany(typeof(QuatreTitreTitres))]
        public List<Titre> Faces { get; set; }
    }
}
