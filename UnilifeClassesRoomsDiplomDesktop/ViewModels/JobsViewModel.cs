using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unilife_ClasseRoom.Windows;
using UnilifeClassesRoomsDiplomDesktop.Pages;
using UnilifeClassesRoomsDiplomDesktop.Properties;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;
using UnilifeClassesRoomsDiplomDesktop.Windows;

namespace UnilifeClassesRoomsDiplomDesktop.ViewModels
{
    public class JobsViewModel : BaseModelOrView
    {
        private object _menuFrameView, _pageFrameView;

        string _message;
        User _selectedItem;
        LinksTask _selectedLinksTask;
        FilesTask _selectedFilesTask;
        private RelayCommand _sendMessage, _reportCommand;

        public RelayCommand ReportCommand
        {
            get
            {
                return _reportCommand ??
                   (_reportCommand = new RelayCommand(obj =>
                   {
                       ReportJobs rep = new ReportJobs(Users.ToList(), Task);
                       rep.Show();
                   }));
            }
        }
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        public UnilifeServiceReference.Task Task { get; set; }
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
        public User SelectedItem
        {
            get { return _selectedItem; }
            set
            {

                MenuFrameView = new JobPage(Task, value);
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");

            }
        }
        public LinksTask SelectedLinksTask
        {
            get { return _selectedLinksTask; }
            set
            {
                try
                {
                    _selectedLinksTask = value;
                    System.Diagnostics.Process.Start(_selectedLinksTask.Link);

                    _selectedLinksTask = value;
                    OnPropertyChanged("SelectedLinksTask");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public FilesTask SelectedFilesTask
        {
            get { return _selectedFilesTask; }
            set
            {
                try { 
                _selectedFilesTask = value;
                string fileName = String.Format($"../../Files/{_selectedFilesTask.Name}");

                using (File.Create(fileName)) ;
                File.WriteAllBytes(fileName, _selectedFilesTask.File);
                string fullFileName = System.IO.Path.GetFullPath(fileName);

                System.Diagnostics.Process.Start(fullFileName);

                OnPropertyChanged("SelectedFilesTask");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);                 
                }
            }
        }
        public RelayCommand SendMessage
        {
            get
            {
                return _sendMessage ??
                   (_sendMessage = new RelayCommand(obj =>
                   {
                       try
                       {
                           MessagesTask m = new MessagesTask();
                           m.Message = Message;
                           m.Time = DateTime.Now;
                           m.TaskId = Task.Id;
                           var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                           var client1 = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                           int byf = client.AddMessage(m, Settings.Default.HashKey);
                           m.Account = client1.GetAccount(byf);
                           m.Account.User = client.GetUser(m.Account.UserId);
                           Messags.Add(m);

                           Message = "";
                       }
                       catch (Exception ex)
                       {
                           MessageBox.Show(ex.Message);
                       }
                   }));
            }
        }

        public ObservableCollection<MessagesTask> Messags { get; set; }
        public ObservableCollection<User> Users { get; set; }

        public JobsViewModel(UnilifeServiceReference.Task _task, int _classId)
        {
            try
            {
                var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                var client1 = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");

                Messags = new ObservableCollection<MessagesTask>(client.GetMessages(_task.Id));
                foreach (var m in Messags)
                {
                    m.Account = client1.GetAccount(m.AccountId);
                    m.Account.User = client.GetUser(m.Account.UserId);
                }
                Users = new ObservableCollection<User>(client.GetUsersClass(_classId));
           
                Task = _task;
                Task.FilesTasks = client.GetFileTask(_task.Id);
                Task.LinksTasks = client.GetLinksTask(_task.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
