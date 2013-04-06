namespace Crowbar.Demo.Mvc.Raven.Application.Models
{
    public class Password
    {
        public string Hash { get; private set; }

        public bool IsValid(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, Hash);
        }

        public static Password Generate(string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password, 10);
            return new Password { Hash = hash };
        }
    }
}