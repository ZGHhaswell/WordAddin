using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpUtils
{
    public static class CommandConstant
    {
        public static readonly IList<string> AddinAvaiableCommands = new List<string>()
        {
            "SelectDoc",//选中文档
            "DeleteDoc",//删除文档
            "InsertDoc",//插入文档
            "ModifyDoc", //修改文档
            "InsertTable"//增加表格
        };

        public const string Command_SelectDoc = "SelectDoc";

        public const string Command_DeleteDoc = "DeleteDoc";

        public const string Command_InsertDoc = "InsertDoc";

        public const string Command_ModifyDoc = "ModifyDoc";

        public const string Command_InsertTable = "InsertTable";

        public static readonly IList<string> MonitorAvaiableCommands = new List<string>()
        {
            "InitDoc", //初始化文档
        };

        public const string Command_InitDoc = "InitDoc";

        /// <summary>
        /// 从 Addin 传到 App
        /// </summary>
        public static readonly IList<int> MonitorAvaiableCommandIndexs = new List<int>()
        {
            Command_InitDocIndex, //初始化文档
            Command_InitContentIndex,
        };

        public const int Command_InitDocIndex = 0x00ff;

        public const int Command_InitContentIndex = 0x0000;
        

    }
}
