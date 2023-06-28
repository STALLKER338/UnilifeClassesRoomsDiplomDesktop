using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using Unilife_ClasseRoom.Windows;

using UnilifeClassesRoomsDiplomDesktop.Pages;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;
using UnilifeClassesRoomsDiplomDesktop.Windows;

namespace UnilifeClassesRoomsDiplomDesktop.ViewModels
{
    public class AccountsPageViewModel : BaseModelOrView
    {
        string _search = "";
        object _pageFrameView;

        RelayCommand _searchCommand, _addCommand, _editCommand, _sessionsCommand, _reportCommand;
        Account _selectedItemAccount;

        public AccountsPageViewModel()
        {
            try
            {
                var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                DefaultAccounts = new ObservableCollection<Account>(client.GetAccounts().ToList());
                var client1 = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                foreach (var a in DefaultAccounts)
                {
                    a.Role = client1.GetRole(a.RoleId);
                    a.User = client1.GetUser(a.UserId);
                    if (a.Icon != null)
                    {
                        a.IconImage = ToImage(a.Icon);
                    }
                }
                Accounts = new ObservableCollection<Account>(DefaultAccounts);
                client1.Close();
                client.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ObservableCollection<Account> Accounts { get; set; }
        public ObservableCollection<Account> DefaultAccounts { get; set; }

        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                OnPropertyChanged("Search");
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
        public Account SelectedItemAccount
        {
            get { return _selectedItemAccount; }
            set
            {

                _selectedItemAccount = value;
                OnPropertyChanged("SelectedItemAccount");

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
                                     where d.Login.Contains(Search)
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
                    try
                    {
                        AccountWindow accountsWindow = new AccountWindow(new Account());
                        if (accountsWindow.ShowDialog() == true)
                        {

                            Account account = accountsWindow.Account;
                            Role r = new Role();
                            User u = new User();
                            if (account.Role != null)
                            {
                                r=account.Role;
                                account.RoleId = account.Role.Id;
                                account.Role = null;
                            }
                            if (account.User != null)
                            {
                                u = account.User;
                                account.UserId = account.User.Id;
                                account.User = null;
                               
                            }
                            if (account.IconImage != null)
                            {
                                byte[] data;
                                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create(accountsWindow.Account.IconImage));
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    encoder.Save(ms);
                                    data = ms.ToArray();
                                }
                                account.Icon = data;
                                account.IconImage = null;
                            }
                            var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                            account.Id = client.AddAccount(account);
                            client.Close();
                            //db.Accounts.Add(account);
                            //db.SaveChanges();
                            account.Role = r;
                            account.User = u;
                            if (account.Icon != null)
                            {
                                account.IconImage = ToImage(account.Icon);
                            }
                            DefaultAccounts.Add(account);
                            Accounts.Add(account);

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
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
                        try
                        {
                            Account account = selectedItem as Account;
                            if (account == null) return;

                            Account vm = new Account
                            {
                                Id = account.Id,
                                Login = account.Login,
                                Password = account.Password,
                                Mail = account.Mail,
                                RoleId = account.RoleId,
                                UserId = account.UserId,
                                Role = account.Role,
                                User = account.User,
                                Active = account.Active,
                                IconImage = account.IconImage,
                                Icon = account.Icon,
                            };

                            AccountWindow accountsWindow = new AccountWindow(vm);
                            if (accountsWindow.ShowDialog() == true)
                            {


                                account.Login = accountsWindow.Account.Login;
                                account.Password = accountsWindow.Account.Password;
                                account.Active = accountsWindow.Account.Active;
                                account.Mail = accountsWindow.Account.Mail;

                                account.IconImage = accountsWindow.Account.IconImage;
                                if (accountsWindow.Account.IconImage != null)
                                {
                                    byte[] data;
                                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                                    encoder.Frames.Add(BitmapFrame.Create(accountsWindow.Account.IconImage));
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        encoder.Save(ms);
                                        data = ms.ToArray();
                                    }
                                    account.Icon = data;
                                    account.IconImage = null;
                                }
                                if (accountsWindow.Account.User != null)
                                {
                                    account.UserId = accountsWindow.Account.User.Id;
                                    account.User = null;
                                }
                                if (accountsWindow.Account.Role != null)
                                {
                                    account.RoleId = accountsWindow.Account.Role.Id;
                                    account.Role = null;
                                }
                                var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                                client.UpdAccount(account);
                                client.Close();
                                if (accountsWindow.Account.User != null)
                                {
                                    account.User = accountsWindow.Account.User;
                                }
                                if (accountsWindow.Account.Role != null)
                                {
                                    account.Role = accountsWindow.Account.Role;
                                }
                                if (accountsWindow.Account.Icon != null)
                                {
                                    account.IconImage = ToImage(account.Icon);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }));
            }
        }
        public RelayCommand SessionsCommand
        {
            get
            {
                return _sessionsCommand ??
                    (_sessionsCommand = new RelayCommand((selectedItem) =>
                    {
                        try
                        {
                            Account account = selectedItem as Account;
                            PageFrameView = new SessionsPage(account.Id);
                        }
  catch (Exception ex) {
                            MessageBox.Show(ex.Message);
                        }
                    }));
            }
        }
        public RelayCommand ReportCommand
        {
            get
            {
                return _reportCommand ??
                    (_reportCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            Report report = new Report(Accounts);
                            report.Show();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }));
            }
        }

        public BitmapImage ToImage(byte[] array)
        {
            try
            {
                using (var ms = new System.IO.MemoryStream(array))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad; // here
                    image.StreamSource = ms;
                    image.EndInit();
                    return image;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
