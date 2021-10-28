using System;
using System.Windows;
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
        private CompetitionStatistics _competitionStatistics;
        private RaceStatistics _raceStatistics;

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

            Dispatcher.Invoke(() => { args.Race.DriversChanged += ((MainDataContext)DataContext).OnDriversChanged; });
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

        private void MenuItem_Competition_OnClick(object sender, RoutedEventArgs e)
        {
            _competitionStatistics = new CompetitionStatistics();

            Data.NextRaceEvent += ((CompetitionDataContext)_competitionStatistics.DataContext).OnNextRaceEvent;

            ((CompetitionDataContext)_competitionStatistics.DataContext).OnNextRaceEvent(null,
                new NextRaceEventArgs(Data.CurrentRace));

            _competitionStatistics.Show();
        }

        private void MenuItem_Race_OnClick(object sender, RoutedEventArgs e)
        {
            _raceStatistics = new RaceStatistics();
            _raceStatistics.Show();
        }

        private void MenuItem_Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}