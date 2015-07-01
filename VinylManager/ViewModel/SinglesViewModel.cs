using VinylManager.Managers;
using VinylManager.Services;
using VinylManager.MVVM;
using VinylManager.JoinModels;
using VinylManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.ViewModel
{
    internal class SinglesViewModel : ViewModelBase
    {
        private ObservableCollection<SingleJoinDataViewModel> singles = new ObservableCollection<SingleJoinDataViewModel>();

        public SinglesViewModel() { }

        public ObservableCollection<SingleJoinDataViewModel> Search_Singles_Executed(String query)
        {
            List<SinglesJoinData> models = SinglesJoinService.GetAllSinglesByTitre(query);

            singles.Clear();
            foreach (var m in models)
            {
                this.singles.Add(new SingleJoinDataViewModel(m));
            }

            return singles;
        }

        public ObservableCollection<SingleJoinDataViewModel> Search_Singles_ByUser_Executed(String query, int artisteId)
        {
            List<SinglesJoinData> models = SinglesJoinService.GetAllSinglesByTitreAndArtiste(query, artisteId);

            singles.Clear();
            foreach (var m in models)
            {
                this.singles.Add(new SingleJoinDataViewModel(m));
            }

            return singles;
        }

        public ObservableCollection<SingleJoinDataViewModel> Search_Singles_Titres_Executed(String query)
        {
            List<SinglesJoinData> models = SinglesJoinService.GetAllSinglesTitresByTitre(query);

            singles.Clear();
            foreach (var m in models)
            {
                this.singles.Add(new SingleJoinDataViewModel(m));
            }

            return singles;
        }

        public ObservableCollection<SingleJoinDataViewModel> Search_Singles_Titres_ByUser_Executed(String query, int artisteId)
        {
            List<SinglesJoinData> models = SinglesJoinService.GetAllSinglesTitresByTitreAndArtiste(query, artisteId);

            singles.Clear();
            foreach (var m in models)
            {
                this.singles.Add(new SingleJoinDataViewModel(m));
            }

            return singles;
        }

        public SingleViewModel Select_Childs_Selected_Single(int singleId)
        {
            Singles singleWithChilds =
                SinglesService.GetSingleWithChildren(singleId);

            return new SingleViewModel(singleWithChilds);
        }

        public ObservableCollection<SingleJoinDataViewModel> saveSingle(Singles single)
        {
            // SinglesService.SaveSingle(single);
            SinglesManager.SaveOrUpdate(single);
            return Search_Singles_Executed("");
        }

        public ObservableCollection<SingleJoinDataViewModel> deleteSingle(Singles single)
        {
            SinglesService.DeleteSingle(single);

            return Search_Singles_Executed("");
        }
    }
}
