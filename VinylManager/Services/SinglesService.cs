using SQLiteNetExtensions.Extensions;
using SQLite;
using VinylManager.Models;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Services
{
    class SinglesService
    {
        public static List<Singles> GetAllSingles()
        {
            List<Singles> singles;
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                // Activate Tracking
                db.Trace = true;

                singles = (from a in db.Table<Singles>()
                            select a).ToList();
            }

            return singles;
        }

        public static List<Singles> GetAllSinglesByGivenQuery(String query)
        {
            List<Singles> singles;
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                // Activate Tracking
                db.Trace = true;

                /* singles = (from a in db.Table<Singles>()
                          where a.Nom.Contains(query)
                          select a).ToList(); */

                if (query.Equals(""))
                {
                    singles = db.GetAllWithChildren<Singles>();
                }
                else
                {
                    singles = db.GetAllWithChildren<Singles>(s => s.Nom.Contains(query));
                }
            }

            return singles;
        }

        /* public static List<Singles> GetAllSinglesByTitre(String query)
        {
            List<Singles> singles;
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                singles = db.Query<Singles>("SELECT S.* FROM Singles S INNER JOIN SinglesFaces SF ON S.Id = SF.SingleId INNER JOIN Face F ON F.Id = SF.FaceId WHERE F.Nom like '%"+query+"%'");

            }
            return singles;
        } */

        public static List<Singles> GetAllSinglesByArtisteAndGivenQuery(String query, int artisteId)
        {
            List<Singles> singles;
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                // Activate Tracking
                db.Trace = true;

                if (query.Equals(""))
                {
                    singles = db.GetAllWithChildren<Singles>(s => s.ArtisteId.Equals(artisteId));
                }
                else
                {
                    singles = db.GetAllWithChildren<Singles>(s => s.Nom.Contains(query) && s.ArtisteId.Equals(artisteId));
                }
                
            }

            return singles;
        }

        public static Singles GetSingleWithChildren(int Id)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                return db.GetWithChildren<Singles>(Id);
            }
        }

        public static Singles GetSingleById(int Id)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                Singles m = (from a in db.Table<Singles>()
                             where a.Id == Id
                             select a).FirstOrDefault();
                return m;
            }
        }

        public static void SaveSingle(Singles single)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Trace = true;

                if (single.Id == 0)
                {
                    // New
                    db.Insert(single);
                }
                else
                {
                    // Update
                    db.Update(single);
                }
            }
        }

        public Singles insertSingle(string nom, Artiste artiste, Titre[] singleFaces, int singleCounter)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                Singles single = new Singles();
                single.Nom = nom;
                single.FaceAId = singleFaces[0].Id;
                if (null != singleFaces[1])
                {
                    single.FaceBId = singleFaces[1].Id;
                }
                try
                {
                    db.RunInTransaction(() =>
                    {
                        // database calls inside the transaction
                        db.Insert(single);
                        SinglesTitres singleFaceA = new SinglesTitres();
                        singleFaceA.FaceId = single.FaceAId;
                        singleFaceA.SingleId = single.Id;
                        db.Insert(singleFaceA);
                        if (null != singleFaces[1])
                        {
                            SinglesTitres singleFaceB = new SinglesTitres();
                            singleFaceB.FaceId = single.FaceBId;
                            singleFaceB.SingleId = single.Id;
                            db.Insert(singleFaceB);
                        }
                        Inventaire inventaire = new Inventaire();
                        inventaire.DisqueId = single.Id;
                        inventaire.Etat = "Excellent";
                        inventaire.Couleur = "Noir";
                        inventaire.EtatPochette = "Excellente";
                        inventaire.TypeId = 1;
                        db.Insert(inventaire);
                        single.ArtisteId = artiste.Id;
                        db.Update(single);
                        artiste.singleCounter = singleCounter;
                        db.Update(artiste);
                    });
                    return single;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error insert Single: " + nom + ": " + e.Message);
                    return null;
                }
            }
        }

        public static void DeleteSingle(Singles single)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Trace = true;

                // Object model:
                // db.Delete(artiste);

                // SQL Syntax:
                db.Execute("DELETE FROM Single WHERE Id = ?", single.Id);
            }
        }
    }
}
