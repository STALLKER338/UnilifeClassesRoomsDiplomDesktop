using System.Windows.Controls;
using UnilifeClassesRoomsDiplomDesktop.ViewModels;

namespace UnilifeClassesRoomsDiplomDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для ConfirmPage.xaml
    /// </summary>
    public partial class ConfirmPage : Page
    {
        public ConfirmPage()
        {
            InitializeComponent();
            DataContext = new ConfirmPageViewModel();
        }
    }
}
