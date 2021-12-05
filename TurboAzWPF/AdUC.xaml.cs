using System;
using System.Collections.Generic;
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

namespace TurboAzWPF
{
    public partial class AdUC : UserControl
    {
        public DataClasses1DataContext dtx { get; set; } = new DataClasses1DataContext();

        public string MakeMod
        {
            get { return (string)GetValue(MakeModProperty); }
            set { SetValue(MakeModProperty, value); }
        }
        public static readonly DependencyProperty MakeModProperty =
            DependencyProperty.Register("MakeMod", typeof(string), typeof(AdUC));

        public string Prc
        {
            get { return (string)GetValue(PrcProperty); }
            set { SetValue(PrcProperty, value); }
        }
        public static readonly DependencyProperty PrcProperty =
            DependencyProperty.Register("Prc", typeof(string), typeof(AdUC));

        public string Inf
        {
            get { return (string)GetValue(InfProperty); }
            set { SetValue(InfProperty, value); }
        }
        public static readonly DependencyProperty InfProperty =
            DependencyProperty.Register("Inf", typeof(string), typeof(AdUC));

        public string City
        {
            get { return (string)GetValue(CityProperty); }
            set { SetValue(CityProperty, value); }
        }
        public static readonly DependencyProperty CityProperty =
            DependencyProperty.Register("City", typeof(string), typeof(AdUC));





        public AdUC(Car car)
        {
            InitializeComponent();
            DataContext = this;
            Ad = car;

            var make = dtx.Cars
                .Join(dtx.Makes, cars => new { Id = Convert.ToInt32(cars.MakeId) }, makes => new { makes.Id },
                    (cars, makes) => new { cars, makes })
                .Where(t => t.cars.Id == Ad.Id)
                .Select(t => new { t.makes.Name });
            var model = dtx.Cars
                .Join(dtx.Models, cars => new { Id = Convert.ToInt32(cars.ModelId) }, models => new { models.Id },
                    (cars, models) => new { Cars = cars, Models = models })
                .Where(t => t.Cars.Id == Ad.Id)
                .Select(t => new { t.Models.Name });

            var cty = dtx.Cars
                .Join(dtx.Cities, Cars => new { Id = Convert.ToInt32(Cars.CityId) }, cities => new { Id = cities.Id },
                    (Cars, Cities) => new { Cars, Cities })
                .Where(t => t.Cars.Id == Ad.Id)
                .Select(t => new { t.Cities.Name });

            MakeMod = $"{make.ToList()[0].Name} {model.ToList()[0].Name}";
            Prc = $"{Ad.Price} {Ad.PriceSymbol}";
            Inf = $"{Ad.Year}, {Ad.Engine} L, {Ad.Range} km";
            City = $"{cty.ToList()[0].Name}";
        }


        public Car Ad
        {
            get { return (Car)GetValue(AdProperty); }
            set { SetValue(AdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ad.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AdProperty =
            DependencyProperty.Register("Ad", typeof(Car), typeof(AdUC));


    }
}
