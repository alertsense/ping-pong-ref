using System;

namespace AlertSense.PingPong.Common.Interfaces
{
    public interface IAuditEntity
    {
        DateTime? CreatedOn { get; set; }
        string CreatedBy { get; set; }
        DateTime? ModifiedOn { get; set; }
        string ModifiedBy { get; set; }
    }
}