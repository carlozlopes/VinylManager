using SQLiteNetExtensions.Extensions;
using SQLite;
using VinylManager.Services;
using VinylManager.Models;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Managers
{
    class SinglesManager
    {
        public static void SaveOrUpdate(Singles single)
        {
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                db.Trace = true;

                if (single.Id == 0)
                {
                    try
                    {
                        db.RunInTransaction(() =>
                        {
                            // database calls inside the transaction
                            Titre faceA = single.Faces[0];
                            Titre faceB = single.Faces[1];
                            int faceAId = TitreService.selectOrInsertTitre(faceA.Nom, faceA.Annee);
                            int faceBId = TitreService.selectOrInsertTitre(faceB.Nom, faceB.Annee);
                            single.FaceAId = faceAId;
                            single.FaceBId = faceBId;
                            db.Insert(single);
                            SinglesTitres singleFaceA = new SinglesTitres();
                            singleFaceA.FaceId = single.FaceAId;
                            singleFaceA.SingleId = single.Id;
                            db.Insert(singleFaceA);
                            SinglesTitres singleFaceB = new SinglesTitres();
                            singleFaceB.FaceId = single.FaceBId;
                            singleFaceB.SingleId = single.Id;
                            db.Insert(singleFaceB);
                            Artiste artiste = ArtisteService.GetArtisteById(single.ArtisteId);
                            artiste.singleCounter += 1;
                            db.InsertOrReplace(artiste);
                            /* Pochette pochette = new Pochette();
                            pochette.Etat = "Excellent";
                            db.Insert(pochette);
                            Inventaire inventaire = new Inventaire();
                            inventaire.DisqueId = single.Id;
                            inventaire.Etat = "Excellent";
                            inventaire.Couleur = "Noir";
                            inventaire.TypeId = 1;
                            inventaire.PochetteId = pochette.Id;
                            db.Insert(inventaire); */
                        });
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Error insert Single: " + single.Nom + ": " + e.Message);
                    }
                }
                else
                {
                    try
                    {
                        db.RunInTransaction(() =>
                        {
                            // database calls inside the transaction
                            int oldFaceAId = single.FaceAId;
                            int oldFaceBId = single.FaceBId;
                            Titre faceA = single.Faces[0];
                            Titre faceB = single.Faces[1];
                            int faceAId = TitreService.selectOrInsertTitre(faceA.Nom, faceA.Annee);
                            int faceBId = TitreService.selectOrInsertTitre(faceB.Nom, faceB.Annee);
                            single.FaceAId = faceAId;
                            single.FaceBId = faceBId;
                            db.Update(single);
                            if (oldFaceAId != faceAId)
                            {
                                db.Execute("UPDATE SinglesTitres SET FaceId = ? WHERE SingleId = ? AND FaceId = ?", faceAId, single.Id, oldFaceAId);
                                // SinglesTitresService.updateSingleTitre(single.Id, faceAId, oldFaceAId);
                            }
                            if (oldFaceBId != faceBId)
                            {
                                db.Execute("UPDATE SinglesTitres SET FaceId = ? WHERE SingleId = ? AND FaceId = ?", faceBId, single.Id, oldFaceBId);
                                // SinglesTitresService.updateSingleTitre(single.Id, faceBId, oldFaceBId);
                            }
                            /* Pochette pochette = new Pochette();
                            pochette.Etat = "Excellent";
                            db.Insert(pochette);
                            Inventaire inventaire = new Inventaire();
                            inventaire.DisqueId = single.Id;
                            inventaire.Etat = "Excellent";
                            inventaire.Couleur = "Noir";
                            inventaire.TypeId = 1;
                            inventaire.PochetteId = pochette.Id;
                            db.Insert(inventaire); */
                        });
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Error insert Single: " + single.Nom + ": " + e.Message);
                    }
                }
            }
        }

    }
}
