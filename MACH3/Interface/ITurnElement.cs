using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mach3_netframework.MACH3.Mach3;

namespace Mach3_netframework.MACH3.Interface
{
    public interface ITurnElement
    {
        public TurnDelegate TurnOnAll { get; set; }
        public bool ThisStop { get; }
    }
}
