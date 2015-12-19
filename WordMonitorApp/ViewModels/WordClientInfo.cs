using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using System.Collections.ObjectModel;

namespace WordMonitorApp.ViewModels
{
    public class WordClientInfo : NotificationObject
    {
        public string Ip { get; set; }

        public int Port { get; set; }

        public string ClientInfo 
        {
            get
            {
                return string.Format("{0} {1}", Ip, Port);
            }
        }


        public ObservableCollection<WordGraphInfo> WordGraphs { get; private set; }

        public WordClientInfo()
        {
            WordGraphs = new ObservableCollection<WordGraphInfo>();
        }
    }
}
