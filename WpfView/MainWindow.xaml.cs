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
using System.Windows.Threading;
using Controller;
using Model;

namespace WpfView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Data.Initialize();
            Data.NextRaceEvent += OnNextRaceEvent;
            Data.NextRace();
        }

        private void OnNextRaceEvent(object sender, NextRaceEventArgs args)
        {
            ImageCache.ClearCache();
            Visualize.Initialize(args.Race);

            args.Race.DriversChanged += OnDriversChanged;
            args.Race.RaceFinished += OnRaceFinished;

            Dispatcher.Invoke(() => { args.Race.DriversChanged += OnDriversChanged; });
        }

        private void OnDriversChanged(object sender, DriversChangedEventArgs args)
        {
            Track.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    Track.Source = null;
                    Track.Source = Visualize.DrawTrack(args.Track);
                }));
        }

        private void OnRaceFinished(object sender, EventArgs args)
        {
            ImageCache.ClearCache();
        }
    }
}