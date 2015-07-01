using SQLiteNetExtensions.Attributes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Models
{
    class SinglesTitres
    {
        [ForeignKey(typeof(Singles)), NotNull]
        public int SingleId { get; set; }

        [ForeignKey(typeof(Titre)), NotNull]
        public int FaceId { get; set; }
    }
}
