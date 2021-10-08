using DrTech.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DrTech.Common.Enums
{
    public enum DropdownTypeEnum
    {
        [SubTypeTitle("Donation Sub Type")]
        [TypeTitle("Donation Type")]
        [Description("DonationType")]
        DonationType = 0,

        [SubTypeTitle("Donation To Sub Type")]
        [TypeTitle("Donation To Type")]
        [Description("DonationToType")]
        DonateToType = 1,

        [TypeTitle("City")]
        [Description("City")]
        City = 2,

        [TypeTitle("Age Group")]
        [Description("AgeGroup")]
        AgeGroup = 3,

        [TypeTitle("Plant Type")]
        [Description("PlantType")]
        PlantType = 4,

        [TypeTitle("Grades")]
        [Description("Grades")]
        Grades = 5,

        [TypeTitle("Category")]
        [Description("Category")]
        Category = 6,

        [TypeTitle("Size")]
        [Description("Size")]
        Size = 7,

        [TypeTitle("Schools")]
        [Description("Schools")]
        Schools = 8,

        [TypeTitle("Refuse")]
        [Description("Refuse")]
        Refuse = 9,

        [TypeTitle("Reduce")]
        [Description("Reduce")]
        Reduce = 10,

        [TypeTitle("Recycle")]
        [Description("Recycle")]
        Recycle = 11,

        [TypeTitle("Reuse")]
        [Description("Reuse")]
        Reuse = 12,

        [TypeTitle("Report")]
        [Description("Report")]
        Report = 13,

        [TypeTitle("Kitchen")]
        [Description("Kitchen")]
        Kitchen = 14
    }
}
