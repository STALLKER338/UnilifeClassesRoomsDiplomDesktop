using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using UnilifeClassesRoomsDiplomDesktop.Pages;
using UnilifeClassesRoomsDiplomDesktop.Properties;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;
using UnilifeClassesRoomsDiplomDesktop.Windows;

namespace UnilifeClassesRoomsDiplomDesktop.ViewModels
{
    public class AdmClassesViewModel : BaseModelOrView
    {
        string _search = "";
        private object _pageFrameView;
        Class _selectedItem;

        private RelayCommand _searchCommand, _addCommand, _editCommand, _userCommand;
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

                _selectedItem = value;
                OnPropertyChanged("SelectedItem");

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
                     (_addCommand = new RelayCommand(obj =>
                     {
                         try
                         {
                             ClassWindow postWindow = new ClassWindow(new Class());
                             if (postWindow.ShowDialog() == true)
                             {

                                 Class class1 = postWindow.Class;

                                 var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                                 class1.Id = client.AddClass(class1);

                                 DefaultClasses.Add(class1);
                                 Classes.Add(class1);

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
                               Class class1 = selectedItem as Class;
                               if (class1 == null) return;

                               Class vm = new Class
                               {
                                   Id = class1.Id,
                                   Name = class1.Name,
                                   KeyClass = class1.KeyClass,

                               };

                               ClassWindow accountsWindow = new ClassWindow(vm);
                               if (accountsWindow.ShowDialog() == true)
                               {

                                   class1.Name = accountsWindow.Class.Name;
                                   class1.KeyClass = accountsWindow.Class.KeyClass;

                                   var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                                   client.UpdClass(class1);

                               }
                           }
                           catch (Exception ex)
                           {
                               MessageBox.Show(ex.Message);
                           }

                       }));
            }
        }
        public RelayCommand UsersCommand
        {
            get
            {
                return _userCommand ??
                          (_userCommand = new RelayCommand((selectedItem) =>
                          {
                              try
                              {
                                  PageFrameView = new UsersClass(selectedItem as Class);
                                  //ClassesWindow divisionWindow = new ClassesWindow();
                                  //if (divisionWindow.ShowDialog() == true)
                                  //{
                                  //    PageFrameView = new ClassesPage(Account);
                                  //}
                              }
                              catch (Exception ex)
                              {
                                  MessageBox.Show(ex.Message);
                              }
                          }));
            }
        }

        public AdmClassesViewModel()
        {
            try
            {
                var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");

                DefaultClasses = new ObservableCollection<Class>(client.GetClasses());
                Classes = new ObservableCollection<Class>(DefaultClasses);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}