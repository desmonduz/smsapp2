﻿using Prism.Mvvm;
using Prism.Commands;
using SMSApp.DataAccess;
using System.Collections.ObjectModel;

using SMSApp.Views;
using System.Threading.Tasks;
using Autofac;
using SMSApp.Repositories.Core;
using System.Collections.Generic;

namespace SMSApp.ViewModels
{
    public class UserManagerViewModel : ViewModelBase
    {
        private IUnitOfWork uw;


        public DelegateCommand OpenAddUserCommand { get; private set; }
        public DelegateCommand OpenEditUserCommand { get; private set; }
        public DelegateCommand RemoveUserCommand { get; private set; }

        public UserManagerViewModel(IContainer container, IUnitOfWork unitOfWork) : base(container)
        {
            uw = unitOfWork;
            OpenAddUserCommand = new DelegateCommand(ExecuteOpenAddUserCommand);
            OpenEditUserCommand = new DelegateCommand(ExecuteEditAddUserCommand, ()=>selectedUser!=null);
            RemoveUserCommand = new DelegateCommand(ExecuteRemoveUserCommand, ()=>selectedUser!=null);
        }
      
        public IEnumerable<User> Users
        {
            get { return uw.Users.GetAll();}
        }

        private User selectedUser;

        public User SelectedUser
        {
            get { return selectedUser; }
            set { selectedUser = value;
                RaisePropertyChanged();
                OpenEditUserCommand.RaiseCanExecuteChanged();
                RemoveUserCommand.RaiseCanExecuteChanged(); }
        }

        public void ExecuteOpenAddUserCommand()
        {
            UserAddView view = _container.Resolve<UserAddView>();
            view.ShowDialog();
            RaisePropertyChanged("Users");
        }

        public void ExecuteEditAddUserCommand()
        {
            UserAddView view = _container.Resolve<UserAddView>(
                new NamedParameter("viewModel", _container.Resolve<UserAddViewModel>(
                    new NamedParameter("userModel", SelectedUser),
                    new NamedParameter("isUpdate", true)
                )));
            view.ShowDialog();
            RaisePropertyChanged("Users");
        }

        public void ExecuteRemoveUserCommand()
        {
            uw.Users.Remove(SelectedUser);
            uw.Complete();
            RaisePropertyChanged("Users");
        }
    }
}