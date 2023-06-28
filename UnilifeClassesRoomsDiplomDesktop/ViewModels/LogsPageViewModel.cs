using DevExpress.Data.Filtering;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UnilifeClassesRoomsDiplomDesktop.UnilifeServiceReference;

namespace UnilifeClassesRoomsDiplomDesktop.ViewModels
{
    public class LogsPageViewModel : BaseModelOrView
    {
        //UnilifeDB db=new UnilifeDB();
        public ObservableCollection<Log> Logs { get; set; }
        public ObservableCollection<Log> DefaultLogs { get; set; }
        public LogsPageViewModel()
        {
            try
            {
                //Logs=new ObservableCollection<Log>(db.Logs);
                //DefaultLogs = new ObservableCollection<Log>(Logs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
