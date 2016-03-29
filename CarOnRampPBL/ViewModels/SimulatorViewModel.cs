using CarOnRampPBL.Services.PhysicsCalculator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace CarOnRampPBL.ViewModels
{
    #region Page Events
    public delegate void AddTimeLineMarkEventHandler(double xPos);
    #endregion

    class SimulatorViewModel : ViewModelBase
    {
        #region Private Members
        private const double DisplayLengthToUse = 1620;
        private const double DisplayHeightToUse = 500;
        private int _timeLineTime;
        private DispatcherTimer _smulatorTimer = new DispatcherTimer();

        public static event AddTimeLineMarkEventHandler AddTimeLineMark;
        #endregion

        public SimulatorViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                
            }

            _smulatorTimer.Tick += Smulatorimer_Tick;
            _smulatorTimer.Interval = new TimeSpan(0, 0, 0, 0, 250);
        }

        private void Smulatorimer_Tick(object sender, object e)
        {
            _timeLineTime++;
            TimeLinePointer += 20;
            if (TimeLinePointer >= 1615)
            {
                _smulatorTimer.Stop();
                TimeLinePointer = 10;
                _timeLineTime = 0;
            }
            else
            {
                App.PhyCal.CalculateCarPosition(new TimeSpan(0, 0, 0, 0, (int)(_timeLineTime * _smulatorTimer.Interval.TotalMilliseconds)));
                if (App.PhyCal.IsCarAtBottomOfRamp)
                {
                    if (AddTimeLineMark != null)
                        AddTimeLineMark(_timeLineTime * _smulatorTimer.Interval.TotalMilliseconds);
                }
            }
        }

        #region Bound Properties
        //Display
        public double DisplayLength { get { return DisplayLengthToUse; } }
        public double DisplayHeight { get { return DisplayHeightToUse; } }

        //Time Line
        private double _timeLinePointer = 10.0f;
        public double TimeLinePointer { get { return _timeLinePointer; } set { Set(ref _timeLinePointer, value); } }

        //Ramp
        private double _rampBottomX = 0.0f;
        public Double RampBottomX { get { return _rampBottomX; } set { Set(ref _rampBottomX, value); } }

        private double _rampBottomY = 0.0f;
        public Double RampBottomY { get { return _rampBottomY; } set { Set(ref _rampBottomY, value); } }

        private double _rampTopX = 0.0f;
        public Double RampTopX { get { return _rampTopX; } set { Set(ref _rampTopX, value); } }

        private double _rampTopY = 0.0f;
        public Double RampTopY { get { return _rampTopY; } set { Set(ref _rampTopY, value); } }

        //Floor
        private double _floorFrictionX1 = 0.0f;
        public Double FloorFrictionX1 { get { return _floorFrictionX1; } set { Set(ref _floorFrictionX1, value); } }

        private double _floorFrictionY1 = 0.0f;
        public Double FloorFrictionY1 { get { return _floorFrictionY1; } set { Set(ref _floorFrictionY1, value); } }

        private double _floorFrictionX2 = 0.0f;
        public Double FloorFrictionX2 { get { return _floorFrictionX2; } set { Set(ref _floorFrictionX2, value); } }

        private double _floorFrictionY2 = 0.0f;
        public Double FloorFrictionY2 { get { return _floorFrictionY2; } set { Set(ref _floorFrictionY2, value); } }

        private double _floorFriction = 0.0f;
        public Double FloorFriction { get { return _floorFriction; } set { Set(ref _floorFriction, value); } }

        //Sliders
        private double _rampLenght = 0.0f;
        public double RampLength
        {
            get { return _rampLenght; }
            set
            {
                Set(ref _rampLenght, value);
                DrawRamp();
            }
        }

        private double _rampHeight = 0.0f;
        public double RampHeight
        {
            get { return _rampHeight; }
            set
            {
                Set(ref _rampHeight, value);
                DrawRamp();
            }
        }

        private double _carPosition = 0.0f;
        public double CarPosition
        {
            get { return _carPosition; }
            set
            {
                Set(ref _carPosition, value);
                if ((RampLength > 0) && (RampHeight > 0))
                {
                    DrawRamp();
                }
            }
        }

        private double _carWeight = 0.0f;
        public double CarWeight
        {
            get { return _carWeight; }
            set
            {
                Set(ref _carWeight, value);
            }
        }

        private double _fCRamp = 0.0f;
        public double FCRamp
        {
            get { return _fCRamp; }
            set
            {
                Set(ref _fCRamp, value);
            }
        }

        private double _fCFloor = 0.0f;
        public double FCFloor
        {
            get { return _fCFloor; }
            set
            {
                Set(ref _fCFloor, value);
            }
        }

        private double _damageC = 0.0f;
        public double DamageC
        {
            get { return _damageC; }
            set
            {
                Set(ref _damageC, value);
            }
        }
        #endregion

        #region Commands
        public void RunSimulation()
        {
            _timeLineTime = 0;
            TimeLinePointer = 10;
            _smulatorTimer.Start();
        }

        public void PauseSimulation()
        {
            _smulatorTimer.Stop();
        }
        #endregion

        #region Navigation
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (App.PhyCal == null)
            {
                App.PhyCal = (suspensionState.ContainsKey(nameof(App.PhyCal))) ? (Calulator)suspensionState[nameof(App.PhyCal)] : null;
                if (App.PhyCal == null)
                    App.PhyCal = new Calulator();
                else
                {
                    if (AddTimeLineMark != null)
                            AddTimeLineMark(App.PhyCal.MarkerCarAtButtomOfRamp);
                }
            }
            else
            {
                if (AddTimeLineMark != null)
                    AddTimeLineMark(App.PhyCal.MarkerCarAtButtomOfRamp);
            }

            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            _smulatorTimer.Stop();

            if (suspending)
            {
                suspensionState[nameof(App.PhyCal)] = App.PhyCal;
            }

            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }
        #endregion

        #region DrawDisplay
        private void DrawRamp()
        {
            RampBottomX = DisplayLengthToUse - (RampLength * DisplayLengthToUse / 200);
            RampBottomY = DisplayHeightToUse - 10;

            double RampAngle = CalculateRampAngle();
            PointF RampTop = PointOnCircle((RampLength * DisplayLengthToUse / 200), RampAngle, new PointF() { X = RampBottomX, Y = RampBottomY });

            if (!double.IsNaN(RampTop.X) && !double.IsNaN(RampTop.Y))
            {
                RampTopX = RampTop.X;
                RampTopY = RampTop.Y;
            }
            else
            {
                RampTopX = DisplayLengthToUse;
                RampTopY = RampBottomY;
            }
        }

        private PointF PointOnCircle(double radius, double angleInDegrees, PointF origin)
        {
            // Convert from degrees to radians via multiplication by PI/180        
            double x = (double)(radius * Math.Cos(angleInDegrees * Math.PI / 180F)) + origin.X;
            double y = (double)(radius * Math.Sin(angleInDegrees * Math.PI / 180F)) + origin.Y;

            return new PointF() { X = x, Y = y };
        }

        private double CalculateRampAngle()
        {
            double rad = 0.0;

            if (RampLength > 0)
            {
                rad = Math.Asin(RampHeight / RampLength);
            }
            else
                return 0;

            return ((rad * 180F) / Math.PI) * -1;
        }
        #endregion
    }

    public class PointF
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
