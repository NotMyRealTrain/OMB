using NpgsqlTypes;

namespace OMB.Api.Enums
{
    public enum OrderStatus
    {
        [PgName("DRAFT")]
        DRAFT,
        [PgName("SUBMITTED")]
        SUBMITTED,
        [PgName("LOCKED")]
        LOCKED
    }
}