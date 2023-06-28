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
    /// Логика взаимодействия для UserToClassWindow.xaml
    /// </summary>
    public partial class UserToClassWindow : Window
    {
        public ClassAccount ClassAccount { get; set; }
        List<Account> Accounts;
        public UserToClassWindow(ClassAccount classAccount)
        {
            InitializeComponent();
            ClassAccount=classAccount;
            DataContext = ClassAccount;
            var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
            var client1 = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
            Accounts = new List<Account>(client.GetAccounts());
            foreach (var account in Accounts)
            {
                account.User = client1.GetUser(account.UserId);
            }
            AccountsComboBox.ItemsSource = Accounts;
            if (ClassAccount.Account != null)
            {
                AccountsComboBox.SelectedItem = Accounts.FirstOrDefault(x => x.Login.Equals(ClassAccount.Account.Login));
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if(ClassAccount.Account==null)
            {
                MessageBox.Show("Выберите пользователя");
            }
            else
            {
              
                DialogResult = true;
                
            }
        }
    }
}
