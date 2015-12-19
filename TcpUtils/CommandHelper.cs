using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpUtils
{
    public class CommandHelper
    {
        #region 简单协议

        /// <summary>
        /// 发送处理【客户端】
        /// </summary>
        /// <param name="command">命令类型[枚举]</param>
        /// <param name="protocol">网络协议类型[枚举]</param>
        /// <param name="data">标题编号</param>
        /// <returns></returns>
        public static string SetProtocol(CommandData data)
        {

            // protocol            4字节
            // command             4字节
            // content       

            byte[] buffer = Encoding.Unicode.GetBytes(data.Content);//内容

            byte[] bytes = new byte[4 + 4 + buffer.Length];

            int index = 0;

            Array.Copy(BitConverter.GetBytes(data.Protocol), 0, bytes, index, 4);
            index += 4;

            Array.Copy(BitConverter.GetBytes(data.Command), 0, bytes, index, 4);
            index += 4;

            Array.Copy(buffer, 0, bytes, index, buffer.Length);

            return System.Text.Encoding.Unicode.GetString(bytes);
        }


        /// <summary>
        /// 接收处理【服务器】
        /// </summary>
        /// <param name="reciveData">接收消息</param>
        /// <returns></returns>
        public static CommandData GetProtocol(string reciveData,int protocol,int command)
        {
            CommandData data = new CommandData();

            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(reciveData);

            int index = 0;

            //int protocol = BitConverter.ToInt32(bytes, index);
            //index += 4;

            //int command = BitConverter.ToInt32(bytes, index);
            //index += 4;

            string content = Encoding.Unicode.GetString(bytes, index, bytes.Length - index).TrimEnd('\0');

            data.Protocol = protocol;
            data.Command = command;
            data.Content = content;

            return data;
        }


        #endregion
    }

    #region  协议集合
    public class CommandData
    {
        /// <summary>
        /// 命令类型
        /// </summary>
        public Int32 Protocol { get; set; }
        /// <summary>
        /// 命令编码
        /// </summary>
        public Int32 Command { get; set; }
        /// <summary>
        /// 传输内容
        /// </summary>
        public string Content { get; set; }
    }
    #endregion
}
