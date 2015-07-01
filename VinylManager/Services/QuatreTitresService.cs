using SQLite;
using VinylManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Services
{
    class QuatreTitresService
    {
        public static List<QuatreTitres> GetAllQuatreTitres()
        {
            List<QuatreTitres> quatreTitres;
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                // Activate Tracking
                db.Trace = true;

                quatreTitres = (from a in db.Table<QuatreTitres>()
                            select a).ToList();
            }

            return quatreTitres;
        }

        public static QuatreTitres GetArtisteById(int Id)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                QuatreTitres m = (from a in db.Table<QuatreTitres>()
                             where a.Id == Id
                             select a).FirstOrDefault();
                return m;
            }
        }

        public static void SaveQuatreTitres(QuatreTitres quatreTitres)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Trace = true;

                if (quatreTitres.Id == 0)
                {
                    // New
                    db.Insert(quatreTitres);
                }
                else
                {
                    // Update
                    db.Update(quatreTitres);
                }
            }
        }

        public static void DeleteQuatreTitres(QuatreTitres quatreTitres)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Trace = true;

                // Object model:
                // db.Delete(artiste);

                // SQL Syntax:
                db.Execute("DELETE FROM QuatreTitres WHERE Id = ?", quatreTitres.Id);
            }
        }
    }
}
