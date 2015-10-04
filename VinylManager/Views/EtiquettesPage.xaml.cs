using VinylManager.ViewModel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.System;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VinylManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EtiquettesPage : NavigationPage
    {
        SinglesViewModel SinglesViewModel = new SinglesViewModel();
        SinglesSearchPageViewModel singlesSearchPageViewModel = new SinglesSearchPageViewModel();
        ArtisteViewModel selectedArtiste;
        List<SingleJoinDataViewModel> SinglesToEtiquettes = new List<SingleJoinDataViewModel>();
        string path = @"Etiquettes.pdf";
        // StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        StorageFolder storageFolder = ApplicationData.Current.TemporaryFolder;

        public EtiquettesPage()
        {
            this.InitializeComponent();
        }

        private void Artiste_Search_Box_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            if (!SelectArtistePopUp.IsOpen)
            {
                ArtisteSearchBox.QueryText = args.QueryText;
                ArtistesListView.DataContext = singlesSearchPageViewModel.Search_Artistes_Executed(args.QueryText);

                SelectArtistePopUpBorder.Width = 650;
                SelectArtistePopUp.HorizontalOffset = Window.Current.Bounds.Width - 1000;
                SelectArtistePopUp.VerticalOffset = Window.Current.Bounds.Height - 750;
                SelectArtistePopUp.IsOpen = true;
            }
        }

        private void ArtistesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedArtiste = (ArtisteViewModel)ArtistesListView.SelectedItem;
        }

        private void SelectArtistePopUp_Closed(object sender, object e)
        {
            if (null != selectedArtiste)
            {
                Artiste_Search_Box.QueryText = selectedArtiste.Nom;
            }
        }

        private void Titre_Search_Box_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            if (null != selectedArtiste)
            {
                SinglesListView.DataContext = SinglesViewModel.Search_Singles_Titres_ByUser_Executed(args.QueryText, selectedArtiste.Id);
            }
            else
            {
                SinglesListView.DataContext = SinglesViewModel.Search_Singles_Titres_Executed(args.QueryText);
            }
        }

        private void ArtisteSearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {

        }

        private void SinglesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SinglesEtiquettesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            // singlesToEtiquettes
            if (SinglesListView.SelectedItems.Count != 0)
            {
                bool alreadyInEtiquettesList;
                foreach (SingleJoinDataViewModel item in SinglesListView.SelectedItems)
                {
                    alreadyInEtiquettesList = false;
                    foreach (SingleJoinDataViewModel addedItems in SinglesEtiquettesListView.Items)
                    {
                        if (addedItems.Id == item.Id)
                        {
                            alreadyInEtiquettesList = true;
                        }
                    }
                    if (!alreadyInEtiquettesList)
                    {
                        SinglesToEtiquettes.Add(item);
                    }
                }
            }
            SinglesEtiquettesListView.DataContext = null;
            SinglesEtiquettesListView.DataContext = SinglesToEtiquettes;
        }

        private void Delete_Selected_Singles_Click(object sender, RoutedEventArgs e)
        {
            if (SinglesEtiquettesListView.SelectedItems.Count != 0)
            {
                foreach (SingleJoinDataViewModel item in SinglesEtiquettesListView.SelectedItems)
                {
                    SinglesToEtiquettes.Remove(item);
                }
                SinglesEtiquettesListView.DataContext = null;
                SinglesEtiquettesListView.DataContext = SinglesToEtiquettes;
            }
        }

        private void Generer_Etiquettes_Click(object sender, RoutedEventArgs e)
        {
            createEtiquettes(SinglesEtiquettesListView.Items);
        }

        private async void createEtiquettes(ItemCollection singlesForEtiquettes)
        {
            // Create empty PDF file.
            bool exists = false;
            StorageFile file = null;
            try
            {
                file = await storageFolder.CreateFileAsync(path, CreationCollisionOption.FailIfExists);
            }
            catch (Exception alreadyExistsException)
            {
                exists = true;
            }
            if (exists == true)
            {
                file = await storageFolder.CreateFileAsync(path, CreationCollisionOption.OpenIfExists);
                StorageFile sampleFileToDelete = await storageFolder.GetFileAsync(path);
                await sampleFileToDelete.DeleteAsync();
                file = await storageFolder.CreateFileAsync(path, CreationCollisionOption.OpenIfExists);
            }

            // Open to PDF file for read/write.
            StorageFile sampleFile = await storageFolder.GetFileAsync(file.Name);
            var stream = await sampleFile.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
            
            // Create an instance of the document class which represents the PDF document itself.
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);

            // Create an instance to the PDF file by creating an instance of the PDF 
            // Writer class using the document and the filestrem in the constructor.
            PdfWriter writer = PdfWriter.GetInstance(document, stream.AsStream());

            // Add meta information to the document
            document.AddAuthor("Gilbert Rouchard");
            document.AddCreator("Carlos' Software");
            document.AddKeywords("PDF Etiquettes");
            // document.AddSubject("");
            document.AddTitle("Etiquettes pour jukebox.");

            if (SinglesEtiquettesListView.Items.Count != 0)
            {
                // Open the document to enable you to write to the document
                document.Open();
                PdfPTable table = new PdfPTable(3);
                int counter = 0;
                foreach (SingleJoinDataViewModel item in SinglesEtiquettesListView.Items)
                {
                    table.AddCell(createEtiquette(item));
                    counter = counter + 1;
                }
                int missingCells = counter % 3;
                if (missingCells == 1)
                {
                    table.AddCell(createEmptyEtiquette());
                    table.AddCell(createEmptyEtiquette());
                }
                else if (missingCells == 2)
                {
                    table.AddCell(createEmptyEtiquette());
                }
                document.Add(table);
            }
            // Close the document
            document.Close();
            // Close the writer instance
            writer.Close();
            await Launcher.LaunchFileAsync(sampleFile);
            // await sampleFile.DeleteAsync();
        }

        private PdfPTable createEtiquette(SingleJoinDataViewModel item)
        {
            PdfPTable internalTable = new PdfPTable(1);
            PdfPCell titreACell = new PdfPCell(new Phrase(item.TitreA));
            PdfPCell titreBCell = new PdfPCell(new Phrase(item.TitreB));
            PdfPCell artisteCell = new PdfPCell(new Phrase(item.Artiste));
            titreACell.FixedHeight = 20;
            artisteCell.FixedHeight = 40;
            titreBCell.FixedHeight = 20;
            internalTable.AddCell(titreACell);
            internalTable.AddCell(artisteCell);
            internalTable.AddCell(titreBCell);

            return internalTable;
        }

        private PdfPTable createEmptyEtiquette()
        {
            PdfPTable internalTable = new PdfPTable(1);
            PdfPCell titreACell = new PdfPCell();
            PdfPCell titreBCell = new PdfPCell();
            PdfPCell artisteCell = new PdfPCell();
            titreACell.FixedHeight = 20;
            artisteCell.FixedHeight = 40;
            titreBCell.FixedHeight = 20;
            internalTable.AddCell(titreACell);
            internalTable.AddCell(artisteCell);
            internalTable.AddCell(titreBCell);

            return internalTable;
        }
    }
}
