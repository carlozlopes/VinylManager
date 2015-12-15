using VinylManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.ViewModel
{
    class InventaireSingleViewModel : ViewModelBase
    {
        private Inventaire model;

        public InventaireSingleViewModel(Inventaire model)
        {
            this.model = model;
        }

        public InventaireSingleViewModel()
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

        public string Etat
        {
            get
            {
                if (this.model == null)
                {
                    return string.Empty;
                }

                return this.model.Etat;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Etat = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string Couleur
        {
            get
            {
                if (this.model == null)
                {
                    return string.Empty;
                }

                return this.model.Couleur;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Couleur = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string EtatPochette
        {
            get
            {
                if (this.model == null)
                {
                    return string.Empty;
                }

                return this.model.EtatPochette;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.EtatPochette = value;
                    this.OnPropertyChanged();
                }
            }
        }
    }
}
