using System;

namespace MACH3
{
    internal class Mach3Sensor
    {
        public event EventHandler<bool> SensorChanged;
        public string Name { get; set; } = "Empty";
        public int Number { get; }
        public int TrueValue { get; }
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

        public Mach3Sensor(string name, int number, int truevalue)
        {
            this.Name = name;
            this.Number = number;
            this.TrueValue = truevalue;
        }

        public void SetStat(int value)
        {
            this.Detect = (this.TrueValue == value);
        }
    }
}
