using VinylManager.Services;
using VinylManager.MVVM;
using VinylManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.ViewModel
{
    class InventaireSinglesViewModel : ViewModelBase
    {
        private ObservableCollection<InventaireSingleViewModel> inventaires = new ObservableCollection<InventaireSingleViewModel>();

        public InventaireSinglesViewModel() { }

        public ObservableCollection<InventaireSingleViewModel> Select_Inventary_From_SingleId(int singleId)
        {
            List<Inventaire> models = InventaireService.GetAllInventaireBySingleId(singleId);
            inventaires.Clear();
            foreach (var m in models)
            {
                this.inventaires.Add(new InventaireSingleViewModel(m));
            }

            return inventaires;
        }

        public ObservableCollection<InventaireSingleViewModel> saveVynil(Inventaire vynil)
        {
            InventaireService.SaveInventaire(vynil);

            return Select_Inventary_From_SingleId(vynil.DisqueId);
        }

        public ObservableCollection<InventaireSingleViewModel> deleteVynil(Inventaire vynil)
        {
            InventaireService.DeleteInventaire(vynil);

            return Select_Inventary_From_SingleId(vynil.DisqueId);
        }
    }
}
