using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DrTech.Amal.Common.Enums
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
        [Description("Declined")]
        Declined = 5,
        [Description("Approved")]
        Approved = 6,
        [Description("Delivered")]
        Delivered = 7,
        [Description("New")]
        New = 8,
        [Description("Assigned")]
        Assigned = 9,
        [Description("No Show")]
        NoShow = 10,
        [Description("Collected")]
        Collected = 11,
        [Description("Complete")]
        Complete = 12,
        [Description("Pending Confirmation")]
        PendingConfirmation = 13,
        [Description("Disputed")]
        Disputed = 14,
        [Description("Pending")]
        Pending = 15,
    }
}
