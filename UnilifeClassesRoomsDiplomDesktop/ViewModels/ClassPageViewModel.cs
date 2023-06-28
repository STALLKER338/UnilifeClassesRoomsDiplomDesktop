using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;

using UnilifeClassesRoomsDiplomDesktop.Pages;
using UnilifeClassesRoomsDiplomDesktop.Properties;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;
using UnilifeClassesRoomsDiplomDesktop.Windows;
using static DevExpress.Xpf.Core.HandleDecorator.Helpers.NativeMethods;

namespace UnilifeClassesRoomsDiplomDesktop.ViewModels
{
    public class ClassPageViewModel : BaseModelOrView
    {
        string _search = "";

        private object _pageFrameView;
        int _classId;
        UnilifeServiceReference.Task _selectedItemTask;
        Account _selectedItemUser;
        private RelayCommand _searchCommand, _addCommand, _delUserCommand;
        public ObservableCollection<UnilifeServiceReference.Task> Tasks { get; set; }
        public ObservableCollection<UnilifeServiceReference.Task> DefaultTasks { get; set; }
        public ObservableCollection<Account> Accounts { get; set; }
        public ObservableCollection<Account> DefaultAccounts { get; set; }

        Visibility _addTaskVisible;
        public Visibility AddTaskVisible
        {
            get { return _addTaskVisible; }
            set { _addTaskVisible = value; OnPropertyChanged(nameof(AddTaskVisible)); }
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
        public UnilifeServiceReference.Task SelectedItemTask
        {
            get { return _selectedItemTask; }
            set
            {
                try
                {
                    _selectedItemTask = value;
                    OnPropertyChanged("SelectedItem");
                    var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");

                    var res = client.СheckingTeacher(_classId, Settings.Default.HashKey);

                    if (res == true)
                    {
                        PageFrameView = new JobsPage(value, _classId);
                    }
                    else
                    {
                        PageFrameView = new TaskPage(value);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public Account SelectedItemUser
        {
            get { return _selectedItemUser; }
            set
            {

                _selectedItemUser = value;
                OnPropertyChanged("SelectedItemUser");
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
                        var result = from d in DefaultTasks
                                     where d.Name.Contains(Search)
                                     select d;

                        Tasks.Clear();
                        foreach (var item in result)
                        {
                            Tasks.Add(item);
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
                            AddTask add = new AddTask(new UnilifeServiceReference.Task(), _classId);
                            if (add.ShowDialog() == true)
                            {

                                DefaultTasks.Add(add.TaskClass);
                                Tasks.Add(add.TaskClass);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }));
            }
        }
        public RelayCommand DelUserCommand
        {
            get
            {
                return _delUserCommand ??
                    (_delUserCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            //var role = (from s in db.Sessions
                            //           join a in db.Accounts on s.AccountId equals a.Id
                            //           join ca in db.ClassAccounts on a.Id equals ca.AccountId
                            //           where s.SessionKey==Settings.Default.HashKey && ca.ClassId== _classId
                            //            select ca).FirstOrDefault();
                            //if (role != null)
                            //{
                            //    if (role.Teacher == true)
                            //    {
                            //        var res = from ca in db.ClassAccounts
                            //                  where ca.AccountId == SelectedItemUser.Id && ca.ClassId == _classId
                            //                  select ca;
                            //        db.ClassAccounts.RemoveRange(res);
                            //        db.SaveChanges();
                            //        DefaultAccounts.Remove(SelectedItemUser);
                            //        Accounts.Remove(SelectedItemUser);
                            //    }
                            //}
                            //else
                            //{
                            //    Settings.Default.HashKey = "";
                            //    Settings.Default.Save();
                            //    MainWindow main = new MainWindow();
                            //    main.Show();
                            //    Application.Current.MainWindow.Close();
                            //}
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }));
            }
        }

        public ClassPageViewModel(int _classId)
        {
            try
            {
                this._classId = _classId;
                var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                var client1 = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");


                DefaultTasks = new ObservableCollection<UnilifeServiceReference.Task>(client.GetTasksClass(_classId).ToList());
                Tasks = new ObservableCollection<UnilifeServiceReference.Task>(DefaultTasks);

                DefaultAccounts = new ObservableCollection<Account>(client1.GetAccountsClass(_classId).ToList());
                foreach (var item in DefaultAccounts)
                {
                    item.User = client.GetUser(item.Id);
                    if (item.User != null)
                    {
                        if (item.User.Photo != null)
                        {
                            item.User.PhotoImage = ToImage(item.User.Photo);
                        }
                    }
                }
                Accounts = new ObservableCollection<Account>(DefaultAccounts);
                var res = client.СheckingTeacher(_classId, Settings.Default.HashKey);
                if (res != true)
                {
                    AddTaskVisible = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

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
