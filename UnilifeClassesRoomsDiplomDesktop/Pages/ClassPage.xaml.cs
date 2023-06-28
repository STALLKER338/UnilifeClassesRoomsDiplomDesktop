using System;
using System.Linq;
using System.Windows.Controls;
using UnilifeClassesRoomsDiplomDesktop.ViewModels;

namespace UnilifeClassesRoomsDiplomDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClassPage.xaml
    /// </summary>
    public partial class ClassPage : Page
    {
        public ClassPage(int _classId)
        {
            InitializeComponent();
            DataContext = new ClassPageViewModel(_classId);
        }


    }
}
