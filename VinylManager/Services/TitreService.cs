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
    class TitreService
    {
        public static List<Titre> GetAllFaces()
        {
            List<Titre> faces;
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                // Activate Tracking
                db.Trace = true;

                faces = (from a in db.Table<Titre>()
                            select a).ToList();
            }

            return faces;
        }

        public static List<Titre> GetAllTitresByGivenQuery(String query)
        {
            if (query.Equals(""))
            {
                return GetAllFaces();
            }
            List<Titre> titres;
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                // Activate Tracking
                db.Trace = true;

                titres = (from a in db.Table<Titre>()
                            where a.Nom.Contains(query)
                            select a).ToList();
            }

            return titres;
        }

        public static Titre GetTitreWithChildren(int id)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                return db.GetWithChildren<Titre>(id);
            }
        }

        public static Titre GetTitreById(int Id)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                Titre m = (from a in db.Table<Titre>()
                             where a.Id == Id
                             select a).FirstOrDefault();
                return m;
            }
        }

        public static int selectOrInsertTitre(String nom, String annee) {
            int id = selectTitre(nom, annee);

            if (-1 != id)
            {
                return id;
            }
                
            Titre titre = new Titre();
            titre.Nom = nom;
            titre.Annee = annee;

            SaveTitre(titre);

            return titre.Id;
        }

        public static int selectTitre(String nom, String annee) {
            List<Titre> titres;
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                titres = db.Query<Titre>("SELECT * from Titre t WHERE t.nom = '" + nom + "' AND t.annee = '" + annee + "'");
            }
            if (0 == titres.Count())
            {
                return -1;
            }
            else {
                Titre titre = titres.Single();
                return titre.Id;
            }
        }

        public static void SaveTitre(Titre face)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Trace = true;

                if (face.Id == 0)
                {
                    // New
                    db.Insert(face);
                }
                else
                {
                    // Update
                    db.Update(face);
                }
            }
        }

        public static void DeleteTitre(Titre titre)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Trace = true;

                // Object model:
                // db.Delete(face);

                // SQL Syntax:
                db.Execute("DELETE FROM Titre WHERE Id = ?", titre.Id);
            }
        }
    }
}
