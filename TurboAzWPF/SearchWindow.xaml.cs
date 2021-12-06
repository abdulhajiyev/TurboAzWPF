using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using TurboAzWPF.Annotations;
using TurboAzWPF.DataAccess.Context;

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
        public ObservableCollection<string> PriceTypes { get; set; }


        private string _makeSelected;
        public string MakeSelected
        {
            get => _makeSelected;
            set { _makeSelected = value; OnPropertyChanged(); }
        }

        private string _modelSelected;
        public string ModelSelected
        {
            get => _modelSelected;
            set { _modelSelected = value; OnPropertyChanged(); }
        }

        private string _banSelected;
        public string BanSelected
        {
            get => _banSelected;
            set { _banSelected = value; OnPropertyChanged(); }
        }

        private string _colorSelected;
        public string ColorSelected
        {
            get => _colorSelected;
            set { _colorSelected = value; OnPropertyChanged(); }
        }

        private string _symbolSelected;
        public string SymbolSelected
        {
            get { return _symbolSelected; }
            set { _symbolSelected = value; OnPropertyChanged(); }
        }

        private string _citySelected;
        public string CitySelected
        {
            get { return _citySelected; }
            set { _citySelected = value; OnPropertyChanged(); }
        }

        private string _fuelSelected;
        public string FuelSelected
        {
            get { return _fuelSelected; }
            set { _fuelSelected = value; }
        }

        private string _drivetrainSelected;
        public string DrivetrainSelected
        {
            get { return _drivetrainSelected; }
            set { _drivetrainSelected = value; OnPropertyChanged(); }
        }

        private string _transmissionSelected;
        public string TransmissionSelected
        {
            get { return _transmissionSelected; }
            set { _transmissionSelected = value; OnPropertyChanged(); }
        }

        private decimal _engMin;
        public decimal EngMin
        {
            get { return _engMin; }
            set { _engMin = value; OnPropertyChanged(); }
        }

        private decimal _engMax;
        public decimal EngMax
        {
            get { return _engMax; }
            set { _engMax = value; OnPropertyChanged(); }
        }







        public SearchWindow()
        {
            InitializeComponent();

            DataContext = this;
            dtx = new DataClasses1DataContext();

            #region ObservableCollection Instances
            Makes = new ObservableCollection<string>();
            Models = new ObservableCollection<string>();
            Bans = new ObservableCollection<string>();
            Colors = new ObservableCollection<string>();
            Cities = new ObservableCollection<string>();
            FuelTypes = new ObservableCollection<string>();
            Drivetrains = new ObservableCollection<string>();
            Transmissions = new ObservableCollection<string>();
            Engines = new ObservableCollection<string>();
            PriceTypes = new ObservableCollection<string>();
            #endregion

            #region Makes
            // Adding Makes
            var makes = dtx.Makes.Select(make => new { make.Name });
            foreach (var make in makes)
            {
                Makes.Add(make.Name);
            }
            #endregion

            #region Models
            // Adding Models
            var models = dtx.Models.Select(model => new { model.Name });
            foreach (var model in models)
            {
                Models.Add(model.Name);
            }
            #endregion

            #region Ban Types
            // Adding Bans
            var bans = dtx.BanTypes.Select(ban => new { ban.Name });
            foreach (var ban in bans)
            {
                Bans.Add(ban.Name);
            }
            #endregion

            #region Colors
            // Adding Colors
            var colors = dtx.Colors.Select(color => new { color.Name });
            foreach (var color in colors)
            {
                Colors.Add(color.Name);
            }
            #endregion

            #region Cities
            // Adding Cities
            var cities = dtx.Cities.Select(city => new { city.Name });
            foreach (var city in cities)
            {
                Cities.Add(city.Name);
            }
            #endregion

            #region Fuel Types
            // Adding FuelTypes
            var fuelTypes = dtx.Cars.Select(cars => new { cars.Fuel }).Distinct();
            foreach (var type in fuelTypes)
            {
                FuelTypes.Add(type.Fuel);
            }
            #endregion

            #region Drivetrains
            // Adding Drivetrains
            var driveTrains = dtx.Cars.Select(cars => new { cars.Drivetrain }).Distinct();
            foreach (var driveTrain in driveTrains)
            {
                Drivetrains.Add(driveTrain.Drivetrain);
            }
            #endregion

            #region Transmissions
            // Adding Transmissions
            var transmissions = dtx.Cars.Select(cars => new { cars.Transmission }).Distinct();
            foreach (var transmission in transmissions)
            {
                Transmissions.Add(transmission.Transmission);
            }
            #endregion

            #region Engines
            // Adding Engines
            var engines = dtx.Cars.Select(cars => new { cars.Engine }).Distinct();
            foreach (var eng in engines)
            {
                Engines.Add(eng.Engine.ToString());
            }
            #endregion

            #region Price Symbol
            // Adding priceTypes
            var priceTypes = dtx.Cars.Select(cars => new { cars.PriceSymbol }).Distinct();
            foreach (var priceType in priceTypes)
            {
                PriceTypes.Add(priceType.PriceSymbol);
            }
            #endregion

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            #region Models

            Models.Clear();

            var models = dtx.Models.Where(model => model.Make.Name == MakeSelected)
                .Select(model => new { model.Name });
            foreach (var model in models)
            {
                Models.Add(model.Name);
            }

            #endregion

            #region Ban Types

            Bans.Clear();

            var bans =
            (
                from car in dtx.Cars
                join ban in dtx.BanTypes on car.BanId equals ban.Id
                join make in dtx.Makes on car.MakeId equals make.Id
                where make.Name == MakeSelected
                select new { ban.Name }).Distinct();

            foreach (var ban in bans)
            {
                Bans.Add(ban.Name);
            }

            #endregion

            #region Colors

            Colors.Clear();

            var colors =
            (
                from car in dtx.Cars
                join color in dtx.Colors on car.ColorId equals color.Id
                join make in dtx.Makes on car.MakeId equals make.Id
                where make.Name == MakeSelected
                select new { color.Name }).Distinct();

            foreach (var color in colors)
            {
                Colors.Add(color.Name);
            }

            #endregion

            #region Cities

            Cities.Clear();

            var cities =
            (
                from car in dtx.Cars
                join city in dtx.Cities on car.CityId equals city.Id
                join make in dtx.Makes on car.MakeId equals make.Id
                where make.Name == MakeSelected
                select new { city.Name }).Distinct();
            foreach (var city in cities)
            {
                Cities.Add(city.Name);
            }

            #endregion
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}