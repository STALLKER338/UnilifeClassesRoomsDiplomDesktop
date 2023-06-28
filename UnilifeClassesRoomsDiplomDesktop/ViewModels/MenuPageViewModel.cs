using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

using UnilifeClassesRoomsDiplomDesktop.Pages;
using UnilifeClassesRoomsDiplomDesktop.Properties;
using System.Windows.Media.Imaging;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;

namespace UnilifeClassesRoomsDiplomDesktop.ViewModels
{
    public class MenuPageViewModel : INotifyPropertyChanged
    {
        private object _menuFrameView, _pageFrameView;
        private RelayCommand _adminPanelCommand, _classesCommand, _logoutCommand, _settingsCommand, _helpCommand;
        private Visibility _adminMenu;
        Account account;

      
        public Visibility AdminMenu
        {
            get { return _adminMenu; }
            set
            {
                _adminMenu = value;
                OnPropertyChanged("AdminMenu");
            }
        }
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

        public RelayCommand AdminPanelCommand
        {
            get
            {
                return _adminPanelCommand ??
                    (_adminPanelCommand = new RelayCommand(obj =>
                    {
                        PageFrameView = new AdminMenuPage(Account);
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
                        MenuFrameView = new ClassesPage(Account);
                    }));
            }
        }
        public RelayCommand LogoutCommand
        {
            get
            {
                return _logoutCommand ??
                    (_logoutCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                            bool res = client.LogOut(Settings.Default.HashKey);
                            client.Close();
                            if (res == true)
                            {
                                Settings.Default.HashKey = "";
                                Settings.Default.Save();
                                MainWindow main = new MainWindow();
                                main.Show();
                                Application.Current.MainWindow.Close();
                            }
                            else
                            {
                                MessageBox.Show("Что-то пошло не так! Рекомендуем перезагрузить приложение");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }));
            }
        }
        public RelayCommand SettingsCommand
        {
            get
            {
                return _settingsCommand ??
                    (_settingsCommand = new RelayCommand(obj =>
                    {
                        MenuFrameView = new SettingsPage(Account);
                    }));
            }
        }
        public RelayCommand HelpCommand
        {
            get
            {
                return _helpCommand ??
                    (_helpCommand = new RelayCommand(obj =>
                    {

                    }));
            }
        }

        public MenuPageViewModel()
        {
            try
            {
                var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                Account = client.GetAccountUser(Settings.Default.HashKey);

                Account.IconImage = ToImage(Account.Icon);
                if (Account.UserId != null)
                {
                    var client1 = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                    Account.User = client1.GetUser(Account.UserId);
                    client1.Close();
                }

                int powerRole = client.GetPowerRole(Settings.Default.HashKey);
                client.Close();
                if (powerRole < 20)
                {
                    AdminMenu = Visibility.Collapsed;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public BitmapImage ToImage(byte[] array)
        {
            try{ 
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


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
