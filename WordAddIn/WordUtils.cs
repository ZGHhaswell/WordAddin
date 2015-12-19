using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;
using System.Diagnostics;
using WordAddIn.Data;

namespace WordAddIn
{
    public static class WordUtils
    {
        public static Dictionary<int, WordTitle> GetDocRanges()
        {
            var ranges = new Dictionary<int, WordTitle>();
            int index = 0;
            var documents = Globals.ThisAddIn.Application.Documents;

            WordTitle current = null;
            foreach (Microsoft.Office.Interop.Word.Document document in documents)
            {
                var paragraphs = document.Paragraphs;

                foreach (Paragraph item in paragraphs)
                {
                    var range = item.Range;
                    var text = range.GetText();

                    var font = range.Font;
                    var format = range.ParagraphFormat;

                    var outLine = format.OutlineLevel;

                    //font.Size == 14 && font.Name == "黑体" && format.Alignment == WdParagraphAlignment.wdAlignParagraphLeft && 
                    var isTitle = (int)outLine <10;

                    if (isTitle)
                    {
                        //current is Title and text is not empty
                        if (!string.IsNullOrEmpty(text))
                        {
                            //create a wordTitle 
                            var wordTitle = new WordTitle()
                            {
                                Level = (int)outLine,
                                Paragraph = item
                            };

                            current = wordTitle;

                            if (!ranges.ContainsKey(index))
                            {
                                ranges.Add(index, current);
                                index++;
                            }
                        }
                    }
                    else
                    {
                        if (current != null)
                        {
                            current.ParagraphFlow.Add(item);
                        }
                    }

                    

                    
                }
            }
            return ranges;
        }

        public static string GetWordDocPath()
        {
            return Globals.ThisAddIn.Application.ActiveDocument.FullName;
        }


        public static IList<WordPara> GetWordParas()
        {
            var list = new List<WordPara>();

            var paras = new Dictionary<int, WordPara>();

            var ranges = GetDocRanges();

            foreach (KeyValuePair<int, WordTitle> indexToWordTitle in ranges)
            {
                var wordPara = new WordPara()
                {
                    Content = indexToWordTitle.Value.Paragraph.Range.GetText(),
                    Level = indexToWordTitle.Value.Paragraph.Format.OutlineLevel,
                    Paragraph = indexToWordTitle.Value.Paragraph,
                    ParagraphFlow = indexToWordTitle.Value.ParagraphFlow,
                };
                paras.Add(indexToWordTitle.Key, wordPara);
            }

            int max = paras.Keys.Max();

            for (int i = max; i >= 1; i--)
            {
                var wordPara = paras[i];
                if (wordPara.Level == WdOutlineLevel.wdOutlineLevel2 || wordPara.Level == WdOutlineLevel.wdOutlineLevel1)
                    continue;

                var parentLevel = wordPara.Level - 1;
                WordPara parentPara = null;

                for (int j = i - 1; j >= 1; j--)
                {
                    var previousPara = paras[j];
                    if (previousPara.Level == parentLevel)
                    {
                        parentPara = previousPara;
                        break;
                    }
                }

                if (parentPara != null)
                {
                    parentPara.Children.Add(wordPara);
                }
            }

            list.AddRange(paras.Values.Where(para => para.Level == WdOutlineLevel.wdOutlineLevel2));

            return list;
        }

        public static string GetText(this Range range)
        {
            string text = range.Text;

            text = text.Replace("\r", "");
            text = text.Replace("\a", "");
            text = text.Replace("\f", "");
            text = text.Replace("\n", "");
            return text;
        }
    }

   
}
