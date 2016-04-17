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
        private const double CarLength = 15;
        private const double OffSetX = -10;
        private const double OffSetY = -10;
        private int _timeLineTime;
        private DispatcherTimer _smulatorTimer = new DispatcherTimer();
        private bool _isRampChanging = false;
        #endregion

        #region Events
        public event AddTimeLineMarkEventHandler AddTimeLineMark;
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
        //Car
        private double _carBodyP1x = 0.0f;
        public double CarBodyP1x { get { return _carBodyP1x; } set { Set(ref _carBodyP1x, value); } }

        private double _carBodyP1y = 0.0f;
        public double CarBodyP1y { get { return _carBodyP1y; } set { Set(ref _carBodyP1y, value); } }

        private double _carBodyP2x = 0.0f;
        public double CarBodyP2x { get { return _carBodyP2x; } set { Set(ref _carBodyP2x, value); } }

        private double _carBodyP2y = 0.0f;
        public double CarBodyP2y { get { return _carBodyP2y; } set { Set(ref _carBodyP2y, value); } }

        private double _carWheelFrontx = 0.0f;
        public double CarWheelFrontx { get { return _carWheelFrontx; } set { Set(ref _carWheelFrontx, value); } }

        private double _carWheelFronty = 0.0f;
        public double CarWheelFronty { get { return _carWheelFronty; } set { Set(ref _carWheelFronty, value); } }

        private double _carWheelBackx = 0.0f;
        public double CarWheelBackx { get { return _carWheelBackx; } set { Set(ref _carWheelBackx, value); } }

        private double _carWheelBacky = 0.0f;
        public double CarWheelBacky { get { return _carWheelBacky; } set { Set(ref _carWheelBacky, value); } }

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
        private double _rampLength = 20.0f;
        public double RampLength
        {
            get { return _rampLength; }
            set
            {
                Set(ref _rampLength, value);
                if (!_isRampChanging)
                {
                    _isRampChanging = true;
                    RampLengthDisplay = string.Format("{0:###} cm", value);
                    DrawRamp();
                }
                _isRampChanging = false;
            }
        }

        private string _rampLengthDisplay = string.Empty;
        public string RampLengthDisplay
        {
            get { return _rampLengthDisplay; }
            set
            {
                string strRampLength = value.TrimEnd(new char[] { 'c', 'm' });
                double dRampLength = 0.0;
                if (double.TryParse(strRampLength, out dRampLength))
                {
                    Set(ref _rampLengthDisplay, value);
                    if (!_isRampChanging)
                    {
                        _isRampChanging = true;
                        RampLength = dRampLength;
                        DrawRamp();
                    }
                    _isRampChanging = false;
                }
                else
                {
                    RampLength = 20.0f;
                    Set(ref _rampLengthDisplay, "20 cm");
                }
            }
        }

        private double _rampHeight = 0.0f;
        public double RampHeight
        {
            get { return _rampHeight; }
            set
            {
                if (value <= _rampLength)
                {
                    Set(ref _rampHeight, value);
                    if (!_isRampChanging)
                    {
                        _isRampChanging = true;
                        RampHeightDisplay = string.Format("{0:###} cm", value);
                        DrawRamp();
                    }
                    _isRampChanging = false;
                }
            }
        }

        private string _rampHeightDisplay = string.Empty;
        public string RampHeightDisplay
        {
            get { return _rampHeightDisplay; }
            set
            {
                string strRampHeight = value.TrimEnd(new char[] { 'c', 'm' });
                double dRampHeight = 0.0;
                if (double.TryParse(strRampHeight, out dRampHeight))
                {
                    if (dRampHeight <= _rampLength)
                    {
                        Set(ref _rampHeightDisplay, value);
                        if (!_isRampChanging)
                        {
                            _isRampChanging = true;
                            RampHeight = dRampHeight;
                            DrawRamp();
                        }
                        _isRampChanging = false;
                    }
                }
                else
                {
                    RampHeight = 0.0f;
                    Set(ref _rampHeightDisplay, "0 cm");
                }
            }
        }

        private double _carPosition = (CarLength / 2);
        public double CarPosition
        {
            get { return _carPosition; }
            set
            {
                if ((value <= (_rampLength - CarLength / 2)) && (value >= (CarLength / 2)))
                {
                    Set(ref _carPosition, value);
                    if (!_isRampChanging)
                    {
                        _isRampChanging = true;
                        CarPositionDisplay = string.Format("{0:###} cm", value);
                        DrawRamp();
                    }
                    _isRampChanging = false;
                }
            }
        }

        private string _carPositionDisplay = string.Empty;
        public string CarPositionDisplay
        {
            get { return _carPositionDisplay; }
            set
            {
                string strCarPosition = value.TrimEnd(new char[] { 'c', 'm' });
                double dCarPosition = 0.0;
                if (double.TryParse(strCarPosition, out dCarPosition))
                {
                    if ((dCarPosition <= (_rampLength - CarLength / 2)) && (dCarPosition >= (CarLength / 2)))
                    {
                        Set(ref _carPositionDisplay, value);
                        if (!_isRampChanging)
                        {
                            _isRampChanging = true;
                            CarPosition = dCarPosition;
                            DrawRamp();
                        }
                        _isRampChanging = false;
                    }
                }
                else
                {
                    CarPosition = (CarLength / 2);
                    Set(ref _carPositionDisplay, string.Format("{0:###} cm", (CarLength / 2)));
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
        private void DrawCar()
        {
            double CarLengthScale = (CarLength * DisplayLengthToUse / 200);
            double OffSetXScale = CalculateOffSetX((OffSetX * DisplayLengthToUse / 200));
            double OffSetYScale = CalculateOffSetY((OffSetY * DisplayLengthToUse / 200));
            double WheelOffSetScale = 26;
            double CarPositionScale = (CarPosition * DisplayLengthToUse / 200);
            double angleInDegrees = CalculateRampAngle();

            PointF CarBodyFront = new PointF();
            CarBodyFront.X = (double)((CarPositionScale - (CarLengthScale / 2)) * Math.Cos(angleInDegrees * Math.PI / 180F)) + RampBottomX + OffSetXScale;
            CarBodyFront.Y = (double)((CarPositionScale - (CarLengthScale / 2)) * Math.Sin(angleInDegrees * Math.PI / 180F)) + RampBottomY + OffSetYScale;

            PointF CarBodyBack = new PointF();
            CarBodyBack.X = (double)((CarPositionScale + (CarLengthScale / 2)) * Math.Cos(angleInDegrees * Math.PI / 180F)) + RampBottomX + OffSetXScale;
            CarBodyBack.Y = (double)((CarPositionScale + (CarLengthScale / 2)) * Math.Sin(angleInDegrees * Math.PI / 180F)) + RampBottomY + OffSetYScale;

            PointF CarWheelFront = new PointF();
            CarWheelFront.X = (double)((CarPositionScale - (CarLengthScale / 2) + WheelOffSetScale) * Math.Cos(angleInDegrees * Math.PI / 180F)) + RampBottomX + OffSetXScale;
            CarWheelFront.Y = (double)((CarPositionScale - (CarLengthScale / 2) + WheelOffSetScale) * Math.Sin(angleInDegrees * Math.PI / 180F)) + RampBottomY + OffSetYScale;

            PointF CarWheelBack = new PointF();
            CarWheelBack.X = (double)((CarPositionScale + (CarLengthScale / 2) - WheelOffSetScale) * Math.Cos(angleInDegrees * Math.PI / 180F)) + RampBottomX + OffSetXScale;
            CarWheelBack.Y = (double)((CarPositionScale + (CarLengthScale / 2) - WheelOffSetScale) * Math.Sin(angleInDegrees * Math.PI / 180F)) + RampBottomY + OffSetYScale;

            CarBodyP1x = CarBodyFront.X;
            CarBodyP1y = CarBodyFront.Y;

            CarBodyP2x = CarBodyBack.X;
            CarBodyP2y = CarBodyBack.Y;

            CarWheelBackx = CarWheelBack.X - 20;
            CarWheelBacky = CarWheelBack.Y - 20;

            CarWheelFrontx = CarWheelFront.X - 20;
            CarWheelFronty = CarWheelFront.Y - 20;
        }

        private double CalculateOffSetY(double OffSetScale)
        {
            double angleInDegrees = CalculateRampAngle();
            double SinOfAngle = Math.Sin((180 - angleInDegrees) * Math.PI / 180F);

            if ((SinOfAngle > 0) && !double.IsNaN(SinOfAngle))
                return OffSetScale / SinOfAngle;
            else
                return 0;
        }

        private double CalculateOffSetX(double OffSetScale)
        {
            double angleInDegrees = CalculateRampAngle();
            double SinOfAngle = Math.Sin(angleInDegrees * Math.PI / 180F);

            if (!double.IsNaN(SinOfAngle))
                return OffSetScale * SinOfAngle;
            else
                return 0;
        }

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
            DrawCar();
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
