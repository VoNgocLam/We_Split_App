using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace We_Slipt_App
{
    
    public partial class SplashScreen : Window
    {
        private Random _rng = new Random();
        ObservableCollection<Trips> _trips;
        ObservableCollection<Trips> _trip;


        DispatcherTimer dTimer = new DispatcherTimer();
        string sFile = "";
        bool flag = true;
        public SplashScreen()
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            sFile = $"{folder}Check.txt";
            var data = File.ReadAllText(sFile);
            InitializeComponent();
            if (data == "checked")
            {
                MainWindow m = new MainWindow();
                m.Show();
                this.Close();
            }
            else
            {
                dTimer.Tick += new EventHandler(dTimer_Tick);
                dTimer.Interval = new TimeSpan(0, 0, 60);
                dTimer.Start();
            }
        }

        private void dTimer_Tick(object sender, EventArgs e)
        {
            if (flag == true)
            {
                MainWindow m = new MainWindow();
                m.Show();
                dTimer.Stop();
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory;
            var direcFile = $"{folder}Trips.txt";
            int lineCount = File.ReadLines(direcFile).Count();
            var database = File.ReadAllLines(direcFile);
            int count = lineCount / 6;
            _trips = new ObservableCollection<Trips>();
            for (int i = 0; i < count; i++)
            {
                var line1 = database[i * 6];
                var line2 = database[i * 6 + 1];
                var line3 = database[i * 6 + 2];
                var trip = new Trips()
                {
                    Name = line1,
                    Introduce = line2,
                    Picture = line3,
                };

                _trips.Add(trip);
            }
            var k = _rng.Next(_trips.Count);
            _trip = new ObservableCollection<Trips>();
            _trip.Add(_trips[k]);
            Title.Text = _trips[k].Name;
            sFile = $"{folder}Images\\{_trips[k].Picture}";
            BackgoundImg.ImageSource = new BitmapImage(new Uri(sFile));
            TripIntroduce.ItemsSource = _trip;
        }

        private void Check_Unchecked(object sender, RoutedEventArgs e)
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            sFile = $"{folder}Check.txt";
            File.Create(sFile).Close();
        }

        private void Check_Checked(object sender, RoutedEventArgs e)
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            sFile = $"{folder}Check.txt";
            File.AppendAllText(sFile, "checked");
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            flag = false;
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }
    }
}
