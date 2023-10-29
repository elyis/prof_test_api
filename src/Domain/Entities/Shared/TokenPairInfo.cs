using prof_tester_api.src.Domain.Enums;

namespace prof_tester_api.src.Domain.Entities.Shared
{
    public class TokenPairInfo
    {
        public TokenPair TokenPair { get; set; }
        public UserRole Role { get; set; }
    }
}