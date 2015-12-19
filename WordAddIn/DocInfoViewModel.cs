using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using System.Collections.ObjectModel;
using WordAddIn.Data;

namespace WordAddIn
{
    public class DocInfoViewModel : NotificationObject
    {

        private bool? _dialogResult;
        public bool? DialogResult
        {
            get
            {
                return _dialogResult;
            }
            set
            {
                _dialogResult = value;
                RaisePropertyChanged("DialogResult");
            }
        }

        private ObservableCollection<WordPara> _wordParas;
        public ObservableCollection<WordPara> WordParas
        {
            get
            {
                return _wordParas;
            }
            set
            {
                _wordParas = value;
                RaisePropertyChanged("WordParas");
            }
        }


        public DocInfoViewModel()
        {
            WordParas = new ObservableCollection<WordPara>(WordUtils.GetWordParas());
        }
    }
}
