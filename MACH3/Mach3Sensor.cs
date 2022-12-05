using System;

namespace Mach3_netframework.MACH3
{
    public class Mach3Sensor
    {
        public event EventHandler<bool> SensorChanged;
        public int Number { get; }
        public int CheckValue { get; }
        public bool Detect
        {
            get => _detect;
            set
            {
                if (_detect != value)
                {
                    _detect = value;
                    SensorChanged?.Invoke(this, _detect);
                }
            }
        }
        private bool _detect = false;

        public Mach3Sensor(int number, int checkvalue)
        {
            this.Number = number;
            this.CheckValue = checkvalue;
        }

        public bool IsThis(int value)
        {
            return this.Number == value || (this.Number + 1) == value;
        }

        public void SetStat(int value)
        {
            this.Detect = (this.CheckValue == value);
        }
    }
}
