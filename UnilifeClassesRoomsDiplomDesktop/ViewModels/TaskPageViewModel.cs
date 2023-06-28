using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;
using UnilifeClassesRoomsDiplomDesktop.Pages;
using UnilifeClassesRoomsDiplomDesktop.Properties;
using UnilifeClassesRoomsDiplomDesktop.Windows;

namespace UnilifeClassesRoomsDiplomDesktop.ViewModels
{
    public class TaskPageViewModel : BaseModelOrView
    {
        private object _pageFrameView;
        bool send = false;
        int _id;
        string _message;
        string _sendJobButton;

        RelayCommand _deleteFile, _sendMessage, _sendJob, _addFile;
        LinksTask _selectedLinkTask;
        LinksJob _selectedLinkJob;
        FilesJob _selectedFileJob;
        FilesTask _selectedFileTask;
        public object PageFrameView
        {
            get { return _pageFrameView; }
            set
            {
                _pageFrameView = value;
                OnPropertyChanged("PageFrameView");
            }
        }
        public TaskPageViewModel(UnilifeServiceReference.Task _task)
        {
            try
            {
                Task = _task;
                FilesJobs = new ObservableCollection<FilesJob>();

                var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                var client1 = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                Task.FilesTasks = client1.GetFileTask(_task.Id);
                Task.LinksTasks = client1.GetLinksTask(_task.Id);
                Job = client.GetJob(Settings.Default.HashKey, Task.Id);

                if (Job != null)
                {
                    Job.FilesJobs = client1.GetFilesJob(Job.Id);
                }
                Messags = new ObservableCollection<MessagesTask>(client.GetMessages(_task.Id));
                foreach (var m in Messags)
                {
                    m.Account = client1.GetAccount(m.AccountId);
                    m.Account.User = client.GetUser(m.Account.UserId);
                }

                if (Job == null)
                {
                    SendJobButton = "Отправить";

                    Job = new Job();
                    Job.AccountId = _id;
                    Job.TaskId = _task.Id;
                }
                else
                {
                    SendJobButton = "Отменить";
                    send = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
        public Job Job { get; set; }
        public ObservableCollection<FilesJob> FilesJobs { get; set; }
        //public ObservableCollection<FilesTask> FilesTasks { get; set; }
        ////public ObservableCollection<LinksJob> LinksJobs { get; set; }
        //public ObservableCollection<LinksTask> LinksTasks { get; set; }
        public User User { get; set; }
        public ObservableCollection<MessagesTask> Messags { get; set; }
        public FilesTask SelectedFileTask
        {
            get
            {
                return _selectedFileTask;
            }
            set
            {
                try {
                    _selectedFileTask = value;

                    string fileName = String.Format($"../../Files/{_selectedFileTask.Name}");

                    using (File.Create(fileName)) ;
                    File.WriteAllBytes(fileName, _selectedFileTask.File);
                    string fullFileName = System.IO.Path.GetFullPath(fileName);

                    System.Diagnostics.Process.Start(fullFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public FilesJob SelectedFilesJob
        {
            get
            {
                return _selectedFileJob;
            }
            set
            {
                try
                {
                    _selectedFileJob = value;
                    if (_selectedFileJob != null)
                    {
                        string fileName = String.Format($"../../Files/{_selectedFileJob.Name}");

                        using (File.Create(fileName)) ;
                        File.WriteAllBytes(fileName, _selectedFileJob.File);
                        string fullFileName = System.IO.Path.GetFullPath(fileName);

                        System.Diagnostics.Process.Start(fullFileName);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public LinksTask SelectedLinkTask
        {
            get
            {
                return _selectedLinkTask;
            }
            set
            {
                _selectedLinkTask = value;
                System.Diagnostics.Process.Start(_selectedLinkTask.Link);
            }
        }
        public LinksJob SelectedLinkJob
        {
            get
            {
                return _selectedLinkJob;
            }
            set
            {
                _selectedLinkJob = value;
                System.Diagnostics.Process.Start(_selectedLinkJob.Link);
            }
        }
        public string SendJobButton 
        {
            get { return _sendJobButton; }
            set { _sendJobButton=value; OnPropertyChanged(nameof(SendJobButton)); }
        }
        public RelayCommand SendJob
        {
            get
            {
                return _sendJob ??
                    (_sendJob = new RelayCommand(obj =>
                    {

                        try
                        {
                            var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                            if (send == false)
                            {

                                Job.Deleted = false;
                                Job.Time = DateTime.Now;
                       
                                Job.Id= client.AddJob(Job,Settings.Default.HashKey);
                                //    db.Jobs.Add(Job);
                                //    db.SaveChanges();

                                SendJobButton = "Отменить";
                                send = true;

                                PageFrameView = new TaskPage(Task);
                            }
                            else
                            {
                                
                                Job.Deleted = true;
                                client.UpdJob(Job);
                                SendJobButton = "Отправить";
                                FilesJobs = new ObservableCollection<FilesJob>();
                                send = false;
                                PageFrameView = new TaskPage(Task);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }));
            }
        }
        public RelayCommand AddFile
        {
            get
            {
                return _addFile ??
                    (_addFile = new RelayCommand(obj =>
                    {
                        try
                        {
                            if (send == false)
                            {
                                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                                dlg.FileName = ""; // Default file name
                                dlg.DefaultExt = ""; // Default file extension
                                dlg.Filter = "All files (*.*)|*.*"; // Filter files by extension

                                // Show open file dialog box
                                Nullable<bool> result = dlg.ShowDialog();

                                // Process open file dialog box results
                                if (result == true)
                                {
                                    FilesJob file = new FilesJob();
                                    file.Name = dlg.FileName.Substring(dlg.FileName.LastIndexOf('\\') + 1); // cats.jpg

                                    using (System.IO.FileStream fs = new System.IO.FileStream(dlg.FileName, FileMode.Open))
                                    {
                                        file.File = new byte[fs.Length];
                                        fs.Read(file.File, 0, file.File.Length);
                                    }
                                    FilesJobs.Add(file);
                                    Job.FilesJobs = FilesJobs.ToArray();
                                   
                                    //    // Open document
                                    //    string filename = dlg.FileName;
                                    //    BitmapImage logo = new BitmapImage();
                                    //    logo.BeginInit();
                                    //    logo.UriSource = new Uri(filename);
                                    //    logo.EndInit();
                                    //    User.Photo = logo;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Нельзя изменять файлы когда работа отправлена на проверку.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }));
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
        public RelayCommand AddLink
        {
            get
            {
                return _addFile ??
                    (_addFile = new RelayCommand(obj =>
                    {
                        try {
                            //AddLinkWindow addLinkWindow = new AddLinkWindow(new LinksTask());
                            //if (addLinkWindow.ShowDialog() == true)
                            //{
                            //    LinksJob l=new LinksJob();
                            //    l.Link = addLinkWindow.LinksTask.Link;
                            //    Job.LinksJobs.Add(l);
                            //}
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }));
            }
        }
        public RelayCommand DeleteFile
        {
            get
            {
                return _deleteFile ??
                    (_deleteFile = new RelayCommand(obj =>
                    {
                        try {
                            if (send == false)
                            {
                                var buf = SelectedFilesJob;
                                SelectedFilesJob = null;
                                FilesJobs.Remove(buf);
                                Job.FilesJobs = FilesJobs.ToArray();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }));
            }
        }
        public RelayCommand DeleteLink
        {
            get
            {
                return _deleteFile ??
                    (_deleteFile = new RelayCommand(obj =>
                    {
                        try { 
                        if (send == false)
                        {
                           // Job.FilesJobs.Remove(SelectedFilesJob);
                        }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }));
            }
        }
    }
}
