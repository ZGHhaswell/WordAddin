using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpUtils
{
    /// <summary>
    /// 客户端TCP连接状态更新
    /// </summary>
    /// <param name="ip">IP地址</param>
    /// <param name="port">端口号</param>
    /// <param name="ifAdd">是否是新增（true-新增/false-删除）</param>
    public delegate void DelConnectUpdate(string ip, int port, bool ifAdd);

    /// <summary>
    /// 处理接收到的数据的委托
    /// </summary>
    public delegate void DelHandleReceiveData(string ip, int port, string receiveDate, int protocol, int command);
}
