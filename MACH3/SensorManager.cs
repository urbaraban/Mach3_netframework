using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static MACH3.InpOut32x64.InpOut;

namespace MACH3
{
    internal class SensorManager : List<Mach3Sensor>
    {
        public OutDelegate Out { get; set; }
        public InpDelegate Inp { get; set; }

        private Timer timer { get; }

        public SensorManager()
        {
            TimerCallback timerCallback = new TimerCallback(PollingSensors);
            this.timer = new Timer(timerCallback, 0, 0, 1);
        }

        private void PollingSensors(object obj)
        {
            if (Inp != null)
            {
                int request = Inp(889);
                for (int i = 8; i > 0; i -= 1) 
                {
                    GetByNumber(15 * i)?.SetStat(request % 2);
                    request /= 2;
                }
            }
        }

        public Mach3Sensor GetByNumber(int value)
        {
            return this.FirstOrDefault(e => e.Number == value);
        }

        public void Stop()
        {
            this.timer.Dispose();
        }

    }
}
