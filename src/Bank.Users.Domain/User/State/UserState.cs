namespace Bank.Users.Domain.User.State
{
    public class UserState
    {
        public long IssuerId { get; internal set; }

        public string PasswordHash { get; internal set; }
    }
}