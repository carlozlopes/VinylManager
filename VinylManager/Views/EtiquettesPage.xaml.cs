using VinylManager.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
                    // ShoppingCart.Text += item.Title + ", ";
                }
                // ShoppingCart.Text = ShoppingCart.Text.TrimEnd(charsToTrim);
                // ShoppingCart.Text += " added to cart";
            }
            SinglesEtiquettesListView.DataContext = null;
            SinglesEtiquettesListView.DataContext = SinglesToEtiquettes;
        }
    }
}
