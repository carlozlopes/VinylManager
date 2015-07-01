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
    internal class TitresViewModel : ViewModelBase
    {
        private ObservableCollection<TitreViewModel> titres = new ObservableCollection<TitreViewModel>();

        public TitresViewModel() { }

        public ObservableCollection<TitreViewModel> Search_Titres_Executed(String query)
        {
            List<Titre> models = TitreService.GetAllTitresByGivenQuery(query);

            titres.Clear();
            foreach (var m in models)
            {
                this.titres.Add(new TitreViewModel(m));
            }

            return titres;
        }

        public void Select_Childs_Selected_Titre(TitreViewModel selectedTitre)
        {
            Titre titreWithChilds =
                TitreService.GetTitreWithChildren(selectedTitre.Id);
        }

        public static TitreViewModel Select_Titre_ById(int titreId)
        {
            Titre face = TitreService.GetTitreById(titreId);
            TitreViewModel titreViewModel = new TitreViewModel(face);

            return titreViewModel;
        }

        public ObservableCollection<TitreViewModel> saveTitre(Titre face)
        {
            TitreService.SaveTitre(face);
            return Search_Titres_Executed("");
        }

        public ObservableCollection<TitreViewModel> deleteTitre(Titre face)
        {
            TitreService.DeleteTitre(face);

            return Search_Titres_Executed("");
        }
    }
}
