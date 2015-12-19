using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;

namespace WordMonitorApp.ViewModels
{
    public class WordGraphInfo : NotificationObject
    {
        private string _graphInfo;
        public string GraphInfo
        {
            get
            {
                return _graphInfo;
            }
            set
            {
                _graphInfo = value;
                RaisePropertyChanged("GraphInfo");
            }
        }
    }
}
