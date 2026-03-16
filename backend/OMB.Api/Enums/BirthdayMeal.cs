using NpgsqlTypes;

namespace OMB.Api.Enums
{
    public enum BirthdayMeal
    {
        [PgName("HOUSE")]
        NONE,
        [PgName("MEAL1")]
        MEAL1,
        [PgName("MEAL2")]
        MEAL2,
        [PgName("MEAL3")]
        MEAL3,
        [PgName("MEAL4")]
        MEAL4
    }
}