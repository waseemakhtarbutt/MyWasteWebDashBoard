using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DrTech.Common.Enums
{
    public enum UserTypeEnum
    {
        [Description("Registered User")]
        RegisteredUser = 1,
        [Description("Basic User")]
        BasicUser = 2,
        [Description("Public User")]
        PublicUser = 3,
        [Description("Andriod User")]
        AndriodUser = 4,
        [Description("ios User")]
        iosUser = 5
    }
}
