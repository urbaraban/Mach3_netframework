using static Mach3_netframework.MACH3.Mach3;

namespace Mach3_netframework.MACH3.Interface
{
    public interface ITurnElement
    {
        public TurnDelegate TurnOnAll { get; set; }
        public bool ThisStop { get; }
    }
}
