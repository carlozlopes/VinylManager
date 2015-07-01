using VinylManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.ViewModel
{
    class TitreViewModel : ViewModelBase
    {
        private Titre model;

        public TitreViewModel(Titre model)
        {
            this.model = model;
        }

        public TitreViewModel()
        {
            // TODO: Complete member initialization
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

        public string Annee
        {
            get
            {
                if (this.model == null)
                {
                    return string.Empty;
                }

                return this.model.Annee;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Annee = value;
                    this.OnPropertyChanged();
                }
            }
        }
    }
}
