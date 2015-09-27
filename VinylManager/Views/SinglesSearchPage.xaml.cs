using VinylManager.MVVM;
using VinylManager.ViewModel;
using VinylManager.Services;
using VinylManager.Models;
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
    public sealed partial class SinglesSearchPage : Page
    {
        SinglesSearchPageViewModel singlesSearchPageViewModel = new SinglesSearchPageViewModel();
        ArtisteViewModel selectedArtiste;
        SinglesViewModel singlesViewModel = new SinglesViewModel();
        SingleViewModel selectedSingle = new SingleViewModel();
        SingleJoinDataViewModel tempSingle = new SingleJoinDataViewModel();

        public SinglesSearchPage()
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
                SinglesListView.DataContext = singlesViewModel.Search_Singles_Titres_ByUser_Executed(args.QueryText, selectedArtiste.Id);
            }
            else
            {
                SinglesListView.DataContext = singlesViewModel.Search_Singles_Titres_Executed(args.QueryText);
            }
        }

        private void ArtisteSearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            ArtistesListView.DataContext = singlesSearchPageViewModel.Search_Artistes_Executed(args.QueryText);
        }

        private void SinglesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tempSingle = (SingleJoinDataViewModel)SinglesListView.SelectedItem;

            if (null != tempSingle)
            {
                /* selectedSingle = singlesViewModel.Select_Childs_Selected_Single(tempSingle.Id);

                SingleBorder.DataContext = selectedSingle;
                FaceA.Text = selectedSingle.FaceA.Nom;
                AnneeFaceA.Text = selectedSingle.FaceA.Annee;
                FaceB.Text = selectedSingle.FaceB.Nom;
                AnneeFaceB.Text = selectedSingle.FaceB.Annee;
                Artiste.Text = selectedSingle.Artiste.Nom; */
            }
        }
    }
}
