using VinylManager.Models;
using SQLite;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.Services
{
    internal static class SQLiteDataService
    {
        
        public static string dbPath = string.Empty;
        public static string DbPath
        {
            get
            {
                if (string.IsNullOrEmpty(dbPath))
                {
                    dbPath = Path.Combine(
                        Windows.Storage.ApplicationData.Current.LocalFolder.Path,
                        "vinyls.sqlite");
                }
                return dbPath;
            }
        }

        private static void CreateDatabase()
        {
            using (var connection = new SQLiteConnection(DbPath))
            {

                connection.CreateTable<Artiste>();
                connection.CreateTable<Titre>();
                connection.CreateTable<Inventaire>();
                connection.CreateTable<QuatreTitres>();
                connection.CreateTable<Singles>();
                connection.CreateTable<TrenteTroisTours>();
                connection.CreateTable<TrenteTroisToursTitres>();
            }
        }
    }
}
