namespace Fidenz.Customers.Data.Common.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateJwtToken(string username, string role);
    }
}
