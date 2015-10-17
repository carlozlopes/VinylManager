using VinylManager.ViewModel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp;
using System.Collections.ObjectModel;
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
        ObservableCollection<FontFamily> titresFonts = new ObservableCollection<FontFamily>();
        ObservableCollection<Int32> titresFontSizes = new ObservableCollection<Int32>();
        ObservableCollection<String> titresFontColors = new ObservableCollection<String>();
        ObservableCollection<FontFamily> artisteFonts = new ObservableCollection<FontFamily>();
        ObservableCollection<Int32> artisteFontSizes = new ObservableCollection<Int32>();
        ObservableCollection<String> artisteFontColors = new ObservableCollection<String>();
        String tFont;
        Int32 tFontSize;
        String tFontColor;
        String aFont;
        Int32 aFontSize;
        String aFontColor;
        
        Font titreFont;
        Font artisteFont;


        public EtiquettesPage()
        {
            this.InitializeComponent();
            initializeFonts();
        }

        private void initializeFonts()
        {
            populateFonts(titresFonts);
            populateFontSizes(titresFontSizes);
            populateFontColors(titresFontColors);

            populateFonts(artisteFonts);
            populateFontSizes(artisteFontSizes);
            populateFontColors(artisteFontColors);


            TitresFontsCombo.DataContext = titresFonts;
            TitresFontsCombo.SelectedIndex = 0;
            TitresFontSizeCombo.DataContext = titresFontSizes;
            TitresFontSizeCombo.SelectedIndex = 4;
            TitresFontsColorCombo.DataContext = titresFontColors;
            TitresFontsColorCombo.SelectedIndex = 1;

            ArtisteFontsCombo.DataContext = artisteFonts;
            ArtisteFontsCombo.SelectedIndex = 0;
            ArtisteFontSizeCombo.DataContext = artisteFontSizes;
            ArtisteFontSizeCombo.SelectedIndex = 6;
            ArtisteFontsColorCombo.DataContext = artisteFontColors;
            ArtisteFontsColorCombo.SelectedIndex = 10;
        }

        private void populateFonts(ObservableCollection<FontFamily> fonts)
        {
            fonts.Add(new FontFamily("Courier"));
            fonts.Add(new FontFamily("Courier-Bold"));
            fonts.Add(new FontFamily("Courier-BoldOblique"));
            fonts.Add(new FontFamily("Courier-Oblique"));
            fonts.Add(new FontFamily("Helvetica"));
            fonts.Add(new FontFamily("Helvetica-Bold"));
            fonts.Add(new FontFamily("Helvetica-BoldOblique"));
            fonts.Add(new FontFamily("Helvetica-Oblique"));
            fonts.Add(new FontFamily("Symbol"));
            fonts.Add(new FontFamily("Times"));
            fonts.Add(new FontFamily("Times-Bold"));
            fonts.Add(new FontFamily("Times-BoldItalic"));
            fonts.Add(new FontFamily("Times-Italic"));
            fonts.Add(new FontFamily("Times-Roman"));
            fonts.Add(new FontFamily("Times-ZapfDingbats"));
        }

        private void populateFontSizes(ObservableCollection<Int32> fontSizes)
        {
            fontSizes.Add(8);
            fontSizes.Add(9);
            fontSizes.Add(10);
            fontSizes.Add(11);
            fontSizes.Add(12);
            fontSizes.Add(13);
            fontSizes.Add(14);
            fontSizes.Add(15);
            fontSizes.Add(16);
            fontSizes.Add(17);
            fontSizes.Add(18);
            fontSizes.Add(19);
            fontSizes.Add(20);
        }

        private void populateFontColors(ObservableCollection<String> fontColors)
        {
            fontColors.Add("BLACK");
            fontColors.Add("BLUE");
            fontColors.Add("CYAN");
            fontColors.Add("DARK_GRAY");
            fontColors.Add("GRAY");
            fontColors.Add("GREEN");
            fontColors.Add("LIGHT_GRAY");
            fontColors.Add("MAGENTA");
            fontColors.Add("ORANGE");
            fontColors.Add("PINK");
            fontColors.Add("RED");
            fontColors.Add("WHITE");
            fontColors.Add("YELLOW");
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

            setFonts();

            if (SinglesEtiquettesListView.Items.Count != 0)
            {
                // Open the document to enable you to write to the document
                document.Open();
                PdfPTable table = new PdfPTable(2);
                // table.DefaultCell.FixedHeight = Utilities.MillimetersToInches(25.4F);
                table.TotalWidth = Utilities.MillimetersToPoints(160);
                int counter = 0;
                foreach (SingleJoinDataViewModel item in SinglesEtiquettesListView.Items)
                {
                    table.AddCell(createEtiquette(item));
                    counter = counter + 1;
                }
                int missingCells = counter % 2;
                if (missingCells == 1)
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
            PdfPCell initialSpace = new PdfPCell();
            PdfPCell titreACell = new PdfPCell(new Phrase(item.TitreA, titreFont));
            titreACell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            titreACell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            PdfPCell titreBCell = new PdfPCell(new Phrase(item.TitreB, titreFont));
            titreBCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            titreBCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            PdfPCell artisteCell = new PdfPCell(new Phrase(item.Artiste, artisteFont));
            artisteCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            artisteCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;

            initialSpace.FixedHeight = Utilities.MillimetersToInches(3.5f);
            titreACell.FixedHeight = Utilities.MillimetersToPoints(6.5f);
            artisteCell.FixedHeight = Utilities.MillimetersToPoints(6.5f);
            titreBCell.FixedHeight = Utilities.MillimetersToPoints(6.5f);
            internalTable.AddCell(initialSpace);
            internalTable.AddCell(titreACell);
            internalTable.AddCell(artisteCell);
            internalTable.AddCell(titreBCell);

            return internalTable;
        }

        private PdfPTable createEmptyEtiquette()
        {
            PdfPTable internalTable = new PdfPTable(1);
            internalTable.TotalWidth = Utilities.MillimetersToPoints(160);
            PdfPCell initialSpace = new PdfPCell();
            PdfPCell titreACell = new PdfPCell();
            PdfPCell titreBCell = new PdfPCell();
            PdfPCell artisteCell = new PdfPCell();
            initialSpace.FixedHeight = Utilities.MillimetersToInches(3.5f);
            titreACell.FixedHeight = Utilities.MillimetersToPoints(6.5f);
            artisteCell.FixedHeight = Utilities.MillimetersToPoints(6.5f);
            titreBCell.FixedHeight = Utilities.MillimetersToPoints(6.5f);
            internalTable.AddCell(initialSpace);
            internalTable.AddCell(titreACell);
            internalTable.AddCell(artisteCell);
            internalTable.AddCell(titreBCell);

            return internalTable;
        }

        private void setFonts()
        {
            titreFont = FontFactory.GetFont(tFont, tFontSize, retrieveBaseColor(tFontColor));
            artisteFont = FontFactory.GetFont(aFont, aFontSize, retrieveBaseColor(aFontColor));
        }

        private BaseColor retrieveBaseColor(String color)
        {
            BaseColor myColor;
            switch (color)
            {
                case "BLACK":
                    myColor = BaseColor.BLACK;
                    break;
                case "BLUE":
                    myColor = BaseColor.BLUE;
                    break;
                case "CYAN":
                    myColor = BaseColor.CYAN;
                    break;
                case "DARK_GRAY":
                    myColor = BaseColor.DARK_GRAY;
                    break;
                case "GRAY":
                    myColor = BaseColor.GRAY;
                    break;
                case "GREEN":
                    myColor = BaseColor.GREEN;
                    break;
                case "LIGHT_GRAY":
                    myColor = BaseColor.LIGHT_GRAY;
                    break;
                case "MAGENTA":
                    myColor = BaseColor.MAGENTA;
                    break;
                case "ORANGE":
                    myColor = BaseColor.ORANGE;
                    break;
                case "PINK":
                    myColor = BaseColor.PINK;
                    break;
                case "RED":
                    myColor = BaseColor.RED;
                    break;
                case "WHITE":
                    myColor = BaseColor.WHITE;
                    break;
                case "YELLOW":
                    myColor = BaseColor.YELLOW;
                    break;
                default:
                    myColor = BaseColor.BLACK;
                    break;
            }
            return myColor;
        } 

        private void FontsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tFont = ((FontFamily)TitresFontsCombo.SelectedItem).Source;
        }

        private void TitresFontSizeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tFontSize = ((Int32)TitresFontSizeCombo.SelectedItem);
        }

        private void TitresFontsColorCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tFontColor = ((String)TitresFontsColorCombo.SelectedItem);
        }

        private void ArtisteFontsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            aFont = ((FontFamily)ArtisteFontsCombo.SelectedItem).Source;
        }

        private void ArtisteFontSizeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            aFontSize = ((Int32)ArtisteFontSizeCombo.SelectedItem);
        }

        private void ArtisteFontsColorCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            aFontColor = ((String)ArtisteFontsColorCombo.SelectedItem);
        }
        

        /* private void TitresColorCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tFontColor = ((BaseColor)Titres)
            // titreFont.SetColor((BaseColor)TitresColorCombo.SelectedIndex);
        } */
    }
}
