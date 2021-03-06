﻿using System;
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
using System.Globalization;

namespace We_Slipt_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
        }

        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        ObservableCollection<Trips> _trips;
        ObservableCollection<object> _detailtrip;


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#2188fb");
            var fore = (Brush)converter.ConvertFromString("white");
            var border = (Brush)converter.ConvertFromString("Transparent");

            All.Background = brush;
            All.Foreground = fore;
            All.BorderBrush = border;

            string sMember = "";
            int j = 0;
            int k = 1;
            var folder = AppDomain.CurrentDomain.BaseDirectory;
            var direcFile = $"{folder}Trips.txt";
            var memFile = $"{folder}Trips-Members.txt";
            int lineCount = File.ReadLines(direcFile).Count();
            var database = File.ReadAllLines(direcFile);
            var dataFile = File.ReadAllLines(memFile);
            int count = lineCount / 6;
            _trips = new ObservableCollection<Trips>();
            for (int i = 0; i < count; i++)
            {
                var line1 = database[i * 6];
                var line2 = database[i * 6 + 1];
                var line3 = database[i * 6 + 2];
                int line4 = int.Parse(database[i * 6 + 3]);
                DateTime line5 = DateTime.ParseExact(database[i * 6 + 4], "MM-dd-yyyy", CultureInfo.InvariantCulture);
                DateTime line6 = DateTime.ParseExact(database[i * 6 + 5], "MM-dd-yyyy", CultureInfo.InvariantCulture);
                var trip = new Trips()
                {
                    Name = line1,
                    Introduce=line2,
                    Picture = $"{folder}Images\\{line3}",
                    Type = (Trips.TripType)line4,
                    StartDate = line5.ToShortDateString(),
                    EndDate = line6.ToShortDateString(),
                    Members= new BindingList<string>(),
                    sMembers ="",
                };
                while (j < dataFile.Length - 1)
                {
                    if (dataFile[j].Contains("- " + line1))
                    {
                        while (k < dataFile.Length)
                        {
                            trip.Members.Add(dataFile[k]);
                            sMember += dataFile[k] + " ";
                            k++;
                            j++;
                            if (k == dataFile.Length)
                            {
                                trip.sMembers = sMember;
                                break;
                            }
                            if (dataFile[k].Contains("-"))
                            {
                                trip.sMembers = sMember;
                                j++;
                                k++;
                                break;
                            };
                        };
                        sMember = "";
                        break;
                    }
                    else
                    {
                        j++;
                        k++;
                    }
                }
                _trips.Add(trip);
            }
            dataListview.ItemsSource = _trips;
            SearchTextBox.Text = "";
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dataListview.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Type");
            view.GroupDescriptions.Add(groupDescription);
        }

         private void ExitButton_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
           }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(Main.ActualWidth == 1400)
                {
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void All_Click(object sender, RoutedEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#2188fb");
            var border = (Brush)converter.ConvertFromString("#e5e5e5");
            var white = (Brush)converter.ConvertFromString("white");
            var black = (Brush)converter.ConvertFromString("black");
            var transparent = (Brush)converter.ConvertFromString("Transparent");

            All.Background = brush;
            All.Foreground = white;
            All.BorderBrush = transparent;
            Processing.Background = transparent;
            Processing.Foreground = black;
            Processing.BorderBrush = border;
            Accomplished.Background = transparent;
            Accomplished.Foreground = black;
            Accomplished.BorderBrush = border;

            string sMember = "";
            int j = 0;
            int k = 1;
            var folder = AppDomain.CurrentDomain.BaseDirectory;
            var direcFile = $"{folder}Trips.txt";
            var memFile = $"{folder}Trips-Members.txt";
            int lineCount = File.ReadLines(direcFile).Count();
            var database = File.ReadAllLines(direcFile);
            var dataFile = File.ReadAllLines(memFile);
            int count = lineCount / 6;
            _trips = new ObservableCollection<Trips>();
            for (int i = 0; i < count; i++)
            {
                var line1 = database[i * 6];
                var line2 = database[i * 6 + 1];
                var line3 = database[i * 6 + 2];
                int line4 = int.Parse(database[i * 6 + 3]);
                DateTime line5 = DateTime.ParseExact(database[i * 6 + 4], "MM-dd-yyyy", CultureInfo.InvariantCulture);
                DateTime line6 = DateTime.ParseExact(database[i * 6 + 5], "MM-dd-yyyy", CultureInfo.InvariantCulture);

                var trip = new Trips()
                {
                    Name = line1,
                    Introduce = line2,
                    Picture = $"{folder}Images\\{line3}",
                    Type = (Trips.TripType)line4,
                    StartDate = line5.ToShortDateString(),
                    EndDate = line6.ToShortDateString(),
                    Members = new BindingList<string>(),
                    sMembers = "",
                };
                while (j < dataFile.Length - 1)
                {
                    if (dataFile[j].Contains("- " + line1))
                    {
                        while (k < dataFile.Length)
                        {
                            trip.Members.Add(dataFile[k]);
                            sMember += dataFile[k] + " ";
                            k++;
                            j++;
                            if (k == dataFile.Length)
                            {
                                trip.sMembers = sMember;
                                break;
                            }
                            if (dataFile[k].Contains("-"))
                            {
                                trip.sMembers = sMember;
                                j++;
                                k++;
                                break;
                            };
                        };
                        sMember = "";
                        break;
                    }
                    else
                    {
                        j++;
                        k++;
                    }
                }
                _trips.Add(trip);
            }
            dataListview.ItemsSource = _trips;
            MemberItemsControl.ItemsSource = _trips.Take(0);
            SearchTextBox.Text = "";

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dataListview.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Type");
            view.GroupDescriptions.Add(groupDescription);
        }

        private void Processing_Click(object sender, RoutedEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#2188fb");
            var border = (Brush)converter.ConvertFromString("#e5e5e5");
            var white = (Brush)converter.ConvertFromString("white");
            var black = (Brush)converter.ConvertFromString("black");
            var transparent = (Brush)converter.ConvertFromString("Transparent");
             
            Processing.Background = brush;
            Processing.Foreground = white;
            Processing.BorderBrush = transparent;
            All.Background = transparent;
            All.Foreground = black;
            All.BorderBrush = border;
            Accomplished.Background = transparent;
            Accomplished.Foreground = black;
            Accomplished.BorderBrush = border;

            string sMember = "";
            int j = 0;
            int k = 1;
            var folder = AppDomain.CurrentDomain.BaseDirectory;
            var direcFile = $"{folder}Trips.txt";
            var memFile = $"{folder}Trips-Members.txt";
            int lineCount = File.ReadLines(direcFile).Count();
            var database = File.ReadAllLines(direcFile);
            var dataFile = File.ReadAllLines(memFile);
            int count = lineCount / 6;
            _trips = new ObservableCollection<Trips>();
            for (int i = 0; i < count; i++)
            {
                int line4 = int.Parse(database[i * 6 + 3]);
                if (line4 == 0)
                {
                    var line1 = database[i * 6];
                    var line2 = database[i * 6 + 1];
                    var line3 = database[i * 6 + 2];
                    DateTime line5 = DateTime.ParseExact(database[i * 6 + 4], "MM-dd-yyyy", CultureInfo.InvariantCulture);
                    DateTime line6 = DateTime.ParseExact(database[i * 6 + 5], "MM-dd-yyyy", CultureInfo.InvariantCulture);

                    var trip = new Trips()
                    {
                        Name = line1,
                        Introduce = line2,
                        Picture = $"{folder}Images\\{line3}",
                        Type = (Trips.TripType)line4,
                        StartDate = line5.ToShortDateString(),
                        EndDate = line6.ToShortDateString(),
                        Members = new BindingList<string>(),
                        sMembers = "",
                    };
               
                    while (j < dataFile.Length - 1)
                    {
                        if (dataFile[j].Contains("- " + line1))
                        {
                            while (k < dataFile.Length)
                            {
                                trip.Members.Add(dataFile[k]);
                                sMember += dataFile[k] + " ";
                                k++;
                                j++;
                                if (k == dataFile.Length)
                                {
                                    trip.sMembers = sMember;
                                    break;
                                }
                                if (dataFile[k].Contains("-"))
                                {
                                    trip.sMembers = sMember;
                                    j++;
                                    k++;
                                    break;
                                };
                            };
                            sMember = "";
                            break;
                        }
                        else
                        {
                            j++;
                            k++;
                        }
                    }
                    _trips.Add(trip);
                }              
            }
            dataListview.ItemsSource = _trips;
            MemberItemsControl.ItemsSource = _trips.Take(0);
            SearchTextBox.Text = "";

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dataListview.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Type");
            view.GroupDescriptions.Add(groupDescription);

        }

        private void Accomplished_Click(object sender, RoutedEventArgs e)
        {
            var converter = new System.Windows.Media.BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#2188fb");
            var border = (Brush)converter.ConvertFromString("#e5e5e5");
            var white = (Brush)converter.ConvertFromString("white");
            var black = (Brush)converter.ConvertFromString("black");
            var transparent = (Brush)converter.ConvertFromString("Transparent");
            
            Accomplished.Background = brush;
            Accomplished.Foreground = white;
            Accomplished.BorderBrush = transparent;
            All.Background = transparent;
            All.Foreground = black;
            All.BorderBrush = border;
            Processing.Background = transparent;
            Processing.Foreground = black;
            Processing.BorderBrush = border;

            string sMember = "";
            int j = 0;
            int k = 1;
            var folder = AppDomain.CurrentDomain.BaseDirectory;
            var direcFile = $"{folder}Trips.txt";
            var memFile = $"{folder}Trips-Members.txt";
            int lineCount = File.ReadLines(direcFile).Count();
            var database = File.ReadAllLines(direcFile);
            var dataFile = File.ReadAllLines(memFile);
            int count = lineCount / 6;
            _trips = new ObservableCollection<Trips>();
            for (int i = 0; i < count; i++)
            {
                int line4 = int.Parse(database[i * 6 + 3]);
                if (line4 == 1)
                {
                    var line1 = database[i * 6];
                    var line2 = database[i * 6 + 1];
                    var line3 = database[i * 6 + 2];
                    DateTime line5 = DateTime.ParseExact(database[i * 6 + 4], "MM-dd-yyyy", CultureInfo.InvariantCulture);
                    DateTime line6 = DateTime.ParseExact(database[i * 6 + 5], "MM-dd-yyyy", CultureInfo.InvariantCulture);

                    var trip = new Trips()
                    {
                        Name = line1,
                        Introduce = line2,
                        Picture = $"{folder}Images\\{line3}",
                        Type = (Trips.TripType)line4,
                        StartDate = line5.ToShortDateString(),
                        EndDate = line6.ToShortDateString(),
                        Members = new BindingList<string>(),
                        sMembers = "",
                    };

                    while (j < dataFile.Length - 1)
                    {
                        if (dataFile[j].Contains("- " + line1))
                        {
                            while (k < dataFile.Length)
                            {
                                trip.Members.Add(dataFile[k]);
                                sMember += dataFile[k] + " ";
                                k++;
                                j++;
                                if (k == dataFile.Length)
                                {
                                    trip.sMembers = sMember;
                                    break;
                                }
                                if (dataFile[k].Contains("-"))
                                {
                                    trip.sMembers = sMember;
                                    j++;
                                    k++;
                                    break;
                                };
                            };
                            sMember = "";
                            break;
                        }
                        else
                        {
                            j++;
                            k++;
                        }
                    }
                    _trips.Add(trip);
                }
            }
            dataListview.ItemsSource = _trips;
            MemberItemsControl.ItemsSource = _trips.Take(0);
            SearchTextBox.Text = "";

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dataListview.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Type");
            view.GroupDescriptions.Add(groupDescription);

        }
               
        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                dataListview.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            dataListview.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        public class SortAdorner : Adorner
        {
            private static Geometry ascGeometry =
                Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

            private static Geometry descGeometry =
                Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

            public ListSortDirection Direction { get; private set; }

            public SortAdorner(UIElement element, ListSortDirection dir)
                : base(element)
            {
                this.Direction = dir;
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);

                if (AdornedElement.RenderSize.Width < 20)
                    return;

                TranslateTransform transform = new TranslateTransform
                    (
                        AdornedElement.RenderSize.Width - 15,
                        (AdornedElement.RenderSize.Height - 5) / 2
                    );
                drawingContext.PushTransform(transform);

                Geometry geometry = ascGeometry;
                if (this.Direction == ListSortDirection.Descending)
                    geometry = descGeometry;
                drawingContext.DrawGeometry(Brushes.Black, null, geometry);

                drawingContext.Pop();
            }
        }

        private void dataListview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = dataListview.SelectedValue;
            _detailtrip = new ObservableCollection<Object>();
            _detailtrip.Add(index);
            MemberItemsControl.ItemsSource = _detailtrip;
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var searchkeyword = SearchTextBox.Text;
           
                ObservableCollection<Trips> results = new ObservableCollection<Trips>();
                for (int i = 0; i < _trips.Count; i++)
                {
                    if ((_trips[i].Name).ToLower().Contains((SearchTextBox.Text).ToLower()) || (_trips[i].Name).ToUpper().Contains((SearchTextBox.Text).ToUpper()) || (_trips[i].sMembers).ToLower().Contains((SearchTextBox.Text).ToLower()) || (_trips[i].sMembers).ToUpper().Contains((SearchTextBox.Text).ToUpper()))
                    {
                        results.Add(_trips[i]);
                    }
                }
                if (results.Count > 0)
                {
                    dataListview.ItemsSource = results;
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dataListview.ItemsSource);
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("Type");
                    view.GroupDescriptions.Add(groupDescription);
                }
                if (results.Count == 0)
                {
                    dataListview.ItemsSource = results.Take(0);
                }
            }

        private void dataListview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var trip = (sender as ListView).SelectedItem as Trips;
            if (trip != null)
            {
                Main.Children.Clear();
                Main.Children.Add(new TripDetailControl(trip));
            }
        }
    }
}
