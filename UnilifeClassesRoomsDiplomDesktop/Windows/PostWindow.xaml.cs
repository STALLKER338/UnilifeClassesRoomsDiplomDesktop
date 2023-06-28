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
    /// Логика взаимодействия для PostWindow.xaml
    /// </summary>
    public partial class PostWindow : Window
    {
        public Post Post { get; set; }
        public PostWindow(Post post)
        {
            InitializeComponent();
            Post=post;
            DataContext= Post;
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (Post.Name == null)
            {
                MessageBox.Show("Поле название должности не заполнено");
            }
            else
            {
                DialogResult = true;
            }
        }
    }
}
