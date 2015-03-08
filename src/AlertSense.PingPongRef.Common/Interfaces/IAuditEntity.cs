using System;

namespace AlertSense.PingPongRef.Common.Interfaces
{
    public interface IAuditEntity
    {
        DateTime? CreatedOn { get; set; }
        string CreatedBy { get; set; }
        DateTime? ModifiedOn { get; set; }
        string ModifiedBy { get; set; }
    }
}