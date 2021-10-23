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

        private void OnNextRaceEvent(object sender, NextRaceEventArgs e)
        {
            // reinitialize
            ImageCache.ClearCache();
            Visualize.Initialize(e.Race);

            // link event
            e.Race.DriversChanged += OnDriversChanged;

            // Dispatcher is needed for execution of OnDriversChanged. Otherwise thread exceptions will occur.
            Dispatcher.Invoke((Action)(() =>
            {
                // e.Race.DriversChanged += ((MainDataContext)this.DataContext).OnDriversChanged;
            }));
        }

        private void OnDriversChanged(object sender, DriversChangedEventArgs e)
        {
            Track.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    Track.Source = null;
                    Track.Source = Visualize.DrawTrack(e.Track);
                }));
        }
    }
}