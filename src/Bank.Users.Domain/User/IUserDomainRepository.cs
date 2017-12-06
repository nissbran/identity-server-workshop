namespace Bank.Users.Domain.User
{
    using System.Threading.Tasks;

    public interface IUserDomainRepository
    {
        Task<UserAggregateRoot> GetUserByUsername(string username);

        Task SaveUser(UserAggregateRoot user);
    }
}