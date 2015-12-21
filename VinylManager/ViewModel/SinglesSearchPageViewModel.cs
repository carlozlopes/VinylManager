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
    class SinglesSearchPageViewModel : ViewModelBase
    {
        private DelegateCommand searchArtistesCommand;
        private ObservableCollection<ArtisteViewModel> artistes = new ObservableCollection<ArtisteViewModel>();

        public SinglesSearchPageViewModel()
        {
            this.searchArtistesCommand = new DelegateCommand(this.Search_Artistes_Executed);
        }

        public ObservableCollection<ArtisteViewModel> Artistes
        {
            get { return this.artistes; }
            set { this.SetProperty(ref this.artistes, value); }
        }
        
        public ICommand SearchArtistesCommand
        {
            get { return this.searchArtistesCommand; }
        }
        
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
    }
}
