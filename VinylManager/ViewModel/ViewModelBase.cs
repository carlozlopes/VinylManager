﻿using VinylManager.MVVM;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylManager.ViewModel
{
    internal class ViewModelBase : BindableBase
    {
        protected DelegateCommand editCommand; // oops, not private ...
        private bool isInEditMode = false;

        public ViewModelBase()
        {
            if (this.IsInDesignMode)
            {
                return;
            }

            this.editCommand = new DelegateCommand(this.Edit_Executed, this.Edit_CanExecute);
        }

        public ICommand EditCommand
        {
            get { return this.editCommand; }
        }

        public bool IsInDesignMode
        {
            get { return Windows.ApplicationModel.DesignMode.DesignModeEnabled; }
        }

        public bool IsInEditMode
        {
            get
            {
                return this.isInEditMode;
            }

            set
            {
                this.SetProperty(ref this.isInEditMode, value);
                this.editCommand.RaiseCanExecuteChanged();
            }
        }

        protected virtual bool Edit_CanExecute()
        {
            return !this.IsInEditMode;
        }

        protected virtual void Edit_Executed()
        {
            this.IsInEditMode = true;
        }
    }
}
