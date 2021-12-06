using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TurboAzWPF.DataAccess.Concrete;
using TurboAzWPF.DataAccess.Context;

namespace TurboAzWPF
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<AdUC> AdUCs { get; set; } = new ObservableCollection<AdUC>();
        public ObservableCollection<AdUC> DefUCs { get; set; } = new ObservableCollection<AdUC>();
        public DataClasses1DataContext dtx { get; set; } = new DataClasses1DataContext();
        public GenericRepositoryPattern<Car> Repository = new GenericRepositoryPattern<Car>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            QueryAllCars();

        }

        private void QueryAllCars()
        {
            var result = Repository.GetAll().ToList();
            Dispatcher.Invoke(() => AdUCs.Clear());

            foreach (var car in result)
            {
                var carAd = new AdUC(car) { Width = 229, Height = 246 };
                Dispatcher.Invoke(() => AdUCs.Add(carAd));
                Dispatcher.Invoke(() => DefUCs.Add(carAd));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SearchTxt.Text != "")
            {
                SearchTxt.Text = "";
            }
            var uc = new SearchWindow();
            uc.ShowDialog();

            AdUCs.Clear();

            var query = from cars in dtx.Cars
                        join makes in dtx.Makes on cars.MakeId equals makes.Id
                        join models in dtx.Models on cars.ModelId equals models.Id
                        join banTypes in dtx.BanTypes on cars.BanId equals banTypes.Id
                        join colors in dtx.Colors on cars.ColorId equals colors.Id
                        join cities in dtx.Cities on cars.CityId equals cities.Id
                        select new
                        {
                            Cars = cars,
                            Makes = makes,
                            Models = models,
                            BanTypes = banTypes,
                            Colors = colors,
                            Cities = cities
                        };
            if (uc.MakeSelected != null)
            {
                query = query.Where(q => q.Makes.Name == uc.MakeSelected);
            }

            if (uc.ModelSelected != null)
            {
                query = query.Where(q => q.Models.Name == uc.ModelSelected);
            }

            if (uc.BanSelected != null)
            {
                query = query.Where(q => q.BanTypes.Name == uc.BanSelected);
            }

            if (uc.ColorSelected != null)
            {
                query = query.Where(q => q.Colors.Name == uc.ColorSelected);
            }

            if (uc.SymbolSelected != null)
            {
                query = query.Where(q => q.Cars.PriceSymbol == uc.SymbolSelected);
            }

            if (uc.CitySelected != null)
            {
                query = query.Where(q => q.Cities.Name == uc.CitySelected);
            }

            if (uc.FuelSelected != null)
            {
                query = query.Where(q => q.Cars.Fuel == uc.FuelSelected);
            }

            if (uc.DrivetrainSelected != null)
            {
                query = query.Where(q => q.Cars.Drivetrain == uc.DrivetrainSelected);
            }

            if (uc.TransmissionSelected != null)
            {
                query = query.Where(q => q.Cars.Transmission == uc.TransmissionSelected);
            }

            if (uc.EngMin != 0)
            {
                query = query.Where(q => q.Cars.Engine >= uc.EngMin);
            }

            if (uc.EngMax != 0)
            {
                query = query.Where(q => q.Cars.Engine <= uc.EngMax);
            }

            if (uc.YearMin != 0)
            {
                query = query.Where(q => q.Cars.Year >= uc.YearMin);
            }

            if (uc.YearMax != 0)
            {
                query = query.Where(q => q.Cars.Year <= uc.YearMax);
            }

            if (uc.MinRangeText == "0")
            {
                query = query.Where(q => q.Cars.Range >= int.Parse(uc.MinRangeText));
            }

            if (uc.MaxRangeText == "0")
            {
                query = query.Where(q => q.Cars.Range <= int.Parse(uc.MaxRangeText));
            }

            if (uc.MinPriceText != 0)
            {
                query = query.Where(q => q.Cars.Price >= uc.MinPriceText);
            }

            if (uc.MaxPriceText != 0)
            {
                query = query.Where(q => q.Cars.Price <= uc.MaxPriceText);
            }

            if (query.Any() != true)
            {
                MessageBox.Show("Bağışlayın, göndərdiyiniz sorğu üzrə heç bir nəticə tapılmamışdır.\nDigər meyarlar üzrə axtarışa cəhd edin.");
                QueryAllCars();
            }
            else
            {
                foreach (var smth in query)
                {
                    var carAd = new AdUC(smth.Cars) { Width = 229, Height = 246 };
                    AdUCs.Add(carAd);
                }
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox txt)) return;
            var text = txt.Text.ToLower();
            var result = DefUCs.Where(p => p.MakeMod.ToLower().Contains(text)).ToList();

            Dispatcher.Invoke(() => AdUCs.Clear());

            foreach (var car in result)
            {
                var carAd = new AdUC(car.Ad) { Width = 229, Height = 246 };
                Dispatcher.Invoke(() => AdUCs.Add(carAd));
            }
        }
    }
}
