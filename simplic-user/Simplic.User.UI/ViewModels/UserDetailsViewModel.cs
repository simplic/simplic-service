﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Simplic.UI.MVC;

namespace Simplic.User.UI
{
    class UserDetailsViewModel : ViewModelBase, IUserDialogViewModel
    {
        #region fields
        private string _userName;
        private string _firstName;
        private string _lastName;
        private string _email;
        private bool _isActive;
        private string _phone;
        private bool _isADUser;
        private UserViewModel _user;
        private ObservableCollection<IDialogViewModel> _dialogs;
        private ICommand _changePasswordCommand;
        private bool _isCreate;
        #endregion

        #region ctr
        public UserDetailsViewModel(UserViewModel user, bool isCreate)
        {
            if (!isCreate)
            {
                User = user;
                UserName = User.UserName;
                FirstName = User.FirstName;
                LastName = User.LastName;
                Email = User.Email;
                Phone = User.Phone;
                IsActive = User.IsActive;
            }
            else
                User = null;
            
            Dialogs = new ObservableCollection<IDialogViewModel>();
            ChangePasswordCommand = new RelayCommand(OnChangePassword);
            IsCreate = isCreate;
        }
        #endregion

        #region methods
        private void OnChangePassword(object arg)
        {
            Dialogs.Add(new ChangeUserPasswordViewModel(User));
        }

        private void Close()
        {
            DialogClosing(this, new EventArgs());
        }

        public void RequestClose()
        {
            Close();
        }

        private void OnOk(object arg)
        {
            if (IsCreate)
                User = new UserViewModel
                {
                    UserName = UserName,
                    IsADUser = this.IsADUser
                };
            User.FirstName = FirstName;
            User.LastName = LastName;
            User.Email = Email;
            User.Phone = Phone;
            User.IsActive = IsActive;
            Close();
        }

        private void OnCancel(object arg)
        {
            Close();
        }
        #endregion

        #region events
        public event EventHandler DialogClosing;
        #endregion

        #region properties
        public ICommand OkCommand { get { return new RelayCommand(OnOk); } }

        public ICommand CancelCommand { get { return new RelayCommand(OnCancel); } }

        public UserViewModel User
        {
            get { return _user; }
            set { PropertySetter(value, newValue => _user = newValue); }
        }

        public string UserName
        {
            get { return _userName; }
            set { PropertySetter(value, newValue => _userName = newValue); }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { PropertySetter(value, newValue => _firstName = newValue); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { PropertySetter(value, newValue => _lastName = newValue); }
        }

        public string Email
        {
            get { return _email; }
            set { PropertySetter(value, newValue => _email = newValue); }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { PropertySetter(value, newValue => _isActive = newValue); }
        }

        public string Phone
        {
            get { return _phone; }
            set { PropertySetter(value, newValue => _phone = newValue); }
        }

        public bool IsModal => true;

        public ObservableCollection<IDialogViewModel> Dialogs
        {
            get { return _dialogs; }
            set { PropertySetter(value, newValue => _dialogs = newValue); }
        }

        public ICommand ChangePasswordCommand
        {
            get { return _changePasswordCommand; }
            set { PropertySetter(value, newValue => _changePasswordCommand = newValue); }
        }

        public bool IsCreate
        {
            get { return _isCreate; }
            set { PropertySetter(value, newValue => _isCreate = newValue); }
        }

        public bool IsADUser
        {
            get { return _isADUser; }
            set { PropertySetter(value, newValue => _isADUser = newValue); }
        }
        #endregion
    }
}