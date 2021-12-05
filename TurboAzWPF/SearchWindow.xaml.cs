using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TurboAzWPF.Annotations;

namespace TurboAzWPF
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DataClasses1DataContext dtx { get; set; } = new DataClasses1DataContext();

        public ObservableCollection<string> Makes { get; set; }
        public ObservableCollection<string> Models { get; set; }
        public ObservableCollection<string> Bans { get; set; }
        public ObservableCollection<string> Colors { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public ObservableCollection<string> FuelTypes { get; set; }
        public ObservableCollection<string> Drivetrains { get; set; }
        public ObservableCollection<string> Transmissions { get; set; }
        public ObservableCollection<string> Engines { get; set; }


        private string _makeSelected;

        public string MakeSelected
        {
            get { return _makeSelected; }
            set { _makeSelected = value;
                OnPropertyChanged();
            }
        }



        public SearchWindow()
        {
            InitializeComponent();

            DataContext = this;
            dtx = new DataClasses1DataContext();

            Makes = new ObservableCollection<string>();
            Models = new ObservableCollection<string>();
            Bans = new ObservableCollection<string>();
            Colors = new ObservableCollection<string>();
            Cities = new ObservableCollection<string>();
            FuelTypes = new ObservableCollection<string>();
            Drivetrains = new ObservableCollection<string>();
            Transmissions = new ObservableCollection<string>();
            Engines = new ObservableCollection<string>();

            // Adding Engines
            var engines = dtx.Cars.Select(cars => new { cars.Engine }).Distinct();
            foreach (var eng in engines)
            {
                Engines.Add(eng.Engine.ToString());
            }

            // Adding Makes
            var makes = dtx.Makes.Select(make => new { make.Name });
            foreach (var make in makes)
            {
                Makes.Add(make.Name);
            }

            // Adding Models
            var models = dtx.Models.Select(model => new { model.Name });
            foreach (var model in models)
            {
                Models.Add(model.Name);
            }

            // Adding Bans
            var bans = dtx.BanTypes.Select(ban => new { ban.Name });
            foreach (var ban in bans)
            {
                Bans.Add(ban.Name);
            }

            // Adding Colors
            var colors = dtx.Colors.Select(color => new { color.Name });
            foreach (var color in colors)
            {
                Colors.Add(color.Name);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Models.Clear();
            var models = dtx.Models.Where(model => model.Make.Name == MakeSelected)
                .Select(model => new { model.Name });

            foreach (var model in models)
            {
                Models.Add(model.Name);
            }
        }

    }
}