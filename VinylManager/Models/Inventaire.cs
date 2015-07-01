using SQLiteNetExtensions.Attributes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Models
{
    public class Inventaire
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int DisqueId { get; set; }

        [MaxLength(50)]
        public string Etat { get; set; }

        [MaxLength(50)]
        public string Couleur { get; set; }

        [NotNull]
        public int TypeId { get; set; }

        [ForeignKey(typeof(Pochette))]
        public int PochetteId { get; set; }

        [OneToOne]
        public Pochette Pochette { get; set; }
    }
}
