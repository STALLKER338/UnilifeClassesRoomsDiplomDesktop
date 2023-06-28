using System;
using System.Linq;
using System.Windows.Controls;
using UnilifeClassesRoomsDiplomDesktop.ViewModels;

namespace UnilifeClassesRoomsDiplomDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AccountsPage.xaml
    /// </summary>
    public partial class AccountsPage : Page
    {
        public AccountsPage()
        {
            InitializeComponent();
            DataContext = new AccountsPageViewModel();
        }
    }
}
