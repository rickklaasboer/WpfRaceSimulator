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
            CompetitionDataContext competitionDataContext = (CompetitionDataContext)_competitionStatistics.DataContext;

            Data.NextRaceEvent += competitionDataContext.OnNextRaceEvent;
            competitionDataContext.OnNextRaceEvent(null, new NextRaceEventArgs(Data.CurrentRace));
            Data.CurrentRace.DriversChanged += competitionDataContext.OnDriversChanged;

            _competitionStatistics.Show();
        }

        private void MenuItem_Race_OnClick(object sender, RoutedEventArgs e)
        {
            _raceStatistics = new RaceStatistics();
            RaceDataContext raceDataContext = (RaceDataContext)_raceStatistics.DataContext;

            Data.NextRaceEvent += raceDataContext.OnNextRaceEvent;
            raceDataContext.OnNextRaceEvent(null, new NextRaceEventArgs(Data.CurrentRace));
            Data.CurrentRace.DriversChanged += raceDataContext.OnDriversChanged;

            _raceStatistics.Show();
        }

        private void MenuItem_Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}