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
using LiveCharts;
using LiveCharts.Wpf;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace We_Slipt_App
{
    /// <summary>
    /// Interaction logic for TripDetailControl.xaml
    /// </summary>
    public partial class TripDetailControl : UserControl
    {
        private Trips _data;

        ObservableCollection<Cash> listMember = new ObservableCollection<Cash>();
        ObservableCollection<Cash> listExpenses = new ObservableCollection<Cash>();


        BindingList<Trips> _listdetails;
        public SeriesCollection ReceivedMoneyData { get; set; }
        public SeriesCollection ExpensesData { get; set; }
        public TripDetailControl(Trips t)
        {
            InitializeComponent();
            _data = t;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = _data;

            if(_data.Type==0)
            {
                Accomplished.Visibility = Visibility.Visible;
            };

            _listdetails = new BindingList<Trips>();
            ReceivedMoneyData = new SeriesCollection() { };
            ExpensesData = new SeriesCollection() { };
            Photo.ItemsSource = _listdetails;
            RoutesItemsControl.ItemsSource = _listdetails;
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            folder = folder + $"\\List\\{_data.Name}\\";
            var trip = new Trips()
            {
                ReceivedMoney = new ObservableCollection<Cash>(),
                Expenses = new ObservableCollection<Cash>(),
                Routes = new BindingList<string>(),
                Images = new BindingList<string>()
            };

            //Images
            string fileImages = folder + "Images.txt";
            var dataFileImages = File.ReadAllLines(fileImages);
            for(int i =0; i<dataFileImages.Length; i++)
            {
                trip.Images.Add(folder + dataFileImages[i]);
            };

            //Receive Money
            string fileReceivedMoney = folder + "Members.txt";
            var dataFileMember = File.ReadAllLines(fileReceivedMoney);
            int fileMemberCount = dataFileMember.Count() / 2;
            for (int i = 0; i < fileMemberCount; i++)
            {
                var line1 = dataFileMember[i * 2];
                int line2 = int.Parse(dataFileMember[i * 2 + 1]);
                var chartData = new PieSeries()
                {
                    Title=line1,
                    Values=new ChartValues<int> { line2 }
                };
                var cash = new Cash()
                {
                    Name = line1,
                    Value = line2
                };
                trip.ReceivedMoney.Add(cash);
                ReceivedMoneyData.Add(chartData);
            };

            // Expenses
            string fileExpenses = folder + "Expenses.txt";
            var dataFileExpenses = File.ReadAllLines(fileExpenses);
            int fileExpensesCount = dataFileExpenses.Count() / 2;
            for(int i=0; i<fileExpensesCount; i++)
            {
                var line1 = dataFileExpenses[i * 2];
                int line2 = int.Parse(dataFileExpenses[i * 2 + 1]);
                var chartData = new PieSeries()
                {
                    Title = line1,
                    Values = new ChartValues<int> { line2 }
                };
                var cash = new Cash()
                {
                    Name = line1,
                    Value = line2
                };
                trip.Expenses.Add(cash);
                ExpensesData.Add(chartData);
            }

            //Routes
            string fileRoutes = folder + "Location.txt";
            var dataFileRoutes = File.ReadAllLines(fileRoutes);
            for (int i = 0; i < dataFileRoutes.Length; i++)
            {
                trip.Routes.Add(dataFileRoutes[i]);
            };
            _listdetails.Add(trip);

            RoutesPanel.Visibility = Visibility.Visible;
            ButtonPanel.Visibility = Visibility.Visible;
            ExpensesChart.DataContext = ExpensesData;
            ReceivedMoneyChart.DataContext = ReceivedMoneyData;
            dataMembers.Visibility = Visibility.Visible;
            dataMembers.ItemsSource = _listdetails[0].ReceivedMoney;

            
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow m = new MainWindow();
                m.Show();
                Window.GetWindow(this).Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            if(HideButton.Content.Equals("Ẩn lộ trình"))
            {
                RoutesItemsControl.Visibility = Visibility.Collapsed;
                HideButton.Content = "Hiển thị lộ trình";
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString("#33c7b6");
                HideButton.Background = brush;
            }
            else
            {

                RoutesItemsControl.Visibility = Visibility.Visible;
                HideButton.Content = "Ẩn lộ trình";
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString("#fd7462");
                HideButton.Background = brush;

            }
        }

        private void AddMember_Click(object sender, RoutedEventArgs e)
        {
            ButtonPanel.Visibility = Visibility.Collapsed;
            dataMembers.Visibility = Visibility.Collapsed;
            MemberUserControl.Visibility = Visibility.Visible;
        }

        private void UpdateMember_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image Files (*png; *.jpg; *.jpeg; *.gif; *.bmp; )|*.png; *.jpg; *.jpeg; *.gif; *.bmp;";
            if (openFileDialog.ShowDialog() == true)
            {
                var folder = AppDomain.CurrentDomain.BaseDirectory;
                string folderTrip = folder + $"\\List\\{_data.Name}\\";
                string pathImage = folderTrip+ "Images.txt";
                using (StreamWriter sw = File.AppendText(pathImage))
                {
                    foreach (string item in openFileDialog.FileNames)
                    {
                        string nameImg = System.IO.Path.GetFileName(item);
                        sw.WriteLine(nameImg);
                        _listdetails[0].Images.Add(folderTrip + nameImg);
                        string imgDirec = folderTrip + "\\" + nameImg;
                        File.Copy(item, imgDirec, true);
                    }
                }               
            }
            Photo.ItemsSource = _listdetails;          
        }
        private void Accomplished_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có muốn kết thúc?", "", MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            {
                var folder = AppDomain.CurrentDomain.BaseDirectory;
                var direcFile = $"{folder}Trips.txt";
                int lineCount = File.ReadLines(direcFile).Count();
                var database = File.ReadAllLines(direcFile);
                int count = lineCount / 6;
                for (int i = 0; i < count; i++)
                {
                    var line1 = database[i * 6];
                    int line2 = int.Parse(database[i * 6 + 3]);
                    if (line1 == _data.Name && line2 == 0)
                    {
                        database[i * 6 + 3] = "1";
                    }
                }
                File.WriteAllLines(direcFile, database);
                try
                {
                    MainWindow m = new MainWindow();
                    m.Show();
                    Window.GetWindow(this).Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void AddMemberUseControl_Click(object sender, RoutedEventArgs e)
        {
            if(NameMember.Text == "")
            {
                MessageBox.Show("Họ tên không được để trống");
                ValueCash.Text = "";
            }
            else 
            {
                if (ValueCash.Text == "")
                    ValueCash.Text = "0";
                Cash item = new Cash()
                {
                    Name = NameMember.Text,
                    Value = int.Parse(ValueCash.Text)
                };
                var chartData = new PieSeries()
                {
                    Title = item.Name,
                    Values = new ChartValues<int> { item.Value }
                };

                listMember.Add(item);
                _listdetails[0].ReceivedMoney.Add(item);
                ReceivedMoneyData.Add(chartData);
                NameMember.Text = "";
                ValueCash.Text = "";
            }               
        }

        private void CreateMemberUseControl_Click(object sender, RoutedEventArgs e)
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory;
            string folderTrip = folder + $"\\List\\{_data.Name}\\";
            string pathMember = folderTrip + "Members.txt";
            for (int i = 0; i < listMember.Count(); i++)
            {
                using (StreamWriter sw = File.AppendText(pathMember))
                {
                    sw.WriteLine(listMember[i].Name);
                    sw.WriteLine(listMember[i].Value);
                }
            }
            ButtonPanel.Visibility = Visibility.Visible;
            dataMembers.Visibility = Visibility.Visible;
            dataMembers.ItemsSource = _listdetails[0].ReceivedMoney;
            MemberUserControl.Visibility = Visibility.Collapsed;
        }

        private void AddDetailTrip_Click(object sender, RoutedEventArgs e)
        {
            RoutesPanel.Visibility = Visibility.Collapsed;
            AddRoutesPanel.Visibility = Visibility.Visible;
        }
        private void AddExpenses_Click(object sender, RoutedEventArgs e)
        {
            if (TitlExpenses.Text == "" || ValueExpenses.Text == "")
            {
                MessageBox.Show("Mô tả không được để trống");
                TitlExpenses.Text = "";
                ValueExpenses.Text = "";
            }
            else
            {               
                Cash item = new Cash()
                {
                    Name = TitlExpenses.Text,
                    Value = int.Parse(ValueExpenses.Text)
                };
                var chartData = new PieSeries()
                {
                    Title = item.Name,
                    Values = new ChartValues<int> { item.Value }
                };
                listExpenses.Add(item);
                _listdetails[0].Expenses.Add(item);
                ExpensesData.Add(chartData);
                TitlExpenses.Text = "";
                ValueExpenses.Text = "";
            }
        }

        private void CreateRoutes_Click(object sender, RoutedEventArgs e)
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory;
            string folderTrip = folder + $"\\List\\{_data.Name}\\";
            string pathLocation = folderTrip + "Location.txt";
            string pathExpenses = folderTrip + "Expenses.txt";

            for (int i = 0; i < listExpenses.Count(); i++)
            {
                using (StreamWriter sw = File.AppendText(pathExpenses))
                {
                    sw.WriteLine(listExpenses[i].Name);
                    sw.WriteLine(listExpenses[i].Value);
                }
            }
            if(NameRoutes.Text != "")
            {
                using (StreamWriter sw = File.AppendText(pathLocation))
                {
                    sw.WriteLine(NameRoutes.Text);
                }
                _listdetails[0].Routes.Add(NameRoutes.Text);
            }
            RoutesItemsControl.ItemsSource = _listdetails;
            RoutesPanel.Visibility = Visibility.Visible;
            AddRoutesPanel.Visibility = Visibility.Collapsed;
        }

      
    }
}
