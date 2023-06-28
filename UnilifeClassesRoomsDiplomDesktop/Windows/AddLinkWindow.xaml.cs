using System;
using System.Collections.Generic;
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

namespace UnilifeClassesRoomsDiplomDesktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddLinkWindow.xaml
    /// </summary>
    public partial class AddLinkWindow : Window
    {
        public LinksTask LinksTask { get; set; }
        public AddLinkWindow(LinksTask _linkTask)
        {
            try
            {
                InitializeComponent();
                LinksTask = _linkTask;
                DataContext = LinksTask;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (LinksTask.Link == null)
            {
                MessageBox.Show("Поле ccылки не заполнено");
            }
            else
            {
                DialogResult = true;
            }
        }
    }
}
