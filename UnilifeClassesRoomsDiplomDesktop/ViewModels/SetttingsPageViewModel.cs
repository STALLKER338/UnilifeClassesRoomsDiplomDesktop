using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using UnilifeClassesRoomsDiplomDesktop.Pages;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;
using UnilifeClassesRoomsDiplomDesktop.Windows;

namespace UnilifeClassesRoomsDiplomDesktop.ViewModels
{
    public class SetttingsPageViewModel : INotifyPropertyChanged
    {
        RelayCommand _updateAccountCommand, _updateImageCommand;
        private UserControl DialogControl;

        public SetttingsPageViewModel(Account acc)
        {
            try { 
            Account = acc;
                //Account=(from a in db.Accounts
                //        where a.Id==1
                //        select a).FirstOrDefault();
                //Account.IconImage = ToImage(Account.Icon);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public Account Account { get; set; }
 
        public RelayCommand UpdateAccountCommand
        {
            get
            {
                return _updateAccountCommand ??
                    (_updateAccountCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            Account acc2 = Account;
                            if (Account.IconImage != null)
                            {
                                byte[] data;
                                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create(Account.IconImage));
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    encoder.Save(ms);
                                    data = ms.ToArray();
                                }
                                Account.Icon = data;
                                acc2.IconImage = null;
                            }
                            //db.Entry(Account).State = System.Data.Entity.EntityState.Modified;
                            //db.SaveChanges();
                            var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                            User u = acc2.User;
                            acc2.User = null;
                            client.UpdAccount(acc2);
                            client.Close();
                            acc2.User = u; 
                            Account.IconImage = ToImage(Account.Icon);
                            MessageBox.Show("Настройки успешно измененны.");
                  
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }));
            }
        }
        public RelayCommand UpdateImageCommand
        {
            get
            {
                return _updateImageCommand ??
                    (_updateImageCommand = new RelayCommand(obj =>
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
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }));
            }
        }
        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
