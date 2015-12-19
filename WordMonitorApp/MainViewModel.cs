using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using System.Collections.ObjectModel;
using WordMonitorApp.ViewModels;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using TcpUtils;

namespace WordMonitorApp
{
    public class MainViewModel : NotificationObject
    {
        private bool? _dialogReuslt;
        public bool? DialogResult
        {
            get
            {
                return _dialogReuslt;
            }
            set
            {
                _dialogReuslt = value;
                RaisePropertyChanged("DialogResult");
            }
        }

        private ObservableCollection<WordClientInfo> _clients;
        public ObservableCollection<WordClientInfo> Clients
        {
            get
            {
                return _clients;
            }
            set
            {
                _clients = value;
                RaisePropertyChanged("Clients");
            }
        }

        private WordClientInfo _client;
        public WordClientInfo Client
        {
            get
            {
                return _client;
            }
            set
            {
                _client = value;
                RaisePropertyChanged("Client");
            }
        }

        private ObservableCollection<WordGraphInfo> _graphs;
        public ObservableCollection<WordGraphInfo> Graphs
        {
            get
            {
                return _graphs;
            }
            set
            {
                _graphs = value;
                RaisePropertyChanged("Graphs");
            }
        }

        private WordGraphInfo _graph;
        public WordGraphInfo Graph
        {
            get
            {
                return _graph;
            }
            set
            {
                _graph = value;
                RaisePropertyChanged("Graph");
            }
        }

        public ICommand SelectCommand { get; private set; }

        private void SelectCommandExecute()
        {
            if (Client == null)
                return;
            if (Graph == null)
                return;

            var index = Graphs.IndexOf(Graph);

            //TcpServerCenter.Instance.SendData(Client.Ip, Client.Port, CommandConstant.Command_SelectDoc);
            //TcpServerCenter.Instance.SendData(Client.Ip, Client.Port, string.Format("{0}", index));


            CommandData data = new CommandData();
            data.Protocol = 0x01;//选择命令
            data.Command = index;//索引值
            data.Content = Graph.GraphInfo;
            string commandData = CommandHelper.SetProtocol(data);
            TcpServerCenter.Instance.SendData(Client.Ip, Client.Port, commandData);
        }

        public ICommand DeleteCommand { get; private set; }

        private void DeleteCommandExecute()
        {
            if (Client == null)
                return;
            if (Graph == null)
                return;

            var index = Graphs.IndexOf(Graph);

            CommandData data = new CommandData();
            data.Protocol = 0x03;//删除命令
            data.Command = index;//索引值
            data.Content = Graph.GraphInfo;
            string commandData = CommandHelper.SetProtocol(data);
            TcpServerCenter.Instance.SendData(Client.Ip, Client.Port, commandData);

            Graphs.RemoveAt(index);
        }

        private string _insertContent;
        public string InsertContent
        {
            get
            {
                return _insertContent;
            }
            set
            {
                _insertContent = value;
                RaisePropertyChanged("InsertContent");
            }
        }

        public ICommand InsertCommand { get; private set; }

        private void InsertCommandExecute()
        {
            if (Client == null)
                return;
            if (Graph == null)
                return;

            var index = Graphs.IndexOf(Graph);

            CommandData data = new CommandData();
            data.Protocol = 0x04;//新增命令
            data.Command = index;//索引值
            data.Content = string.Format("{0};{1}", Graph.GraphInfo, InsertContent);
            string commandData = CommandHelper.SetProtocol(data);

            TcpServerCenter.Instance.SendData(Client.Ip, Client.Port, commandData);

            Graphs.Insert(index, new WordGraphInfo()
            {
                GraphInfo = InsertContent
            });
        }

        private string _modifyContent;
        public string ModifyContent
        {
            get
            {
                return _modifyContent;
            }
            set
            {
                _modifyContent = value;
                RaisePropertyChanged("ModifyContent");
            }
        }

        public ICommand ModifyCommand { get; private set; }

