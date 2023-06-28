using DevExpress.XtraRichEdit.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;
using UnilifeClassesRoomsDiplomDesktop.Windows;

namespace UnilifeClassesRoomsDiplomDesktop.ViewModels
{
    public class UsersPageViewModel : BaseModelOrView
    {
        string _search = "";

        RelayCommand _searchCommand, _addCommand, _editCommand;
        User _selectedItem;

        public UsersPageViewModel()
        {
            try { 
            var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
            var client1 = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");

            DefaultUsers = new ObservableCollection<User>(client.GetUsers());
            foreach (var u in DefaultUsers)
            {
                u.Division = client1.GetDivision(u.DivisionId);
                u.Post = client1.GetPost(u.PostId);
                if (u.Photo != null)
                {
                    u.PhotoImage = ToImage(u.Photo);
                }
            }
            Users = new ObservableCollection<User>(DefaultUsers);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<User> DefaultUsers { get; set; }

        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                OnPropertyChanged("Search");
            }
        }
        public User SelectedItem
        {
            get { return _selectedItem; }
            set
            {

                _selectedItem = value;
                OnPropertyChanged("SelectedItem");

            }
        }
        public RelayCommand SearchCommand
        {
            get
            {
                return _searchCommand ??
                    (_searchCommand = new RelayCommand(obj =>
                    {
                        var result = from d in DefaultUsers
                                     where d.Name.Contains(Search)
                                     select d;

                        Users.Clear();
                        foreach (var item in result)
                        {
                            Users.Add(item);
                        }
                    }));
            }
        }
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ??
                (_addCommand = new RelayCommand(obj =>
                {
                    try
                    {
                        UserWindow divisionWindow = new UserWindow(new User());
                        if (divisionWindow.ShowDialog() == true)
                        {
                            BitmapImage PhotoByf = new BitmapImage();
                            User user = divisionWindow.User;
                            if (user.PhotoImage != null)
                            {
                                byte[] data;
                                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create(divisionWindow.User.PhotoImage));
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    encoder.Save(ms);
                                    data = ms.ToArray();
                                }
                                user.Photo = data;
                                PhotoByf = user.PhotoImage;
                                user.PhotoImage = null;
                            }
                            Division d = new Division();
                            Post p = new Post();
                            if (user.Post != null)
                            {
                                p = user.Post;
                                user.PostId = user.Post.Id;
                                user.Post = null;
                            }
                            if (user.Division != null)
                            {
                                d = user.Division;
                                user.DivisionId = user.Division.Id;
                                user.Division = null;
                            }
                            var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                            user.Id = client.AddUser(user);
                            user.PhotoImage = PhotoByf;
                            user.Post = p;
                            user.Division = d;
                            DefaultUsers.Add(user);
                            Users.Add(user);

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }));
            }
        }
        public RelayCommand EditCommand
        {
            get
            {
                return _editCommand ??
                    (_editCommand = new RelayCommand((selectedItem) =>
                    {
                        try
                        {
                            User user = selectedItem as User;
                            if (user == null) return;

                            User vm = new User
                            {
                                Id = user.Id,
                                Name = user.Name,
                                Birthday = user.Birthday,
                                Active = user.Active,
                                Division = user.Division,
                                PostId=user.PostId,
                                DivisionId=user.DivisionId,
                                Post = user.Post,
                                Photo = user.Photo,
                                PhotoImage = user.PhotoImage,
                            };

                            UserWindow accountsWindow = new UserWindow(vm);
                            if (accountsWindow.ShowDialog() == true)
                            {

                                user.Name = accountsWindow.User.Name;
                                user.Birthday = accountsWindow.User.Birthday;
                                user.Active = accountsWindow.User.Active;


                                user.PhotoImage = accountsWindow.User.PhotoImage;
                                if (accountsWindow.User.PhotoImage != null)
                                {
                                    byte[] data;
                                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                                    encoder.Frames.Add(BitmapFrame.Create(accountsWindow.User.PhotoImage));
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        encoder.Save(ms);
                                        data = ms.ToArray();
                                    }
                                    user.Photo = data;
                                    user.PhotoImage = null;
                                }
                                if (accountsWindow.User.Post != null)
                                {
                                    user.PostId = accountsWindow.User.Post.Id;
                                    user.Post = null;
                                }
                                if (accountsWindow.User.Division != null)
                                {
                                    user.DivisionId = accountsWindow.User.Division.Id;
                                    user.Division = null;
                                }
                                var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                                client.UpdUser(user);

                                user.PhotoImage = accountsWindow.User.PhotoImage;
                                if (accountsWindow.User.Division != null)
                                {
                                    user.Division = accountsWindow.User.Division;
                                }
                                if (accountsWindow.User.Post != null)
                                {
                                    user.Post = accountsWindow.User.Post;
                                }
                                if(accountsWindow.User.PhotoImage!=null)
                                {
                                    user.PhotoImage = accountsWindow.User.PhotoImage;
                                }
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
    }
}
