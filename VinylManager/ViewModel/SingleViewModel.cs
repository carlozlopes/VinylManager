using VinylManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.ViewModel
{
    class SingleViewModel : ViewModelBase
    {
        private Singles model;

        public SingleViewModel(Singles model)
        {
            this.model = model;
        }

        public SingleViewModel()
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

        public int FaceAId
        {
            get
            {
                if (this.model == null)
                {
                    return 0;
                }

                return this.model.FaceAId;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.FaceAId = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public Titre FaceA
        {
            get
            {
                if (this.model == null)
                {
                    return new Titre();
                }

                return this.model.Faces[0];
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Faces[0] = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public int FaceBId
        {
            get
            {
                if (this.model == null)
                {
                    return 0;
                }

                return this.model.FaceBId;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.FaceBId = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public Titre FaceB
        {
            get
            {
                if (this.model == null)
                {
                    return new Titre();
                }
                if (this.model.Faces.Count > 1)
                {
                    return this.model.Faces[1];
                }
                else
                {
                    return this.model.Faces[0];
                }
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Faces[1] = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public Artiste Artiste
        {
            get
            {
                if (this.model == null)
                {
                    return new Artiste();
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

    }
}
