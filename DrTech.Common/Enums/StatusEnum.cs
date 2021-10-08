using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DrTech.Common.Enums
{
    public enum StatusEnum
    {
        [Description("Submitted")]
        Submit = 1,
        [Description("In Progress")]
        InProgress = 2,
        [Description("Resolved")]
        Resolved = 3,
        [Description("Pendding Approval")]
        PenddingApproval = 4,
        [Description("Rejected")]
        Rejected = 5,
        [Description("Approved")]
        Approved = 6,
        [Description("Delivered")]
        Delivered = 7,
    }
}
