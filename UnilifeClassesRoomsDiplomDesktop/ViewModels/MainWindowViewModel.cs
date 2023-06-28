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

namespace UnilifeClassesRoomsDiplomDesktop.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        private string _login, _password;
        bool _save;
        private object _currentView;
        private RelayCommand _validateCommand;

        public MainWindowViewModel()
        {
            try
            {
                if (Settings.Default.HashKey != null || Settings.Default.HashKey != "")
                {
                    var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                    bool res = client.CheakHashKey(Settings.Default.HashKey);
                    client.Close();
                    if (res == true)
                    {
                        CurrentView = new MenuPage();
                    }
                    else
                    {
                        Settings.Default.HashKey = "";
                        Settings.Default.Save();
                    }
                }

                Login = Settings.Default.Login;
                Password = Settings.Default.Password;
                if (Password != "")
                {
                    Save = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public bool Save
        {
            get => _save;
            set
            {
                _save = value;
                OnPropertyChanged(nameof(Save));
            }
        }

        public RelayCommand ValidateCommand
        {
            get
            {
                return _validateCommand ??
                    (_validateCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                            string res1 = client.LogInAccount(Login, Password);
                            client.Close();
                            if (res1 != "0")
                            {
                                if (Save == true)
                                {
                                    Settings.Default.Login = Login;
                                    Settings.Default.Password = Password;

                                }
                                else
                                {
                                    Settings.Default.Login = "";
                                    Settings.Default.Password = "";
                                }

                                Settings.Default.HashKey = res1;
                                Settings.Default.Save();
                                CurrentView = new ConfirmPage();
                            }
                            else
                            {
                                MessageBox.Show("Данный пользователь не обнаружен или заблокирован.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
