//Server Setting ViewModel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Prism.Commands;

namespace WordMonitorApp
{
    public class ServerSettingViewModel : NotificationObject
    {
        private bool? _dialogResult;
        public bool? DialogResult
        {
            get
            {
                return _dialogResult;
            }
            set
            {
                _dialogResult = value;
                RaisePropertyChanged("DialogResult");
            }
        }


        

        private string _serverIp;
        public string ServerIp
        {
            get
            {
                return _serverIp;
            }
            set
            {
                _serverIp = value;
                RaisePropertyChanged("ServerIp");
            }
        }

        private int _serverPort;

        public int ServerPort
        {
            get
            {
                return _serverPort;
            }
            set
            {
                _serverPort = value;
                RaisePropertyChanged("ServerPort");
            }
        }


        private string _tips;
        public string Tips
        {
            get
            {
                return _tips;
            }
            set
            {
                _tips = value;
                RaisePropertyChanged("Tips");
            }
        }

        public ICommand StartServerCommand { get; private set; }

        private void StartServerCommandExecute()
        {
            try
            {
                TcpServerCenter.Instance.DoInit(ServerIp, ServerPort);

                DialogResult = true;
            }
            catch
            {
                Tips = "连接失败！";
            }
            

            
        }

        public ServerSettingViewModel()
        {
            ServerIp = "127.0.0.1";
            ServerPort = 6118;

            StartServerCommand = new DelegateCommand(StartServerCommandExecute);
        }
    }
}
