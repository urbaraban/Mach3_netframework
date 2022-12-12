using Mach3_netframework.MACH3;
using Mach3_netframework.MACH3.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static Mach3_netframework.MACH3.InpOut32x64.InpOut;

namespace Mach3_netframework.MACH3
{
    public class Mach3SensorPoller : IInpOutElement
    {
        public event EventHandler UpdateSensor;

        public OutDelegate Out { get; set; }
        public InpDelegate Inp { get; set; }

        public short Request { get; set; }

        public byte[] pins { get; } = new byte[8];

        private Timer poller { get; }

        public Mach3SensorPoller()
        {
            TimerCallback timerCallback = new TimerCallback(PollingSensors);
            this.poller = new Timer(timerCallback, 0, 0, 10);
        }

        private void PollingSensors(object obj)
        {
            if (Inp != null)
            {
                Request = Inp(889);
                for (int i = 7; i > -1; i -= 1) 
                {
                    byte temp = (byte)(Request % 2);
                    if (temp != pins[i])
                    {
                        pins[i] = temp;
                        UpdateSensor?.Invoke(this, EventArgs.Empty);
                    }

                    Request /= 2;
                }
            }
        }

        public void Stop()
        {
            this.poller.Dispose();
        }
    }
}