        private void ModifyCommandExecute()
        {
            if (Client == null)
                return;
            if (Graph == null)
                return;

            var index = Graphs.IndexOf(Graph);

            CommandData data = new CommandData();
            data.Protocol = 0x02;//修改命令
            data.Command = index;//索引值
            data.Content = string.Format("{0};{1}", Graph.GraphInfo, ModifyContent);
            string commandData = CommandHelper.SetProtocol(data);
            TcpServerCenter.Instance.SendData(Client.Ip, Client.Port, commandData);

            Graph.GraphInfo = ModifyContent;
        }

        public MainViewModel()
        {
            SelectCommand = new DelegateCommand(SelectCommandExecute);
            DeleteCommand = new DelegateCommand(DeleteCommandExecute);
            InsertCommand = new DelegateCommand(InsertCommandExecute);
            ModifyCommand = new DelegateCommand(ModifyCommandExecute);


            Clients = new ObservableCollection<WordClientInfo>();

            TcpServerCenter.Instance.ClientUpdateEvent += new TcpUtils.DelConnectUpdate(Instance_ClientUpdateEvent);
            TcpServerCenter.Instance.ServerReceivedDataEvent += new TcpUtils.DelHandleReceiveData(Instance_ServerReceivedDataEvent);
        }

        public string CurrentCommand { get; set; }

        public int CurrentCommandIndex { get; set; }


        void Instance_ServerReceivedDataEvent(string ip, int port, string receiveDate, int protocol, int command)
        {
            //锁定操作的文档 
            var client = GetClient(ip, port);
            if (client == null)
                return;

            if (string.IsNullOrEmpty(receiveDate))
                return;

            if (Graphs != client.WordGraphs)
            {
                Graphs = client.WordGraphs;
            }


            #region add by zhangyou


            CommandData commandData = CommandHelper.GetProtocol(receiveDate, protocol, command);

            //if (CommandConstant.MonitorAvaiableCommandIndexs.Contains(commandData.Protocol))
            //{

            if (protocol == 0x06)
            {
                if (Graphs != null)
                {
                    StaInvoke(() =>
                    {
                        MessageBox.Show(receiveDate);
                    });
                }
            }


            if (protocol == 0x05)
            {
                if (Graphs != null)
                {
                    StaInvoke(() =>
                    {
                        Graphs.Clear();
                    });
                }
            }

            if (protocol == 0x00)
            {
                if (Graphs != null)
                {
                    StaInvoke(() =>
                    {
                        Graphs.Add(new WordGraphInfo()
                        {
                            GraphInfo = commandData.Content
                        });
                    });
                }
            }
            //}
            #endregion


            ////切换命令模式
            //if (CommandConstant.MonitorAvaiableCommands.Contains(receiveDate))
            //{
            //    CurrentCommand = receiveDate;

            //    //command header
            //    if (CurrentCommand == CommandConstant.Command_InitDoc)
            //    {
            //        //clear current
            //        if (Graphs != null)
            //        {
            //            StaInvoke(() =>
            //            {
            //                Graphs.Clear();
            //            });
            //        }

            //    }
            //}
            //else
            //{
            //    switch (CurrentCommand)
            //    {
            //        case CommandConstant.Command_InitDoc:
            //            StaInvoke(() =>
            //            {
            //                Graphs.Add(new WordGraphInfo()
            //                {
            //                    GraphInfo = receiveDate
            //                });
            //            });
            //            break;
            //    }
            //}


        }

        private void StaInvoke(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        private WordClientInfo GetClient(string ip, int port)
        {
            var firstClient = Clients.FirstOrDefault(client => string.Equals(client.Ip, ip) && port == client.Port);
            return firstClient;
        }

        void Instance_ClientUpdateEvent(string ip, int port, bool ifAdd)
        {
            if (ifAdd)
            {
                StaInvoke(() =>
                {
                    Clients.Add(new WordClientInfo()
                    {
                        Ip = ip,
                        Port = port
                    });
                    Client = Clients.Count > 0 ? Clients[0] : null;
                });
            }
            else
            {
                StaInvoke(() =>
                {
                    var firstClient = GetClient(ip, port);
                    if (firstClient != null)
                    {
                        Clients.Remove(firstClient);
                    }
                });

            }
        }
    }
}
