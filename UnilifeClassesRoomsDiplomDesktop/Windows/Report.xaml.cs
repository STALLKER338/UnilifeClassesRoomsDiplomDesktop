using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Xpf.Printing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
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
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;

namespace Unilife_ClasseRoom.Windows
{
    /// <summary>
    /// Логика взаимодействия для Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        private ObservableCollection<Account> Data;
       
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
        public Report(ObservableCollection<Account> Accounts)
        {
            
            InitializeComponent();
            Data = Accounts;
        }
 
    }
}
