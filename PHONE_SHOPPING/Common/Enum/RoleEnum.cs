using System.ComponentModel;

namespace Common.Enum
{
    public enum RoleEnum
    {
        [Description("")]
        None = 0,
        [Description("Admin")]
        Admin = 1,
        [Description("Customer")]
        Customer = 2,
    }
}
