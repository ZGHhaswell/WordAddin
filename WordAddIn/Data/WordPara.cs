using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Office.Interop.Word;
using System.Collections.ObjectModel;

namespace WordAddIn.Data
{
    public class WordPara :NotificationObject
    {
        public string Content { get; set; }

        public WdOutlineLevel Level { get; set; }

        public Paragraph Paragraph { get; set; }

        public IList<Paragraph> ParagraphFlow { get; set; }

        public ObservableCollection<WordPara> Children { get; set; }


        public WordPara()
        {
            Children = new ObservableCollection<WordPara>();
        }
    }
}
