using Fidenz.Customers.Data.Common.Interfaces;
using Fidenz.Customers.Data.Data;

namespace Fidenz.Customers.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IUserRepository User { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            User = new UserRepository(_context);
        }
    }
}
