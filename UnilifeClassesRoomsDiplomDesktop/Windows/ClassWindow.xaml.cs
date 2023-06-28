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
    /// Логика взаимодействия для ClassWindow.xaml
    /// </summary>
    public partial class ClassWindow : Window
    {
        public Class Class { get; set; }
        public ClassWindow(Class class1)
        {
            InitializeComponent();
            Class = class1;
            DataContext = Class;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (Class.Name == null)
            {
                MessageBox.Show("Поле название класса не заполнено");
            }
            else if (Class.KeyClass == null)
            {
                MessageBox.Show("Поле ключ класса не заполнено");
            }
            else
            {
                DialogResult = true;
            }
        }
    }
}
