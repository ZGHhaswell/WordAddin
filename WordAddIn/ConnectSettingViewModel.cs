//Tcp Connect ViewModel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Prism.Commands;

namespace WordAddIn
{
    public class ConnectSettingViewModel : NotificationObject
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


        private string _clientIp;
        public string ClientIp
        {
            get
            {
                return _clientIp;
            }
            set
            {
                _clientIp = value;
                RaisePropertyChanged("ClientIp");
            }
        }

        private int _clientPort;

        public int ClientPort
        {
            get
            {
                return _clientPort;
            }
            set
            {
                _clientPort = value;
                RaisePropertyChanged("ClientPort");
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

        public ICommand ConnectCommand { get;private set; }

        private void ConnectCommandExecute()
        {
            try
            {
                TcpClientCenter.Instance.DoInit(ClientIp, ClientPort);
                TcpClientCenter.Instance.Connect(ServerIp, ServerPort);

                DialogResult = true;
            }
            catch
            {
                Tips = "连接失败！";
            }
            

            
        }

        public ICommand CancelCommand { get; private set; }

        private void CancelCommandExecute()
        {
            DialogResult = false;
        }

        
        public ConnectSettingViewModel()
        {
            ClientIp = "192.168.1.115";
            ClientPort = 3001;
            ServerIp = "192.168.1.101";
            ServerPort = 6118;

            ConnectCommand = new DelegateCommand(ConnectCommandExecute);
            CancelCommand = new DelegateCommand(CancelCommandExecute);
        }
    }
}
