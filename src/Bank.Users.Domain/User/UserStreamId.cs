namespace Bank.Users.Domain.User
{
    using Infrastructure.EventStore;

    public class UserStreamId : StreamId
    {
        public string Username { get; }

        public UserStreamId(string username)
        {
            Username = username;
        }

        public override string ToStreamName()
        {
            return $"User-{Username}";
        }
    }
}