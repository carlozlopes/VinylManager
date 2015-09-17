﻿using VinylManager.MVVM;
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
using Windows.UI.Xaml.Media.Imaging;

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
        BitmapImage biUpGrayArrow;
        BitmapImage biDownGrayArrow;
        BitmapImage biUpOrangeArrow;
        BitmapImage biDownOrangeArrow;
        Image currentSelectedArrow = null;

        public SinglesSearchPage()
        {
            this.InitializeComponent();
            /* biUpGrayArrow = new BitmapImage(new Uri(@"/Images/up-circular-gray.png", UriKind.Relative));
            biDownGrayArrow = new BitmapImage(new Uri(@"/Images/down-circular-gray.png", UriKind.Relative));
            biUpOrangeArrow = new BitmapImage(new Uri(@"/Images/up-circular-orange.png", UriKind.Relative));
            biDownOrangeArrow = new BitmapImage(new Uri(@"/Images/down-circular-orange.png", UriKind.Relative)); */

            biUpGrayArrow = new BitmapImage(new Uri("ms-appx:///Images/up-circular-gray.png", UriKind.Absolute));
            biDownGrayArrow = new BitmapImage(new Uri("ms-appx:///Images/down-circular-gray.png", UriKind.Absolute));
            biUpOrangeArrow = new BitmapImage(new Uri("ms-appx:///Images/up-circular-orange.png", UriKind.Absolute));
            biDownOrangeArrow = new BitmapImage(new Uri("ms-appx:///Images/down-circular-orange.png", UriKind.Absolute));
        }

        private void SelectArtisteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!SelectArtistePopUp.IsOpen)
            {
                ArtisteSearchBox.QueryText = ArtisteInput.Text;
                ArtistesListView.DataContext = singlesSearchPageViewModel.Search_Artistes_Executed(ArtisteInput.Text);

                SelectArtistePopUpBorder.Width = 650;
                SelectArtistePopUp.HorizontalOffset = Window.Current.Bounds.Width - 1000;
                SelectArtistePopUp.VerticalOffset = Window.Current.Bounds.Height - 750;
                SelectArtistePopUp.IsOpen = true;
            }
        }

        private void SelectArtistePopUp_Closed(object sender, object e)
        {
            ArtisteInput.Text = selectedArtiste.Nom;
        }

        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
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

        private void ArtistesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedArtiste = (ArtisteViewModel)ArtistesListView.SelectedItem;
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

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            swapSelectedArrows(sender as Image);
        }

        private void swapSelectedArrows(Image img)
        {
            if (null == currentSelectedArrow)
            {
                if (img.Name.Contains("up"))
                {
                    img.Source = biUpOrangeArrow;
                }
                else
                {
                    img.Source = biDownOrangeArrow;
                }
            }
            else if (currentSelectedArrow.Name.Contains("up"))
            {
                currentSelectedArrow.Source = biUpGrayArrow;
                if (img.Name.Contains("up"))
                {
                    img.Source = biUpOrangeArrow;
                }
                else
                {
                    img.Source = biDownOrangeArrow;
                }
            } 
            else if (currentSelectedArrow.Name.Contains("down"))
            {
                currentSelectedArrow.Source = biDownGrayArrow;
                if (img.Name.Contains("up"))
                {
                    img.Source = biUpOrangeArrow;
                }
                else
                {
                    img.Source = biDownOrangeArrow;
                }
            }
            currentSelectedArrow = img;
        }
    }
}
