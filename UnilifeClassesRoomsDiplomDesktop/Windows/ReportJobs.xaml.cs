using DevExpress.Xpf.Printing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using UnilifeClassesRoomsDiplomDesktop.Properties;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;
using UnilifeClassesRoomsDiplomDesktop.ViewModels;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace UnilifeClassesRoomsDiplomDesktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для ReportJobs.xaml
    /// </summary> 
 
    public partial class ReportJobs : Window
    {

        private ObservableCollection<Rep> Data;
        UnilifeServiceReference.Task Task{get;set;}
        UnilifeServiceReference.Class Class { get; set; }
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            // Create a link and specify templates for various document sections.
            SimpleLink CategoryLink = new SimpleLink();
            CategoryLink.ReportHeaderTemplate = (DataTemplate)Resources["HeaderTemplate"];
            CategoryLink.DetailTemplate = (DataTemplate)Resources["ProductsTemplate"];

            // Specify the link's DetailCount property, which defines the number of records to display.
            // The following code will print all data available in the data source for the detail section.
            CategoryLink.DetailCount = Data.Count;

            // Assign the created link to the DocumentSource property of the Document Preview control.
            preview.DocumentSource = CategoryLink;

            // Create a detail section for every entry in your data source and generate the document.
            CategoryLink.CreateDetail += link_CreateDetail;
            CategoryLink.CreateDocument(true);
        }

        void link_CreateDetail(object sender, CreateAreaEventArgs e)
        {
            e.Data = Data[e.DetailIndex];
        }
        public ReportJobs(List<User> users, UnilifeServiceReference.Task task)
        {
            Class=new Class();
            Class.Name = "123";
            ObservableCollection<Rep> reps = new ObservableCollection<Rep>();
            Task=new UnilifeServiceReference.Task();
            Task.Name = "ЛР2";
            foreach (User user in users) 
            {
                Rep byf= new Rep();
                byf.User= user;
                var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                byf.Job = client.GetJobUser(user.Id, task.Id);

                if (byf.Job == null)
                {
                    byf.Job=new Job();
                    byf.Job.Score = 0;
                }
                reps.Add(byf);
            }
            InitializeComponent();
            Data = reps;
        }
        class Rep : BaseModelOrView
        {
            User user;
            Job job;
            public User User
            {
                get { return user; }
                set
                {
                    user = value;
                    OnPropertyChanged("User");
                }
            }
            public Job Job
            {
                get { return job; }
                set
                {
                    job = value;
                    OnPropertyChanged(nameof(Job));
                }
            }
        }
    }
}
