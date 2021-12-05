using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HandyControl.Themes;


namespace TurboAzWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<AdUC> AdUCs { get; set; }
        public DataClasses1DataContext dtx { get; set; } = new DataClasses1DataContext();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            var result = from c in dtx.Cars select c;
            var resultList = result.ToList();
            AdUCs = new ObservableCollection<AdUC>();
            for (var index = 0; index < resultList.Count; index++)
            {
                var car = resultList[index];
                var temp = new AdUC(car);
                temp.Width = 229;
                temp.Height = 246;
                AdUCs.Add(temp);
            }
        }

        private void scroll_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("yaay");
        }
    }
}
