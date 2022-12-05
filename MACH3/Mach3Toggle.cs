using Mach3_netframework.MACH3.Interface;
using System.Threading.Tasks;
using static Mach3_netframework.MACH3.InpOut32x64.InpOut;
using static Mach3_netframework.MACH3.Mach3;

namespace Mach3_netframework.MACH3
{
    public class Mach3Toggle : ITurnElement, IInpOutElement
    {
        public TurnDelegate TurnOnAll { get; set; }
        public bool ThisStop => CheckStop();
        private bool CheckStop()
        {
            if (TurnOnAll != null && Inp != null)
            {
                return TurnOnAll.Invoke() == false;
            }
            return true;
        }

        public OutDelegate Out { get; set; }
        public InpDelegate Inp { get; set; }


        private readonly short Port; //890
        private readonly byte Delta;
        private readonly int OnWait;
        private readonly int OffWait;


        public Mach3Toggle(short port, byte delta)
        { 
            this.Port = port;
            this.Delta = delta;
        }

        public void On(int wait)
        {
            if (ThisStop == false)
            {
                byte b = (byte)(Inp.Invoke(this.Port) - this.Delta);
                Out?.Invoke(this.Port, b);
                for (int i = 0; i < wait; i += 50)
                {
                    Task.Delay(50);
                    if (ThisStop == true)
                        this.Off();
                }
            }
        }

        public void Off()
        {
            if (Inp != null)
            {
                byte b = (byte)(Inp.Invoke(this.Port) + this.Delta);
                Out?.Invoke(this.Port, b);
            }
        }
    }
}
