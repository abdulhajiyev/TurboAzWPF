using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TurboAzWPF.DataAccess.Concrete;
using TurboAzWPF.DataAccess.Context;

namespace TurboAzWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<AdUC> AdUCs { get; set; } = new ObservableCollection<AdUC>();
        public DataClasses1DataContext dtx { get; set; } = new DataClasses1DataContext();

        public GenericRepositoryPattern<Car> Repository = new GenericRepositoryPattern<Car>();
        //public ObservableCollection<Car> Data { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            QueryAllCars();

        }

        private void QueryAllCars()
        {
            Dispatcher.Invoke(() => AdUCs.Clear());

            var result = Repository.GetAll().ToList();

            //AdUCs.Clear();
            foreach (var car in result)
            {
                var carAd = new AdUC(car) { Width = 229, Height = 246 };
                Dispatcher.Invoke(() => AdUCs.Add(carAd));
                //AdUCs.Add(carAd);
            }
        }

        private void scroll_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("yaay");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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
            Dispatcher.Invoke(QueryAllCars);

            if (!(sender is TextBox txt)) return;
            var text = txt.Text.ToLower();
            var result = AdUCs.Where(p => p.MakeMod.ToLower().Contains(text)).ToList();

            Dispatcher.Invoke(() => AdUCs.Clear());

            foreach (var car in result)
            {
                var carAd = new AdUC(car.Ad) { Width = 229, Height = 246 };
                Dispatcher.Invoke(() => AdUCs.Add(carAd));
            }
        }
    }
}
