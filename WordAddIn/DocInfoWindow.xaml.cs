using System.Windows;
using System.Windows.Controls;


namespace WordAddIn
{
    /// <summary>
    /// DocInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DocInfoWindow : Window
    {
        public DocInfoWindow()
        {
            InitializeComponent();


            DataContext = new DocInfoViewModel();
        }

        private void TreeView_Loaded(object sender, RoutedEventArgs e)
        {
            //var treeView = sender as TreeView;
            //foreach (var item in treeView.Items)
            //{
                
            //}
        }
    }
}
