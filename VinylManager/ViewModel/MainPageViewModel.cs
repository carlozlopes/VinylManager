using VinylManager.Services;
using VinylManager.Models;
using VinylManager.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VinylManager.ViewModel
{
    class MainPageViewModel : ViewModelBase
    {
        private bool hasSelection = false;
        private ObservableCollection<ArtisteViewModel> artistes = new ObservableCollection<ArtisteViewModel>();
        private DelegateCommand selectCommand;
        private ArtisteViewModel selectedArtiste = null;

        public MainPageViewModel()
        {
            if (this.IsInDesignMode)
            {
                return;
            }
            this.selectCommand = new DelegateCommand(this.Select_Executed);
        }

        public ObservableCollection<ArtisteViewModel> Artistes
        {
            get { return this.artistes; }
            set { this.SetProperty(ref this.artistes, value); }
        }

        public ICommand SelectCommand
        {
            get { return this.selectCommand; }
        }

        public ArtisteViewModel SelectedArtiste
        {
            get { return this.selectedArtiste; }
            set
            {
                this.SetProperty(ref this.selectedArtiste, value);
                this.hasSelection = this.selectedArtiste != null;
            }
        }

        private void Select_Executed()
        {
            List<Artiste> models = ArtisteService.GetAllArtistes();

            this.artistes.Clear();
            foreach (var m in models)
            {
                this.artistes.Add(new ArtisteViewModel(m));
            }

            this.isDatabaseCreated = true;
        }

        public bool isDatabaseCreated { get; set; }
    }
}
