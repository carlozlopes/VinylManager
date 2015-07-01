using VinylManager.Services;
using VinylManager.MVVM;
using VinylManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VinylManager.ViewModel
{
    internal class AdminPageViewModel : ViewModelBase
    {
        private DelegateCommand searchArtistesCommand;
        private DelegateCommand newCommand;
        private ObservableCollection<ArtisteViewModel> artistes = new ObservableCollection<ArtisteViewModel>();
        // private ArtisteViewModel selectedArtiste = null;
        private bool hasSelection = false;

        public AdminPageViewModel()
        {
            this.searchArtistesCommand = new DelegateCommand(this.Search_Artistes_Executed);
        }

        public ObservableCollection<ArtisteViewModel> Artistes
        {
            get { return this.artistes; }
            set { this.SetProperty(ref this.artistes, value); }
        }

        public bool HasSelection
        {
            get { return this.hasSelection; }
            private set { this.SetProperty(ref this.hasSelection, value); }
        }
        
        public ICommand SearchArtistesCommand
        {
            get { return this.searchArtistesCommand; }
        }

        /* public ArtisteViewModel SelectedArtiste
        {
            get
            {
                this.selectedArtiste.IsInEditMode = true;
                return this.selectedArtiste;
            }
            set
            {
                this.SetProperty(ref this.selectedArtiste, value);
                this.HasSelection = this.selectedArtiste != null;
            }
        } */
        
        private void Search_Artistes_Executed()
        {
            List<Artiste> models = ArtisteService.GetAllArtistes();

            this.artistes.Clear();
            foreach (var m in models)
            {
                this.artistes.Add(new ArtisteViewModel(m));
            }
        }

        public ObservableCollection<ArtisteViewModel> Search_Artistes_Executed(String query)
        {
            List<Artiste> models = ArtisteService.GetAllArtistesByGivenQuery(query);

            this.artistes.Clear();
            foreach (var m in models)
            {
                this.artistes.Add(new ArtisteViewModel(m));
            }

            return this.artistes;
        }

        public void Select_Childs_Selected_Artiste(ArtisteViewModel selectedArtiste)
        {
            Artiste artisteWithChilds =
                ArtisteService.GetArtisteById(selectedArtiste.Id);
        }

        public ObservableCollection<ArtisteViewModel> saveArtiste(Artiste artiste)
        {
            ArtisteService.SaveArtiste(artiste);

            return Search_Artistes_Executed("");
        }

        public ObservableCollection<ArtisteViewModel> deleteArtiste(Artiste artiste)
        {
            ArtisteService.DeleteArtiste(artiste);

            return Search_Artistes_Executed("");
        }
    }
}
