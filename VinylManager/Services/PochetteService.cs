using SQLite;
using VinylManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Services
{
    class PochetteService
    {
        public static List<Pochette> GetAllPochettes()
        {
            List<Pochette> pochettes;
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                // Activate Tracking
                db.Trace = true;

                pochettes = (from a in db.Table<Pochette>()
                            select a).ToList();
            }

            return pochettes;
        }

        public static Pochette GetPochetteById(int Id)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                Pochette m = (from a in db.Table<Pochette>()
                             where a.Id == Id
                             select a).FirstOrDefault();
                return m;
            }
        }

        public static void SavePochette(Pochette pochette)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Trace = true;

                if (pochette.Id == 0)
                {
                    // New
                    db.Insert(pochette);
                }
                else
                {
                    // Update
                    db.Update(pochette);
                }
            }
        }

        public static void DeleteArtiste(Pochette pochette)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Trace = true;

                // Object model:
                // db.Delete(artiste);

                // SQL Syntax:
                db.Execute("DELETE FROM Pochette WHERE Id = ?", pochette.Id);
            }
        }
    }
}
