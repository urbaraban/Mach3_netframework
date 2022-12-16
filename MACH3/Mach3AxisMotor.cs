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

        public bool ThisStop => TurnOnAll.Invoke() == false;

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

        public void Tic(MoveVector vector, long delay)
        {
            int index = (1 + (int)vector) / 2;

            Out?.Invoke(888, Ports[index, 0]);
            TimeTools.udelay(delay);
            Out?.Invoke(888, Ports[index, 1]);
            TimeTools.udelay(delay);
            Position += (int)vector;
        }

        public void SetZero() => this.Position = 0;
    }

    public enum MoveVector
    {
        UP = 1,
        DOWN = -1
    }
}
