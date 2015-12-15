using SQLite;
using VinylManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Services
{
    class InventaireService
    {
        public static List<Inventaire> GetAllInventaires()
        {
            List<Inventaire> inventaires;
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                // Activate Tracking
                db.Trace = true;

                inventaires = (from a in db.Table<Inventaire>()
                            select a).ToList();
            }

            return inventaires;
        }

        public static List<Inventaire> GetAllInventaireBySingleId(int singleId)
        {
            List<Inventaire> inventaires;
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                inventaires = (from a in db.Table<Inventaire>()
                               where a.DisqueId == singleId && a.TypeId == 1
                               select a).ToList();
            }
            return inventaires;

        }

        public static Inventaire GetInventaireById(int Id)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                Inventaire m = (from a in db.Table<Inventaire>()
                             where a.Id == Id
                             select a).FirstOrDefault();
                return m;
            }
        }

        public static void SaveInventaire(Inventaire inventaire)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Trace = true;

                if (inventaire.Id == 0)
                {
                    // New
                    db.Insert(inventaire);
                }
                else
                {
                    // Update
                    db.Update(inventaire);
                }
            }
        }

        public static void DeleteInventaire(Inventaire inventaire)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Trace = true;

                // Object model:
                // db.Delete(artiste);

                // SQL Syntax:
                db.Execute("DELETE FROM Inventaire WHERE Id = ?", inventaire.Id);
            }
        }

        public static void DeleteAllInventairesOfSingle(int singleId)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Trace = true;

                // Object model:
                // db.Delete(artiste);

                // SQL Syntax:
                db.Execute("DELETE FROM Inventaire WHERE DisqueId = ? and TypeId = ?", singleId, 1);
            }
        }
    }
}
