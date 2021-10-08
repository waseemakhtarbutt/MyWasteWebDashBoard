using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DrTech.Amal.Common.Enums
{
    public enum UserRoleTypeEnum
    {
        [Description("Admin")]
        Admin = 1,
        [Description("SchoolAdmin")]
        SchoolAdmin = 2,
        [Description("OrganizationAdmin")]
        OrganizationAdmin = 3,
        [Description("BusinessAdmin")]
        BusinessAdmin = 4,
        [Description("NGO")]
        NGO = 5,
        [Description("MobileUser")]
        MobileUser = 6,
        [Description("SubSchoolAdmin")]
        SubSchoolAdmin = 7,
        [Description("SubOrganizationAdmin")]
        SubOrganizationAdmin = 8,
        [Description("SubBusinessAdmin")]
        SubBusinessAdmin = 9,
        [Description("CompanyAdmin")]
        CompanyAdmin = 10,
        [Description("WWF")]
        WWF = 11,
        [Description("GOIBusinessStaffAdmin")]
        GOIBusinessStaffAdmin = 12
    }
    public enum ConstantValues
    {
        [Description("WasteDefaultGCValuePerKG")]
        WasteDefaultGCValuePerKG = 24,

        [Description("NotWasteTypeID")]
        NotWasteTypeID = 10,
        [Description("Greenwaste")]
        Greenwaste = 17,
        [Description("TeaBags")]
        TeaBags = 19,
        [Description("Tissue")]
        Tissue = 21,
    }
}
