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
    public sealed partial class TitresPage : Page
    {
        AdminPageViewModel adminPageViewModel = new AdminPageViewModel();
        TitresViewModel titresViewModel = new TitresViewModel();
        TitreViewModel selectedTitre = new TitreViewModel();

        public TitresPage()
        {
            this.InitializeComponent();
            desactivateEditControlsAndResetTopBar();
        }

        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            TitresListView.DataContext = titresViewModel.Search_Titres_Executed(args.QueryText);
        }

        private void TitresListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTitre = (TitreViewModel)TitresListView.SelectedItem;

            if (null != selectedTitre)
            {
                titresViewModel.Select_Childs_Selected_Titre(selectedTitre);
                TitreBorder.DataContext = selectedTitre;
                EditTitre.IsEnabled = true;
                DeleteTitre.IsEnabled = true;
            }
            NewTitre.IsChecked = false;
            EditTitre.IsChecked = false;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            TitreViewModel newTitre = new TitreViewModel();
            newTitre.IsInEditMode = true;
            TitreBorder.DataContext = newTitre;
            topButtonBarClicked();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            selectedTitre.IsInEditMode = true;
            topButtonBarClicked();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            topButtonBarClicked();
            MessageDialog message = new MessageDialog("Etes-vous sûr de vouloir effacer le titre sélectionné?");

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

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            Titre titre = new Titre();
            Artiste artiste = new Artiste();
            
            if (true == NewTitre.IsChecked)
            {
                titre.Nom = Nom.Text;
                titre.Annee = Annee.Text;
            }
            else
            {
                titre.Id = Convert.ToInt32(Id.Text);
                titre.Nom = Nom.Text;
                titre.Annee = Annee.Text;
            }

            TitresListView.DataContext = titresViewModel.saveTitre(titre);
            topButtonBarClicked();
            desactivateEditControlsAndResetTopBar();
        }

        private void CancelButton_Click_1(object sender, RoutedEventArgs e)
        {
            desactivateEditControlsAndResetTopBar();
        }

        private void AcceptDeleteEventHandler(IUICommand command)
        {
            Titre titre = TitreService.GetTitreById(Convert.ToInt32(Id.Text));
            TitresListView.DataContext = titresViewModel.deleteTitre(titre);
            desactivateEditControlsAndResetTopBar();
        }

        private void CancelDeleteEventHandler(IUICommand command)
        {
            desactivateEditControlsAndResetTopBar();
        }

        private void topButtonBarClicked()
        {
            if (true == NewTitre.IsChecked)
            {
                NewTitre.IsEnabled = true;
                EditTitre.IsEnabled = false;
                EditTitre.IsChecked = false;
                DeleteTitre.IsEnabled = false;
                activateEditControls();
            }
            else if (true == EditTitre.IsChecked)
            {
                EditTitre.IsEnabled = true;
                NewTitre.IsChecked = false;
                NewTitre.IsEnabled = false;
                DeleteTitre.IsEnabled = false;
                activateEditControls();
            }
            else
            {
                DeleteTitre.IsEnabled = true;
                NewTitre.IsChecked = false;
                NewTitre.IsEnabled = false;
                EditTitre.IsChecked = false;
                EditTitre.IsEnabled = false;
            }
        }

        private void activateEditControls()
        {
            Nom.IsReadOnly = false;
            Annee.IsReadOnly = false;
        }

        private void desactivateEditControlsAndResetTopBar()
        {
            SearchBox.QueryText = "";
            TitresListView.SelectedIndex = -1;
            selectedTitre = null;
            TitreBorder.DataContext = null;
            NewTitre.IsEnabled = true;
            NewTitre.IsChecked = false;
            EditTitre.IsEnabled = false;
            EditTitre.IsChecked = false;
            DeleteTitre.IsEnabled = false;
            Nom.IsReadOnly = true;
            Nom.Text = "";
            Annee.IsReadOnly = true;
            Annee.Text = "";
        }
    }
}
