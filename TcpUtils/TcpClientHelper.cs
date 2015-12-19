using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace TcpUtils
{
    public class TcpClientHelper
    {
        /// <summary>
        /// 处理数据
        /// </summary>
        private event DelHandleReceiveData HandleData;

        #region  属性
        private TcpClient Client;
        /// <summary>
        /// 监听线程
        /// </summary>
        private Thread _receiveDataThread;
        /// <summary>
        /// 数据处理句柄
        /// </summary>
        private ReceiveHandle handle;


        #endregion

        /// <summary>
        /// TCP 通讯 客户端助手
        /// </summary>
        /// <param name="ip">本地IP地址</param>
        /// <param name="port">本地端口号</param>
        /// <param name="handle">数据处理接口</param>
        public TcpClientHelper(string ip, int port, DelHandleReceiveData handle)
        {
            HandleData += handle;
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Client = new TcpClient(localEndPoint);
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="ip">服务器Ip地址</param>
        /// <param name="port">服务器端口号</param>
        public void ConnectToServer(string ip, int port)
        {
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Client.Connect(serverEndPoint);
            NetworkStream networkStream = Client.GetStream();
            StreamWriter streamWriter = new StreamWriter(networkStream, Encoding.GetEncoding("gb2312"));
            BinaryReader streamReader = new BinaryReader(networkStream);
            //new MemoryStream(networkStream, Encoding.GetEncoding("gb2312"));
            handle = new ReceiveHandle();
            handle.Client = Client;
            handle.Reader = streamReader;
            handle.Writer = streamWriter;
            handle.BaseStream = networkStream;
            Thread listenThread = new Thread(new ParameterizedThreadStart(ReceiveData));
            listenThread.IsBackground = true;
            listenThread.Start(handle);
            handle.ListenThread = listenThread;
        }

        public void SendDataToServer(string content)
        {
            handle.Writer.WriteLine(content);
            handle.Writer.Flush();
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
                    if (i >= 8)
                    {
                        byte[] destinationArray = new byte[i - 8];
                        int protocol = BitConverter.ToInt32(Handle.ReceiveBuff, 0);
                        int command = BitConverter.ToInt32(Handle.ReceiveBuff, 4);
                        Array.Copy(Handle.ReceiveBuff, 8, destinationArray, 0, i - 8);
                        Array.Clear(Handle.ReceiveBuff, 0, i);
                        string content = Encoding.GetEncoding("gb2312").GetString(destinationArray);
                        if (HandleData != null)
                            HandleData(remote.Address.ToString(), remote.Port, content, protocol, command);
                    }
                }
                if (HandleData != null) HandleData(remote.Address.ToString(), remote.Port, string.Empty, 0xFF, 0);
            }
            catch (Exception)
            {
                //数据接收发现异常  关闭连接
                if (HandleData != null) HandleData(remote.Address.ToString(), remote.Port, string.Empty, 0xFF, 0);
            }
        }

        public void Close()
        {
            if (handle != null)
            {
                if (handle.ListenThread != null)
                    handle.ListenThread.Abort();
            }
            
            Client.Close();
        }

    }
}
