using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;
using UnilifeClassesRoomsDiplomDesktop.Properties;

namespace UnilifeClassesRoomsDiplomDesktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddTask.xaml
    /// </summary>
    public partial class AddTask : Window
    {
        public UnilifeServiceReference.Task TaskClass { get; set; }
        List<FilesTask> FilesTasks { get; set; }
        List<LinksTask> LinksTasks { get; set; }
        public AddTask(UnilifeServiceReference.Task _task, int _idClass)
        {
            try
            {
                InitializeComponent();
                TaskClass = _task;
                TaskClass.ClassId = _idClass;
                FilesTasks = new List<FilesTask>();
                LinksTasks = new List<LinksTask>();
                DataContext = TaskClass;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TaskClass.Name == null || TaskClass.Name.Length == 0)
                {
                    MessageBox.Show("Заполните название задания");
                }
                else if(TaskClass.MaxScore<=0)
                {
                    MessageBox.Show("Максимальный бал не может быть равен 0 или меньше 0.");
                }
                else if (TaskClass.Description == null)
                {
                    MessageBox.Show("Описание не может быть пустым");
                }
                else if (TaskClass.DelivaryTime < DateTime.Now)
                {
                    MessageBox.Show("Срок сдачи не может быть меньше чем текущее время.");
                }
                else
                {
                    TaskClass.CreateTime = DateTime.Now;
                    var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                    TaskClass.Id = client.AddTask(TaskClass, Settings.Default.HashKey);
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addFile_Click(object sender, RoutedEventArgs e)
        {
            try
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
                    FilesTask file = new FilesTask();
                    file.Name = dlg.FileName.Substring(dlg.FileName.LastIndexOf('\\') + 1);

                    using (System.IO.FileStream fs = new System.IO.FileStream(dlg.FileName, FileMode.Open))
                    {
                        file.File = new byte[fs.Length];
                        fs.Read(file.File, 0, file.File.Length);
                    }
                    FilesTasks.Add(file);
                    TaskClass.FilesTasks = FilesTasks.ToArray();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addLink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddLinkWindow addLinkWindow = new AddLinkWindow(new LinksTask());
                if (addLinkWindow.ShowDialog() == true)
                {
                    LinksTasks.Add(addLinkWindow.LinksTask);
                    TaskClass.LinksTasks = LinksTasks.ToArray();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
