using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarOnRampPBL.Services.PhysicsCalculator
{
    public class Calulator
    {
        #region public Properties
        public double SetCarPosition { get; set; }
        public bool IsCarAtBottomOfRamp { get; set; }
        public double MarkerCarAtButtomOfRamp { get; set; }
        public double SetCarWeight { get; set; }
        public double SetRampHeight { get; set; }
        #endregion

        public Calulator()
        {
            IsCarAtBottomOfRamp = false;
        }

        #region Calulations
        public double CalulateCarVelocity(TimeSpan ClockMark)
        {
            return 0;
        }

        public double CalculateCarPosition(TimeSpan ClockMark)
        {
            if (ClockMark.TotalMilliseconds == 3000)
            {
                MarkerCarAtButtomOfRamp = ClockMark.TotalMilliseconds;
                IsCarAtBottomOfRamp = true;
            }
            else
                IsCarAtBottomOfRamp = false;

            return 0;
        }
        #endregion

    }
}
