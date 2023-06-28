using MaterialDesignThemes.Wpf;
using System.Windows.Controls;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;
using UnilifeClassesRoomsDiplomDesktop.ViewModels;
using UnilifeClassesRoomsDiplomDesktop.Windows;

namespace UnilifeClassesRoomsDiplomDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClassesPage.xaml
    /// </summary>
    public partial class ClassesPage : Page
    {
        public ClassesPage(Account acc)
        {
            InitializeComponent();
            DataContext = new ClassesPageViewModel(acc);
        }
    }
}
