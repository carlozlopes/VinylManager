using VinylManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.ViewModel
{
    class ArtisteViewModel : ViewModelBase
    {
        private Artiste model;

        public ArtisteViewModel(Artiste model)
        {
            this.model = model;
        }

        public ArtisteViewModel()
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

        public string Nationalite
        {
            get
            {
                if (this.model == null)
                {
                    return string.Empty;
                }

                return this.model.Nationalite;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Nationalite = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public int SingleCounter
        {
            get
            {
                if (this.model == null)
                {
                    return 0;
                }
                return this.model.singleCounter;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.singleCounter = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public int QuatreTitresCounter
        {
            get
            {
                if (this.model == null)
                {
                    return 0;
                }
                return this.model.quatreTitresCounter;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.quatreTitresCounter = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public int TrenteTroisTitresCounter
        {
            get
            {
                if (this.model == null)
                {
                    return 0;
                }
                return this.model.trenteTroisTitresCounter;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.trenteTroisTitresCounter = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public int Interprete
        {
            get
            {
                if (this.model == null)
                {
                    return 0;
                }
                return this.model.interprete;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.interprete = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public int Auteur
        {
            get
            {
                if (this.model == null)
                {
                    return 0;
                }
                return this.model.auteur;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.auteur = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public int Compositeur
        {
            get
            {
                if (this.model == null)
                {
                    return 0;
                }
                return this.model.compositeur;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.compositeur = value;
                    this.OnPropertyChanged();
                }
            }
        }
    }
}
