using static Mach3_netframework.MACH3.InpOut32x64.InpOut;

namespace Mach3_netframework.MACH3.Interface
{
    public interface IInpOutElement
    {
        public OutDelegate Out { get; set; }
        public InpDelegate Inp { get; set; }
    }
}
