//TcpServerCenter

using TcpUtils;

namespace WordMonitorApp
{
    public class TcpServerCenter
    {
        private static TcpServerCenter _instance;
        public static TcpServerCenter Instance
        {
            get
            {
                return _instance ?? (_instance = new TcpServerCenter());
            }
        }

        private TcpServerCenter()
        {

        }





        private TcpServerHelper _server;

        public void DoInit(string ip, int port)
        {
            _server = new TcpServerHelper();
            _server.Init(ip, port, DoConnectUpdate, DoReceiveData);
        }


        public void DoConnectUpdate(string ip, int port, bool ifAdd)
        {
            //client connected/disconnected
            if (ClientUpdateEvent != null)
            {
                ClientUpdateEvent(ip, port, ifAdd);
            }
        }

        public event DelConnectUpdate ClientUpdateEvent;

        public void DoReceiveData(string ip, int port, string receiveData, int protocol, int command)
        {
            //receoive
            if (ServerReceivedDataEvent != null)
            {
                ServerReceivedDataEvent(ip, port, receiveData, protocol, command);
            }
        }

        public event DelHandleReceiveData ServerReceivedDataEvent;

        public void SendData(string ip, int port, string data)
        {
            _server.SendData(ip, port, data);
        }

        public void Close()
        {

        }


    }
}
