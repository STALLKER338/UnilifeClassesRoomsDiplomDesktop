using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;
using UnilifeClassesRoomsDiplomDesktop.Windows;

namespace UnilifeClassesRoomsDiplomDesktop.ViewModels
{
    public class UsersClassViewModel: BaseModelOrView
    {  
        string _search = "";

        RelayCommand _deletedCommand, _addCommand,_editCommand, _searchCommand;
        ClassAccount _selectedItem;
        public Class Class { get; set; }

        public UsersClassViewModel(Class class1)
        {
            Class = class1;
            var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
            var client1 = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
            DefaultAccounts = new ObservableCollection<ClassAccount>(client.GetClassAccounts(Class.Id));
            foreach(var a in DefaultAccounts)
            {
                a.Account = client1.GetAccount(a.AccountId);
                a.Account.User = client.GetUser(a.Account.UserId); 
            }
            Accounts = new ObservableCollection<ClassAccount>(DefaultAccounts);
        }

        public ObservableCollection<ClassAccount> Accounts { get; set; }
        public ObservableCollection<ClassAccount> DefaultAccounts { get; set; }

        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                OnPropertyChanged("Search");
            }
        }
        public ClassAccount SelectedItem
        {
            get { return _selectedItem; }
            set
            {

                _selectedItem = value;
                OnPropertyChanged("SelectedItem");

            }
        }
        public RelayCommand SearchCommand
        {
            get
            {
                return _searchCommand ??
                    (_searchCommand = new RelayCommand(obj =>
                    {
                        var result = from d in DefaultAccounts
                                     where d.Account.Login.Contains(Search)
                                     select d;

                        Accounts.Clear();
                        foreach (var item in result)
                        {
                            Accounts.Add(item);
                        }
                    }));
            }
        }
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ??
                    (_addCommand = new RelayCommand(obj =>
                    {
                        var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                       
                        UserToClassWindow add = new UserToClassWindow(new ClassAccount());
                        if (add.ShowDialog() == true)
                        {
                           
                            ClassAccount classAccount = add.ClassAccount;
                            Account acc=add.ClassAccount.Account;
                            classAccount.ClassId = Class.Id;
                            classAccount.AccountId = classAccount.Account.Id;
                            classAccount.Account = null;
                            add.ClassAccount.ClassId = Class.Id;
                            classAccount.Id=client.AddAccountToClass(classAccount);
                            //AddTask add = new AddTask(new UnilifeServiceReference.Task(), _classId);

                            classAccount.Account = acc;
                            classAccount.Account.User = client.GetUser(classAccount.Account.UserId);

                            DefaultAccounts.Add(add.ClassAccount);
                            Accounts.Add(add.ClassAccount);

                        }
                    }));
            }
        }
        public RelayCommand EditCommand
        {
            get
            {
                return _editCommand ??
                    (_editCommand = new RelayCommand((selectedItem) =>
                    {

                        ClassAccount classAccount = selectedItem as ClassAccount;
                        if (classAccount == null) return;

                        ClassAccount vm = new ClassAccount
                        {
                            Id = classAccount.Id,
                            Account = classAccount.Account,
                            AccountId = classAccount.AccountId,
                            ClassId = classAccount.ClassId,
                            Teacher = classAccount.Teacher,
                        };

                        UserToClassWindow accountsWindow = new UserToClassWindow(vm);
                        if (accountsWindow.ShowDialog() == true)
                        {

                            classAccount.AccountId = accountsWindow.ClassAccount.AccountId;
                            classAccount.ClassId = accountsWindow.ClassAccount.ClassId;
                            classAccount.Teacher = accountsWindow.ClassAccount.Teacher;
                            classAccount.Account = null;
                            classAccount.Class = null;

                            var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                            client.UpdAccountToClass(classAccount);
                           
                            classAccount.Account= accountsWindow.ClassAccount.Account;
                            classAccount.Account.User = client.GetUser(classAccount.Account.UserId);

                        }

                    }));
            }
        }
        public RelayCommand DeletedCommand
        {
            get
            {
                return _deletedCommand ??
                    (_deletedCommand = new RelayCommand((selectedItem) =>
                    {
                        if (selectedItem != null)
                        {
                            ClassAccount classAccount = selectedItem as ClassAccount;
                            var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                            client.DelAccountToClass(classAccount);

                            DefaultAccounts.Remove(classAccount);
                            Accounts.Remove(classAccount);
                        }
                    }));
            }
        }
    }
}
