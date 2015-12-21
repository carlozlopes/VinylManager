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
using Windows.UI.Popups;
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
    public sealed partial class ArtistesPage : Page
    {
        AdminPageViewModel adminPageViewModel = new AdminPageViewModel();
        ArtisteViewModel selectedArtiste = new ArtisteViewModel();
        
        public ArtistesPage()
        {
            this.InitializeComponent();
            desactivateEditControlsAndResetTopBar();
        }

        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            ArtistesListView.DataContext = adminPageViewModel.Search_Artistes_Executed(args.QueryText);
        }

        private void ArtistesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {          
            selectedArtiste = (ArtisteViewModel)ArtistesListView.SelectedItem;
            if (null != selectedArtiste)
            {
                adminPageViewModel.Select_Childs_Selected_Artiste(selectedArtiste);
                selectQualites(selectedArtiste);
                ArtisteBorder.DataContext = selectedArtiste;
                EditArtiste.IsEnabled = true;
                DeleteArtiste.IsEnabled = true;
            }
            NewArtiste.IsChecked = false;
            EditArtiste.IsChecked = false;
        }

        private void selectQualites(ArtisteViewModel selectedArtiste)
        {
            interpreteRadioButton.IsChecked = false;
            auteurRadioButton.IsChecked = false;
            compositeurRadioButton.IsChecked = false;

            if (1 == selectedArtiste.Interprete)
            {
                interpreteRadioButton.IsChecked = true;   
            }
            if (1 == selectedArtiste.Auteur)
            {
                auteurRadioButton.IsChecked = true;
            }
            if (1 == selectedArtiste.Compositeur)
            {
                compositeurRadioButton.IsChecked = true;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ArtisteViewModel newArtiste = new ArtisteViewModel();
            newArtiste.IsInEditMode = true;
            interpreteRadioButton.IsChecked = false;
            compositeurRadioButton.IsChecked = false;
            auteurRadioButton.IsChecked = false;
            ArtisteBorder.DataContext = newArtiste;
            topButtonBarClicked();

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            selectedArtiste.IsInEditMode = true;
            topButtonBarClicked();
        }

        private async void DeleteArtiste_Click(object sender, RoutedEventArgs e)
        {
            topButtonBarClicked();
            MessageDialog message = new MessageDialog("Etes-vous sûr de vouloir effacer l'artiste sélectionné?");

            message.Commands.Add(new UICommand(
                "OUI",
                new UICommandInvokedHandler(this.AcceptDeleteEventHandler)));

            message.Commands.Add(new UICommand(
                "NON",
                new UICommandInvokedHandler(this.CancelDeleteEventHandler)));

            // Set the command that will be invoked by default
            message.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            message.CancelCommandIndex = 1;

            await message.ShowAsync();
        }

        private void topButtonBarClicked()
        {
            if (true == NewArtiste.IsChecked)
            {
                NewArtiste.IsEnabled = true;
                EditArtiste.IsEnabled = false;
                DeleteArtiste.IsEnabled = false;
                activateEditControls();
            }
            else if (true == EditArtiste.IsChecked)
            {
                EditArtiste.IsEnabled = true;
                NewArtiste.IsChecked = false;
                NewArtiste.IsEnabled = false;
                DeleteArtiste.IsEnabled = false;
                activateEditControls();
            }
            else
            {
                DeleteArtiste.IsEnabled = true;
                NewArtiste.IsChecked = false;
                NewArtiste.IsEnabled = false;
                EditArtiste.IsChecked = false;
                EditArtiste.IsEnabled = false;
            }
        }

        private void AcceptDeleteEventHandler(IUICommand command)
        {
            Artiste artiste = ArtisteService.GetArtisteById(Convert.ToInt32(Id.Text));
            ArtistesListView.DataContext = adminPageViewModel.deleteArtiste(artiste);
            desactivateEditControlsAndResetTopBar();
        }

        private void CancelDeleteEventHandler(IUICommand command)
        {
            desactivateEditControlsAndResetTopBar();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (checkAtLeastOneQualiteSelected())
            {
                Artiste artiste = new Artiste();

                if (true == NewArtiste.IsChecked)
                {
                    artiste.Nom = Nom.Text;
                    artiste.Nationalite = Nationalite.Text;

                }
                else
                {
                    artiste.Id = Convert.ToInt32(Id.Text);
                    artiste.Nom = Nom.Text;
                    artiste.Nationalite = Nationalite.Text;
                }

                addQualitesArtiste(artiste);
                SearchBox.QueryText = "";
                ArtistesListView.DataContext = adminPageViewModel.saveArtiste(artiste);
                updateTopButtonBar();
            }
        }

        private void CancelButton_Click_1(object sender, RoutedEventArgs e)
        {
            desactivateEditControlsAndResetTopBar();
        }

        private void activateEditControls()
        {
            Nom.IsReadOnly = false;
            Nationalite.IsReadOnly = false;
            interpreteRadioButton.IsEnabled = true;
            compositeurRadioButton.IsEnabled = true;
            auteurRadioButton.IsEnabled = true;
        }

        private void desactivateEditControlsAndResetTopBar()
        {
            NewArtiste.IsEnabled = true;
            NewArtiste.IsChecked = false;
            EditArtiste.IsEnabled = false;
            EditArtiste.IsChecked = false;
            DeleteArtiste.IsEnabled = false;
            Nom.IsReadOnly = true;
            Nom.Text = "";
            Nationalite.IsReadOnly = true;
            Nationalite.Text = "";
            interpreteRadioButton.IsEnabled = false;
            interpreteRadioButton.IsChecked = false;
            compositeurRadioButton.IsEnabled = false;
            compositeurRadioButton.IsChecked = false;
            auteurRadioButton.IsEnabled = false;
            auteurRadioButton.IsChecked = false;
        }

        private void updateTopButtonBar()
        {
            if (true == NewArtiste.IsChecked)
            {
                NewArtiste.IsChecked = false;
            }
            else if (true == EditArtiste.IsChecked)
            {
                EditArtiste.IsChecked = false;
                EditArtiste.IsEnabled = false;
            }
            else
            {
                DeleteArtiste.IsEnabled = false;
            }
            NewArtiste.IsEnabled = true;
        }

        private Boolean checkAtLeastOneQualiteSelected()
        {
            if (true == interpreteRadioButton.IsChecked || true == auteurRadioButton.IsChecked || true == compositeurRadioButton.IsChecked)
            {
                return true;
            }
            else
            {
                selectAtLeastOneQualiteMessage();
                return false;
            }
        }

        private async void selectAtLeastOneQualiteMessage()
        {
            MessageDialog message = new MessageDialog("Merci de séléctionner au moins une qualité");
            await message.ShowAsync();
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            
        }

        private void addQualitesArtiste(Artiste artiste)
        {
            if (true == interpreteRadioButton.IsChecked)
            {
                artiste.interprete = 1;
            }

            if (true == auteurRadioButton.IsChecked)
            {
                artiste.auteur = 1;
            }

            if (true == compositeurRadioButton.IsChecked)
            {
                artiste.compositeur = 1;
            }
        }
    }
}
