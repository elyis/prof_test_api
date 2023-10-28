using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace prof_tester_api.src.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserRole
    {
        Employee,
        HrManager,
        Manager,
        Admin,
    }
}