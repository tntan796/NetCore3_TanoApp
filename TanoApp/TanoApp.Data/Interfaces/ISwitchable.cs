using TanoApp.Data.Enums;

namespace TanoApp.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { set; get; }
    }
}
