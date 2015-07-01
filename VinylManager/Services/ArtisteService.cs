using SQLiteNetExtensions.Extensions;
using SQLite;
using VinylManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Services
{
    public class ArtisteService
    {
        public static List<Artiste> GetAllArtistes()
        {
            List<Artiste> artistes;
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                // Activate Tracking
                db.Trace = true;

                artistes = (from a in db.Table<Artiste>()
                          select a).ToList();
            }

            return artistes;
        }

        public static List<Artiste> GetAllArtistesByGivenQuery(String query)
        {
            if (query.Equals(""))
            {
                return GetAllArtistes();
            }
            List<Artiste> artistes;
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                // Activate Tracking
                db.Trace = true;

                artistes = (from a in db.Table<Artiste>()
                            where a.Nom.Contains(query)
                            select a).ToList();
            }

            return artistes;
        }

        public static Artiste GetArtisteWithChildren(int id) {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                return db.GetWithChildren<Artiste>(id);
            }
        }

        public static Artiste GetArtisteById(int Id)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                Artiste m = (from a in db.Table<Artiste>()
                            where a.Id == Id
                            select a).FirstOrDefault();
                return m;
            }
        }

        public static void SaveArtiste(Artiste artiste)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Trace = true;

                if (artiste.Id == 0)
                {
                    // New
                    db.Insert(artiste);
                }
                else
                {
                    // Update
                    db.Update(artiste);
                }
            }
        }

        public static void updateSingleCounter(int artisteId) {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Execute("UPDATE Artiste SET singleCounter=singleCounter + 1 WHERE Id=?", artisteId);
            }
        }

        public static void updateQuatreTitresCounter(int artisteId)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Execute("UPDATE Artiste SET quatreTitresCounter=quatreTitresCounter + 1 WHERE Id=?", artisteId);
            }
        }

        public static void updateTrenteTroisTitresCounter(int artisteId)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Execute("UPDATE Artiste SET trenteTroisTitresCounter=quatreTitresCounter + 1 WHERE Id=?", artisteId);
            }
        }

        public static void DeleteArtiste(Artiste artiste)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Trace = true;

                // Object model:
                // db.Delete(artiste);

                // SQL Syntax:
                db.Execute("DELETE FROM Artiste WHERE Id = ?", artiste.Id);
            }
        }
    }
}
