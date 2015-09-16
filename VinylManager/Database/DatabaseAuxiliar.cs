using SQLiteNetExtensions.Extensions;
using SQLite;
using System.Diagnostics;
using VinylManager.Models;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Database
{
    public class DatabaseAuxiliar
    {
        public string DBPath { get; set; }
        public SQLiteConnection conn { get; set; }

        public DatabaseAuxiliar()
        {
            this.DBPath = Path.Combine(
                Windows.Storage.ApplicationData.Current.LocalFolder.Path, "vinyls.sqlite");
            conn = new SQLiteConnection(this.DBPath);
        }

        public void initDatabase()
        {
            this.conn.DropTable<Artiste>();
            this.conn.DropTable<Titre>();
            this.conn.DropTable<Inventaire>();
            this.conn.DropTable<Pochette>();
            this.conn.DropTable<QuatreTitres>();
            this.conn.DropTable<QuatreTitreTitres>();
            this.conn.DropTable<Singles>();
            this.conn.DropTable<SinglesTitres>();
            this.conn.DropTable<TrenteTroisTours>();
            this.conn.DropTable<TrenteTroisToursTitres>();
            this.conn.CreateTable<Artiste>();
            this.conn.CreateTable<Titre>();
            this.conn.CreateTable<Inventaire>();
            this.conn.CreateTable<Pochette>();
            this.conn.CreateTable<QuatreTitres>();
            this.conn.CreateTable<QuatreTitreTitres>();
            this.conn.CreateTable<Singles>();
            this.conn.CreateTable<SinglesTitres>();
            this.conn.CreateTable<TrenteTroisTours>();
            this.conn.CreateTable<TrenteTroisToursTitres>();
        }

        public Artiste insertArtiste(string nom, String nationalite, int interprete, int compositeur, int auteur)
        {
            Artiste artiste = new Artiste();
            artiste.Nom = nom;
            artiste.Nationalite = nationalite;
            artiste.interprete = interprete;
            artiste.auteur = auteur;
            artiste.compositeur = compositeur;
            try
            {
                this.conn.RunInTransaction(() =>
                {
                    // database calls inside the transaction
                    this.conn.Insert(artiste);
                });
                return artiste;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error insert Artiste: " + nom + ": " + e.Message);
                return null;
            }
        }

        public Artiste selectArtiste(String nom)
        {
            var query = this.conn.Table<Artiste>().Where(v => v.Nom == nom);
            foreach (var artiste in query)
            {
                return (Artiste)artiste;
            }
            return null;
        }

        public Titre insertTitre(string nom, string annee)
        {
            var titre = new Titre();
            titre.Nom = nom;
            titre.Annee = annee;
            try
            {
                this.conn.Insert(titre);
                return titre;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error insert titre: " + e.Message);
                return null;
            }
        }

        public Titre selectTitre(String nom, String annee)
        {
            var query = this.conn.Table<Titre>().Where(v => v.Nom == nom && v.Annee == annee);
            foreach (var titre in query)
            {
                return (Titre)titre;
            }
            return null;
        }

        public Singles insertSingle(string nom, Artiste artiste, Titre[] singleFaces, int singleCounter)
        {
            Singles single = new Singles();
            single.Nom = nom;
            single.FaceAId = singleFaces[0].Id;
            single.FaceA = singleFaces[0].Nom;
            if (null != singleFaces[1])
            {
                single.FaceBId = singleFaces[1].Id;
                single.FaceB = singleFaces[1].Nom;
            }
            try
            {
                this.conn.RunInTransaction(() =>
                {
                    // database calls inside the transaction
                    this.conn.Insert(single);
                    SinglesTitres singleFaceA = new SinglesTitres();
                    singleFaceA.FaceId = single.FaceAId;
                    singleFaceA.SingleId = single.Id;
                    this.conn.Insert(singleFaceA);
                    if (null != singleFaces[1])
                    {
                        SinglesTitres singleFaceB = new SinglesTitres();
                        singleFaceB.FaceId = single.FaceBId;
                        singleFaceB.SingleId = single.Id;
                        this.conn.Insert(singleFaceB);   
                    }
                    Pochette pochette = new Pochette();
                    pochette.Etat = "Excellent";
                    this.conn.Insert(pochette);
                    Inventaire inventaire = new Inventaire();
                    inventaire.DisqueId = single.Id;
                    inventaire.Etat = "Excellent";
                    inventaire.Couleur = "Noir";
                    inventaire.TypeId = 1;
                    this.conn.Insert(inventaire);
                    inventaire.PochetteId = pochette.Id;
                    this.conn.Update(inventaire);
                    single.ArtisteId = artiste.Id;
                    this.conn.Update(single);
                    artiste.singleCounter = singleCounter;
                    this.conn.Update(artiste);
                });
                return single;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error insert Single: " + nom + ": " + e.Message);
                return null;
            }
        }

        public Singles selectSingle(int artisteId, int faceAId, int faceBId)
        {
            TableQuery<Singles> query;
            if (-1 != faceBId)
            {
                query = this.conn.Table<Singles>().Where(v => v.ArtisteId == artisteId
                   && v.FaceAId == faceAId && v.FaceBId == faceBId);
            }
            else
            {
                query = this.conn.Table<Singles>().Where(v => v.ArtisteId == artisteId
                   && v.FaceAId == faceAId && v.FaceBId == null);
            }
            foreach (var single in query)
            {
                return (Singles)single;
            }
            return null;
        }

        public QuatreTitres insert4t(string nom, Artiste artiste, Titre[] quatreTFaces, int quatreTCounter)
        {
            QuatreTitres quatreT = new QuatreTitres();
            quatreT.Nom = nom;
            quatreT.FaceA1Id = quatreTFaces[0].Id;
            if (null != quatreTFaces[1])
            {
                quatreT.FaceA2Id = quatreTFaces[1].Id;
            }
            quatreT.FaceB1Id = quatreTFaces[2].Id;
            if (null != quatreTFaces[3])
            {
                quatreT.FaceB2Id = quatreTFaces[3].Id;
            }
            try
            {
                this.conn.RunInTransaction(() =>
                {
                    // database calls inside the transaction
                    this.conn.Insert(quatreT);
                    QuatreTitreTitres faceA1 = new QuatreTitreTitres();
                    faceA1.FaceId = quatreT.FaceA1Id;
                    faceA1.QuatreTitresId = quatreT.Id;
                    this.conn.Insert(faceA1);
                    QuatreTitreTitres faceB1 = new QuatreTitreTitres();
                    faceB1.FaceId = quatreT.FaceB1Id;
                    faceB1.QuatreTitresId = quatreT.Id;
                    this.conn.Insert(faceB1);
                    if (null != quatreT.FaceA2Id)
                    {
                        QuatreTitreTitres faceA2 = new QuatreTitreTitres();
                        faceA2.FaceId = quatreT.FaceA2Id;
                        faceA2.QuatreTitresId = quatreT.Id;
                        this.conn.Insert(faceA2);
                    }
                    if (null != quatreT.FaceB2Id)
                    {
                        QuatreTitreTitres faceB2 = new QuatreTitreTitres();
                        faceB2.FaceId = quatreT.FaceB2Id;
                        faceB2.QuatreTitresId = quatreT.Id;
                        this.conn.Insert(faceB2);
                    }
                    Pochette pochette = new Pochette();
                    pochette.Etat = "Excellent";
                    this.conn.Insert(pochette);
                    Inventaire inventaire = new Inventaire();
                    inventaire.DisqueId = quatreT.Id;
                    inventaire.Etat = "Excellent";
                    inventaire.Couleur = "Noir";
                    inventaire.TypeId = 2;
                    this.conn.Insert(inventaire);
                    inventaire.PochetteId = pochette.Id;
                    this.conn.Update(inventaire);
                    quatreT.ArtisteId = artiste.Id;
                    this.conn.Update(quatreT);
                    artiste.quatreTitresCounter = quatreTCounter;
                    this.conn.Update(artiste);
                });
                return quatreT;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error insert Quatre Titres: " + nom + ": " + e.Message);
                return null;
            }
        }

        public QuatreTitres selecte4t(int artisteId, int faceA1Id, int faceA2Id, int faceB1Id, int faceB2Id)
        {
            TableQuery<QuatreTitres> query;
            if (-1 != faceA2Id && -1 != faceB2Id)
            {
                query = this.conn.Table<QuatreTitres>().Where(v => v.ArtisteId == artisteId
                    && v.FaceA1Id == faceA1Id && v.FaceA2Id == faceA2Id && v.FaceB1Id == faceB1Id
                    && v.FaceB2Id == faceB2Id);
            }
            else if (-1 != faceA2Id)
            {
                query = this.conn.Table<QuatreTitres>().Where(v => v.ArtisteId == artisteId
                    && v.FaceA1Id == faceA1Id && v.FaceA2Id == faceA2Id && v.FaceB1Id == faceB1Id
                    && v.FaceB2Id == null);
            }
            else if (-1 != faceB2Id)
            {
                query = this.conn.Table<QuatreTitres>().Where(v => v.ArtisteId == artisteId
                    && v.FaceA1Id == faceA1Id && v.FaceA2Id == null && v.FaceB1Id == faceB1Id
                    && v.FaceB2Id == faceB2Id);
            }
            else
            {
                query = this.conn.Table<QuatreTitres>().Where(v => v.ArtisteId == artisteId
                    && v.FaceA1Id == faceA1Id && v.FaceA2Id == null && v.FaceB1Id == faceB1Id
                    && v.FaceB2Id == null);
            }
            foreach (var quatreT in query)
            {
                return (QuatreTitres)quatreT;
            }
            return null;
        }

        public TrenteTroisTours insert33t(String nom, Artiste artiste, List<Titre> list33tA,
            List<Titre> list33tB, int trenteTroisTCounter)
        {
            TrenteTroisTours trente3t = new TrenteTroisTours();
            trente3t.Nom = nom;
            try
            {
                this.conn.RunInTransaction(() =>
                {
                    // database calls inside the transaction
                    this.conn.Insert(trente3t);
                    Pochette pochette = new Pochette();
                    pochette.Etat = "Excellent";
                    this.conn.Insert(pochette);
                    Inventaire inventaire = new Inventaire();
                    inventaire.DisqueId = trente3t.Id;
                    inventaire.Etat = "Excellent";
                    inventaire.Couleur = "Noir";
                    inventaire.TypeId = 1;
                    this.conn.Insert(inventaire);
                    inventaire.PochetteId = pochette.Id;
                    this.conn.Update(inventaire);
                    trente3t.ArtisteId = artiste.Id;
                    this.conn.Update(trente3t);

                    foreach (var faceA in list33tA)
                    {
                        TrenteTroisToursTitres trente3tFaceA = new TrenteTroisToursTitres();
                        trente3tFaceA.TrenteTroisToursId = trente3t.Id;
                        trente3tFaceA.FaceId = faceA.Id;
                        this.conn.Insert(trente3tFaceA);
                    }

                    foreach (var faceB in list33tB)
                    {
                        TrenteTroisToursTitres trente3tFaceB = new TrenteTroisToursTitres();
                        trente3tFaceB.TrenteTroisToursId = trente3t.Id;
                        trente3tFaceB.FaceId = faceB.Id;
                        this.conn.Insert(trente3tFaceB);
                    }
                    artiste.trenteTroisTitresCounter = trenteTroisTCounter;
                    this.conn.Update(artiste);
                });
                return trente3t;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error insert Trente trois Tours: " + nom + ": " + e.Message);
                return null;
            }
        }
    }
}
