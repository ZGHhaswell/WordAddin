using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace WordAddIn.Data
{
    public class WordTitle
    {
        public Paragraph Paragraph { get; set; }


        public int Level { get; set; }

        public IList<Paragraph> ParagraphFlow { get; set; }

        public WordTitle()
        {
            ParagraphFlow = new List<Paragraph>();
        }
    }
}
