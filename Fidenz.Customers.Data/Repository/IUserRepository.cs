using Fidenz.Customers.Data.Models.Dto;
using Fidenz.Customers.Data.Models;

namespace Fidenz.Customers.Data.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> SearchUsersAsync(string word);
        Task<IEnumerable<UsersByZipCodeDto>> GetUsersGroupedByZipCodeAsync();
        void UpdateUser(User user);
        Task<double> CalculateDistanceAsync(string userId, double latitude, double longitude);
    }
}
