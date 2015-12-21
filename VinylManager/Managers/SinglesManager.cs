using VinylManager.Utils;
using VinylManager.Services;
using VinylManager.Models;
using SQLiteNetExtensions.Extensions;
using SQLite;
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
                            Inventaire vinyl = new Inventaire();
                            vinyl.DisqueId = single.Id;
                            vinyl.Etat = InventaryConstants.DEFAULT_VINYL_STATE;
                            vinyl.Couleur = InventaryConstants.DEFAULT_VINYL_COLOR;
                            vinyl.EtatPochette = InventaryConstants.DEFAULT_POCHETTE_STATE;
                            vinyl.TypeId = InventaryConstants.SINGLE_TYPE;
                            db.Insert(vinyl);
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
