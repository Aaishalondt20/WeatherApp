using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Windows.Input;
//using ThreadNetwork;
using WeatherApp.Models;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        private int _temp;

        public int Temp
        {
            get { return _temp; }
            set
            {
                _temp = value;
                OnPropertyChanged();
            }
        }

        private double _wind;
        public double Wind
        {
            get { return _wind; }
            set
            {
                _wind = value;
                OnPropertyChanged();
            }
        }

        private int _humidity;
        public int Humidity
        {
            get { return _humidity; }
            set
            {
                _humidity = value;
                OnPropertyChanged();
            }
        }

        private int _pressure;

        public int Pressure
        {
            get { return _pressure; }
            set
            {
                _pressure = value;
                OnPropertyChanged();
            }
        }

        private string _country;
        public string Country
        {
            get { return _country; }
            set
            {
                _country = value;
                OnPropertyChanged();
            }
        }

        private int _clouds;
        public int Clouds
        {
            get { return _clouds; }
            set
            {
                _clouds = value;
                OnPropertyChanged();
            }
        }

        private string _weatherdescription;
        public string WeatherDescription
        {
            get { return _weatherdescription; }
            set
            {
                _weatherdescription = value;
                OnPropertyChanged();
            }
        }

        private string _sunrise;

        public string Sunrise
        {
            get { return _sunrise; }
            set
            {
                _sunrise = value;
                OnPropertyChanged();
            }
        }

        private string _sunset;

        public string Sunset
        {
            get { return _sunset; }
            set
            {
                _sunset = value;
                OnPropertyChanged();
            }
        }

        private string dateModified;
        public string DateModified
        {
            get => dateModified;
            set
            {
                dateModified = value;
                OnPropertyChanged();
            }
        }

        private double _tempmin;

        public double TempMin
        {
            get { return _tempmin; }
            set { _tempmin = value; OnPropertyChanged(); }
        }
        private double _tempmax;
        public double TempMax
        {
            get { return _tempmax; }
            set { _tempmax = value; OnPropertyChanged(); }
        }

        private double _feelslike;

        public double FeelsLike
        {
            get { return _feelslike; }
            set { _feelslike = value; OnPropertyChanged(); }
        }

        private GPSModule _gpsmodule;
        private string _currentWeather;

        public MainPage()
        {
            InitializeComponent();
            ShowWeatherCommand = new Command(GetLatestWeather);
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _gpsmodule = new GPSModule();
            BindingContext = this;

        }

        public ICommand ShowWeatherCommand { get; set; }

        private HttpClient _client;

        public async void GetLatestWeather(object parameters)
        {
            activityindicator.IsRunning = true;
            activityindicator.IsVisible = true;
            activityindicator.HeightRequest = 30;
            activityindicator.WidthRequest = 30;
            await DisplayAlert("Notice", "Weather Is Updating...", "Okay");

            Location location = await _gpsmodule.GetCurrentLocation();
            double lat = location.Latitude;
            double lng = location.Longitude;

            string appid = "f0fd1ab3ab9ca3cbfa834f60222fa11c";
            string response = await _client.GetStringAsync(new Uri($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lng}&appid={appid}&units=metric"));

            WeatherForecast currentweather = JsonConvert.DeserializeObject<WeatherForecast>(response);



            if (currentweather != null)
            {
                Temp = (int)Math.Round(currentweather.main.temp);

                Wind = currentweather.wind.speed;

                DateTimeOffset dtOffset = DateTimeOffset.FromUnixTimeSeconds(currentweather.sys.sunrise);
                Sunrise = dtOffset.UtcDateTime.ToString();

                dtOffset = DateTimeOffset.FromUnixTimeSeconds(currentweather.sys.sunset);
                Sunset = dtOffset.UtcDateTime.ToString();

                dtOffset = DateTimeOffset.FromUnixTimeSeconds(currentweather.dt);
                DateModified = dtOffset.UtcDateTime.ToString();

                TempMax = Math.Round(currentweather.main.temp_max);

                TempMin = Math.Round(currentweather.main.temp_min);

                FeelsLike = Math.Round(currentweather.main.feels_like);

                Humidity = currentweather.main.humidity;

                Pressure = currentweather.main.pressure;

                Country = currentweather.sys.country;

                Clouds = currentweather.clouds.all;

                if (currentweather.weather.Count > 0)
                {
                    WeatherDescription = currentweather.weather[0].description.ToUpper();
                }

            }

            activityindicator.IsRunning = false;
            activityindicator.IsVisible = false;
        }
    }
}
