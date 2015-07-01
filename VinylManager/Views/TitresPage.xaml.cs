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
    public sealed partial class TitresPage : Page
    {
        AdminPageViewModel adminPageViewModel = new AdminPageViewModel();
        TitresViewModel titresViewModel = new TitresViewModel();
        TitreViewModel selectedTitre = new TitreViewModel();

        public TitresPage()
        {
            this.InitializeComponent();
        }

        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            TitresListView.DataContext = titresViewModel.Search_Titres_Executed(args.QueryText);
        }

        private void TitresListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTitre = (TitreViewModel)TitresListView.SelectedItem;

            titresViewModel.Select_Childs_Selected_Titre(selectedTitre);
            TitreBorder.DataContext = selectedTitre;

            EditTitre.IsEnabled = true;
            DeleteTitre.IsEnabled = true;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.DeleteTitre.IsEnabled = true;
            TitreViewModel newTitre = new TitreViewModel();
            newTitre.IsInEditMode = true;
            TitreBorder.DataContext = newTitre;
            NewTitre.IsChecked = true;
            EditTitre.IsChecked = false;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            selectedTitre.IsInEditMode = true;
            EditTitre.IsChecked = true;
            NewTitre.IsChecked = false;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Titre face = TitreService.GetTitreById(Convert.ToInt32(Id.Text));
            TitresListView.DataContext = titresViewModel.deleteTitre(face);
            TitreBorder.DataContext = null;
            NewTitre.IsChecked = false;
            EditTitre.IsChecked = false;
            EditTitre.IsEnabled = false;
            DeleteTitre.IsEnabled = false;
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
            resetAll();
        }

        private void CancelButton_Click_1(object sender, RoutedEventArgs e)
        {
            resetAll();
        }

        private void resetAll()
        {
            TitresListView.SelectedIndex = -1;
            NewTitre.IsChecked = false;
            EditTitre.IsChecked = false;
            EditTitre.IsEnabled = false;
            DeleteTitre.IsEnabled = false;
            selectedTitre = null;
            TitreBorder.DataContext = null;
        }
    }
}
