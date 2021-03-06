﻿using VinylManager.JoinModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Services
{
    class SinglesJoinService
    {
        public static List<SinglesJoinData> GetAllSinglesByTitre(String query)
        {
            List<SinglesJoinData> singles;
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                if (query.Equals(""))
                {
                    singles = db.Query<SinglesJoinData>("SELECT DISTINCT S.Id, S.Nom, A.Nom as Artiste FROM Singles S INNER JOIN Artiste A ON A.Id = S.ArtisteId INNER JOIN SinglesTitres ST ON S.Id = ST.SingleId INNER JOIN Titre T ON T.Id = ST.FaceId");
                }
                else
                {
                    singles = db.Query<SinglesJoinData>("SELECT DISTINCT S.Id, S.Nom, A.Nom as Artiste FROM Singles S INNER JOIN Artiste A ON A.Id = S.ArtisteId INNER JOIN SinglesTitres ST ON S.Id = ST.SingleId INNER JOIN Titre T ON T.Id = ST.FaceId WHERE T.Nom like '%" + query + "%'");
                }

            }
            return singles;
        }

        public static List<SinglesJoinData> GetAllSinglesByTitreAndArtiste(String query, int artisteId)
        {
            List<SinglesJoinData> singles;
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                if (query.Equals(""))
                {
                    singles = db.Query<SinglesJoinData>("SELECT DISTINCT S.Id, S.Nom, A.Nom as Artiste FROM Singles S INNER JOIN Artiste A ON A.Id = S.ArtisteId INNER JOIN SinglesTitres ST ON S.Id = ST.SingleId INNER JOIN Titre T ON T.Id = ST.FaceId WHERE A.Id = '" + artisteId + "'");
                }
                else
                {
                    singles = db.Query<SinglesJoinData>("SELECT DISTINCT S.Id, S.Nom, A.Nom as Artiste FROM Singles S INNER JOIN Artiste A ON A.Id = S.ArtisteId INNER JOIN SinglesTitres ST ON S.Id = ST.SingleId INNER JOIN Titre T ON T.Id = ST.FaceId WHERE T.Nom like '%" + query + "%' AND A.Id = '" + artisteId + "'");
                }
            }
            return singles;
        }

        public static List<SinglesJoinData> GetAllSinglesTitresByTitre(String query)
        {
            List<SinglesJoinData> singles;
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                if (query.Equals(""))
                {
                    singles = db.Query<SinglesJoinData>("SELECT S.Id, S.Nom, A.Nom as Artiste, T.Nom as Titre, T.Annee FROM Singles S INNER JOIN Artiste A ON A.Id = S.ArtisteId INNER JOIN SinglesTitres ST ON S.Id = ST.SingleId INNER JOIN Titre T ON T.Id = ST.FaceId");
                }
                else
                {
                    singles = db.Query<SinglesJoinData>("SELECT DISTINCT S.Id, S.Nom, A.Nom as Artiste, T.Nom as Titre, T.Annee FROM Singles S INNER JOIN Artiste A ON A.Id = S.ArtisteId INNER JOIN SinglesTitres ST ON S.Id = ST.SingleId INNER JOIN Titre T ON T.Id = ST.FaceId WHERE T.Nom like '%" + query + "%'");
                }

            }
            return singles;
        }

        public static List<SinglesJoinData> GetAllSinglesTitresByTitreAndArtiste(String query, int artisteId)
        {
            List<SinglesJoinData> singles;
            using (var db = new SQLiteConnection(SQLiteDataService.DbPath))
            {
                if (query.Equals(""))
                {
                    singles = db.Query<SinglesJoinData>("SELECT S.Id, S.Nom, A.Nom as Artiste, T.Nom as Titre, T.Annee FROM Singles S INNER JOIN Artiste A ON A.Id = S.ArtisteId INNER JOIN SinglesTitres ST ON S.Id = ST.SingleId INNER JOIN Titre T ON T.Id = ST.FaceId WHERE A.Id = '" + artisteId + "'");
                }
                else
                {
                    singles = db.Query<SinglesJoinData>("SELECT DISTINCT S.Id, S.Nom, A.Nom as Artiste, T.Nom as Titre, T.Annee FROM Singles S INNER JOIN Artiste A ON A.Id = S.ArtisteId INNER JOIN SinglesTitres ST ON S.Id = ST.SingleId INNER JOIN Titre T ON T.Id = ST.FaceId WHERE T.Nom like '%" + query + "%' AND A.Id = '" + artisteId + "'");
                }
            }
            return singles;
        }
    }
}
