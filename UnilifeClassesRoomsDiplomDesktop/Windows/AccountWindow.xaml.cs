using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;

namespace UnilifeClassesRoomsDiplomDesktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : Window
    {
        public Account Account { get; private set; }
        List<Role> Roles { get; set; }
        List<User> Users { get; set; }
        public AccountWindow(Account account)
        {
            try
            {
                InitializeComponent();
                Account = account;
                DataContext = Account;
                //using (UnilifeDB db = new UnilifeDB())
                //{
                //    Roles = new List<Role>(db.Roles);
                //    Users = new List<User>(db.Users);
                //}
                var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                Roles = new List<Role>(client.GetRoles());
                Users = new List<User>(client.GetUsers());
                RolesAccountsComboBox.ItemsSource = Roles;
                if (Account.Role != null)
                {
                    RolesAccountsComboBox.SelectedItem = Roles.FirstOrDefault(x => x.Name.Equals(Account.Role.Name));
                }
                UsersAccountsComboBox.ItemsSource = Users;
                if (Account.User != null)
                {
                    UsersAccountsComboBox.SelectedItem = Users.FirstOrDefault(x => x.Name.Equals(Account.User.Name));
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
                    Account.IconImage = logo;
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
            if (Account.Login == null)
            {
                MessageBox.Show("Поле логин аккаунта не заполнено");
            }
            else if (Account.Password == null)
            {
                MessageBox.Show("Поле пароля аккаунта не заполнено");
            }
            else if (Account.Mail == null)
            {
                MessageBox.Show("Поле почты аккаунта не заполнено");
            }
            else if (Account.Role == null)
            {
                MessageBox.Show("Поле роли аккаунта не заполнено");
            }
            else if (Account.Role == null)
            {
                MessageBox.Show("Поле пользователя аккаунта не заполнено");
            }
            else
            {
                DialogResult = true;
            }
        }
    }
}
