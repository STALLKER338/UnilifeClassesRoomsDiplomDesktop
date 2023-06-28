using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;
using UnilifeClassesRoomsDiplomDesktop.Windows;

namespace UnilifeClassesRoomsDiplomDesktop.ViewModels
{
    public class PostsPageViewModel: INotifyPropertyChanged
    {
        string _search = "";
        RelayCommand _searchCommand, _addCommand, _editCommand;
        Post _selectedItem;

        public PostsPageViewModel()
        {
            try
            {
                var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");

                DefaultPosts = new ObservableCollection<Post>(client.GetPosts());
                Posts = new ObservableCollection<Post>(DefaultPosts);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ObservableCollection<Post> Posts { get; set; }
        public ObservableCollection<Post> DefaultPosts { get; set; }

        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                OnPropertyChanged("Search");
            }
        }
        public Post SelectedItem
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
                        var result = from d in DefaultPosts
                                     where d.Name.Contains(Search)
                                     select d;

                        Posts.Clear();
                        foreach (var item in result)
                        {
                            Posts.Add(item);
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
                    try { 
                    PostWindow postWindow = new PostWindow(new Post());
                    if (postWindow.ShowDialog() == true)
                    {

                        Post post = postWindow.Post;

                        var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                        post.Id=client.AddPost(post);

                        DefaultPosts.Add(post);
                        Posts.Add(post);

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
                        try { 
                        Post post = selectedItem as Post;
                        if (post == null) return;

                        Post vm = new Post
                        {
                            Id = post.Id,
                            Name = post.Name,
                            Active = post.Active,

                        };

                        PostWindow accountsWindow = new PostWindow(vm);
                        if (accountsWindow.ShowDialog() == true)
                        {

                            post.Name = accountsWindow.Post.Name;
                            post.Active = accountsWindow.Post.Active;
                            var client = new UnilifeServiceReference.UnilifeClassesRoomsDiplomServerDDLClient("NetTcpBinding_IUnilifeClassesRoomsDiplomServerDDL");
                            client.UpdPost(post);
                           
                        }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
