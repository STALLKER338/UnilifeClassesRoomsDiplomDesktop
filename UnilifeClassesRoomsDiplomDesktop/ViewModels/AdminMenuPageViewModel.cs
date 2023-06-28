using System.ComponentModel;
using System.Runtime.CompilerServices;

using UnilifeClassesRoomsDiplomDesktop.Pages;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;

namespace UnilifeClassesRoomsDiplomDesktop.ViewModels
{
    public class AdminMenuPageViewModel : BaseModelOrView
    {
        private object _menuFrameView, _pageFrameView;
        private RelayCommand _appCommand, _accountsCommand, _divisionsCommand, _postCommand, _classesCommand,
            _logsCommand, _usersCommand;
        Account account;

        public object MenuFrameView
        {
            get { return _menuFrameView; }
            set
            {
                _menuFrameView = value;
                OnPropertyChanged("MenuFrameView");
            }
        }
        public object PageFrameView
        {
            get { return _pageFrameView; }
            set
            {
                _pageFrameView = value;
                OnPropertyChanged("PageFrameView");
            }
        }
        public Account Account
        {
            get { return account; }
            set
            {
                account = value;
                OnPropertyChanged("Account");
            }
        }
        public RelayCommand AppCommand
        {
            get
            {
                return _appCommand ??
                    (_appCommand = new RelayCommand(obj =>
                    {
                        PageFrameView = new MenuPage();
                    }));
            }
        }
        public RelayCommand AccountsCommand
        {
            get
            {
                return _accountsCommand ??
                    (_accountsCommand = new RelayCommand(obj =>
                    {
                        MenuFrameView = new AccountsPage();
                    }));
            }
        }
        public RelayCommand DivisionsCommand
        {
            get
            {
                return _divisionsCommand ??
                    (_divisionsCommand = new RelayCommand(obj =>
                    {
                        MenuFrameView = new DivisionsPage();
                    }));
            }
        }
        public RelayCommand PostCommand
        {
            get
            {
                return _postCommand ??
                    (_postCommand = new RelayCommand(obj =>
                    {
                        MenuFrameView = new PostsPage();
                    }));
            }
        }
        public RelayCommand ClassesCommand
        {
            get
            {
                return _classesCommand ??
                    (_classesCommand = new RelayCommand(obj =>
                    {
                        MenuFrameView = new AdmClasses();
                    }));
            }
        }
        public RelayCommand LogsCommand
        {
            get
            {
                return _logsCommand ??
                    (_logsCommand = new RelayCommand(obj =>
                    {
                        MenuFrameView = new LogsPage();
                    }));
            }
        }
        public RelayCommand UsersCommand
        {
            get
            {
                return _usersCommand ??
                    (_usersCommand = new RelayCommand(obj =>
                    {
                        MenuFrameView = new UsersPage();
                    }));
            }
        }
        public AdminMenuPageViewModel(Account acc)
        {
            Account = acc;
        }
    }
}
