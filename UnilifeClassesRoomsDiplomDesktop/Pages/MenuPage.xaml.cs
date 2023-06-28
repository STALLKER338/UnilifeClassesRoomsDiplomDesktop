using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using UnilifeClassesRoomsDiplomDesktop.ViewModels;

namespace UnilifeClassesRoomsDiplomDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
            DataContext = new MenuPageViewModel();
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
