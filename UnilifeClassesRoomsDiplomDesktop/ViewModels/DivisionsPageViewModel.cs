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
    public class DivisionsPageViewModel : INotifyPropertyChanged
    {
        string _search = "";

        RelayCommand _searchCommand, _addCommand, _editCommand;
        Division _selectedItem;

        public DivisionsPageViewModel()
        {
            try
            { 
            var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
            DefaultDivisions = new ObservableCollection<Division>(client.GetDivisions());
            Divisions = new ObservableCollection<Division>(DefaultDivisions);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ObservableCollection<Division> Divisions { get; set; }
        public ObservableCollection<Division> DefaultDivisions { get; set; }

        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                OnPropertyChanged("Search");
            }
        }
        public Division SelectedItem
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
                        var result = from d in DefaultDivisions
                                     where d.Name.Contains(Search)
                                     select d;

                        Divisions.Clear();
                        foreach (var item in result)
                        {
                            Divisions.Add(item);
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
                        DivisionWindow divisionWindow = new DivisionWindow(new Division());
                        if (divisionWindow.ShowDialog() == true)
                        {

                            Division division = divisionWindow.Division;

                            var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                            division.Id = client.AddDivision(division);

                            DefaultDivisions.Add(division);
                            Divisions.Add(division);

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
                            Division division = selectedItem as Division;
                            if (division == null) return;

                            Division vm = new Division
                            {
                                Id = division.Id,
                                Name = division.Name,
                                Active = division.Active,

                            };

                            DivisionWindow divisionWindow = new DivisionWindow(vm);
                            if (divisionWindow.ShowDialog() == true)
                            {

                                division.Name = divisionWindow.Division.Name;
                                division.Active = divisionWindow.Division.Active;

                                var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                                client.UpdDivision(division);
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
