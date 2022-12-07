using Mach3_netframework.MACH3.Interface;
using System;
using System.Threading.Tasks;
using static Mach3_netframework.MACH3.InpOut32x64.InpOut;
using static Mach3_netframework.MACH3.Mach3;

namespace Mach3_netframework.MACH3
{
    public class Mach3AxisMotor : ITurnElement, IInpOutElement
    {
        public OutDelegate Out { get; set; }
        public InpDelegate Inp { get; set; }

        public string Name { get; }

        public TurnDelegate TurnOnAll { get; set; }
        public bool ThisStop => CheckStop();
        private bool CheckStop()
        {
            if (TurnOnAll != null)
            {
                return TurnOnAll.Invoke() == false
                    || Position == Minimum
                    || Position == Maximum;
            }
            return true;
        }


        public int Minimum { get; set; } = 0;
        public int Position { get; private set; } = 0;
        public int Maximum { get; set; } = int.MaxValue;


        /// <summary>
        /// Ports 
        /// [0][0], [0][1] — Up move ports
        /// [1][0], [1][1] — Down move ports
        /// </summary>
        private readonly short[,] Ports;

        public bool InverseZero { get; set; }

        private void Mashsensor_SensorChanged(object sender, bool e)
        {
            if (e == true)
            {
                this.Position = InverseZero ? 0 : (int)(Maximum);
            }
        }

        public Mach3AxisMotor(string name, short[,] ports)
        {
            Name = name;
            Ports = ports;
        }

        public void Tic(int vector, int delay)
        {
            if (this.ThisStop == false)
            {
                int index = (1 + vector) / 2;

                Out?.Invoke(888, Ports[index, 0]);
                Task.Delay(delay);
                Out?.Invoke(888, Ports[index, 1]);
                Task.Delay(delay);
                Position += vector;
            }
        }
    }
}
