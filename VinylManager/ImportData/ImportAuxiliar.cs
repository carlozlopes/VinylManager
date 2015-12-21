using VinylManager.Database;
using VinylManager.Models;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.ImportData
{
    public class ImportAuxiliar
    {
        DatabaseAuxiliar databaseAuxiliar = null;
        Artiste currentArtiste = null;
        String currentType = "";
        Titre[] singleFaces = new Titre[2];
        Singles currentSingle = null;
        Titre[] quatreTitresFaces = new Titre[4];
        QuatreTitres current4t = null;
        List<Titre> list33tA = new List<Titre>();
        List<Titre> list33tB = new List<Titre>();
        TrenteTroisTours current33t = null;
        Boolean pendingInsertion = false;
        int counterSingleByArtiste;
        int counter4tByArtiste;
        int counter33tByArtiste;

        private static int INTERPRETE = 1;
        private static int AUTEUR = 0;
        private static int COMPOSITEUR = 0;

        private static readonly String ARTISTE_INCONNUE = "Inconnue";

        private static readonly String SINGLES_PREFIX = "Singles";

        private static readonly String QUARENTE_CINQUE_PREFIX = "45 t";

        private static readonly String QUATRE_PREFIX = "4 titres";

        private static readonly String TRENTE_TROIS_PREFIX = "33 t";

        private static readonly String FACEA = "A";
    
        public async void readCSVFile() {

            databaseAuxiliar = new DatabaseAuxiliar();
            databaseAuxiliar.initDatabase();
            // settings
            var path = @"CSV\export_complete_test.csv";
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            // acquire file
            var file = await folder.GetFileAsync(path);
            var readFile = await Windows.Storage.FileIO.ReadLinesAsync(file);
            foreach (var line in readFile)
            {
                // Debug.WriteLine("" + line.Split(';')[0]);
                decodeLine(line.Split(';'));
            }
            if (pendingInsertion)
            {
                insertPending();
            }
        }

        private String[] decode33tSongs(String songs)
        {
            String[] faces = songs.Split('/');
            int counter = 0;
            foreach (String face in faces)
            {
                String word = face;
                word = word.TrimEnd();
                word = word.TrimStart();
                faces[counter] = word;
                // Debug.WriteLine(word);
                counter++;
            }
            return faces;
        }

        private void decodeLine(String[] values)
        {
            if (!values[0].Equals(currentType) && pendingInsertion)
            {
                insertPending();
            }
            String artistNameDecoded = decodeArtisteName(values[1]);
            if (null == currentArtiste)
            {
                currentArtiste = databaseAuxiliar.insertArtiste(
                    artistNameDecoded, ARTISTE_INCONNUE, INTERPRETE, COMPOSITEUR, AUTEUR);
                if (null == currentArtiste)
                {
                    currentArtiste = databaseAuxiliar.selectArtiste(artistNameDecoded);
                }
                counterSingleByArtiste = 1;
                counter4tByArtiste = 1;
                counter33tByArtiste = 1;
            }
            else if (!currentArtiste.Nom.Equals(artistNameDecoded))
            {
                currentArtiste = databaseAuxiliar.insertArtiste(
                    artistNameDecoded, ARTISTE_INCONNUE, INTERPRETE, COMPOSITEUR, AUTEUR);
                if (null == currentArtiste)
                {
                    currentArtiste = databaseAuxiliar.selectArtiste(artistNameDecoded);
                }
                counterSingleByArtiste = 1;
                counter4tByArtiste = 1;
                counter33tByArtiste = 1;
            }

            if (values[0].Equals(SINGLES_PREFIX) || values[0].Equals(QUARENTE_CINQUE_PREFIX))
            {
                currentType = values[0];
                insertSingle(values);
            }
            else if (values[0].Equals(QUATRE_PREFIX))
            {
                currentType = values[0];
                insert4t(values);
            }
            else if (values[0].Equals(TRENTE_TROIS_PREFIX))
            {
                currentType = values[0];
                insert33t(values);
            }
        }

        private string decodeArtisteName(String name) {
            String[] names = name.Split(' ');
            if (names.Length > 1)
            {
                if (names[names.Length - 1].StartsWith("(") && names[names.Length - 1].EndsWith(")"))
                {
                    String formattedName = names[names.Length - 1].Substring(1, names[names.Length - 1].Length - 2);
                    int i = 0;
                    while (i < names.Length - 1) {
                        formattedName += " " + names[i];
                        i++;
                    }
                    return formattedName;
                }
                else
                {
                    return names[1] + " " + names[0];
                }
            }
            return name;
        }

        private void insertPending()
        {
            if (null != singleFaces[0])
            {
                currentSingle = databaseAuxiliar.insertSingle(SINGLES_PREFIX + counterSingleByArtiste,
                        currentArtiste, singleFaces, counterSingleByArtiste);
                if (null == currentSingle)
                {
                    selectSingle();
                }
                else
                {
                    counterSingleByArtiste++;
                }
                singleFaces = new Titre[2];
                pendingInsertion = false;
            }
            else if (null != quatreTitresFaces[0] && null != quatreTitresFaces[2])
            {
                current4t = databaseAuxiliar.insert4t(QUATRE_PREFIX + counter4tByArtiste,
                        currentArtiste, quatreTitresFaces, counter4tByArtiste);
                if (null == current4t)
                {
                    select4t();
                }
                else
                {
                    counter4tByArtiste++;
                }
                quatreTitresFaces = new Titre[4];
                pendingInsertion = false;
            }
            else if (0 < list33tA.Count && 0 < list33tB.Count)
            {
                current33t = databaseAuxiliar.insert33t(TRENTE_TROIS_PREFIX + counter33tByArtiste,
                        currentArtiste, list33tA, list33tB, counter33tByArtiste);
                counter33tByArtiste++;
                list33tA = new List<Titre>();
                list33tB = new List<Titre>();
                pendingInsertion = false;
            }
            else
            {
                throw new Exception("Ne pending valid case");
            }
        }

        private void insertSingle(String[] single)
        {
            if (null == singleFaces[0] )
            {
                if (single[26].Equals(FACEA))
                {
                    singleFaces[0] = databaseAuxiliar.insertTitre(single[28], single[31]);
                    if (null == singleFaces[0])
                    {
                        singleFaces[0] = databaseAuxiliar.selectTitre(single[28], single[31]);
                    }
                    pendingInsertion = true;
                }
                else
                {
                    throw new Exception("Lonely single Face B");
                }

            }
            else
            {
                if (pendingInsertion && single[26].Equals(FACEA))
                {
                    // Insert single with face A only
                    currentSingle = databaseAuxiliar.insertSingle(
                        SINGLES_PREFIX + counterSingleByArtiste, currentArtiste, singleFaces, counterSingleByArtiste);
                    if (null == currentSingle)
                    {
                        selectSingle();
                    }
                    else
                    {
                        counterSingleByArtiste++;
                    }

                    // insert and return face A
                    singleFaces[0] = databaseAuxiliar.insertTitre(single[28], single[31]);
                    if (null == singleFaces[0])
                    {
                        singleFaces[0] = databaseAuxiliar.selectTitre(single[28], single[31]);
                    }
                }
                else
                {
                    // insert and return face B
                    singleFaces[1] = databaseAuxiliar.insertTitre(single[28], single[31]);
                    if (null == singleFaces[1])
                    {
                        singleFaces[1] = databaseAuxiliar.selectTitre(single[28], single[31]);
                    }
                    
                    // insert single with face A and B
                    currentSingle = databaseAuxiliar.insertSingle(SINGLES_PREFIX + counterSingleByArtiste,
                        currentArtiste, singleFaces, counterSingleByArtiste);
                    if (null == currentSingle)
                    {
                        selectSingle();
                    }
                    else
                    {
                        counterSingleByArtiste++;
                    }
                    // reinitialize values
                    singleFaces = new Titre[2];
                    pendingInsertion = false;
                }
            }
        }

        private void insert4t(String[] quatreT)
        {
            if (quatreT[26].Equals(FACEA))
            {
                if (pendingInsertion && null != quatreTitresFaces[0] && null != quatreTitresFaces[2])
                {
                    // insert pending
                    current4t = databaseAuxiliar.insert4t(QUATRE_PREFIX + counter4tByArtiste,
                        currentArtiste, quatreTitresFaces, counter4tByArtiste);
                    if (null == current4t)
                    {
                        select4t();
                    }
                    else
                    {
                        counter4tByArtiste++;
                    }
                    quatreTitresFaces = new Titre[4];
                    pendingInsertion = false;
                }

                if (null == quatreTitresFaces[0])
                {
                    quatreTitresFaces[0] = databaseAuxiliar.insertTitre(quatreT[28], quatreT[31]);
                    if (null == quatreTitresFaces[0])
                    {
                        quatreTitresFaces[0] = databaseAuxiliar.selectTitre(quatreT[28], quatreT[31]);
                    }
                }
                else if (null == quatreTitresFaces[1])
                {
                    quatreTitresFaces[1] = databaseAuxiliar.insertTitre(quatreT[28], quatreT[31]);
                    if (null == quatreTitresFaces[1])
                    {
                        quatreTitresFaces[1] = databaseAuxiliar.selectTitre(quatreT[28], quatreT[31]);
                    }
                }
                else
                {
                    throw new Exception("3 A faces for 4 titres");
                }
            }
            else
            {
                if (null == quatreTitresFaces[2])
                {
                    quatreTitresFaces[2] = databaseAuxiliar.insertTitre(quatreT[28], quatreT[31]);
                    if (null == quatreTitresFaces[02])
                    {
                        quatreTitresFaces[2] = databaseAuxiliar.selectTitre(quatreT[28], quatreT[31]);
                    }
                    pendingInsertion = true;
                }
                else if (null == quatreTitresFaces[3])
                {
                    quatreTitresFaces[3] = databaseAuxiliar.insertTitre(quatreT[28], quatreT[31]);
                    if (null == quatreTitresFaces[3])
                    {
                        quatreTitresFaces[3] = databaseAuxiliar.selectTitre(quatreT[28], quatreT[31]);
                    }

                    current4t = databaseAuxiliar.insert4t(QUATRE_PREFIX + counter4tByArtiste,
                       currentArtiste, quatreTitresFaces, counter4tByArtiste);
                    if (null == current4t)
                    {
                        select4t();
                    }
                    else
                    {
                        counter4tByArtiste++;
                    }
                    quatreTitresFaces = new Titre[4];
                    pendingInsertion = false;
                }
                else
                {
                    throw new Exception("3 B faces for 4 titres");
                }
            }
        }

        private void insert33t(String[] trente3t)
        {
            if (trente3t[26].Equals(FACEA))
            {
                if (pendingInsertion && 0 < list33tB.Count)
                {
                    current33t = databaseAuxiliar.insert33t(TRENTE_TROIS_PREFIX + counter33tByArtiste,
                        currentArtiste, list33tA, list33tB, counter33tByArtiste);
                    counter33tByArtiste++;
                    list33tA = new List<Titre>();
                    list33tB = new List<Titre>();
                    pendingInsertion = false;

                }
                String[] trimmedFaces = decode33tSongs(trente3t[28]);
                Titre tmp = new Titre();
                foreach (String face in trimmedFaces)
                {
                    tmp = databaseAuxiliar.insertTitre(face, trente3t[31]);
                    if (null == tmp)
                    {
                        tmp = databaseAuxiliar.selectTitre(face, trente3t[31]);
                    }
                    list33tA.Add(tmp);
                }
            }
            else
            {
                String[] trimmedFaces = decode33tSongs(trente3t[28]);
                Titre tmp = new Titre();
                foreach (String face in trimmedFaces)
                {
                    tmp = databaseAuxiliar.insertTitre(face, trente3t[31]);
                    if (null == tmp)
                    {
                        tmp = databaseAuxiliar.selectTitre(face, trente3t[31]);
                    }
                    list33tB.Add(tmp);
                }
                if (0 < list33tA.Count && 0 < list33tB.Count)
                {
                    pendingInsertion = true;
                }
            }
        }

        private void selectSingle()
        {
            if (null != singleFaces[1])
            {
                currentSingle = databaseAuxiliar.selectSingle(currentArtiste.Id,
                singleFaces[0].Id, singleFaces[1].Id);
            }
            else
            {
                currentSingle = databaseAuxiliar.selectSingle(currentArtiste.Id,
                singleFaces[0].Id, -1);
            }
        }

        private void select4t()
        {
            if (null != quatreTitresFaces[1] && null != quatreTitresFaces[3])
            {
                current4t = databaseAuxiliar.selecte4t(currentArtiste.Id,
                    quatreTitresFaces[0].Id, quatreTitresFaces[1].Id, quatreTitresFaces[2].Id,
                    quatreTitresFaces[3].Id);
            }
            else if (null != quatreTitresFaces[1])
            {
                current4t = databaseAuxiliar.selecte4t(currentArtiste.Id,
                    quatreTitresFaces[0].Id, quatreTitresFaces[1].Id, quatreTitresFaces[2].Id,
                    -1);
            }
            else if (null != quatreTitresFaces[3])
            {
                current4t = databaseAuxiliar.selecte4t(currentArtiste.Id,
                    quatreTitresFaces[0].Id, -1, quatreTitresFaces[2].Id,
                    quatreTitresFaces[3].Id);
            }
            else
            {
                current4t = databaseAuxiliar.selecte4t(currentArtiste.Id,
                    quatreTitresFaces[0].Id, -1, quatreTitresFaces[2].Id,
                    -1);
            }
        }
    }

}
