using Mach3_netframework.MACH3.Interface;
using Mach3_netframework.Tools;
using System;
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

        public bool TryStart { get; set; } = false;
        public bool ThisStop => CheckStop();
        private bool CheckStop()
        {
            if (TurnOnAll != null)
            {
                bool result = TurnOnAll.Invoke() == false ||
                    ((Position > Maximum) && TryStart == false);
                this.TryStart = false;
                return result;
            }
            return true;
        }

        public long Minimum { get; set; } = 0;
        public long Position { get; private set; } = 0;
        public long Maximum { get; set; } = int.MaxValue;

        /// <summary>
        /// Ports 
        /// [0][0], [0][1] — Up move ports
        /// [1][0], [1][1] — Down move ports
        /// </summary>
        private readonly short[,] Ports;

        public Mach3AxisMotor(string name, short[,] ports)
        {
            Name = name;
            Ports = ports;
        }

        public void Tic(int vector, long delay)
        {
            if (this.ThisStop == false)
            {
                int index = (1 + vector) / 2;

                Out?.Invoke(888, Ports[index, 0]);
                TimeTools.udelay(delay);
                Out?.Invoke(888, Ports[index, 1]);
                TimeTools.udelay(delay);
                Position += vector;
            }
        }

        public void SetZero() => this.Position = 0;
    }
}
