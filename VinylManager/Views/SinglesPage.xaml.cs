using VinylManager.MVVM;
using VinylManager.ViewModel;
using VinylManager.Services;
using VinylManager.Models;
using System.Windows;
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
    public sealed partial class SinglesPage : Page
    {
        AdminPageViewModel adminPageViewModel = new AdminPageViewModel();
        ArtisteViewModel selectedArtiste;
        ArtisteViewModel selectedSingleArtiste;
        SinglesViewModel singlesViewModel = new SinglesViewModel();
        SingleViewModel selectedSingle = new SingleViewModel();
        SingleJoinDataViewModel tempSingle = new SingleJoinDataViewModel();
        TitresViewModel titresViewModel = new TitresViewModel();
        InventaireSingleViewModel selectedInventaire = new InventaireSingleViewModel();
        InventaireSinglesViewModel inventaireSinglesViewModel =
            new InventaireSinglesViewModel();


        private bool selectSingleArtisteClicked = false;

        public SinglesPage()
        {
            this.InitializeComponent();
            desactivateFieldsAndButtons();

        }

        private void Titre_Search_Box_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            if (null != selectedArtiste)
            {
                SinglesListView.DataContext = singlesViewModel.Search_Singles_ByUser_Executed(args.QueryText, selectedArtiste.Id);
            }
            else
            {
                SinglesListView.DataContext = singlesViewModel.Search_Singles_Executed(args.QueryText);
            }
        }

        private void SinglesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tempSingle = (SingleJoinDataViewModel)SinglesListView.SelectedItem;
            if (null != tempSingle)
            {
                DeleteSingle.IsEnabled = true;
                selectedSingle = singlesViewModel.Select_Childs_Selected_Single(tempSingle.Id);
                inventaireListView.DataContext =
                    inventaireSinglesViewModel.Select_Inventary_From_SingleId(tempSingle.Id);

                SingleBorder.DataContext = selectedSingle;
                SelectArtisteInput.Text = selectedSingle.Artiste.Nom;
                FaceA.Text = selectedSingle.FaceA.Nom;
                AnneeFaceA.Text = selectedSingle.FaceA.Annee;
                if (null != selectedSingle.FaceB)
                {
                    FaceB.Text = selectedSingle.FaceB.Nom;
                    AnneeFaceB.Text = selectedSingle.FaceB.Annee;
                }

                EditSingle.IsEnabled = true;
                DeleteSingle.IsEnabled = true;
                NewVinyl.IsEnabled = true;
                desactivateFieldsAndButtons();
            }
            else
            {
                DeleteSingle.IsEnabled = false;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NewSingle.IsChecked = true;
            NewSingle.IsEnabled = true;
            EditSingle.IsEnabled = false;
            EditSingle.IsChecked = false;
            DeleteSingle.IsEnabled = false;
            cleanFields();
            activateCommonFieldsAndButtons();
            activateAddFieldsAndButtons();
            SinglesListView.DataContext = null;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditSingle.IsChecked = true;
            EditSingle.IsEnabled = true;
            NewSingle.IsChecked = false;
            NewSingle.IsEnabled = false;
            DeleteSingle.IsEnabled = false;
            activateCommonFieldsAndButtons();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog message = new MessageDialog("Etes-vous sûr de vouloir effacer le single sélectionné?");

            message.Commands.Add(new UICommand(
                "OK",
                new UICommandInvokedHandler(this.AcceptDeleteSingleEventHandler)));

            message.Commands.Add(new UICommand(
                "NO",
                new UICommandInvokedHandler(this.CancelDeleteSingleEventHandler)));

            // Set the command that will be invoked by default
            message.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            message.CancelCommandIndex = 1;

            await message.ShowAsync();
        }

        private void AcceptDeleteSingleEventHandler(IUICommand command)
        {
            Singles single = SinglesService.GetSingleById(Convert.ToInt32(selectedSingle.Id));
            InventaireService.DeleteAllInventairesOfSingle(Convert.ToInt32(single.Id));
            SinglesListView.DataContext = singlesViewModel.deleteSingle(single);
            cleanFields();
        }

        private void CancelDeleteSingleEventHandler(IUICommand command)
        {

        }

        private void deleteSingle()
        {
            Singles single = new Singles();
            single.Id = selectedSingle.Id;
            singlesViewModel.deleteSingle(single);
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            Singles single = new Singles();
            if (checksBeforeSaveOrEdit())
            {
                if (true == NewSingle.IsChecked)
                {
                    single.Nom = Nom.Text;
                    single.ArtisteId = selectedSingleArtiste.Id;
                }
                else
                {
                    single.Id = selectedSingle.Id;
                    single.Nom = selectedSingle.Nom;
                    single.ArtisteId = selectedSingle.Artiste.Id;
                    single.FaceAId = selectedSingle.FaceAId;
                    single.FaceBId = selectedSingle.FaceBId;
                }

                single.FaceA = FaceA.Text;
                single.FaceB = FaceB.Text;

                Titre faceA = new Titre();
                faceA.Nom = FaceA.Text;
                faceA.Annee = AnneeFaceA.Text;
                Titre faceB = new Titre();
                faceB.Nom = FaceB.Text;
                faceB.Annee = AnneeFaceB.Text;
                single.Faces = new List<Titre>();
                single.Faces.Add(faceA);
                single.Faces.Add(faceB);

                SinglesListView.DataContext = singlesViewModel.saveSingle(single);
                cleanFields();

            }
        }

        private Boolean checksBeforeSaveOrEdit() {
            if (null == selectedSingleArtiste && false == EditSingle.IsEnabled)
            {
                showMessageDialog("Artiste non sélectionné");
                return false;
            }
            if ("".Equals(FaceA.Text))
            {
                showMessageDialog("Face A non sélectionnée");
                return false;
            }
            if ("".Equals(FaceB.Text))
            {
                showMessageDialog("Face B non sélectionnée");
                return false;
            }

            return true;
        }

        private async void showMessageDialog(String text)
        {
            MessageDialog message = new MessageDialog(text);
            await message.ShowAsync();
        }

        private void CancelButton_Click_1(object sender, RoutedEventArgs e)
        {
            activateCommonFieldsAndButtons();
        }

        private void desactivateFieldsAndButtons()
        {
            FaceA.IsReadOnly = true;
            AnneeFaceA.IsReadOnly = true;
            FaceB.IsReadOnly = true;
            AnneeFaceB.IsReadOnly = true;
            SelectArtisteInput.IsReadOnly = true;
            AddArtistToSingleButton.IsEnabled = false;
            SaveSingle.IsEnabled = false;
            ClearSingleInfo.IsEnabled = false;
            
        }

        private void activateAddFieldsAndButtons()
        {
            SelectArtisteInput.IsReadOnly = false;
            AddArtistToSingleButton.IsEnabled = true;
        }

        private void activateCommonFieldsAndButtons()
        {
            FaceA.IsReadOnly = false;
            AnneeFaceA.IsReadOnly = false;
            FaceB.IsReadOnly = false;
            AnneeFaceB.IsReadOnly = false;
            SaveSingle.IsEnabled = true;
            ClearSingleInfo.IsEnabled = true;
        }

        private void cleanFields()
        {
            Nom.Text = "";
            FaceA.Text = "";
            AnneeFaceA.Text = "";
            FaceB.Text = "";
            AnneeFaceB.Text = "";
            SelectArtisteInput.Text = "";
            selectedSingleArtiste = null;
            selectSingleArtisteClicked = false;
            selectedInventaire = null;
            inventaireListView.DataContext = null;
        }

        private void activateEditAndDeleteVynil()
        {
            EditVinyl.IsEnabled = true;
            DeleteVinyl.IsEnabled = true;
        }

        private void deactivateVynilButtons()
        {
            NewVinyl.IsEnabled = false;
            EditVinyl.IsEnabled = false;
            DeleteVinyl.IsEnabled = false;
        }

        private void Artiste_Search_Box_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            ArtistesListView.DataContext = null;
            selectSingleArtisteClicked = false;
            if (!SelectArtistePopUp.IsOpen)
            {
                ArtisteSearchBox.QueryText = args.QueryText;
                ArtistesListView.DataContext = adminPageViewModel.Search_Artistes_Executed(args.QueryText);

                SelectArtistePopUpBorder.Width = 650;
                SelectArtistePopUp.HorizontalOffset = Window.Current.Bounds.Width - 1000;
                SelectArtistePopUp.VerticalOffset = Window.Current.Bounds.Height - 750;
                SelectArtistePopUp.IsOpen = true;
            }
        }

        private void ArtistesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectSingleArtisteClicked)
            {
                selectedSingleArtiste = (ArtisteViewModel)ArtistesListView.SelectedItem;
            }
            else
            {
                selectedArtiste = (ArtisteViewModel)ArtistesListView.SelectedItem;
            }
        }

        private void ArtisteSearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            ArtistesListView.DataContext = adminPageViewModel.Search_Artistes_Executed(args.QueryText);
        }

        private void SelectArtistePopUp_Closed(object sender, object e)
        {
            if (selectSingleArtisteClicked)
            {
                if (null != selectedSingleArtiste)
                {
                    SelectArtisteInput.Text = selectedSingleArtiste.Nom;
                    int nextSingleValue = selectedSingleArtiste.SingleCounter + 1;
                    Nom.Text = "single" + nextSingleValue;
                }
            }
            else
            {
                if (null != selectedArtiste)
                {
                    Artiste_Search_Box.QueryText = selectedArtiste.Nom;
                }
            }
        }

        private void AddArtistToSingleButton_Click(object sender, RoutedEventArgs e)
        {
            selectSingleArtisteClicked = true;
            if (!SelectArtistePopUp.IsOpen)
            {
                ArtisteSearchBox.QueryText = SelectArtisteInput.Text;
                ArtistesListView.DataContext = 
                    adminPageViewModel.Search_Artistes_Executed(Artiste_Search_Box.QueryText);

                SelectArtistePopUpBorder.Width = 650;
                SelectArtistePopUp.HorizontalOffset = Window.Current.Bounds.Width - 1000;
                SelectArtistePopUp.VerticalOffset = Window.Current.Bounds.Height - 750;
                SelectArtistePopUp.IsOpen = true;
            }

        }

        private void inventaireListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activateEditAndDeleteVynil();
            selectedInventaire = (InventaireSingleViewModel)inventaireListView.SelectedItem;
        }

        private void NewVinyl_Click(object sender, RoutedEventArgs e)
        {
            if (!CreateEditInventairePopUp.IsOpen)
            {
                CreateEditInventairePopUpBorder.Width = 400;
                CreateEditInventairePopUp.HorizontalOffset = Window.Current.Bounds.Width - 1000;
                CreateEditInventairePopUp.VerticalOffset = Window.Current.Bounds.Height - 530;
                CreateEditInventairePopUp.IsOpen = true;
            }
        }

        private void EditVinyl_Click(object sender, RoutedEventArgs e)
        {
            if (!CreateEditInventairePopUp.IsOpen)
            {
                CreateEditInventairePopUpBorder.Width = 400;
                CreateEditInventairePopUp.HorizontalOffset = Window.Current.Bounds.Width - 1000;
                CreateEditInventairePopUp.VerticalOffset = Window.Current.Bounds.Height - 530;
                EtatVynilCB.SelectedIndex = EtatVynilCB.Items.IndexOf(selectedInventaire.Etat);
                EtatVynilCB.SelectedValue = selectedInventaire.Etat;
                CouleurVynilCB.SelectedValue = selectedInventaire.Couleur;
                EtatPochetteCB.SelectedValue = selectedInventaire.EtatPochette;
                CreateEditInventairePopUp.IsOpen = true;
            }
        }

        private async void DeleteVinyl_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog message = new MessageDialog("Etes-vous sûr de vouloir effacer le vynil sélectionné");

            message.Commands.Add(new UICommand(
                "OK",
                new UICommandInvokedHandler(this.AcceptDeleteEventHandler)));

            message.Commands.Add(new UICommand(
                "NO",
                new UICommandInvokedHandler(this.CancelDeleteEventHandler)));

            // Set the command that will be invoked by default
            message.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            message.CancelCommandIndex = 1;

            await message.ShowAsync();
        }

        private void AcceptDeleteEventHandler(IUICommand command)
        {
            Inventaire vynil = InventaireService.GetInventaireById(Convert.ToInt32(selectedInventaire.Id));
            inventaireListView.DataContext = inventaireSinglesViewModel.deleteVynil(vynil);
        }

        private void CancelDeleteEventHandler(IUICommand command)
        {
            
        }

        private void CreateEditInventairePopUp_Closed(object sender, object e)
        {
            CreateEditInventairePopUp.IsOpen = false;

        }

        private void SaveInventary_Click(object sender, RoutedEventArgs e)
        {
            Inventaire vynil = new Inventaire();
            vynil.DisqueId = selectedSingle.Id;
            vynil.Etat = EtatVynilCB.SelectedValue.ToString();
            vynil.Couleur = CouleurVynilCB.SelectedValue.ToString();
            vynil.EtatPochette = EtatPochetteCB.SelectedValue.ToString();

            inventaireListView.DataContext = inventaireSinglesViewModel.saveVynil(vynil);
        }

        private void CancelInventary_Click(object sender, RoutedEventArgs e)
        {
            CreateEditInventairePopUp.IsOpen = false;
        }

        
    }
}
