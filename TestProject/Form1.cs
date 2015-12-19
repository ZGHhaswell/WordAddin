using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TcpUtils;

namespace TestProject
{
    public delegate void ShowLog(string log);
    public partial class Form1 : Form
    {
        private TcpClientHelper client;
        public Form1()
        {
            InitializeComponent();
            client = new TcpClientHelper("127.0.0.1", 61181, HandleReceiveData);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.ConnectToServer("127.0.0.1", 6118);
            client.SendDataToServer("你好，我已经链接上服务器了。下面发送带有协议的内容。");
        }

        private void HandleReceiveData(string ip, int port, string receiveDate,int protocol,int command)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelHandleReceiveData(HandleReceiveData), ip, port, receiveDate, protocol, command);
            }
            else
            {
                if (dd.Checked)
                {
                    this.rch_ReceiveContent.AppendText(string.Format("已经接收来自{0}:{1}的数据：\t 协议：{2}\t命令：{3}\t内容：{4}" + Environment.NewLine, ip, port, IPAddress.NetworkToHostOrder(protocol), IPAddress.NetworkToHostOrder(command), receiveDate));
                }
                else
                {
                    this.rch_ReceiveContent.AppendText(string.Format("已经接收来自{0}:{1}的数据：\t 协议：{2}\t命令：{3}\t内容：{4}" + Environment.NewLine, ip, port, protocol, command, receiveDate));
                }                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CommandData data = new CommandData();
            data.Protocol = int.Parse(this.txt_p.Text.Trim());
            data.Command = int.Parse(this.txt_c.Text.Trim());
            if(dd.Checked)
            {
                data.Protocol = IPAddress.NetworkToHostOrder(data.Protocol);
                data.Command = IPAddress.NetworkToHostOrder(data.Command);
            }            
            data.Content = this.rch_SendContent.Text;
            client.SendDataToServer(CommandHelper.SetProtocol(data));
            this.rch_SendContent.Clear();
        }
    }
}
