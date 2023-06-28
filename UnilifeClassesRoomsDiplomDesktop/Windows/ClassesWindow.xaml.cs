using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

using UnilifeClassesRoomsDiplomDesktop.Pages;
using UnilifeClassesRoomsDiplomDesktop.Properties;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;

namespace UnilifeClassesRoomsDiplomDesktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для ClassWindow.xaml
    /// </summary>
    public partial class ClassesWindow : Window, INotifyPropertyChanged
    {

        public string Key { get; set; }
  
        public ClassesWindow()
        {
            InitializeComponent();
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (KeyTextBox.Text == null || KeyTextBox.Text =="")
                {
                    MessageBox.Show("Введите код");
                }
                else
                {
                    var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                    bool res = client.AddUserToClass(Settings.Default.HashKey, KeyTextBox.Text);
                    if (res == true)
                    {
                        MessageBox.Show("Вы успешно добавились в класс");
                        DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show("Класс не найден или вы уже в нём");
                        DialogResult = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
