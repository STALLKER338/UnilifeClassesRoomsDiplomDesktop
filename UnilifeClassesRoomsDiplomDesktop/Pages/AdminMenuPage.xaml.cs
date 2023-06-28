using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;
using UnilifeClassesRoomsDiplomDesktop.ViewModels;

namespace UnilifeClassesRoomsDiplomDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminMenuPage.xaml
    /// </summary>
    public partial class AdminMenuPage : Page
    {
        public AdminMenuPage(Account acc)
        {
            InitializeComponent();
            DataContext = new AdminMenuPageViewModel(acc);
        }
        private void Anim1(object sender, RoutedEventArgs e)
        {
            if (Menu.ActualWidth <= 100)
            {
                DoubleAnimation buttonAnimation = new DoubleAnimation();
                buttonAnimation.From = Menu.ActualWidth;
                buttonAnimation.To = 200;
                buttonAnimation.Duration = TimeSpan.FromSeconds(0.2);
                Menu.BeginAnimation(Button.WidthProperty, buttonAnimation);
            }
            else if (Menu.ActualWidth >= 150)
            {
                DoubleAnimation buttonAnimation = new DoubleAnimation();
                buttonAnimation.From = Menu.ActualWidth;
                buttonAnimation.To = 50;
                buttonAnimation.Duration = TimeSpan.FromSeconds(0.2);
                Menu.BeginAnimation(Button.WidthProperty, buttonAnimation);
            }
        }
    }
}
