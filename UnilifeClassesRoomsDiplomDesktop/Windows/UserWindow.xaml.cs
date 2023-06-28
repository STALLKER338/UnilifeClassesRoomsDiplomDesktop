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
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public User User { get; set; }
        List<Post> Posts { get; set; }
        List<Division> Divisions { get; set; }
        public UserWindow(User user)
        {
            try
            {
                InitializeComponent();
                User = user;
                DataContext = User;
                var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");

                Posts = new List<Post>(client.GetPosts());
                Divisions = new List<Division>(client.GetDivisions());

                DivisionsComboBox.ItemsSource = Divisions;
                PostsComboBox.ItemsSource = Posts;
                if (User.Division != null)
                {
                    DivisionsComboBox.SelectedItem = Divisions.FirstOrDefault(x => x.Name.Equals(User.Division.Name));
                }
                if (User.Division != null)
                {
                    PostsComboBox.SelectedItem = Posts.FirstOrDefault(x => x.Name.Equals(User.Post.Name));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Configure open file dialog box
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.FileName = ""; // Default file name
                dlg.DefaultExt = ""; // Default file extension
                dlg.Filter = "Photo (*.png,*.jpg)|*.png;*.jpg"; // Filter files by extension

                // Show open file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    // Open document
                    string filename = dlg.FileName;
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(filename);
                    logo.EndInit();
                    User.PhotoImage = logo;
                    PhotoUser.Source = logo;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (User.Name==null)
            {
                MessageBox.Show("Поле имя не заполнено");
            }
            else if(User.Birthday == null)
            {
                MessageBox.Show("Поле дня рождения не заполнено");
            }
            else if (User.Division == null)
            {
                MessageBox.Show("Поле подразделения не заполнено");
            }
            else if (User.Post == null)
            {
                MessageBox.Show("Поле должности не заполнено");
            }
            else
            {
                DialogResult = true;
            }
        }
    }
}
