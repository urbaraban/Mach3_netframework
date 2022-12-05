using System;
using System.Threading.Tasks;
using static MACH3.InpOut32x64.InpOut;
using static MACH3.Mach3Controller;

namespace MACH3
{
    internal class AxisMotor
    {
        public TurnDelegate Turn { get; set; }

        public OutDelegate Out { get; set; }
        public InpDelegate Inp { get; set; }

        public string Name { get; }

        public double RealCoordinatePosition => StepPostion / StepsInMillimetre;
        public double StartPosition { get; set; }
        public double AlreadyPosition => RealCoordinatePosition - StartPosition;
        public int StepPostion { get; private set; }

        public double Maximum { get; set; }


        public int StepsInMillimetre { get; set; }
        public double MilllimetresPerSec { get; set; }
        public int LPTSpeed => (int)(500000 / (MilllimetresPerSec * StepsInMillimetre));

        ///Ports
        public Tuple<short, short> UpPort { get; set; }
        public Tuple<short, short> DownPort { get; set; }

        public bool InverseZero { get; set; }
        public Mach3Sensor Sensor 
        {
            get => mashsensor;
            set
            {
                if (mashsensor != null)
                    mashsensor.SensorChanged -= Mashsensor_SensorChanged;
                mashsensor = value;
                mashsensor.SensorChanged += Mashsensor_SensorChanged;
            }
        }
        private Mach3Sensor mashsensor;

        private void Mashsensor_SensorChanged(object sender, bool e)
        {
            if (e == true)
            {
                this.StepPostion = InverseZero ? 0 : (int)(Maximum * StepsInMillimetre);
            }
        }

        public bool GoTo(double coordinate)
        {
            bool down = coordinate < AlreadyPosition;
            int vector = down ? -1 : 1;
            short port1 = down ? DownPort.Item1 : UpPort.Item1;
            short port2 = down ? DownPort.Item2 : UpPort.Item2;

            while (Turn?.Invoke() == true &&
                Sensor.Detect == false &&
                RealCoordinatePosition > 0.001 &&
                RealCoordinatePosition <= Maximum)
            {
                Out?.Invoke(888, port1);
                Task.Delay(this.LPTSpeed);
                Out?.Invoke(888, port2);
                Task.Delay(this.LPTSpeed);
                StepPostion += 1;
            }
            
            return Math.Abs(AlreadyPosition - coordinate) > 0.01;
        }

        public bool GoHome() => GoTo(InverseZero ? Maximum : 0);
    }
}
