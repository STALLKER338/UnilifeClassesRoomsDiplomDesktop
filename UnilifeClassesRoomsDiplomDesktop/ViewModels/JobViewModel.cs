using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnilifeClassesRoomsDiplomDesktop.Pages;
using UnilifeClassesRoomsDiplomDesktop.Properties;
using System.Windows;
using System.IO;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;

namespace UnilifeClassesRoomsDiplomDesktop.ViewModels
{
    public class JobViewModel : INotifyPropertyChanged
    {

        RelayCommand _sendScore;
        FilesJob _selectedFileJob;

        public JobViewModel(UnilifeServiceReference.Task task, User user)
        {
            try
            { 
            Task = task;
            User = user; 
            var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
            var client1 = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
            Job = client.GetJobUser(User.Id,Task.Id);

            if (Job != null)
            {           
                FileJobs = new ObservableCollection<FilesJob>(client1.GetFilesJob(Job.Id));
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public ObservableCollection<FilesJob> FileJobs { get; set; }
        public Job Job { get; set; }
        public User User { get; set; }
        public FilesJob SelectedFileJob
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
        
        public UnilifeServiceReference.Task Task { get; set; }
        public RelayCommand SendScore
        {
            get
            {
                return _sendScore ??
                    (_sendScore = new RelayCommand(obj =>
                    {
                        try
                        {
                            if (Job != null)
                            {
                                if (Job.Score > Task.MaxScore || Job.Score < 0)
                                {
                                    MessageBox.Show("Дааная отметка не может быть поставлена так как больше максимального бала за данное задание или меньше 0.");
                                }
                                else
                                {
                                    var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                                    client.UpdJob(Job);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Данный пользователь не отправлял работу");
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
