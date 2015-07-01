using SQLite;
using VinylManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Services
{
    class TrenteTroisToursService
    {
        public static List<TrenteTroisTours> GetAllTrenteTroisTours()
        {
            List<TrenteTroisTours> trenteTroisTours;
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                // Activate Tracking
                db.Trace = true;

                trenteTroisTours = (from a in db.Table<TrenteTroisTours>()
                            select a).ToList();
            }

            return trenteTroisTours;
        }

        public static TrenteTroisTours GetTrenteTroisToursById(int Id)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                TrenteTroisTours m = (from a in db.Table<TrenteTroisTours>()
                             where a.Id == Id
                             select a).FirstOrDefault();
                return m;
            }
        }

        public static void SaveTrenteTroisTours(TrenteTroisTours trenteTroisTours)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Trace = true;

                if (trenteTroisTours.Id == 0)
                {
                    // New
                    db.Insert(trenteTroisTours);
                }
                else
                {
                    // Update
                    db.Update(trenteTroisTours);
                }
            }
        }

        public static void DeleteArtiste(TrenteTroisTours trenteTroisTours)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Trace = true;

                // Object model:
                // db.Delete(artiste);

                // SQL Syntax:
                db.Execute("DELETE FROM TrenteTroisTours WHERE Id = ?", trenteTroisTours.Id);
            }
        }
    }
}
