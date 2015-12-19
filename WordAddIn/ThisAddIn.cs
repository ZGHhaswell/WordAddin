using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using System.Threading;
using System.IO;
using Microsoft.Office.Interop.Word;
using TcpUtils;
using System.Diagnostics;
using WordAddIn.Data;

namespace WordAddIn
{
    public partial class ThisAddIn
    {
        public static Dictionary<int, WordTitle> Ranges { get; set; }

        public static void UpdateRanges()
        {
            Ranges = WordUtils.GetDocRanges();
        }

        public static void UpdateRangesIncrease(int index, Paragraph paragraph)
        {
            if (Ranges == null)
                return;

            var cloneRanges = new Dictionary<int, WordTitle>();
            foreach (var key in Ranges.Keys)
            {
                if (key <= index)
                {
                    cloneRanges.Add(key, Ranges[key]);
                }
                else
                {
                    cloneRanges.Add(key + 1, Ranges[key]);
                }
            }
            cloneRanges.Add(index + 1, new WordTitle()
                {
                    Paragraph = paragraph
                });

            Ranges = cloneRanges;
        }

        public static void UpdateRangesDecrease(int index, Paragraph paragraph)
        {
            if (Ranges == null)
                return;

            var cloneRanges = new Dictionary<int, WordTitle>();
            foreach (var key in Ranges.Keys)
            {
                if (key < index)
                {
                    cloneRanges.Add(key, Ranges[key]);
                }
                else
                {
                    cloneRanges.Add(key + 1, Ranges[key]);
                }
            }
            cloneRanges.Add(index, new WordTitle()
            {
                Paragraph = paragraph
            });

            Ranges = cloneRanges;
        }

        public static void UpdateRanges(int index)
        {
            if (Ranges == null)
                return;

            var cloneRanges = new Dictionary<int, WordTitle>();
            foreach (var key in Ranges.Keys)
            {
                if (key < index)
                {
                    cloneRanges.Add(key, Ranges[key]);
                }
                else if (key > index)
                {
                    cloneRanges.Add(key - 1, Ranges[key]);
                }
            }
            Ranges = cloneRanges;
        }


        public static KeyValuePair<int, WordTitle> GetWordTitle(string content)
        {
            foreach (KeyValuePair<int, WordTitle> keyPair in Ranges)
            {
                var text = keyPair.Value.Paragraph.Range.GetText();
                if (text.Contains(content))
                    return keyPair;
            }

            return default(KeyValuePair<int, WordTitle>);
        }

        public static KeyValuePair<int, WordTitle> GetNextWordTitle(KeyValuePair<int, WordTitle> current)
        {
            var currentIndex = current.Key;
            var level = current.Value.Paragraph.Format.OutlineLevel;
            var max = Ranges.Keys.Max();


            int stepIndex = currentIndex + 1;

            bool isFound = false;
            int nextIndex= 0;

            for (int i = stepIndex; i <= max; i++)
            {
                if (Ranges.ContainsKey(i))
                {
                    var indexWordTitle = Ranges[i];
                    var outLineLevel = indexWordTitle.Paragraph.Format.OutlineLevel;

                    if (outLineLevel < level)
                        break;

                    if (outLineLevel == level)
                    {
                        isFound = true;
                        nextIndex = i;
                        break;
                    }
                }
            }

            if (isFound)
                return new KeyValuePair<int, WordTitle>(nextIndex, Ranges[nextIndex]);
            else
            {
                return current;
            }

        }

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            while (true)
            {
                if (!string.IsNullOrEmpty(GetCurrentDocName()))
                {
                    break;
                }
                Thread.Sleep(1000);
            }

            System.Action doConnect = () =>
                {
                    
                    while (true)
                    {
                        bool isSuccess = false;
                        try
                        {
                            TcpClientCenter.Instance.DoInit("127.0.0.1", 3001);
                            TcpClientCenter.Instance.Connect("127.0.0.1", 6118);
                            isSuccess = true;
                        }
                        catch
                        {
                            isSuccess = false;
                        }
                        if (isSuccess)
                        {
                            
                            break;
                        }
                        else
                        {
                            Thread.Sleep(2000);
                        }
                    }

                    Update();
                    //if (!isSuccess)
                    //{
                    //    ConnectSettingWindow connectWindow = null;
                    //    bool? dialogResult = null;

                    //    connectWindow = new ConnectSettingWindow();
                    //    dialogResult = connectWindow.ShowDialog();

                    //    if (dialogResult != null && dialogResult.Value)
                    //    {
                    //        Update();

                    //    }
                    //}
                };

            doConnect.BeginInvoke(null, null);

            //display doc info
            //var docInfoWindow = new DocInfoWindow();
            //docInfoWindow.ShowDialog();

            Application.DocumentBeforeSave += new ApplicationEvents4_DocumentBeforeSaveEventHandler(Application_DocumentBeforeSave);

        }

        private void Update()
        {
            #region add by zhangyou

            try
            {
                //send doc path
                CommandData pathCommandData = new CommandData();
                pathCommandData.Protocol = 0x06;
                pathCommandData.Command = 0;
                pathCommandData.Content = WordUtils.GetWordDocPath();
                TcpClientCenter.Instance.SendData(CommandHelper.SetProtocol(pathCommandData));

                //ctrl + s  send word content 
                Ranges = WordUtils.GetDocRanges();

                StringBuilder sb = new StringBuilder();
                foreach (var range in Ranges.Values)
                {
                    sb.Append(string.Format("{0}*{1};", range.Paragraph.Range.GetText(), range.Level));
                }

                CommandData commandData = new CommandData();
                commandData.Protocol = 0x07;
                commandData.Command = 0;
                commandData.Content = sb.ToString();
                TcpClientCenter.Instance.SendData(CommandHelper.SetProtocol(commandData));
            }
            catch
            {
            }
            //初始化标记 
            
            #endregion

            //TcpClientCenter.Instance.SendData(CommandConstant.Command_InitDoc);
            //Ranges = WordUtils.GetDocRanges();
            //foreach (var range in Ranges.Values)
            //{
            //    TcpClientCenter.Instance.SendData(range.Paragraph.Range.GetText());
            //}
        }

        void Application_DocumentBeforeSave(Word.Document Doc, ref bool SaveAsUI, ref bool Cancel)
        {
            Update();
        }

        private string GetCurrentDocName()
        {

            try
            {
                var path = Application.ActiveDocument.FullName;
                var fileInfo = new FileInfo(path);
                return fileInfo.DirectoryName;
            }
            catch
            {
                return string.Empty;
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            TcpClientCenter.Instance.Close();
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
