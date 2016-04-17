using CarOnRampPBL.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CarOnRampPBL.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Simulator : Page
    {
        public Simulator()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = DataContext as SimulatorViewModel;
            vm.AddTimeLineMark += SimulatorViewModel_AddTimeLineMark;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var vm = DataContext as SimulatorViewModel;
            vm.AddTimeLineMark -= SimulatorViewModel_AddTimeLineMark;
        }

        private void SimulatorViewModel_AddTimeLineMark(double xPosTime)
        {
            Polygon p = new Polygon();
            p.Stroke = new SolidColorBrush(Colors.Black);
            p.Fill = new SolidColorBrush(Colors.Yellow);
            p.StrokeThickness = 1;
            p.HorizontalAlignment = HorizontalAlignment.Left;
            p.VerticalAlignment = VerticalAlignment.Center;

            p.Points = new PointCollection()
            {
                new Point(5, 0),
                new Point(10, 5),
                new Point(5, 10),
                new Point(0, 5)
            };

            Canvas.SetLeft(p, (xPosTime * 20 / 250));
            Canvas.SetTop(p, 15);

            cvsTimeLine.Children.Add(p);
        }
    }
}
