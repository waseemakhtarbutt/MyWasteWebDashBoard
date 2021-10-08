using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrTech.Amal.Common.Enums
{
    public enum FiveREnum
    {
        [Description("Refuse")]
        Refuse =1,
        [Description("Reuse")]
        Reuse = 2,
        [Description("Reduce")]
        Reduce = 3,
        [Description("Report")]
        Report =4 ,
        [Description("Replant")]
        Replant = 5,
        [Description("Recycle")]
        Recycle = 6,
        [Description("BuyBin")]
        BuyBin = 7,
        [Description("Regift")]
        Regift = 8,
    }
    public enum GPNTypeEnum
    {
        [Description("School")]
        School,
        [Description("Business")]
        Business,
        [Description("Organization")]
        Organization,
    }
    public enum BusinessType
    {
        [Description("WWF")]
        WWF,
    }

    public enum OrganizationType
    {
        [Description("Donation")]
        Donation,
    }

}
