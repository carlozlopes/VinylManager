using VinylManager.JoinModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.ViewModel
{
    class SingleJoinDataViewModel : ViewModelBase
    {
        private SinglesJoinData model;

        public SingleJoinDataViewModel(SinglesJoinData model)
        {
            this.model = model;
        }

        public SingleJoinDataViewModel()
        {

        }

        public int Id
        {
            get
            {
                if (this.model == null)
                {
                    return 0;
                }

                return this.model.Id;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Id = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string Nom
        {
            get
            {
                if (this.model == null)
                {
                    return string.Empty;
                }

                return this.model.Nom;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Nom = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string Artiste
        {
            get
            {
                if (this.model == null)
                {
                    return string.Empty;
                }

                return this.model.Artiste;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Artiste = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string TitreA
        {
            get
            {
                if (this.model == null)
                {
                    return string.Empty;
                }

                return this.model.TitreA;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.TitreA = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string TitreB
        {
            get
            {
                if (this.model == null)
                {
                    return string.Empty;
                }

                return this.model.TitreB;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.TitreB = value;
                    this.OnPropertyChanged();
                }
            }
        }
    }
}
