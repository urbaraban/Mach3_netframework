using MACH3.InpOut32x64;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace MACH3
{
    internal class Mach3Controller
    {
        public delegate bool TurnDelegate();
        public bool IsTurn { get; set; }
        public bool Press { get; set; }

        public List<AxisMotor> motorList { get; } = new List<AxisMotor>();

        public Mach3Controller Sensors { get; set; }

        private InpOut inpOut = new InpOut();

        public Mach3Controller()
        {

        }

        public bool GetTurn() => IsTurn;

        public bool GoHome()
        {
            bool result = true;
            for (int i = 0; i < motorList.Count && result == true; i += 1)
            {
               result = motorList[i].GoHome();
            }
            return result;
        }
    }
}
