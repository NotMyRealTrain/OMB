using NpgsqlTypes;

namespace OMB.Api.Enums
{
    public enum RoleName
    {
        [PgName("HOUSE")]
        HOUSE,
        [PgName("KITCHEN")]
        KITCHEN,
        [PgName("ADMIN")]
        ADMIN,
        [PgName("CARE_SPECIALIST")]
        CARE_SPECIALIST
    }
}