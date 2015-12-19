using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace TcpUtils
{
    public class TcpServerHelper
    {
        #region 委托
        /// <summary>
        /// 更新新的链接
        /// </summary>
        private event DelConnectUpdate UpdateConnect;

        /// <summary>
        /// 处理数据
        /// </summary>
        private event DelHandleReceiveData HandleData;

        #endregion

        #region  属性

        private bool _isStart = false;
        /// <summary>
        /// 监听线程
        /// </summary>
        private Thread _listenThread;
        /// <summary>
        /// TCP监听
        /// </summary>
        private TcpListener _listener;
        /// <summary>
        /// 客户端连接列表
        /// </summary>
        private Dictionary<string, ReceiveHandle> _clientLis = new Dictionary<string, ReceiveHandle>();
        /// <summary>
        /// 最大连接数量
        /// </summary>
        private readonly int _maxLink = 100000;
        /// <summary>
        /// 当前连接数量
        /// </summary>
        private int _currentLinked = 0;
        /// <summary>
        /// 线程同步
        /// </summary>
        private ManualResetEvent _tcpClientConnected = new ManualResetEvent(false);

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>初始化结果</returns>
        public void Init(string ip, int port, DelConnectUpdate updateConnect, DelHandleReceiveData handle)
        {
            UpdateConnect += updateConnect;
            HandleData += handle;
            Thread t = new Thread(new ParameterizedThreadStart(DoInit));
            AddressModel model = new AddressModel();
            model.IP = ip;
            model.Port = port;
            t.Start(model);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        public void SendData(string ip, int port, string content)
        {
            string key = ip + ":" + port;
            ReceiveHandle handle = _clientLis[key];
            handle.Writer.WriteLine(content);
            handle.Writer.Flush();
        }


        /// <summary>
        /// TCP监听进程初始化
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口号</param>
        private void DoInit(object o)
        {
            AddressModel model = o as AddressModel;
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(model.IP), model.Port);
            _listener = new TcpListener(serverEndPoint);
            _listener.ExclusiveAddressUse = false;
            _listener.Start();
            _isStart = true;
            _tcpClientConnected.Reset();
            IAsyncResult result = _listener.BeginAcceptTcpClient(new AsyncCallback(DoAccept), _listener);
            _tcpClientConnected.WaitOne();
        }

        /// <summary>
        /// 循环等待接收数据
        /// </summary>
        private void DoAccept(IAsyncResult o)
        {
            TcpListener server = o.AsyncState as TcpListener;
            TcpClient client = null;
            try
            {
                client = server.EndAcceptTcpClient(o);
                //开始接收数据
                NetworkStream networkStream = client.GetStream();//获取到连接之间的通道
                BinaryReader sr = new BinaryReader(networkStream);
                //new StreamReader(networkStream, Encoding.GetEncoding("gb2312"));//读取数据通道
                StreamWriter sw = new StreamWriter(networkStream, Encoding.Unicode);//写数据通道
                //启动适当的线程的接收数据
                ReceiveHandle handle = new ReceiveHandle();
                handle.Client = client;
                handle.Reader = sr;
                handle.Writer = sw;
                handle.BaseStream = networkStream;
                Thread listenThread = new Thread(new ParameterizedThreadStart(ReceiveData));
                listenThread.IsBackground = true;
                listenThread.Start(handle);
                handle.ListenThread = listenThread;
                AddNewConnect(client, handle);
            }
            catch
            {
                //TODO:LOG
            }
            finally
            {
                _tcpClientConnected.Reset();
                IAsyncResult result = server.BeginAcceptTcpClient(new AsyncCallback(DoAccept), server);
                _tcpClientConnected.WaitOne();
            }
            
            if (client != null)
            {
                Close(client);
            }
            
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <param name="client"></param>
        public void Close(TcpClient client)
        {
            if (client.Connected)
            {
                var remote = (IPEndPoint)client.Client.RemoteEndPoint;
                lock (_clientLis)
                {
                    string key = remote.Address + ":" + remote.Port;
                    if (!_clientLis.ContainsKey(key))
                    {
                        _clientLis.Remove(key);
                        System.Threading.Interlocked.Decrement(ref _currentLinked);
                        UpdateConnect(remote.Address.ToString(), remote.Port, false);
                    }
                }
                client.Client.Shutdown(SocketShutdown.Both);
            }
            client.Client.Close();
            client.Close();

            System.Threading.Interlocked.Decrement(ref _currentLinked);
        }

        /// <summary>
        /// 添加新的链接
        /// </summary>
        /// <param name="client"></param>
        /// <param name="handle"></param>
        private void AddNewConnect(TcpClient client, ReceiveHandle handle)
        {
            //TODO:需要把当前客户端的信息更新的数据中
            var remote = client.Client.RemoteEndPoint as IPEndPoint;
            lock (_clientLis)
            {
                string key = remote.Address + ":" + remote.Port;
                if (!_clientLis.ContainsKey(key))
                {
                    _clientLis.Add(key, handle);
                    System.Threading.Interlocked.Increment(ref _currentLinked);
                    UpdateConnect(remote.Address.ToString(), remote.Port, true);
                }
            }
        }
        /// <summary>
        /// 接收数据监听线程
        /// </summary>
        private void ReceiveData(object sReader)
        {
            var Handle = sReader as ReceiveHandle;
            var remote = Handle.Client.Client.RemoteEndPoint as IPEndPoint;
            try
            {
                while (Handle.Client.Connected)
                {
                    int i = Handle.BaseStream.Read(Handle.ReceiveBuff, 0, Handle.ReceiveBuff.Length);
                    if (i>=8)
                    {
                        byte[] destinationArray = new byte[i - 8];
                        int protocol = BitConverter.ToInt32(Handle.ReceiveBuff, 0);
                        int command = BitConverter.ToInt32(Handle.ReceiveBuff, 4);
                        Array.Copy(Handle.ReceiveBuff, 8, destinationArray, 0, i - 8);
                        Array.Clear(Handle.ReceiveBuff, 0, i);
                        string content = Encoding.Unicode.GetString(destinationArray);
                        if (HandleData != null)
                            HandleData(remote.Address.ToString(), remote.Port, content, protocol, command);
                    }
                }
                if (HandleData != null) HandleData(remote.Address.ToString(), remote.Port, string.Empty,0xFF,0);
            }
            catch (Exception)
            {
                //数据接收发现异常  关闭连接
                if (HandleData != null) HandleData(remote.Address.ToString(), remote.Port, string.Empty,0xFF,0);
            }
        }
    }

    public class ReceiveHandle
    {
        private static readonly int BuffSize = 1024;

        public TcpClient Client { get; set; }

        public BinaryReader Reader { get; set; }

        public StreamWriter Writer { get; set; }

        public NetworkStream BaseStream { get; set; }

        public Thread ListenThread { get; set; }

        public byte[] ReceiveBuff = new byte[BuffSize];

        public string BuffString = string.Empty;

    }

    public class AddressModel
    {
        public string IP { get; set; }

        public int Port { get; set; }
    }

}
