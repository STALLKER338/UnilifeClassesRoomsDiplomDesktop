using DevExpress.Office.Utils;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using UnilifeClassesRoomsDiplomDesktop.Pages;
using UnilifeClassesRoomsDiplomDesktop.Properties;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;
using UnilifeClassesRoomsDiplomDesktop.Windows;

namespace UnilifeClassesRoomsDiplomDesktop.ViewModels
{
    public class ClassesPageViewModel : BaseModelOrView
    {
        string _search = "";
        Account account;
        private object _pageFrameView;
        Class _selectedItem;
        int _id;

        private RelayCommand _searchCommand, _addCommand;
        public ObservableCollection<Class> Classes { get; set; }
        public ObservableCollection<Class> DefaultClasses { get; set; }

        public object PageFrameView
        {
            get { return _pageFrameView; }
            set
            {
                _pageFrameView = value;
                OnPropertyChanged("PageFrameView");
            }
        }
        public Class SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                PageFrameView = new ClassPage(value.Id);
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");

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
        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                OnPropertyChanged("Search");
            }
        }
        public RelayCommand SearchCommand
        {
            get
            {
                return _searchCommand ??
                    (_searchCommand = new RelayCommand(obj =>
                    {
                        var result = from d in DefaultClasses
                                     where d.Name.Contains(Search)
                                     select d;

                        Classes.Clear();
                        foreach (var item in result)
                        {
                            Classes.Add(item);
                        }
                    }));
            }
        }
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ??
                    (_searchCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            ClassesWindow divisionWindow = new ClassesWindow();
                            if (divisionWindow.ShowDialog() == true)
                            {
                                PageFrameView = new ClassesPage(Account);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }));
            }
        }
        public RelayCommand AddClassCommand
        {
            get
            {
                return _addCommand ??
                    (_searchCommand = new RelayCommand(obj =>
                    {

                        ClassesWindow divisionWindow = new ClassesWindow();
                        if (divisionWindow.ShowDialog() == true)
                        {
                            PageFrameView = new ClassesPage(Account);
                        }
                    }));
            }
        }
        public ClassesPageViewModel(Account acc)
        {
            try { 

            var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
         
                List<Class> classes = new List<Class>(client.GetClassesUser(Settings.Default.HashKey));

           
            for (int i = 0; i < classes.Count; i++)
            {
                var client1 = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");

                classes[i].Tasks = client1.GetTasksClassFalse(classes[i].Id, Settings.Default.HashKey); 
                //var client2 = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                //List<UnilifeServiceReference.Task> task2 = client2.GetTasksClass(classes[i].Id, Settings.Default.HashKey).ToList();
             
                //classes[i].Tasks = task2.Except(task1).ToArray();
            }
            DefaultClasses = new ObservableCollection<Class>(classes);
            Classes = new ObservableCollection<Class>(DefaultClasses);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
