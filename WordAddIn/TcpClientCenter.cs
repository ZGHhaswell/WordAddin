//TcpClientCenter

using System;
using TcpUtils;
using System.Collections;
using Microsoft.Office.Interop.Word;
using System.Linq;
using WordAddIn.Data;

namespace WordAddIn
{
    public class TcpClientCenter
    {
        private static TcpClientCenter _instance;
        public static TcpClientCenter Instance
        {
            get
            {
                return _instance ?? (_instance = new TcpClientCenter());
            }
        }

        private TcpClientCenter()
        {

        }



        private TcpClientHelper _client;

        public void DoInit(string ip, int port)
        {
            if (_client != null)
            {
                _client.Close();
            }
            _client = new TcpClientHelper(ip, port, DoReceiveData);
        }

        public string CurrentCommand { get; set; }

        public void DoReceiveData(string ip, int port, string receiveData, int protocol,int command)
        {
            try
            {
                if (string.IsNullOrEmpty(receiveData))
                    return;

                #region 新代码 add by zhangyou
                CommandData commandData = CommandHelper.GetProtocol(receiveData, protocol, command);
                //命令转化
                switch (protocol)
                {
                    case 0x01:
                        CurrentCommand = CommandConstant.Command_SelectDoc;
                        break;
                    case 0x02:
                        CurrentCommand = CommandConstant.Command_ModifyDoc;
                        break;
                    case 0x03:
                        CurrentCommand = CommandConstant.Command_DeleteDoc;
                        break;
                    case 0x04:
                        CurrentCommand = CommandConstant.Command_InsertDoc;
                        break;
                    case 0x05:
                        CurrentCommand = CommandConstant.Command_InsertTable;
                        break;
                }



                //解析命令
                switch (CurrentCommand)
                {
                    case CommandConstant.Command_SelectDoc:
                        {
                            //var ranges = ThisAddIn.Ranges;

                            var keyPair = ThisAddIn.GetWordTitle(commandData.Content);

                            if (keyPair.Value != null)
                                keyPair.Value.Paragraph.Range.Select();
                            break;
                        }

                    case CommandConstant.Command_DeleteDoc:
                        {
                            var keyPair = ThisAddIn.GetWordTitle(commandData.Content);

                            if (keyPair.Value != null)
                            {
                                object start = keyPair.Value.Paragraph.Range.Start;
                                object end = null;
                                var lastParagraphFlow = keyPair.Value.ParagraphFlow.LastOrDefault();
                                if (lastParagraphFlow != null)
                                    end = lastParagraphFlow.Range.End;
                                if (end == null)
                                    end = keyPair.Value.Paragraph.Range.End;

                                var range = Globals.ThisAddIn.Application.ActiveDocument.Range(ref start, ref end);

                                if (range != null)
                                {
                                    range.Delete();
                                    ThisAddIn.UpdateRanges(keyPair.Key);
                                }
                                
                            }
                            break;
                        }

                    case CommandConstant.Command_ModifyDoc:
                        {
                            //var ranges2 = ThisAddIn.Ranges;
                            //var index2 = command;
                            //var content = commandData.Content;
                            //if (ranges2.ContainsKey(index2))
                            //{
                            //    ranges2[index2].Paragraph.Range.Select();
                            //    Globals.ThisAddIn.Application.Selection.TypeText(content);
                            //}
                            var spiltedStr = commandData.Content.Split(new char[] { ';' });
                            if (spiltedStr.Length == 2)
                            {
                                var oldStr = spiltedStr[0];
                                var newStr = spiltedStr[1];

                                var keyPair = ThisAddIn.GetWordTitle(oldStr);

                                if (keyPair.Value != null)
                                {
                                    keyPair.Value.Paragraph.Range.Select();
                                    Globals.ThisAddIn.Application.Selection.TypeText(newStr);
                                }
                            }
                            break;
                        }

                    case CommandConstant.Command_InsertDoc:
                        {
                            //var ranges3 = ThisAddIn.Ranges;

                            //var index3 = command;
                            //var content = commandData.Content;
                            //if (ranges3.ContainsKey(index3))
                            //{
                            //    ranges3[index3].Paragraph.Range.InsertBefore(content + Environment.NewLine);
                            //    var graph = ranges3[index3].Paragraph.Previous();
                            //    ThisAddIn.UpdateRanges(index3, graph);
                            //}

                            var spiltedStr = commandData.Content.Split(new char[] { ';' });
                            if (spiltedStr.Length == 2)
                            {
                                var oldStr = spiltedStr[0];
                                var newStr = spiltedStr[1];

                                var keyPair = ThisAddIn.GetWordTitle(oldStr);

                                var currentWordTitle = keyPair.Value;

                                if (currentWordTitle != null)
                                {
                                    var nextWordTitle = ThisAddIn.GetNextWordTitle(keyPair);

                                    if (nextWordTitle.Value != keyPair.Value)
                                    {
                                        //存在相邻段落
                                        var nextWordIndex = nextWordTitle.Key;
                                        var nextWord = nextWordTitle.Value;

                                        nextWord.Paragraph.Range.InsertParagraphBefore();
                                        var previousParagraph = nextWord.Paragraph.Previous();

                                        if (previousParagraph != null)
                                        {
                                            previousParagraph.Range.Select();
                                            Globals.ThisAddIn.Application.Selection.TypeText(newStr);

                                            ThisAddIn.UpdateRangesDecrease(nextWordIndex, previousParagraph);
                                        }
                                        
                                    }
                                    else
                                    {
                                        //不存在相邻段落
                                        var currentWordIndex = nextWordTitle.Key;
                                        var currentWord = nextWordTitle.Value;

                                        //当前段落拷贝格式
                                        currentWord.Paragraph.Range.Select();
                                        currentWord.Paragraph.Range.Copy();

                                        //获得最下面的段落
                                        var lastGraph = currentWord.ParagraphFlow.LastOrDefault();
                                        lastGraph = lastGraph == null ? currentWord.Paragraph : lastGraph;

                                        //插入段落
                                        lastGraph.Range.InsertParagraphAfter();

                                        //获得下一个段落
                                        var paraGraph = lastGraph.Next();

                                        if (paraGraph != null)
                                        {
                                            //先选中
                                            paraGraph.Range.Select();
                                            //黏贴
                                            paraGraph.Range.Paste();
                                            //选中 
                                            paraGraph.Previous().Range.Select();
                                            //全局输入内容
                                            Globals.ThisAddIn.Application.Selection.TypeText(newStr);

                                            ThisAddIn.UpdateRangesIncrease(keyPair.Key, paraGraph);
                                        }
                                    }

                                }
                            }
                            break;
                        }
                    case CommandConstant.Command_InsertTable://新增一个空白表格
                        {
                            var ranges3 = ThisAddIn.Ranges;
                            var splitedStr = receiveData.Split(new char[] { ';' });                            
                            if (splitedStr.Length == 2)
                            {
                                var keyPair = ThisAddIn.GetWordTitle(splitedStr[0]);
                                var currentWordTitle = keyPair.Value;
                                var currentParaGraph = currentWordTitle.Paragraph;
                                //插入段落
                                currentParaGraph.Range.InsertParagraphAfter();
                                //获得下一个段落
                                var paraGraph = currentParaGraph.Next();
                                if (paraGraph != null)
                                {
                                    //先选中
                                    paraGraph.Range.Select();
                                    //新增加表格
                                    Globals.ThisAddIn.Application.ActiveDocument.Tables.Add(paraGraph.Range, 5, 5);
                                    //选中 
                                    paraGraph.Range.Select();
                                    ThisAddIn.UpdateRangesIncrease(keyPair.Key, paraGraph);
                                }
                            }
                            break;
                        }
                }


                #endregion

                #region 原代码
                //if (CommandConstant.AddinAvaiableCommands.Contains(receiveData))
                //{
                //    //切换命令
                //    CurrentCommand = receiveData;
                //}
                //else
                //{
                //    //解析命令
                //    switch (CurrentCommand)
                //    {
                //        case CommandConstant.Command_SelectDoc:
                //            {
                //                var ranges = ThisAddIn.Ranges;
                //                var index = Convert.ToInt32(receiveData);
                //                if (ranges.ContainsKey(index))
                //                    ranges[index].Paragraph.Range.Select();
                //                break;
                //            }

                //        case CommandConstant.Command_DeleteDoc:
                //            {
                //                var ranges1 = ThisAddIn.Ranges;
                //                var index1 = Convert.ToInt32(receiveData);
                //                if (ranges1.ContainsKey(index1))
                //                {
                //                    var wordTitle = ranges1[index1];

                //                    wordTitle.Paragraph.Range.Delete();

                //                    foreach (Paragraph paragraph in wordTitle.ParagraphFlow)
                //                    {
                //                        paragraph.Range.Delete();
                //                    }
                //                }

                //                ThisAddIn.UpdateRanges(index1);

                //                break;
                //            }

                //        case CommandConstant.Command_ModifyDoc:
                //            {
                //                var ranges2 = ThisAddIn.Ranges;
                //                var splitedStr = receiveData.Split(new char[] { ',' });
                //                if (splitedStr.Length == 2)
                //                {
                //                    var index2 = Convert.ToInt32(splitedStr[0]);
                //                    var content = splitedStr[1];
                //                    if (ranges2.ContainsKey(index2))
                //                    {
                //                        ranges2[index2].Paragraph.Range.Select();
                //                        Globals.ThisAddIn.Application.Selection.TypeText(content);
                //                    }
                //                }
                //                break;
                //            }

                //        case CommandConstant.Command_InsertDoc:
                //            {
                //                var ranges3 = ThisAddIn.Ranges;
                //                var splitedStr = receiveData.Split(new char []{','});
                //                if (splitedStr.Length == 2)
                //                {
                //                    var index3 = Convert.ToInt32(splitedStr[0]);
                //                    var content = splitedStr[1];
                //                    if (ranges3.ContainsKey(index3))
                //                    {
                //                        ranges3[index3].Paragraph.Range.InsertBefore(content + Environment.NewLine);
                //                        var graph = ranges3[index3].Paragraph.Previous();
                //                        ThisAddIn.UpdateRanges(index3, graph);
                //                    }
                //                }

                //                break;
                //            }

                //    }
                //}
                #endregion
            }
            catch
            {
            }
            
        }

        public void Connect(string ip, int port)
        {
            _client.ConnectToServer(ip, port);
        }

        public void SendData(string data)
        {
            _client.SendDataToServer(data);
        }

        public void Close()
        {

        }
    }
}
