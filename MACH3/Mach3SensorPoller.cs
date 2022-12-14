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
        public event EventHandler<byte> UpdateRequest;
        public event EventHandler<int> UpdateSensor;

        public OutDelegate Out { get; set; }
        public InpDelegate Inp { get; set; }

        public bool[] pins { get; } = new bool[8];

        private Timer poller { get; }

        public Mach3SensorPoller()
        {
            TimerCallback timerCallback = new TimerCallback(PollingSensors);
            this.poller = new Timer(timerCallback, 0, 0, 1);
        }

        private void PollingSensors(object obj)
        {
            if (Inp != null)
            {
                byte request = Inp(889);
                // byte request = 159;
                UpdateRequest?.Invoke(this, request);

                for (int i = 0; i < 7; i += 1)
                {
                    pins[i] = (request % 2 == 0);
                    UpdateSensor?.Invoke(this, i);
                    request /= 2;
                }
            }
        }

        public void Stop()
        {
            this.poller.Dispose();
        }
    }
}
