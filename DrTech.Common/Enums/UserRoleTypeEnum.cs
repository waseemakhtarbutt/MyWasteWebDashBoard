using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DrTech.Common.Enums
{
    public enum UserRoleTypeEnum
    {
        [Description("Mobile User")]
        Mobile = 0,
        [Description("Admin User")]
        Admin = 1,
        [Description("NGO")]
        NGO = 2
    }
}
