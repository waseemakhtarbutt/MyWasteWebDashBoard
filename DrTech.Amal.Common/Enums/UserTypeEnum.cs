using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DrTech.Amal.Common.Enums
{
    public enum UserTypeEnum
    {
        [Description("Registered User")]
        RegisteredUser = 6,
        [Description("Basic User")]
        BasicUser = 7,
        [Description("Public User")]
        PublicUser = 3,
        [Description("Andriod User")]
        AndriodUser = 4,
        [Description("ios User")]
        iosUser = 5,
        [Description("Mobile")]
        Mobile = 1,
        [Description("Web")]
        Web = 2
    }
}
