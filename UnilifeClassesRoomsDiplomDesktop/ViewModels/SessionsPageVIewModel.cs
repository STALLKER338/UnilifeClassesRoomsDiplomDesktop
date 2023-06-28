using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;
using UnilifeClassesRoomsDiplomDesktop.Windows;

namespace UnilifeClassesRoomsDiplomDesktop.ViewModels
{
    public class SessionsPageVIewModel : INotifyPropertyChanged
    {
        string _search = "";

        RelayCommand _deletedCommand, _searchCommand;
        Session _selectedItem;

        public SessionsPageVIewModel(int idAccount)
        {
            try
            {
                var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");

                DefaultSessions = new ObservableCollection<Session>(client.GetSessionsAccount(idAccount));
                Sessions = new ObservableCollection<Session>(DefaultSessions);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ObservableCollection<Session> Sessions { get; set; }
        public ObservableCollection<Session> DefaultSessions { get; set; }

        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                OnPropertyChanged("Search");
            }
        }
        public Session SelectedItem
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
                        try 
                        { 
                        var result = from d in DefaultSessions
                                     where d.SessionKey.Contains(Search)
                                     select d;

                        Sessions.Clear();
                        foreach (var item in result)
                        {
                            Sessions.Add(item);
                        }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
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
                        try
                        { 
                        if (selectedItem != null)
                        {
                            Session session = selectedItem as Session;
                            var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                            client.DelSession(session);
                           
                            DefaultSessions.Remove(session);
                            Sessions.Remove(session);
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
