using Fidenz.Customers.Data.Repository;

namespace Fidenz.Customers.Data.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
    }
}
