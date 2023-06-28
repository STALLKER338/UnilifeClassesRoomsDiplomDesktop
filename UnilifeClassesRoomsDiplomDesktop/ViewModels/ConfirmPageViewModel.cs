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
    public class ConfirmPageViewModel : INotifyPropertyChanged
    {
        private string _key;
        private object _pageFrameView; 
        RelayCommand _validateKeyCommand;

        public object PageFrameView
        {
            get { return _pageFrameView; }
            set
            {
                _pageFrameView = value;
                OnPropertyChanged("PageFrameView");
            }
        }
        public string Key
        {
            get { return _key; }
            set { _key = value; OnPropertyChanged(nameof(Key)); }
        }
        public RelayCommand ValidateKeyCommand
        {
            get
            {
                return _validateKeyCommand ??
                    (_validateKeyCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                            bool res1 = client.ConfirmLogInAccount(Settings.Default.HashKey, Key);
                            client.Close();
                            if (res1 == true)
                            {

                                PageFrameView = new MenuPage();
                            }
                            else
                            {
                                MessageBox.Show("Неверен код.");
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
